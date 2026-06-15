    using System;
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
            pictureLogo.Image = Image.FromFile(@"\\iut.bx\Etudiants\Home\lleguennec\Documents\Projet\s2.04-05\pythonScripts\img\Logo.png");
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
            defaultAndCustomInitAndClick(true);
            Process cmd = Constants.GenerateCmd();
            string commandePythonComboBox = $"python {Constants.scriptDataSetCollectionName}";
            Constants.CmdAction(commandePythonComboBox, cmd);
            string rawOutput = Constants.CmdActionWithOutput(commandePythonComboBox);
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
            
            string commandeGetQueries = $"python \"Z:\\Documents\\Projet\\s2.04-05\\pythonScripts\\get_queries.py\" \"{typLib1}\" \"{typLib2}\" \"{argumentTypeGraph}\"";
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                Arguments = $"/c {commandeGetQueries}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process p = new Process { StartInfo = psi };
            p.Start();
            string stdout = p.StandardOutput.ReadToEnd();
            string stderr = p.StandardError.ReadToEnd();
            p.WaitForExit();                                   

            string rawOutput = Constants.CmdActionWithOutput(commandeGetQueries);

            if (string.IsNullOrEmpty(rawOutput) || rawOutput.StartsWith("ERREUR"))
            {
                MessageBox.Show("Erreur lors de la génération des requêtes :\n" + rawOutput, "Erreur");
                return;
            }

            string[] parts = rawOutput.Split(new[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                MessageBox.Show("Impossible de récupérer les deux requêtes SQL.\n\n" + rawOutput, "Erreur");
                return;
            }

            string query1 = parts[0].Replace("\r", " ").Replace("\n", " ").Trim();
            string query2 = parts[1].Replace("\r", " ").Replace("\n", " ").Trim();

            Process cmd = Constants.GenerateCmd();
            string commandePython = $"python {Constants.scriptGraphicName} ETD \"{query1}\" \"{query2}\" \"{argumentTypeGraph}\"";
            Constants.CmdAction(commandePython, cmd);



            ProcessStartInfo psi2 = new ProcessStartInfo("cmd.exe")
            {
                Arguments = $"/c python {Constants.scriptGraphicName} ETD \"{query1}\" \"{query2}\" \"{argumentTypeGraph}\"",
                WorkingDirectory = Constants.pythonScriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process p2 = new Process { StartInfo = psi2 };
            p2.Start();
            string stdout2 = p2.StandardOutput.ReadToEnd();
            string stderr2 = p2.StandardError.ReadToEnd();
            p2.WaitForExit();

            showGraphic graphicInterface = new showGraphic();
            graphicInterface.ShowDialog();
        }

        private void showStatsButton_Click(object sender, EventArgs e)
        {
            showStatistics statisticsInterface = new showStatistics();
            statisticsInterface.ShowDialog();
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
            string argumentCSV = "testDataSet.csv";
            string argumentTypeGraph = "Boite à moustaches";
            Process cmd = Constants.GenerateCmd();
            string commandePython = $"python {Constants.scriptGraphicName} \"{argumentCSV}\" \"{argumentTypeGraph}\"";
            Constants.CmdAction(commandePython, cmd);
            showGraphic graphicInterface = new showGraphic();
            graphicInterface.ShowDialog();
        }

        private void buttonPreSet2_Click(object sender, EventArgs e)
        {
            string argumentCSV = "testDataSet.csv";
            string argumentTypeGraph = "Boite à moustaches";
            Process cmd = Constants.GenerateCmd();
            string commandePython = $"python {Constants.scriptGraphicName} \"{argumentCSV}\" \"{argumentTypeGraph}\"";
            Constants.CmdAction(commandePython, cmd);
            showGraphic graphicInterface = new showGraphic();
            graphicInterface.ShowDialog();
        }

        private void buttonPreSet3_Click(object sender, EventArgs e)
        {
            string argumentCSV = "testDataSet.csv";
            string argumentTypeGraph = "Boite à moustaches";
            Process cmd = Constants.GenerateCmd();
            string commandePython = $"python {Constants.scriptGraphicName} \"{argumentCSV}\" \"{argumentTypeGraph}\"";
            Constants.CmdAction(commandePython, cmd);
            showGraphic graphicInterface = new showGraphic();
            graphicInterface.ShowDialog();
        }

        private void buttonPreSet4_Click(object sender, EventArgs e)
        {
            string argumentCSV = "testDataSet.csv";
            string argumentTypeGraph = "Boite à moustaches";
            Process cmd = Constants.GenerateCmd();
            string commandePython = $"python {Constants.scriptGraphicName} \"{argumentCSV}\" \"{argumentTypeGraph}\"";
            Constants.CmdAction(commandePython, cmd);
            showGraphic graphicInterface = new showGraphic();
            graphicInterface.ShowDialog();
        }

        

        private void importButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Process cmd = Constants.GenerateCmd();
                string commandePythonImport = $"python {Constants.scriptImportName} \"{openFileDialog.FileName}\" ?\"{separatorTextBox.Text}\"";
                Constants.CmdAction(commandePythonImport, cmd);

                string commandePythonComboBox = $"python {Constants.scriptDataSetCollectionName}";
                string rawOutput = Constants.CmdActionWithOutput(commandePythonComboBox);
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
            string commandePython = $"python \"Z:\\Documents\\Projet\\s2.04-05\\pythonScripts\\commonDataset.py\" \"{typLibSelected}\"";
            string rawOutput = Constants.CmdActionWithOutput(commandePython);

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