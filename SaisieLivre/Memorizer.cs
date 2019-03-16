using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memorizer
{
    public class Memorizer
    {
        private Dictionary<Control, List<string>> DMemo = new Dictionary<Control, List<string>>();
        private Control PreviousControl = null;
        private int PosInList = 0;

        public void Set(Control c)
        {
            if (c.Text != "")
            {
                if (!DMemo.ContainsKey(c))
                {
                    List<string> LS = new List<string>();
                    LS.Add(c.Text);
                    DMemo.Add(c, LS);
                }
                else
                {
                    //entrée précédente : 
                    string Previous = DMemo[c][0].ToString();
                    
                
                    if (c.Text != Previous)
                    {
                        DMemo[c].Insert(0, c.Text);

                        string strfull = string.Empty;
                        foreach (string item in DMemo[c])
                        {
                            strfull+= item + "\r\n";
                        }
                        
                        //MessageBox.Show("insert " + c.Name + "\r\n" + strfull);
                        
                        //purge au dela de 2000 entrées par control dans le mémo, pour ne pas saturer la ram
                        if (DMemo[c].Count > 2000)
                            DMemo[c].RemoveAt(2001);
                    }
                }
            }
        }

        public void Get(Control c)
        {
            if (DMemo.ContainsKey(c))
            {
                if (PreviousControl ==  c)
                {                                                              
                    PosInList++;
                    //MessageBox.Show("Remonte le tableau --> PosInList : " + PosInList.ToString());
                    c.Text = DMemo[c][PosInList].ToString();
                   
                }  
                else
                {                    
                    PosInList = 0;
                    //MessageBox.Show("Nouveau controle : " + PosInList.ToString());
                    
                    try 
                    { 
                        c.Text = DMemo[c][PosInList].ToString(); 
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                              
                PreviousControl = c;
                //MessageBox.Show(PreviousControl.Name);
                
                //on a fait le tour du dictionnaire, donc on repart à zero

                if (PosInList == DMemo[c].Count - 1)
                {
                    PreviousControl = null;
                }
            }
        }

        //à la validation, pas sur que ça serve...
        public void SetAll(Form f)
        {
            foreach (Control c in GetControls(f))
            {
                Set(c);
            }
            PreviousControl = null;
        }

        private IEnumerable<Control> GetControls(Control form)
        {
            foreach (Control childControl in form.Controls)
            {   
                foreach (Control grandChild in GetControls(childControl))
                {
                    yield return grandChild;
                }
                yield return childControl;
            }
        }
    }
}
