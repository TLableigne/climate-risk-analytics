from pandas.plotting import parallel_coordinates
import matplotlib.pyplot as plt

def draw_parallel_coordinates(X1,X2):
    plt.figure()
    parallel_coordinates(X1, X1.columns[1])
    parallel_coordinates(X2, X2.columns[1])
    plt.savefig('img/Graph.png')
    plt.close()