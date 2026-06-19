"""Draws a parallel coordinates plot for two data series and saves to disk."""

from pandas.plotting import parallel_coordinates
import matplotlib.pyplot as plt


def draw_parallel_coordinates(X1, X2):
    """Overlay parallel coordinates plots of X1 and X2 and save to img/Graph.png."""
    plt.figure()
    parallel_coordinates(X1, X1.columns[1])
    parallel_coordinates(X2, X2.columns[1])
    plt.savefig('img/Graph.png')
    plt.close()
