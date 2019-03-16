using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdDB;

namespace SaisieLivre
{
    public partial class TradForm : Form
    {
        private FDB db;
        public TextBox TB;
        private Dictionary<string, string> DTrad;

        public TradForm(Dictionary<string, string> Tra, TextBox T, FDB xdb)
        {
            InitializeComponent();

            CB_SelTraducteur.Sorted = true;

            //on transmet les paramètres aux variables
            TB = T;            
            DTrad = Tra;
            db = xdb;

            var source = new AutoCompleteStringCollection();         

            CB_SelTraducteur.Items.Clear();
            foreach (var p in Tra)
            {
                CB_SelTraducteur.Items.Add(p.Key);
                source.Add(p.Key);
            }
            CB_SelTraducteur.AutoCompleteCustomSource = source;
            
            if (TB.Text != "")
            {
                foreach (var pair in Tra)
                {
                    if (pair.Value == TB.Text)
                    {
                        CB_SelTraducteur.Text = pair.Key;
                        break;
                    }
                }
            }

        }

        private void BT_Select_Click(object sender, EventArgs e)
        {
            TB.Text = DTrad[CB_SelTraducteur.Text];
            this.Close();
        }


        private void BT_CreateTrad_Click(object sender, EventArgs e)
        {
            db.Query( string.Format("SELECT * from TRADUCTEURS "+
                                    "where STRCRUNCH( nom, 1 )= STRCRUNCH( '{0}', 1 ) and "+
                                    "STRCRUNCH( prenom, 1 )= STRCRUNCH( '{1}', 1 )",
                                    TB_Nom.Text.Replace("'", "''"), TB_Prenom.Text.Replace( "'", "''")));
            
            string Found = "";
            foreach (var row in db.FetchAll())
            {
                Found = row["nom"].ToString() + ", " + row["prenom"].ToString();    
            }

            if (Found != "")
            {
                CB_SelTraducteur.Text = Found;
                MessageBox.Show("traducteur déjà existant !");
            }
            else
            {
                db.Query("select max(id) as a1 from traducteurs");
                int x = 0;
                foreach (var row in db.FetchAll())
                {
                    x = Convert.ToInt32(row["a1"]);
                }

                if (x > 0)
                {
                    x++;
                    db.Query(string.Format("insert into TRADUCTEURS ( ID, NOM, PRENOM ) values ( {0}, '{1}', '{2}' )",
                                             x.ToString(), TB_Nom.Text.Replace("'", "''"), TB_Prenom.Text.Replace("'", "''")));

                    CB_SelTraducteur.Text = TB_Nom.Text + ", " + TB_Prenom.Text;

                    DTrad.Add(CB_SelTraducteur.Text, x.ToString());

                }
            }
            
            TB_Prenom.Text = "";
            TB_Nom.Text = "";
            BT_Select_Click(this, e);
        }


        private void TB_Prenom_TextChanged(object sender, EventArgs e)
        {
            if (TB_Nom.Text != "")
                BT_CreateTrad.Enabled = true;
            else
                BT_CreateTrad.Enabled = false;
        }

        private void TB_Nom_TextChanged(object sender, EventArgs e)
        {
            if (TB_Nom.Text != "")
                BT_CreateTrad.Enabled = true;
            else
                BT_CreateTrad.Enabled = false;
        }        

    }
}

