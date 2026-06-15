import matplotlib.pyplot as plt
import pandas as pd
import seaborn as sns
import statsmodels.api as sm
import sys

def draw_partial_regression(X: pd.DataFrame):
    """
    Partial Linear Regression plot if 3 or more columns are provided.
    Simple Linear Regression if only 2 columns are provided.
    Format expected: Column 0 (Target), Column 1 (Variable of interest), Column 2+ (Controls)
    """
    if len(X.columns) < 2:
        print("Error: Not enough columns for regression.", file=sys.stderr)
        return

    plt.figure()

    if len(X.columns) == 2:
        col_y = X.columns[0]
        col_x = X.columns[1]
        
        sns.regplot(data=X, x=col_x, y=col_y, scatter_kws={'alpha':0.5}, line_kws={'color':'red'})
        plt.title(f'Linear Regression: {col_y} vs {col_x}')
        
        plt.tight_layout()
        plt.savefig('img/Graph.png')
        plt.close()

    else:
        col_y = X.columns[0]
        col_x = X.columns[1]
        exog_others = X.columns[2:].tolist()

        fig = sm.graphics.plot_partregress(
            endog=col_y, 
            exog_i=col_x, 
            exog_others=exog_others, 
            data=X, 
            obs_labels=False, 
            ret_coords=False
        )
        
        fig.tight_layout()
        fig.savefig('img/Graph.png')
        plt.close(fig)