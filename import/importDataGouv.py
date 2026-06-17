import pandas as pd
import sqlalchemy as sa
from helperFunctions import KEPT_DEPARTEMENT, KEPT_METEO, typesBD, toDate, datesBD

def importDataGouv(file_path, engine):

    print(f"[DataGouv] Import : {file_path}")

    df = pd.read_csv(file_path, sep=";", encoding="latin-1", dtype=str)
    df["dep"] = df["NUM_POSTE"].astype(str).str.zfill(8).str[:2]
    df = df[df["dep"].isin(KEPT_DEPARTEMENT)]

    if df.empty:
        print(f"[DataGouv] Aucune donnée NA — fichier ignoré.")
        return

    cols_meteo = [c for c in KEPT_METEO if c in df.columns]

    if not cols_meteo:
        print(f"[DataGouv] Aucune colonne météo — fichier ignoré.")
        return

    df["date_mesure"] = pd.to_datetime(df["AAAAMMJJ"], format="%Y%m%d", errors="coerce")
    df = df.dropna(subset=["date_mesure"])

    for col in cols_meteo:
        df[col] = pd.to_numeric(df[col], errors="coerce")

    df_long = df[["NUM_POSTE","NOM_USUEL","date_mesure"] + cols_meteo].melt(
        id_vars=["NUM_POSTE","NOM_USUEL","date_mesure"],
        value_vars=cols_meteo, var_name="param", value_name="valeur"
    ).dropna(subset=["valeur"])

    if df_long.empty:
        print(f"[DataGouv] Toutes les valeurs NaN — fichier ignoré.")
        return
    
    print(f"[DataGouv] {len(df_long)} mesures à insérer ({', '.join(cols_meteo)})")

    with engine.begin() as conn:
        typ_map = typesBD(conn, [KEPT_METEO[p][0] for p in df_long["param"].unique()])
        date_pairs = {(toDate(d), 0) for d in df_long["date_mesure"].drop_duplicates()}
        dat_map = datesBD(conn, date_pairs)


        df_long["typ_ref"] = [typ_map.get(KEPT_METEO[p][0]) for p in df_long["param"]]
        df_long["dat_ref"] = [dat_map.get((toDate(d), 0)) for d in df_long["date_mesure"]]

        df_valid = df_long.dropna(subset=["typ_ref", "dat_ref"])
        
        skipped = len(df_long) - len(df_valid)
        
        for _, row in df_valid.iterrows():
            conn.execute(
                sa.text("INSERT INTO T_MESURE_MES (COM_REF_MES, TYP_REF_MES, DAT_REF_MES, MES_VALEUR) "
                        "VALUES (:com, :typ, :dat, :val)"),
                {"com": None, "typ": row["typ_ref"], "dat": row["dat_ref"], "val": float(row["valeur"])}
            )
            
    print(f"[DataGouv] {len(df_valid)} insérées, {skipped} ignorées.")