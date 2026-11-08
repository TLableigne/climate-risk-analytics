# Aqui'Alerts — Python Scripts



## Architecture



```

pythonScripts/
├── choosenGraph.py          # Main entry point — chart generation dispatcher
├── boxplot.py               # Chart: Box plot
├── radarChart.py            # Chart: Radar chart (spider plot)
├── scatterPlot.py           # Chart: Scatter plot
├── linearRegression.py      # Chart: Linear regression
├── histogram.py             # Chart: Histogram
├── parallelCoordinates.py   # Chart: Parallel coordinates
├── geographicPlot.py        # Chart: Choropleth map
├── dataSetCollection.py     # Metric type retrieval from database
├── lecteurJson.py           # CSV import & schema mapping to database
├── conf.json                # Column-to-SQL table mapping configuration
└── img/
    ├── Graph.png            # Rendered chart (overwritten on each run)
    └── Logo.png             # Application logo

```



---



## Prerequisites



```bash

pip install -r requirements.txt

```



All scripts must be executed from the `pythonScripts/` directory. The `img/` directory must exist:



```bash

cd pythonScripts/

mkdir -p img

```



---



## Scripts



### `choosenGraph.py` — Main Entry Point



Receives arguments dispatched by the C# interface and redirects execution to the corresponding visualization script. This is the only script called directly by the main application.



**Usage:**

```bash

python3 choosenGraph.py <query1> <query2> <graph_name> <graph_names>

```



| Argument | Description |
|---|---|
| `query1` | SQL query for the primary dataset |
| `query2` | SQL query for the secondary dataset (radar chart only) |
| `graph_name` | Name of the chart to generate |
| `graph_names` | Serialized list of available chart names (string) |



| `graph_name` Value | Generated Chart |
|---|---|
| `Nuage de points` | `scatterPlot.py` |
| `Boite à moustaches` | `boxplot.py` |
| `Diagramme en étoile` | `radarChart.py` |
| `Régression linéaire` | `linearRegression.py` |
| `Histogramme` | `histogram.py` |
| `Coordonnées parallèles` | `parallelCoordinates.py` |
| `Carte Choroplète` | `geographicPlot.py` |



**Output:** `img/Graph.png`



---



### `scatterPlot.py` — Scatter Plot



Visualizes relationships between two numerical variables. Useful for identifying correlations, such as weather metrics versus natural risk event counts.



**Function:**

```python

draw_scatterplot(X: pd.DataFrame)

```

Column 0 → X-axis, Column 1 → Y-axis.



---



### `boxplot.py` — Box Plot



Displays the statistical distribution of a continuous variable (median, quartiles, extreme outliers). Allows variance comparisons across multiple risk types or event durations.



**Function:**

```python

draw_boxplot(X: pd.DataFrame)

```

Generates one boxplot per column in the DataFrame.



---



### `radarChart.py` — Radar Chart



Compares two data series across multiple axes simultaneously. Suited for contrasting risk distributions between two municipalities or historical time periods.



**Function:**

```python

draw_radarchart(X1: pd.DataFrame, X2: pd.DataFrame)

```

Each DataFrame must contain 2 columns: Column 0 = category (axis), Column 1 = numerical value.



---



### `linearRegression.py` — Linear Regression



Models linear relationships between two variables and assesses statistical significance. Generates the fitted line alongside a residual plot, displaying the equation, R², and p-value.



**Function:**

```python

draw_linear_regression(df: pd.DataFrame)

```

Column 0 → explanatory variable X, Column 1 → dependent variable Y.



---



### `histogram.py` — Histogram



Displays the binned frequency distribution of a continuous variable. Allows visualization of event duration distributions or pollution thresholds.



**Function:**

```python

draw_histogram(X: pd.DataFrame)

```



---



### `parallelCoordinates.py` — Parallel Coordinates



Simultaneously plots multiple variables on parallel axes to identify clusters and multivariate trends. Ideal for evaluating risk profiles across numerous indicators.



**Function:**

```python

draw_parallel_coordinates(X: pd.DataFrame)

```



---



### `geographicPlot.py` — Choropleth Map



Projects regional risk indicators onto a geographic map by coloring administrative zones according to values (incident counts, average durations). Offers immediate spatial awareness across Nouvelle-Aquitaine.



**Function:**

```python

draw_geographic_plot(X: pd.DataFrame)

```



---



### `dataSetCollection.py` — Metric Type Listing



Queries the database and returns the list of available measurement categories. Used by the C# interface to populate UI dropdowns dynamically.



**Usage:**

```bash

python dataSetCollection.py

```



**Output (stdout):** One measurement category per line.



**Executed Query:**

```sql

SELECT DISTINCT TYP_LIB

FROM T_MESURE_MES

INNER JOIN T_TYPE_TYP ON TYP_REF_MES = TYP_ID

```


---

### `lecteurJson.py` — CSV Import & Mapping

Scans local CSV files in the working directory, automatically parses headers, and maps them to target database tables using `conf.json`.

**Usage:**

```bash

python lecteurJson.py

```

| Detected CSV Column | SQL Target Mapping |
|---|---|
| `lib_commune`, `NOM COMMUNE` | `T_COMMUNE_COM.COM_LIB` |
| `lib_zone`, `NOM DEPARTEMENT` | `T_DEPARTEMENT_DEP.DEP_LIB` |
| `lib_risque_jo`, `LIBELLE RISQUE 1`… | `T_TYPE_TYP.TYP_LIB` |
| `dat_deb`, `PRESCRIPTION`… | `T_DATE_DAT.DAT_DATE_DEB` |
| `dat_fin`, `ANNULATION`… | `T_DATE_DAT.DATE_DUREE` |
