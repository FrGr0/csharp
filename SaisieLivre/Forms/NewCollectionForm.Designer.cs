namespace SaisieLivre.Forms
{
    partial class NewCollectionForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewCollectionForm));
            this.LV_Active = new System.Windows.Forms.ListView();
            this.ch_coll_champ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_coll_donnee = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.P_OBCode = new System.Windows.Forms.Panel();
            this.CB_OBCode = new System.Windows.Forms.ComboBox();
            this.TB_OBCode = new System.Windows.Forms.TextBox();
            this.P_Genres = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CB_Genre_3 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CB_Genre_2 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CB_Genre_1 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CB_Genre_0 = new System.Windows.Forms.ComboBox();
            this.TB_Genre_0 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.BT_AutFunc_Coll = new System.Windows.Forms.Button();
            this.BT_AddGTLSecColl = new System.Windows.Forms.Button();
            this.BT_Annuler_Active = new System.Windows.Forms.Button();
            this.CLB_Hidden = new System.Windows.Forms.CheckedListBox();
            this.CB_Hidden = new SuggestComboBox.SuggestComboBox.CSuggestComboBox();
            this.TB_Edit_Hidden_Coll = new System.Windows.Forms.TextBox();
            this.BT_Save_Active = new System.Windows.Forms.Button();
            this.TB_Numbers = new System.Windows.Forms.TextBox();
            this.LB_AutFuncComp = new System.Windows.Forms.ListBox();
            this.LB_AutFunc = new System.Windows.Forms.ListBox();
            this.TB_Editeur = new System.Windows.Forms.TextBox();
            this.TB_Libelle = new System.Windows.Forms.TextBox();
            this.P_Langue = new System.Windows.Forms.Panel();
            this.TB_Langue = new System.Windows.Forms.TextBox();
            this.CB_Langue_2 = new System.Windows.Forms.ComboBox();
            this.CB_Langue_1 = new System.Windows.Forms.ComboBox();
            this.CB_Langue_0 = new System.Windows.Forms.ComboBox();
            this.BT_CreateSerie = new System.Windows.Forms.Button();
            this.LB_Suggest_Auteurs = new System.Windows.Forms.ListBox();
            this.LB_Serie_Soeurs = new System.Windows.Forms.ListBox();
            this.LB_Serie_Enfants = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LB_Serie_Gencods = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copierLesEANDansLePressePapierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajouterLesEANÀLaListeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chargerLaFicheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.P_OBCode.SuspendLayout();
            this.P_Genres.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.P_Langue.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LV_Active
            // 
            this.LV_Active.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV_Active.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_coll_champ,
            this.ch_coll_donnee});
            this.LV_Active.FullRowSelect = true;
            this.LV_Active.GridLines = true;
            this.LV_Active.Location = new System.Drawing.Point(12, 12);
            this.LV_Active.MultiSelect = false;
            this.LV_Active.Name = "LV_Active";
            this.LV_Active.Size = new System.Drawing.Size(736, 604);
            this.LV_Active.TabIndex = 7;
            this.LV_Active.TabStop = false;
            this.LV_Active.UseCompatibleStateImageBehavior = false;
            this.LV_Active.View = System.Windows.Forms.View.Details;
            this.LV_Active.ItemActivate += new System.EventHandler(this.LV_ItemActivate);
            // 
            // ch_coll_champ
            // 
            this.ch_coll_champ.Text = "champ";
            this.ch_coll_champ.Width = 120;
            // 
            // ch_coll_donnee
            // 
            this.ch_coll_donnee.Text = "valeur";
            this.ch_coll_donnee.Width = 608;
            // 
            // P_OBCode
            // 
            this.P_OBCode.Controls.Add(this.CB_OBCode);
            this.P_OBCode.Controls.Add(this.TB_OBCode);
            this.P_OBCode.Location = new System.Drawing.Point(136, 465);
            this.P_OBCode.Name = "P_OBCode";
            this.P_OBCode.Size = new System.Drawing.Size(541, 28);
            this.P_OBCode.TabIndex = 44;
            this.P_OBCode.Visible = false;
            this.P_OBCode.Leave += new System.EventHandler(this.P_OBCode_Leave);
            // 
            // CB_OBCode
            // 
            this.CB_OBCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_OBCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.CB_OBCode.BackColor = System.Drawing.SystemColors.Window;
            this.CB_OBCode.DropDownWidth = 500;
            this.CB_OBCode.FormattingEnabled = true;
            this.CB_OBCode.Location = new System.Drawing.Point(87, 3);
            this.CB_OBCode.Name = "CB_OBCode";
            this.CB_OBCode.Size = new System.Drawing.Size(451, 21);
            this.CB_OBCode.TabIndex = 75;
            this.CB_OBCode.Tag = "LIBOLDCSR";
            this.CB_OBCode.TextChanged += new System.EventHandler(this.CB_OBCode_TextChanged);
            // 
            // TB_OBCode
            // 
            this.TB_OBCode.Location = new System.Drawing.Point(3, 3);
            this.TB_OBCode.Name = "TB_OBCode";
            this.TB_OBCode.Size = new System.Drawing.Size(78, 20);
            this.TB_OBCode.TabIndex = 74;
            this.TB_OBCode.Tag = "OBCODE";
            this.TB_OBCode.TextChanged += new System.EventHandler(this.TB_OBCode_TextChanged);
            // 
            // P_Genres
            // 
            this.P_Genres.Controls.Add(this.groupBox1);
            this.P_Genres.Location = new System.Drawing.Point(136, 322);
            this.P_Genres.Name = "P_Genres";
            this.P_Genres.Size = new System.Drawing.Size(541, 137);
            this.P_Genres.TabIndex = 43;
            this.P_Genres.Visible = false;
            this.P_Genres.Leave += new System.EventHandler(this.P_Genres_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CB_Genre_3);
            this.groupBox1.Controls.Add(this.TB_Genre_3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.CB_Genre_2);
            this.groupBox1.Controls.Add(this.TB_Genre_2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.CB_Genre_1);
            this.groupBox1.Controls.Add(this.TB_Genre_1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.CB_Genre_0);
            this.groupBox1.Controls.Add(this.TB_Genre_0);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(3, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 128);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Genre Principal par défaut :";
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
            this.CB_Genre_1.TextChanged += new System.EventHandler(this.CB_Langue_1_TextChanged);
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(61, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 126;
            this.label8.Text = "Niveau 2 :";
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 124;
            this.label9.Text = "Niveau 1 :";
            // 
            // BT_AutFunc_Coll
            // 
            this.BT_AutFunc_Coll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BT_AutFunc_Coll.Enabled = false;
            this.BT_AutFunc_Coll.Image = global::SaisieLivre.Properties.Resources.user;
            this.BT_AutFunc_Coll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_AutFunc_Coll.Location = new System.Drawing.Point(173, 622);
            this.BT_AutFunc_Coll.Name = "BT_AutFunc_Coll";
            this.BT_AutFunc_Coll.Size = new System.Drawing.Size(115, 23);
            this.BT_AutFunc_Coll.TabIndex = 42;
            this.BT_AutFunc_Coll.Text = "Fonctions Auteurs";
            this.BT_AutFunc_Coll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BT_AutFunc_Coll.UseVisualStyleBackColor = true;
            this.BT_AutFunc_Coll.Click += new System.EventHandler(this.BT_AutFunc_Coll_Click);
            // 
            // BT_AddGTLSecColl
            // 
            this.BT_AddGTLSecColl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BT_AddGTLSecColl.Image = global::SaisieLivre.Properties.Resources._398091;
            this.BT_AddGTLSecColl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_AddGTLSecColl.Location = new System.Drawing.Point(12, 622);
            this.BT_AddGTLSecColl.Name = "BT_AddGTLSecColl";
            this.BT_AddGTLSecColl.Size = new System.Drawing.Size(155, 23);
            this.BT_AddGTLSecColl.TabIndex = 41;
            this.BT_AddGTLSecColl.Text = "Ajouter Genre Secondaire";
            this.BT_AddGTLSecColl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BT_AddGTLSecColl.UseVisualStyleBackColor = true;
            this.BT_AddGTLSecColl.Click += new System.EventHandler(this.BT_AddGTLSecColl_Click);
            // 
            // BT_Annuler_Active
            // 
            this.BT_Annuler_Active.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_Annuler_Active.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BT_Annuler_Active.Image = global::SaisieLivre.Properties.Resources.Red_Annuler1;
            this.BT_Annuler_Active.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Annuler_Active.Location = new System.Drawing.Point(541, 623);
            this.BT_Annuler_Active.Name = "BT_Annuler_Active";
            this.BT_Annuler_Active.Size = new System.Drawing.Size(70, 23);
            this.BT_Annuler_Active.TabIndex = 39;
            this.BT_Annuler_Active.Text = "Annuler";
            this.BT_Annuler_Active.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BT_Annuler_Active.UseVisualStyleBackColor = true;
            this.BT_Annuler_Active.Click += new System.EventHandler(this.BT_Annuler_Active_Click);
            // 
            // CLB_Hidden
            // 
            this.CLB_Hidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CLB_Hidden.CheckOnClick = true;
            this.CLB_Hidden.FormattingEnabled = true;
            this.CLB_Hidden.Items.AddRange(new object[] {
            " "});
            this.CLB_Hidden.Location = new System.Drawing.Point(358, 625);
            this.CLB_Hidden.Name = "CLB_Hidden";
            this.CLB_Hidden.Size = new System.Drawing.Size(32, 19);
            this.CLB_Hidden.TabIndex = 38;
            this.CLB_Hidden.TabStop = false;
            this.CLB_Hidden.Visible = false;
            this.CLB_Hidden.Leave += new System.EventHandler(this.TB_Edit_Hidden_Coll_Leave);
            // 
            // CB_Hidden
            // 
            this.CB_Hidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Hidden.FilterRule = null;
            this.CB_Hidden.FormattingEnabled = true;
            this.CB_Hidden.Location = new System.Drawing.Point(326, 624);
            this.CB_Hidden.Name = "CB_Hidden";
            this.CB_Hidden.PropertySelector = null;
            this.CB_Hidden.Size = new System.Drawing.Size(26, 21);
            this.CB_Hidden.SuggestBoxHeight = 96;
            this.CB_Hidden.SuggestListOrderRule = null;
            this.CB_Hidden.TabIndex = 37;
            this.CB_Hidden.TabStop = false;
            this.CB_Hidden.Visible = false;
            this.CB_Hidden.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_Edit_Hidden_Coll_KeyDown);
            this.CB_Hidden.Validating += new System.ComponentModel.CancelEventHandler(this.CB_Hidden_Validating);
            // 
            // TB_Edit_Hidden_Coll
            // 
            this.TB_Edit_Hidden_Coll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Edit_Hidden_Coll.Location = new System.Drawing.Point(297, 624);
            this.TB_Edit_Hidden_Coll.Name = "TB_Edit_Hidden_Coll";
            this.TB_Edit_Hidden_Coll.Size = new System.Drawing.Size(23, 20);
            this.TB_Edit_Hidden_Coll.TabIndex = 36;
            this.TB_Edit_Hidden_Coll.TabStop = false;
            this.TB_Edit_Hidden_Coll.Visible = false;
            this.TB_Edit_Hidden_Coll.TextChanged += new System.EventHandler(this.TB_Edit_Hidden_Coll_TextChanged);
            this.TB_Edit_Hidden_Coll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_Edit_Hidden_Coll_KeyDown);
            this.TB_Edit_Hidden_Coll.Leave += new System.EventHandler(this.TB_Edit_Hidden_Coll_Leave);
            // 
            // BT_Save_Active
            // 
            this.BT_Save_Active.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_Save_Active.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BT_Save_Active.Enabled = false;
            this.BT_Save_Active.Image = global::SaisieLivre.Properties.Resources.Green_Valider1;
            this.BT_Save_Active.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Save_Active.Location = new System.Drawing.Point(617, 623);
            this.BT_Save_Active.Name = "BT_Save_Active";
            this.BT_Save_Active.Size = new System.Drawing.Size(69, 23);
            this.BT_Save_Active.TabIndex = 35;
            this.BT_Save_Active.Text = "Modifier";
            this.BT_Save_Active.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BT_Save_Active.UseVisualStyleBackColor = true;
            this.BT_Save_Active.Click += new System.EventHandler(this.BT_Save_Active_Click);
            // 
            // TB_Numbers
            // 
            this.TB_Numbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Numbers.Location = new System.Drawing.Point(396, 625);
            this.TB_Numbers.Name = "TB_Numbers";
            this.TB_Numbers.Size = new System.Drawing.Size(32, 20);
            this.TB_Numbers.TabIndex = 40;
            this.TB_Numbers.TabStop = false;
            this.TB_Numbers.Visible = false;
            this.TB_Numbers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_Edit_Hidden_Coll_KeyDown);
            this.TB_Numbers.Leave += new System.EventHandler(this.TB_Edit_Hidden_Coll_Leave);
            // 
            // LB_AutFuncComp
            // 
            this.LB_AutFuncComp.FormattingEnabled = true;
            this.LB_AutFuncComp.Location = new System.Drawing.Point(680, 462);
            this.LB_AutFuncComp.Name = "LB_AutFuncComp";
            this.LB_AutFuncComp.Size = new System.Drawing.Size(64, 134);
            this.LB_AutFuncComp.TabIndex = 46;
            this.LB_AutFuncComp.Visible = false;
            // 
            // LB_AutFunc
            // 
            this.LB_AutFunc.FormattingEnabled = true;
            this.LB_AutFunc.Location = new System.Drawing.Point(680, 322);
            this.LB_AutFunc.Name = "LB_AutFunc";
            this.LB_AutFunc.Size = new System.Drawing.Size(64, 134);
            this.LB_AutFunc.TabIndex = 45;
            this.LB_AutFunc.Visible = false;
            // 
            // TB_Editeur
            // 
            this.TB_Editeur.Location = new System.Drawing.Point(644, 12);
            this.TB_Editeur.Name = "TB_Editeur";
            this.TB_Editeur.Size = new System.Drawing.Size(100, 20);
            this.TB_Editeur.TabIndex = 47;
            this.TB_Editeur.Visible = false;
            // 
            // TB_Libelle
            // 
            this.TB_Libelle.Location = new System.Drawing.Point(538, 12);
            this.TB_Libelle.Name = "TB_Libelle";
            this.TB_Libelle.Size = new System.Drawing.Size(100, 20);
            this.TB_Libelle.TabIndex = 48;
            this.TB_Libelle.Visible = false;
            // 
            // P_Langue
            // 
            this.P_Langue.Controls.Add(this.TB_Langue);
            this.P_Langue.Controls.Add(this.CB_Langue_2);
            this.P_Langue.Controls.Add(this.CB_Langue_1);
            this.P_Langue.Controls.Add(this.CB_Langue_0);
            this.P_Langue.Location = new System.Drawing.Point(136, 499);
            this.P_Langue.Name = "P_Langue";
            this.P_Langue.Size = new System.Drawing.Size(608, 28);
            this.P_Langue.TabIndex = 49;
            this.P_Langue.Visible = false;
            this.P_Langue.Leave += new System.EventHandler(this.P_Langue_Leave);
            // 
            // TB_Langue
            // 
            this.TB_Langue.Location = new System.Drawing.Point(2, 3);
            this.TB_Langue.MaxLength = 5;
            this.TB_Langue.Name = "TB_Langue";
            this.TB_Langue.Size = new System.Drawing.Size(39, 20);
            this.TB_Langue.TabIndex = 5;
            this.TB_Langue.Tag = "CODELANGUE";
            this.TB_Langue.TextChanged += new System.EventHandler(this.TB_Langue_TextChanged);
            // 
            // CB_Langue_2
            // 
            this.CB_Langue_2.DropDownWidth = 250;
            this.CB_Langue_2.FormattingEnabled = true;
            this.CB_Langue_2.Location = new System.Drawing.Point(421, 3);
            this.CB_Langue_2.Name = "CB_Langue_2";
            this.CB_Langue_2.Size = new System.Drawing.Size(185, 21);
            this.CB_Langue_2.TabIndex = 8;
            this.CB_Langue_2.TextChanged += new System.EventHandler(this.CB_Langue_2_TextChanged);
            // 
            // CB_Langue_1
            // 
            this.CB_Langue_1.DropDownWidth = 250;
            this.CB_Langue_1.FormattingEnabled = true;
            this.CB_Langue_1.Location = new System.Drawing.Point(233, 3);
            this.CB_Langue_1.Name = "CB_Langue_1";
            this.CB_Langue_1.Size = new System.Drawing.Size(185, 21);
            this.CB_Langue_1.TabIndex = 7;
            this.CB_Langue_1.TextChanged += new System.EventHandler(this.CB_Langue_1_TextChanged);
            // 
            // CB_Langue_0
            // 
            this.CB_Langue_0.FormattingEnabled = true;
            this.CB_Langue_0.Location = new System.Drawing.Point(45, 3);
            this.CB_Langue_0.Name = "CB_Langue_0";
            this.CB_Langue_0.Size = new System.Drawing.Size(185, 21);
            this.CB_Langue_0.TabIndex = 6;
            this.CB_Langue_0.TextChanged += new System.EventHandler(this.CB_Langue_0_TextChanged);
            // 
            // BT_CreateSerie
            // 
            this.BT_CreateSerie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_CreateSerie.Enabled = false;
            this.BT_CreateSerie.Image = global::SaisieLivre.Properties.Resources.Save_icon;
            this.BT_CreateSerie.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_CreateSerie.Location = new System.Drawing.Point(692, 623);
            this.BT_CreateSerie.Name = "BT_CreateSerie";
            this.BT_CreateSerie.Size = new System.Drawing.Size(56, 23);
            this.BT_CreateSerie.TabIndex = 50;
            this.BT_CreateSerie.Text = "Créer";
            this.BT_CreateSerie.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BT_CreateSerie.UseVisualStyleBackColor = true;
            this.BT_CreateSerie.Click += new System.EventHandler(this.BT_CreateSerie_Click);
            // 
            // LB_Suggest_Auteurs
            // 
            this.LB_Suggest_Auteurs.FormattingEnabled = true;
            this.LB_Suggest_Auteurs.Location = new System.Drawing.Point(136, 142);
            this.LB_Suggest_Auteurs.Name = "LB_Suggest_Auteurs";
            this.LB_Suggest_Auteurs.Size = new System.Drawing.Size(608, 160);
            this.LB_Suggest_Auteurs.TabIndex = 51;
            this.LB_Suggest_Auteurs.Visible = false;
            this.LB_Suggest_Auteurs.DoubleClick += new System.EventHandler(this.LB_Suggest_Auteurs_DoubleClick);
            this.LB_Suggest_Auteurs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LB_Suggest_Auteurs_KeyDown);
            this.LB_Suggest_Auteurs.Leave += new System.EventHandler(this.LB_Suggest_Auteurs_Leave);
            // 
            // LB_Serie_Soeurs
            // 
            this.LB_Serie_Soeurs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_Serie_Soeurs.FormattingEnabled = true;
            this.LB_Serie_Soeurs.Location = new System.Drawing.Point(12, 35);
            this.LB_Serie_Soeurs.Name = "LB_Serie_Soeurs";
            this.LB_Serie_Soeurs.Size = new System.Drawing.Size(244, 173);
            this.LB_Serie_Soeurs.TabIndex = 53;
            this.LB_Serie_Soeurs.Visible = false;
            this.LB_Serie_Soeurs.DoubleClick += new System.EventHandler(this.LB_Serie_Associees_DoubleClick);
            // 
            // LB_Serie_Enfants
            // 
            this.LB_Serie_Enfants.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_Serie_Enfants.FormattingEnabled = true;
            this.LB_Serie_Enfants.Location = new System.Drawing.Point(12, 237);
            this.LB_Serie_Enfants.Name = "LB_Serie_Enfants";
            this.LB_Serie_Enfants.Size = new System.Drawing.Size(244, 173);
            this.LB_Serie_Enfants.TabIndex = 55;
            this.LB_Serie_Enfants.Visible = false;
            this.LB_Serie_Enfants.DoubleClick += new System.EventHandler(this.LB_Serie_Associees_DoubleClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Séries Soeurs :";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 221);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "Séries Enfants :";
            this.label2.Visible = false;
            // 
            // LB_Serie_Gencods
            // 
            this.LB_Serie_Gencods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_Serie_Gencods.FormattingEnabled = true;
            this.LB_Serie_Gencods.IntegralHeight = false;
            this.LB_Serie_Gencods.Location = new System.Drawing.Point(12, 443);
            this.LB_Serie_Gencods.Name = "LB_Serie_Gencods";
            this.LB_Serie_Gencods.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.LB_Serie_Gencods.Size = new System.Drawing.Size(244, 173);
            this.LB_Serie_Gencods.TabIndex = 59;
            this.LB_Serie_Gencods.Visible = false;
            this.LB_Serie_Gencods.DoubleClick += new System.EventHandler(this.LB_Serie_Gencods_DoubleClick);
            this.LB_Serie_Gencods.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LB_Serie_Gencods_KeyDown);
            this.LB_Serie_Gencods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LB_Serie_Gencods_MouseDown);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 427);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 60;
            this.label3.Text = "Références associées :";
            this.label3.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copierLesEANDansLePressePapierToolStripMenuItem,
            this.ajouterLesEANÀLaListeToolStripMenuItem,
            this.chargerLaFicheToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(323, 70);
            // 
            // copierLesEANDansLePressePapierToolStripMenuItem
            // 
            this.copierLesEANDansLePressePapierToolStripMenuItem.Name = "copierLesEANDansLePressePapierToolStripMenuItem";
            this.copierLesEANDansLePressePapierToolStripMenuItem.Size = new System.Drawing.Size(322, 22);
            this.copierLesEANDansLePressePapierToolStripMenuItem.Text = "Copier les EAN dans le presse papier";
            this.copierLesEANDansLePressePapierToolStripMenuItem.Click += new System.EventHandler(this.copierLesEANDansLePressePapierToolStripMenuItem_Click);
            // 
            // ajouterLesEANÀLaListeToolStripMenuItem
            // 
            this.ajouterLesEANÀLaListeToolStripMenuItem.Name = "ajouterLesEANÀLaListeToolStripMenuItem";
            this.ajouterLesEANÀLaListeToolStripMenuItem.Size = new System.Drawing.Size(322, 22);
            this.ajouterLesEANÀLaListeToolStripMenuItem.Text = "Ajouter les EAN à la liste des références à traiter";
            this.ajouterLesEANÀLaListeToolStripMenuItem.Click += new System.EventHandler(this.ajouterLesEANÀLaListeToolStripMenuItem_Click);
            // 
            // chargerLaFicheToolStripMenuItem
            // 
            this.chargerLaFicheToolStripMenuItem.Enabled = false;
            this.chargerLaFicheToolStripMenuItem.Name = "chargerLaFicheToolStripMenuItem";
            this.chargerLaFicheToolStripMenuItem.Size = new System.Drawing.Size(322, 22);
            this.chargerLaFicheToolStripMenuItem.Text = "Charger la Fiche";
            this.chargerLaFicheToolStripMenuItem.Click += new System.EventHandler(this.chargerLaFicheToolStripMenuItem_Click);
            // 
            // NewCollectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 657);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LB_Serie_Gencods);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LB_Serie_Enfants);
            this.Controls.Add(this.LB_Serie_Soeurs);
            this.Controls.Add(this.LB_Suggest_Auteurs);
            this.Controls.Add(this.BT_CreateSerie);
            this.Controls.Add(this.P_Langue);
            this.Controls.Add(this.TB_Libelle);
            this.Controls.Add(this.TB_Editeur);
            this.Controls.Add(this.LB_AutFuncComp);
            this.Controls.Add(this.LB_AutFunc);
            this.Controls.Add(this.P_OBCode);
            this.Controls.Add(this.P_Genres);
            this.Controls.Add(this.BT_AutFunc_Coll);
            this.Controls.Add(this.BT_AddGTLSecColl);
            this.Controls.Add(this.BT_Annuler_Active);
            this.Controls.Add(this.CLB_Hidden);
            this.Controls.Add(this.CB_Hidden);
            this.Controls.Add(this.TB_Edit_Hidden_Coll);
            this.Controls.Add(this.BT_Save_Active);
            this.Controls.Add(this.TB_Numbers);
            this.Controls.Add(this.LV_Active);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewCollectionForm";
            this.Text = "NewCollectionForm";
            this.Load += new System.EventHandler(this.NewCollectionForm_Load);
            this.P_OBCode.ResumeLayout(false);
            this.P_OBCode.PerformLayout();
            this.P_Genres.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.P_Langue.ResumeLayout(false);
            this.P_Langue.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader ch_coll_champ;
        private System.Windows.Forms.ColumnHeader ch_coll_donnee;
        private System.Windows.Forms.Panel P_OBCode;
        private System.Windows.Forms.ComboBox CB_OBCode;
        private System.Windows.Forms.TextBox TB_OBCode;
        private System.Windows.Forms.Panel P_Genres;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CB_Genre_3;
        private System.Windows.Forms.TextBox TB_Genre_3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CB_Genre_2;
        private System.Windows.Forms.TextBox TB_Genre_2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CB_Genre_1;
        private System.Windows.Forms.TextBox TB_Genre_1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox CB_Genre_0;
        private System.Windows.Forms.TextBox TB_Genre_0;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BT_AutFunc_Coll;
        private System.Windows.Forms.Button BT_AddGTLSecColl;
        private System.Windows.Forms.Button BT_Annuler_Active;
        private System.Windows.Forms.CheckedListBox CLB_Hidden;
        private SuggestComboBox.SuggestComboBox.CSuggestComboBox CB_Hidden;
        private System.Windows.Forms.TextBox TB_Edit_Hidden_Coll;
        private System.Windows.Forms.Button BT_Save_Active;
        private System.Windows.Forms.TextBox TB_Numbers;
        public System.Windows.Forms.ListBox LB_AutFuncComp;
        public System.Windows.Forms.ListBox LB_AutFunc;
        public System.Windows.Forms.ListView LV_Active;
        public System.Windows.Forms.TextBox TB_Editeur;
        public System.Windows.Forms.TextBox TB_Libelle;
        private System.Windows.Forms.Panel P_Langue;
        private System.Windows.Forms.TextBox TB_Langue;
        private System.Windows.Forms.ComboBox CB_Langue_2;
        private System.Windows.Forms.ComboBox CB_Langue_1;
        private System.Windows.Forms.ComboBox CB_Langue_0;
        private System.Windows.Forms.Button BT_CreateSerie;
        private System.Windows.Forms.ListBox LB_Suggest_Auteurs;
        private System.Windows.Forms.ListBox LB_Serie_Soeurs;
        private System.Windows.Forms.ListBox LB_Serie_Enfants;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox LB_Serie_Gencods;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copierLesEANDansLePressePapierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ajouterLesEANÀLaListeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chargerLaFicheToolStripMenuItem;

    }
}