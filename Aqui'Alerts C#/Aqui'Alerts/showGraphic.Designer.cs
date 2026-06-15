namespace Aqui_Alerts
{
    partial class showGraphic
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleAquiAlerts = new Label();
            informationLabel = new Label();
            pictureGraphic = new PictureBox();
            pictureLogo = new PictureBox();
            buttonQuit = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureGraphic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureLogo).BeginInit();
            SuspendLayout();
            // 
            // titleAquiAlerts
            // 
            titleAquiAlerts.AutoSize = true;
            titleAquiAlerts.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            titleAquiAlerts.ForeColor = Color.White;
            titleAquiAlerts.Location = new Point(12, 9);
            titleAquiAlerts.Name = "titleAquiAlerts";
            titleAquiAlerts.Size = new Size(162, 37);
            titleAquiAlerts.TabIndex = 8;
            titleAquiAlerts.Text = "Aqui'Alerts";
            // 
            // informationLabel
            // 
            informationLabel.AutoSize = true;
            informationLabel.ForeColor = Color.White;
            informationLabel.Location = new Point(343, 21);
            informationLabel.Name = "informationLabel";
            informationLabel.Size = new Size(109, 15);
            informationLabel.TabIndex = 9;
            informationLabel.Text = "infos, def et échelle";
            // 
            // pictureGraphic
            // 
            pictureGraphic.Location = new Point(130, 49);
            pictureGraphic.Name = "pictureGraphic";
            pictureGraphic.Size = new Size(567, 380);
            pictureGraphic.TabIndex = 10;
            pictureGraphic.TabStop = false;
            // 
            // pictureLogo
            // 
            pictureLogo.Location = new Point(703, 353);
            pictureLogo.Name = "pictureLogo";
            pictureLogo.Size = new Size(85, 85);
            pictureLogo.TabIndex = 11;
            pictureLogo.TabStop = false;
            // 
            // buttonQuit
            // 
            buttonQuit.Location = new Point(12, 379);
            buttonQuit.Name = "buttonQuit";
            buttonQuit.Size = new Size(112, 50);
            buttonQuit.TabIndex = 16;
            buttonQuit.Text = "Quitter";
            buttonQuit.UseVisualStyleBackColor = true;
            buttonQuit.Click += buttonQuit_Click;
            // 
            // showGraphic
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonQuit);
            Controls.Add(pictureLogo);
            Controls.Add(pictureGraphic);
            Controls.Add(informationLabel);
            Controls.Add(titleAquiAlerts);
            Name = "showGraphic";
            Text = "Aqui'Alerts";
            ((System.ComponentModel.ISupportInitialize)pictureGraphic).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleAquiAlerts;
        private Label informationLabel;
        private PictureBox pictureGraphic;
        private PictureBox pictureLogo;
        private Button buttonQuit;
    }
}