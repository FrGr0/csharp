using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdDB;

namespace SaisieLivre.Forms
{
    public partial class NewCollectionForm : Form
    {
        private MainForm Main;
        public string ID;
        private string query, Which;
        private int EditFieldColl;
        private string MemoPresentation = string.Empty;
        private string MemoPresent_Titre = string.Empty;
        private string MemoLibAffichage = string.Empty;
        private bool create = false;

        private List<string> Fields_Numeric = new List<string>();
        private List<string> Fields_CheckBox = new List<string>();
        private List<string> Fields_ComboBox = new List<string>();
        
        private Dictionary<string, string> CollSupport = new Dictionary<string, string>();
        private Dictionary<string, string> CSDico = new Dictionary<string, string>();
        private Dictionary<string, string> SeriesID = new Dictionary<string, string>();



        private string Langue0 = string.Empty, Langue1 = string.Empty, MemoAuteurs=string.Empty;
        private bool BActivate = true;
        private bool ForceCreation = false;
        

        public NewCollectionForm(MainForm mf, string CS, string Query)
        {
            Main = mf;
            Which = CS;
            query = Query;
            InitializeComponent();
            
            //on verifie si le libelle étendu editeur existe pour le convertir en libellé conforme à l'annuaire
            if (Main.DEdiFullToEdi.ContainsKey(TB_Editeur.Text))
                TB_Editeur.Text = Main.DEdiFullToEdi[TB_Editeur.Text];


            if (Which == "S")
            {
                this.Width = this.Width + 250;
                LV_Active.Width = LV_Active.Width - 250;

                label1.Left = LV_Active.Width + 20;
                label1.Visible = true;
                LB_Serie_Soeurs.Left = LV_Active.Width + 20;
                LB_Serie_Soeurs.Visible = true;

                label2.Left = LV_Active.Width + 20;
                label2.Visible = true;
                LB_Serie_Enfants.Left = LV_Active.Width + 20;
                LB_Serie_Enfants.Visible = true;

                label3.Left = LV_Active.Width + 20;
                label3.Top = 427;
                label3.Visible = true;                
                LB_Serie_Gencods.Left = LV_Active.Width + 20;
                LB_Serie_Gencods.Top = 443;
                LB_Serie_Gencods.Height = 173;
                LB_Serie_Gencods.Visible = true;
            }
            else
            {
                this.Width = this.Width + 250;
                LV_Active.Width = LV_Active.Width - 250;
                label3.Top = 12;
                label3.Left = LV_Active.Width + 20;
                label3.Visible = true;
                LB_Serie_Gencods.Top = 35;
                LB_Serie_Gencods.Height = 581;
                LB_Serie_Gencods.Left = LV_Active.Width + 20;
                LB_Serie_Gencods.Visible = true;

            }
        }
    

        private void BT_AddGTLSecColl_Click(object sender, EventArgs e)
        {
            int Max = 1;
            foreach (ListViewItem lvi in LV_Active.Items)
            {
                if (lvi.Text.Length < 17 || lvi.Text.Substring(0, 17) != "genre_secondaire_")
                {
                    //rien
                }
                else
                {
                    Max = Convert.ToInt32(lvi.Text.Remove(0, 17)) + 1;
                }
            }
            ListViewItem newlvi = new ListViewItem();
            newlvi.Text = "genre_secondaire_" + Max.ToString();
            newlvi.SubItems.Add("");
            LV_Active.Items.Add(newlvi);
        }


        private void BT_AutFunc_Coll_Click(object sender, EventArgs e)
        {
            string Selected = string.Empty;
            foreach (ListViewItem lvi in LV_Active.Items)
            {
                if (lvi.Text == "auteurs")
                {
                    Selected = lvi.SubItems[1].Text;
                    break;
                }
            }
         
            AuteursForm af = new AuteursForm(Main.DLangue, Main.DPays, Main.DFunctionAuteurs);
            af.Font = Main.LoadedFont;
            af.TB_Auteurs_From_Main.Text = Selected;
            af.ncf = this;
            af.db = Main.db;            
            af.ShowDialog();

            if (create)
                BT_CreateSerie.Enabled = true;
            else
                BT_Save_Active.Enabled = true;
        }

        private void NewCollectionForm_Load(object sender, EventArgs e)
        {
            if (Which == "C")
                this.Text = "Modification de Collection";
            else
                this.Text = "Modification de Série";

            //récupère les codes supports depuis le dico de la mainform
            foreach (var pair in Main.DSupport)
                CollSupport.Add(pair.Value, pair.Key);
            CollSupport.Add("", "");

            foreach (var pair in Main.DOBCode)
                CB_OBCode.Items.Add(pair.Key);            
            CB_OBCode.Items.Add("");

            foreach (var pair in Main.DCL_Genre_0)
                CB_Genre_0.Items.Add(pair.Key);

            foreach (var pair in Main.DCL_Langue_0)
                CB_Langue_0.Items.Add(pair.Key);

            #region répartition des types de champs
            Fields_Numeric.Add("hauteur");
            Fields_Numeric.Add("largeur");
            Fields_Numeric.Add("nbrpages");
            Fields_Numeric.Add("newgenre");
            Fields_Numeric.Add("poids");

            Fields_CheckBox.Add("iad");
            Fields_CheckBox.Add("cartonne");
            Fields_CheckBox.Add("luxe");
            Fields_CheckBox.Add("broche");
            Fields_CheckBox.Add("relie");
            Fields_CheckBox.Add("present_titre");
            Fields_CheckBox.Add("livre_lu");
            Fields_CheckBox.Add("grands_cara");
            Fields_CheckBox.Add("multilingue");
            Fields_CheckBox.Add("illustre");

            Fields_ComboBox.Add("lectorat");
            Fields_ComboBox.Add("codesupport");
            Fields_ComboBox.Add("traducteur");
            Fields_ComboBox.Add("langue_de");
            Fields_ComboBox.Add("langue_en");
            Fields_ComboBox.Add("style");
            Fields_ComboBox.Add("serie_mere");
            #endregion

            LoadCollSerie();        
        }


        private void LoadCollSerie()
        {
            MemoPresentation = "";

            string QSearchCompColl = " and c.collection = '" + TB_Libelle.Text.Replace("'", "''") + "'";
            string QSearchCompSeri = " and s.libelle = '" + TB_Libelle.Text.Replace("'", "''") + "'";


            CSDico.Clear();
            create = false;
            LV_Active.Items.Clear();

            if (Which == "C")
            {
                //Clipboard.SetText(string.Format(query, TB_Editeur.Text, QSearchCompColl));
                Main.db.Query(string.Format(query, TB_Editeur.Text.Replace("'", "''"), QSearchCompColl));
            }
            else
            {
                //Clipboard.SetText(string.Format(query, QSearchCompSeri));
                Main.db.Query(string.Format(query, QSearchCompSeri));
            }

            ID = "";
            bool presence_auteur = false;
            bool found = false;
            BT_Save_Active.Enabled = false;
            BT_CreateSerie.Enabled = true;

            foreach (Row row in Main.db.FetchAll())
            {
                found = true;
                foreach (var pair in row)
                {
                    if (pair.Key == "id")
                    {
                        ID = row["id"].ToString();
                    }

                    else if (pair.Key == "presentation")
                    {
                        MemoPresentation = pair.Value.ToString();
                        string[] showpresent = pair.Value.ToString().Replace("\r", "").Split('\n');
                        string affichage = pair.Value.ToString();
                        if (showpresent.Length > 1)
                            affichage = showpresent[0] + "...";

                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = pair.Key;
                        lvi.SubItems.Add(affichage);
                        LV_Active.Items.Add(lvi);
                        CSDico.Add(pair.Key, pair.Value.ToString());
                    }

                    else if (pair.Key.Length < 5 || pair.Key.Substring(0, 5) != "test_")
                    {
                        string Val = pair.Value.ToString();

                        if (pair.Key == "auteurs")
                            presence_auteur = true;

                        if (pair.Key == "present_titre")
                            MemoPresent_Titre = pair.Value.ToString();

                        if (pair.Key == "libelle_affichage")
                            MemoLibAffichage = pair.Value.ToString();

                        if (pair.Key == "genre_principal")
                        {
                            Val = Main.SetGTL(Val);
                            if (Val == "00-00-00-00")
                                Val = "";
                        }

                        if (pair.Key == "codelangue" && Val == "0")
                            Val = "";

                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = pair.Key;
                        lvi.SubItems.Add(Val);
                        LV_Active.Items.Add(lvi);
                        CSDico.Add(pair.Key, Val);
                    }
                    else
                    {
                        int i = 1;
                        foreach (string s in pair.Value.ToString().Split(';'))
                        {
                            if (s != "")
                            {
                                ListViewItem lvi = new ListViewItem();
                                lvi.Text = "genre_secondaire_" + i.ToString();
                                lvi.SubItems.Add(Main.SetGTL(s));
                                LV_Active.Items.Add(lvi);
                                CSDico.Add(lvi.Text, Main.SetGTL(s));
                            }
                            i++;
                        }
                    }
                }
            }


            if (ID != "" && Convert.ToInt32(ID) > 0)
            {
                BT_Save_Active.Enabled = true;
                BT_CreateSerie.Enabled = false;

                Main.db.Query(string.Format("select libelle from libseries where id_serie_mere={0}", ID));
                foreach (Row row in Main.db.FetchAll())
                    if (row["libelle"].ToString().ToUpper() != TB_Libelle.Text.ToUpper())                        
                        LB_Serie_Enfants.Items.Add(row["libelle"].ToString());

                Main.db.Query(string.Format("select libelle from libseries where id_serie_mere=(select id_serie_mere from libseries where id={0} and id_serie_mere>0)", ID));
                foreach (Row row in Main.db.FetchAll())
                    if ( row["libelle"].ToString().ToUpper() != TB_Libelle.Text.ToUpper())
                        LB_Serie_Soeurs.Items.Add(row["libelle"].ToString());

                if (Which == "S")
                {
                    Main.db.Query(string.Format("select gencod from livre where idlibserie={0}", ID));
                    foreach (Row row in Main.db.FetchAll())
                        LB_Serie_Gencods.Items.Add(row["gencod"].ToString());
                }
                else
                {
                    Main.db.Query(string.Format("select gencod from livre where idcollection={0}", ID));
                    foreach (Row row in Main.db.FetchAll())
                        LB_Serie_Gencods.Items.Add(row["gencod"].ToString());
                }
            }

            if (presence_auteur)
                BT_AutFunc_Coll.Enabled = true;
            else
                BT_AutFunc_Coll.Enabled = false;

            if (ID != "")
            {
                string Type = "coll";
                if (Which != "C")
                    Type = "serie";

                Main.LoadArtFuncID(ID, Type);
            }

            if (Which != "C")
            {
                SeriesID.Clear();
                Main.db.Query("select id, case when(id=0) then '' else libelle end as libelle from libseries order by 2");
                foreach (Row row in Main.db.FetchAll())
                {
                    if (!SeriesID.ContainsKey(row["libelle"].ToString()))
                        SeriesID.Add(row["libelle"].ToString(), row["id"].ToString());
                }
            }

            if (!found)
            {
                create = true;

                if (Which == "C")
                    NewCollection();
                else
                    NewSerie();

                BT_CreateSerie.Enabled = true;
            }    
        }


        private void NewCollection()
        {
            string QuerySearch = Properties.Resources.SelectFromCollectionsForNewCollection.Replace("SELECT", "SELECT FIRST 1");
            QuerySearch = QuerySearch.Remove(QuerySearch.Length - 30, 30);

            Main.db.Query(QuerySearch);

            foreach (Row row in Main.db.FetchAll())
            {
                foreach (var pair in row)
                {
                    if (pair.Key != "test_gtl_second" && pair.Key!="id")
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = pair.Key;
                        switch (pair.Key)
                        {
                            case "editeur":
                                lvi.SubItems.Add(TB_Editeur.Text);
                                break;
                            case "collection":
                                lvi.SubItems.Add(TB_Libelle.Text);
                                break;
                            case "hauteur":
                            case "largeur":
                            case "iad":
                            case "present_titre":
                            case "nbrpage":
                            case "poids":
                            case "luxe":
                            case "relie":
                            case "broche":
                            case "livre_lu":
                            case "grands_cara":
                            case "multilingue":
                            case "illustre":
                            case "cartonne":
                                lvi.SubItems.Add("0");
                                break;
                            case "lectorat":
                            case "style":
                                lvi.SubItems.Add("");
                                break;
                            case "langue_de":
                            case "langue_en":
                                lvi.SubItems.Add("");
                                break;
                            case "traducteur":
                                lvi.SubItems.Add("");
                                break;
                            default:
                                lvi.SubItems.Add("");
                                break;
                        }

                        LV_Active.Items.Add(lvi);
                    }
                }
            }
        }

        private void NewSerie()
        {
            string QuerySearch = Properties.Resources.SelectFromLibseriesForNewCollection.Replace("SELECT", "SELECT FIRST 1");
            QuerySearch = QuerySearch.Remove(QuerySearch.Length - 16, 16);

            //MessageBox.Show(QuerySearch);
            Main.db.Query(QuerySearch);

            foreach (Row row in Main.db.FetchAll())
            {
                foreach (var pair in row)
                {
                    if (pair.Key != "test_gtl_second" && pair.Key!="id")
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = pair.Key;
                        switch (pair.Key)
                        {
                            case "serie":
                                lvi.SubItems.Add(TB_Libelle.Text);
                                break;
                            case "hauteur":
                            case "largeur":
                            case "iad":
                            case "present_titre":
                            case "nbrpage":
                            case "poids":
                            case "luxe":
                            case "relie":
                            case "broche":
                            case "livre_lu":
                            case "grands_cara":
                            case "multilingue":
                            case "illustre":
                                lvi.SubItems.Add("0");
                                break;
                            case "langue_de":
                            case "langue_en":
                                lvi.SubItems.Add("");
                                break;
                            case "traducteur":
                                lvi.SubItems.Add("");
                                break;
                            case "lectorat":
                            case "style":
                                lvi.SubItems.Add("");
                                break;
                            default:
                                lvi.SubItems.Add("");
                                break;
                        }

                        LV_Active.Items.Add(lvi);
                    }
                }
            }
        }

        

        private void LV_ItemActivate(object sender, EventArgs e)
        {
            string Header;
            string Values;
            ColumnHeader c1 = LV_Active.Columns[0];
            ColumnHeader c2 = LV_Active.Columns[1];

            try
            {
                EditFieldColl = LV_Active.SelectedIndices[0];
                Point p = LV_Active.Items[EditFieldColl].Position;

                Header = LV_Active.Items[EditFieldColl].SubItems[0].Text;
                Values = LV_Active.Items[EditFieldColl].SubItems[1].Text;

                if (Fields_ComboBox.Contains(Header))
                {
                    switch (Header)
                    {
                        case "lectorat":
                            SetComboBoxHidden((object)Main.DLectorat, false);
                            break;
                        case "style":
                            SetComboBoxHidden((object)Main.DStyles);
                            break;
                        case "codesupport":
                            SetComboBoxHidden((object)CollSupport);
                            break;
                        case "traducteur":
                            SetComboBoxHidden((object)Main.DTraducteur);
                            break;
                        case "serie_mere":
                            SetComboBoxHidden((object)SeriesID);
                            break;
                        case "langue_de":
                        case "langue_en":
                            SetComboBoxHidden((object)Main.DLangue);
                            break;                                                
                    }


                    CB_Hidden.Text = Values;

                    CB_Hidden.Width = c2.Width;

                    CB_Hidden.Top = LV_Active.Top + p.Y;
                    CB_Hidden.Left = LV_Active.Left + p.X + c1.Width;
                    CB_Hidden.Visible = true;
                    CB_Hidden.BringToFront();
                    CB_Hidden.SelectAll();
                    CB_Hidden.Focus();

                }
                else if (Fields_CheckBox.Contains(Header))
                {
                    if (Values == "1")
                        CLB_Hidden.SetItemChecked(0, true);
                    else
                        CLB_Hidden.SetItemChecked(0, false);


                    CLB_Hidden.Width = c2.Width;

                    CLB_Hidden.Top = LV_Active.Top + p.Y;
                    CLB_Hidden.Left = LV_Active.Left + p.X + c1.Width;
                    CLB_Hidden.Visible = true;
                    CLB_Hidden.BringToFront();
                    CLB_Hidden.Focus();
                }
                else if (Header == "genre_principal" || Header.Length > 17 && Header.Substring(0, 17) == "genre_secondaire_")
                {
                    groupBox1.Text = Header.Replace("_", " ") + " par défaut";

                    string selcode = Values;

                    selcode = selcode.Replace("-", "");

                    //on remet le code sur 8 chiffres
                    while (selcode.Length < 8)
                        selcode = "0" + selcode;
                    
                    TB_Genre_0.Text = Convert.ToInt32(selcode.Substring(0, 2)).ToString();
                    if (selcode.Length >= 3)
                        TB_Genre_1.Text = Convert.ToInt32(selcode.Substring(2, 2)).ToString();

                    if (selcode.Length >= 5)
                        TB_Genre_2.Text = Convert.ToInt32(selcode.Substring(4, 2)).ToString();

                    if (selcode.Length >= 7)
                        TB_Genre_3.Text = Convert.ToInt32(selcode.Substring(6, 2)).ToString();

                    P_Genres.Top = LV_Active.Top + p.Y;
                    P_Genres.Left = LV_Active.Left + p.X + c1.Width;
                    P_Genres.Visible = true;
                    P_Genres.BringToFront();
                    P_Genres.Focus();
                }
                else if (Header == "obcode")
                {
                    TB_OBCode.Text = Values;
                    P_OBCode.Top = LV_Active.Top + p.Y;
                    P_OBCode.Left = LV_Active.Left + p.X + c1.Width;
                    P_OBCode.Visible = true;
                    P_OBCode.BringToFront();
                    P_OBCode.Focus();
                }
                else if (Header == "codelangue")
                {
                    Langue0 = string.Empty;
                    Langue1 = string.Empty;

                    TB_Langue.Text = Values;
                    P_Langue.Top = LV_Active.Top + p.Y;
                    P_Langue.Left = LV_Active.Left + p.X + c1.Width;
                    P_Langue.Visible = true;
                    P_Langue.BringToFront();
                    P_Langue.Focus();
                }
                else
                {
                    TextBox tb = TB_Edit_Hidden_Coll;
                    tb.CharacterCasing = CharacterCasing.Normal;

                    if (Header == "editeur")
                        return;

                    else if (Fields_Numeric.Contains(Header))
                    {
                        tb = TB_Numbers;
                    }
                    
                    else if (Header == "presentation")
                    {
                        tb.Multiline = true;
                        tb.Height = 100;
                        tb.Text = MemoPresentation;
                    }
                    else
                    {
                        if (Header == "auteurs")
                        {
                            tb.CharacterCasing = CharacterCasing.Upper;
                        }

                        tb.Multiline = false;
                        tb.Height = 20;
                        tb.Text = Values;
                    }                        

                    tb.Width = c2.Width;

                    tb.Top = LV_Active.Top + p.Y;
                    tb.Left = LV_Active.Left + p.X + c1.Width;
                    tb.Visible = true;
                    tb.BringToFront();
                    tb.SelectAll();
                    tb.Focus();
                }
            }
            catch { }
        }


        private void SetComboBoxHidden(object From, bool order = true)
        {
            Dictionary<string, string> _From = (Dictionary<string, string>)From;

            CB_Hidden.Items.Clear();
            if (From as Dictionary<string, string> != null)
            {
                if (order)
                {
                    foreach (var pair in _From.OrderBy(i => i.Key))
                        CB_Hidden.Items.Add(pair.Key);
                }
                else
                {
                    foreach (var pair in _From)
                        CB_Hidden.Items.Add(pair.Key);
                    
                    if (!_From.ContainsValue("0"))
                        CB_Hidden.Items.Insert(0, "");
                }
            }
            else
            {
                foreach (string s in (List<string>)From)
                    CB_Hidden.Items.Add(s);
            }
        }

        private void CB_Genre_0_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_Genre_0.Text = Main.DCL_Genre_0[((ComboBox)sender).Text];
        }

        private void TB_Genre_0_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && ((TextBox)sender).Text != "0")
            {
                foreach (var pair in Main.DCL_Genre_0)
                {
                    if (pair.Value == TB_Genre_0.Text)
                    {
                        CB_Genre_0.Text = pair.Key;
                        break;
                    }
                }


                CB_Genre_1.Items.Clear();
                CB_Genre_2.Items.Clear();
                CB_Genre_3.Items.Clear();

                TB_Genre_1.Text = "";
                TB_Genre_2.Text = "";
                TB_Genre_3.Text = "";

                foreach (var pair in Main.DCL_Genre_1)
                {
                    string v = pair.Value;

                    v = v.Substring(0, v.Length - 2);

                    if (v == TB_Genre_0.Text)
                    {
                        CB_Genre_1.Items.Add(pair.Key.Split(';')[1].ToString());
                    }
                }
            }
            else
            {
                CB_Genre_0.Text = "";
                TB_Genre_1.Text = "";
                TB_Genre_2.Text = "";
                TB_Genre_3.Text = "";
            }

        }

        private void CB_Genre_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string LibSelected = ((ComboBox)sender).Text;
            foreach (var pair in Main.DCL_Genre_1)
            {
                if (pair.Value.Substring(0, TB_Genre_0.Text.Length) == TB_Genre_0.Text)
                {
                    if (pair.Key.Split(';')[1] == LibSelected)
                    {
                        LibSelected = pair.Key;
                        break;
                    }
                }
            }

            string code = Main.DCL_Genre_1[LibSelected];
            code = code.Substring(TB_Genre_0.Text.Length, code.Length - TB_Genre_0.Text.Length);
            int skip0 = Convert.ToInt32(code);
            code = skip0.ToString();

            TB_Genre_1.Text = code;
        }

        private void TB_Genre_1_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != "".Trim() && ((TextBox)sender).Text != "0")
            {

                string code0 = TB_Genre_0.Text;
                string code1 = TB_Genre_1.Text;
                while (code1.Length < 2)
                    code1 = "0" + code1;

                foreach (var pair in Main.DCL_Genre_1)
                {
                    if (pair.Value == code0 + code1)
                    {
                        CB_Genre_1.Text = pair.Key.Split(';')[1].ToString();
                        break;
                    }
                }

                CB_Genre_2.Items.Clear();
                CB_Genre_3.Items.Clear();

                TB_Genre_2.Text = "";
                TB_Genre_3.Text = "";

                foreach (var pair in Main.DCL_Genre_2)
                {
                    string v = pair.Value;
                    v = v.Substring(0, v.Length - 2);

                    if (v == code0 + code1)
                        CB_Genre_2.Items.Add(pair.Key.Split(';')[1].ToString());
                }
            }
            else
                CB_Genre_1.Text = "";

        }

        private void CB_Genre_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code0 = TB_Genre_0.Text;
            string code1 = TB_Genre_1.Text;
            while (code1.Length < 2)
                code1 = "0" + code1;
            int topcodelength = (code0.Length + code1.Length);


            string LibSelected = ((ComboBox)sender).Text;
            foreach (var pair in Main.DCL_Genre_2)
            {
                if (pair.Value.Substring(0, topcodelength) == code0 + code1)
                {
                    if (pair.Key.Split(';')[1] == LibSelected)
                    {
                        LibSelected = pair.Key;
                        break;
                    }
                }
            }

            string code = Main.DCL_Genre_2[LibSelected];

            code = code.Substring(topcodelength, code.Length - topcodelength);
            int skip0 = Convert.ToInt32(code);
            code = skip0.ToString();

            TB_Genre_2.Text = code;
        }

        private void TB_Genre_2_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != "".Trim() && ((TextBox)sender).Text != "0")
            {

                string code0 = TB_Genre_0.Text;
                string code1 = TB_Genre_1.Text;
                string code2 = TB_Genre_2.Text;
                while (code1.Length < 2)
                    code1 = "0" + code1;
                while (code2.Length < 2)
                    code2 = "0" + code2;

                foreach (var pair in Main.DCL_Genre_2)
                {
                    if (pair.Value == code0 + code1 + code2)
                    {
                        CB_Genre_2.Text = pair.Key.Split(';')[1];
                        break;
                    }
                }

                CB_Genre_3.Items.Clear();
                TB_Genre_3.Text = "";

                foreach (var pair in Main.DCL_Genre_3)
                {
                    string v = pair.Value;
                    v = v.Substring(0, v.Length - 2);

                    if (v == code0 + code1 + code2)
                        CB_Genre_3.Items.Add(pair.Key.Split(';')[1]);
                }
            }
            else
                CB_Genre_2.Text = "";
        }

        private void CB_Genre_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code0 = TB_Genre_0.Text;
            string code1 = TB_Genre_1.Text;
            while (code1.Length < 2)
                code1 = "0" + code1;
            string code2 = TB_Genre_2.Text;
            while (code2.Length < 2)
                code2 = "0" + code2;

            int topcodelength = (code0.Length + code1.Length + code2.Length);

            string LibSelected = ((ComboBox)sender).Text;
            foreach (var pair in Main.DCL_Genre_3)
            {
                if (pair.Value.Substring(0, topcodelength) == code0 + code1 + code2)
                {
                    if (pair.Key.Split(';')[1] == LibSelected)
                    {
                        LibSelected = pair.Key;
                        break;
                    }
                }
            }

            string code = Main.DCL_Genre_3[LibSelected];

            code = code.Substring(topcodelength, code.Length - topcodelength);
            int skip0 = Convert.ToInt32(code);
            code = skip0.ToString();

            TB_Genre_3.Text = code;
        }

        private void TB_Genre_3_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && ((TextBox)sender).Text != "0")
            {

                string code0 = TB_Genre_0.Text;
                string code1 = TB_Genre_1.Text;
                string code2 = TB_Genre_2.Text;
                string code3 = TB_Genre_3.Text;
                while (code1.Length < 2)
                    code1 = "0" + code1;
                while (code2.Length < 2)
                    code2 = "0" + code2;
                while (code3.Length < 2)
                    code3 = "0" + code3;

                foreach (var pair in Main.DCL_Genre_3)
                {
                    if (pair.Value == code0 + code1 + code2 + code3)
                    {
                        CB_Genre_3.Text = pair.Key.Split(';')[1];
                        break;
                    }
                }
            }
            else
                CB_Genre_3.Text = "";
        }

        private string set2chars(string s)
        {
            string S = s;
            while (S.Length < 2)
                S = "0" + S;
            return S;
        }

        private void P_Genres_Leave(object sender, EventArgs e)
        {
            ValideHiddenText(true);
        }

        private void TB_OBCode_TextChanged(object sender, EventArgs e)
        {
            if (Main.DOBCode.ContainsValue(TB_OBCode.Text))
            {
                foreach (var pair in Main.DOBCode)
                {
                    if (pair.Value == TB_OBCode.Text)
                    {
                        CB_OBCode.Text = pair.Key;
                        break;
                    }
                }
            }
            else
                CB_OBCode.Text = "";
        }

        private void CB_OBCode_TextChanged(object sender, EventArgs e)
        {
            if (Main.DOBCode.ContainsKey(CB_OBCode.Text))
                TB_OBCode.Text = Main.DOBCode[CB_OBCode.Text];
        }

        private void P_OBCode_Leave(object sender, EventArgs e)
        {
            ValideHiddenText(true);
        }


        private void ValideHiddenText(bool checkifempty = false)
        {
            string Header = LV_Active.Items[EditFieldColl].SubItems[0].Text;

            Control c;
            if (Fields_ComboBox.Contains(Header))
                c = CB_Hidden;

            else if (Fields_CheckBox.Contains(Header))
                c = CLB_Hidden;

            else if (Fields_Numeric.Contains(Header))
                c = TB_Numbers;

            else
                c = TB_Edit_Hidden_Coll;

            if (!Fields_CheckBox.Contains(Header))
            {
                if (Header == "genre_principal" || Header.Length > 17 && Header.Substring(0, 17) == "genre_secondaire_")
                {
                    string Newgenre = "";

                    if (TB_Genre_0.Text != "")
                    {
                        Newgenre += TB_Genre_0.Text;

                        if (TB_Genre_1.Text == "")
                            TB_Genre_1.Text = "0";

                        string G1 = set2chars(TB_Genre_1.Text);
                        Newgenre += G1;

                        if (TB_Genre_2.Text == "")
                            TB_Genre_2.Text = "0";

                        string G2 = set2chars(TB_Genre_2.Text);
                        Newgenre += G2;

                        if (TB_Genre_3.Text == "")
                            TB_Genre_3.Text = "0";

                        string G3 = set2chars(TB_Genre_3.Text);
                        Newgenre += G3;
                    }
                    
                    Newgenre = Main.SetGTL(Newgenre);

                    if (Newgenre == "00-00-00-00")
                        Newgenre = "";

                    if (create)
                        BT_CreateSerie.Enabled = true;
                    else
                        BT_Save_Active.Enabled = true;
                        
                    LV_Active.Items[EditFieldColl].SubItems[1].Text = Newgenre;
                    P_Genres.SendToBack();
                    P_Genres.Visible = false;
                }
                else if (Header == "obcode")
                {
                    LV_Active.Items[EditFieldColl].SubItems[1].Text = TB_OBCode.Text;
                    BT_Save_Active.Enabled = true;
                    P_OBCode.SendToBack();
                    P_OBCode.Visible = false;
                }
                else if (Header == "codelangue")
                {
                    if (TB_Langue.Text != "" && TB_Langue.Text != "0")
                    {
                        /* on vire les codes incomplets... */
                        if (TB_Langue.TextLength == 2)
                            TB_Langue.Text = TB_Langue.Text.Substring(0, 1);

                        else if (TB_Langue.TextLength == 4)
                            TB_Langue.Text = TB_Langue.Text.Substring(0, 3);                        
                    }

                    LV_Active.Items[EditFieldColl].SubItems[1].Text = TB_Langue.Text;
                    BT_Save_Active.Enabled = true;
                    P_Langue.SendToBack();
                    P_Langue.Visible = false;
                }

                else
                {
                    if (checkifempty)
                        if (string.IsNullOrEmpty(c.Text) && LV_Active.Items[EditFieldColl].SubItems[1].Text != "")
                        {
                            c.Visible = false;
                            
                            if (create)
                                BT_CreateSerie.Enabled = true;
                            else
                                BT_Save_Active.Enabled = true;

                            return;
                        }
                        

                    string text = c.Text;

                    if (Header == "presentation")
                    {
                        text = Main.utf8_to_iso(text);
                        MemoPresentation = text;

                        if (text.Replace("\r", "").Split('\n').Length > 1)
                        {                            
                            text = text.Replace("\r", "").Split('\n')[0] + "...";
                        }
                    }
                    
                    if (Header == "auteurs" && text != "")                                            
                        BT_AutFunc_Coll.Enabled = true;

                    if (Header == "serie" && text != TB_Libelle.Text)
                    {
                        BT_CreateSerie.Enabled = true;
                        BT_Save_Active.Text = "Modifier";
                    }

                    else if (Header == "serie" && text == TB_Libelle.Text)
                    {
                        if(!create)
                            BT_CreateSerie.Enabled = false;                        
                    }
                    
                    if (!CSDico.ContainsKey(Header) || (!create && CSDico[Header] != text))
                        BT_Save_Active.Enabled = true;

                    LV_Active.Items[EditFieldColl].SubItems[1].Text = text;
                    c.Text = "";
                    c.Visible = false;
                }
            }
            else
            {
                string text = string.Empty;
                CheckedListBox clb = (CheckedListBox)c;

                if (clb.GetItemCheckState(0) == CheckState.Checked)
                    text = "1";
                else
                    text = "0";

                LV_Active.Items[EditFieldColl].SubItems[1].Text = text;

                if (!CSDico.ContainsKey(Header) || (!create && CSDico[Header] != text))
                    BT_Save_Active.Enabled = true;

                c.Visible = false;
            }

        }

        private void BT_Save_Active_Click(object sender, EventArgs e)
        {
            string gtlp = string.Empty;
            List<string> gtls = new List<string>();
            string LibColl = TB_Libelle.Text;
            string NewPresent_Titre = string.Empty;
            string NewLibAffichage = string.Empty;

            string q = "", f = "", v = "";
            foreach (ListViewItem lvi in LV_Active.Items)
            {
                if (lvi.Text == "genre_principal")
                {
                    //requete de suppression dans la table des gtl series principal=1
                    //requete d'insertion dans la table des gtl series principal=1  uniquement si lvi.Subitems[1].Text n'est pas vide;
                    //après la requete de sauvegarde pour récuperer l'id en cas de création
                    gtlp = lvi.SubItems[1].Text.Replace( "-", "" );
                }               
                else if (lvi.Text.Length > 17 && lvi.Text.Substring(0, 17) == "genre_secondaire_")
                {
                    //requete de suppression dans la table des gtl series coalesce( principal, 0)=0;
                    //requete d'insertion dans la table des gtl series principal=0 uniquement si lvi.Subitems[1].Text n'est pas vide;
                    //après la requete de sauvegarde pour récuperer l'id en cas de création
                    gtls.Add(lvi.SubItems[1].Text.Replace("-", ""));
                }
                else if (lvi.Text == "hauteur" || lvi.Text == "largeur" || lvi.Text == "nbrpage")
                {
                    f += lvi.Text + ", ";
                    string code = "null";
                    if (lvi.SubItems[1].Text != "")
                        code = lvi.SubItems[1].Text;
                    v += code + ", ";
                    q += lvi.Text + "=" + code + ", ";
                }
                else if (lvi.Text == "lectorat")
                {
                    string H = "id_lectorat";
                    if (Which != "C")
                        H = "idlectorat";
                    
                    f += H + ", ";
                    string code = "0";
                    if (Main.DLectorat.ContainsKey(lvi.SubItems[1].Text))
                    {
                        code = Main.DLectorat[lvi.SubItems[1].Text];
                    }
                    v += "'" + code + "', ";
                    q += H + "='" + code + "', ";
                }
                else if (lvi.Text == "style")
                {
                    f += "idstyle, ";
                    string code = "0";
                    if (Main.DStyles.ContainsKey(lvi.SubItems[1].Text))
                    {
                        code = Main.DStyles[lvi.SubItems[1].Text];
                    }
                    v += "'" + code + "', ";
                    q += "idstyle ='" + code + "', ";
                }
                else if (lvi.Text == "serie_mere")
                {
                    f += "id_serie_mere, ";
                    string code = "0";
                    if (SeriesID.ContainsKey(lvi.SubItems[1].Text))
                    {
                        code = SeriesID[lvi.SubItems[1].Text];
                    }
                    v += "'" + code + "', ";
                    q += "id_serie_mere ='" + code + "', ";
                }
                else if (lvi.Text == "grands_cara")
                {
                    f += "bigletters, ";
                    v += "'" + lvi.SubItems[1].Text + "', ";
                    q += "bigletters='" + lvi.SubItems[1].Text + "', ";
                }
                else if (lvi.Text == "presentation")
                {
                    f += lvi.Text + ", ";
                    v += "'" + MemoPresentation.Replace("'", "''") + "', ";
                    q += lvi.Text + "='" + MemoPresentation.Replace("'", "''") + "', ";
                }
                else if (lvi.Text == "traducteur")
                {
                    f += "idtraducteur, ";
                    string code = "0";
                    if (Main.DTraducteur.ContainsKey(lvi.SubItems[1].Text))
                    {
                        code = Main.DTraducteur[lvi.SubItems[1].Text];
                    }
                    v += "'" + code + "', ";
                    q += "idtraducteur ='" + code + "', ";
                }
                else if (lvi.Text == "langue_de")
                {
                    f += "idlangue_de, ";
                    string code = "0";
                    if (Main.DLangue.ContainsKey(lvi.SubItems[1].Text))
                    {
                        code = Main.DLangue[lvi.SubItems[1].Text];
                    }
                    v += "'" + code + "', ";
                    q += "idlangue_de ='" + code + "', ";
                }

                else if (lvi.Text == "langue_en")
                {
                    f += "idlangue_en, ";
                    string code = "0";
                    if (Main.DLangue.ContainsKey(lvi.SubItems[1].Text))
                    {
                        code = Main.DLangue[lvi.SubItems[1].Text];
                    }
                    v += "'" + code + "', ";
                    q += "idlangue_en ='" + code + "', ";
                }
                else if (lvi.Text == "codelangue")
                {
                    f += "codelangue, ";
                    string codelangue = "0";
                    if (!string.IsNullOrEmpty(lvi.SubItems[1].Text))
                    {
                        codelangue = lvi.SubItems[1].Text;
                    }
                    v += "'" + codelangue + "', ";
                    q += "codelangue ='" + codelangue + "', ";

                }
                else
                {
                    if (lvi.Text == "collection" || lvi.Text == "serie")
                        LibColl = lvi.SubItems[1].Text;

                    if (lvi.Text == "present_titre")
                        NewPresent_Titre = lvi.SubItems[1].Text;

                    if (lvi.Text == "libelle_affichage")
                        NewLibAffichage = lvi.SubItems[1].Text;

                    string H = lvi.Text;
                    if (H == "serie")
                        H = "libelle";

                    f += H + ", ";
                    v += "'" + lvi.SubItems[1].Text.Replace("'", "''") + "', ";
                    q += H + "='" + lvi.SubItems[1].Text.Replace("'", "''") + "', ";
                }
            }
            if (Which == "C")
            {
                bool MajRefs = false;
                Dictionary<string, Row> D = new Dictionary<string, Row>();
                
                //par défaut on modifie
                create = false;

                //si le libelle collection a changé et que present titre = 1 OU que present_titre a changé, on propose la mise a jour à l'utilisateur
                if ((Main.RemoveDiacritics(TB_Libelle.Text).ToUpper() != Main.RemoveDiacritics(LibColl).ToUpper() && NewPresent_Titre == "1") ||
                    (MemoPresent_Titre != NewPresent_Titre))
                {
                    ID = "";
                    Main.db.Query("SELECT id from Collections where collection='" + TB_Libelle.Text.Replace("'", "''") +
                            "' and editeur='" + TB_Editeur.Text.Replace("'", "''") + "'");

                    foreach (Row row in Main.db.FetchAll())                   
                        ID = row["id"].ToString();


                    if (ID == "")
                        create = true;

                    else if (ID!="" && !ForceCreation)
                    {
                        string Q = "SELECT l.gencod, l.titre, l.codesupport, l.libelle, Coalesce(s.present_titre,0) as pts, coalesce(c.present_titre,0) as ptc, " +
                                    " coalesce( l.serie_coffret, 0) as serie_coffret, " +
                                    " coalesce( l.serie_hs, 0) as serie_hs, coalesce( l.serie_integrale, 0 ) as serie_integrale, " +
                                    " l.noserie, coalesce( l.serie_contenu_1, 0) as serie_contenu_1, coalesce( l.serie_contenu_2, 0) as serie_contenu_2, " +
                                    " cast( trim( case when ( s.libelle_affichage is null ) then strupper( s.libelle ) else strupper( s.libelle_affichage ) end) as varchar(80)) as libserie, " +
                                    " coalesce( l.col_coffret, 0) as coll_coffret, " +
                                    " coalesce( l.col_hs, 0) as coll_hs, l.nocol as nocoll, " +
                                    " coalesce( l.coll_contenu_1, 0) as coll_contenu_1, coalesce( l.coll_contenu_2, 0) as coll_contenu_2, c.collection as libcoll " +
                                    " from collections c join livre l on l.idcollection=c.id left join libseries s on l.idlibserie=s.id " +
                                    "where ( s.present_titre=1 or c.present_titre=1 ) and c.id=" + ID +  " and" +
                                    "( Cast(Coalesce(l.libelle, '')as varchar(250))!='' or ( Cast(Coalesce(l.libelle, '')as varchar(250))='' and "+
                                    "                                                        exists ( select o.gencod from orgcab o where o.gencod=l.gencod ) ) )";

                        Main.db.Query(Q);

                        foreach (Row row in Main.db.FetchAll())
                        {
                            D.Add(row["gencod"].ToString(), row);
                        }

                        if (D.Count > 0)
                        {
                            DialogResult dr = MessageBox.Show("Modifier la collection " + TB_Libelle.Text + " en " + LibColl +
                                                                " ?\r\n\r\nOui: modification du libellé et mise à jour des titres des " + D.Count.ToString() +
                                                                " références rattachées à l'ancienne collection.",
                                                                "Confirmation", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    MajRefs = true;
                                    break;

                                default:
                                    return;
                            }
                        }
                    }
                }
                   

                if (create || ForceCreation)
                {
                    bool createok = true;
                    //on verifie que le nouveau libelle ne correspond pas a une collection existante
                    Main.db.Query(string.Format("select * from collections where strupper(collection)='{0}' and strupper(editeur)='{1}'", LibColl.Replace("'", "''"), TB_Editeur.Text.Replace("'", "''")));
                    foreach (Row row in Main.db.FetchAll())
                    {
                        MessageBox.Show("La collection " + LibColl + " existe déjà chez l'éditeur " + TB_Editeur.Text, "Erreur Collection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        createok = false;
                        break;
                    }

                    if (createok)
                    {
                        f = f.Remove(f.Length - 2, 2);
                        v = v.Remove(v.Length - 2, 2);
                        string Query = "INSERT into COLLECTIONS ( " + f + ", datum ) VALUES ( " + v + ", 'now' )";

                        Main.db.Query(Query);
                        MessageBox.Show(string.Format("la collection {0} a été créée !", LibColl), "Création OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ID = "";
                        Main.db.Query("SELECT id from collections where collection='" + LibColl.Replace("'", "''") + "'");

                        foreach (Row row in Main.db.FetchAll())
                            ID = row["id"].ToString();

                    }
                }

                else
                {
                    string Query = "update collections set " + q + " datum='now' where collection='" + TB_Libelle.Text.Replace("'", "''") + "' and editeur='" + TB_Editeur.Text.Replace("'", "''") + "'";
                    Main.db.Query(Query);

                    if (MajRefs)
                        UpdateRefs(D, TB_Libelle.Text, LibColl, ID, "C");

                    MessageBox.Show(string.Format("la collection {0} a été modifiée !", TB_Libelle.Text), "Modification OK", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                }


                Main.db.Query("delete from collections_gtl where idcoll=" + ID + " and principal=1");
                if (gtlp != "")
                    Main.db.Query("insert into collections_gtl ( idcoll, gtl, principal ) values (" + ID + ", " + gtlp + ", 1 )");

                Main.db.Query("delete from collections_gtl where idcoll=" + ID + " and coalesce( principal, 0 ) =0");
                if (gtls.Count > 0)
                    foreach (string s in gtls)
                        if (s != "")
                            Main.db.Query("insert into collections_gtl ( idcoll, gtl, principal ) values (" + ID + ", " + s + ", 0 )");

                WorkAuteurFonctions(ID, "coll");
                            
            }

            /***************************************************************/
            else //SERIE
            {                
                bool MajRefs = false;
                Dictionary<string, Row> D = new Dictionary<string, Row>();

               
                //par défaut on modifie
                create = false;

                //si le libelle collection a changé et que present titre = 1 OU que present_titre a changé, on propose la mise a jour à l'utilisateur
                if ((Main.RemoveDiacritics(NewLibAffichage).ToUpper() != Main.RemoveDiacritics(MemoLibAffichage).ToUpper() && NewPresent_Titre == "1") ||
                    (MemoPresent_Titre != NewPresent_Titre))
                {
                    ID = "";
                    Main.db.Query("SELECT id from libseries where libelle='" + TB_Libelle.Text.Replace("'", "''") + "'");
                                        
                    foreach (Row row in Main.db.FetchAll())                    
                        ID = row["id"].ToString();

                    if (ID == "")
                        create = true;

                    else if ( ID!="" && !ForceCreation)
                    {
                        string Q = "SELECT l.gencod, l.titre, l.codesupport, l.libelle, Coalesce(s.present_titre,0) as pts, coalesce(c.present_titre,0) as ptc, " +
                                    " coalesce( l.serie_coffret, 0) as serie_coffret, " +
                                    " coalesce( l.serie_hs, 0) as serie_hs, coalesce( l.serie_integrale, 0 ) as serie_integrale, " +
                                    " l.noserie, coalesce( l.serie_contenu_1, 0) as serie_contenu_1, coalesce( l.serie_contenu_2, 0) as serie_contenu_2, " +
                                    " cast( trim( case when ( s.libelle_affichage is null ) then strupper( s.libelle ) else strupper( s.libelle_affichage ) end) as varchar(80)) as libserie, " +
                                    " coalesce( l.col_coffret, 0) as coll_coffret, " +
                                    " coalesce( l.col_hs, 0) as coll_hs, l.nocol as nocoll, " +
                                    " coalesce( l.coll_contenu_1, 0) as coll_contenu_1, coalesce( l.coll_contenu_2, 0) as coll_contenu_2, c.collection as libcoll " +
                                    " from libseries s join livre l on l.idlibserie=s.id left join collections c on l.idcollection=c.id " +
                                    "where ( s.present_titre=1 or c.present_titre=1 ) and s.id=" + ID + " and" +
                                    "( Cast(Coalesce(l.libelle, '')as varchar(250))!='' or ( Cast(Coalesce(l.libelle, '')as varchar(250))='' and "+
                                    "                                                        exists ( select o.gencod from orgcab o where o.gencod=l.gencod ) ) )";

                        Main.db.Query(Q);

                        foreach (Row row in Main.db.FetchAll())
                        {
                            D.Add(row["gencod"].ToString(), row);
                        }

                        if (D.Count > 0)
                        {
                            DialogResult dr = MessageBox.Show("Modifier la série " + TB_Libelle.Text + " en " + LibColl +
                                                                " ?\r\n\r\nOui: modification du libellé et mise à jour des titres des " + D.Count.ToString() +
                                                                " références rattachées à l'ancienne série.",
                                                                "Confirmation", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    MajRefs = true;
                                    break;

                                default:
                                    return;
                            }
                        }
                    }
                }                                  
                
                if (create || ForceCreation)
                {
                    bool createok = true;
                    //on verifie que le nouveau libelle ne correspond pas a une collection existante
                    Main.db.Query(string.Format("select * from libseries where strupper(libelle)=strupper('{0}')", LibColl.Replace("'", "''")));
                    foreach (Row row in Main.db.FetchAll())
                    {
                        MessageBox.Show("La série " + LibColl + " existe déjà dans l'annuaire.", "Erreur Séries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        createok = false;
                        break;
                    }

                    if (createok)
                    {
                        f = f.Remove(f.Length - 2, 2);
                        v = v.Remove(v.Length - 2, 2);
                        string Query = "INSERT into LIBSERIES ( " + f + ", datemaj ) VALUES ( " + v + ", 'now' )";

                        Main.db.Query(Query);
                        MessageBox.Show(string.Format("la série {0} a été créée !", LibColl), "Création OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ID = "";
                        Main.db.Query("SELECT id from libseries where libelle='" + LibColl.Replace("'", "''") + "'");

                        foreach (Row row in Main.db.FetchAll())
                            ID = row["id"].ToString();

                    }
                }

                else
                {
                    Main.db.Query("update libseries set " + q + " datemaj='now' where libelle='" + TB_Libelle.Text.Replace("'", "''") + "'");
                    

                    if (MajRefs)
                        UpdateRefs(D, TB_Libelle.Text, NewLibAffichage, ID, "S");

                    MessageBox.Show(string.Format("la série {0} a été modifiée !", LibColl), "Modification OK", MessageBoxButtons.OK, MessageBoxIcon.Information);                     
                }


                Main.db.Query("delete from libseries_gtl where idserie=" + ID + " and principal=1");
                if (gtlp != "")
                    Main.db.Query("insert into libseries_gtl ( idserie, gtl, principal ) values (" + ID + ", " + gtlp + ", 1 )");

                Main.db.Query("delete from libseries_gtl where idserie=" + ID + " and coalesce( principal, 0 ) =0");
                if (gtls.Count > 0)
                    foreach (string s in gtls)
                        if (s != "")
                            Main.db.Query("insert into libseries_gtl ( idserie, gtl, principal ) values (" + ID + ", " + s + ", 0 )");

                WorkAuteurFonctions(ID, "serie");                        
            }

            if (TB_Libelle.Text != LibColl)            
                TB_Libelle.Text = LibColl;
            

            LV_Active.Items.Clear();

            MemoPresentation = "";

            this.Close();
        }


        private void UpdateRefs(Dictionary<string,Row> D, string OldCol, string NewCol, string IDColl, string Which ="C")
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            foreach (var pair in D)
            {
                string cab = pair.Key;

                string PTC = pair.Value["ptc"].ToString(),
                       PTS = pair.Value["pts"].ToString();

                string OldTitre = pair.Value["titre"].ToString().Replace("\r", "").Replace("\n", ""),
                       Libelle = pair.Value["libelle"].ToString().Replace("\r", "").Replace("\n", ""),
                       LibSerie = pair.Value["libserie"].ToString().Replace("\r", "").Replace("\n", ""),
                       LibColl = pair.Value["libcoll"].ToString().Replace("\r", "").Replace("\n", ""),
                       Noserie = pair.Value["noserie"].ToString(),
                       NoColl = pair.Value["nocoll"].ToString(),
                       CodeSupport = pair.Value["codesupport"].ToString();

                int SerieCoffret = Convert.ToInt32(pair.Value["serie_coffret"]),
                    SerieHS = Convert.ToInt32(pair.Value["serie_hs"]),
                    SerieIntegrale = Convert.ToInt32(pair.Value["serie_integrale"]),
                    Serie_Contenu_1 = Convert.ToInt32(pair.Value["serie_contenu_1"]),
                    Serie_Contenu_2 = Convert.ToInt32(pair.Value["serie_contenu_2"]),
                    CollCoffret = Convert.ToInt32(pair.Value["coll_coffret"]),
                    CollHS = Convert.ToInt32(pair.Value["coll_hs"]),
                    Coll_Contenu_1 = Convert.ToInt32(pair.Value["coll_contenu_1"]),
                    Coll_Contenu_2 = Convert.ToInt32(pair.Value["coll_contenu_2"]);


                if (Which == "C")
                {
                    string NewTitre = RewriteTitreFromSerieColl(PTS, PTC, OldTitre, Libelle, CodeSupport, LibSerie, SerieCoffret, SerieHS, SerieIntegrale, Noserie, Serie_Contenu_1,
                                          Serie_Contenu_2, NewCol, CollCoffret, CollHS, NoColl, Coll_Contenu_1, Coll_Contenu_2, OldCol, "");

                    if (NewTitre!="")
                        Main.db.Query(string.Format("update livre set titre='{0}', collection='{1}', idcollection={2}, datemaj='now', mbexport=1 where gencod='{3}'", NewTitre.Replace("'", "''"), NewCol.Replace("'", "''"), IDColl, cab));
                }
                else
                {
                    string NewTitre = RewriteTitreFromSerieColl(PTS, PTC, OldTitre, Libelle, CodeSupport, NewCol, SerieCoffret, SerieHS, SerieIntegrale, Noserie, Serie_Contenu_1,
                                             Serie_Contenu_2, LibColl, CollCoffret, CollHS, NoColl, Coll_Contenu_1, Coll_Contenu_2, "", OldCol);
                    
                    if (NewTitre!="")
                        Main.db.Query(string.Format("update livre set titre='{0}', idlibserie={1}, datemaj='now', mbexport=1 where gencod='{2}'", NewTitre.Replace("'", "''"), IDColl, cab));
                }
            }
            Cursor = Cursors.Default;
            Application.DoEvents();
        }

        private void WorkAuteurFonctions(string id, string serie_coll = "coll")
        {
            string SC = serie_coll;
            if (serie_coll == "serie")
                SC = "series";

            Main.db.Query(string.Format("delete from AUTEURS_FONCTIONS_{0} where id{1}='{2}'", SC, serie_coll, id));

            for (int i = 0; i < LB_AutFunc.Items.Count; i++)
            {
                string codes = LB_AutFuncComp.Items[i].ToString();
                foreach (string code in codes.Split(','))
                {
                    string _code = code.Trim();

                    if (_code != "")
                        Main.db.Query(string.Format("INSERT INTO AUTEURS_FONCTIONS_{0} values ( '{1}', '{2}', '{3}' )",
                                                  SC,
                                                  id,
                                                  LB_AutFunc.Items[i].ToString().Replace("'", "''"),
                                                  _code));
                }
            }
        }


        private void TB_Edit_Hidden_Coll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ((Control)sender).Text = "";
                ((Control)sender).Visible = false;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if ((Control.ModifierKeys & Keys.Control) == 0)
                {
                    ValideHiddenText();
                }
            }
            else if (e.KeyCode == Keys.F2 && (LV_Active.Items[LV_Active.SelectedIndices[0]].Text == "collection" ||
                                              LV_Active.Items[LV_Active.SelectedIndices[0]].Text == "serie"))
            {                
                TB_Edit_Hidden_Coll.Text = TB_Edit_Hidden_Coll.Text.ToLower();
            }

            else if (e.KeyCode == Keys.F2 && LV_Active.Items[LV_Active.SelectedIndices[0]].Text == "auteurs")
            {
                MemoAuteurs = TB_Edit_Hidden_Coll.Text;
                BT_AutFunc_Coll.PerformClick();
            }
            
            else if (e.KeyCode == Keys.F4 && LV_Active.Items[LV_Active.SelectedIndices[0]].Text == "auteurs")
            {
                MemoAuteurs = TB_Edit_Hidden_Coll.Text;
                LB_Suggest_Auteurs.Top = TB_Edit_Hidden_Coll.Top + TB_Edit_Hidden_Coll.Height;
                LB_Suggest_Auteurs.Left = TB_Edit_Hidden_Coll.Left;
                Main.SuggestAuteur(LB_Suggest_Auteurs, TB_Edit_Hidden_Coll);                
            }
            
        }

        private void TB_Edit_Hidden_Coll_Leave(object sender, EventArgs e)
        {
            ValideHiddenText(true);
        }

        private void BT_Annuler_Active_Click(object sender, EventArgs e)
        {
            this.Close();
        }        

        private void CB_Langue_0_TextChanged(object sender, EventArgs e)
        {
            if (Main.DCL_Langue_0.ContainsKey(CB_Langue_0.Text))
            {
                TB_Langue.Text = Main.DCL_Langue_0[CB_Langue_0.Text];

                CB_Langue_1.Items.Clear();
                Main.DCL_Langue_1.Clear();

                CB_Langue_2.Items.Clear();
                Main.DCL_Langue_2.Clear();

                Main.db.Query(string.Format("select id, libelle from CL_Langue_1 where id starting '{0}'", TB_Langue.Text));
                foreach (Row row in Main.db.FetchAll())
                {
                    CB_Langue_1.Items.Add(row["libelle"].ToString());
                    Main.DCL_Langue_1.Add(row["libelle"].ToString(), row["id"].ToString());
                }
            }
            else
                TB_Langue.Text = "";
        }

        private void CB_Langue_1_TextChanged(object sender, EventArgs e)
        {
            if (Main.DCL_Langue_1.ContainsKey(CB_Langue_1.Text))
            {
                BActivate = false;
                TB_Langue.Text = Main.DCL_Langue_1[CB_Langue_1.Text];
                
                CB_Langue_2.Items.Clear();
                Main.DCL_Langue_2.Clear();

                Main.db.Query(string.Format("select id, libelle from CL_Langue_2 where id starting '{0}'", TB_Langue.Text));
                foreach (Row row in Main.db.FetchAll())
                {
                    CB_Langue_2.Items.Add(row["libelle"].ToString());
                    Main.DCL_Langue_2.Add(row["libelle"].ToString(), row["id"].ToString());
                }
                BActivate = true;
            }
            else
                if (Langue0!="")
                    TB_Langue.Text = Main.DCL_Langue_0[Langue0];
        }

        private void CB_Langue_2_TextChanged(object sender, EventArgs e)
        {

            if (Main.DCL_Langue_2.ContainsKey(CB_Langue_2.Text))
            {
                BActivate = false;
                TB_Langue.Text = Main.DCL_Langue_2[CB_Langue_2.Text];
                BActivate = true;
            }
            else
                TB_Langue.Text = Main.DCL_Langue_1[Langue1];
            
        }
        
        private void TB_Langue_TextChanged(object sender, EventArgs e)
        {
            if (BActivate)
            {

                if (Main.DCL_Langue_0.ContainsValue(TB_Langue.Text) && TB_Langue.TextLength == 1)
                {
                    foreach (var pair in Main.DCL_Langue_0)
                    {
                        if (pair.Value == TB_Langue.Text)
                        {
                            CB_Langue_0.Text = pair.Key;
                            Langue0 = pair.Key;
                            break;
                        }
                    }
                }
                else if (TB_Langue.TextLength == 1)
                    CB_Langue_0.Text = "";

                if (Main.DCL_Langue_1.ContainsValue(TB_Langue.Text) && TB_Langue.TextLength == 3)
                {
                    foreach (var pair in Main.DCL_Langue_1)
                    {
                        if (pair.Value == TB_Langue.Text)
                        {
                            if (Langue0 == "")
                            {
                                foreach (var _pair in Main.DCL_Langue_0)
                                {
                                    if (_pair.Value == TB_Langue.Text.Substring(0, 1))
                                    {
                                        Langue0 = _pair.Key;
                                        break;
                                    }
                                }
                            }

                            CB_Langue_0.Text = Langue0;
                            CB_Langue_1.Text = pair.Key;
                            Langue1 = pair.Key;
                            break;
                        }
                    }
                }
                else if (TB_Langue.TextLength == 3)
                    CB_Langue_1.Text = "";

                if (Main.DCL_Langue_2.ContainsValue(TB_Langue.Text) && TB_Langue.TextLength == 5)
                {
                    foreach (var pair in Main.DCL_Langue_2)
                    {
                        if (pair.Value == TB_Langue.Text)
                        {
                            if (Langue0 == "")
                            {
                                foreach (var _pair in Main.DCL_Langue_0)
                                {
                                    if (_pair.Value == TB_Langue.Text.Substring(0, 1))
                                    {
                                        Langue0 = _pair.Key;
                                        break;
                                    }
                                }
                            }

                            if (Langue1 == "")
                            {
                                foreach (var _pair in Main.DCL_Langue_1)
                                {
                                    if (_pair.Value == TB_Langue.Text.Substring(0, 3))
                                    {
                                        Langue1 = _pair.Key;
                                        break;
                                    }
                                }
                            }

                            CB_Langue_0.Text = Langue0;
                            CB_Langue_1.Text = Langue1;
                            CB_Langue_2.Text = pair.Key;
                            break;
                        }
                    }
                }
                else if (TB_Langue.TextLength == 5)
                    CB_Langue_2.Text = "";
            }
        }

        private void P_Langue_Leave(object sender, EventArgs e)
        {
            ValideHiddenText(true);
        }

        private void BT_CreateSerie_Click(object sender, EventArgs e)
        {
            string quoi = "série";
            if ( Which == "C")
                quoi = "collection";

            DialogResult dr = MessageBox.Show("confirmer la création d'une nouvelle " + quoi + " ?", "confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                BT_Save_Active.Enabled = true;
                ForceCreation = true;
                BT_Save_Active.PerformClick();
                ForceCreation = false;
                BT_Save_Active.Enabled = false;
            }
        }

        private void TB_Edit_Hidden_Coll_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).CharacterCasing == CharacterCasing.Upper)
            {
                int MemoSelectionStart = ((TextBox)sender).SelectionStart;
                ((TextBox)sender).Text = Main.RemoveDiacritics(((TextBox)sender).Text);
                ((TextBox)sender).SelectionStart = MemoSelectionStart;
            }
        }

        private void LB_Suggest_Auteurs_DoubleClick(object sender, EventArgs e)
        {

        }

        private void LB_Suggest_Auteurs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                LB_Suggest_Auteurs.Items.Clear();
                LB_Suggest_Auteurs.Visible = false;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                string NewAut = LB_Suggest_Auteurs.Items[LB_Suggest_Auteurs.SelectedIndex].ToString();

                TB_Edit_Hidden_Coll.Text = MemoAuteurs.Replace(Main.LBSAut, NewAut);

                ValideHiddenText();
                LB_Suggest_Auteurs.Items.Clear();
                LB_Suggest_Auteurs.Visible = false;
                MemoAuteurs = string.Empty;
            }
        }

        private void LB_Suggest_Auteurs_Leave(object sender, EventArgs e)
        {
            LB_Suggest_Auteurs.Items.Clear();
            LB_Suggest_Auteurs.Visible = false;
        }

        #region reecriture du titre

        private string StructColSer(string OldLib, int Coffret, int HS, int Integ, string No, int Contenu1, int Contenu2, string CodeSupp)
        {
            string OldTitreStart = OldLib;
            if (HS > 0)
                OldTitreStart += "HORS-SERIE";
            if (Coffret > 0)
            {
                OldTitreStart += " ; COFFRET";
                if (Integ == 0)
                {
                    if (!string.IsNullOrEmpty(No))
                    {
                        if (Convert.ToInt32(No) > 0)
                        {
                            OldTitreStart += " VOL." + No;

                            if (Contenu1 > 0 && Contenu2 > 0 && Contenu2 > Contenu1)
                                OldTitreStart += " ;";
                        }
                    }
                }
            }

            if (Integ > 0)
            {
                if (Coffret > 0)
                    OldTitreStart += " INTEGRALE";
                else
                    OldTitreStart += " ; INTEGRALE";

                if (!string.IsNullOrEmpty(No))
                {
                    if (Convert.ToInt32(No) > 0)
                    {
                        OldTitreStart += " VOL." + No;

                        if (Contenu1 > 0 && Contenu2 > 0 && Contenu2 > Contenu1)
                            OldTitreStart += " ;";
                    }
                }
            }

            string SerieTomSansPointVirgule = OldTitreStart;

            if ((Coffret > 0 || Integ > 0) &&
                 Contenu1 > 0 && Contenu2 > 0 && Contenu2 > Contenu1)
            {

                string SepTom = " T.";
                if (CodeSupp == "C" || CodeSupp == "R")
                    SepTom = " N.";

                if (Contenu1 > 0)
                {
                    OldTitreStart += OldTitreStart + Contenu1.ToString();

                    if (Contenu2 > 0)
                    {
                        try
                        {
                            if (Contenu2 == Contenu1 + 1)
                            {
                                OldTitreStart += " ET" + SepTom + Contenu2.ToString();
                            }
                            else if (Contenu2 > Contenu1 + 1)
                            {
                                OldTitreStart += " A" + SepTom + Contenu2.ToString();
                            }
                        }
                        catch
                        { }
                    }
                }
            }

            if (Coffret == 0 && Integ == 0)
            {
                string ST = " T.";
                if (CodeSupp == "C" || CodeSupp == "R")
                    ST = " N.";

                if (No != "" && No != "0")
                {
                    OldTitreStart += ST + No;

                    if (Contenu1 > 0)
                    {
                        try
                        {
                            if (Contenu1 == Contenu2 + 1)
                            {
                                OldTitreStart += " ET" + ST + Contenu1.ToString();
                            }
                            else if (Contenu1 > Contenu2 + 1)
                            {
                                OldTitreStart += " A" + ST + Contenu1.ToString();
                            }
                        }
                        catch
                        { }
                    }
                }
            }

            return OldTitreStart;
        }

        private string RewriteTitreFromSerieColl(string PTS,
                                                 string PTC,
                                                 string OldTitre,
                                                 string RowLibelle,
                                                 string CodeSupport,
                                                 string RowLibAffichageSerie, //s.Libelle_Affichage
                                                 int SerieCoffret,
                                                 int SerieHorsSerie,
                                                 int SerieIntegrale,
                                                 string NoSerie,
                                                 int Serie_Contenu_1,
                                                 int Serie_Contenu_2,
                                                 string RowLibAffichageColl,
                                                 int CollCoffret,
                                                 int CollHorsSerie,
                                                 string NoColl,
                                                 int Coll_Contenu_1,
                                                 int Coll_Contenu_2,
                                                 string OldCol, 
                                                 string OldSerie)
        {
            string NewTitre = OldTitre;

            string SerieStr = "", CollStr = "";
                        
            if (PTS == "1")
                SerieStr = SetTitreParamFromPresentTitreSerie(OldTitre, RowLibelle, RowLibAffichageSerie, SerieCoffret, SerieHorsSerie, SerieIntegrale, NoSerie, Serie_Contenu_1, Serie_Contenu_2, CodeSupport);

            if (PTC == "1")
                CollStr = SetTitreParamFromPresentTitreColl(OldTitre, RowLibelle, RowLibAffichageColl, CollCoffret, CollHorsSerie, NoColl, Coll_Contenu_1, Coll_Contenu_2, CodeSupport, true);

            NewTitre = CollStr + SerieStr;

            string NewLibelle = Main.RemoveDiacritics(RowLibelle).ToUpper();

            if (NewLibelle.Length > CollStr.Length)
                if (NewLibelle.Substring(0, CollStr.Length) == CollStr)
                    NewLibelle = NewLibelle.Remove(0, CollStr.Length);

            if (NewLibelle.Length > SerieStr.Length)
                if (NewLibelle.Substring(0, SerieStr.Length) == SerieStr)
                    NewLibelle = NewLibelle.Remove(0, SerieStr.Length);

            if (RowLibelle != "")
                NewTitre += NewLibelle;

            return NewTitre;
        }


        


        //methodes SetTitre
        private string SetTitreParamFromPresentTitreSerie(string OldTitre,
                                                          string RowLibelle,
                                                          string RowLibAffichageSerie, //s.Libelle_Affichage
                                                          int SerieCoffret,
                                                          int SerieHorsSerie,
                                                          int SerieIntegrale,
                                                          string NoSerie,
                                                          int Serie_Contenu_1,
                                                          int Serie_Contenu_2,
                                                          string CodeSupport)
        {
            
            string NewTitre = OldTitre;

            string _Titre = Main.RemoveDiacritics(RowLibelle).ToUpper();

            string SerieTom = Main.RemoveDiacritics(RowLibAffichageSerie).ToUpper();

            NewTitre = StructColSer(SerieTom, SerieCoffret, SerieHorsSerie, SerieIntegrale, NoSerie, Serie_Contenu_1, Serie_Contenu_2, CodeSupport);                        

            if (! string.IsNullOrEmpty(_Titre))
                NewTitre = NewTitre + " ; ";

            return NewTitre;
        }


        private string SetTitreParamFromPresentTitreColl(string OldTitre,
                                                         string RowLibelle,
                                                         string RowLibAffichageColl, 
                                                         int CollCoffret,
                                                         int CollHorsSerie,
                                                         string NoColl,
                                                         int Coll_Contenu_1,
                                                         int Coll_Contenu_2,
                                                         string CodeSupport,
                                                         bool GetPreviousTitre = false)
        {

            string NewTitre = OldTitre;

            string _Titre = Main.RemoveDiacritics(RowLibelle).ToUpper(); //TB_Titre.Text;
            /*if (GetPreviousTitre)
                _Titre = OldTitre;*/

            //on reconstruit l'info COLLECTION T.Tomaison ;
            string CollTom = Main.RemoveDiacritics(RowLibAffichageColl).ToUpper();

            NewTitre = StructColSer(CollTom, CollCoffret, CollHorsSerie, 0, NoColl, Coll_Contenu_1, Coll_Contenu_2, CodeSupport);            

            NewTitre = CollTom;

            if (! string.IsNullOrEmpty(_Titre))
                NewTitre = NewTitre + " ; ";

            return NewTitre;
        }
        #endregion


        private void LB_Serie_Associees_DoubleClick(object sender, EventArgs e)
        {
            TB_Libelle.Text = ((ListBox)sender).Items[((ListBox)sender).SelectedIndex].ToString();

            LB_Serie_Soeurs.Items.Clear();
            LB_Serie_Enfants.Items.Clear();
            LB_Serie_Gencods.Items.Clear();

            //on remet la grille à zero
            if (Which == "C")
                NewCollection();
            else
                NewSerie();

            LoadCollSerie();
        }

        private void LB_Serie_Gencods_DoubleClick(object sender, EventArgs e)
        {
            try
            {                
                Clipboard.SetText(LB_Serie_Gencods.Items[LB_Serie_Gencods.SelectedIndex].ToString());
            }
            catch
            {}
        }

        private void CB_Hidden_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                CB_Hidden.Text = CB_Hidden.Items[CB_Hidden.SelectedIndex].ToString();
                ValideHiddenText(true);
                CB_Hidden.DroppedDown = false;
            }
            catch { }
        }

        private void LB_Serie_Gencods_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && LB_Serie_Gencods.SelectedIndices.Count >= 1)
            {
                if (LB_Serie_Gencods.SelectedIndices.Count > 1)
                    chargerLaFicheToolStripMenuItem.Enabled = false;
                else
                    chargerLaFicheToolStripMenuItem.Enabled = true;

                contextMenuStrip1.Show(this, new Point((LB_Serie_Gencods.Left + e.X), (LB_Serie_Gencods.Top+e.Y)));
            }
        }

        private void copierLesEANDansLePressePapierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string res = "";
            foreach (int i in LB_Serie_Gencods.SelectedIndices)
            {
                res += LB_Serie_Gencods.Items[i].ToString() + "\r\n";
            }
            Clipboard.SetText(res);
        }

        private void ajouterLesEANÀLaListeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (int i in LB_Serie_Gencods.SelectedIndices)
            {
                Main.LB_Saisie_EAN.Items.Add(LB_Serie_Gencods.Items[i].ToString());
            }                        
        }

        private void chargerLaFicheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main.TB_EAN.Text = LB_Serie_Gencods.Items[LB_Serie_Gencods.SelectedIndex].ToString();
            Main.LoadFiche();
            this.Close();
        }

        private void LB_Serie_Gencods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                for (int i = 0; i < LB_Serie_Gencods.Items.Count; i++)
                {
                    LB_Serie_Gencods.SelectedIndices.Add(i);
                }
                LB_Serie_Gencods.Select();
            }

            if (e.KeyCode == Keys.C && e.Control)
                copierLesEANDansLePressePapierToolStripMenuItem_Click(this, null);

            if (e.KeyCode == Keys.O && e.Control)
            {
                if (LB_Serie_Gencods.SelectedIndices.Count == 1)
                    chargerLaFicheToolStripMenuItem_Click(this, null);
            }
        }
    }        
}
