namespace SaisieLivre
{
    partial class TradForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradForm));
            this.CB_SelTraducteur = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BT_CreateTrad = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_Nom = new System.Windows.Forms.TextBox();
            this.TB_Prenom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_SelTraducteur
            // 
            this.CB_SelTraducteur.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_SelTraducteur.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.CB_SelTraducteur.FormattingEnabled = true;
            this.CB_SelTraducteur.Location = new System.Drawing.Point(12, 21);
            this.CB_SelTraducteur.Name = "CB_SelTraducteur";
            this.CB_SelTraducteur.Size = new System.Drawing.Size(357, 21);
            this.CB_SelTraducteur.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(256, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Sélectionner";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BT_Select_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BT_CreateTrad);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TB_Nom);
            this.groupBox1.Controls.Add(this.TB_Prenom);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 122);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nouveau Traducteur";
            // 
            // BT_CreateTrad
            // 
            this.BT_CreateTrad.Enabled = false;
            this.BT_CreateTrad.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_CreateTrad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_CreateTrad.Location = new System.Drawing.Point(276, 89);
            this.BT_CreateTrad.Name = "BT_CreateTrad";
            this.BT_CreateTrad.Size = new System.Drawing.Size(75, 23);
            this.BT_CreateTrad.TabIndex = 4;
            this.BT_CreateTrad.Text = "Créer";
            this.BT_CreateTrad.UseVisualStyleBackColor = true;
            this.BT_CreateTrad.Click += new System.EventHandler(this.BT_CreateTrad_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Prenom :";
            // 
            // TB_Nom
            // 
            this.TB_Nom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TB_Nom.Location = new System.Drawing.Point(72, 63);
            this.TB_Nom.Name = "TB_Nom";
            this.TB_Nom.Size = new System.Drawing.Size(279, 20);
            this.TB_Nom.TabIndex = 2;
            this.TB_Nom.TextChanged += new System.EventHandler(this.TB_Nom_TextChanged);
            // 
            // TB_Prenom
            // 
            this.TB_Prenom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TB_Prenom.Location = new System.Drawing.Point(72, 37);
            this.TB_Prenom.Name = "TB_Prenom";
            this.TB_Prenom.Size = new System.Drawing.Size(279, 20);
            this.TB_Prenom.TabIndex = 1;
            this.TB_Prenom.TextChanged += new System.EventHandler(this.TB_Prenom_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom :";
            // 
            // TradForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 209);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CB_SelTraducteur);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(397, 247);
            this.MinimumSize = new System.Drawing.Size(397, 113);
            this.Name = "TradForm";
            this.Text = "Traducteur";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BT_CreateTrad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_Nom;
        private System.Windows.Forms.TextBox TB_Prenom;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox CB_SelTraducteur;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}