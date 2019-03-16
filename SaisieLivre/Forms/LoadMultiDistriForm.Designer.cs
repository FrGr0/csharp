namespace SaisieLivre
{
    partial class LoadMultiDistriForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadMultiDistriForm));
            this.LB_MultiDistri = new System.Windows.Forms.ListBox();
            this.BT_SelectMultiDistri = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_DistriPrincipal = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LB_MultiDistri
            // 
            this.LB_MultiDistri.FormattingEnabled = true;
            this.LB_MultiDistri.Location = new System.Drawing.Point(12, 112);
            this.LB_MultiDistri.Name = "LB_MultiDistri";
            this.LB_MultiDistri.Size = new System.Drawing.Size(260, 95);
            this.LB_MultiDistri.TabIndex = 0;
            this.LB_MultiDistri.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.LB_MultiDistri.DoubleClick += new System.EventHandler(this.LB_MultiDistri_DoubleClick);
            // 
            // BT_SelectMultiDistri
            // 
            this.BT_SelectMultiDistri.Enabled = false;
            this.BT_SelectMultiDistri.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_SelectMultiDistri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_SelectMultiDistri.Location = new System.Drawing.Point(12, 213);
            this.BT_SelectMultiDistri.Name = "BT_SelectMultiDistri";
            this.BT_SelectMultiDistri.Size = new System.Drawing.Size(260, 23);
            this.BT_SelectMultiDistri.TabIndex = 1;
            this.BT_SelectMultiDistri.Text = "Sélectionner le distributeur secondaire";
            this.BT_SelectMultiDistri.UseVisualStyleBackColor = true;
            this.BT_SelectMultiDistri.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Distributeurs secondaires :";
            // 
            // TB_DistriPrincipal
            // 
            this.TB_DistriPrincipal.Location = new System.Drawing.Point(12, 28);
            this.TB_DistriPrincipal.Name = "TB_DistriPrincipal";
            this.TB_DistriPrincipal.Size = new System.Drawing.Size(260, 20);
            this.TB_DistriPrincipal.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(12, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Sélectionner le distributeur principal";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Distributeur principal :";
            // 
            // LoadMultiDistriForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 248);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TB_DistriPrincipal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BT_SelectMultiDistri);
            this.Controls.Add(this.LB_MultiDistri);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(300, 286);
            this.MinimumSize = new System.Drawing.Size(300, 286);
            this.Name = "LoadMultiDistriForm";
            this.Text = "Multi Distribution";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LB_MultiDistri;
        private System.Windows.Forms.Button BT_SelectMultiDistri;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_DistriPrincipal;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
    }
}