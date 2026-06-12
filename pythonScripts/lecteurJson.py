import json
import glob

with open("conf.json", "r") as f:
    data = json.load(f)

cles_fichiers = {}
tableauCSV = []
final = []
i = 0

for fichier in glob.glob("*.csv"):
     if fichier in data and data[fichier]:
        y = 0
        print("\n"+ fichier + " :   " +"\n")
        tableauCSV.append(fichier)
        """
        print(fichier + " : ", end=" ")
        print(data[fichier].keys())
        """
        cles_fichiers[fichier] = list(data[fichier].keys())
        
        print(cles_fichiers.get(tableauCSV[i]))

        for cle in list(data[fichier].keys()):
            print(data[fichier][cle].strip("[]"))
            divise = data[fichier][cle].strip("[]").split(":")
            final.append(divise)
            print(final[y])
            y = y +1
        i = i + 1