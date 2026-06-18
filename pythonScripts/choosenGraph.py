import sys
import os
current_folder = os.path.dirname(os.path.abspath(__file__))
packages_path = os.path.join(current_folder, "lib", "site-packages")
sys.path.append(packages_path)
import unicodedata
import pandas as pd
import sqlalchemy as sa

# Import your graph functions
from histogram import draw_histogram
from linearRegression import draw_partial_regression
from boxplot import draw_boxplot
from radarChart import draw_radarchart
from parallelCoordinates import draw_parallel_coordinates
from scatterPlot import draw_scatterplot

def strip_accents(s):
   return ''.join(c for c in unicodedata.normalize('NFD', s)
                  if unicodedata.category(c) != 'Mn')

def choose():
    query1 = sys.argv[1]        
    query2 = sys.argv[2]        
    raw_graph_name = sys.argv[3]

    graphName = strip_accents(raw_graph_name).lower().strip()

    server = "info-mssql-etd"
    user = "ETD" 
    psw = "ETD"
    database = "AquiAlertsTest"
    
    engine = sa.create_engine(f'mssql+pymssql://{user}:{psw}@{server}/{database}')

    with engine.connect() as conn:
        X1 = pd.read_sql_query(query1, conn)
        X2 = pd.read_sql_query(query2, conn)

    if "boite" in graphName or "moustache" in graphName:
        draw_boxplot(X1, X2)
    elif "etoile" in graphName or "radar" in graphName:
        draw_radarchart(X1, X2)
    elif "nuage" in graphName or "scatter" in graphName:
        draw_scatterplot(X1, X2)
    elif "hist" in graphName:
        draw_histogram(X1)
    elif "reg" in graphName:
        draw_partial_regression(X1)
    elif "parall" in graphName:
        draw_parallel_coordinates(X1, X2)
    else:
        print(f"Aucun graphe de ce nom matching: '{raw_graph_name}'", file=sys.stderr)

if __name__ == "__main__":
    if len(sys.argv) < 5:
        print("Usage: python choosenGraph.py <query1> <query2> <graph_name> <list_of_graphs>", file=sys.stderr)
        sys.exit(1)
    choose()