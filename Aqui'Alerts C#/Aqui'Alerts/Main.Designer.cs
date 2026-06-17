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
            pictureLogo = new PictureBox();
            buttonQuit = new Button();
            buttonPreSet1 = new Button();
            buttonPreSet2 = new Button();
            buttonPreSet3 = new Button();
            buttonPreSet4 = new Button();
            importButton = new Button();
            separatorTextBox = new TextBox();
            separatorLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureLogo).BeginInit();
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
            customButton.Click += customButton_Click;
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
            dataset1.SelectedIndexChanged += dataset1_SelectedIndexChanged;
            // 
            // dataset2
            // 
            dataset2.FormattingEnabled = true;
            dataset2.Location = new Point(101, 215);
            dataset2.Name = "dataset2";
            dataset2.Size = new Size(236, 23);
            dataset2.TabIndex = 5;
            dataset2.Text = "Veuillez sélectionner un set de données à comparer";
            // 
            // graphicType
            // 
            graphicType.FormattingEnabled = true;
            graphicType.Items.AddRange(new object[] { "Boite a moustaches", "Nuage de points", "Diagramme en etoile" });
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
            // pictureLogo
            // 
            pictureLogo.Location = new Point(703, 353);
            pictureLogo.Name = "pictureLogo";
            pictureLogo.Size = new Size(85, 85);
            pictureLogo.TabIndex = 8;
            pictureLogo.TabStop = false;
            // 
            // buttonQuit
            // 
            buttonQuit.Location = new Point(12, 379);
            buttonQuit.Name = "buttonQuit";
            buttonQuit.Size = new Size(112, 50);
            buttonQuit.TabIndex = 17;
            buttonQuit.Text = "Quitter";
            buttonQuit.UseVisualStyleBackColor = true;
            buttonQuit.Click += buttonQuit_Click;
            // 
            // buttonPreSet1
            // 
            buttonPreSet1.Location = new Point(211, 94);
            buttonPreSet1.Name = "buttonPreSet1";
            buttonPreSet1.Size = new Size(117, 77);
            buttonPreSet1.TabIndex = 18;
            buttonPreSet1.Text = "Comparer les risques sur le littoral avec intérieur des terres";
            buttonPreSet1.UseVisualStyleBackColor = true;
            buttonPreSet1.Click += buttonPreSet1_Click;
            // 
            // buttonPreSet2
            // 
            buttonPreSet2.Location = new Point(476, 94);
            buttonPreSet2.Name = "buttonPreSet2";
            buttonPreSet2.Size = new Size(117, 77);
            buttonPreSet2.TabIndex = 19;
            buttonPreSet2.Text = "Comparer les communes ayant eu un un choc mécanique avec celle ayant eu une avalanche";
            buttonPreSet2.UseVisualStyleBackColor = true;
            buttonPreSet2.Click += buttonPreSet2_Click;
            // 
            // buttonPreSet3
            // 
            buttonPreSet3.Location = new Point(211, 272);
            buttonPreSet3.Name = "buttonPreSet3";
            buttonPreSet3.Size = new Size(117, 77);
            buttonPreSet3.TabIndex = 20;
            buttonPreSet3.Text = "Test de données 3";
            buttonPreSet3.UseVisualStyleBackColor = true;
            buttonPreSet3.Click += buttonPreSet3_Click;
            // 
            // buttonPreSet4
            // 
            buttonPreSet4.Location = new Point(476, 272);
            buttonPreSet4.Name = "buttonPreSet4";
            buttonPreSet4.Size = new Size(117, 77);
            buttonPreSet4.TabIndex = 21;
            buttonPreSet4.Text = "Test de données 4";
            buttonPreSet4.UseVisualStyleBackColor = true;
            buttonPreSet4.Click += buttonPreSet4_Click;
            // 
            // importButton
            // 
            importButton.Location = new Point(401, 24);
            importButton.Name = "importButton";
            importButton.Size = new Size(94, 29);
            importButton.TabIndex = 22;
            importButton.Text = "Importer";
            importButton.UseVisualStyleBackColor = true;
            importButton.Click += importButton_Click;
            // 
            // separatorTextBox
            // 
            separatorTextBox.Location = new Point(273, 28);
            separatorTextBox.Name = "separatorTextBox";
            separatorTextBox.Size = new Size(100, 23);
            separatorTextBox.TabIndex = 23;
            // 
            // separatorLabel
            // 
            separatorLabel.AutoSize = true;
            separatorLabel.ForeColor = Color.White;
            separatorLabel.Location = new Point(198, 31);
            separatorLabel.Name = "separatorLabel";
            separatorLabel.Size = new Size(69, 15);
            separatorLabel.TabIndex = 24;
            separatorLabel.Text = "Séparateur :";
            // 
            // AquiAlerts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(800, 450);
            Controls.Add(separatorLabel);
            Controls.Add(separatorTextBox);
            Controls.Add(importButton);
            Controls.Add(buttonPreSet4);
            Controls.Add(buttonPreSet3);
            Controls.Add(buttonPreSet2);
            Controls.Add(buttonPreSet1);
            Controls.Add(buttonQuit);
            Controls.Add(pictureLogo);
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
            ((System.ComponentModel.ISupportInitialize)pictureLogo).EndInit();
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
        private PictureBox pictureLogo;
        private Button buttonQuit;
        private Button buttonPreSet1;
        private Button buttonPreSet2;
        private Button buttonPreSet3;
        private Button buttonPreSet4;
        private Button importButton;
        private TextBox separatorTextBox;
        private Label separatorLabel;
    }
}
