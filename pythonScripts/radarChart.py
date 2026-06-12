import plotly.express as px
import pandas as pd

def draw_radarchart(X1, X2):
    col_axe = X1.columns[0]
    col_valeurs = X1.columns[1]

    df_x1 = pd.DataFrame({
    'r': X1[col_valeurs],
    'theta': X1[col_axe],
    'Nom du modèle': 'Série X1'
    })

    df_x2 = pd.DataFrame({
        'r': X2[col_valeurs],
        'theta': X2[col_axe],
        'Nom du modèle': 'Série X2'
        })

    
    df_total = pd.concat([df_x1, df_x2], ignore_index=True)
       
    fig = px.line_polar(df_total, r='r', theta='theta', color='Nom du modèle', line_close=True)
    fig.write_image('img/Graph.png')