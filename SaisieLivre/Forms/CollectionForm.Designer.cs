namespace SaisieLivre
{
    partial class CollectionForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TB_Editeur = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_CollName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CB_Genre_3 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CB_Genre_2 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CB_Genre_1 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CB_Genre_0 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_0 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CB_OBCode = new System.Windows.Forms.ComboBox();
            this.lb_obcode = new System.Windows.Forms.Label();
            this.TB_OBCode = new System.Windows.Forms.TextBox();
            this.TB_Web = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.CBX_PresentTitre = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CB_Lectorat = new System.Windows.Forms.ComboBox();
            this.TB_Largeur = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TB_Hauteur = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.TB_Auteurs = new System.Windows.Forms.TextBox();
            this.TB_NBPages = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CB_CodeSupport = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BT_Save = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TB_Presentation = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TB_Editeur);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TB_CollName);
            this.groupBox1.Location = new System.Drawing.Point(8, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // TB_Editeur
            // 
            this.TB_Editeur.BackColor = System.Drawing.SystemColors.Window;
            this.TB_Editeur.Location = new System.Drawing.Point(121, 45);
            this.TB_Editeur.Name = "TB_Editeur";
            this.TB_Editeur.ReadOnly = true;
            this.TB_Editeur.Size = new System.Drawing.Size(398, 20);
            this.TB_Editeur.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Editeur :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nom de la collection :";
            // 
            // TB_CollName
            // 
            this.TB_CollName.Location = new System.Drawing.Point(121, 19);
            this.TB_CollName.Name = "TB_CollName";
            this.TB_CollName.Size = new System.Drawing.Size(398, 20);
            this.TB_CollName.TabIndex = 0;
            this.TB_CollName.Leave += new System.EventHandler(this.TB_CollName_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CB_Genre_3);
            this.groupBox2.Controls.Add(this.TB_Genre_3);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.CB_Genre_2);
            this.groupBox2.Controls.Add(this.TB_Genre_2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.CB_Genre_1);
            this.groupBox2.Controls.Add(this.TB_Genre_1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.CB_Genre_0);
            this.groupBox2.Controls.Add(this.TB_Genre_0);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(8, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(535, 128);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Genre Principal par défaut :";
            // 
            // CB_Genre_3
            // 
            this.CB_Genre_3.FormattingEnabled = true;
            this.CB_Genre_3.Location = new System.Drawing.Point(166, 100);
            this.CB_Genre_3.Name = "CB_Genre_3";
            this.CB_Genre_3.Size = new System.Drawing.Size(353, 21);
            this.CB_Genre_3.TabIndex = 131;
            this.CB_Genre_3.SelectedIndexChanged += new System.EventHandler(this.CB_Genre_3_SelectedIndexChanged);
            // 
            // TB_Genre_3
            // 
            this.TB_Genre_3.Location = new System.Drawing.Point(120, 100);
            this.TB_Genre_3.Name = "TB_Genre_3";
            this.TB_Genre_3.Size = new System.Drawing.Size(39, 20);
            this.TB_Genre_3.TabIndex = 123;
            this.TB_Genre_3.Tag = "IDNC_GENRE4";
            this.TB_Genre_3.TextChanged += new System.EventHandler(this.TB_Genre_3_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 130;
            this.label7.Text = "Niveau 4 :";
            // 
            // CB_Genre_2
            // 
            this.CB_Genre_2.FormattingEnabled = true;
            this.CB_Genre_2.Location = new System.Drawing.Point(166, 73);
            this.CB_Genre_2.Name = "CB_Genre_2";
            this.CB_Genre_2.Size = new System.Drawing.Size(353, 21);
            this.CB_Genre_2.TabIndex = 129;
            this.CB_Genre_2.SelectedIndexChanged += new System.EventHandler(this.CB_Genre_2_SelectedIndexChanged);
            // 
            // TB_Genre_2
            // 
            this.TB_Genre_2.Location = new System.Drawing.Point(120, 73);
            this.TB_Genre_2.Name = "TB_Genre_2";
            this.TB_Genre_2.Size = new System.Drawing.Size(39, 20);
            this.TB_Genre_2.TabIndex = 122;
            this.TB_Genre_2.Tag = "IDNC_GENRE3";
            this.TB_Genre_2.TextChanged += new System.EventHandler(this.TB_Genre_2_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(61, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 128;
            this.label6.Text = "Niveau 3 :";
            // 
            // CB_Genre_1
            // 
            this.CB_Genre_1.FormattingEnabled = true;
            this.CB_Genre_1.Location = new System.Drawing.Point(166, 47);
            this.CB_Genre_1.Name = "CB_Genre_1";
            this.CB_Genre_1.Size = new System.Drawing.Size(353, 21);
            this.CB_Genre_1.TabIndex = 127;
            this.CB_Genre_1.SelectedIndexChanged += new System.EventHandler(this.CB_Genre_1_SelectedIndexChanged);
            // 
            // TB_Genre_1
            // 
            this.TB_Genre_1.Location = new System.Drawing.Point(120, 47);
            this.TB_Genre_1.Name = "TB_Genre_1";
            this.TB_Genre_1.Size = new System.Drawing.Size(39, 20);
            this.TB_Genre_1.TabIndex = 121;
            this.TB_Genre_1.Tag = "IDNC_GENRE2";
            this.TB_Genre_1.TextChanged += new System.EventHandler(this.TB_Genre_1_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 126;
            this.label5.Text = "Niveau 2 :";
            // 
            // CB_Genre_0
            // 
            this.CB_Genre_0.FormattingEnabled = true;
            this.CB_Genre_0.Location = new System.Drawing.Point(166, 21);
            this.CB_Genre_0.Name = "CB_Genre_0";
            this.CB_Genre_0.Size = new System.Drawing.Size(353, 21);
            this.CB_Genre_0.TabIndex = 125;
            this.CB_Genre_0.SelectedIndexChanged += new System.EventHandler(this.CB_Genre_0_SelectedIndexChanged);
            // 
            // TB_Genre_0
            // 
            this.TB_Genre_0.Location = new System.Drawing.Point(120, 21);
            this.TB_Genre_0.Name = "TB_Genre_0";
            this.TB_Genre_0.Size = new System.Drawing.Size(39, 20);
            this.TB_Genre_0.TabIndex = 120;
            this.TB_Genre_0.Tag = "IDNC_GENRE1";
            this.TB_Genre_0.TextChanged += new System.EventHandler(this.TB_Genre_0_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 124;
            this.label4.Text = "Niveau 1 :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.CB_OBCode);
            this.groupBox3.Controls.Add(this.lb_obcode);
            this.groupBox3.Controls.Add(this.TB_OBCode);
            this.groupBox3.Controls.Add(this.TB_Web);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.CBX_PresentTitre);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.CB_Lectorat);
            this.groupBox3.Controls.Add(this.TB_Largeur);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.TB_Hauteur);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.TB_Auteurs);
            this.groupBox3.Controls.Add(this.TB_NBPages);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.CB_CodeSupport);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(8, 225);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(535, 159);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Autres valeurs par défaut : ";
            // 
            // CB_OBCode
            // 
            this.CB_OBCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_OBCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.CB_OBCode.BackColor = System.Drawing.SystemColors.Window;
            this.CB_OBCode.DropDownWidth = 500;
            this.CB_OBCode.FormattingEnabled = true;
            this.CB_OBCode.Location = new System.Drawing.Point(196, 128);
            this.CB_OBCode.Name = "CB_OBCode";
            this.CB_OBCode.Size = new System.Drawing.Size(322, 21);
            this.CB_OBCode.TabIndex = 12;
            this.CB_OBCode.Tag = "LIBOLDCSR";
            this.CB_OBCode.SelectedIndexChanged += new System.EventHandler(this.CB_OBCode_SelectedIndexChanged);
            // 
            // lb_obcode
            // 
            this.lb_obcode.AutoSize = true;
            this.lb_obcode.Location = new System.Drawing.Point(86, 131);
            this.lb_obcode.Name = "lb_obcode";
            this.lb_obcode.Size = new System.Drawing.Size(28, 13);
            this.lb_obcode.TabIndex = 73;
            this.lb_obcode.Text = "OB :";
            // 
            // TB_OBCode
            // 
            this.TB_OBCode.Location = new System.Drawing.Point(120, 128);
            this.TB_OBCode.Name = "TB_OBCode";
            this.TB_OBCode.Size = new System.Drawing.Size(67, 20);
            this.TB_OBCode.TabIndex = 11;
            this.TB_OBCode.Tag = "OBCODE";
            this.TB_OBCode.TextChanged += new System.EventHandler(this.TB_OBCode_TextChanged);
            // 
            // TB_Web
            // 
            this.TB_Web.Location = new System.Drawing.Point(120, 102);
            this.TB_Web.Name = "TB_Web";
            this.TB_Web.Size = new System.Drawing.Size(398, 20);
            this.TB_Web.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(57, 105);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Site Web :";
            // 
            // CBX_PresentTitre
            // 
            this.CBX_PresentTitre.AutoSize = true;
            this.CBX_PresentTitre.Location = new System.Drawing.Point(290, 77);
            this.CBX_PresentTitre.Name = "CBX_PresentTitre";
            this.CBX_PresentTitre.Size = new System.Drawing.Size(229, 17);
            this.CBX_PresentTitre.TabIndex = 9;
            this.CBX_PresentTitre.Text = "Presence du nom de collection dans le titre";
            this.CBX_PresentTitre.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(62, 78);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "Lectorat :";
            // 
            // CB_Lectorat
            // 
            this.CB_Lectorat.FormattingEnabled = true;
            this.CB_Lectorat.Location = new System.Drawing.Point(120, 75);
            this.CB_Lectorat.Name = "CB_Lectorat";
            this.CB_Lectorat.Size = new System.Drawing.Size(136, 21);
            this.CB_Lectorat.TabIndex = 8;
            this.CB_Lectorat.SelectedIndexChanged += new System.EventHandler(this.CB_Lectorat_SelectedIndexChanged);
            // 
            // TB_Largeur
            // 
            this.TB_Largeur.Location = new System.Drawing.Point(478, 49);
            this.TB_Largeur.Name = "TB_Largeur";
            this.TB_Largeur.Size = new System.Drawing.Size(41, 20);
            this.TB_Largeur.TabIndex = 7;
            this.TB_Largeur.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(423, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Largeur :";
            // 
            // TB_Hauteur
            // 
            this.TB_Hauteur.Location = new System.Drawing.Point(302, 48);
            this.TB_Hauteur.Name = "TB_Hauteur";
            this.TB_Hauteur.Size = new System.Drawing.Size(41, 20);
            this.TB_Hauteur.TabIndex = 6;
            this.TB_Hauteur.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(245, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Hauteur :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(197, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Auteurs :";
            // 
            // TB_Auteurs
            // 
            this.TB_Auteurs.Location = new System.Drawing.Point(252, 23);
            this.TB_Auteurs.Name = "TB_Auteurs";
            this.TB_Auteurs.Size = new System.Drawing.Size(267, 20);
            this.TB_Auteurs.TabIndex = 4;
            // 
            // TB_NBPages
            // 
            this.TB_NBPages.Location = new System.Drawing.Point(121, 49);
            this.TB_NBPages.Name = "TB_NBPages";
            this.TB_NBPages.Size = new System.Drawing.Size(41, 20);
            this.TB_NBPages.TabIndex = 5;
            this.TB_NBPages.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(57, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "NBPages :";
            // 
            // CB_CodeSupport
            // 
            this.CB_CodeSupport.FormattingEnabled = true;
            this.CB_CodeSupport.Location = new System.Drawing.Point(121, 23);
            this.CB_CodeSupport.Name = "CB_CodeSupport";
            this.CB_CodeSupport.Size = new System.Drawing.Size(41, 21);
            this.CB_CodeSupport.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "CodeSupport :";
            // 
            // BT_Save
            // 
            this.BT_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BT_Save.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.BT_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Save.Location = new System.Drawing.Point(8, 498);
            this.BT_Save.Name = "BT_Save";
            this.BT_Save.Size = new System.Drawing.Size(535, 23);
            this.BT_Save.TabIndex = 3;
            this.BT_Save.Text = "Enregistrer";
            this.BT_Save.UseVisualStyleBackColor = true;
            this.BT_Save.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.TB_Presentation);
            this.groupBox4.Location = new System.Drawing.Point(8, 390);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(535, 102);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Presentation";
            // 
            // TB_Presentation
            // 
            this.TB_Presentation.Location = new System.Drawing.Point(15, 16);
            this.TB_Presentation.Multiline = true;
            this.TB_Presentation.Name = "TB_Presentation";
            this.TB_Presentation.Size = new System.Drawing.Size(504, 78);
            this.TB_Presentation.TabIndex = 0;
            // 
            // CollectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 533);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.BT_Save);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(569, 571);
            this.MinimumSize = new System.Drawing.Size(569, 571);
            this.Name = "CollectionForm";
            this.Text = "Création / Modification de collection";
            this.Load += new System.EventHandler(this.CollectionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TB_Editeur;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox CB_Genre_3;
        private System.Windows.Forms.TextBox TB_Genre_3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CB_Genre_2;
        private System.Windows.Forms.TextBox TB_Genre_2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CB_Genre_1;
        private System.Windows.Forms.TextBox TB_Genre_1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CB_Genre_0;
        private System.Windows.Forms.TextBox TB_Genre_0;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox TB_Largeur;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TB_Hauteur;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TB_Auteurs;
        private System.Windows.Forms.TextBox TB_NBPages;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox CB_CodeSupport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_Web;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox CBX_PresentTitre;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox CB_Lectorat;
        private System.Windows.Forms.ComboBox CB_OBCode;
        private System.Windows.Forms.Label lb_obcode;
        private System.Windows.Forms.TextBox TB_OBCode;
        private System.Windows.Forms.Button BT_Save;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox TB_Presentation;
        public System.Windows.Forms.TextBox TB_CollName;
    }
}