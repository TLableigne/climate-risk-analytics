import sys
import pandas as pd
from boxplot import draw_boxplot
from radarChart import draw_radar_chart

def getData():
    csv_path = sys.argv[1]
    X = pd.read_csv(csv_path)
    return X

def choose():
    graph_name = sys.argv[2]
    if graph_name == "Boite à moustaches":
        draw_boxplot(getData())
    elif graph_name == "Diagramme en étoile":
        draw_radar_chart(getData())
    else:
        print("Aucun graphe de ce nom", file=sys.stderr)

if __name__ == "__main__":
    choose()