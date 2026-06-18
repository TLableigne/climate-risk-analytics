import pandas as pd
import sqlalchemy as sa
from helperFunctions import KEPT_DEPARTEMENT, KEPT_METEO, toDate, datesBD, communesBD, typesBD

def get_typ(val, typ_map):
    return typ_map.get(str(val).strip())

def get_dat_mesures(row, dat_map): 
    dt = toDate(row["date_debut"])
    if not dt:
        return None
    return dat_map.get((dt, int(row["dat_jou"])))

def get_dat_alertes(row, dat_map): 
    dt = toDate(row["date_ech"])
    if not dt:
        return None
    return dat_map.get((dt, int(row["dat_jou"])))

def get_dat_expositions_inventaire(row, dat_map): 
    dt = toDate(row["date_mesure"])
    if not dt:
        return None
    return dat_map.get((dt, 0))


def importAtmona(file_path, engine, sep):
    nom = file_path.lower()
    if "mesure" in nom:
        import_mesures(file_path, engine, sep)
    elif "alerte" in nom:
        import_alertes(file_path, engine, sep)
    elif "exposition" in nom:
        import_expositions(file_path, engine, sep)
    elif "inventaire" in nom:
        import_inventaire(file_path, engine, sep)
    else:
        print(f"[Atmona] Fichier non reconnu : {file_path}")

def import_mesures(file_path, engine):
    print(f"[Atmona] Import mesures : {file_path}")

    df = pd.read_csv(file_path, sep=sep, encoding="latin-1", dtype=str)

    df = df[df["insee_com"].astype(str).str[:2].isin(KEPT_DEPARTEMENT)]
    df = df[df["statut_valid"].astype(str).str.strip().str.lower() == "true"]

    df["date_debut"] = pd.to_datetime(df["date_debut"], utc=True, errors="coerce")
    df["date_fin"]   = pd.to_datetime(df["date_fin"],   utc=True, errors="coerce")
    df["dat_jou"]    = (df["date_fin"] - df["date_debut"]).dt.days.fillna(0).astype(int)
    df.loc[df["date_debut"] == df["date_fin"], "dat_jou"] = 0
    df = df.dropna(subset=["date_debut", "nom_poll"])
    df["valeur"] = pd.to_numeric(df["valeur"], errors="coerce")

    with engine.begin() as conn:
        existing_com = pd.read_sql("SELECT COM_ID FROM T_COMMUNE_COM", conn)["COM_ID"].astype(str).tolist()
        df_com = df[["insee_com","nom_com"]].drop_duplicates(subset=["insee_com"]).dropna()
        for _, row in df_com.iterrows():
            com_id = str(row["insee_com"]).strip()
            if com_id not in existing_com:
                conn.execute(
                    sa.text(
                        "INSERT INTO T_COMMUNE_COM (COM_ID, COM_LIB, DEP_REF_COM) "
                        "VALUES (:com_id, :com_lib, :dep_ref)"
                    ),
                    {
                        "com_id": int(com_id),
                        "com_lib": str(row["nom_com"]).strip(),
                        "dep_ref": com_id[:2],
                    },
                )

        typ_map = typesBD(conn, df["nom_poll"].dropna().unique())

        pairs = {
            (toDate(row["date_debut"]), int(row["dat_jou"]))
            for _, row in df[["date_debut","dat_jou"]].drop_duplicates().iterrows()
            if toDate(row["date_debut"]) is not None
        }
        dat_map = datesBD(conn, pairs)

        df["typ_ref"] = [get_typ(v, typ_map) for v in df["nom_poll"]]
        df["dat_ref"] = [get_dat_mesures(row, dat_map) for _, row in df.iterrows()]
        df_valid = df.dropna(subset=["typ_ref", "dat_ref"])

        for _, row in df_valid.iterrows():
            conn.execute(
                sa.text(
                    "INSERT INTO T_MESURE_MES (COM_REF_MES, TYP_REF_MES, DAT_REF_MES, MES_VALEUR) "
                    "VALUES (:com_ref, :typ_ref, :dat_ref, :valeur)"
                ),
                {
                    "com_ref": int(str(row["insee_com"]).strip()),
                    "typ_ref": row["typ_ref"],
                    "dat_ref": row["dat_ref"],
                    "valeur": row["valeur"] if pd.notna(row["valeur"]) else None,
                },
            )

    print(f"[Atmona mesures] {len(df_valid)} insérées, {len(df) - len(df_valid)} ignorées.")


def import_alertes(file_path, engine):
    print(f"[Atmona] Import alertes : {file_path}")

    df = pd.read_csv(file_path, sep=sep, encoding="latin-1", dtype=str)
    df = df[df["code_zone"].astype(str).isin(KEPT_DEPARTEMENT)]

    df["date_ech"] = pd.to_datetime(df["date_ech"], utc=True, errors="coerce")
    df["date_dif"] = pd.to_datetime(df["date_dif"], utc=True, errors="coerce")
    df["dat_jou"]  = (df["date_dif"] - df["date_ech"]).dt.days.fillna(1).astype(int)
    df = df.dropna(subset=["date_ech", "lib_pol", "etat"])

    df["lib_type"] = df["lib_pol"].str.strip() + " - " + df["etat"].str.strip()

    with engine.begin() as conn:
        typ_map = typesBD(conn, df["lib_type"].unique())

        pairs = {
            (toDate(row["date_ech"]), int(row["dat_jou"]))
            for _, row in df[["date_ech","dat_jou"]].drop_duplicates().iterrows()
            if toDate(row["date_ech"]) is not None
        }
        dat_map = datesBD(conn, pairs)

        df["typ_ref"] = [get_typ(v, typ_map) for v in df["lib_type"]]
        df["dat_ref"] = [get_dat_alertes(row, dat_map) for _, row in df.iterrows()]
        df_valid = df.dropna(subset=["typ_ref", "dat_ref"])

        for _, row in df_valid.iterrows():
            conn.execute(
                "INSERT INTO T_RISQUE_RIS (COM_REF_RIS, TYP_REF_RIS, DAT_REF_RIS) VALUES (%s, %s, %s)",
                (None, row["typ_ref"], row["dat_ref"])
            )

    print(f"[Atmona alertes] {len(df_valid)} insérées, {len(df) - len(df_valid)} ignorées.")

def import_expositions(file_path, engine):
    print(f"[Atmona] Import expositions : {file_path}")

    df = pd.read_csv(file_path, sep=sep, encoding="latin-1", dtype=str)
    df = df.dropna(subset=["lib_polluant", "lib_metrique", "annee"])
    df["annee"] = pd.to_numeric(df["annee"], errors="coerce")
    df = df.dropna(subset=["annee"])

    df["lib_type"] = "Exposition : " + df["lib_polluant"].str.strip() + " - " + df["lib_metrique"].str.strip()
    df["date_mesure"] = pd.to_datetime(df["annee"].astype(int).astype(str) + "-01-01", format="%Y-%m-%d")

    with engine.begin() as conn:
        typ_map = typesBD(conn, df["lib_type"].unique())

        pairs = {(row["date_mesure"].to_pydatetime(), 0) for _, row in df.iterrows()}
        dat_map = datesBD(conn, pairs)

        df["typ_ref"] = [get_typ(v, typ_map) for v in df["lib_type"]]
        df["dat_ref"] = [get_dat_expositions_inventaire(row, dat_map) for _, row in df.iterrows()]
        df_valid = df.dropna(subset=["typ_ref", "dat_ref"])

        for _, row in df_valid.iterrows():
            conn.execute(
                "INSERT INTO T_RISQUE_RIS (COM_REF_RIS, TYP_REF_RIS, DAT_REF_RIS) VALUES (%s, %s, %s)",
                (None, row["typ_ref"], row["dat_ref"])
            )

    print(f"[Atmona expositions] {len(df_valid)} insérées, {len(df) - len(df_valid)} ignorées.")

def import_inventaire(file_path, engine):

    print(f"[Atmona] Import inventaire : {file_path}")

    df = pd.read_csv(file_path, sep=sep, encoding="latin-1", dtype=str)
    df = df.dropna(subset=["nom_poll", "annee"])
    df["annee"] = pd.to_numeric(df["annee"], errors="coerce")
    df = df.dropna(subset=["annee"])

    df["lib_type"] = "Émission : " + df["nom_poll"].str.strip()
    df["date_mesure"] = pd.to_datetime(df["annee"].astype(int).astype(str) + "-01-01", format="%Y-%m-%d")

    with engine.begin() as conn:
        typ_map = typesBD(conn, df["lib_type"].unique())

        pairs = {(row["date_mesure"].to_pydatetime(), 0) for _, row in df.iterrows()}
        dat_map = datesBD(conn, pairs)

        df["typ_ref"] = [get_typ(v, typ_map) for v in df["lib_type"]]
        df["dat_ref"] = [get_dat_expositions_inventaire(row, dat_map) for _, row in df.iterrows()]
        df_valid = df.dropna(subset=["typ_ref", "dat_ref"])

        for _, row in df_valid.iterrows():
            conn.execute(
                "INSERT INTO T_RISQUE_RIS (COM_REF_RIS, TYP_REF_RIS, DAT_REF_RIS) VALUES (%s, %s, %s)",
                (None, row["typ_ref"], row["dat_ref"])
            )

    print(f"[Atmona inventaire] {len(df_valid)} insérées, {len(df) - len(df_valid)} ignorées.")

    
