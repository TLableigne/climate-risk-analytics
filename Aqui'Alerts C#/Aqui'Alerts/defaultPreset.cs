using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aqui_Alerts
{
    public partial class defaultPreset : Form
    {
        public defaultPreset()
        {
            InitializeComponent();
            this.BackColor = Constants.colorBackground;
            titleAquiAlerts.BackColor = Constants.colorBackground;
            pictureLogo.Image = Image.FromFile(@"\\iut.bx\Etudiants\Home\lleguennec\Documents\Projet\s2.04-05\Images\Logo.png");
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
