import pandas as pd

def importUnknown(file_path, conn, sep):
    print(f"Format non reconnu ou fichier ignoré : {file_path}")
    try:
        df = pd.read_csv(file_path, sep=sep, encoding="latin-1", nrows=0)
        print(f"Colonnes disponibles : {', '.join(df.columns)}")
    except Exception as e:
        print("Impossible de lire les colonnes du fichier.")