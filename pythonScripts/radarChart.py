import plotly.express as px
import matplotlib.pyplot as plt
import pandas as pd

def draw_radar_chart(X):
    plt.figure()
    px.line_polar(df, r='r', theta='theta', line_close=True)
    plt.savefig(f'img/Graph.png')

    df = pd.DataFrame(dict(
    r=[1, 5, 2, 2, 3],
    theta=['processing cost','mechanical properties','chemical stability',
           'thermal stability', 'device integration']))
    fig = px.line_polar(df, r='r', theta='theta', line_close=True)
    fig.show()
