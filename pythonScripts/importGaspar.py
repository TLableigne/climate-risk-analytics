import pandas as pd
import sqlalchemy as sa
import datetime
from helperFunctions import communesBD, typesBD, datesBD, toDate, KEPT_DEPARTEMENT

MODELES_EXCLUS = {
    "Plan de Prévention des Risques Miniers",
    "Plan de Prévention des Risques Technologiques",
}
RISQUES_EXCLUS = {
    "Transport de marchandises dangereuses",
    "Risque nucléaire",
    "Risque industriel ou technologique",
    "Risque minier",
}

def importGaspar(file_path, engine, sep):
    try:
        df = pd.read_csv(file_path, sep=sep, dtype=str, encoding="utf-8")
    except UnicodeDecodeError:
        df = pd.read_csv(file_path, sep=sep, dtype=str, encoding="latin-1")

    profile = getProfile(set(df.columns), df)

    if profile == "catnat":
        importCatnat(df, file_path, engine)
    elif profile == "ppr":
        importPPR(df, file_path, engine)
    elif profile == "azi":
        importAzi(df, file_path, engine)
    elif profile == "risq":
        importRisque(df, file_path, engine)
    elif profile == "ignore":
        print(f"[Gaspar] Fichier hors scope ignoré : {file_path}")
    else:
        print(f"[Gaspar] Profil inconnu, ignoré : {file_path}")


def getProfile(cols, df):
    if "lib_risque_jo" in cols and "date_debut" in cols:
        return "catnat"
    if "CODE INSEE COMMUNE" in cols and "APPROBATION" in cols and "LIBELLE MODELE" in cols:
        modeles = df["LIBELLE MODELE"].dropna().unique()
        if all(m in MODELES_EXCLUS for m in modeles):
            return "ignore"
        return "ppr"
    if "list_risques" in cols and "dat_diffusion" in cols and "cod_commune" in cols:
        return "azi"
    if "lib_risque" in cols and "num_risque" in cols and "cod_commune" in cols:
        return "risq"
    datecols = [c for c in cols if "dat" in c.lower() or "date" in c.lower()]
    comcols  = [c for c in cols if "commune" in c.lower() or "cod_com" in c.lower()]
    if comcols and len(datecols) <= 1 and len(cols) <= 4:
        return "ignore"
    return "unknown"


def firstCol(df, *candidates):
    for c in candidates:
        if c in df.columns:
            return c
    return None

def importCatnat(df, file_path, engine):
    print(f"[Gaspar catnat] Import : {file_path}")
    col_com = firstCol(df, "code_commune", "cod_commune")
    col_lib = firstCol(df, "libelle_commune", "lib_commune")
    col_fin = firstCol(df, "date_fin")

    df = df[df[col_com].astype(str).str[:2].isin(KEPT_DEPARTEMENT)]
    df = df.dropna(subset=[col_com, "lib_risque_jo", "date_debut"])
    df["debut"] = pd.to_datetime(df["date_debut"], errors="coerce")
    df["fin"]   = pd.to_datetime(df[col_fin], errors="coerce") if col_fin else df["debut"]
    df["dur"]   = (df["fin"] - df["debut"]).dt.days.fillna(0).astype(int)
    df.loc[df["debut"] == df["fin"], "dur"] = 0
    df = df.dropna(subset=["debut"])

    with engine.begin() as conn:
        com_rows = [(r[col_com], r.get(col_lib, r[col_com]), str(r[col_com])[:2])
                    for _, r in df[[col_com] + ([col_lib] if col_lib else [])].drop_duplicates().iterrows()]
        communesBD(conn, com_rows)

        typ_map = typesBD(conn, df["lib_risque_jo"].dropna().unique())
        dat_map = datesBD(conn, {(toDate(r["debut"]), int(r["dur"]))
                                  for _, r in df[["debut", "dur"]].drop_duplicates().iterrows()
                                  if toDate(r["debut"])})

        df["typ_ref"] = [typ_map.get(str(v).strip()) for v in df["lib_risque_jo"]]
        df["dat_ref"] = [dat_map.get((toDate(r["debut"]), int(r["dur"]))) for _, r in df.iterrows()]
        df_valid = df.dropna(subset=["typ_ref", "dat_ref"])

        for _, row in df_valid.iterrows():
            conn.execute(
                sa.text("INSERT INTO T_MESURE_MES (COM_REF_MES, TYP_REF_MES, DAT_REF_MES)"
                        " VALUES (:com, :typ, :dat)"),
                {"com": str(row[col_com]), "typ": row["typ_ref"], "dat": row["dat_ref"]}
            )

    print(f"[Gaspar catnat] {len(df_valid)} insérées, {len(df) - len(df_valid)} ignorées.")

def importPPR(df, file_path, engine):
    print(f"[Gaspar PPR] Import : {file_path}")
    df = df[~df["LIBELLE MODELE"].isin(MODELES_EXCLUS)]
    df = df[df["CODE INSEE DEPARTEMENT"].isin(KEPT_DEPARTEMENT)]
    if df.empty:
        print("[Gaspar PPR] Aucune donnée.")
        return

    with engine.begin() as conn:
        df_com = (df[["CODE INSEE COMMUNE", "NOM COMMUNE", "CODE INSEE DEPARTEMENT"]]
                  .drop_duplicates(subset=["CODE INSEE COMMUNE"])
                  .dropna(subset=["CODE INSEE COMMUNE"]))
        communesBD(conn, [(r["CODE INSEE COMMUNE"], r["NOM COMMUNE"], r["CODE INSEE DEPARTEMENT"])
                           for _, r in df_com.iterrows()])

        libs = set()
        for label in ["LIBELLE MODELE", "LIBELLE RISQUE 2", "LIBELLE RISQUE 3"]:
            if label in df.columns:
                libs.update(df[label].dropna().unique())
        typ_map = typesBD(conn, list(libs))

        all_dates = (df["PRESCRIPTION"].tolist() if "PRESCRIPTION" in df.columns else []) + df["APPROBATION"].tolist()
        dat_map = datesBD(conn, {(toDate(d), 0) for d in all_dates if toDate(d)})

        col_proc = firstCol(df, "CODE PROCEDURE", "CODE PROECEDURE")
        df["_proc"] = df[col_proc].astype(str).str.strip() if col_proc else ""
        df["typ_ris"] = [typ_map.get(str(v).strip()) for v in df["LIBELLE MODELE"]]
        df["dat_ris"] = [dat_map.get((toDate(v), 0)) for v in df["APPROBATION"]]
        df_mes = df.drop_duplicates(subset=["_proc"]).dropna(subset=["typ_ris", "dat_ris"])

        for _, row in df_mes.iterrows():
            conn.execute(
                sa.text("INSERT INTO T_MESURE_MES (COM_REF_MES, TYP_REF_MES, DAT_REF_MES)"
                        " VALUES (:com, :typ, :dat)"),
                {"com": str(row["CODE INSEE COMMUNE"]).strip(),
                 "typ": row["typ_ris"], "dat": row["dat_ris"]}
            )
        print(f"[Gaspar PPR] T_MESURE_MES : {len(df_mes)} insérées.")

        df["dat_ris"] = [dat_map.get((toDate(v), 0))
                          for v in (df["PRESCRIPTION"] if "PRESCRIPTION" in df.columns else df["APPROBATION"])]

        for label in ["LIBELLE RISQUE 2", "LIBELLE RISQUE 3"]:
            if label not in df.columns:
                pass
            else:
                df["typ_ris"] = [typ_map.get(str(v).strip()) if str(v).strip() != "nan" else None
                                  for v in df[label]]
                df_ris = df.dropna(subset=["typ_ris", "dat_ris"])
                for _, row in df_ris.iterrows():
                    conn.execute(
                        sa.text("INSERT INTO T_RISQUE_RIS (COM_REF_RIS, TYP_REF_RIS, DAT_REF_RIS)"
                                " VALUES (:com, :typ, :dat)"),
                        {"com": str(row["CODE INSEE COMMUNE"]).strip(),
                         "typ": row["typ_ris"], "dat": row["dat_ris"]}
                    )
                print(f"[Gaspar PPR] T_RISQUE_RIS ({label}) : {len(df_ris)} insérées.")

def importAzi(df, file_path, engine):
    print(f"[Gaspar AZI] Import : {file_path}")
    col_com = firstCol(df, "cod_commune", "code_commune")
    col_lib = firstCol(df, "lib_commune", "libelle_commune")

    df["_dep"] = df[col_com].astype(str).str[:2]
    df = df[df["_dep"].isin(KEPT_DEPARTEMENT)]
    if df.empty:
        print("[Gaspar AZI] Aucune donnée NA.")
        return

    with engine.begin() as conn:
        com_cols = [col_com, "_dep"] + ([col_lib] if col_lib else [])
        communesBD(conn, [(r[col_com], r.get(col_lib, r[col_com]), r["_dep"])
                           for _, r in df[com_cols].drop_duplicates(subset=[col_com]).iterrows()])

        all_risques = {r.strip() for val in df["list_risques"].dropna()
                       for r in val.split(",") if r.strip()}
        typ_map = typesBD(conn, list(all_risques))
        dat_map = datesBD(conn, {(toDate(d), 0) for d in df["dat_diffusion"].tolist() if toDate(d)})

        df["_dat_ref"] = [dat_map.get((toDate(v), 0)) for v in df["dat_diffusion"]]
        df_valid = df.dropna(subset=["_dat_ref"])

        df_exploded = df_valid.assign(
            risque=df_valid["list_risques"].str.split(",")
        ).explode("risque")
        df_exploded["risque"] = df_exploded["risque"].str.strip()
        df_exploded = df_exploded[df_exploded["risque"] != ""]
        df_exploded["typ_ref"] = [typ_map.get(r) for r in df_exploded["risque"]]
        df_exploded = df_exploded.dropna(subset=["typ_ref"])

        for _, row in df_exploded.iterrows():
            conn.execute(
                sa.text("INSERT INTO T_RISQUE_RIS (COM_REF_RIS, TYP_REF_RIS, DAT_REF_RIS)"
                        " VALUES (:com, :typ, :dat)"),
                {"com": str(row[col_com]).strip(), "typ": row["typ_ref"], "dat": row["_dat_ref"]}
            )

    print(f"[Gaspar AZI] {len(df_exploded)} insérées, {len(df) - len(df_valid)} ignorées.")



def importRisque(df, file_path, engine):
    print(f"[Gaspar risq] Import : {file_path}")
    col_com = firstCol(df, "cod_commune", "code_commune")
    col_lib = firstCol(df, "lib_commune", "libelle_commune")

    df = df[df[col_com].astype(str).str[:2].isin(KEPT_DEPARTEMENT)]
    df = df.dropna(subset=[col_com, "lib_risque"])
    df = df[~df["lib_risque"].isin(RISQUES_EXCLUS)]
    if df.empty:
        print("[Gaspar risq] Aucune donnée naturelle NA.")
        return

    SENTINEL = datetime.datetime(1900, 1, 1)
    with engine.begin() as conn:
        com_cols = [col_com] + ([col_lib] if col_lib else [])
        communesBD(conn, [(r[col_com], r.get(col_lib, r[col_com]), str(r[col_com])[:2])
                           for _, r in df[com_cols].drop_duplicates(subset=[col_com]).iterrows()])

        typ_map = typesBD(conn, df["lib_risque"].dropna().unique())
        dat_map = datesBD(conn, {(SENTINEL, 0)})

        df["typ_ref"] = [typ_map.get(str(v).strip()) for v in df["lib_risque"]]
        df["_dat_ref"] = dat_map.get((SENTINEL, 0))
        df_valid = df.dropna(subset=["typ_ref", "_dat_ref"])

        for _, row in df_valid.iterrows():
            conn.execute(
                sa.text("INSERT INTO T_RISQUE_RIS (COM_REF_RIS, TYP_REF_RIS, DAT_REF_RIS)"
                        " VALUES (:com, :typ, :dat)"),
                {"com": str(row[col_com]).strip(), "typ": row["typ_ref"], "dat": row["_dat_ref"]}
            )

    print(f"[Gaspar risq] {len(df_valid)} insérées, {len(df) - len(df_valid)} ignorées.")