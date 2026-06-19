    using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace Aqui_Alerts
{
    public partial class AquiAlerts : Form
    {

        public AquiAlerts()
        {
            InitializeComponent();
            this.BackColor = Constants.colorBackground;
            titleAquiAlerts.BackColor = Constants.colorBackground;
            pictureLogo.Image = Image.FromFile(Constants.logoImgPath);
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
            defaultAndCustomInitAndClick(true);
            string arguments = $"\"{Constants.scriptDataSetCollectionName}\"";
            string rawOutput = Constants.CmdActionWithOutput("python", arguments);
            List<string> dataSetCollection = new List<string>(rawOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            dataset1.Items.Clear();
            foreach (string dataset in dataSetCollection)
            {
                dataset1.Items.Add(dataset);
            }
        }

        private void showGraphicButton_Click(object sender, EventArgs e)
        {
            string typLib1 = dataset1.Text;
            string typLib2 = dataset2.Text;
            string argumentTypeGraph = graphicType.Text;
            if (string.IsNullOrEmpty(typLib1) || string.IsNullOrEmpty(typLib2) || string.IsNullOrEmpty(argumentTypeGraph))
            {
                MessageBox.Show("Veuillez sélectionner les deux datasets et un type de graphique");
                return;
            }
            string[] queries = GetSqlQueries(typLib1, typLib2, argumentTypeGraph);
            if (queries == null) return;
            GenerateGraphic(queries[0], queries[1], argumentTypeGraph);
            new showGraphic().ShowDialog();
        }

        private string[] GetSqlQueries(string typLib1, string typLib2, string argumentTypeGraph)
        {
            string command = $"\"{Constants.pythonScriptGetQueries}\" \"{typLib1}\" \"{typLib2}\" \"{argumentTypeGraph}\"";
            string output = Constants.CmdActionWithOutput("python", command);        
            if (string.IsNullOrEmpty(output) || output.StartsWith("ERREUR"))
            {
                MessageBox.Show("Erreur lors de la génération des requêtes :\n" + output, "Erreur");
                return null;
            }
            string[] parts = output.Split(new[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                MessageBox.Show("Impossible de récupérer les deux requêtes SQL.\n\n" + output, "Erreur");
                return null;
            }

            return parts.Select(p => p.Replace("\r", " ").Replace("\n", " ").Trim()).ToArray();
        }

        private void GenerateGraphic(string query1, string query2, string argumentTypeGraph)
        {
            string graphicList = string.Join(",", graphicType.Items.Cast<object>());            
            string args = $"\"{Constants.scriptGraphicName}\" \"{query1}\" \"{query2}\" \"{argumentTypeGraph}\" \"{graphicList}\"";
            string stderr = Constants.CmdActionWithOutput("python", args);
            if (!string.IsNullOrEmpty(stderr))
            {
                MessageBox.Show("Erreur lors de la génération du graphique :\n" + stderr, "Erreur");
            }                
        }

        public override string ToString()
        {
            return string.Join(", ", this);
        }

        private void linearRegressionButton_Click(object sender, EventArgs e)
        {
            string typLib1 = dataset1.Text;
            string typLib2 = dataset2.Text;
            string argumentTypeGraph = "Regression lineaire";
            if (string.IsNullOrEmpty(typLib1) || string.IsNullOrEmpty(typLib2) || string.IsNullOrEmpty(argumentTypeGraph))
            {
                MessageBox.Show("Veuillez sélectionner les deux datasets et un type de graphique");
                return;
            }
            string[] queries = GetSqlQueries(typLib1, typLib2, argumentTypeGraph);
            if (queries == null) return;
            GenerateGraphic(queries[0], queries[1], argumentTypeGraph);
        }

        private void defaultButton_Click(object sender, EventArgs e)
        {
            defaultAndCustomInitAndClick(true);
        }

        private void customButton_Click(object sender, EventArgs e)
        {
            defaultAndCustomInitAndClick(false);
        }

        private void defaultAndCustomInitAndClick(Boolean click)
        {
            dataset1.Visible = !click;
            dataset2.Visible = !click;
            graphicType.Visible = !click;
            showGraphicButton.Visible = !click;
            linearRegressionButton.Visible = !click;
            defaultButton.Enabled = !click;
            buttonPreSet1.Visible = click;
            buttonPreSet2.Visible = click;
            buttonPreSet3.Visible = click;
            buttonPreSet4.Visible = click;            
            customButton.Enabled = click;
            if (click)
            {
                defaultButton.BackColor = Color.LightGray;
                customButton.BackColor = Color.White;
            }
            else
            {
                customButton.BackColor = Color.LightGray;
                defaultButton.BackColor = Color.White;
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void buttonPreSet1_Click(object sender, EventArgs e)
        {
            // Le profil des risques sur le Littoral
            string query1 = "SELECT T.TYP_LIB AS [Type de risque], COUNT(R.ID_RIS) AS [Nombre d'évenements] FROM T_RISQUE_RIS R JOIN T_COMMUNE_COM C ON C.COM_ID = R.COM_REF_RIS JOIN T_DEPARTEMENT_DEP DEP ON DEP.DEP_ID = C.DEP_REF_COM JOIN T_TYPE_TYP T ON T.TYP_ID = R.TYP_REF_RIS WHERE DEP.DEP_LIB IN ('Charente-Maritime', 'Gironde', 'Landes') GROUP BY R.TYP_REF_RIS, T.TYP_LIB;";
            // Le profil des risques à l'Intérieur des terres
            string query2 = "SELECT T.TYP_LIB AS [Type de risque], COUNT(R.ID_RIS) AS [Nombre d'évenements] FROM T_RISQUE_RIS R JOIN T_COMMUNE_COM C ON C.COM_ID = R.COM_REF_RIS JOIN T_DEPARTEMENT_DEP DEP ON DEP.DEP_ID = C.DEP_REF_COM JOIN T_TYPE_TYP T ON T.TYP_ID = R.TYP_REF_RIS WHERE DEP.DEP_LIB NOT IN ('Charente-Maritime', 'Gironde', 'Landes') GROUP BY R.TYP_REF_RIS, T.TYP_LIB;";
            string argumentTypeGraph = "Boite a moustaches";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }
        
        private void buttonPreSet2_Click(object sender, EventArgs e)
        {
            // Nombre d'événements par Année et par Commune
            string query1 = "SELECT MONTH(dat.DAT_DATE_DEB) AS [Mois], AVG(mes.MES_VALEUR) AS [Pluie moyenne par Mois] FROM T_MESURE_MES mes JOIN T_TYPE_TYP typ ON mes.TYP_REF_MES = typ.TYP_ID JOIN T_DATE_DAT dat ON mes.DAT_REF_MES = dat.DAT_ID JOIN T_COMMUNE_COM com ON mes.COM_REF_MES = com.COM_ID WHERE typ.TYP_LIB = 'Pluie (RR)' AND dat.DAT_DATE_DEB IS NOT NULL GROUP BY MONTH(dat.DAT_DATE_DEB), com.DEP_REF_COM ORDER BY [Mois];";
            string query2 = "SELECT MONTH(dat.DAT_DATE_DEB) AS [Mois], COUNT(r.ID_RIS) AS [Nombre d'Inondations par Mois] FROM T_RISQUE_RIS r JOIN T_TYPE_TYP typ ON r.TYP_REF_RIS = typ.TYP_ID JOIN T_DATE_DAT dat ON r.DAT_REF_RIS = dat.DAT_ID JOIN T_COMMUNE_COM com ON r.COM_REF_RIS = com.COM_ID WHERE typ.TYP_LIB LIKE '%Inondation%' AND dat.DAT_DATE_DEB IS NOT NULL GROUP BY MONTH(dat.DAT_DATE_DEB), com.DEP_REF_COM ORDER BY [Mois];";
            string argumentTypeGraph = "Nuage de points";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }

        private void buttonPreSet3_Click(object sender, EventArgs e)
        {
            // Pluie moyenne par Mois
            string query1 = "SELECT DEP.DEP_LIB AS [Departement], COUNT(R.ID_RIS) AS [Nb d'evenements] FROM T_RISQUE_RIS R JOIN T_COMMUNE_COM C ON C.COM_ID = R.COM_REF_RIS JOIN T_DEPARTEMENT_DEP DEP ON DEP.DEP_ID = C.DEP_REF_COM JOIN T_TYPE_TYP T ON T.TYP_ID = R.TYP_REF_RIS WHERE T.TYP_LIB LIKE '%Inondation%' OR T.TYP_LIB LIKE 'Tempête' GROUP BY DEP.DEP_ID, DEP.DEP_LIB ORDER BY DEP.DEP_LIB;";
            // Nombre d'Inondations par Mois
            string query2 = "SELECT DEP.DEP_LIB AS [Departement], COUNT(R.ID_RIS) AS [Nb d'evenements] FROM T_RISQUE_RIS R JOIN T_COMMUNE_COM C ON C.COM_ID = R.COM_REF_RIS JOIN T_DEPARTEMENT_DEP DEP ON DEP.DEP_ID = C.DEP_REF_COM JOIN T_TYPE_TYP T ON T.TYP_ID = R.TYP_REF_RIS WHERE T.TYP_LIB LIKE '%Sécheresse%' OR T.TYP_LIB LIKE '%Feu%' OR T.TYP_LIB LIKE '%Mouvement de terrain%' GROUP BY DEP.DEP_ID, DEP.DEP_LIB ORDER BY DEP.DEP_LIB;";
            string argumentTypeGraph = "Diagramme en etoile";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }

        private void buttonPreSet4_Click(object sender, EventArgs e)
        {
            // année de chaque d'inondations et sécheresses
            string query1 = "SELECT YEAR(dat.DAT_DATE_DEB) AS [Année] FROM T_RISQUE_RIS r JOIN T_TYPE_TYP typ ON r.TYP_REF_RIS = typ.TYP_ID JOIN T_DATE_DAT dat ON r.DAT_REF_RIS = dat.DAT_ID WHERE typ.TYP_LIB LIKE '%Inondation%' AND dat.DAT_DATE_DEB IS NOT NULL;";
            string query2 = "SELECT YEAR(dat.DAT_DATE_DEB) AS [Année] FROM T_RISQUE_RIS r JOIN T_TYPE_TYP typ ON r.TYP_REF_RIS = typ.TYP_ID JOIN T_DATE_DAT dat ON r.DAT_REF_RIS = dat.DAT_ID WHERE typ.TYP_LIB LIKE '%Sécheresse%' AND dat.DAT_DATE_DEB IS NOT NULL;";
            string argumentTypeGraph = "Histogramme";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }

        

        private void importButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Process cmd = Constants.GenerateCmd();
                string commandePythonImport = $"\"{Constants.scriptImportName}\" \"{openFileDialog.FileName}\" \"{separatorTextBox.Text}\" \"{false}\"";
                string output = Constants.CmdActionWithOutput("python", commandePythonImport);
                if (string.IsNullOrEmpty(output) || output.Contains("Format non reconnu") || separatorTextBox.Text != ";")
                {
                    MessageBox.Show("Erreur lors de l'importation des données :\n" + output, "Erreur");
                    return;
                }
                MessageBox.Show("Les données ont bien été importées");
                string arguments = $"\"{Constants.scriptDataSetCollectionName}\"";
                string rawOutput = Constants.CmdActionWithOutput("python", arguments);
                List<string> dataSetCollection = new List<string>(rawOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));                
                dataset1.Items.Clear();
                foreach (string dataset in dataSetCollection)
                {
                    dataset1.Items.Add(dataset);
                }
            }
        }

        private void dataset1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string typLibSelected = dataset1.Text;            
            string arguments = $"\"{Constants.scriptDataSetCollectionName}\" \"{typLibSelected}\"";
            string rawOutput = Constants.CmdActionWithOutput("python", arguments);
            List<string> coherentDatasets = new List<string>(
                rawOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            );
            dataset2.Items.Clear();
            foreach (string ds in coherentDatasets)
            {
                dataset2.Items.Add(ds);
            }
        }
    }
}
