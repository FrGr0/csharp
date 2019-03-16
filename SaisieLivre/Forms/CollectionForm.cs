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
    public partial class CollectionForm : Form
    {
        private Dictionary<string, string> DCL_Genre_0 = new Dictionary<string,string>(),
                                           DCL_Genre_1 = new Dictionary<string, string>(),
                                           DCL_Genre_2 = new Dictionary<string, string>(),
                                           DCL_Genre_3 = new Dictionary<string, string>(), 
                                           DSupports = new Dictionary<string,string>(),
                                           DLectorat = new Dictionary<string,string>(),
                                           DOBCode = new Dictionary<string,string>(),
                                           DEdiFullToEdi = new Dictionary<string,string>();
        
        private string StrEditeur = string.Empty, 
                       StrCollection = string.Empty,
                       CollDefaut = string.Empty;

        private MainForm mf;
        private FDB db;
        private bool Creation = true;

        public string codeLectorat, IDColl;

        public CollectionForm( MainForm sender,
                               FDB DB,
                               string Editeur,
                               Dictionary<string, string> Genre_0,
                               Dictionary<string, string> Genre_1,
                               Dictionary<string, string> Genre_2,
                               Dictionary<string, string> Genre_3,
                               Dictionary<string, string> Supports,
                               Dictionary<string, string> Lectorat,
                               Dictionary<string, string> OBCode,
                               Dictionary<string, string> EdiFull,
                               string CollName = ""
                               )
        {

            mf = sender;
            DCL_Genre_0 = Genre_0;
            DCL_Genre_1 = Genre_1;
            DCL_Genre_2 = Genre_2;
            DCL_Genre_3 = Genre_3;
            DSupports = Supports;
            DLectorat = Lectorat;                     
            DEdiFullToEdi = EdiFull;
            StrEditeur = Editeur;

            if (DEdiFullToEdi.Count > 0)
            {
                if (DEdiFullToEdi.ContainsKey(StrEditeur))
                    StrEditeur = DEdiFullToEdi[StrEditeur];
            }

            CollDefaut = CollName;
            StrCollection = mf.RemoveDiacritics( CollName ).ToUpper();
            DOBCode = OBCode;
            db = DB;

            InitializeComponent();

            this.Icon = global::SaisieLivre.Properties.Resources._39809;        
        }


        private void CB_Genre_0_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_Genre_0.Text = DCL_Genre_0[((ComboBox)sender).Text];
        }

        private void TB_Genre_0_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && ((TextBox)sender).Text != "0")
            {
                foreach (var pair in DCL_Genre_0)
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

                foreach (var pair in DCL_Genre_1)
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
            foreach (var pair in DCL_Genre_1)
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

            string code = DCL_Genre_1[LibSelected];
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

                foreach (var pair in DCL_Genre_1)
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

                foreach (var pair in DCL_Genre_2)
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
            foreach (var pair in DCL_Genre_2)
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

            string code = DCL_Genre_2[LibSelected];

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

                foreach (var pair in DCL_Genre_2)
                {
                    if (pair.Value == code0 + code1 + code2)
                    {
                        CB_Genre_2.Text = pair.Key.Split(';')[1];
                        break;
                    }
                }

                CB_Genre_3.Items.Clear();
                TB_Genre_3.Text = "";

                foreach (var pair in DCL_Genre_3)
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
            foreach (var pair in DCL_Genre_3)
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

            string code = DCL_Genre_3[LibSelected];

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

                foreach (var pair in DCL_Genre_3)
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

        private void TB_OBCode_TextChanged(object sender, EventArgs e)
        {
            mf.FindIdByLib(DOBCode, TB_OBCode.Text, CB_OBCode);
        }

        private void CB_OBCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DOBCode.ContainsKey(CB_OBCode.Text))
                TB_OBCode.Text = DOBCode[CB_OBCode.Text];
        }


        private void CollectionForm_Load(object sender, EventArgs e)
        {
            TB_CollName.Text = StrCollection;
            TB_Editeur.Text = StrEditeur;

            foreach (var pair in DSupports)
            {
                CB_CodeSupport.Items.Add(pair.Value);
            }

            foreach (var pair in DLectorat)
            {
                CB_Lectorat.Items.Add(pair.Key);
            }

            foreach (var pair in DCL_Genre_0)
            {
                CB_Genre_0.Items.Add(pair.Key);
            }

            foreach (var pair in DCL_Genre_1)
            {
                CB_Genre_1.Items.Add(pair.Key);
            }
            
            foreach (var pair in DCL_Genre_2)
            {
                CB_Genre_2.Items.Add(pair.Key);
            }
            
            foreach (var pair in DCL_Genre_3)
            {
                CB_Genre_3.Items.Add(pair.Key);
            }

            foreach (var pair in DOBCode)
            {
                CB_OBCode.Items.Add(pair.Key);
            }
            
            LoadCollection();
        }


        private void LoadFromRow(Row row)
        {
            if (row["newgenre"].ToString() != "")
            {
                string[] STRCode = mf.ParseCodeGenre(row["newgenre"].ToString()).Split('-');

                TB_Genre_0.Text = Convert.ToInt32(STRCode[0]).ToString();
                TB_Genre_1.Text = Convert.ToInt32(STRCode[1]).ToString();
                TB_Genre_2.Text = Convert.ToInt32(STRCode[2]).ToString();
                TB_Genre_3.Text = Convert.ToInt32(STRCode[3]).ToString();
            }

            if (row["auteurs"].ToString() != "")
                TB_Auteurs.Text = row["auteurs"].ToString();

            if (row["codesupport"].ToString() != "")
                CB_CodeSupport.Text = row["codesupport"].ToString();

            codeLectorat = "0";            
            if (row["id_lectorat"].ToString() != "")
                codeLectorat = row["id_lectorat"].ToString();

            CB_Lectorat.Text = mf.FindIdByLib(DLectorat, codeLectorat); //DLectorat[codeLectorat];

            string Hauteur = "0";
            if (row["hauteur"].ToString() != "")
                Hauteur = row["hauteur"].ToString();

            TB_Hauteur.Text = Hauteur;

            string Largeur = "0";
            if (row["largeur"].ToString() != "")
                Largeur = row["largeur"].ToString();

            TB_Largeur.Text = Largeur;

            string NBPages = "0";
            if (row["nbrpage"].ToString() != "")
                NBPages = row["nbrpage"].ToString();

            TB_NBPages.Text = NBPages;

            if (row["present_titre"].ToString() == "1")
                CBX_PresentTitre.Checked = true;

            TB_Web.Text = row["website"].ToString();
            TB_OBCode.Text = row["obcode"].ToString();
            TB_Presentation.Text = row["presentation"].ToString();

        }

        private string set2chars(string s)
        {
            string S=s;
            while (S.Length < 2)
                S = "0" + S;
            return S;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                       
            string Query;

            string Hauteur = "null", NBPages = "null", Largeur="null";
            
            if (!string.IsNullOrEmpty(TB_Hauteur.Text))
                Hauteur = TB_Hauteur.Text;
            
            if (!string.IsNullOrEmpty(TB_Largeur.Text))
                Largeur = TB_Largeur.Text;

            if (!string.IsNullOrEmpty(TB_NBPages.Text))
                NBPages = TB_NBPages.Text;

            string Present_titre = "0";
            if ( CBX_PresentTitre.Checked)
                Present_titre = "1";

            string Newgenre = "";
            if (TB_Genre_0.Text!="")
            {
                Newgenre+=TB_Genre_0.Text;
            
                if (TB_Genre_1.Text=="")
                    TB_Genre_1.Text = "0";
            
                string G1 = set2chars(TB_Genre_1.Text);
                Newgenre+=G1;

                if (TB_Genre_2.Text=="")
                    TB_Genre_2.Text = "0";
                
                string G2 = set2chars(TB_Genre_2.Text);
                Newgenre+=G2;

                if (TB_Genre_3.Text=="")
                    TB_Genre_3.Text = "0";
                
                string G3 = set2chars(TB_Genre_3.Text);
                Newgenre+=G3;
            }

            //on attend un entier...
            if (Newgenre == "")
                Newgenre = "null";

            
            if ((StrCollection != TB_CollName.Text) && 
                (!string.IsNullOrEmpty(StrCollection)))
            {
                DialogResult dr = MessageBox.Show("Modifier la collection " + StrCollection + " en " + TB_CollName.Text +
                                                   " ?\r\n\r\nOui: modification du libellé et mise à jour de toutes les références rattachées à l'ancienne collection.\r\n\r\nNon: création d'une nouvelle collection.",
                                                   "Confirmation", MessageBoxButtons.YesNoCancel);

                switch (dr) 
                {
                    case DialogResult.Yes: 
                        Creation = false;
                        break;
                    
                    case DialogResult.No:
                        Creation = true;
                        break;
                    
                    default:
                        return;                        
                }                    
            }

            string CN = StrCollection;
            if (string.IsNullOrEmpty(CN) || Creation)
                CN = TB_CollName.Text;

            string Qry = string.Format("select * from collections where trim(editeur)='{1}' and trim(collection)='{0}'", CN.Replace("'", "''"), TB_Editeur.Text.Replace("'", "''"));
            try
            {
                db.Query(Qry);
                foreach (Row row in db.FetchAll())
                {
                    IDColl = row["id"].ToString();
                    Creation = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


            //lectorat
            string IDLectorat = "null";
            if (DLectorat.ContainsKey(CB_Lectorat.Text) && CB_Lectorat.Text!="")                           
                IDLectorat = "'" + DLectorat[CB_Lectorat.Text] + "'";
                        

            if (Creation)
            {
                //on n'ajoute pas le champ ID, il est généré par trigger.

                Query = "INSERT INTO COLLECTIONS                     "+
                        "( COLLECTION, EDITEUR, CODESUPPORT,         "+
                        "  NEWGENRE, HAUTEUR, LARGEUR, AUTEURS,      "+
                        "   PRESENTATION, NBRPAGE, WEBSITE, OBCODE,  "+
                        "   PRESENT_TITRE, ID_LECTORAT, DATUM,       "+
                        "   CODECOLLECTION     )                     "+
                        "VALUES (                                    "+
                        "  '{10}', '{11}', '{0}',                    "+
                        "  {1}, {2}, {3}, '{4}',                     "+ //param 1 : pas de guillemets, c'est volontaire
                        "  '{5}', {6}, '{7}', '{8}',                 "+
                        "  '{9}', {12}, 'now',                       "+ //param 12 : pas de guillemets, c'est volontaire
                        "  Gen_Id(collection_num,1) ) -- {13}";         //param 13 en commentaire
            }
            else
            {
               Cursor = Cursors.WaitCursor;

               Query = "UPDATE COLLECTIONS          " +
                       "SET                         " +
                       "    Datum='now',            " +
                       "    CODESUPPORT='{0}',      " +
                       "    NEWGENRE={1},           " +
                       "    HAUTEUR={2},            " +
                       "    LARGEUR={3},            " +
                       "    AUTEURS='{4}',          " +
                       "    PRESENTATION='{5}',     " +
                       "    NBRPAGE={6},            " +
                       "    WEBSITE='{7}',          " +
                       "    OBCODE='{8}',           " +
                       "    PRESENT_TITRE='{9}',    " +
                       "    ID_LECTORAT= {12},      " +      //param 12 : pas de guillemets, c'est volontaire
                       "    COLLECTION='{10}'       " +  
                       "WHERE                       " +
                       "    cast(trim(collection) as varchar(160)) = '{13}' AND " +
                       "    EDITEUR    = '{11}'";
            } 
                
            db.Query(string.Format(Query,
                                   CB_CodeSupport.Text,
                                   Newgenre,
                                   Hauteur,
                                   Largeur,
                                   TB_Auteurs.Text.Replace("'", "''"),
                                   TB_Presentation.Text.Replace("'", "''"),
                                   NBPages,
                                   TB_Web.Text,
                                   TB_OBCode.Text,
                                   Present_titre,
                                   TB_CollName.Text.Replace("'", "''"),
                                   TB_Editeur.Text.Replace("'", "''"),
                                   IDLectorat,
                                   StrCollection.Replace("'", "''")));

            if (Creation)
            {
                try
                {
                    db.Query(Qry);
                    foreach (Row row in db.FetchAll())
                    {
                        IDColl = row["id"].ToString();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

            else
            {
                //maj des réfs liées à la collection uniquement si le libellé collection change.
                if (StrCollection != TB_CollName.Text)
                {
                    UpdateRefs();

                    //mise à jour du combobox
                    int oldidx = mf.CB_Collection.Items.IndexOf(CollDefaut);
                    mf.CB_Collection.Items.RemoveAt(oldidx);
                    mf.CB_Collection.Items.Insert(oldidx, TB_CollName.Text);
                    mf.TB_IDCollection.Text = IDColl;
                }
            }

            Cursor = Cursors.Default;
    }

        private void UpdateRefs()
        {
            List<string> EAN = new List<string>();
            db.Query(string.Format("SELECT gencod from livre where editeur='{0}' and collection='{1}'", TB_Editeur.Text.Replace("'", "''"), StrCollection.Replace("'", "''")));
            foreach (Row row in db.FetchAll())
            {
                EAN.Add(row["gencod"].ToString());
            }

            foreach (string cab in EAN)
            { 
                db.Query( string.Format( "update livre set collection='{0}', idcollection={1}, datemaj='now', mbexport=1 where gencod='{2}'",
                                         TB_CollName.Text.Replace("'", "''"), IDColl, cab));
            }

        }

        
        private void TB_CollName_Leave(object sender, EventArgs e)
        {
            LoadCollection();   
        }

        private void LoadCollection()
        {
            db.Query(string.Format("SELECT * from Collections where COLLECTION='{0}' and editeur='{1}'", TB_CollName.Text.Replace("'", "''"), TB_Editeur.Text.Replace("'", "''")));
            foreach (var row in db.FetchAll())
            {
                LoadFromRow(row);
                Creation = false;
            }
        }

        private void CB_Lectorat_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeLectorat = DLectorat[CB_Lectorat.Text];
        }
       
    }
}
