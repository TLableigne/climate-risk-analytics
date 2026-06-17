import pandas as pd
import sqlalchemy as sa
import csv



defaultSeparator = ";"
encoding = "latin-1"

KEPT_DEPARTEMENT = ["16","17","19","23","24","33","40","47","64","79","86","87"]

KEPT_METEO = {
    "RR": ("Précipitations", "mm"),
    "TN": ("Température minimale", "°C"),
    "TX": ("Température maximale", "°C"),
    "TM": ("Température moyenne", "°C"),
    "FFM": ("Vitesse vent moyenne", "m/s"),
    "UM": ("Humidité relative", "%"),
    "INST": ("Insolation", "h"),
}


def getDepartement(insee):
    number = abs(int(insee))
    return str(number).zfill(5)[:2]

def get_existing_ids(engine, table_name, column_name):
    try:
        df_exist = pd.read_sql(f"SELECT {column_name} FROM {table_name}", engine)
        return df_exist[column_name].tolist()
    except Exception:
        return []
    
def communesBD(conn, rows):
    existing = set(
        pd.read_sql("SELECT COM_ID FROM T_COMMUNE_COM", conn)["COM_ID"].astype(str)
    )
    for com_id, com_lib, dep_id in rows:
        if str(com_id) not in existing:
            conn.execute(
                sa.text("INSERT INTO T_COMMUNE_COM (COM_ID, COM_LIB, DEP_REF_COM)"
                        " VALUES (:id, :lib, :dep)"),
                {"id": str(com_id), "lib": str(com_lib), "dep": str(dep_id)}
            )
            existing.add(str(com_id))
 
 
def typesBD(conn, libs):
    existing = pd.read_sql("SELECT TYP_LIB FROM T_TYPE_TYP", conn)["TYP_LIB"].tolist()
    for lib in libs:
        lib = str(lib).strip()
        if lib and lib not in existing:
            conn.execute(
                sa.text("INSERT INTO T_TYPE_TYP (TYP_LIB) VALUES (:lib)"),
                {"lib": lib}
            )
            existing.append(lib)
    return {
        r["TYP_LIB"].strip(): r["TYP_ID"]
        for _, r in pd.read_sql("SELECT TYP_ID, TYP_LIB FROM T_TYPE_TYP", conn).iterrows()
    }
 
 
def datesBD(conn, pairs):
    existing_df = pd.read_sql("SELECT DAT_ID, DAT_DATE_DEB, DAT_DUREE FROM T_DATE_DAT", conn)
    existing = {(r["DAT_DATE_DEB"], r["DAT_DUREE"]): r["DAT_ID"]
                for _, r in existing_df.iterrows()}
    for key in pairs:
        if key not in existing:
            conn.execute(
                sa.text("INSERT INTO T_DATE_DAT (DAT_DATE_DEB, DAT_DUREE) VALUES (:d, :j)"),
                {"d": key[0], "j": key[1]}
            )
    df2 = pd.read_sql("SELECT DAT_ID, DAT_DATE_DEB, DAT_DUREE FROM T_DATE_DAT", conn)
    return {(r["DAT_DATE_DEB"], r["DAT_DUREE"]): r["DAT_ID"] for _, r in df2.iterrows()}
 
 
def toDate(val):
    try:
        ts = pd.to_datetime(val, errors="coerce")
        if pd.isnull(ts):
            return None
        return ts.to_pydatetime().replace(tzinfo=None, hour=0, minute=0, second=0, microsecond=0)
    except Exception:
        return None

