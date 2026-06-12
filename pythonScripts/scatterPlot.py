import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

def draw_scatterplot(X: pd.DataFrame):
    x = []
    y = []

    col1 = X.columns[0]
    col2 = X.columns[1]

    for index, row in X.iterrows():
        x.append(row[col1])
        y.append(row[col2])

    plt.figure() 
    plt.scatter(x, y)
    plt.xlabel(col1)
    plt.ylabel(col2)
    plt.savefig('img/Graph.png')
    plt.close()