import pymssql as ps
import pandas as pd
import sys
import warnings

warnings.filterwarnings('ignore', category=UserWarning)

def get_mesure_types_collection():
    SERVER = 'info-mssql-etd'
    USER = 'ETD'
    PASSWORD = 'ETD'
    DATABASE = 'AquiAlerts'

    try:
        conn = ps.connect(SERVER, USER, PASSWORD, DATABASE)               
        query = """
            SELECT DISTINCT TYP_LIB 
            FROM T_MESURE_MES
            INNER JOIN T_TYPE_TYP ON TYP_REF_MES = TYP_ID
        """
        
        df = pd.read_sql(query, conn)
        conn.close()
        
        for item in df['TYP_LIB'].tolist():
            print(item)

    except Exception as e:
        print(f"Erreur SQL/Python : {e}") #, file=sys.stderr)

    finally:
        if 'conn' in locals():
            conn.close()

if __name__ == "__main__":
    get_mesure_types_collection()