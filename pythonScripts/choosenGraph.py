import sys
import pandas as pd
import sqlalchemy as sa
# from histogram import draw_histogram
# from linearRegression import draw_partial_regression
from boxplot import draw_boxplot
from radarChart import draw_radarchart
from scatterPlot import draw_scatterplot


sep = ','
listName = sys.argv[4]
chooseName = listName.split(sep)

def choose():

    graph_name = sys.argv[1]  

    server = "info-mssql-etd"
    user = "ETD" 
    psw = "ETD"
    database = "AquiAlerts"
    
    engine = sa.create_engine(f'mssql+pymssql://{user}:{psw}@{server}/{database}')
    
    with engine.connect() as conn:
        X1 = pd.read_sql_query(sys.argv[2], conn)
        X2 = pd.read_sql_query(sys.argv[3], conn)
    
    if graph_name == chooseName[0]:
        draw_boxplot(X1, X2)
    elif graph_name == chooseName[1]:
        draw_radarchart(X1, X2)
    elif graph_name == chooseName[2]:
        draw_scatterplot(X1, X2)
    elif graph_name == chooseName[3]:
        pass
        # draw_histogram(X1)
    elif graph_name == chooseName[4]:
        pass
        # draw_partial_regression(X1)
    else:
        print("Aucun graphe de ce nom", file=sys.stderr)

if __name__ == "__main__":

    #if len(sys.argv) < 4:
    #    print("Usage: python choosenGraph.py <query1> <query2> <graph_name> <list_of_graphs>", file=sys.stderr)
    #     sys.exit(1)
    choose()