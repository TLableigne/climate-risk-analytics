using System.CodeDom;
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
        }

        private void showGraphicButton_Click(object sender, EventArgs e)
        {
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
