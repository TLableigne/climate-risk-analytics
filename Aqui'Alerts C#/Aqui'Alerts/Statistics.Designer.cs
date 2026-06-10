namespace Aqui_Alerts
{
    partial class showStatistics
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
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
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
            // pictureBox1
            // 
            pictureBox1.Location = new Point(703, 353);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(85, 85);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // showStatistics
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox1);
            Controls.Add(titleAquiAlerts);
            Name = "showStatistics";
            Text = "Aqui'Alerts";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleAquiAlerts;
        private PictureBox pictureBox1;
    }
}