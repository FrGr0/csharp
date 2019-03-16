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
    public partial class DicoAuteurForm : Form
    {
        private FDB db;
        private Dictionary<string, string> DLangue = new Dictionary<string, string>();
        private Dictionary<string, string> DPays = new Dictionary<string, string>();
        public Dictionary<string, string> BestInfo = new Dictionary<string, string>();
        private bool Creation = false;
        private string AuteurParam;
        int id_auteur = -1;
        int idOrigine = 0;
        private List<string> LGencods = new List<string>();

        public DicoAuteurForm(bool Create, 
                              string Auteur, 
                              FDB fdb,
                              Dictionary<string, string>DL, 
                              Dictionary<string, string> DP,
                              string CreateNom, 
                              string CreatePrenom)
        {
            
            InitializeComponent();

            db = fdb;

            AuteurParam = Auteur;
            DLangue = DL;
            DPays = DP;
            Creation = Create;

            string Origine = Environment.UserName;
            idOrigine = GetIdOrigine(Origine);

            if (idOrigine == 0)
            {
                db.Query("INSERT INTO SESSIONS ( session ) values ( '" + Origine + "' )");
                idOrigine = GetIdOrigine(Origine);
            }

            pictureBox1.Image = null;

            foreach (var pair in DLangue)
            {
                CB_langue.Items.Add(pair.Key);
            }

            foreach (var pair in DPays)
            {
                CB_pays.Items.Add(pair.Key);
            }


            if (!Create)
                LoadDico(Auteur);
            else
            {
                TB_Nom.Text = CreateNom;
                TB_Prenom.Text = CreatePrenom;
            }
        }


        public void LoadDico(string Auteur)
        {
            db.Query( string.Format("SELECT * From AUTEURSUNIQUE where NOMAUTEUR='{0}'", Auteur.Replace( "'", "''") ));
            
            foreach(var row in db.FetchAll())
            {
                id_auteur = Convert.ToInt32(row["id_auteur"]);

                foreach (var pair in row)
                {
                    foreach (Control c in this.Controls["groupBox3"].Controls)
                    {
                        if (c.Tag!=null)
                            if (c.Tag.ToString() == pair.Key)
                                c.Text = pair.Value.ToString();
                    }
                }

                if (TB_Nom.Text == "")
                {
                    string[] splitAuteur = Auteur.Split(',');
                    if (splitAuteur.Length > 1)
                    {
                        TB_Nom.Text = splitAuteur[0].Trim().ToLower();
                        TB_Prenom.Text = splitAuteur[1].Trim().ToLower();
                    }
                    else
                        TB_Nom.Text = Auteur;

                }

                TB_Biographie.Text = row["biographie"].ToString();

                if (row["certification"].ToString() == "1")
                {
                    pictureBox1.Image = SaisieLivre.Properties.Resources.certified_1;
                }
            }

        }

        private void CB_langue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DLangue.ContainsKey(CB_langue.SelectedItem.ToString()))
            {
                TB_Langue.Text = DLangue[CB_langue.SelectedItem.ToString()].ToString();
            }
        }

        private void CB_pays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DPays.ContainsKey(CB_pays.SelectedItem.ToString()))
            {
                TB_Pays.Text = DPays[CB_pays.SelectedItem.ToString()].ToString();
            }
        }

        private void TB_Langue_TextChanged(object sender, EventArgs e)
        {
            SetComboById(DLangue, CB_langue, TB_Langue);
        }

        private void TB_Pays_TextChanged(object sender, EventArgs e)
        {
            SetComboById(DPays, CB_pays, TB_Pays);
        }
        
        private void SetComboById(Dictionary<string, string> Dico, ComboBox CB, TextBox TB)
        {
            string code = "0";
            if (TB.Text == "")
                TB.Text = "0";

            try
            {
                code = Convert.ToInt32(TB.Text).ToString();
            }
            catch { }

            if (Dico.ContainsValue(code))
            {
                foreach (var item in Dico)
                {
                    if (item.Value == TB.Text)
                    {
                        CB.Text = item.Key;
                        break;
                    }
                }
            }
        }

        private void BT_Valider_Click(object sender, EventArgs e)
        {           
            SaveAut();
            this.Close();
        }



        private void SaveAut()
        {
            string result = "UPDATE AUTEURSUNIQUE SET id_session={2}, {0} WHERE id_auteur={1}";

            if (Creation)
                result = "INSERT INTO AUTEURSUNIQUE ( {0}, id_session ) VALUES ( {1}, {2} )";

            string fields = string.Empty;
            string values = string.Empty;
            string up = string.Empty;

            foreach (Control c in this.Controls["groupBox3"].Controls)
            {
                if (c.Tag != null)
                {
                    //c.text peut être null, on peut retirer des informations...
                    fields += c.Tag + ", ";
                    
                    string ValText = utf8_to_iso(c.Text);

                    //cas particulier pour les champs numériques.
                    if (c.Tag.ToString().Contains("langue") ||
                        c.Tag.ToString().Contains("pays"))
                        if (ValText == "")
                            ValText = "0";

                    if (c.Tag.ToString().Contains("naissance_jour") ||
                        c.Tag.ToString().Contains("deces_jour") ||
                        c.Tag.ToString().Contains("naissance_mois") ||
                        c.Tag.ToString().Contains("deces_mois"))
                        if (ValText == ""|| ValText == "0")
                            ValText = "1";

                    if (c.Tag.ToString().Contains("naissance_annee") ||
                        c.Tag.ToString().Contains("deces_annee"))
                        if (ValText == "")
                            ValText = "2070";


                    values += "'" + ValText + "', ";
                    up += c.Tag + "='" + ValText + "', ";
                }
            }

            if (TB_Biographie.Text != "")
            {
                fields += "biographie, ";
                values += "'" + utf8_to_iso(TB_Biographie.Text) + "', ";
                up += "biographie='" + utf8_to_iso(TB_Biographie.Text) + "', ";
            }

            if (cbx_valide.Checked)
            {
                fields += "Certification, ";
                values += "1, ";
                up += "Certification=1, ";
            }


            /* NOM AUTEUR */
            string AncienNom = string.Empty;
            if (!Creation)
                AncienNom = AuteurParam;

            string NouveauNom = TB_Nom.Text.Trim();

            if (TB_Prenom.Text.Trim() != "")
                NouveauNom += ", " + TB_Prenom.Text.Trim();

            //ajout de la particule si elle existe
            if (TB_Particule.Text.Trim() != "")
                NouveauNom += " " + TB_Particule.Text.Trim();

            //supprime les accents 
            NouveauNom = StrUpper(NouveauNom);


            /* ici on verifie que le nouveau nom n'est pas déjà connu dans le dico */
            bool Del = false;
            bool Upd = true;
            int id_auteur_remplacant = -1;


            //on a sélectionné un auteur dans la liste, pas de doute il s'écrit de cette façon.
            string QueryFind = "SELECT * from AUTEURSUNIQUE where trim( nomauteur )='" + utf8_to_iso(NouveauNom) + "'";

            //cas particulier création auteur, on cherche nom/prénom inversé et eventuellement particule pour être sur de ne pas créer de doublon.
            if (Creation)
                QueryFind = "SELECT * from AUTEURSUNIQUE where SearchIndexAlpha=StrCrunch( WordParserSort( WordParser( '" + utf8_to_iso(NouveauNom) + "' , 1, 30, 0 ) ) , 1 )";

            db.Query(QueryFind);
            foreach (Row row in db.FetchAll())
            {
                //id de l'auteur remplacant dans le cadre d'une modification.
                id_auteur_remplacant = Convert.ToInt32(row["id_auteur"]);

                //si c'est une création de nouvel auteur et qu'il existe déjà, on annule tout simplement.
                if (Creation)
                {
                    //ici pour être sur de ne pas créer un doublon, on teste si nouveaunom!=row[nomauteur]
                    //si true : c'est un doublon, il ne faut pas le créer, si false il est possible qu'il s'agisse d'un autre auteur (nom/prenom inversé) --> Creation = true
                    //on laisse le choix à l'utilisateur. on n'utilise pas la variable AncienNom car sinon le programme proposerait de modifier les réfs rattachées à l'autre auteur.
                    string TestNom = row["nomauteur"].ToString().Trim();

                    if (NouveauNom != TestNom)
                    {
                        DialogResult dr = MessageBox.Show("L'auteur " + TestNom + " existe déjà dans le dictionnaire.\r\n" +
                                                           "Confirmer la création de l'auteur " + NouveauNom + " ?", "Confirmation", MessageBoxButtons.YesNo);
                        if (dr != DialogResult.Yes)
                            Upd = false;
                    } 
                    else
                    {
                        MessageBox.Show("L'auteur " + NouveauNom + " existe déjà dans le dictionnaire.");
                        Upd = false;
                    }
                }
                else
                {
                    //on verifie qu'on a pas simplement changé une bio / date / langue mais qu'on a bien changé le nom d'auteur.
                    if (AncienNom != NouveauNom)
                    {
                        DialogResult dr = MessageBox.Show("L'auteur " + NouveauNom + " existe déjà dans le dictionnaire.\r\n" +
                                                          "Rattacher les références à " + NouveauNom + "\r\n" +
                                                          "et supprimer " + AncienNom + " ?", "Confirmation", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            Del = true;
                        }
                        else
                        {
                            dr = MessageBox.Show("Confirmer la mise à jour de l'auteur " + AncienNom + " ?", "Confirmation", MessageBoxButtons.YesNo);
                            if (dr == DialogResult.No)
                                Upd = false;
                        }
                    }
                }
                break;
            }

            //remplacement d'un auteur par un autre.
            if (Del)
            {
                //on compare les infos de l'auteur sélectionné avec les infos de l'auteur initial (BestInfo)
                //les infos de meilleure qualité sont selectionnées dans le dictionaire.
                //on utilise la propriété TAG des Controls. ils doivent être identiques aux noms des champs de la table auteursunique.
                foreach (Control c in this.Controls["groupBox3"].Controls)
                {
                    if (c.Tag != null)
                    {
                        if (BestInfo.ContainsKey(c.Tag.ToString()))
                        {
                            if ((c.Text.Length > BestInfo[c.Tag.ToString()].Length) && (c.Text != "0"))
                            {
                                BestInfo[c.Tag.ToString()] = c.Text;
                            }
                        }
                        else
                        {
                            if (c.Text.Length > 0)
                                BestInfo[c.Tag.ToString()] = c.Text;
                        }
                    }
                }

                //particule :l'auteur en cours a toujours raison.
                if (BestInfo.ContainsKey("particule"))
                    BestInfo["particule"] = TB_Particule.Text;
                

                //prenom :l'auteur en cours a toujours raison aussi.
                if (BestInfo.ContainsKey("prenom"))
                    BestInfo["prenom"] = TB_Prenom.Text;


                //l'auteur mal orthographié a une bio:
                if (BestInfo.ContainsKey("biographie"))
                {
                    //la bio de le l'auteur mal orthographié est connue
                    if (TB_Biographie.Text != BestInfo["biographie"])
                    {
                        /*
                        bio de l'auteur mal orthographie == bio de l'auteur correct, on ne fait rien                        
                        bio de l'auteur incorrect plus longue, on stock celle de l'auteur correct et on applique la bio de l'auteur incorrect au bon auteur. 
                        bio de l'auteur correct plus longue que celle de l'auteur incorrect --> on stock celle de l'auteur incorrect dans un txt 
                        */
                        if (TB_Biographie.Text.Length > BestInfo["biographie"].Length)
                        {
                            //writebio(id_auteur_remplacant, BestInfo["biographie"]);
                            BestInfo["biographie"] = TB_Biographie.Text;
                        }
                        /*else
                        {
                            writebio(id_auteur_remplacant, TB_Biographie.Text);
                        }*/
                    }
                }
                //l'auteur incorrect n'avait pas de bio, l'auteur correct en a une
                else
                {
                    if (TB_Biographie.Text.Length > 0)
                        BestInfo["biographie"] = TB_Biographie.Text;
                }

            }

            //on prépare la requete de modification ou de création.

            /* ATTENTION LES UPNOM - PRENOM SONT GERES PAR TRIGGER EN FONCTION DU CHAMP NOMAUTEUR SAUF POUR UPDATE */
            //UPNOM 
            fields += "upnom, ";
            values += "CAST( STRUPPER( '"+ utf8_to_iso(TB_Nom.Text) +"' ) as varchar(60)), ";
            up += "upnom=CAST( STRUPPER( '"+ utf8_to_iso(TB_Nom.Text) +"' ) as varchar(60)), ";

            //UPPRENOM 
            fields += "upprenom, ";
            values += "CAST( STRUPPER( '" + utf8_to_iso(TB_Prenom.Text) + "' ) as varchar(60)), ";
            up += "upprenom=CAST( STRUPPER( '" + utf8_to_iso(TB_Prenom.Text) + "' ) as varchar(60)), ";
            /*******************************************************************************************************/


            fields += "nomauteur, ";
            values += "'" + utf8_to_iso(NouveauNom) + "', ";
            up += "nomauteur='" + utf8_to_iso(NouveauNom) + "', ";


            fields = fields.Substring(0, fields.Length - 2);
            values = values.Substring(0, values.Length - 2);
            up = up.Substring(0, up.Length - 2);


            //on finalise la construction de la requête.
            if (Del)
                result = "delete from auteursunique where id_auteur=" + id_auteur.ToString();

            else if (Creation)
                result = string.Format(result, fields, values, idOrigine); //Insert into... id_auteur est incrémenté via trigger.

            else
                result = string.Format(result, up, id_auteur, idOrigine); //Update...


            //on execute la requête, sauf si on a annulé plus haut (Upd=false)
            if (Upd)
            {
                Cursor = Cursors.WaitCursor;
                db.Query(result);

                //on remplace les meilleures infos même si il n'y a pas de gencods associés
                if (Del)
                    update_dico(BestInfo, id_auteur_remplacant);


                //si il faut mettre à jour, on construit la liste des références.
                if (AncienNom != NouveauNom && AncienNom != string.Empty)
                    LoadGencods(AncienNom);


                //on met à jour les références associées 
                if (LGencods.Count > 0)
                {
                    //dans le cas d'un remplacement d'auteur, on le fait sans confirmation.
                    if (Del)
                    {
                        update_refs(AncienNom, NouveauNom);
                    }

                    //on verifie que le NomAuteur a été modifié, si oui on demande confirmation de MAJ.
                    else if (AncienNom != NouveauNom)
                    {
                        DialogResult dr = MessageBox.Show("Mettre à jour les " + LGencods.Count.ToString() + " réfs rattachées à l'auteur ?", "Confirmation", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            update_refs(AncienNom, NouveauNom);
                        }
                    }
                }
            }
        }

        private string utf8_to_iso(string utf8_str, bool doublequotes = true)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-15");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(utf8_str);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            string result = iso.GetString(isoBytes);
            if (doublequotes)
                result = result.Replace("'", "''");
            return result;
        }

        public string StrUpper(string input)
        {
            string stFormD = input.Normalize(NormalizationForm.FormD);
            int len = stFormD.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC)).ToUpper();
        }

        private int GetIdOrigine(string Origine)
        {
            int i = 0;
            db.Query("SELECT * FROM SESSIONS WHERE SESSION='" + Origine + "'");
            foreach (Row row in db.FetchAll())
            {
                i = Convert.ToInt32(row["id"]);
            }
            
            return i;
        }
                
        private void update_dico(Dictionary<string, string> BestInfo, int id_auteur_remplacant)
        {
            string q = "Update AUTEURSUNIQUE SET {0} WHERE ID_AUTEUR={1}";
            string r = string.Empty;
            foreach (var pair in BestInfo)
            {
                if ((pair.Value != "" && pair.Value != "0") && (pair.Key != "nom" && pair.Key != "prenom"))
                    r += pair.Key + "='" + utf8_to_iso(pair.Value) + "', ";
            }
            if (r != "")
            {
                r = r.Substring(0, r.Length - 2);
                q = string.Format(q, r, id_auteur_remplacant);

                db.Query(q);
            }
        }

        private void update_refs(string AncienNom, string NouveauNom)
        {
            int c = 0;
            foreach (string cab in LGencods)
            {
                UpdateAuteursByGencod(utf8_to_iso(AncienNom),
                                      utf8_to_iso(NouveauNom),
                                      cab);
                c++; ;
            }
        }

        private void UpdateAuteursByGencod(string _old, string _new, string _cab)
        {
            string CorrectionAuteur = string.Format(
                                          "Update LIVRE " +
                                          "set " +
                                          "auteurs = cast( STRREPLACE( auteurs, '{0}', '{1}', 0 ) as varchar(120)) " +
                                          "where " +
                                          "gencod = '{2}'", _old, _new, _cab);

            db.Query(CorrectionAuteur);
        }


        private void LoadGencods(string auteur)
        {
            LGencods.Clear();
            db.Query("Select gencod from auteurs where trim(auteur)='" + utf8_to_iso(auteur) + "'");
            foreach (Row row in db.FetchAll())
                LGencods.Add(row["gencod"].ToString());
        }


        private void ReplaceOfficeQuotes(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace("ʼ", "'");
        }

    }
}
