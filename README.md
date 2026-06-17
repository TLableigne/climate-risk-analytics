# Aqui'Alerts — Scripts Python

## Architecture

```
pythonScripts/
├── choosenGraph.py          # Point d'entrée principal — génération de graphiques
├── boxplot.py               # Graphique : boîte à moustaches
├── radarChart.py            # Graphique : diagramme en étoile (radar)
├── scatterPlot.py           # Graphique : nuage de points
├── linearRegression.py      # Graphique : régression linéaire
├── histogram.py             # Graphique : histogramme
├── parallelCoordinates.py   # Graphique : coordonnées parallèles
├── geographicPlot.py        # Graphique : carte choroplète
├── dataSetCollection.py     # Récupération des types de mesures depuis la BDD
├── lecteurJson.py           # Import et mapping de fichiers CSV vers la BDD
├── conf.json                # Configuration de mapping colonnes CSV → tables SQL
└── img/
    ├── Graph.png            # Graphique généré (écrasé à chaque génération)
    └── Logo.png             # Logo de l'application
```

---

## Prérequis

```bash
pip install -r requirements.txt
```

Tous les scripts se lancent depuis le dossier `pythonScripts/`. Le dossier `img/` doit exister :

```bash
cd pythonScripts/
mkdir -p img
```

---

## Scripts

### `choosenGraph.py` — Point d'entrée principal

Reçoit les arguments de l'interface C# et redirige vers le script de graphique correspondant. C'est le seul script appelé directement par l'application.

**Usage :**
```bash
python3 choosenGraph.py <query1> <query2> <graph_name> <graph_names>
```

| Argument | Description |
|---|---|
| `query1` | Requête SQL pour le premier jeu de données |
| `query2` | Requête SQL pour le second jeu de données (radar uniquement) |
| `graph_name` | Nom du graphique à générer |
| `graph_names` | Liste des noms de graphiques disponibles (string) |

| Valeur `graph_name` | Graphique généré |
|---|---|
| `Nuage de points` | `scatterPlot.py` |
| `Boite à moustaches` | `boxplot.py` |
| `Diagramme en étoile` | `radarChart.py` |
| `Régression linéaire` | `linearRegression.py` |
| `Histogramme` | `histogram.py` |
| `Coordonnées parallèles` | `parallelCoordinates.py` |
| `Carte Choroplète` | `geographicPlot.py` |

**Sortie :** `img/Graph.png`

---

### `scatterPlot.py` — Nuage de points

Visualise la relation entre deux variables numériques. Utile pour observer des corrélations entre, par exemple, des données climatiques et le nombre d'événements à risque.

**Fonction :**
```python
draw_scatterplot(X: pd.DataFrame)
```
Colonne 0 → axe X, colonne 1 → axe Y.

---

### `boxplot.py` — Boîte à moustaches

Représente la distribution statistique d'une variable (médiane, quartiles, valeurs extrêmes). Permet de comparer la dispersion de différents types de risques ou de durées d'événements.

**Fonction :**
```python
draw_boxplot(X: pd.DataFrame)
```
Génère un boxplot par colonne du DataFrame.

---

### `radarChart.py` — Diagramme en étoile

Compare deux séries de données sur plusieurs axes simultanément. Adapté pour mettre en parallèle la distribution de risques entre deux communes ou deux périodes.

**Fonction :**
```python
draw_radarchart(X1: pd.DataFrame, X2: pd.DataFrame)
```
Chaque DataFrame doit avoir 2 colonnes : colonne 0 = catégorie (axe), colonne 1 = valeur numérique.

---

### `linearRegression.py` — Régression linéaire

Modélise la relation linéaire entre deux variables et évalue sa significativité. Génère la droite ajustée ainsi qu'un diagramme des résidus, et affiche l'équation, le R² et la p-value.

**Fonction :**
```python
draw_linear_regression(df: pd.DataFrame)
```
Colonne 0 → variable explicative X, colonne 1 → variable à expliquer Y.

---

### `histogram.py` — Histogramme

Affiche la distribution de fréquence d'une variable continue. Permet de visualiser, par exemple, la répartition des durées d'événements ou des niveaux de pollution par tranche.

**Fonction :**
```python
draw_histogram(X: pd.DataFrame)
```

---

### `parallelCoordinates.py` — Coordonnées parallèles

Représente simultanément plusieurs variables sur des axes parallèles pour identifier des tendances ou des groupes. Particulièrement adapté à la comparaison de plusieurs types de risques sur de nombreux indicateurs.

**Fonction :**
```python
draw_parallel_coordinates(X: pd.DataFrame)
```

---

### `geographicPlot.py` — Carte choroplète

Projette des données sur une carte géographique en colorant les zones selon une valeur (nombre de risques, durée moyenne…). Permet une lecture territoriale immédiate des données de Nouvelle-Aquitaine.

**Fonction :**
```python
draw_geographic_plot(X: pd.DataFrame)
```

---

### `dataSetCollection.py` — Liste des types de mesures

Interroge la BD et retourne la liste des types de mesures disponibles. Utilisé par l'interface C# pour peupler les menus déroulants.

**Usage :**
```bash
python dataSetCollection.py
```

**Sortie (stdout) :** un type de mesure par ligne.

**Requête exécutée :**
```sql
SELECT DISTINCT TYP_LIB
FROM T_MESURE_MES
INNER JOIN T_TYPE_TYP ON TYP_REF_MES = TYP_ID
```

---

### `lecteurJson.py` — Import et mapping CSV

Détecte les fichiers CSV du dossier courant, identifie automatiquement leurs colonnes et les mappe aux tables SQL correspondantes via `conf.json`.

**Usage :**
```bash
python lecteurJson.py
```

| Colonne CSV détectée | Mapping SQL |
|---|---|
| `lib_commune`, `NOM COMMUNE` | `T_COMMUNE_COM.COM_LIB` |
| `lib_zone`, `NOM DEPARTEMENT` | `T_DEPARTEMENT_DEP.DEP_LIB` |
| `lib_risque_jo`, `LIBELLE RISQUE 1`… | `T_TYPE_TYP.TYP_LIB` |
| `dat_deb`, `PRESCRIPTION`… | `T_DATE_DAT.DAT_DATE_DEB` |
| `dat_fin`, `ANNULATION`… | `T_DATE_DAT.DATE_DUREE` |
