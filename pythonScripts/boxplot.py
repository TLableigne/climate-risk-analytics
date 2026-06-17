import matplotlib.pyplot as plt
import seaborn as sns

def draw_boxplot(X1, X2):
    val_col1 = X1.columns[1]
    val_col2 = X2.columns[1]
    fig, axes = plt.subplots(1, 2, figsize=(12, 5))
    sns.boxplot(x=X1[val_col1], ax=axes[0])
    axes[0].set_title("Série X1")
    # Plot X2 on the second subplot (axes[1])
    sns.boxplot(x=X2[val_col2], ax=axes[1])
    axes[1].set_title("Série X2")
    plt.savefig('img/Graph.png')
    plt.close()