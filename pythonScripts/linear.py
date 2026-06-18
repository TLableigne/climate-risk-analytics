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
    x = df['x'].values
    y = df['y'].values
    
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

def histogram(df: pd.DataFrame):
    """
    Affiche un histogramme pour visualiser la distribution des données générées.
    """
    plt.figure(figsize=(10, 6))
    
    plt.hist(df['x'], bins=20, color='skyblue', edgecolor='black', alpha=0.7, label='Distribution X')
    plt.hist(df['y'], bins=20, color='lightgreen', edgecolor='black', alpha=0.7, label='Distribution Y')
    
    plt.title("Histogram of Variables")
    plt.xlabel("Values")
    plt.ylabel("Frequency")
    plt.grid(True, alpha=0.3)
    plt.legend()
    plt.show()

def draw_linear_regression(df: pd.DataFrame):
    piecewiseLinearRegression(df)

if __name__ == "__main__":
    # 1. Time seed
    #np.random.seed(int(time.time())) 
    
    # 2. Random data generation
    #x_data = np.random.negative_binomial(n=3, p=0.08, size=850).astype(float)
    #y_data = np.clip(30 - (x_data * 0.5) + np.random.normal(0, 8, size=850), 0, None)
    
    # 3. DataFrame
    #df = pd.DataFrame({
    #    'x': x_data,
    #    'y': y_data
    #})
    
    #print("New DataFrame successfully generated (preview):\n", df.head())
    
    # CALL FUNCTIONS
    piecewiseLinearRegression(df)
    histogram(df)