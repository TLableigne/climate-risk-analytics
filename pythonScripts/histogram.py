import matplotlib.pyplot as plt
import pandas as pd
import seaborn as sns

def draw_histogram(X: pd.DataFrame):
    """
    Generates a histogram.
    Expects a DataFrame and uses the first column.
    """
    col_name = X.columns[0]
    
    plt.figure()
    
    sns.histplot(data=X, x=col_name, color='red')
    
    plt.title(f'Histogram of {col_name}')
    plt.xlabel(col_name)
    plt.ylabel('Occurences')
    
    plt.savefig('img/Graph.png')
    plt.close()