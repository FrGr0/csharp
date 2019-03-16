namespace SaisieLivre
{
    partial class AuteursForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuteursForm));
            this.TB_Auteurs_From_Main = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBX_CheckDico = new System.Windows.Forms.CheckBox();
            this.BT_ShowDico = new System.Windows.Forms.Button();
            this.TB_IdxAuteur = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TB_Correction = new System.Windows.Forms.TextBox();
            this.BT_Modifier = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.TB_exist = new System.Windows.Forms.TextBox();
            this.BT_OK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CLB_FonctionsAuteur = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TB_Auteurs_From_Main
            // 
            this.TB_Auteurs_From_Main.Location = new System.Drawing.Point(105, 12);
            this.TB_Auteurs_From_Main.Name = "TB_Auteurs_From_Main";
            this.TB_Auteurs_From_Main.ReadOnly = true;
            this.TB_Auteurs_From_Main.Size = new System.Drawing.Size(569, 20);
            this.TB_Auteurs_From_Main.TabIndex = 0;
            this.TB_Auteurs_From_Main.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Champ Auteurs :";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 19);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(364, 173);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBX_CheckDico);
            this.groupBox1.Controls.Add(this.BT_ShowDico);
            this.groupBox1.Controls.Add(this.TB_IdxAuteur);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TB_Correction);
            this.groupBox1.Controls.Add(this.BT_Modifier);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.listBox2);
            this.groupBox1.Controls.Add(this.TB_exist);
            this.groupBox1.Location = new System.Drawing.Point(6, 253);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 202);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dictionnaire";
            // 
            // CBX_CheckDico
            // 
            this.CBX_CheckDico.AutoSize = true;
            this.CBX_CheckDico.Location = new System.Drawing.Point(14, 25);
            this.CBX_CheckDico.Name = "CBX_CheckDico";
            this.CBX_CheckDico.Size = new System.Drawing.Size(210, 17);
            this.CBX_CheckDico.TabIndex = 9;
            this.CBX_CheckDico.Text = "Vérifier la présence dans le dictionnaire";
            this.CBX_CheckDico.UseVisualStyleBackColor = true;
            this.CBX_CheckDico.CheckedChanged += new System.EventHandler(this.CBX_CheckDico_CheckedChanged);
            // 
            // BT_ShowDico
            // 
            this.BT_ShowDico.Enabled = false;
            this.BT_ShowDico.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_ShowDico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_ShowDico.Location = new System.Drawing.Point(269, 21);
            this.BT_ShowDico.Name = "BT_ShowDico";
            this.BT_ShowDico.Size = new System.Drawing.Size(101, 22);
            this.BT_ShowDico.TabIndex = 8;
            this.BT_ShowDico.Text = "Ajouter au dico";
            this.BT_ShowDico.UseVisualStyleBackColor = true;
            this.BT_ShowDico.Click += new System.EventHandler(this.BT_ShowDico_Click);
            // 
            // TB_IdxAuteur
            // 
            this.TB_IdxAuteur.Location = new System.Drawing.Point(260, 173);
            this.TB_IdxAuteur.Name = "TB_IdxAuteur";
            this.TB_IdxAuteur.Size = new System.Drawing.Size(27, 20);
            this.TB_IdxAuteur.TabIndex = 7;
            this.TB_IdxAuteur.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Correction : ";
            // 
            // TB_Correction
            // 
            this.TB_Correction.Enabled = false;
            this.TB_Correction.Location = new System.Drawing.Point(88, 140);
            this.TB_Correction.Name = "TB_Correction";
            this.TB_Correction.Size = new System.Drawing.Size(282, 20);
            this.TB_Correction.TabIndex = 5;
            this.TB_Correction.TextChanged += new System.EventHandler(this.TB_Correction_TextChanged);
            // 
            // BT_Modifier
            // 
            this.BT_Modifier.Enabled = false;
            this.BT_Modifier.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_Modifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Modifier.Location = new System.Drawing.Point(293, 171);
            this.BT_Modifier.Name = "BT_Modifier";
            this.BT_Modifier.Size = new System.Drawing.Size(77, 22);
            this.BT_Modifier.TabIndex = 4;
            this.BT_Modifier.Text = "Modifier";
            this.BT_Modifier.UseVisualStyleBackColor = true;
            this.BT_Modifier.Click += new System.EventHandler(this.BT_Modifier_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Suggestions :";
            // 
            // listBox2
            // 
            this.listBox2.Enabled = false;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(88, 52);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(282, 82);
            this.listBox2.TabIndex = 2;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            this.listBox2.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
            // 
            // TB_exist
            // 
            this.TB_exist.Location = new System.Drawing.Point(225, 23);
            this.TB_exist.Name = "TB_exist";
            this.TB_exist.ReadOnly = true;
            this.TB_exist.Size = new System.Drawing.Size(38, 20);
            this.TB_exist.TabIndex = 1;
            this.TB_exist.TextChanged += new System.EventHandler(this.TB_exist_TextChanged);
            // 
            // BT_OK
            // 
            this.BT_OK.BackColor = System.Drawing.Color.Green;
            this.BT_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_OK.ForeColor = System.Drawing.Color.White;
            this.BT_OK.Location = new System.Drawing.Point(6, 461);
            this.BT_OK.Name = "BT_OK";
            this.BT_OK.Size = new System.Drawing.Size(682, 23);
            this.BT_OK.TabIndex = 7;
            this.BT_OK.Text = "OK";
            this.BT_OK.UseVisualStyleBackColor = false;
            this.BT_OK.Click += new System.EventHandler(this.BT_OK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Location = new System.Drawing.Point(6, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 202);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auteurs Correspondants";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.CLB_FonctionsAuteur);
            this.groupBox3.Location = new System.Drawing.Point(396, 45);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(292, 410);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fonctions";
            // 
            // CLB_FonctionsAuteur
            // 
            this.CLB_FonctionsAuteur.CheckOnClick = true;
            this.CLB_FonctionsAuteur.Enabled = false;
            this.CLB_FonctionsAuteur.FormattingEnabled = true;
            this.CLB_FonctionsAuteur.Location = new System.Drawing.Point(6, 19);
            this.CLB_FonctionsAuteur.Name = "CLB_FonctionsAuteur";
            this.CLB_FonctionsAuteur.Size = new System.Drawing.Size(280, 379);
            this.CLB_FonctionsAuteur.TabIndex = 0;
            this.CLB_FonctionsAuteur.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CLB_FonctionsAuteur_ItemCheck);
            // 
            // AuteursForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 491);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BT_OK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_Auteurs_From_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AuteursForm";
            this.Text = "Verification Auteurs";
            this.Load += new System.EventHandler(this.AuteursForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.TextBox TB_Auteurs_From_Main;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.TextBox TB_exist;
        private System.Windows.Forms.Button BT_Modifier;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TB_Correction;
        private System.Windows.Forms.Button BT_OK;
        private System.Windows.Forms.TextBox TB_IdxAuteur;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BT_ShowDico;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox CLB_FonctionsAuteur;
        private System.Windows.Forms.CheckBox CBX_CheckDico;
    }
}