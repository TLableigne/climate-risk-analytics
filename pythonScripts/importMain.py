import pandas as pd
import glob
import zipfile
import tarfile
import shutil
import os
import sys
import sqlalchemy as sa
from importGaspar import importGaspar
from importAtmona import importAtmona
from importDataGouv import importDataGouv
from importUnknown import importUnknown

sep = sys.argv[2]

NAME_DEPARTEMENT = {
    "16":"Charente",
    "17":"Charente-Maritime",
    "19":"Corrèze",
    "23":"Creuse",
    "24":"Dordogne",
    "33":"Gironde",
    "40":"Landes",
    "47":"Lot-et-Garonne",
    "64":"Pyrénées-Atlantiques",
    "79":"Deux-Sèvres",
    "86":"Vienne",
    "87":"Haute-Vienne"
}

def unzipFile(file, directory='./tmp'):
    with zipfile.ZipFile(file, 'r') as zip_ref:
        zip_ref.extractall(directory)
    return directory


def untarFile(file, directory='./tmp'):
    with tarfile.open(file, "r:*") as tar_ref:
        tar_ref.extractall(directory)
    return directory


def removeDirectory(path='./tmp'):
    shutil.rmtree(path, ignore_errors=True)


def chooseImport(file, conn):
    fileType = detect_type(file)
    match fileType:
        case "gaspar":
            importGaspar(file, conn, sep)
        case "atmona":
            importAtmona(file, conn, sep)
        case "dataGouv":
            importDataGouv(file, conn, sep)
        case _:
            importUnknown(file, conn, sep)


def fileTreatment(files, conn):
    os.makedirs("./tmp", exist_ok=True)

    if zipfile.is_zipfile(files):
        unzipFile(files)
        for file in glob.glob("tmp/**/*.csv", recursive=True):
            chooseImport(file, conn)

    elif tarfile.is_tarfile(files):
        untarFile(files)
        for file in glob.glob("tmp/**/*.csv", recursive=True):
            chooseImport(file, conn)

    else:
        chooseImport(files, conn)

    removeDirectory()
    print("Fini avec succès.")


def connexion():    
    server   = "info-mssql-etd"
    user     = "etd11"
    psw      = "6db7478e"
    database = "AquiAlertsTest"
    engine = sa.create_engine(f'mssql+pymssql://{user}:{psw}@{server}/{database}')
    return engine


def detect_type(nom_fichier):
    nom = os.path.basename(nom_fichier).lower()
    if "gaspar" in nom:
        return "gaspar"
    if "atmona" in nom:
        return "atmona"
    if nom.startswith("q_") and nom[2:4].isdigit():
        return "dataGouv"
    return "inconnue"

def add_departments(connec):

    print("[Setup]")
    
    with connec.begin() as conn:
        try:
            existing_df = pd.read_sql("SELECT DEP_ID FROM T_DEPARTEMENT_DEP", conn)
            existing_deps = existing_df["DEP_ID"].astype(str).str.strip().tolist()
        except Exception:
            existing_deps = []

        inserted = 0
        
        for dep_id, dep_lib in NAME_DEPARTEMENT.items():
            if dep_id not in existing_deps:
                conn.execute(
                    sa.text("INSERT INTO T_DEPARTEMENT_DEP (DEP_ID, DEP_LIB) VALUES (:dep_id, :dep_lib)"),
                    {"dep_id": dep_id, "dep_lib": dep_lib}
                )
                inserted += 1
                print(f" -> Inserted: {dep_id} - {dep_lib}")
                
    print(f"[Setup] Finished. {inserted} new departments added.")


if __name__ == "__main__" :
    conn = connexion()
    add_departments(conn)
    fileTreatment(sys.argv[1], conn)

