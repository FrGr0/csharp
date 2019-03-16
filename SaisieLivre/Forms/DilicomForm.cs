using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SaisieLivre
{
    public partial class DilicomForm : Form
    {
        public bool CanMove = true;
        public Point DefaultLocation;
        private MainForm mf;
        private bool set_tabpages = false;

        public Dictionary<string, Control> DilicomToTL = new Dictionary<string, Control>();

        public DilicomForm( Dictionary<string, Control> D, MainForm _mf, bool tabpages=false )
        {
            DilicomToTL = D;
            mf = _mf;
            set_tabpages = tabpages;

            //ce formulaire sert uniquement à la consultation de la table livrebis
            InitializeComponent();

            //9782205045284

            if (!set_tabpages)
            {
                tabControl1.SendToBack();
                LV_Dilicom.BringToFront();
                LV_Dilicom.Dock = DockStyle.Fill;
            }
            else
            {
                tabControl1.Dock = DockStyle.Fill;
                tabControl1.BringToFront();
                tabControl1.TabPages[0].Controls.Add(LV_Dilicom);

                LV_Dilicom.Width = (tabControl1.Width / 2) - 5;
                LV_Dilicom.Dock = DockStyle.Left;
                
                LV_Coll2.Width = (tabControl1.Width / 2) - 5;
            }


            LV_Dilicom.FullRowSelect = true;
            LV_Coll2.FullRowSelect = true;

            ColumnHeader c = LV_Dilicom.Columns[1];
            c.Width = -2;
        }

        private void ShowDilicom_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; //annule la fermeture
        }

        private void DilicomForm_Move(object sender, EventArgs e)
        {
            if (!CanMove)
            {
                this.Location = DefaultLocation;
            }
        }

        public void TB_Dilicom_ToLV( int idx = 0 )
        {
            ListView LV = LV_Dilicom;
            TextBox TB = TB_Dilicom;
            
            if (idx==1)
            {
                LV = LV_Serie;
                TB = TB_Serie;
            }
            
            LV.Items.Clear();
            LV_Coll2.Items.Clear();

            #region 2eme listview collection            

            foreach (string s in TB_Coll2.Text.Split('\n'))
            {
                //MessageBox.Show(s);

                string[] s_ = s.Replace("\r", "").Split( '~' );
                if (s_.Length >= 2)
                {
                    ListViewItem lvi = new ListViewItem(s_[0]);

                    if (DilicomToTL.ContainsKey(s_[0]))
                    {                        
                        if (s_[1].ToString() == "")
                            lvi.ForeColor = Color.Red;

                        else if (s_[0] == "collection" || s_[0] == "distributeur" || s_[0] == "liblectorat")
                        {
                            if (((ComboBox)DilicomToTL[s_[0]]).Items.Contains(s_[1]))
                            {
                                if (((ComboBox)DilicomToTL[s_[0]]).Text != s_[1])
                                    lvi.ForeColor = Color.Blue;
                            }
                            else
                                lvi.ForeColor = Color.Red;                                            
                        }

                        else if (s_[0] == "codegenre")
                        {
                            string Val = s_[1];

                            while (Val.Length < 8)
                                Val = "0" + Val;

                            Val = Val.Substring(0, 2) + "-" + Val.Substring(2, 2) + "-" + Val.Substring(4, 2) + "-" + Val.Substring(6, 2);
                            
                            if (DilicomToTL[s_[0]].Text != Val)
                                lvi.ForeColor = Color.Blue;

                        }
                        else if (s_[0] == "dateparution")
                        {                        
                            if (((DateTimePicker)DilicomToTL[s_[0]]).Value.ToString( "dd/MM/yyyy" ) != s_[1].Trim())
                                lvi.ForeColor = Color.Blue;
                        }
                        else if ( s_[0] == "supportclil" )
                        {
                            if (s_[1] == "L" || s_[1] == "R" || s_[1] == "B")
                            {
                                if (!((CheckBox)DilicomToTL[s_[1]]).Checked)
                                    lvi.ForeColor = Color.Blue;
                            }
                        }
                            
                        else if (DilicomToTL[s_[0]].Text != s_[1])
                            lvi.ForeColor = Color.Blue;
                        
                    }

                    lvi.SubItems.Add(s_[1].ToString());
                    LV_Coll2.Items.Add(lvi);
                }
            }
            #endregion

            foreach (string s in TB.Text.Split('\n'))
            {                
                string[] s_ = s.Replace("\r", "").Split('~');
                if (s_.Length >= 2)
                {
                    ListViewItem lvi = new ListViewItem(s_[0]);

                    if (DilicomToTL.ContainsKey(s_[0]) || (s_[0].Length>17 && s_[0].Substring(0,17)=="genre_secondaire_"))
                    {
                        if (s_[1].ToString() == "")
                            lvi.ForeColor = Color.Red;

                        else if (s_[0] == "collection" || s_[0] == "distributeur" || s_[0] == "liblectorat")
                        {
                            if (((ComboBox)DilicomToTL[s_[0]]).Items.Contains(s_[1]))
                            {
                                if (((ComboBox)DilicomToTL[s_[0]]).Text != s_[1])
                                    lvi.ForeColor = Color.Blue;
                            }
                            else
                                lvi.ForeColor = Color.Red;
                        }

                        else if (s_[0] == "codegenre" || s_[0]=="genre_principal")
                        {                            
                            s_[1] = mf.SetGTL(s_[1]);
                            
                            if (DilicomToTL[s_[0]].Text != s_[1])
                                lvi.ForeColor = Color.Blue;

                        }
                        else if (s_[0].Length > 17 && s_[0].Substring(0, 17) == "genre_secondaire_")
                        {                            
                            s_[1] = mf.SetGTL(s_[1]);
                            
                            bool setblue = true;                            
                            foreach (MyListBoxItem mlbi in mf.LB_Genres.Items)
                            {                               
                                if(mlbi.Message == s_[1])
                                {
                                    setblue = false;
                                    break;
                                }
                            }

                            if (setblue)
                                lvi.ForeColor = Color.Blue;
                        }

                        else if (s_[0] == "dateparution")
                        {
                            if (((DateTimePicker)DilicomToTL[s_[0]]).Value.ToString("dd/MM/yyyy") != s_[1].Trim())
                                lvi.ForeColor = Color.Blue;
                        }
                        else if (s_[0] == "supportclil")
                        {
                            if (s_[1] == "L" || s_[1] == "R" || s_[1] == "B")
                            {
                                if (!((CheckBox)DilicomToTL[s_[1]]).Checked)
                                    lvi.ForeColor = Color.Blue;
                            }
                        }

                        else if (DilicomToTL[s_[0]].Text != s_[1])
                            lvi.ForeColor = Color.Blue;

                    }

                    lvi.SubItems.Add(s_[1].ToString());
                    LV.Items.Add(lvi);
                }
            }

        }

        

        private void LV_Dilicom_DoubleClick(object sender, EventArgs e)
        {
            int idx = ((ListView)sender).SelectedIndices[0];
            ListViewItem lvi = ((ListView)sender).Items[idx];
            string Key = lvi.SubItems[0].Text;
            string Val = lvi.SubItems[1].Text;

            if (DilicomToTL.ContainsKey(Key) || (Key.Length>17 &&  Key.Substring(0, 17) == "genre_secondaire_"))
            {
                if ((Key == "codegenre" || Key == "genre_principal") && Val != "")
                {
                    bool work = true;
                    bool remove = false;
                    bool deleteoldprincipal = false;
                    int lbidx = 0;
                    string oldprincipal = "";
                    int lbidxoldprincipal = 0;
                    foreach (MyListBoxItem mlbi in mf.LB_Genres.Items)
                    {
                        if (mlbi.ItemColor == Color.Blue)
                        {
                            oldprincipal = mlbi.Message;
                            lbidxoldprincipal = lbidx;
                            deleteoldprincipal = true;                            
                        }

                        if (mlbi.Message == Val)
                        {
                            if (mlbi.ItemColor == Color.Blue)
                            {
                                work = false;
                                break;
                            }
                            else
                            {
                                remove = true;
                                break;
                            }
                        }

                        lbidx++;
                    }
                    
                    if (work)
                    {       
                        if (remove)
                            mf.LB_Genres.Items.RemoveAt(lbidx);

                        if (deleteoldprincipal)
                            mf.LB_Genres.Items.RemoveAt(lbidxoldprincipal);
                                                                            
                        mf.LB_Genres.Items.Add(new MyListBoxItem(Color.Blue, Val, mf.boldfont));
                        mf.TB_GenrePrincipal.Text = Val;
                        
                        lvi.ForeColor = SystemColors.ControlText;
                    }
                }                
                
                else if ((Key.Length > 17 && Key.Substring(0, 17) == "genre_secondaire_") && Val != "")
                {
                    bool work = true;
                    foreach (MyListBoxItem mlbi in mf.LB_Genres.Items)
                    {
                        if (mlbi.Message == Val)
                        {
                            work = false;
                            break;
                        }
                    }
                    if (work)
                    {
                        //mf.LoadGenres(Val.Replace("-", "") + ":0");
                        mf.LB_Genres.Items.Add(new MyListBoxItem(SystemColors.WindowText, Val, mf.deffont));
                        lvi.ForeColor = SystemColors.ControlText;
                    }
                }

                else if (Key == "collection" || Key == "distributeur")
                {
                    if (((ComboBox)DilicomToTL[Key]).Items.Contains(Val))
                    {
                        DilicomToTL[Key].Text = Val;
                        lvi.ForeColor = SystemColors.ControlText;
                    }
                    else
                        MessageBox.Show("la donnée " + this.Text + " \"" + Key + "\" n'existe pas dans l'annuaire "+ Key +".");
                }

                else if (Key == "supportclil")
                {
                    if (Val == "L")
                        mf.CBX_Luxe.Checked = true;

                    else if (Val == "R")
                        mf.CBX_relie.Checked = true;

                    else if (Val == "B")
                        mf.CBX_broche.Checked = true;

                    lvi.ForeColor = SystemColors.ControlText;
                }

                else
                {                    
                    if (Val == "")
                        MessageBox.Show("la donnée " + this.Text + " \"" + Key + "\" est vide");

                    else if (Val == DilicomToTL[Key].Text)
                        MessageBox.Show("la donnée " + this.Text + " \"" + Key + "\" est identique");

                    else
                    {                        
                        DilicomToTL[Key].Text = Val;
                        if (Key == "codelangue")
                        {
                            //on force 3 clicks pour les 3 niveaux... à cause des recalculs de listes genres2/3
                            DilicomToTL[Key].Text = Val;
                            DilicomToTL[Key].Text = Val;
                        }


                        lvi.ForeColor = SystemColors.ControlText;
                    }
                }
            }
        }

        public void SetOnTop()
        {
            this.Activate();
            this.BringToFront();
            mf.Focus();

        }


        private void DilicomForm_ResizeEnd(object sender, EventArgs e)
        {
            if (set_tabpages)
            {
                LV_Dilicom.Width = (tabControl1.Width / 2) - 5;
                LV_Coll2.Width = (tabControl1.Width / 2) - 5;
            }
        }

    }
}
