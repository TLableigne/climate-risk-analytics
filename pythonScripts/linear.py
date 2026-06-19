import matplotlib.pyplot as plt
import numpy as np 
import pandas as pd
import time 

def create_design_matrix(x, internal_breaks):
    """
    Creates the matrix for continuous piecewise linear regression using hinge functions.
    """
    # Initialize matrix with ones (for intercept) and x (for initial slope)
    X = np.zeros((len(x), 2 + len(internal_breaks)))
    X[:, 0] = 1 # Intercept 
    X[:, 1] = x # Base linear term 
    
    # Add hinge functions for each breakpoint to change the slope
    for i, b in enumerate(internal_breaks):
        X[:, i+2] = np.maximum(0, x - b)
        
    return X

def piecewiseLinearRegression(df: pd.DataFrame):
    """
    Realize and show a piecewise linear regression based on the dataframe, 
    calculated manually using numpy (no pwlf).
    """
    if df.shape[1] < 2:
        raise ValueError(
            f"La régression linéaire nécessite 2 colonnes (X et Y), "
            f"mais le dataset n'en contient que {df.shape[1]} : {list(df.columns)}"
        )

    x_col, y_col = df.columns[0], df.columns[1]
    x = df[x_col].values
    y = df[y_col].values
    plt.xlabel(x_col)
    plt.ylabel(y_col)
    
    # 1. Sort using the quantiles to define the breaks
    breaks = np.unique(np.quantile(x, [0.0, 0.33, 0.66, 1.0]))
    internal_breaks = breaks[1:-1] # We only need the inner breakpoints for the math
    
    # 2. Model Setup and Fitting (Using Numpy Least Squares)
    X_design = create_design_matrix(x, internal_breaks)
    
    # np.linalg.lstsq finds the best coefficients (betas) to minimize the squared error
    betas, residuals, rank, s = np.linalg.lstsq(X_design, y, rcond=None)
    
    # 3. Prediction and Plotting
    x_plot = np.linspace(x.min(), x.max(), 200)
    X_plot_design = create_design_matrix(x_plot, internal_breaks)
    y_plot_pred = X_plot_design.dot(betas) # Multiply new points by our coefficients
    
    plt.figure(figsize=(10, 6))
    plt.scatter(x, y, color="darkcyan", alpha=0.5, label="Data")
    plt.plot(x_plot, y_plot_pred, "r-", lw=3, label="Manual Piecewise Regression")
    
    # Show breakpoints
    for i, b in enumerate(internal_breaks):
        label = "Breaks" if i == 0 else ""
        plt.axvline(x=b, color="orange", ls="--", label=label)
        
    plt.title("Continuous Piecewise Linear Regression (Numpy only)")
    plt.xlabel("Variable X")
    plt.ylabel("Variable Y")
    plt.grid(True, alpha=0.3)
    plt.legend()
    plt.show()

def draw_linear_regression(df: pd.DataFrame):
    piecewiseLinearRegression(df)