import json
import glob
import csv

dataCommune = ["lib_commune", "NOM COMMUNE"]
dataDepartement = ["lib_zone", "NOM DEPARTEMENT"]
dataRisque = ["lib_risque_jo", "lib_risques", "LIBELLE RISQUE 1", "LIBELLE RISQUE 2", "LIBELLE RISQUE 3", "lib_pol"]
dataDateDeb = ["dat_deb", "dat_program_deb", "PRESCRIPTION", "date_ech"]
dataDateFin = ["dat_fin", "dat_program_fin", "ANNULATION", "ABROGATION", "date_dif"]

def donnee():
    with open("conf.json", "r") as f:
        data = json.load(f)

    cles_fichiers = {}
    tableauCSV = []
    resFinal = []
    i = 0

    for fichier in glob.glob("*.csv"):
        if fichier in data and data[fichier]:
            final = []

            print("\n"+ fichier + " :" +"\n")

            tableauCSV.append(fichier)


            cles_fichiers[fichier] = list(data[fichier].keys())
            
            att = cles_fichiers.get(tableauCSV[i])

            for y,cle in enumerate(att):
                divise = data[fichier][cle].strip("[]").split(":")
                final.append(divise)
                print(att[y] + " :", end=" ")
                print(final[y])
                resFinal.append([cle]+final[y])
            i = i + 1

    return resFinal



def ajoutJson():
    with open("conf.json", "r") as f:
        data = json.load(f)

    cbon = False  

    for fichier in glob.glob("*.csv"):
        
        if fichier in data and data[fichier]:
            print(fichier + " : good")
            
        else:
            print(fichier + " : add", end=" ")
            try:
                    
                with open(fichier, "r", encoding="utf-8-sig") as csv_file:

                    
                    lecteur = csv.reader(csv_file, delimiter=";")
                    colonnes = next(lecteur)

                    lieu = any(c in dataCommune or dataDepartement for c in colonnes)
                    risque  = any(c in dataRisque for c in colonnes)
                    date = any(c in dataDateDeb for c in colonnes)
                    
                    if lieu and risque and date:
                        data[fichier] = {}

                    for col in colonnes:
                        col = col.strip()

                        if col in dataCommune:
                            data[fichier][col] = "[T_COMMUNE_COM:COM_LIB]"

                        elif col in dataDepartement:
                            data[fichier][col] = "[T_DEPARTEMENT_DEP:DEP_LIB]"
                        
                        elif col in ["lib_risque_jo", "lib_risques", "LIBELLE RISQUE 1", "LIBELLE RISQUE 2", "LIBELLE RISQUE 3", "lib_pol"]:
                            data[fichier][col] = "[T_TYPE_TYP:TYP_LIB]"
                            
                        elif col in dataDateDeb:
                            data[fichier][col] = "[T_DATE_DAT:DAT_DATE_DEB]"
                            
                        elif col in dataDateFin:
                            data[fichier][col] = "[T_DATE_DAT:DATE_DUREE]"
                            
                        else: 
                            print(col + " ignoré")
                
                cbon = lieu and risque and date

            except Exception:
                    print("Impossible de lire " + fichier)

    if cbon:
        with open("conf.json", "w") as f:
            json.dump(data, f, indent=4, ensure_ascii=False)
        print("reussie")


ajoutJson()
print(donnee())