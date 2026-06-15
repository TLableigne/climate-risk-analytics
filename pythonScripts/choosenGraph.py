import sys
import pandas as pd
import sqlalchemy as sa
from boxplot import draw_boxplot
from radarChart import draw_radarchart
from scatterPlot import draw_scatterplot

chooseName = ["Boite à moustaches", "Diagramme en étoile", "Nuage de points"]

def getData():

    psw = sys.argv[1]
    query1 = sys.argv[2]
    query2 = sys.argv[3]

    server = "info-mssql-etd"
    user = "ETD"
    database = "AquiAlerts"
    
    engine = sa.create_engine(f'mssql+pymssql://{user}:{psw}@{server}/{database}')
    
    with engine.connect() as conn:
        X1 = pd.read_sql_query(query1, conn)
        X2 = pd.read_sql_query(query2, conn)
        
    return X1, X2

def choose():

    graph_name = sys.argv[4] 
    
    X1, X2 = getData()
    
    if graph_name == chooseName[0]:
        draw_boxplot(X1)
    elif graph_name == chooseName[1]:
        draw_radarchart(X1, X2)
    elif graph_name == chooseName[2]:
        draw_scatterplot(X1)
    else:
        print("Aucun graphe de ce nom", file=sys.stderr)

if __name__ == "__main__":

    if len(sys.argv) < 5:
        print("Usage: python choosenGraph.py <password> <query1> <query2> <graph_name>", file=sys.stderr)
        sys.exit(1)
        
    choose()