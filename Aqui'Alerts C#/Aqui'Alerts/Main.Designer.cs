namespace Aqui_Alerts
{
    partial class AquiAlerts
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            defaultButton = new Button();
            customButton = new Button();
            showGraphicButton = new Button();
            showStatsButton = new Button();
            dataset1 = new ComboBox();
            dataset2 = new ComboBox();
            graphicType = new ComboBox();
            titleAquiAlerts = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // defaultButton
            // 
            defaultButton.Location = new Point(526, 24);
            defaultButton.Name = "defaultButton";
            defaultButton.Size = new Size(94, 29);
            defaultButton.TabIndex = 0;
            defaultButton.Text = "Défaut";
            defaultButton.UseVisualStyleBackColor = true;
            defaultButton.Click += defaultButton_Click;
            // 
            // customButton
            // 
            customButton.Location = new Point(646, 24);
            customButton.Name = "customButton";
            customButton.Size = new Size(94, 29);
            customButton.TabIndex = 1;
            customButton.Text = "Personnalisé";
            customButton.UseVisualStyleBackColor = true;
            // 
            // showGraphicButton
            // 
            showGraphicButton.Location = new Point(199, 357);
            showGraphicButton.Name = "showGraphicButton";
            showGraphicButton.Size = new Size(174, 56);
            showGraphicButton.TabIndex = 2;
            showGraphicButton.Text = "Afficher le graphique";
            showGraphicButton.UseVisualStyleBackColor = true;
            showGraphicButton.Click += showGraphicButton_Click;
            // 
            // showStatsButton
            // 
            showStatsButton.Location = new Point(446, 357);
            showStatsButton.Name = "showStatsButton";
            showStatsButton.Size = new Size(174, 56);
            showStatsButton.TabIndex = 3;
            showStatsButton.Text = "Afficher les données statistiques";
            showStatsButton.UseVisualStyleBackColor = true;
            showStatsButton.Click += showStatsButton_Click;
            // 
            // dataset1
            // 
            dataset1.FormattingEnabled = true;
            dataset1.Items.AddRange(new object[] { "Option A", "Option B", "Option C" });
            dataset1.Location = new Point(101, 112);
            dataset1.Name = "dataset1";
            dataset1.Size = new Size(236, 23);
            dataset1.TabIndex = 4;
            dataset1.Text = "Veuillez sélectionner un set de données";
            // 
            // dataset2
            // 
            dataset2.FormattingEnabled = true;
            dataset2.Items.AddRange(new object[] { "Option A", "Option B", "Option C" });
            dataset2.Location = new Point(101, 215);
            dataset2.Name = "dataset2";
            dataset2.Size = new Size(236, 23);
            dataset2.TabIndex = 5;
            dataset2.Text = "Veuillez sélectionner un set de données à comparer";
            // 
            // graphicType
            // 
            graphicType.FormattingEnabled = true;
            graphicType.Items.AddRange(new object[] { "Option A", "Option B", "Option C" });
            graphicType.Location = new Point(482, 166);
            graphicType.Name = "graphicType";
            graphicType.Size = new Size(258, 23);
            graphicType.TabIndex = 6;
            graphicType.Text = "Veuillez sélectionner un type de graphique";
            // 
            // titleAquiAlerts
            // 
            titleAquiAlerts.AutoSize = true;
            titleAquiAlerts.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            titleAquiAlerts.ForeColor = Color.White;
            titleAquiAlerts.Location = new Point(12, 9);
            titleAquiAlerts.Name = "titleAquiAlerts";
            titleAquiAlerts.Size = new Size(162, 37);
            titleAquiAlerts.TabIndex = 7;
            titleAquiAlerts.Text = "Aqui'Alerts";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(703, 353);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(85, 85);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // AquiAlerts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox1);
            Controls.Add(titleAquiAlerts);
            Controls.Add(graphicType);
            Controls.Add(dataset2);
            Controls.Add(dataset1);
            Controls.Add(showStatsButton);
            Controls.Add(showGraphicButton);
            Controls.Add(customButton);
            Controls.Add(defaultButton);
            Name = "AquiAlerts";
            Text = "AquiAlerts";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button defaultButton;
        private Button customButton;
        private Button showGraphicButton;
        private Button showStatsButton;
        private ComboBox dataset1;
        private ComboBox dataset2;
        private ComboBox graphicType;
        private Label titleAquiAlerts;
        private PictureBox pictureBox1;
    }
}
