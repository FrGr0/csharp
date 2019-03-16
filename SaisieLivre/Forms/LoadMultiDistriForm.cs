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
    public partial class LoadMultiDistriForm : Form
    {
        private Dictionary<int,string> D;
        private TextBox T;
        private TextBox T2;

        public LoadMultiDistriForm(Dictionary<int,string> Dist, TextBox TB, TextBox TBID)
        {
            InitializeComponent();
            D = Dist;
            T = TB;
            T2 = TBID;
            foreach (var pair in D)
            {
                if (pair.Key != 0)
                    LB_MultiDistri.Items.Add(pair.Value);
                else
                    TB_DistriPrincipal.Text = pair.Value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            T.Text = LB_MultiDistri.Items[LB_MultiDistri.SelectedIndex].ToString();
            
            foreach(var pair in D)
            {
                if( pair.Value == T.Text )
                {
                    T2.Text = pair.Key.ToString();
                    break;
                }
            }
            
            this.Close();
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = ((ListBox)sender).SelectedIndex;
            if (idx >= 0)
                BT_SelectMultiDistri.Enabled = true;
            else
                BT_SelectMultiDistri.Enabled = false;
        }


        private void LB_MultiDistri_DoubleClick(object sender, EventArgs e)
        {
            int idx = ((ListBox)sender).SelectedIndex;
            if (idx >= 0)
                button1_Click(sender, e);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            T.Text = TB_DistriPrincipal.Text;
            T2.Text = "0";
            this.Close();
        }

    }
}
