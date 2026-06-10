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
    public partial class showGraphic : Form
    {
        public showGraphic()
        {
            InitializeComponent();
            this.BackColor = Constants.colorBackground;
            titleAquiAlerts.BackColor = Constants.colorBackground;
            /*
             * Test permettant d'afficher correctement une image avec la pictureBox
            pictureGraphic.Image = Image.FromFile(@"\\iut.bx\Etudiants\Home\lleguennec\Downloads\Gemini_Generated_Image_nr6hqxnr6hqxnr6h.png");
            pictureGraphic.BackColor = Color.White;
            pictureGraphic.SizeMode = PictureBoxSizeMode.StretchImage; Normal/Zoom/Autosize/StretchImage sont les différents moyen d'affichage
            */
        }
    }
}
