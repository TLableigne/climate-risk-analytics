using System.CodeDom;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Aqui_Alerts
{
    public partial class AquiAlerts : Form
    {

        public AquiAlerts()
        {
            InitializeComponent();
            this.BackColor = Constants.colorBackground;
            titleAquiAlerts.BackColor = Constants.colorBackground;
            pictureLogo.Image = Image.FromFile(@"\\iut.bx\Etudiants\Home\lleguennec\Documents\Projet\s2.04-05\Images\Logo.png");
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void showGraphicButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            string scriptPath = @"\\iut.bx\Etudiants\Home\lleguennec\Documents\Projet\s2.04-05\pythonScripts\choosenGraph.py";
            string argumentCSV = dataset1.Text;
            string argumentTypeGraph = graphicType.Text;
            start.Arguments = $"\"{scriptPath}\" \"{argumentCSV}\" \"{argumentTypeGraph}\"";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            Process.Start(startInfo);

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
            defaultPreset defaultChoice = new defaultPreset();
            defaultChoice.ShowDialog();
        }
    }
}
