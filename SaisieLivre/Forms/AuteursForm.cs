using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FirebirdDB;
using Ini;

namespace SaisieLivre
{
    public partial class AuteursForm : Form
    {
        public FDB db;
        public MainForm mf;
        public SaisieLivre.Forms.NewCollectionForm ncf;
        private ListBox LB_AutFunc, LB_AutFuncComp;


        private IniFile ini = new IniFile(Path.Combine(Directory.GetCurrentDirectory(), "config.ini"));
        private Dictionary<string, string> DLangue = new Dictionary<string,string>();
        private Dictionary<string, string> DPays = new Dictionary<string, string>();
        private Dictionary<string, string> DFuncAut = new Dictionary<string, string>();
        private bool StopGetFunc = false;

        public AuteursForm(Dictionary<string, string> DL, Dictionary<string, string> DP, Dictionary<string, string> DF)
        {
            InitializeComponent();            
            
            if (ini.IniReadValue("AUTEURS", "DICO") == "1")
            {
                CBX_CheckDico.Checked = true;
            }

            DLangue = DL;
            DPays = DP;
            DFuncAut = DF;

            listBox1.Items.Clear();
            listBox2.Items.Clear();

            foreach (var pair in DFuncAut)
            {
                CLB_FonctionsAuteur.Items.Add(pair.Value, false);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            char[] delim = new[] { ';', '+', '/' };

            foreach (string s in TB_Auteurs_From_Main.Text.Split(delim))
            {
                listBox1.Items.Add(s.Trim());
            }
        }

        private void GetFunctions(string current_name)
        {
            StopGetFunc = true;

            ListBox LBActive = LB_AutFunc;
            ListBox LBActiveComp = LB_AutFuncComp;

            if (ncf != null)
            {
                LBActive = ncf.LB_AutFunc;
                LBActiveComp = ncf.LB_AutFuncComp;
            }

            //on cherche les fonctions deja attribuées;
            foreach (string s in LBActive.Items)
            {
                int idx = -1;
                string strFunc = string.Empty;

                if (s == current_name)
                {
                    idx = LBActive.Items.IndexOf(s);
                }
                if (idx >= 0)
                {
                    strFunc = LBActiveComp.Items[idx].ToString();
                }
                if (strFunc != "")
                {
                    foreach (string code in strFunc.Split(','))
                    {
                        if (code != "")
                            CLB_FonctionsAuteur.SetItemCheckState(CLB_FonctionsAuteur.Items.IndexOf(DFuncAut[code]), CheckState.Checked);
                    }
                }
            }            
            
            StopGetFunc = false;
        }

        private void CLB_FonctionsAuteur_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!StopGetFunc)
            {                
                string Aut = listBox1.Items[listBox1.SelectedIndex].ToString().Trim();

                string Checked = ((CheckedListBox)sender).SelectedItem.ToString();
                string Functs = getfunccode(Checked); 
                bool add = true;

                if (e.NewValue == CheckState.Unchecked)
                {
                    add = false;
                    //MessageBox.Show(Aut + " " + Checked + " unchek");
                }

                if (LB_AutFunc.Items.IndexOf(Aut)<0)
                {
                    LB_AutFunc.Items.Add(Aut);
                    LB_AutFuncComp.Items.Add(Functs);
                }
                else
                {
                    int idx = LB_AutFunc.Items.IndexOf(Aut);

                    if (add)
                    {                        
                        LB_AutFuncComp.Items[idx] = LB_AutFuncComp.Items[idx] + "," + Functs;
                    }

                    else
                    {
                        LB_AutFuncComp.Items[idx] = LB_AutFuncComp.Items[idx].ToString().Replace(Functs, "");
                        LB_AutFuncComp.Items[idx] = LB_AutFuncComp.Items[idx].ToString().Replace(",,", ",");
                        
                        if (LB_AutFuncComp.Items[idx].ToString() == "" ||
                            LB_AutFuncComp.Items[idx].ToString() == ",")
                        {
                            LB_AutFuncComp.Items.RemoveAt(idx);
                            LB_AutFunc.Items.RemoveAt(idx);
                        }
                        
                    }
                }
            }
        }

        private void CheckInDicoAuteurs(string current_name)
        {
            db.Query(string.Format("select * from auteursunique where trim( nomauteur )='{0}'", current_name.ToUpper().Replace("'", "''")));

            string exist = string.Empty;

            foreach (var row in db.FetchAll())
            {
                exist = "OUI";
            }

            if (exist == "")
                exist = "NON";

            TB_exist.Text = exist;
            if (exist == "NON")
            {
                listBox2.Enabled = true;
                TB_Correction.Enabled = true;

                char[] delim = new[] { ' ', '-', ',', '.' }; //ajout de la virgule pour dissocier nom/prenom

                string compl = "";
                string startcompl = "( ";

                foreach (string s in current_name.ToUpper().Split(delim))
                {
                    if (s != string.Empty)
                    {
                        compl += " nomauteur containing '" + s.Replace("'", "''").Trim() + "' and";
                        startcompl += " nomauteur starts with '" + s.Substring(0, 1) + "' or";
                    }
                }

                if (compl != string.Empty)
                {
                    //compl = compl.Substring(0, compl.Length - 4);
                    startcompl = startcompl.Substring(0, startcompl.Length - 3) + " )";

                    db.Query("select * from auteursunique where " + compl + startcompl + " order by nomauteur" );
                    foreach (var row in db.FetchAll())
                        listBox2.Items.Add(row["nomauteur"].ToString());                    
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_exist.Text = "";
            BT_ShowDico.Enabled = false;
            
            StopGetFunc = true;
            CLB_FonctionsAuteur.ClearSelected();
            for (int i = 0; i < CLB_FonctionsAuteur.Items.Count; i++)
                CLB_FonctionsAuteur.SetItemCheckState(i, CheckState.Unchecked);

            StopGetFunc = false;
            
            CLB_FonctionsAuteur.Enabled = false;

            if (listBox1.SelectedIndex >= 0)
            {
                string current_name = listBox1.Items[listBox1.SelectedIndex].ToString();

                CLB_FonctionsAuteur.Enabled = true;

                GetFunctions(current_name);

                listBox2.Items.Clear();

                TB_IdxAuteur.Text = listBox1.SelectedIndex.ToString();


                if (CBX_CheckDico.Checked)
                {
                    CheckInDicoAuteurs(current_name);
                }
            }
            
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex>=0)
                TB_Correction.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
        }


        private void TB_Correction_TextChanged(object sender, EventArgs e)
        {
            if (TB_Correction.Text != "")
                BT_Modifier.Enabled = true;
            else
                BT_Modifier.Enabled = false;
        }


        private void BT_Modifier_Click(object sender, EventArgs e)
        {           
           
            /*string AutEnCours = listBox1.Items[Convert.ToInt32(TB_IdxAuteur.Text)].ToString();

            List<string> c = new List<string>();
            db.Query( "select gencod from livre where auteurs='"+ AutEnCours +"'");
            foreach (Row row in db.FetchAll())
            {
                if (!c.Contains(row["gencod"].ToString()))
                    c.Add( row["gencod"].ToString());
            }

            if (c.Count>0)
            {
                DialogResult DR = MessageBox.Show("Mettre à jour les " + c.Count.ToString() + " références liées à l'auteur " + AutEnCours, "confirmation", MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    foreach (string cab in c)
                    {
                        db.Query(string.Format("update livre set auteurs=STRReplace( auteurs, '{0}', '{1}', 0) where gencod='{2}'", AutEnCours.Replace("'", "''"), TB_Correction.Text.Replace("'", "''"), cab));
                    }
                }
            }
            */
            listBox1.Items.RemoveAt(Convert.ToInt32(TB_IdxAuteur.Text));
            listBox1.Items.Insert(Convert.ToInt32(TB_IdxAuteur.Text), TB_Correction.Text);            
            
            TB_Correction.Text = "";
            TB_exist.Text = "";

            listBox2.Items.Clear();

            listBox1.SelectedIndex = -1;
            listBox1.SelectedIndex = Convert.ToInt32(TB_IdxAuteur.Text);
        }


        private void BT_OK_Click(object sender, EventArgs e)
        {
            ListBox LB_Active = LB_AutFunc, 
                    LB_ActiveComp = LB_AutFuncComp;
            if (ncf != null)
            {
                LB_Active = ncf.LB_AutFunc;
                LB_ActiveComp = ncf.LB_AutFuncComp;
            }



            string NewAuteur = "";
            foreach (string s in listBox1.Items)
            {
                NewAuteur += s + " ; ";                
            }
            if (NewAuteur != "")
            {
                if (mf != null)
                    mf.TB_Auteurs.Text = NewAuteur.Substring(0, NewAuteur.Length - 3);
                else
                {
                    /*foreach (string s in listBox1.Items)
                    {
                        NewAuteur += s + " ; ";
                    }*/
                    if (NewAuteur != "")
                    {
                        //mf.TB_Auteurs.Text = NewAuteur.Substring(0, NewAuteur.Length - 3);
                        foreach (ListViewItem lvi in ncf.LV_Active.Items)
                        {
                            if (lvi.Text == "auteurs")
                            {
                                lvi.SubItems[1].Text = NewAuteur.Substring(0, NewAuteur.Length - 3);
                            }
                        }
                    }
                }
            }

            //nettoyage des fonctions auteur
            List<string> autfuncitem = new List<string>();

            foreach (string s in LB_AutFunc.Items)
                autfuncitem.Add(s);

            foreach (string s in autfuncitem)
            {
                if (!listBox1.Items.Contains(s))
                {
                    int idx = LB_Active.Items.IndexOf(s);
                    LB_ActiveComp.Items.RemoveAt(idx);
                    LB_Active.Items.RemoveAt(idx);
                }
            }
                        
            //impossible de redimensionner la listbox si on la parcourt dans une boucle, on passe par une liste intermediaire
            foreach (string s in autfuncitem)
            {
                if (!listBox1.Items.Contains(s))
                {
                    int idx = LB_Active.Items.IndexOf(s);
                    if (idx >= 0)
                    {
                        LB_ActiveComp.Items.RemoveAt(idx);
                        LB_Active.Items.RemoveAt(idx);
                    }
                }
            }

            this.Close();
        }


        private void TB_exist_TextChanged(object sender, EventArgs e)
        {
            if (TB_exist.Text != "")
            {
                BT_ShowDico.Enabled = true;
                if (TB_exist.Text == "OUI")
                    BT_ShowDico.Text = "Voir les infos";
                else
                    BT_ShowDico.Text = "Ajouter au dico";
            }
            else
            {
                BT_ShowDico.Enabled = true;
            }
        }


        private void BT_ShowDico_Click(object sender, EventArgs e)
        {
            bool Create = false;
            if (TB_exist.Text == "NON")
                Create = true;

            string CurrentAut = listBox1.Items[listBox1.SelectedIndex].ToString();

            string Nom = "";
            string Prenom = "";
            if (Create)
            {
                string[] CNameSplit = CurrentAut.Split(',');                
                Nom = CNameSplit[0].Trim();                
                
                if (CNameSplit.Length > 1)
                    Prenom = CurrentAut.Replace(Nom, "").Replace(",", "").Trim();
            }

            
            DicoAuteurForm DAF = new DicoAuteurForm(Create, CurrentAut, db, DLangue, DPays, Nom.ToLower(), Prenom.ToLower());
            DAF.ShowDialog();
        }


        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (TB_Correction.Text!="")
                BT_Modifier_Click(sender, e);
        }


        private string getfunccode(string lib)
        {
            foreach (var pair in DFuncAut)
            {
                if (pair.Value == lib)
                    return pair.Key;
            }
            return "";
        }

        public string UpperFirstChar(string str_in)
        {
            string str_out = str_in;

            if (str_in.Length > 1)
                str_out = str_in.Substring(0, 1).ToUpper() + str_in.Substring(1, str_in.Length - 1).ToLower();

            return str_out;
        }

        private void CBX_CheckDico_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ini.IniWriteValue("AUTEURS", "DICO", "1");

                string Current_Name = "";
                int idx = listBox1.SelectedIndex;

                if (idx >= 0 && listBox1.Items[idx].ToString() != "")
                {
                    Current_Name = listBox1.Items[idx].ToString();
                    CheckInDicoAuteurs(Current_Name);
                }
            }
            else
                ini.IniWriteValue("AUTEURS", "DICO", "0");
        }

        private void AuteursForm_Load(object sender, EventArgs e)
        {
            if (mf != null)
            {
                LB_AutFunc = mf.LB_AutFunc;
                LB_AutFuncComp = mf.LB_AutFuncComp;
            }
            else
            {
                LB_AutFunc = ncf.LB_AutFunc;
                LB_AutFuncComp = ncf.LB_AutFuncComp;
            }
            
        }

            
    }
}
