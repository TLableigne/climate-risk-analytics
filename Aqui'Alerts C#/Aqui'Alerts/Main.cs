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
            string command = $"python \"{Constants.pythonScriptPath}\\get_queries.py\" \"{typLib1}\" \"{typLib2}\" \"{argumentTypeGraph}\"";
            string output = RunProcess("cmd.exe", $"/c {command}");

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
            string args = $"/c python {Constants.scriptGraphicName} \"{query1}\" \"{query2}\" \"{argumentTypeGraph}\" \"{graphicList}\"";
            string stderr = RunProcess("cmd.exe", args, Constants.pythonScriptPath, captureError: true);

            if (!string.IsNullOrEmpty(stderr))
                MessageBox.Show("Erreur lors de la génération du graphique :\n" + stderr, "Erreur");
        }

        private string RunProcess(string fileName, string arguments, string workingDirectory = null, bool captureError = false)
        {
            var psi = new ProcessStartInfo(fileName)
            {
                Arguments = arguments,
                WorkingDirectory = workingDirectory ?? string.Empty,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var process = new Process { StartInfo = psi })
            {
                process.Start();
                string stdout = process.StandardOutput.ReadToEnd();
                string stderr = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return captureError ? stderr : stdout;
            }
        }

        public override string ToString()
        {
            return string.Join(", ", this);
        }

        private void showStatsButton_Click(object sender, EventArgs e)
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
            new showGraphic().ShowDialog();
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
            showStatsButton.Visible = !click;
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
            string query1 = "";
            string query2 = "";
            string argumentTypeGraph = "Boite à moustaches";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }
        
        private void buttonPreSet2_Click(object sender, EventArgs e)
        {
            string query1 = "";
            string query2 = "";
            string argumentTypeGraph = "Boite à moustaches";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }

        private void buttonPreSet3_Click(object sender, EventArgs e)
        {
            string query1 = "";
            string query2 = "";
            string argumentTypeGraph = "Boite à moustaches";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }

        private void buttonPreSet4_Click(object sender, EventArgs e)
        {
            string query1 = "";
            string query2 = "";
            string argumentTypeGraph = "Boite à moustaches";
            GenerateGraphic(query1, query2, argumentTypeGraph);
            new showGraphic().ShowDialog();
        }

        

        private void importButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Process cmd = Constants.GenerateCmd();
                string commandePythonImport = $"python {Constants.scriptImportName} \"{openFileDialog.FileName}\" ?\"{separatorTextBox.Text}\"";
                Constants.CmdAction(commandePythonImport, cmd);

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
