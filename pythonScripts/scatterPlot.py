"""Draws side-by-side scatter plots for two data series and saves to disk."""

import matplotlib.pyplot as plt
import pandas as pd


def draw_scatterplot(X1, X2):
    """Draw scatter plots of X1 and X2 side by side and save to img/Graph.png."""
    fig, axes = plt.subplots(1, 2, figsize=(12, 5))

    Xs = [X1, X2]
    for i, X in enumerate(Xs):

        col1 = X.columns[0]
        col2 = X.columns[1]

        ax = axes[i]
        ax.scatter(X[col1], X[col2], alpha=0.7)
        ax.set_xlabel(col1)
        ax.set_ylabel(col2)
        ax.grid()
    plt.savefig('img/Graph.png')
    plt.close()
