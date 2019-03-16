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
    public partial class GenListeForm : Form
    {
        private ListBox listBox;
        private FDB db;
        private Dictionary<string, string> Supp = new Dictionary<string,string>();

        public GenListeForm(ListBox LB, FDB fdb, Dictionary<string, Dictionary<int, string>> D, Dictionary<string, string> S)
        {
            InitializeComponent();
            
            listBox = LB;
            db = fdb;
            Supp = S;

            var source = new AutoCompleteStringCollection();
            var source2 = new AutoCompleteStringCollection();

            foreach (var pair in D)
            {
                comboBox1.Items.Add(pair.Key);
                source.Add(pair.Key);

                comboBox2.Items.Add(pair.Value[0]);
                source2.Add(pair.Value[0]);
            }
            comboBox1.AutoCompleteCustomSource = source;
            comboBox2.AutoCompleteCustomSource = source2;

            foreach (var pair in S)
            {
                bool check = false;
                if (pair.Value == "LA" || pair.Value == "A")
                    check = true;
                
                checkedListBox1.Items.Add(pair.Key, check);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string param = string.Empty;
            if (checkBox1.Checked && comboBox1.Text != "")
                param += " editeur='"+ comboBox1.Text.Replace("'", "''") +"' AND";

            if (checkBox4.Checked && comboBox2.Text != "")
                param += " distributeur='" + comboBox2.Text.Replace("'", "''") + "' AND";

            if (checkBox2.Checked)
            {
                string field = string.Empty;
                if (radioButton1.Checked)
                    field = " datemaj";
                else
                    field = " dateparution";

                param += field + " between '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "' and '" + dateTimePicker2.Value.ToString("MM/dd/yyyy") + "' AND";
            } 

            if (checkBox3.Checked)
            {
                string Not = "";

                if (RB_exclure.Checked)
                    Not = "NOT";

                string codes = string.Empty;
                foreach (string s in textBox1.Text.Split(';'))
                {
                    if (s.Trim()!="" )
                        codes += "'" + s.Trim() + "',";
                }

                param += " codesupport "+ Not +" in ( "+ codes.Substring(0, codes.Length-1) +" ) AND";
            }


            if (CBX_aparaitre.Checked && CBX_dispo.Checked)
            {
                param += " dispo in ( 1, 2 ) AND";
            }
            else if (CBX_aparaitre.Checked)
            {
                param += " dispo in ( 2 ) AND";
            }
            else if (CBX_dispo.Checked)
            {
                param += " dispo in ( 1 ) AND";
            }



            /*if (checkBox4.Checked)
            {
                 param+= " dispo in ( 1,2 ) AND";                
            }*/

            //vire le " AND" final
            param = param.Substring(0, param.Length - 4);

            string QGenListe = string.Format(Properties.Resources.QueryGenListe, param);            

            db.Query(QGenListe);

            foreach (var Row in db.FetchAll())
            {
                listBox.Items.Add(Row["gencod"].ToString());
            }

            Cursor = Cursors.Arrow;

            this.Close();           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
                comboBox1.Text = "";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                groupBox1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Parse("01/01/2070");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                textBox1.Enabled = true;
                groupBox3.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                groupBox3.Enabled = false;
            }
        }

        private void CBX_tout_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                CBX_dispo.Checked = false;
                CBX_aparaitre.Checked = false;
                CBX_dispo.Enabled = false;
                CBX_aparaitre.Enabled = false;                
            }
            else
            {
                CBX_dispo.Enabled = true;
                CBX_aparaitre.Enabled = true;
                CBX_dispo.Checked = true;
                CBX_aparaitre.Checked = true;
            }
        }

        private void CBX_dispo_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked && !CBX_aparaitre.Checked)
            {
                CBX_tout.Checked = true;
                CBX_dispo.Enabled = false;
                CBX_aparaitre.Enabled = false;
            }
        }

        private void CBX_aparaitre_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked && !CBX_dispo.Checked)
            {
                CBX_tout.Checked = true;
                CBX_dispo.Enabled = false;
                CBX_aparaitre.Enabled = false;
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            textBox1.Text = "";            
            
            string fullcodes = "";

            try
            {
                string Checked = checkedListBox1.Items[e.Index].ToString();       
                string code = Supp[Checked];

                for (int i=0;i<checkedListBox1.Items.Count;i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                        fullcodes += Supp[checkedListBox1.Items[i].ToString()] + " ; ";
                }

                if (!fullcodes.Contains(code) && e.NewValue == CheckState.Checked )
                    fullcodes += code;

                else if (e.NewValue == CheckState.Unchecked)
                    fullcodes = fullcodes.Replace(code, "");

                textBox1.Text = fullcodes;
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i=0;i<checkedListBox1.Items.Count;i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                comboBox2.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
                comboBox2.Text = "";
            }

        }

        
    }
}
