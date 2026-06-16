# Aqui'Alerts — README

Projet SAÉ 2.04 & 2.05 — Visualisation des risques naturels en Nouvelle-Aquitaine  
Équipe : C. Jenipan, L. Leguennec, L. Ganchegui, T. Lableigne, A. Styzinski, L. Boinard

---

## Contexte

Aqui'Alerts est une application WinForms (C#) couplée à des scripts Python permettant d'importer, stocker et visualiser des données historiques sur les risques naturels en Nouvelle-Aquitaine à partir de données publiques.

L'objectif est de permettre aux habitants de la région de mieux comprendre les risques naturels auxquels ils sont exposés et de les garder en mémoire à travers des graphiques et des cartes interactives.

---

## Architecture du projet

```
projet/
├── pythonScripts/
│   ├── choosenGraph.py          # Point d'entrée principal — génération de graphiques
│   ├── boxplot.py               # Graphique : boîte à moustaches
│   ├── radarChart.py            # Graphique : diagramme en étoile (radar)
│   ├── scatterPlot.py           # Graphique : nuage de points
│   ├── dataSetCollection.py     # Récupération des types de mesures depuis la BDD
│   ├── lecteurJson.py           # Import et mapping de fichiers CSV vers la BDD
│   ├── conf.json                # Configuration de mapping colonnes CSV → tables SQL
│   └── img/
│       ├── Graph.png            # Graphique généré (écrasé à chaque génération)
│       └── Logo.png             # Logo de l'application
├── AquiAlerts/                  # Application C# WinForms
│   ├── Main.cs / Main.Designer.cs
│   ├── showGraphic.cs / showGraphic.Designer.cs
│   ├── Statistics.cs / Statistics.Designer.cs
│   ├── Constants.cs
│   └── Program.cs
└── AQUIALERTS.SQL               # Script de création de la base de données SQL Server
```

---

## Base de données

**Serveur :** `info-mssql-etd`  
**Base :** `AquiAlerts`  
**Authentification :** compte `ETD` (mot de passe : `ETD`) ou authentification Windows (`IUTBX\<login>`)

### Schéma

| Table | Description |
|---|---|
| `T_DEPARTEMENT_DEP` | Départements de Nouvelle-Aquitaine |
| `T_COMMUNE_COM` | Communes, rattachées à un département |
| `T_TYPE_TYP` | Types de risques ou de mesures (inondation, feu…) |
| `T_DATE_DAT` | Dates de début et durées des événements |
| `T_MESURE_MES` | Mesures associées à une commune, un type et une date |
| `T_RISQUE_RIS` | Risques associés à une commune, un type et une date |

Pour initialiser la base, exécuter le script SQL :

```sql
-- Sur SQL Server, exécuter :
AQUIALERTS.SQL
```

---

## Prérequis Python

Python 3.x avec les librairies suivantes :

```bash
pip install pandas sqlalchemy pymssql plotly matplotlib seaborn kaleido
```

---

## Scripts Python

### `choosenGraph.py` — Point d'entrée principal

Appelé par l'interface C# pour générer un graphique à partir de données SQL.

**Usage :**
```bash
python choosenGraph.py <password> <query1> <query2> <graph_name>
```

**Arguments :**

| Argument | Description |
|---|---|
| `password` | Mot de passe du compte `ETD` |
| `dataset1` | Requête SQL pour le premier jeu de données |
| `dataset2` | Requête SQL pour le second jeu de données (utilisée pour le radar) |
| `graph_name` | Nom du graphique parmi les valeurs ci-dessous |

**Valeurs acceptées pour `graph_name` :**

| Valeur | Graphique généré |
|---|---|
| `Nuage de points` | Nuage de points (scatter plot) |
| `Boite à moustaches` | Boîte à moustaches (boxplot) |
| `Diagramme en étoile` | Diagramme radar (radar chart) |

**Connexion SQL :**
- Serveur : `info-mssql-etd`
- Utilisateur : `ETD`
- Base : `AquiAlerts`
- Driver : `pymssql` via SQLAlchemy

**Sortie :** `img/Graph.png` (écrasé à chaque appel)

---

### `scatterPlot.py` — Nuage de points

Génère un nuage de points à partir de deux colonnes d'un DataFrame.

**Fonction :**
```python
draw_scatterplot(X: pd.DataFrame)
```

**Format attendu :** DataFrame avec au minimum 2 colonnes — la colonne 0 devient l'axe X, la colonne 1 devient l'axe Y.

**Sortie :** `img/Graph.png`

**Librairies :** `matplotlib`, `numpy`

**Exemple de test autonome :**
```python
import pandas as pd
from scatterPlot import draw_scatterplot

df = pd.DataFrame({'temperature': [12, 18, 25, 30], 'precipitations': [80, 60, 20, 10]})
draw_scatterplot(df)
# Résultat : img/Graph.png
```
---

### `boxplot.py` — Boîte à moustaches

Génère une boîte à moustaches pour chaque colonne du DataFrame fourni.

**Fonction :**
```python
draw_boxplot(X: pd.DataFrame)
```

**Comportement :** génère un boxplot par colonne de `X` et sauvegarde le dernier dans `img/Graph.png`.

**Librairies :** `matplotlib`, `seaborn`

**Exemple de test autonome :**
```python
import pandas as pd
from boxplot import draw_boxplot

df = pd.DataFrame({'valeurs': [10, 20, 15, 30, 25, 5, 40]})
draw_boxplot(df)
# Résultat : img/Graph.png
```

---

### `radarChart.py` — Diagramme en étoile (radar)

Génère un diagramme radar comparant deux séries de données.

**Fonction :**
```python
draw_radarchart(X1: pd.DataFrame, X2: pd.DataFrame)
```

**Format attendu des DataFrames :**

| Colonne 0 | Colonne 1 |
|---|---|
| Axe (catégorie) | Valeur numérique |

**Comportement :** superpose deux séries (`Série X1` et `Série X2`) sur un graphique polaire fermé et sauvegarde dans `img/Graph.png`.

**Librairies :** `plotly`, `pandas`

**Exemple de test autonome :**
```python
import pandas as pd
from radarChart import draw_radarchart

X1 = pd.DataFrame({'risque': ['Inondation', 'Feu', 'Sécheresse'], 'nb': [30, 15, 20]})
X2 = pd.DataFrame({'risque': ['Inondation', 'Feu', 'Sécheresse'], 'nb': [10, 25, 18]})
draw_radarchart(X1, X2)
# Résultat : img/Graph.png
```

---

### `dataSetCollection.py` — Liste des types de mesures

Interroge la base de données et retourne la liste des types de mesures disponibles. Utilisé par l'interface C# pour peupler les menus déroulants.

**Usage :**
```bash
python dataSetCollection.py
```

**Sortie :** un type de mesure par ligne, ex :
```
Inondation
Feu de forêt
Sécheresse
```

**Connexion :** `pymssql` — serveur `info-mssql-etd`, utilisateur `ETD`, base `AquiAlerts`

**Requête exécutée :**
```sql
SELECT DISTINCT TYP_LIB
FROM T_MESURE_MES
INNER JOIN T_TYPE_TYP ON TYP_REF_MES = TYP_ID
```

---

### `lecteurJson.py` — Import et mapping CSV

Lit les fichiers CSV du dossier courant, les mappe automatiquement aux tables SQL via `conf.json`, et affiche la structure de mapping détectée.

**Usage :**
```bash
python lecteurJson.py
```

**Fonctions internes :**

`ajoutJson()` — Détecte les CSV non encore configurés et tente de mapper automatiquement leurs colonnes :

| Colonne CSV détectée | Mapping SQL |
|---|---|
| `lib_commune`, `NOM COMMUNE` | `T_COMMUNE_COM.COM_LIB` |
| `lib_zone`, `NOM DEPARTEMENT` | `T_DEPARTEMENT_DEP.DEP_LIB` |
| `lib_risque_jo`, `LIBELLE RISQUE 1`… | `T_TYPE_TYP.TYP_LIB` |
| `dat_deb`, `PRESCRIPTION`… | `T_DATE_DAT.DAT_DATE_DEB` |
| `dat_fin`, `ANNULATION`… | `T_DATE_DAT.DATE_DUREE` |

`donnee()` — Lit `conf.json` et retourne la structure de mapping sous forme de liste.

**Prérequis :** un fichier `conf.json` doit exister dans le dossier courant. Les CSV doivent contenir au minimum une colonne de lieu, une colonne de risque et une colonne de date pour être acceptés.

---

## Authentification SQL Server

### Avec compte ETD (mot de passe)
```python
import pymssql
conn = pymssql.connect('info-mssql-etd', 'ETD', 'ETD', 'AquiAlerts')
```

### Avec authentification Windows (recommandé en domaine IUTBX)
```python
import pyodbc
conn = pyodbc.connect(
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=info-mssql-etd;"
    "DATABASE=AquiAlerts;"
    "Trusted_Connection=yes;"
)
```

---

## Calcul de durée (DAT_DUREE)

La durée est stockée directement en `smallint` dans `T_DATE_DAT.DAT_DUREE`. Pour la calculer depuis SQL Server lors d'un import :

```sql
-- Durée en jours entre date de début et date de fin
SELECT DATEDIFF(day, DAT_DATE_DEB, date_fin_csv) AS DAT_DUREE
FROM T_DATE_DAT
```

Côté Python, la colonne arrivera directement comme `int64` dans pandas — aucune conversion nécessaire.

---

## Lancement depuis l'interface C#

L'application C# appelle les scripts via `cmd.exe`. Les chemins sont définis dans `Constants.cs` :

```csharp
public static string pythonScriptPath = @"\\iut.bx\Etudiants\Home\...\pythonScripts";
public static string scriptGraphicName = "choosenGraph.py";
public static string scriptDataSetCollectionName = @"...\dataSetCollection.py";
```

La commande générée ressemble à :
```
python choosenGraph.py "ETD" "SELECT ..." "SELECT ..." "Boite à moustaches"
```

---

## Sources de données

| Données | Source |
|---|---|
| Risques naturels (GASPAR) | https://www.georisques.gouv.fr/donnees/bases-de-donnees/procedures-administratives-relatives-aux-risques |
| Données climatologiques | https://www.data.gouv.fr/datasets/donnees-climatologiques-de-base-quotidiennes |
| Qualité de l'air | https://opendata.atmo-na.org/ |

---

## Niveaux de complexité des visualisations (exigés par le cahier des charges)

| Niveau | Type | Script |
|---|---|---|
| Faible | Nuage de points | `scatterPlot.py` |
| Modéré | Boîte à moustaches | `boxplot.py` |
| Modéré | Diagramme en étoile | `radarChart.py` |
| Élevé | À implémenter | — |

---

## Remarques connues

- Les 4 boutons preset de l'interface principale (`buttonPreSet1` à `buttonPreSet4`) utilisent actuellement les mêmes paramètres — à différencier selon les jeux de données cibles.
- `showStatistics` n'affiche pas encore de données — à connecter à la BDD.
- Le boxplot écrase `img/Graph.png` à chaque colonne — seule la dernière colonne est visible.
- L'encodage `"Boite à moustaches"` peut poser problème selon la configuration système Windows — vérifier la cohérence entre `Main.cs` et `choosenGraph.py`.