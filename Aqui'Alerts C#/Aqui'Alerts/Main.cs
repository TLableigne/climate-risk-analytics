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
            string argumentCSV = dataset1.Text; //  "testDataSet.csv";
            string argumentTypeGraph = graphicType.Text; //  "Boite à moustaches";
            Process cmd = Constants.GenerateCmd();
            string commandePython = $"python {Constants.scriptGraphicName} \"{argumentCSV}\" \"{argumentTypeGraph}\"";
            Constants.CmdAction(commandePython, cmd);
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
    }
}