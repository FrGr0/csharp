namespace SaisieLivre
{
    partial class DilicomForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DilicomForm));
            this.TB_Dilicom = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_collection = new System.Windows.Forms.TabPage();
            this.tp_serie = new System.Windows.Forms.TabPage();
            this.LV_Serie = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVDColChamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVDColValeur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LV_Dilicom = new System.Windows.Forms.ListView();
            this.TB_Serie = new System.Windows.Forms.TextBox();
            this.LV_Coll2 = new System.Windows.Forms.ListView();
            this.TB_Coll2 = new System.Windows.Forms.TextBox();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tp_collection.SuspendLayout();
            this.tp_serie.SuspendLayout();
            this.SuspendLayout();
            // 
            // TB_Dilicom
            // 
            this.TB_Dilicom.BackColor = System.Drawing.SystemColors.Info;
            this.TB_Dilicom.Enabled = false;
            this.TB_Dilicom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_Dilicom.Location = new System.Drawing.Point(12, 348);
            this.TB_Dilicom.Multiline = true;
            this.TB_Dilicom.Name = "TB_Dilicom";
            this.TB_Dilicom.ReadOnly = true;
            this.TB_Dilicom.Size = new System.Drawing.Size(410, 68);
            this.TB_Dilicom.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tp_collection);
            this.tabControl1.Controls.Add(this.tp_serie);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 100);
            this.tabControl1.TabIndex = 2;
            // 
            // tp_collection
            // 
            this.tp_collection.Controls.Add(this.LV_Coll2);
            this.tp_collection.Location = new System.Drawing.Point(4, 22);
            this.tp_collection.Name = "tp_collection";
            this.tp_collection.Padding = new System.Windows.Forms.Padding(3);
            this.tp_collection.Size = new System.Drawing.Size(192, 74);
            this.tp_collection.TabIndex = 0;
            this.tp_collection.Text = "Collection";
            this.tp_collection.UseVisualStyleBackColor = true;
            // 
            // tp_serie
            // 
            this.tp_serie.Controls.Add(this.LV_Serie);
            this.tp_serie.Location = new System.Drawing.Point(4, 22);
            this.tp_serie.Name = "tp_serie";
            this.tp_serie.Padding = new System.Windows.Forms.Padding(3);
            this.tp_serie.Size = new System.Drawing.Size(192, 74);
            this.tp_serie.TabIndex = 1;
            this.tp_serie.Text = "Série";
            this.tp_serie.UseVisualStyleBackColor = true;
            // 
            // LV_Serie
            // 
            this.LV_Serie.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.LV_Serie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LV_Serie.FullRowSelect = true;
            this.LV_Serie.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.LV_Serie.Location = new System.Drawing.Point(3, 3);
            this.LV_Serie.Name = "LV_Serie";
            this.LV_Serie.Size = new System.Drawing.Size(186, 68);
            this.LV_Serie.TabIndex = 2;
            this.LV_Serie.UseCompatibleStateImageBehavior = false;
            this.LV_Serie.View = System.Windows.Forms.View.Details;
            this.LV_Serie.DoubleClick += new System.EventHandler(this.LV_Dilicom_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Champ";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Valeur";
            this.columnHeader2.Width = 330;
            // 
            // LVDColChamp
            // 
            this.LVDColChamp.Text = "Champ";
            this.LVDColChamp.Width = 120;
            // 
            // LVDColValeur
            // 
            this.LVDColValeur.Text = "Valeur";
            this.LVDColValeur.Width = 330;
            // 
            // LV_Dilicom
            // 
            this.LV_Dilicom.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LVDColChamp,
            this.LVDColValeur});
            this.LV_Dilicom.FullRowSelect = true;
            this.LV_Dilicom.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.LV_Dilicom.Location = new System.Drawing.Point(12, 118);
            this.LV_Dilicom.Name = "LV_Dilicom";
            this.LV_Dilicom.Size = new System.Drawing.Size(248, 224);
            this.LV_Dilicom.TabIndex = 1;
            this.LV_Dilicom.UseCompatibleStateImageBehavior = false;
            this.LV_Dilicom.View = System.Windows.Forms.View.Details;
            this.LV_Dilicom.DoubleClick += new System.EventHandler(this.LV_Dilicom_DoubleClick);
            // 
            // TB_Serie
            // 
            this.TB_Serie.Location = new System.Drawing.Point(266, 322);
            this.TB_Serie.Name = "TB_Serie";
            this.TB_Serie.Size = new System.Drawing.Size(142, 20);
            this.TB_Serie.TabIndex = 3;
            // 
            // LV_Coll2
            // 
            this.LV_Coll2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.LV_Coll2.Dock = System.Windows.Forms.DockStyle.Right;
            this.LV_Coll2.FullRowSelect = true;
            this.LV_Coll2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.LV_Coll2.Location = new System.Drawing.Point(89, 3);
            this.LV_Coll2.Name = "LV_Coll2";
            this.LV_Coll2.Size = new System.Drawing.Size(100, 68);
            this.LV_Coll2.TabIndex = 0;
            this.LV_Coll2.UseCompatibleStateImageBehavior = false;
            this.LV_Coll2.View = System.Windows.Forms.View.Details;
            this.LV_Coll2.DoubleClick += new System.EventHandler(this.LV_Dilicom_DoubleClick);
            // 
            // TB_Coll2
            // 
            this.TB_Coll2.Location = new System.Drawing.Point(266, 296);
            this.TB_Coll2.Name = "TB_Coll2";
            this.TB_Coll2.Size = new System.Drawing.Size(142, 20);
            this.TB_Coll2.TabIndex = 4;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 110;
            // 
            // DilicomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 403);
            this.Controls.Add(this.TB_Coll2);
            this.Controls.Add(this.TB_Serie);
            this.Controls.Add(this.LV_Dilicom);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.TB_Dilicom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DilicomForm";
            this.Text = "Dilicom";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowDilicom_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.DilicomForm_ResizeEnd);
            this.Move += new System.EventHandler(this.DilicomForm_Move);
            this.tabControl1.ResumeLayout(false);
            this.tp_collection.ResumeLayout(false);
            this.tp_serie.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox TB_Dilicom;
        private System.Windows.Forms.TabPage tp_collection;
        private System.Windows.Forms.TabPage tp_serie;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader LVDColChamp;
        private System.Windows.Forms.ColumnHeader LVDColValeur;
        private System.Windows.Forms.ListView LV_Dilicom;
        public System.Windows.Forms.TextBox TB_Serie;
        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.ListView LV_Serie;
        private System.Windows.Forms.ListView LV_Coll2;
        public System.Windows.Forms.TextBox TB_Coll2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;

    }
}