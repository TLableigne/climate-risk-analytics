import sys
import pandas as pd
import sqlalchemy as sa
from histogram import draw_histogram
from linearRegression import draw_partial_regression
from boxplot import draw_boxplot
from radarChart import draw_radarchart
from scatterPlot import draw_scatterplot

def choose():

    graphName = sys.argv[2]  
    rawList = sys.argv[3]

    cleanedList = rawList.replace('[', '').replace(']', '').replace('"', '').replace("'", "")

    server = "info-mssql-etd"
    user = "ETD" 
    psw = "ETD"
    database = "AquiAlerts"
    
    engine = sa.create_engine(f'mssql+pymssql://{user}:{psw}@{server}/{database}')
    
    with engine.connect() as conn:
        X1 = pd.read_sql_query(sys.argv[0], conn)
        X2 = pd.read_sql_query(sys.argv[1], conn)
    
    if graphName == cleanedList[0]:
        draw_boxplot(X1, X2)
    elif len(cleanedList) > 1 and graphName == cleanedList[1]:
        draw_radarchart(X1, X2)
    elif len(cleanedList) > 2 and graphName == cleanedList[2]:
        draw_scatterplot(X1, X2)
    elif len(cleanedList) > 3 and graphName == cleanedList[3]:
        pass # draw_histogram(X1)
    elif len(cleanedList) > 4 and graphName == cleanedList[4]:
        pass # draw_partial_regression(X1)
    else:
        print("Aucun graphe de ce nom", file=sys.stderr)

if __name__ == "__main__":

    #if len(sys.argv) < 4:
    #    print("Usage: python choosenGraph.py <query1> <query2> <graph_name> <list_of_graphs>", file=sys.stderr)
    #     sys.exit(1)
    choose()