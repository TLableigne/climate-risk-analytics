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

            using (var stream = new System.IO.FileStream(
                Constants.logoImgPath,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read))
            {
                pictureLogo.Image = Image.FromStream(stream);
            }
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;

            using (var stream = new System.IO.FileStream(
                Constants.graphImgPath,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read))
            {
                pictureGraphic.Image = Image.FromStream(stream);
            }
            pictureGraphic.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}