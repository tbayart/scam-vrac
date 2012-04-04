namespace UITest
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnStartAffichage = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnGenererCarteRef = new System.Windows.Forms.Button();
            this.btnPlacerAgentsTest = new System.Windows.Forms.Button();
            this.btnStopAffichage = new System.Windows.Forms.Button();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnPauseAffichage = new System.Windows.Forms.Button();
            this.btnObjectifTerre = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnDryad = new System.Windows.Forms.Button();
            this.btnRefreshImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1107, 900);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnStartAffichage
            // 
            this.btnStartAffichage.Enabled = false;
            this.btnStartAffichage.Location = new System.Drawing.Point(1143, 104);
            this.btnStartAffichage.Name = "btnStartAffichage";
            this.btnStartAffichage.Size = new System.Drawing.Size(100, 43);
            this.btnStartAffichage.TabIndex = 1;
            this.btnStartAffichage.Text = "Start affichage";
            this.btnStartAffichage.UseVisualStyleBackColor = true;
            this.btnStartAffichage.Click += new System.EventHandler(this.btnStartAffichage_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnGenererCarteRef
            // 
            this.btnGenererCarteRef.Location = new System.Drawing.Point(1143, 12);
            this.btnGenererCarteRef.Name = "btnGenererCarteRef";
            this.btnGenererCarteRef.Size = new System.Drawing.Size(100, 40);
            this.btnGenererCarteRef.TabIndex = 2;
            this.btnGenererCarteRef.Text = "Générer carte réf";
            this.btnGenererCarteRef.UseVisualStyleBackColor = true;
            this.btnGenererCarteRef.Click += new System.EventHandler(this.btnGenererCarteRef_Click);
            // 
            // btnPlacerAgentsTest
            // 
            this.btnPlacerAgentsTest.Enabled = false;
            this.btnPlacerAgentsTest.Location = new System.Drawing.Point(1143, 58);
            this.btnPlacerAgentsTest.Name = "btnPlacerAgentsTest";
            this.btnPlacerAgentsTest.Size = new System.Drawing.Size(100, 40);
            this.btnPlacerAgentsTest.TabIndex = 3;
            this.btnPlacerAgentsTest.Text = "Placer agents test";
            this.btnPlacerAgentsTest.UseVisualStyleBackColor = true;
            this.btnPlacerAgentsTest.Click += new System.EventHandler(this.btnPlacerAgentsTest_Click);
            // 
            // btnStopAffichage
            // 
            this.btnStopAffichage.Enabled = false;
            this.btnStopAffichage.Location = new System.Drawing.Point(1143, 202);
            this.btnStopAffichage.Name = "btnStopAffichage";
            this.btnStopAffichage.Size = new System.Drawing.Size(100, 43);
            this.btnStopAffichage.TabIndex = 4;
            this.btnStopAffichage.Text = "Stop affichage";
            this.btnStopAffichage.UseVisualStyleBackColor = true;
            this.btnStopAffichage.Click += new System.EventHandler(this.btnStopAffichage_Click);
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Location = new System.Drawing.Point(1143, 869);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(100, 43);
            this.btnSaveImage.TabIndex = 5;
            this.btnSaveImage.Text = "Save image";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // btnPauseAffichage
            // 
            this.btnPauseAffichage.Enabled = false;
            this.btnPauseAffichage.Location = new System.Drawing.Point(1143, 153);
            this.btnPauseAffichage.Name = "btnPauseAffichage";
            this.btnPauseAffichage.Size = new System.Drawing.Size(100, 43);
            this.btnPauseAffichage.TabIndex = 6;
            this.btnPauseAffichage.Text = "Pause";
            this.btnPauseAffichage.UseVisualStyleBackColor = true;
            this.btnPauseAffichage.Click += new System.EventHandler(this.btnPauseAffichage_Click);
            // 
            // btnObjectifTerre
            // 
            this.btnObjectifTerre.Enabled = false;
            this.btnObjectifTerre.Location = new System.Drawing.Point(1143, 295);
            this.btnObjectifTerre.Name = "btnObjectifTerre";
            this.btnObjectifTerre.Size = new System.Drawing.Size(100, 40);
            this.btnObjectifTerre.TabIndex = 7;
            this.btnObjectifTerre.Text = "Objectif : Terre";
            this.btnObjectifTerre.UseVisualStyleBackColor = true;
            this.btnObjectifTerre.Click += new System.EventHandler(this.btnObjectifTerre_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(1143, 539);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(100, 40);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnDryad
            // 
            this.btnDryad.Enabled = false;
            this.btnDryad.Location = new System.Drawing.Point(1143, 341);
            this.btnDryad.Name = "btnDryad";
            this.btnDryad.Size = new System.Drawing.Size(100, 40);
            this.btnDryad.TabIndex = 9;
            this.btnDryad.Text = "Dryad";
            this.btnDryad.UseVisualStyleBackColor = true;
            this.btnDryad.Click += new System.EventHandler(this.btnDryad_Click);
            // 
            // btnRefreshImage
            // 
            this.btnRefreshImage.Location = new System.Drawing.Point(1143, 387);
            this.btnRefreshImage.Name = "btnRefreshImage";
            this.btnRefreshImage.Size = new System.Drawing.Size(100, 40);
            this.btnRefreshImage.TabIndex = 10;
            this.btnRefreshImage.Text = "Refresh image";
            this.btnRefreshImage.UseVisualStyleBackColor = true;
            this.btnRefreshImage.Click += new System.EventHandler(this.btnRefreshImage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 986);
            this.Controls.Add(this.btnRefreshImage);
            this.Controls.Add(this.btnDryad);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnObjectifTerre);
            this.Controls.Add(this.btnPauseAffichage);
            this.Controls.Add(this.btnSaveImage);
            this.Controls.Add(this.btnStopAffichage);
            this.Controls.Add(this.btnPlacerAgentsTest);
            this.Controls.Add(this.btnGenererCarteRef);
            this.Controls.Add(this.btnStartAffichage);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Test SMA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnStartAffichage;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnGenererCarteRef;
        private System.Windows.Forms.Button btnPlacerAgentsTest;
        private System.Windows.Forms.Button btnStopAffichage;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnPauseAffichage;
        private System.Windows.Forms.Button btnObjectifTerre;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnDryad;
        private System.Windows.Forms.Button btnRefreshImage;
    }
}

