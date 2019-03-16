namespace SaisieLivre
{
    partial class DicoAuteurForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DicoAuteurForm));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TB_Biographie = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TB_Particule = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TB_Pays = new System.Windows.Forms.TextBox();
            this.TB_Langue = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CB_pays = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CB_langue = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_WebSite = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_Prenom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_Nom = new System.Windows.Forms.TextBox();
            this.BT_Valider = new System.Windows.Forms.Button();
            this.cbx_valide = new System.Windows.Forms.CheckBox();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.TB_Biographie);
            this.groupBox4.Location = new System.Drawing.Point(12, 295);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(385, 158);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Biographie";
            // 
            // TB_Biographie
            // 
            this.TB_Biographie.Location = new System.Drawing.Point(6, 16);
            this.TB_Biographie.Multiline = true;
            this.TB_Biographie.Name = "TB_Biographie";
            this.TB_Biographie.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TB_Biographie.Size = new System.Drawing.Size(373, 134);
            this.TB_Biographie.TabIndex = 0;
            this.TB_Biographie.Tag = "biographie";
            this.TB_Biographie.TextChanged += new System.EventHandler(this.ReplaceOfficeQuotes);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBox1);
            this.groupBox3.Controls.Add(this.TB_Particule);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.TB_Pays);
            this.groupBox3.Controls.Add(this.TB_Langue);
            this.groupBox3.Controls.Add(this.textBox7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.textBox5);
            this.groupBox3.Controls.Add(this.textBox6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.CB_pays);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.CB_langue);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.TB_WebSite);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.TB_Prenom);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.TB_Nom);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(385, 277);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Infos Auteur";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(38, 234);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // TB_Particule
            // 
            this.TB_Particule.Location = new System.Drawing.Point(330, 19);
            this.TB_Particule.Name = "TB_Particule";
            this.TB_Particule.Size = new System.Drawing.Size(40, 20);
            this.TB_Particule.TabIndex = 1;
            this.TB_Particule.Tag = "particule";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(270, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Particule :";
            // 
            // TB_Pays
            // 
            this.TB_Pays.Location = new System.Drawing.Point(330, 180);
            this.TB_Pays.Name = "TB_Pays";
            this.TB_Pays.Size = new System.Drawing.Size(40, 20);
            this.TB_Pays.TabIndex = 21;
            this.TB_Pays.Tag = "pays";
            this.TB_Pays.Text = "0";
            this.TB_Pays.Visible = false;
            this.TB_Pays.TextChanged += new System.EventHandler(this.TB_Pays_TextChanged);
            // 
            // TB_Langue
            // 
            this.TB_Langue.Location = new System.Drawing.Point(330, 154);
            this.TB_Langue.Name = "TB_Langue";
            this.TB_Langue.Size = new System.Drawing.Size(40, 20);
            this.TB_Langue.TabIndex = 20;
            this.TB_Langue.Tag = "langue";
            this.TB_Langue.Text = "0";
            this.TB_Langue.Visible = false;
            this.TB_Langue.TextChanged += new System.EventHandler(this.TB_Langue_TextChanged);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(85, 206);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(285, 20);
            this.textBox7.TabIndex = 12;
            this.textBox7.Tag = "wikipedia";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Wikipedia :";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(177, 180);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(70, 20);
            this.textBox4.TabIndex = 11;
            this.textBox4.Tag = "deces_annee";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(131, 180);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(40, 20);
            this.textBox5.TabIndex = 10;
            this.textBox5.Tag = "deces_mois";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(85, 180);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(40, 20);
            this.textBox6.TabIndex = 9;
            this.textBox6.Tag = "deces_jour";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Decès :";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(177, 154);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(70, 20);
            this.textBox3.TabIndex = 8;
            this.textBox3.Tag = "naissance_annee";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(131, 154);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(40, 20);
            this.textBox2.TabIndex = 7;
            this.textBox2.Tag = "naissance_mois";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(85, 154);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(40, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.Tag = "naissance_jour";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Naissance :";
            // 
            // CB_pays
            // 
            this.CB_pays.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_pays.FormattingEnabled = true;
            this.CB_pays.Location = new System.Drawing.Point(85, 127);
            this.CB_pays.Name = "CB_pays";
            this.CB_pays.Size = new System.Drawing.Size(285, 21);
            this.CB_pays.TabIndex = 5;
            this.CB_pays.SelectedIndexChanged += new System.EventHandler(this.CB_pays_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Pays : ";
            // 
            // CB_langue
            // 
            this.CB_langue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_langue.FormattingEnabled = true;
            this.CB_langue.Location = new System.Drawing.Point(85, 98);
            this.CB_langue.Name = "CB_langue";
            this.CB_langue.Size = new System.Drawing.Size(285, 21);
            this.CB_langue.TabIndex = 4;
            this.CB_langue.SelectedIndexChanged += new System.EventHandler(this.CB_langue_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Langue : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Site Officiel :";
            // 
            // TB_WebSite
            // 
            this.TB_WebSite.Location = new System.Drawing.Point(85, 72);
            this.TB_WebSite.Name = "TB_WebSite";
            this.TB_WebSite.Size = new System.Drawing.Size(285, 20);
            this.TB_WebSite.TabIndex = 3;
            this.TB_WebSite.Tag = "website";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Prénom : ";
            // 
            // TB_Prenom
            // 
            this.TB_Prenom.Location = new System.Drawing.Point(85, 45);
            this.TB_Prenom.Name = "TB_Prenom";
            this.TB_Prenom.Size = new System.Drawing.Size(285, 20);
            this.TB_Prenom.TabIndex = 2;
            this.TB_Prenom.Tag = "prenom";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nom : ";
            // 
            // TB_Nom
            // 
            this.TB_Nom.Location = new System.Drawing.Point(85, 19);
            this.TB_Nom.Name = "TB_Nom";
            this.TB_Nom.Size = new System.Drawing.Size(162, 20);
            this.TB_Nom.TabIndex = 0;
            this.TB_Nom.Tag = "nom";
            // 
            // BT_Valider
            // 
            this.BT_Valider.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_Valider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Valider.Location = new System.Drawing.Point(322, 459);
            this.BT_Valider.Name = "BT_Valider";
            this.BT_Valider.Size = new System.Drawing.Size(75, 23);
            this.BT_Valider.TabIndex = 6;
            this.BT_Valider.Text = "Valider";
            this.BT_Valider.UseVisualStyleBackColor = true;
            this.BT_Valider.Click += new System.EventHandler(this.BT_Valider_Click);
            // 
            // cbx_valide
            // 
            this.cbx_valide.AutoSize = true;
            this.cbx_valide.Location = new System.Drawing.Point(218, 463);
            this.cbx_valide.Name = "cbx_valide";
            this.cbx_valide.Size = new System.Drawing.Size(98, 17);
            this.cbx_valide.TabIndex = 10;
            this.cbx_valide.Text = "Certifier l\'auteur";
            this.cbx_valide.UseVisualStyleBackColor = true;
            // 
            // DicoAuteurForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 491);
            this.Controls.Add(this.cbx_valide);
            this.Controls.Add(this.BT_Valider);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(423, 529);
            this.MinimumSize = new System.Drawing.Size(423, 529);
            this.Name = "DicoAuteurForm";
            this.Text = "Dictionnaire Auteurs";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox TB_Biographie;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox TB_Particule;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TB_Pays;
        private System.Windows.Forms.TextBox TB_Langue;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CB_pays;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CB_langue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_WebSite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_Prenom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_Nom;
        private System.Windows.Forms.Button BT_Valider;
        private System.Windows.Forms.CheckBox cbx_valide;
    }
}