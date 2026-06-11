import matplotlib.pyplot as plt
import pandas as pd
import seaborn as sns
import time

def draw_boxplot(X):
    for i in X:
        plt.figure()
        sns.boxplot(x=X[i])
        plt.savefig(f'img/Graph.png')
        plt.close()