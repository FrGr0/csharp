using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ini;
using FirebirdDB;
using System.Threading;
using SplashScreenThreaded;
using System.IO;
using System.Net;
using Memorizer;
using CustomDateTimePicker;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using SuggestComboBox;

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * !!! IMPORTANT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * 
 * LE PROGRAMME DISPOSE D'UN SYSTEME DE MISE A JOUR AUTOMATIQUE
 * A CHAQUE MISE EN PRODUCTION D'UNE NOUVELLE VERSION DU PROGRAMME, IL FAUT PENSER A METTRE A JOUR
 * LA TABLE T_LOGICIELS DE LA BASE USERS sur STL-SAS-005 AVEC LE BON NUMERO DE VERSION (propriétés du projet -> assembly -> ProductVersion)
 * 
 */

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * !!! CHANGELOG !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
 * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * 
 * 69
 * MainForm : attribution des fonctions auteurs depuis les series si la série est ajoutée après les auteurs (et que les auteurs sont rattachés à la serie)
 * 
 * 68
 * MainForm : Ajout de l'alerte visuelle sur OBCode pour les series (dans Dictionary<string, FData> SerieData)
 * 
 * 67 
 * MainForm : Correction de ReloadSerie (cast as varchar(80) au lieu de varchar(50)
 * TradForm : Possibilité de créer un traducteur avec seulement un nom (pseudo) 
 * 
 * 66
 * MainForm : ajout d'un rechargement du combobox series à chaque chargement de fiche (methode ReloadSerie)
 * MainForm : modif type de champ NoSerie de smallint en varchar(10)
 * MainForm : genre principal activé par défaut si l'info vient de la fenêtre Collection/Series
 * MainForm : récuperation des fonctions auteurs si l'info vient de la fenêtre Collection/Series
 * NewCollectionForm : ajout du champ OBCode dans LIBSERIES
 * 
 * 64/65
 * NewCollectionForm : ajout du formulaire NewCollection utilisé pour gérer les collections et les séries.
 * MainForm : Refonte de la mise en évidences des infos collections/séries différentes des infos en base
 * MainForm : Retrait de la sous collection, ajout des séries
 * DilicomForm : ajout de tabcontrol pour affichage Collections/Séries
 * DilicomForm : ajout de champs (IAD, Scolaire, correspondances editeur/support, distri...) 
 * DilicomForm : modification en listview cliquable, les infos en bleu peuvent être transmises à la mainform sur double clic
 * MainForm : Ajout des checkboxes LUXE et ILLUSTRE
 * MainForm : TB_Titre en lecture seule
 * MainForm : limitation du nombre de caractères pour les champs Titre(250)/libelle(250)/auteurs(120)/nocoll(10)/noscoll(10)
 * MainForm : désactive le champ nocoll si collection est inconnue, sans effacer la valeur.
 * MainForm : modification du "T." en "N." si codesupport R ou C
 * MainForm : ajout de l'autocomplétion sur le libelle OB
 * MainForm : ajout de la tabulation codegenre -> libelle genre
 * MainForm : ajout du memorizer sur les 3 combobox langue
 * MainForm : F12 sur les 2 derniers champs langue.
 * MainForm : F2 pour forcer le contenu du champ Commentaire en minuscules.
 * MainForm : Ajout de la selection du champ millesime en prévision du nouveau champ edition
 * MainForm : Correction de l'alimentation du memorizer sur le combobox collections
 * MainForm : Ajout du millesime/n° d'édition
 * General : nettoyage des sources 
 * 
 * 60
 * mainform : limitation du nombre de caractères dans le champ N°Coll (5)
 * mainform : correction d'un bug de l'updater
 * 
 * 59
 * mainform : supprime l'alimentation du clipboard avec la queryupdatelivre (bug elodie)
 * 
 * 58
 * mainform : correction bug à la création de fiche (TB_IDCollection.Text = "0"; par défaut)
 * mainform : si le libellé commence par le nom de collection, alerte visuelle sur le libelle.
 * auteursform : ajout d'une condition dans la requete de vérification dans le dictionnaire (and nomauteur starts with...)
 * 
 * 57
 * collectionform : si pas d'info dans un champ, valeur = null
 * collectionform : à la validation (creation/update), report de l'IDCollection dans MainForm.TB_IDCollection
 * collectionform : demande de confirmation de modification d'une collection avec nom de collection différent.
 * collectionform : modification des références liées à la collection si le libellé avant!=libellé après
 * mainform : correction F12 : pour les revues, application des evenements present_titre=1 si rappel F12 dans le champ collection.
 * mainform : correction bug suppression d'une collection sur une fiche. 
 * mainform : correction bug load distributeur si livre.distributeur = editeurs.distributeur2 ou 3
 * mainform : amelioration de la prise en charge des caractères office dans les titres, resumés, sommaires...
 * mainform : ajout d'un système de contrôle de version du logiciel (CheckProductVersion();)
 *
 * 56
 * mainform : ajout de la gestion des distributeurs multiples pour un éditeur.
 * Dilicomform : correction bug fenetre détail collection qui boucle à l'infini
 * mainform : correction bug previousean = tb_ean.text quand tb_ean est vide 
 * mainform : correction bug liste d'ean (collé ou depuis txt) avec espaces
 * collectionform : correction bug module collections (editeur + libelle_complet --> editeur)
 * collectionform : correction bug module collections (maj de collection existante si libellé différent à l'ouverture du formulaire)
 */

namespace SaisieLivre
{
    public partial class MainForm : Form
    {       
        #region variables globales               
        public Font deffont = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular),
                     boldfont = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold),
                     LoadedFont;

        private Memorizer.Memorizer M = new Memorizer.Memorizer();

        private IniFile ini = new IniFile(Path.Combine(Directory.GetCurrentDirectory(), "config.ini"));
        public FDB db;

        private bool AutoCopyEan = false,                     
                     MultiDistri = false,
                     AddMultiDistri = false,
                     IsActivated = false,
                     Found;

        public Dictionary<string, string> DLangue = new Dictionary<string, string>(),
                                          DPays = new Dictionary<string, string>(),
                                          DLectorat = new Dictionary<string, string>(),
                                          DSupport = new Dictionary<string, string>(),
                                          DStyles = new Dictionary<string, string>(),
                                          DTraducteur = new Dictionary<string, string>(),
                                          DOBCode = new Dictionary<string, string>(),
                                          DCL_Genre_0 = new Dictionary<string, string>(),
                                          DCL_Genre_1 = new Dictionary<string, string>(),
                                          DCL_Genre_2 = new Dictionary<string, string>(),
                                          DCL_Genre_3 = new Dictionary<string, string>(),
                                          DCL_Langue_0 = new Dictionary<string, string>(),
                                          DCL_Langue_1 = new Dictionary<string, string>(),
                                          DCL_Langue_2 = new Dictionary<string, string>(),
                                          DEdiFullToEdi = new Dictionary<string, string>(), 
                                          DFunctionAuteurs = new Dictionary<string, string>();
            
        private Dictionary<string, string> DOrigine = new Dictionary<string, string>(),                                           
                                           D_Scolaire_Support = new Dictionary<string, string>(),                                           
                                           DMatiere = new Dictionary<string, string>(),
                                           DClasse = new Dictionary<string, string>(),
                                           DSections = new Dictionary<string, string>(),
                                           DSousCollection = new Dictionary<string, string>(),                                           
                                           DTVA = new Dictionary<string, string>(),
                                           DTheme = new Dictionary<string, string>(),                                           
                                           DCSR = new Dictionary<string, string>(),
                                           DDispo = new Dictionary<string, string>(),
                                           DGenresFuret = new Dictionary<string, string>(),                                           
                                           DEdiCodeEdi = new Dictionary<string, string>(),                                                                                                                               
                                           DSeries = new Dictionary<string,string>();


        
        private Row CollectionRow, SerieRow;

        private Dictionary<string, Dictionary<int, string>> DEditeur = new Dictionary<string, Dictionary<int, string>>();
        private Dictionary<string, List<string>> DSousCollByColl = new Dictionary<string, List<string>>();        
        private Dictionary<Control, string> DPSaisieDefault = new Dictionary<Control, string>();
        private Dictionary<string, Row> DCollection = new Dictionary<string, Row>();
        private Dictionary<int, string> DMDistri = new Dictionary<int, string>();
        private Dictionary<string, Control> CollFields = new Dictionary<string, Control>();
        
        private Dictionary<string, FData> CollectionData = new Dictionary<string, FData>(),
                                          SerieData = new Dictionary<string, FData>();


        private static SplashScreenForm sf = new SplashScreenForm();

        private DilicomForm SD, CD; //SD = Dilicom - CD = Collection       

        private ImageForm IF = null;

        private string QueryLivre, 
                       QueryLivreAlternate, 
                       QueryLivreBis, 
                       BaseURL,
                       PreviousEAN      = string.Empty,
                       PreviousGenre    = string.Empty,
                       PreviousResume   = string.Empty,
                       PreviousSommaire = string.Empty,                      
                       LoadedAuteurs    = string.Empty, 
                       MemoSerie        = string.Empty,
                       MemoCollection   = string.Empty;

        public string LBSAut = string.Empty;
        
        private WebClient wc = new WebClient();

        public Dictionary<string, Control> DilicomToTL = new Dictionary<string, Control>();

        private Image I, I4;

        private Color Coll_Diff_Back = Color.FromArgb(255, 204, 102),
                      Coll_Diff_Fore = Color.Red,
                      Seri_Diff_Back = Color.FromArgb(102, 204, 255),
                      Seri_Diff_Fore = Color.Purple;

        #endregion

        public MainForm()
        {
            this.Hide();
            Thread splashthread = new Thread(new ThreadStart(SplashScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();    

            InitializeComponent();           
        }

        #region Methodes MainForm Creation Destruction Handle

        // OUVERTURE DE LA FICHE PRINCIPALE
        // affiche d'abord le splashscreen pour la préparation des listes
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += ProductVersion;
            this.MaximumSize = new Size(883, this.Height);

            string FontName = ini.IniReadValue("FONT", "Name");
            
            string FontSize = ini.IniReadValue("FONT", "Size").Replace( ',', '.' );
            if (FontSize == "")
                FontSize = "8.25";

            float FSize = float.Parse(FontSize, CultureInfo.InvariantCulture.NumberFormat);

            LoadedFont = new Font(FontName, FSize);

            if (FontName != "")
                this.Font = LoadedFont;

            TSSL_DM.Text = "";
            TSSL_DMD.Text = "";
            TSSL_DMI.Text = "";
            TSSL_DTCREA.Text = "";

            #region correspondance libelle dilicomform/mainform
            DilicomToTL.Add("editeurtl", CB_Editeur);
            DilicomToTL.Add("supporttl", TB_CodeSupport);
            DilicomToTL.Add("codesupport", TB_CodeSupport);
            DilicomToTL.Add("supportclil", null);
            DilicomToTL.Add("hauteur", TB_Hauteur);
            DilicomToTL.Add("largeur", TB_Largeur);
            DilicomToTL.Add("epaisseur", TB_Epaisseur);
            DilicomToTL.Add("codelangue", TB_Langue_Concat);
            DilicomToTL.Add("prixeuro", TB_PrixEuro);
            DilicomToTL.Add("dateparution", DTP_DateParution);
            DilicomToTL.Add("auteurs", TB_Auteurs);
            DilicomToTL.Add("titre", TB_Titre);
            DilicomToTL.Add("tva", CB_TVA);
            DilicomToTL.Add("codegenre", TB_GenrePrincipal);
            DilicomToTL.Add("genre_principal", TB_GenrePrincipal);
            DilicomToTL.Add("genre_secondaire_", LB_Genres);
            DilicomToTL.Add("dispo", TB_Dispo);
            DilicomToTL.Add("iad", TB_IAD);
            DilicomToTL.Add("collectiontl", CB_Collection);
            DilicomToTL.Add("nocoll", TB_Nocoll);
            DilicomToTL.Add("nbrpage", TB_NBPages);
            DilicomToTL.Add("obcode", TB_OBCode);
            DilicomToTL.Add("distributeur", CB_Distri);
            DilicomToTL.Add("poids", TB_Poids);
            DilicomToTL.Add("L", CBX_Luxe);
            DilicomToTL.Add("cartonne", cbx_cartonne);
            DilicomToTL.Add("R", CBX_relie);
            DilicomToTL.Add("B", CBX_broche);
            DilicomToTL.Add("livre_lu", TB_LivreLu);
            DilicomToTL.Add("grands_cara", TB_GrandsCara);
            DilicomToTL.Add("multilingue", TB_Multilingue);
            DilicomToTL.Add("illustre", TB_Illustre);
            DilicomToTL.Add("luxe", TB_Luxe);
            DilicomToTL.Add("relie", TB_Relié);
            DilicomToTL.Add("broche", TB_Broché);
            DilicomToTL.Add("lectorat", CB_Lectorat);
            DilicomToTL.Add("langue_de", CB_Langue);
            DilicomToTL.Add("langue_en", CB_TraduitEn);
            DilicomToTL.Add("style", CB_Style);
            DilicomToTL.Add("traducteur", CB_Traducteur);
            #endregion

            SD = new DilicomForm( DilicomToTL, this );
            CD = new DilicomForm( DilicomToTL, this, true );
            
            P_Origine.BringToFront();

            SplashScreen.UdpateStatusTextWithStatus("en cours", TypeOfMessage.Warning);
            Thread.Sleep(1000);
            
            //paramètres proxy par défaut
            wc.Proxy.Credentials = CredentialCache.DefaultCredentials;

            //vide la listbox saisie ean
            LB_Saisie_EAN.Items.Clear();

            #region FDATA
            FData FDAuteurs = new FData("auteurs", TB_Auteurs, BT_Auteurs, null, "Auteurs (F3) :", "Auteurs (F2) :");
            CollectionData.Add("auteurs", FDAuteurs);
            SerieData.Add("auteurs", FDAuteurs);
                                   
            FData FDLectorat = new FData("lectorat", CB_Lectorat, l_lectorat);
            CollectionData.Add("lectorat", FDLectorat);
            SerieData.Add("lectorat", FDLectorat);

            FData FDStyle = new FData("style", CB_Style, l_style);
            CollectionData.Add("style", FDStyle);
            SerieData.Add("style", FDStyle);

            FData FDCodeLangue = new FData("codelangue", TB_Langue_Concat, l_codelangue, CB_Langue_0, "", "", CB_Langue_1, CB_Langue_2);
            CollectionData.Add("codelangue", FDCodeLangue);
            SerieData.Add("codelangue", FDCodeLangue);

            FData FDTraducteur = new FData("traducteur", CB_Traducteur, BT_Trad, null, "Trad (F3) :", "Trad (F2) :");
            CollectionData.Add("traducteur", FDTraducteur);
            SerieData.Add("traducteur", FDTraducteur);

            FData FDLangue_de = new FData("langue_de", CB_Langue, l_langue_de);
            CollectionData.Add("langue_de", FDLangue_de);
            SerieData.Add("langue_de", FDLangue_de);

            FData FDLangue_en = new FData("langue_en", CB_TraduitEn, l_langue_en);
            CollectionData.Add("langue_en", FDLangue_en);
            SerieData.Add("langue_en", FDLangue_en);

            FData FDGTL_Principal = new FData("genre_principal", TB_GenrePrincipal, l_genreprincipal);
            CollectionData.Add("genre_principal", FDGTL_Principal);
            SerieData.Add("genre_principal", FDGTL_Principal);

            FData FDSupport = new FData("codesupport", TB_CodeSupport, l_support, CB_CodeSupport);
            CollectionData.Add("codesupport", FDSupport);

            FData FDLivre_Lu = new FData("livre_lu", TB_LivreLu, CBX_livre_lu);
            CollectionData.Add("livre_lu", FDLivre_Lu);

            FData FDGrandsChara = new FData("grands_cara", TB_GrandsCara, CBX_grandcaractere);
            CollectionData.Add("grands_cara", FDGrandsChara);

            FData FDMultilingue = new FData("multilingue", TB_Multilingue, CBX_Multilingue);
            CollectionData.Add("multilingue", FDMultilingue);

            FData FDIllustre = new FData("illustre", TB_Illustre, CBX_Illustre);
            CollectionData.Add("illustre", FDIllustre);

            FData FDIAD = new FData("iad", TB_IAD, CBX_IAD);
            CollectionData.Add("iad", FDIAD);

            FData FDLuxe = new FData("luxe", TB_Luxe, CBX_Luxe);
            CollectionData.Add("luxe", FDLuxe);

            FData FDCartonne = new FData("cartonne", TB_Cartonne, cbx_cartonne);
            CollectionData.Add("cartonne", FDCartonne);

            FData FDRelie = new FData("relie", TB_Relié, CBX_relie);
            CollectionData.Add("relie", FDRelie);

            FData FDBroche = new FData("broche", TB_Broché, CBX_broche);
            CollectionData.Add("broche", FDBroche);

            FData FDNbpages = new FData("nbrpage", TB_NBPages, l_nbpages);
            CollectionData.Add("nbrpage", FDNbpages);

            FData FDHauteur = new FData("hauteur", TB_Hauteur, l_hauteur);
            CollectionData.Add("hauteur", FDHauteur);

            FData FDLargeur = new FData("largeur", TB_Largeur, l_largeur);
            CollectionData.Add("largeur", FDLargeur);

            FData FDOBCode = new FData("obcode", TB_OBCode, l_obcode, CB_OBCode);
            CollectionData.Add("obcode", FDOBCode);
            SerieData.Add("obcode", FDOBCode);
            #endregion            

            //lecture du fichier de configuration
            //connexion à la base de données            
            TB_Parametre_Host.Text = ini.IniReadValue("BDD", "HOST");
            TB_Parametre_Name.Text = ini.IniReadValue("BDD", "NAME");
            TB_Parametre_User.Text = ini.IniReadValue("BDD", "USER");
            TB_Parametre_Pass.Text = ini.IniReadValue("BDD", "PASS");

            BaseURL = ini.IniReadValue("IMAGES", "URL");

            if (ini.IniReadValue("TABSTOP", "LIBGENRES") == "False")
                CBX_Param_TabStop_LibGenres.Checked = true;            

            if (ini.IniReadValue("ONTOP", "DILICOLL") == "True")
                CBX_OnTopDilicom.Checked = true;

            if (ini.IniReadValue("DEBUG", "BETA") == "1")
            {
                CBX_AffichageDebug.Visible = true;
                if (ini.IniReadValue("DEBUG", "VIEW") == "1")
                {
                    CBX_AffichageDebug.Checked = true;
                }
            }
            
            if (ini.IniReadValue("DILICOM", "ANCRAGE") == "1")
            {
                CBX_AncrageDilicom.Checked = true;
                SD.CanMove = false;
                SD.DefaultLocation = new Point((this.Location.X + this.Width + 2), this.Location.Y); 
                SD.Location = SD.DefaultLocation;
            }

            if (ini.IniReadValue("COLLECTION", "DISPLAY") == "1")
            {
                CBX_AffichageInfoColl.Checked = true;                
                CD.Text = "Collection & Séries";    
            }
            
            if (ini.IniReadValue("COLLECTION", "ANCRAGE") == "1")
            {
                CBX_Ancrage_Collection.Checked = true;
                CD.CanMove = false;
                CD.DefaultLocation = new Point((this.Location.X + this.Width + 2), (this.Location.Y + SD.Height + 2));
                CD.Height = this.Height - (SD.Height + 2);
                CD.Location = CD.DefaultLocation;
            }


            if (ini.IniReadValue("DEVEL", "MultiDistribution") == "1")
            {
                MultiDistri = true;
                CB_Distri.Size = new Size(217, 20);
                BT_AddMultiDistri.Visible = true;
            }

            if (ini.IniReadValue("DEVEL", "Chainage") == "1")
            {
                CBX_SupprChainage.Enabled = true;
            }


            string ConnexionString = string.Format(@"{0}:{1}@{2}:{3}", TB_Parametre_User.Text, 
                                                                       TB_Parametre_Pass.Text,
                                                                       TB_Parametre_Host.Text, 
                                                                       TB_Parametre_Name.Text);

            try
            {

                db = new FDB(ConnexionString);
                                               
                QueryLivre          = SaisieLivre.Properties.Resources.QueryLivre;
                QueryLivreBis       = SaisieLivre.Properties.Resources.QueryLivreBis;
                QueryLivreAlternate = SaisieLivre.Properties.Resources.QueryLivreAlternate;

                DOrigine            = LoadListe("SELECT * from ORIGINE order by Saisie", "ID_ORIGINE", "SAISIE", CB_Origine);

                CB_Origine.Text = ini.IniReadValue("GENERAL", "DEFAULTUSER");

                if (CB_Origine.Items.Contains(CB_Origine.Text))
                {
                    BT_OrigineValider.Enabled = true;
                    CB_Origine.Focus();
                }

                LoadDefaultDicos();
                
                SplashScreen.UdpateStatusTextWithStatus("Chargement des Auteurs", TypeOfMessage.Warning);                                                

                //alimentation du dico des valeurs par défaut
                DPSaisieDefault.Add(TB_NoSColl, "0");
                DPSaisieDefault.Add(TB_Nocoll, "0");
                DPSaisieDefault.Add(TB_DateParution, DateTime.Today.ToString("dd/MM/yyyy"));
                DPSaisieDefault.Add(CB_Distri, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Erreur de connexion à la base de données.\r\nVerifiez les paramètres et redémarrez le programme");
                TC_Main.SelectedTab = TC_Main.TabPages["TP_Parametre"];
            }
                        
            SplashScreen.UdpateStatusTextWithStatus("terminé !", TypeOfMessage.Success);
            Thread.Sleep(1000);

            this.Show();
            SplashScreen.CloseSplashScreen();
            this.Activate();

            CheckProductVersion();            
            
        }

        private void LoadDefaultDicos()
        {
            DSupport = LoadListe("SELECT * from SUPPORT order by LibSupport", "CODESUPPORT", "LIBSUPPORT", CB_CodeSupport);
            DLectorat = LoadListe("SELECT id, case when (id=0) then '' else cast(lower(libelle) as varchar(40)) end as libelle from LIBLECTORAT where actif=1 order by ordre", "id", "libelle", CB_Lectorat);
            DSousCollection = LoadListe("SELECT * from SOUSCOLLECTIONS where libelle!=''", "id", "libelle"); //, CB_SousCollection);
            DSeries = LoadListe("select id, case when (id=0) then '' else libelle end as libelle from libseries", "id", "libelle", CB_Serie);
            CB_Serie.Sorted = true;

            DSousCollByColl = new Dictionary<string, List<string>>();

            //Chargement du dictionnaire DSousCollByColl de type <string, List<string>>
            db.Query("select id, idcollection from souscollections where libelle!=''");
            foreach (var row in db.FetchAll())
            {
                if (!DSousCollByColl.ContainsKey(row["idcollection"].ToString()))
                {
                    List<string> L = new List<string>();
                    L.Add(row["id"].ToString());
                    DSousCollByColl.Add(row["idcollection"].ToString(), L);
                }
                else
                {
                    if (!DSousCollByColl[row["idcollection"].ToString()].Contains(row["id"].ToString()))
                    {
                        DSousCollByColl[row["idcollection"].ToString()].Add(row["id"].ToString());
                    }
                }
            }


            //dictionnaires EDITEUR (on ajoute le libelle_complet dans le menu déroulant... 
            //attention a faire une équivalence avec les libellés simples pour la sauvegarde.
            LoadDicoEditeurs();

            DTVA = LoadListe("SELECT codetva, roundcent( tauxtva ) as tauxtva from tva", "codetva", "tauxtva", CB_TVA);
            DTraducteur = LoadListe("SELECT id, case when (id=0) then '' else cast( Nom || Case when ( COALESCE( prenom, '')!='' ) then ', ' || prenom else '' end as varchar(82)) end as nom from TRADUCTEURS order by 2", "id", "nom");
            DLangue = LoadListe("SELECT id, case when (id=0) then '' else libelle end as libelle from LIBLANGUES order by libelle", "id", "libelle", CB_Langue, CB_TraduitEn);
            DPays = LoadListe("SELECT codepays, libelle from LIBPAYS", "codepays", "libelle");
            DStyles = LoadListe("SELECT col0, case when (col0=0) then '' else col1 end as col1 from LIBSTYLES order by col1", "col0", "col1", CB_Style); //col0 col1 col2 pour des tables en prod sans déconner...                  
            DOBCode = LoadListe("SELECT o.obcode, (select libelle from GetFullLibelleOB( o.obcode )) as libelle from OMBRESBLANCHES_CODES o order by 2", "obcode", "libelle", CB_OBCode);
            DCSR = LoadListe("SELECT codecsr, libcsr from CSR", "libcsr", "codecsr");
            DDispo = LoadListe("SELECT * from libdispo", "col0", "col1", CB_Dispo);
            DCL_Langue_0 = LoadListe("SELECT id, libelle from CL_Langue_0", "id", "libelle", CB_Langue_0);
            DCL_Langue_1 = LoadListe("SELECT id, libelle from CL_Langue_1", "id", "libelle", CB_Langue_1);
            DCL_Langue_2 = LoadListe("SELECT id, libelle from CL_Langue_2 ", "id", "libelle", CB_Langue_2);
            DCL_Genre_0 = LoadListe("SELECT id, libelle from CL_Genre_0", "id", "libelle", CB_Genre_0);
            DCL_Genre_1 = LoadListe("SELECT id, cast( id || ';' || libelle as varchar(200)) as libelle from CL_Genre_1", "id", "libelle");
            DCL_Genre_2 = LoadListe("SELECT id, cast( id || ';' || libelle as varchar(200)) as libelle from CL_Genre_2", "id", "libelle");
            DCL_Genre_3 = LoadListe("SELECT id, cast( id || ';' || libelle as varchar(200)) as libelle from CL_Genre_3", "id", "libelle");
            DGenresFuret = LoadListe("SELECT id, libelle from LIBGENRE_FURET", "libelle", "id");
            DFunctionAuteurs = LoadListe("select id, libelle from libfonction_auteur order by libelle", "libelle", "id");
        }

        //fermeture PROPRE de la connexion à la BDD
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            db.Close();
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (CBX_AncrageDilicom.Checked)
            {
                SD.CanMove = false;
                SD.DefaultLocation = new Point((this.Location.X + this.Width + 2), this.Location.Y);
                SD.Location = SD.DefaultLocation;                
            }

            if (CBX_Ancrage_Collection.Checked)
            {
                CD.CanMove = false;
                CD.DefaultLocation = new Point((this.Location.X + this.Width + 2), (this.Location.Y + SD.Height + 2));
                CD.Location = CD.DefaultLocation;
            }
        }                

        #endregion

        #region Methodes MainForm clic sur labels

        private void LBClickAlternate(Control sender, Control senderlabel)
        {
            if (senderlabel.ForeColor == Coll_Diff_Fore)
            {
                foreach (var pair in CollectionData)
                {
                    FData fd = pair.Value;
                    if (fd.Control.Name == sender.Name && fd.Label == senderlabel)
                    {
                        fd.Control.Text = CollectionRow[fd.Field].ToString();

                        if (fd.Control.Name == "TB_Langue_Concat")
                        { 
                            //on force 3 clicks pour le codegenre à cause des recalculs de listes genres2/3
                            fd.Control.Text = CollectionRow[fd.Field].ToString(); 
                            fd.Control.Text = CollectionRow[fd.Field].ToString();
                        }

                        if (fd.Control.Name == "TB_Auteurs")
                            LoadArtFuncFromSerieColl(CB_Collection, "coll");

                        fd.Control.BackColor = SystemColors.Window;

                        if (fd.Control2!=null)
                            fd.Control2.BackColor = SystemColors.Window;

                        if (fd.Control3!=null)
                            fd.Control3.BackColor = SystemColors.Window;
                        
                        if (fd.Control4 != null)
                            fd.Control4.BackColor = SystemColors.Window;

                        break;
                    }
                }
            }

            else if (senderlabel.ForeColor == Seri_Diff_Fore)
            {               
                foreach (var pair in SerieData)
                {
                    FData fd = pair.Value;
                    if (fd.Control.Name == sender.Name && fd.Label == senderlabel)
                    {                        
                        fd.Control.Text = SerieRow[fd.Field].ToString();

                        if (fd.Control.Name == "TB_Langue_Concat")
                        {
                            //on force 3 clicks pour le codegenre à cause des recalculs de listes genres2/3
                            fd.Control.Text = SerieRow[fd.Field].ToString();
                            fd.Control.Text = SerieRow[fd.Field].ToString();
                        }

                        if (fd.Control.Name == "TB_Auteurs")
                            LoadArtFuncFromSerieColl(CB_Serie, "serie");

                        fd.Control.BackColor = SystemColors.Window;

                        if (fd.Control2 != null)
                            fd.Control2.BackColor = SystemColors.Window;
                        
                        if (fd.Control3 != null)
                            fd.Control3.BackColor = SystemColors.Window;
                        
                        if (fd.Control4 != null)
                            fd.Control4.BackColor = SystemColors.Window;

                        break;
                    }
                }
            }
        }

        private void l_support_Click(object sender, EventArgs e)
        {
            LBClickAlternate(TB_CodeSupport, l_support);       
        }

        private void l_nbpages_Click(object sender, EventArgs e)
        {
            LBClickAlternate(TB_NBPages, l_nbpages);
        }

        private void l_hauteur_Click(object sender, EventArgs e)
        {
            LBClickAlternate(TB_Hauteur, l_hauteur);
        }

        private void l_largeur_Click(object sender, EventArgs e)
        {
            LBClickAlternate(TB_Largeur, l_largeur);
        }

        private void l_obcode_Click(object sender, EventArgs e)
        {
            LBClickAlternate(TB_OBCode, l_obcode);
        }

        private void l_lectorat_Click(object sender, EventArgs e)
        {
            LBClickAlternate(CB_Lectorat, l_lectorat);
        }

        private void l_langue_de_Click(object sender, EventArgs e)
        {
            LBClickAlternate(CB_Langue, l_langue_de);
        }

        private void l_langue_en_Click(object sender, EventArgs e)
        {
            LBClickAlternate(CB_TraduitEn, l_langue_en);
        }

        private void l_style_Click(object sender, EventArgs e)
        {
            LBClickAlternate(CB_Style, l_style);
        }


        private void l_codelangue_Click(object sender, EventArgs e)
        {
            LBClickAlternate(TB_Langue_Concat, l_codelangue);
        }

        private void l_genreprincipal_Click(object sender, EventArgs e)
        {
            if (l_genreprincipal.ForeColor == Coll_Diff_Fore ||
                l_genreprincipal.ForeColor == Seri_Diff_Fore )
            {
                int idxremove = -1;
                MyListBoxItem Replace = null;

                for (int i=0;i<LB_Genres.Items.Count;i++)
                {
                    MyListBoxItem mlbi = (MyListBoxItem)LB_Genres.Items[i];

                    if (mlbi.Message == TB_GenrePrincipal.Text)
                    {
                        idxremove = i;

                        if (mlbi.Message != "00-00-00-00")
                        {
                            Replace = new MyListBoxItem(SystemColors.WindowText, mlbi.Message, deffont);
                        }
                    }
                }
                if (idxremove >= 0)                
                    LB_Genres.Items.RemoveAt(idxremove);
                
                if (Replace != null && Replace.Message != TB_GenrePrincipal.Text)
                    LB_Genres.Items.Add(Replace);

                LBClickAlternate(TB_GenrePrincipal, l_genreprincipal);

                string NewGenrePrincipal = RewriteGenre(TB_GenrePrincipal.Text.Replace("-", "") + ":1");
                TB_GenrePrincipal.Text = NewGenrePrincipal.Substring(0, NewGenrePrincipal.Length - 2);                
                LB_Genres.Items.Insert(0, new MyListBoxItem(Color.Blue, NewGenrePrincipal.Substring(0, NewGenrePrincipal.Length - 2), boldfont));
                LB_Genres.SelectedIndex = 0;
                LB_Genres.Select();                
                l_genreprincipal.ForeColor = SystemColors.ControlText;
            }
        }

        #endregion

        #region Methodes MainForm TextBox

        // Chargement d'une fiche
        private void TB_EAN_TextChanged(object sender, EventArgs e)
        {            
            string source = TB_EAN.Text;

            if (StripEAN(TB_EAN.Text) != source)
            {
                TB_EAN.Text = StripEAN(TB_EAN.Text);
                TB_EAN.SelectionStart = TB_EAN.TextLength;
                return;
            }

            if (TB_EAN.TextLength < 10 || (TB_EAN.TextLength > 10 && TB_EAN.TextLength < 13))
                TB_EAN.BackColor = SystemColors.Window;

            else if (TB_EAN.TextLength == 10)
                TB_EAN.BackColor = Color.Yellow;

            else if (TB_EAN.TextLength == 13 &&  CheckEAN(TB_EAN.Text)==TB_EAN.Text)
                TB_EAN.BackColor = Color.PaleGreen;

            else
                TB_EAN.BackColor = Color.OrangeRed;                
        }        

        private void TB_EAN_KeyUp(object sender, KeyEventArgs e)
        {             
            if (e.KeyData == Keys.Enter)
            {
                PreviousEAN = "";

                string cab = ((TextBox)sender).Text;
                BT_Annuler_Click(sender, e);

                if (cab.Length == 10)
                    ((TextBox)sender).Text = ISBN2CAB(cab);
                else
                    ((TextBox)sender).Text = cab;

                TB_EAN_Leave(sender, e);
            }            
        }        

        private void TB_Lectorat_TextChanged(object sender, EventArgs e)
        {   
            FindIdByLib(DLectorat, TB_Lectorat.Text, CB_Lectorat);                              
        }        
        
        private void TB_CodeTVA_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DTVA, TB_CodeTVA.Text, CB_TVA);
        }

        private void TB_Langue_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DLangue, TB_Langue.Text, CB_Langue);
        }

        private void TB_IDTraduitEn_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DLangue, TB_IDTraduitEn.Text, CB_TraduitEn);
        }

        private void TB_Style_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DStyles, TB_Style.Text, CB_Style);
        }              
        
        private void TB_CodeCSR_TextChanged(object sender, EventArgs e)
        {
            if (DCSR.ContainsKey(TB_CodeCSR.Text.Trim()))
                TB_LibOLDCSR.Text = DCSR[TB_CodeCSR.Text.Trim()];
            else
                TB_LibOLDCSR.Text = "";
        }

        private void TB_Dispo_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DDispo, TB_Dispo.Text, CB_Dispo);
        }
        
        private void TB_IDTraducteur_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DTraducteur, TB_IDTraducteur.Text, CB_Traducteur);
            if (CB_Traducteur.Text == "NON DEFINI, NON DEFINI")
                CB_Traducteur.Text = "";
        }

        private void TB_DateParution_TextChanged(object sender, EventArgs e)
        {
            string str_DateParu = TB_DateParution.Text;
            if (str_DateParu.Length == 10)
            {
                try
                {
                    DateTime DT = DateTime.ParseExact(str_DateParu, "dd/MM/yyyy", null);
                    DTP_DateParution.Value = DT;
                }
                catch
                {
                    DTP_DateParution.Value = DateTime.Today;
                }
            }
        }
        
        //alimentation de la listebox chainage.
        private void TB_IDLivre_TextChanged(object sender, EventArgs e)
        {
            if (TB_IDLivre.Text!="")
            {
                string cr = string.Empty;
                db.Query(string.Format("select coderegroupement from CR_LIVRE where idlivre={0}", TB_IDLivre.Text));
                foreach (var row in db.FetchAll())
                {
                    cr = row["coderegroupement"].ToString();
                }
                if (cr != "")
                {
                    db.Query( string.Format("select " +
                                            "  l.gencod "+
                                            "from livre l "+
                                            "join cr_livre c on l.id=c.idlivre "+
                                            "where "+
                                            "  c.coderegroupement = {0} and "+
                                            "  c.idlivre != {1} and "+
                                            "  coalesce( l.Codesupport, 'T' ) != 'LA' and "+
                                            "  coalesce( l.livreancien, 0) = 0", cr, TB_IDLivre.Text));
                    foreach (var row in db.FetchAll())
                    {
                        LB_RefsChainees.Items.Add(row["gencod"].ToString());
                    }
                }

            }
        }

        private void TB_CodeCSRFuret_TextChanged(object sender, EventArgs e)
        {
            string LibCSRFuret = string.Empty;
            if (TB_CodeCSRFuret.Text != "")
            {
                try
                {
                    LibCSRFuret = DGenresFuret[TB_CodeCSRFuret.Text.Trim()];
                }
                catch
                {
                    LibCSRFuret = "CodeCSR Furet Inconnu !";
                }
            }
            TB_LibCSRFuret.Text = LibCSRFuret;
        }

        private void NumberOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TB_Titre_TextChanged(object sender, EventArgs e)
        {           
            SetNewTitre();
        }

        private void TB_Edition1_TextChanged(object sender, EventArgs e)
        {
            TB_Edition2.Text = "";

            SetNewTitre();
            if (TB_Edition1.TextLength == 4)
                TB_Edition2.Enabled = true;
            else
                TB_Edition2.Enabled = false;
        }

        private void TB_Edition2_TextChanged(object sender, EventArgs e)
        {
            SetNewTitre();
        }

        private void TB_Millesime_TextChanged(object sender, EventArgs e)
        {
            if (TB_Millesime.Text != "")
            {

                int Mil = Convert.ToInt32(TB_Millesime.Text);
                if (Mil < 0)
                {
                    TB_Edition1.Text = TB_Millesime.Text.Substring(1, TB_Millesime.TextLength - 1);
                    TB_Edition2.Text = (Convert.ToInt32(TB_Edition1.Text) + 1).ToString();
                }
                else if (Mil == 0)
                    TB_Edition1.Text = "";

                else if (Mil > 0)
                    TB_Edition1.Text = Mil.ToString();
            }
            else
                TB_Edition1.Text = "";
        }

        private void TB_NoSerie_TextChanged(object sender, EventArgs e)
        {
            //CheckEnabledNoSerie2();
            /*if (TB_NoSerie.Text != "" && TB_NoSerie.Text != "0" && !CBX_Serie_Coffret.Checked && !CBX_Serie_Integrale.Checked)
                TB_Serie_Contenu_1.Enabled = true;*/

            RewriteTitreFromSerieColl();
        }               


        private void TB_Serie_Integrale_TextChanged(object sender, EventArgs e)
        {
            if (TB_Serie_Integrale.Text == "1")
                CBX_Serie_Integrale.Checked = true;
            else
                CBX_Serie_Integrale.Checked = false;
        }

        private void TB_Coll_Coffret_TextChanged(object sender, EventArgs e)
        {
            if (TB_Coll_Coffret.Text == "1")
                CBX_Coll_Coffret.Checked = true;
            else
                CBX_Coll_Coffret.Checked = false;
        }

        private void TB_Serie_Coffret_TextChanged(object sender, EventArgs e)
        {
            if (TB_Serie_Coffret.Text == "1")
                CBX_Serie_Coffret.Checked = true;
            else
                CBX_Serie_Coffret.Checked = false;
        }

        private void TB_Coll_HorsSerie_TextChanged(object sender, EventArgs e)
        {
            if (TB_Coll_HorsSerie.Text == "1")
                CBX_Coll_HorsSerie.Checked = true;
            else
                CBX_Coll_HorsSerie.Checked = false;
        }

        private void TB_Serie_HorsSerie_TextChanged(object sender, EventArgs e)
        {
            if (TB_Serie_HorsSerie.Text == "1")
                CBX_Serie_HorsSerie.Checked = true;
            else
                CBX_Serie_HorsSerie.Checked = false;
        }

        #endregion

        #region Methodes MainForm ListBox

        private void LB_RefsChainees_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = LB_RefsChainees.SelectedIndex;

            if (i >= 0)
            {
                string Selected = LB_RefsChainees.Items[i].ToString();
                LB_RefsChainees.Items.Add(TB_EAN.Text);
                LB_RefsChainees.Items.Remove(Selected);
                TB_EAN.Text = Selected;
                TB_EAN.Focus();
                TB_Libelle.Focus();
            }
        }                       

        private void LB_Genres_DrawItem(object sender, DrawItemEventArgs e)
        {
            MyListBoxItem item = LB_Genres.Items[e.Index] as MyListBoxItem; // Get the current item and cast it to MyListBoxItem
            if (item != null)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.DrawBackground();
                    e.Graphics.FillRectangle(new SolidBrush(Color.Silver), e.Bounds);
                    e.DrawFocusRectangle();
                }
                else
                {
                    e.DrawBackground();
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
                    e.DrawFocusRectangle();
                }

                e.Graphics.DrawString(              // Draw the appropriate text in the ListBox
                    item.Message,                   // The message linked to the item
                    item.Fontstyle,                 // Take the font from the listbox
                    new SolidBrush(item.ItemColor), // Set the color 
                    0,                              // coordonnées pixel X
                    e.Index * LB_Genres.ItemHeight  // coordonnées pixel Y. Multiply the index by the ItemHeight defined in the listbox.
                );
            }
            else
            {
                // The item isn't a MyListBoxItem, do something about it
            }
        }

        #endregion

        #region Methodes MainForm DateTimePicker

        private void DTP_DateParution_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (dt > DateTime.Today.AddDays(1) && dt.ToString("dd/MM/yyyy") != "01/01/2070")
                TB_Dispo.Text = "2";

        }
        
        #endregion

        #region Methodes MainForm ComboBox

        private void CB_CodeSupport_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_CodeSupport.Text = DSupport[CB_CodeSupport.Text];
        }

        private void CB_TVA_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_CodeTVA.Text = DTVA[CB_TVA.Text];
        }

        private void CB_OBCode_TextChanged(object sender, EventArgs e)
        {
            if (!CB_OBCode.Items.Contains(CB_OBCode.Text))
                TB_OBCode.Text = "";
        }

        private void CB_Editeur_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Editeur.SelectedIndex >= 0)
            {
                string Editeur = CB_Editeur.Items[CB_Editeur.SelectedIndex].ToString();
                SetCollectionByEditeur(Editeur);
                SetDistriComboBox();
            }
        }        

        private void CB_Lectorat_TextChanged(object sender, EventArgs e)
        {
            if (CB_Lectorat.Items.Contains(CB_Lectorat.Text))
                TB_Lectorat.Text = DLectorat[CB_Lectorat.Text];
                        
            AlternateValues();
            
        }


        private void CB_Traducteur_TextChanged(object sender, EventArgs e)
        {            
            if (DTraducteur.ContainsKey(CB_Traducteur.Text))
                TB_IDTraducteur.Text = DTraducteur[CB_Traducteur.Text];
            else
                MessageBox.Show("traducteur inconnu ! " + CB_Traducteur.Text );

            AlternateValues();            
        }


        private void CB_Editeur_TextChanged(object sender, EventArgs e)
        {
            CB_Distri.Text = "";
            if (DEditeur.ContainsKey(CB_Editeur.Text))
            {
                CB_Editeur.SelectedIndex = CB_Editeur.Items.IndexOf(CB_Editeur.Text);
            }
            else
                CB_Editeur.SelectedIndex = -1;
        }
        

        private void CB_Style_TextChanged(object sender, EventArgs e)
        {
            if (CB_Style.Items.Contains(CB_Style.Text))
                TB_Style.Text = DStyles[CB_Style.Text];

            AlternateValues();
            
        }

        private void CB_OBCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_OBCode.Text = DOBCode[CB_OBCode.Text];
        }

        private void CB_Dispo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_Dispo.Text = DDispo[CB_Dispo.Text];
        }

        // Empeche la saisie dans les combobox
        private void CB_DisableEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        // modification de la longueur des combobox
        private void CB_Resize_Enter(object sender, EventArgs e)
        {
            (sender as Control).Width = 400;
        }

        #endregion

        #region Methodes MainForm Images

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            if (pictureBox3.Image == null)
                TT_Boutons.Show("absence de 4ème de couverture", (PictureBox)sender);
            else
            {
                TT_Boutons.Show("présence de 4ème de couverture", (PictureBox)sender);
                SetImageToPictureBox(pictureBox1, 4);
            }
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            SetImageToPictureBox(pictureBox1, 1);
        }

        // ouverture du formulaire ImageForm
        private void pictureBox_Click(object sender, EventArgs e)
        {
            IF = new ImageForm();

            IF.Text = this.Text;
            IF.TB_Picture_EAN.Text = TB_EAN.Text;

            IF.pictureBox1.Image = I;
            SetImageToPictureBox(IF.pictureBox2, 4);
            IF.Show();
        }

        #endregion

        #region Methodes MainForm Origine

        private void CB_Origine_TextChanged(object sender, EventArgs e)
        {
            if (DOrigine.ContainsKey(CB_Origine.Text))
                BT_OrigineValider.Enabled = true;
            else
                BT_OrigineValider.Enabled = false;

        }

        private void BT_OrigineValider_Click(object sender, EventArgs e)
        {
            ini.IniWriteValue("GENERAL", "DEFAULTUSER", CB_Origine.Text);

            this.Text += " : " + CB_Origine.Text;
            P_Saisie.Enabled = true;
            P_Origine.Hide();
            TB_EAN.Focus();
        }

        private void CB_Origine_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (BT_OrigineValider.Enabled)
                    BT_OrigineValider_Click(this, e);
            }
        }

        #endregion   

        #region Methodes MainForm Perte de Focus    
        
        private void CB_Resize_Leave(object sender, EventArgs e)
        {
            (sender as Control).Width = 128;
            CB_Combobox_Leave(sender, e);
        }

        private void CB_Combobox_Leave(object sender, EventArgs e)
        {
            (sender as ComboBox).SelectionLength = 0;
            MemorizerSet(sender, e);

            if (((ComboBox)sender).Name == "CB_Editeur")
            {
                SetDistriComboBox();
            }                 
        }
                
        private void CB_Lectorat_Leave(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "")
                TB_Lectorat.Text = "0";
            
            MemorizerSet(sender, e);
        }

        private void CB_SousCollection_Leave(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "")
            {
                TB_SousCollection.Text = "0";                
            }
            MemorizerSet(sender, e);
        }

        private void TB_Libelle_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TB_Libelle.Text.Trim()))
            {
                TB_Titre.Text = RemoveDiacritics(TB_Libelle.Text.ToUpper());
                RewriteTitreFromSerieColl();

                MemorizerSet(sender, e);
            }
            else
            {
                string MemoTitre = TB_Titre.Text;
                
                TB_Titre.Text = "";
                RewriteTitreFromSerieColl();
                if (TB_Titre.Text == "")
                    TB_Titre.Text = MemoTitre;
            }
            
        }

        private void TB_EAN_Leave(object sender, EventArgs e)
        {
            if (PreviousEAN != TB_EAN.Text)
            {
                if (TB_EAN.Text.Length == 9)
                    TB_EAN.Text = ISBN2CAB(TB_EAN.Text + "X");

                else if (TB_EAN.Text.Length == 10)
                    TB_EAN.Text = ISBN2CAB(TB_EAN.Text);

                else if (TB_EAN.Text.Length == 12)
                    TB_EAN.Text = CheckEAN(TB_EAN.Text);

                if (TB_EAN.Text.Length == 13)
                {
                    if (TB_EAN.Text == CheckEAN(TB_EAN.Text))
                        M.Set((Control)sender);
                    
                    //MessageBox.Show("TB_Ean_LEAVE\r\n"+ PreviousEAN+ "\r\n" + TB_EAN.Text+  "\r\n" + TB_EAN.Text.Length );
                    
                    PreviousEAN = TB_EAN.Text;
                    LoadFiche();
                }
                else
                {
                    //TB_EAN.Focus();
                }
            }
            PreviousEAN = TB_EAN.Text;
        }               

        #endregion

        #region Methodes MainForm Boutons

        private void BT_Valider_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            bool _Work = false;

            float prix;
            int cl;
            string codelangue = TB_Langue_Concat.Text;

            string LibEditeur = CB_Editeur.Text;
            
            while (codelangue.Length < 5)
                codelangue = codelangue + "0";

            bool clok = int.TryParse(TB_Langue_Concat.Text, out cl);
            bool prixok = float.TryParse(TB_PrixEuro.Text.Replace('.', ','), out prix);

            if (CB_SousCollection.Text == "" && TB_NoSColl.Text != "0")
            {
                TB_NoSColl.Text = "0";
            }

            if (CB_Collection.Text == "" && TB_Nocoll.Text != "0")
                TB_Nocoll.Text = "0";

            if (TB_Coll_Contenu_1.Text == "")
                TB_Coll_Contenu_1.Text = "0";

            if (TB_NoSerie.Text == "")
                TB_NoSerie.Text = "0";

            if (TB_Serie_Contenu_1.Text == "")
                TB_Serie_Contenu_1.Text = "0";

            if (TB_Coll_Contenu_2.Text == "")
                TB_Coll_Contenu_2.Text = "0";

            if (TB_Serie_Contenu_2.Text == "")
                TB_Serie_Contenu_2.Text = "0";

            if (string.IsNullOrEmpty(TB_Lectorat.Text))
            {
                TB_Lectorat.Text = "0";
            }
            
            //verification des champs obligatoires.            
            if (TB_GenrePrincipal.Text == "" || TB_GenrePrincipal.Text == "00-00-00-00")
                MessageBox.Show("Aucun code genre principal défini !");

            else if (TB_IDCollection.Text == "")
                MessageBox.Show("Erreur de collection !");

            /*else if (TB_SousCollection.Text == "")
                MessageBox.Show("Erreur de sous collection !");*/

            else if (TB_Titre.Text == "")
                MessageBox.Show("Pas de titre !");

            else if (TB_Auteurs.Text == "")
                MessageBox.Show("Pas d'auteur ! ");

            else if (!prixok)
                MessageBox.Show("prix invalide !\r\n" + TB_PrixEuro.Text);

            else if (string.IsNullOrEmpty(TB_Dispo.Text))
            {
                MessageBox.Show("pas de code dispo !");
            }

            else if (string.IsNullOrEmpty(TB_CodeSupport.Text))
            {
                MessageBox.Show("pas de code support !");
            }

            /*else if (string.IsNullOrEmpty(TB_OBCode.Text) && !string.IsNullOrEmpty(TB_CodeCSR.Text))
            {
                MessageBox.Show("pas de code OB !");
            }*/
            
            else if (!DDispo.ContainsKey(CB_Dispo.Text))
                MessageBox.Show("disponibilité incorrecte");

            else if ((!clok) ||
                     (!(DCL_Langue_0.ContainsValue(TB_Langue_Concat.Text) ||
                        DCL_Langue_1.ContainsValue(TB_Langue_Concat.Text) ||
                        DCL_Langue_2.ContainsValue(TB_Langue_Concat.Text))
                     ) && TB_Langue_Concat.Text != "0")
            {
                MessageBox.Show("le code langue est incorrect");
            }


            else if (TB_Edition1.Text != "" && TB_Edition2.Text != "")
            {
                if ((Convert.ToInt32(TB_Edition1.Text) + 1).ToString() != TB_Edition2.Text)
                    MessageBox.Show("Incohérence entre les années d'édition !");
                else
                    _Work = true;
            }


            /*
             * MultiDistri = true;
             * si DMDistri contient le distributeur à enregistrer +
             * le distributeur à enregistrer != TB_MultiDistri_Selected.Text
             */
            else if ((MultiDistri) &&
                     (DMDistri.ContainsValue(CB_Distri.Text)) &&
                     (TB_MultiDistri_Selected.Text != "") &&
                     (CB_Distri.Text != TB_MultiDistri_Selected.Text))
            {
                MessageBox.Show("la référence liée à ce distributeur existe déjà !");
            }


            else if (!CB_Editeur.Items.Contains(CB_Editeur.Text) || CB_Editeur.Text == "")
            {
                DialogResult DR = MessageBox.Show("Editeur inconnu dans l'annuaire !\r\nValider la fiche avec cet éditeur ?", "Confirmation", MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    _Work = true;
                }
            }

            else if (!DEditeur[CB_Editeur.Text].ContainsValue(CB_Distri.Text))
            {
                DialogResult DR = MessageBox.Show("Distributeur différent de celui connu dans l'annuaire !\r\nValider la fiche avec ce distributeur?", "Confirmation", MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    _Work = true;
                }
            }

            else if (prix == 0f)
            {
                DialogResult DR = MessageBox.Show("Attention le prix est à 0 !\r\nValider la fiche avec ce prix ?", "Confirmation", MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    _Work = true;
                }
            }

            //9780375428340

            else
                _Work = true;


            if (_Work)
            {
                //on reconstitue le millésime si nécessaire.
                string Millesime = "0";
                if (TB_Edition1.Text != "")
                {
                    if (Convert.ToInt32(TB_Edition1.Text) < 1000)                    
                        Millesime = TB_Edition1.Text;
                    
                    else if (TB_Edition2.Text != "")                    
                        Millesime = "-" + TB_Edition1.Text;
                    
                    else
                        Millesime = TB_Edition1.Text;
                }

                //equivalence libellé editeur ! 
                if (DEdiFullToEdi.ContainsKey(LibEditeur))                
                    LibEditeur = DEdiFullToEdi[LibEditeur];
                  
                string cab = TB_EAN.Text;

                //construction de la requete de MAJ
                if (!Found)
                {
                    //création de réf, insert into livre + recuperation de l'ID (à passer dans TB_IDLivre.Text) 
                    //pour alimenter les tables T_GENRES / PRESENTATIONS / SOMMAIRES
                    //ensuite détection des éventuelles réfs chainées pour MAJ des genres. a confirmer par une alerte eventuellement
                    string QueryInsertLivre = string.Format(SaisieLivre.Properties.Resources.QueryInsertLivre,
                                                             cab,
                                                             ReplaceOfficeChars(TB_Titre.Text).Replace("'", "''"),
                                                             ReplaceOfficeChars(TB_Libelle.Text).Replace("'", "''"),
                                                             TB_Lectorat.Text,
                                                             LibEditeur.Replace("'", "''"), //l'équivalence editeur pour la sauvegarde
                                                             CB_Distri.Text.Replace("'", "''"),
                                                             CB_Collection.Text.Replace("'", "''"),
                                                             TB_IDCollection.Text,
                                                             TB_PrixEuro.Text.Replace(",", "."),
                                                             TB_CodeTVA.Text,
                                                             ReplaceOfficeChars(TB_Auteurs.Text).Replace("'", "''"),
                                                             DTP_DateParution.Value.ToString("MM/dd/yyyy"),
                                                             TB_CodeSupport.Text,
                                                             TB_IDTraducteur.Text,
                                                             TB_Style.Text,
                                                             TB_Langue.Text,
                                                             TB_IDTraduitEn.Text,
                                                             codelangue,
                                                             TB_Dispo.Text,
                                                             TB_IAD.Text,
                                                             ReplaceOfficeChars(TB_Commentaire.Text).Replace("'", "''"),
                                                             TB_Poids.Text,
                                                             TB_NBPages.Text,
                                                             TB_Hauteur.Text,
                                                             TB_Largeur.Text,
                                                             CB_Origine.Text,
                                                             TB_Relié.Text,
                                                             TB_Broché.Text,
                                                             TB_LivreLu.Text,
                                                             TB_GrandsCara.Text,
                                                             TB_Multilingue.Text,
                                                             TB_OBCode.Text,
                                                             TB_SousCollection.Text,
                                                             TB_NoSColl.Text,
                                                             TB_Nocoll.Text, 
                                                             TB_Luxe.Text,
                                                             TB_Illustre.Text, 
                                                             Millesime,
                                                             TB_IDLibSerie.Text,
                                                             TB_NoSerie.Text,
                                                             TB_Serie_Contenu_1.Text,
                                                             TB_Serie_Integrale.Text,
                                                             TB_Serie_Coffret.Text,
                                                             TB_Serie_HorsSerie.Text,                                                             
                                                             TB_Coll_Coffret.Text,
                                                             TB_Coll_HorsSerie.Text,
                                                             TB_Coll_Contenu_1.Text,
                                                             TB_Coll_Contenu_2.Text,
                                                             TB_Serie_Contenu_2.Text,
                                                             TB_Cartonne.Text);

                    db.Query(QueryInsertLivre);

                    db.Query(string.Format("select id from livre where gencod='{0}'", cab));
                    foreach (var row in db.FetchAll())
                    {
                        TB_IDLivre.Text = row["id"].ToString();
                        break;
                    }

                    maj_table_by_idlivre(TB_IDLivre.Text, "presentation", TB_Resume.Text);

                    maj_table_by_idlivre(TB_IDLivre.Text, "sommaire", TB_Sommaire.Text);

                    WorkGenres("", TB_IDLivre.Text, false);

                    WorkAuteurFonctions();

                    InsertOrgCab();
                }
                else
                {
                    string QueryUpdateLivre = string.Empty;


                    //si la textbox est vide, on force un zero pour la condition ci dessous
                    if (TB_MultiDistri_ID.Text == "")
                        TB_MultiDistri_ID.Text = "0";

                    //la config autorise la multi distribution +
                    //le distributeur en cours est différent de celui par défaut de la table livre. +                    
                    if (MultiDistri && Convert.ToInt32(TB_MultiDistri_ID.Text) > 0)
                    {

                        //1 queryupdate livre est retravaillée pour ne modifier que les champs "oeuvre"
                        QueryUpdateLivre = string.Format(SaisieLivre.Properties.Resources.QueryUpdateLivreAlternate,
                                                         ReplaceOfficeChars(TB_Titre.Text).Replace("'", "''"),
                                                         ReplaceOfficeChars(TB_Libelle.Text).Replace("'", "''"),
                                                         TB_Lectorat.Text,
                                                         TB_CodeTVA.Text,
                                                         ReplaceOfficeChars(TB_Auteurs.Text).Replace("'", "''"),
                                                         TB_CodeSupport.Text,
                                                         TB_IDTraducteur.Text,
                                                         TB_Style.Text,
                                                         TB_Langue.Text,
                                                         TB_IDTraduitEn.Text,
                                                         codelangue,
                                                         TB_IAD.Text,
                                                         ReplaceOfficeChars(TB_Commentaire.Text).Replace("'", "''"),
                                                         TB_Poids.Text,
                                                         TB_NBPages.Text,
                                                         TB_Hauteur.Text,
                                                         TB_Largeur.Text,
                                                         CB_Origine.Text,
                                                         TB_Relié.Text,
                                                         TB_Broché.Text,
                                                         TB_LivreLu.Text,
                                                         TB_GrandsCara.Text,
                                                         TB_Multilingue.Text,
                                                         TB_OBCode.Text,
                                                         TB_SousCollection.Text,
                                                         TB_NoSColl.Text,
                                                         TB_Nocoll.Text,
                                                         TB_IDLivre.Text);

                        //on a cliqué sur le bouton ajouter +
                        //le distributeur en cours 
                        if (TB_MultiDistri_Selected.Text != CB_Distri.Text)
                        {

                            //2 on passe une requête supplémentaire pour modifier les données "produit"
                            string QueryInsertMD = string.Format(SaisieLivre.Properties.Resources.QueryInsertMultiDistri,
                                                                 cab,
                                                                 CB_Distri.Text.Replace("'", "''"),
                                                                 DEdiCodeEdi[CB_Editeur.Text], //le libellé de la combobox pour interroger les dictionnaires
                                                                 TB_IDCollection.Text,
                                                                 TB_Nocoll.Text,
                                                                 TB_PrixEuro.Text.Replace(",", "."),
                                                                 DTP_DateParution.Value.ToString("MM/dd/yyyy"),
                                                                 TB_Dispo.Text);

                            //on execute la requête sur la table T_MULTIDISTRI
                            db.Query(QueryInsertMD);
                        }

                        else if (TB_MultiDistri_Selected.Text != TB_MainDistri.Text)
                        {
                            //2 on passe une requête supplémentaire pour modifier les données "produit"
                            string QueryUpdateMD = string.Format(SaisieLivre.Properties.Resources.QueryUpdateMultiDistri,
                                                                CB_Distri.Text.Replace("'", "''"),
                                                                DEdiCodeEdi[CB_Editeur.Text], //le libellé de la combobox pour interroger les dictionnaires
                                                                TB_IDCollection.Text,
                                                                TB_Nocoll.Text,
                                                                TB_PrixEuro.Text.Replace(",", "."),
                                                                DTP_DateParution.Value.ToString("MM/dd/yyyy"),
                                                                TB_Dispo.Text,
                                                                TB_MultiDistri_ID.Text,
                                                                 CB_Collection.Text.Replace("'", "''"));

                            //on execute la requête sur la table T_MULTIDISTRI
                            db.Query(QueryUpdateMD);
                        }
                    }

                    else
                    {


                        //MAJ de la fiche Livre //Replace("’", "''")
                        QueryUpdateLivre = string.Format(SaisieLivre.Properties.Resources.QueryUpdateLivre,
                                                                 ReplaceOfficeChars(TB_Titre.Text).Replace("'", "''"),
                                                                 ReplaceOfficeChars(TB_Libelle.Text).Replace("'", "''"),
                                                                 TB_Lectorat.Text,
                                                                 LibEditeur.Replace("'", "''"), //l'equivalence pour la sauvegarde
                                                                 CB_Distri.Text.Replace("'", "''"),
                                                                 CB_Collection.Text.Replace("'", "''"),
                                                                 TB_IDCollection.Text,
                                                                 TB_PrixEuro.Text.Replace(",", "."),
                                                                 TB_CodeTVA.Text,
                                                                 ReplaceOfficeChars(TB_Auteurs.Text).Replace("'", "''"),
                                                                 DTP_DateParution.Value.ToString("MM/dd/yyyy"),
                                                                 TB_CodeSupport.Text,
                                                                 TB_IDTraducteur.Text,
                                                                 TB_Style.Text,
                                                                 TB_Langue.Text,
                                                                 TB_IDTraduitEn.Text,
                                                                 codelangue,
                                                                 TB_Dispo.Text,
                                                                 TB_IAD.Text,
                                                                 ReplaceOfficeChars(TB_Commentaire.Text).Replace("'", "''"),
                                                                 TB_Poids.Text,
                                                                 TB_NBPages.Text,
                                                                 TB_Hauteur.Text,
                                                                 TB_Largeur.Text,
                                                                 CB_Origine.Text,
                                                                 TB_Relié.Text,
                                                                 TB_Broché.Text,
                                                                 TB_LivreLu.Text,
                                                                 TB_GrandsCara.Text,
                                                                 TB_Multilingue.Text,
                                                                 TB_OBCode.Text,
                                                                 TB_SousCollection.Text,
                                                                 TB_NoSColl.Text,
                                                                 TB_Nocoll.Text,
                                                                 TB_IDLivre.Text,
                                                                 TB_Luxe.Text,
                                                                 TB_Illustre.Text,
                                                                 Millesime,
                                                                 TB_IDLibSerie.Text,
                                                                 TB_NoSerie.Text,
                                                                 TB_Serie_Contenu_1.Text,
                                                                 TB_Serie_Integrale.Text,
                                                                 TB_Serie_Coffret.Text,
                                                                 TB_Serie_HorsSerie.Text,                                                                 
                                                                 TB_Coll_Coffret.Text,
                                                                 TB_Coll_HorsSerie.Text,
                                                                 TB_Coll_Contenu_1.Text,
                                                                 TB_Coll_Contenu_2.Text,
                                                                 TB_Serie_Contenu_2.Text,
                                                                 TB_Cartonne.Text);
                    }

                    //Clipboard.SetText(QueryUpdateLivre);
                    //MessageBox.Show(QueryUpdateLivre);
                    db.Query(QueryUpdateLivre);
                }
                
                //MAJ du sommaire si nécessaire
                if (PreviousSommaire != TB_Sommaire.Text)
                {
                    maj_table_by_idlivre(TB_IDLivre.Text, "sommaire", TB_Sommaire.Text);
                }


                //MAJ du résumé si nécessaire
                if (PreviousResume != TB_Resume.Text)
                {
                    maj_table_by_idlivre(TB_IDLivre.Text, "presentation", TB_Resume.Text);
                }

                WorkAuteurFonctions();

                //on force la maj du genre si il s'agit d'une valeur issue d'une correspondance au chargement de la fiche.
                if (TB_ForceMajGenre.Text == "1")
                    PreviousGenre = "";

                WorkGenres(PreviousGenre, TB_IDLivre.Text);

                InsertOrgCab();
                
                //CHAINAGE
                string MainTitre = TB_Titre.Text.Replace("'", "''");
                string MainAuteur = TB_Auteurs.Text.Replace("'", "''");
                string MainLibelle = TB_Libelle.Text.Replace("'", "''");
                string MainCodeLangue = codelangue;
                string MainStyle = TB_Style.Text;
                string MainLectorat = TB_Lectorat.Text;
                string MainGenre = GetGenresFromListBox();

                #region mise a jour des references chainées

                if (CBX_SupprChainage.Enabled && !CBX_SupprChainage.Checked)
                {
                    string QSelChain = SaisieLivre.Properties.Resources.QuerySelectChainage;

                    Dictionary<string, Row> Chaine = new Dictionary<string, Row>();
                    foreach (string item in LB_RefsChainees.Items)
                    {
                        db.Query(string.Format(QSelChain, item));

                        foreach (var row in db.FetchAll())
                            Chaine.Add(item, row);
                    }


                    foreach (var pair in Chaine)
                    {
                        if ((pair.Value["titre"].ToString().Replace("'", "''") != MainTitre) ||
                            (pair.Value["auteurs"].ToString().Replace("'", "''") != MainAuteur) ||
                            (pair.Value["libelle"].ToString().Replace("'", "''") != MainLibelle) ||
                            (pair.Value["codelangue"].ToString() != MainCodeLangue) ||
                            (pair.Value["id_style"].ToString() != MainStyle) ||
                            (pair.Value["idlectorat"].ToString() != MainLectorat) || 
                            (pair.Value["returngenre"].ToString() != MainGenre))
                        {
                            string QMajChain = string.Format(SaisieLivre.Properties.Resources.QueryUpdateChainage,
                                                             MainTitre,
                                                             MainAuteur,
                                                             MainLibelle,
                                                             MainCodeLangue,
                                                             MainLectorat,
                                                             MainStyle,
                                                             pair.Key);
                            db.Query(QMajChain);
                        }

                        //maj des genres
                        WorkGenres(pair.Value["returngenre"].ToString(), pair.Value["id"].ToString());

                        //maj des fonctions auteurs
                        WorkAuteurFonctions();

                        //MAJ du résumé si nécessaire
                        if ((Convert.ToInt32(pair.Value["lenpresentation"]) > 0) &&
                            (TB_Resume.Text != pair.Value["presentation"].ToString()) &&
                            (TB_Resume.Text != ""))
                        {

                            maj_table_by_idlivre(pair.Value["id"].ToString(), "presentation", TB_Resume.Text);
                        }
                    }
                }

                #endregion

                M.SetAll(this);

                TB_EAN.Text = "";
                PreviousEAN = "";

                SD.Hide();
                //CD.Hide();
                TB_EAN.Focus();

                ResetMDLABELColors();

                if (AddMultiDistri)
                {
                    LB_RefsChainees.Enabled = true;
                    LB_Saisie_EAN.Enabled = true;
                }
                                
                bool is_chained = false;
                int LBSidx = LB_Saisie_EAN.SelectedIndex;                                
                if (LB_Saisie_EAN.Items.Count > 0 && LBSidx >= 0)
                    is_chained = LB_RefsChainees.Items.Contains(LB_Saisie_EAN.Items[LBSidx]);
                
                ClearFiche(false, true);

                BT_Valider.Enabled = false;
                BT_Annuler.Enabled = false;

                try
                {
                    if ((LB_Saisie_EAN.Items.Count > 0) &&
                        (is_chained ||
                         cab == LB_Saisie_EAN.Items[LBSidx].ToString().Replace("-", "")))
                    {
                        LB_Saisie_EAN.Items.RemoveAt(LBSidx);

                        //il reste des elements dans la liste, on les charge
                        if (LB_Saisie_EAN.Items.Count > 0)
                        {
                            LB_Saisie_EAN.SelectedIndex = LBSidx;
                        }
                        else
                            BT_FlushListe.Enabled = false;

                        //actualisation du compteur
                        SetCompteurListeSaisie();
                    }
                }
                catch
                {
                }
                 
            }

            Cursor = Cursors.Arrow;
        }

        private void InsertOrgCab()
        {
            string q = string.Format("INSERT INTO ORGCAB ( ORIGINE, GENCOD, LATESTUPDATE, APPLICATION ) " +
                                   "VALUES ( '{0}', '{1}', 'now', 0 )", CB_Origine.Text, TB_EAN.Text);
            db.Query(q);
        }


        private void BT_Font_Click(object sender, EventArgs e)
        {
            DialogResult result = fontDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                Font font = fontDialog1.Font;

                LoadedFont = font;
                this.Font = font;

                CD.Font = LoadedFont;
                SD.Font = LoadedFont;

                ini.IniWriteValue("FONT", "Name", this.Font.Name);
                ini.IniWriteValue("FONT", "Size", this.Font.Size.ToString());
                ini.IniWriteValue("FONT", "Style", this.Font.Style.ToString());

            }
        }
        
        private void BT_AddMultiDistri_Click(object sender, EventArgs e)
        {
            AddMultiDistri = true;
            BT_AddMultiDistri.Enabled = false;
            LB_RefsChainees.Enabled = false;
            LB_Saisie_EAN.Enabled = false;
            ResetMDLABELColors();
        }

        // annulation des modifications d'une fiche
        private void BT_Annuler_Click(object sender, EventArgs e)
        {
            M.SetAll(this);

            TB_EAN.Text = "";
            PreviousEAN = "";

            SD.Hide();
            //CD.Hide();

            TB_EAN.Focus();

            ResetMDLABELColors();

            if (AddMultiDistri)
            {
                LB_RefsChainees.Enabled = true;
                LB_Saisie_EAN.Enabled = true;
            }

            ClearFiche(false, true);

            BT_Valider.Enabled = false;
            BT_Annuler.Enabled = false;
        }

        // ouverture du formulaire secondaire Traducteurs.
        private void BT_Trad_Click(object sender, EventArgs e)
        {
            if (BT_Trad.Text == "Trad (F2) :")
            {
                TradForm TF = new TradForm(DTraducteur, TB_IDTraducteur, db);
                TF.CB_SelTraducteur.Text = CB_Traducteur.Text;
                TF.Font = LoadedFont;
                TF.ShowDialog();
            }
            else
                LBClickAlternate(CB_Traducteur, BT_Trad); 
        }

        private void CallAuteursForm()
        {
            if (TB_Auteurs.Text != "")
            {
                AuteursForm AF = new AuteursForm(DLangue, DPays, DFunctionAuteurs);
                AF.Font = LoadedFont;
                AF.TB_Auteurs_From_Main.Text = TB_Auteurs.Text;
                AF.db = db;
                AF.mf = this;
                AF.ShowDialog();
            }
        }

        // ouverture du formulaire secondaire Auteurs.
        private void BT_Auteurs_Click(object sender, EventArgs e)
        {
            if (BT_Auteurs.Text == "Auteurs (F2) :")
                CallAuteursForm();
            else
                LBClickAlternate(TB_Auteurs, BT_Auteurs); 
            
        }

        // duplication du titre en minuscule dans le champ Libelle
        private void BT_Libelle_Click(object sender, EventArgs e)
        {
            if (TB_Titre.Text != "")
            {
                string titre = TB_Titre.Text;

                if (TB_Info_Serie.Text != "")
                    titre = titre.Replace(TB_Info_Serie.Text, "");

                if (TB_Info_Coll.Text != "")
                    titre = titre.Replace(TB_Info_Coll.Text, "");

                TB_Libelle.Text = titre.ToLower();
            }
        }

        #endregion
    
        #region Methodes MainForm Collection

        private void CB_Collection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DCollection.ContainsKey(CB_Collection.Text))
                TB_IDCollection.Text = DCollection[CB_Collection.Text]["id"].ToString(); 
        }

        private void TB_IDCollection_TextChanged(object sender, EventArgs e)
        {
            if (DCollection.ContainsKey(CB_Collection.Text))
            {                
                ChoixCollection(CB_Collection);            
            }
        }
              
        private void CB_Collection_TextChanged(object sender, EventArgs e)
        {            
         
            RemoveCollectionFromTitre();            
            TB_Info_Coll.Text = string.Empty;
            TB_PresentTitre.Text = "0";
            TB_Coll_Contenu_1.Enabled = false;
            TB_Coll_Contenu_2.Enabled = false;
       
            if (CB_Collection.Text == "" || !CB_Collection.Items.Contains(CB_Collection.Text))
            {
                TB_IDCollection.Text = "0";
                TB_SousCollection.Text = "0";
                TB_Nocoll.Text = "0";
                TB_NoSColl.Text = "0";

                CollectionRow = null;
                ResetMDLABELColors();                
                
                TB_Nocoll.Enabled = false;
                CBX_Coll_HorsSerie.Enabled = false;
                CBX_Coll_Coffret.Enabled = false;
                ((ComboBox)sender).ForeColor = Color.OrangeRed;
            }
            
            else
            {
                TB_Nocoll.Enabled = true;
                CBX_Coll_HorsSerie.Enabled = true;
                CBX_Coll_Coffret.Enabled = true;
                CB_Collection.SelectedIndex = CB_Collection.Items.IndexOf(CB_Collection.Text);                                 
                ((ComboBox)sender).ForeColor = SystemColors.WindowText;
            }

            if (TB_PresentTitre.Text == "1" && TB_Libelle.Text.Trim() == "")
            {
                TB_Libelle_Leave(this, e);
                CB_Collection.Focus();
            }

            RewriteTitreFromSerieColl();
            AlternateValues();            
        }

        #region sous collection, masqué depuis v61
        private void CB_SousCollection_TextChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Text == "")
                TB_SousCollection.Text = "0";

            else if (((ComboBox)sender).Items.Contains(((ComboBox)sender).Text))
            {
                ((ComboBox)sender).ForeColor = SystemColors.WindowText;
            }
        }

        private void CB_SousCollection_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if (DSousCollection.ContainsKey(((ComboBox)sender).Text))
            {               
                TB_SousCollection.Text = DSousCollection[((ComboBox)sender).Text].ToString();
            }            
        }

        private void TB_SousCollection_TextChanged(object sender, EventArgs e)
        {                                   
            CB_SousCollection.Text = FindIdByLib(DSousCollection, TB_SousCollection.Text);
        }
        #endregion

        private void TB_Nocoll_TextChanged(object sender, EventArgs e)
        {
            //CheckEnabledNoColl2();    
            /*if (TB_Nocoll.Text != "" && TB_Nocoll.Text != "0" && !CBX_Coll_Coffret.Checked)
                TB_Coll_Contenu_1.Enabled = true;*/

            RewriteTitreFromSerieColl();                    
        }
        #endregion

        #region Methodes MainForm AlternateValuesCollection

        private void TB_GenrePrincipal_TextChanged(object sender, EventArgs e)
        {
            AlternateValues();
            
        }

        private void TB_CodeSupport_TextChanged(object sender, EventArgs e)
        {                      
            ForceUpperTB((TextBox)sender);
            FindIdByLib(DSupport, TB_CodeSupport.Text, CB_CodeSupport);

            if ((TB_CodeSupport.Text == "R" || TB_CodeSupport.Text == "C") && TB_Titre.Text.Contains(" T."))
            {
                TB_Titre.Text = TB_Titre.Text.Replace(" T.", " N.");
                
                if (TB_Info_Coll.Text != "")
                    TB_Info_Coll.Text = TB_Info_Coll.Text.Replace(" T.", " N.");

                if (TB_Info_Serie.Text != "")
                    TB_Info_Serie.Text = TB_Info_Serie.Text.Replace(" T.", " N.");

            }

            else if (TB_CodeSupport.Text != "R" && TB_CodeSupport.Text != "C" && TB_Titre.Text.Contains(" N."))
            {
                TB_Titre.Text = TB_Titre.Text.Replace(" N.", " T.");

                if (TB_Info_Coll.Text != "")
                    TB_Info_Coll.Text = TB_Info_Coll.Text.Replace(" N.", " T.");

                if (TB_Info_Serie.Text != "")
                    TB_Info_Serie.Text = TB_Info_Serie.Text.Replace(" N.", " T.");

            }
            AlternateValues();
        }

        private void TB_Hauteur_TextChanged(object sender, EventArgs e)
        {
            AlternateValues();
        }

        private void TB_Largeur_TextChanged(object sender, EventArgs e)
        {
            AlternateValues();
        }

        private void TB_NBPages_TextChanged(object sender, EventArgs e)
        {
            AlternateValues();
        }

        private void TB_Libelle_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TB_Libelle.Text))
            {
                if (TB_Libelle.TextLength < CB_Collection.Text.Length)
                    TB_Libelle.ForeColor = SystemColors.ControlText;
                else
                {
                    if ((!string.IsNullOrEmpty(CB_Collection.Text)) && (RemoveDiacritics(TB_Libelle.Text.ToUpper().Substring(0, CB_Collection.Text.Length)) == CB_Collection.Text))
                        TB_Libelle.ForeColor = Color.Red;

                    else if (!string.IsNullOrEmpty(CB_Serie.Text) && TB_Libelle.TextLength>CB_Serie.Text.Length && (RemoveDiacritics(TB_Libelle.Text.ToUpper().Substring(0, CB_Serie.Text.Length)) == CB_Serie.Text.ToUpper()))
                        TB_Libelle.ForeColor = Color.Red;

                    else
                        TB_Libelle.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void TB_Auteurs_TextChanged(object sender, EventArgs e)
        {
            AlternateValues();
        }

        private void TB_OBCode_TextChanged(object sender, EventArgs e)
        {            
            FindIdByLib(DOBCode, TB_OBCode.Text, CB_OBCode);
            AlternateValues();
        }

        #endregion

        #region Methodes MainForm Langues

        private void TB_Langue_0_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DCL_Langue_0, TB_Langue_0.Text, CB_Langue_0);

            if (TB_Langue_0.Text != "0")
                TB_Langue_Concat.Text = TB_Langue_0.Text;
        }

        private void TB_Langue_1_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DCL_Langue_1, TB_Langue_1.Text, CB_Langue_1);

            if (TB_Langue_1.Text != "0")
                TB_Langue_Concat.Text = TB_Langue_1.Text;
        }

        private void TB_Langue_2_TextChanged(object sender, EventArgs e)
        {
            FindIdByLib(DCL_Langue_2, TB_Langue_2.Text, CB_Langue_2);

            if (TB_Langue_2.Text != "0")
                TB_Langue_Concat.Text = TB_Langue_2.Text;
        }

        //relation code langue / combobox
        private void TB_Langue_Concat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int LCL = TB_Langue_Concat.Text.Length;
                switch (LCL)
                {
                    case 5:
                    case 4:
                        TB_Langue_0.Text = TB_Langue_Concat.Text.Substring(0, 1);
                        TB_Langue_1.Text = TB_Langue_Concat.Text.Substring(0, 3);
                        TB_Langue_2.Text = TB_Langue_Concat.Text.Substring(0, LCL);

                        if (TB_Langue_2.Text.Length < 5)
                            CB_Langue_2.Text = "";
                        break;
                    
                    case 3:
                    case 2:
                        TB_Langue_0.Text = TB_Langue_Concat.Text.Substring(0, 1);
                        TB_Langue_1.Text = TB_Langue_Concat.Text.Substring(0, LCL);

                        if (TB_Langue_1.Text.Length < 3)
                            CB_Langue_1.Text = "";

                        TB_Langue_2.Text = "0";
                        CB_Langue_2.Text = "";
                        break;

                    case 1:
                        TB_Langue_0.Text = TB_Langue_Concat.Text;
                        TB_Langue_1.Text = "0";
                        CB_Langue_1.Text = "";
                        TB_Langue_2.Text = "0";
                        CB_Langue_2.Text = "";
                        break;

                    case 0:
                        TB_Langue_0.Text = "0";
                        CB_Langue_0.Text = "";
                        TB_Langue_1.Text = "0";
                        CB_Langue_1.Text = "";
                        TB_Langue_2.Text = "0";
                        CB_Langue_2.Text = "";
                        break;                   

                }
            }
            catch
            { }

            AlternateValues();
            
        }        

        private void CB_Langue_TextChanged(object sender, EventArgs e)
        {
            if (CB_Langue.Items.Contains(CB_Langue.Text))
                TB_Langue.Text = DLangue[CB_Langue.Text];

            AlternateValues();
            
        }
       
        private void CB_TraduitEn_TextChanged(object sender, EventArgs e)
        {
            if (CB_TraduitEn.Items.Contains(CB_TraduitEn.Text))
                TB_IDTraduitEn.Text = DLangue[CB_TraduitEn.Text];

            AlternateValues();
            
        }
     
        private void CB_Langue_0_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_Langue_0.Text = DCL_Langue_0[CB_Langue_0.Text];
            
            CB_Langue_1.Items.Clear();
            DCL_Langue_1.Clear();

            CB_Langue_2.Items.Clear();
            DCL_Langue_2.Clear();

            db.Query(string.Format("select id, libelle from CL_Langue_1 where id starting '{0}'", TB_Langue_0.Text));
            foreach (Row row in db.FetchAll())
            {
                CB_Langue_1.Items.Add(row["libelle"].ToString());
                DCL_Langue_1.Add(row["libelle"].ToString(), row["id"].ToString());
            }
        }

        private void CB_Langue_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_Langue_1.Text = DCL_Langue_1[CB_Langue_1.Text];
            if (CB_Langue_1.Text == "")
                TB_Langue_1.Text = "";

            CB_Langue_2.Items.Clear();
            DCL_Langue_2.Clear();

            db.Query(string.Format("select id, libelle from CL_Langue_2 where id starting '{0}'", TB_Langue_1.Text));
            foreach (Row row in db.FetchAll())
            {
                CB_Langue_2.Items.Add(row["libelle"].ToString());
                DCL_Langue_2.Add(row["libelle"].ToString(), row["id"].ToString());
            }
        }

        private void CB_Langue_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_Langue_2.Text = DCL_Langue_2[CB_Langue_2.Text];
            if (CB_Langue_2.Text == "")
                TB_Langue_2.Text = "";
        }

        #endregion
        
        #region Methodes MainForm traitement des images

        /// <summary>
        /// affichage des vignettes
        /// </summary>
        /// <param name="PB"></param>
        /// <param name="ImageType"></param>
        /// <returns></returns>
        
        private void TB_Image_TextChanged(object sender, EventArgs e)
        {
            if (!CBX_desactivervisuel.Checked)
                I = SetImageToPictureBox(pictureBox1, 1);
            else
                pictureBox1.Image = null;
        }

        private void TB_Image_4_TextChanged(object sender, EventArgs e)
        {
            if (!CBX_desactivervisuel.Checked)
            {
                I4 = SetImageToPictureBox(pictureBox2, 4);
                if (((TextBox)sender).Text == "1")
                    pictureBox3.Image = Properties.Resources.picto_visuel;
                else
                    pictureBox3.Image = null;
            }
            else
                pictureBox3.Image = null;

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (TB_Image.Text=="2")
            {
                int wsize = pictureBox1.Width;
                e.Graphics.DrawImage(Properties.Resources.panneau_temporaire_ak14_sans, 0, 0, wsize, wsize);
            }
        } 

        #endregion

        #region Methodes MainForm Liste d'ean à traiter

        private void BT_Saisie_LoadListe_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.Filter = "Fichier Texte|*.txt";
            ofd.Title = "Charger une liste d'EAN";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                //TB_FileName.Text = ofd.FileName;
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            string S = Regex.Replace(line, "[^X0-9]", "");
                            LB_Saisie_EAN.Items.Add(S);
                        }
                    }
                }

                //alimente les textbox
                if (LB_Saisie_EAN.Items.Count > 0)
                {
                    LB_Saisie_EAN.SelectedIndex = 0;
                    BT_FlushListe.Enabled = true;
                }                
                else
                    BT_FlushListe.Enabled = false;
            }

            SetCompteurListeSaisie();
        }       

        private void BT_Saisie_LoadListe_MouseHover(object sender, EventArgs e)
        {
            TT_Boutons.Show("Ouvrir une liste d'EAN/ISBN", (Button)sender);
        }

        private void BT_GenereListe_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            LB_Saisie_EAN.Items.Clear();

            GenListeForm GLF = new GenListeForm(LB_Saisie_EAN, db, DEditeur, DSupport);
            GLF.Font = LoadedFont;
            GLF.ShowDialog();

            //alimente les textbox
            if (LB_Saisie_EAN.Items.Count > 0)
            {
                int idx = LB_Saisie_EAN.SelectedIndex;

                if (LB_Saisie_EAN.Items.Count - 1 > idx)
                    idx = idx + 1;
                else
                    idx = 0;

                LB_Saisie_EAN.SelectedIndex = idx;

                BT_FlushListe.Enabled = true;                
            }
            else
                BT_FlushListe.Enabled = false;

            SetCompteurListeSaisie();
            Cursor = Cursors.Default;
        }

        private void BT_GenereListe_MouseHover(object sender, EventArgs e)
        {
            TT_Boutons.Show("Générer une liste d'EAN", (Button)sender);
        }

        private void BT_FlushListe_Click(object sender, EventArgs e)
        {
            LB_Saisie_EAN.Items.Clear();
            BT_FlushListe.Enabled = false;
            SetCompteurListeSaisie();
        }

        private void BT_FlushListe_MouseHover(object sender, EventArgs e)
        {
            TT_Boutons.Show("Vider la liste", (Button)sender);
        }

        private void BT_Coller_Click(object sender, EventArgs e)
        {
            string clptxt = Clipboard.GetText();

            foreach (string s in clptxt.Replace("\r", "").Split('\n'))
            {
                string S = Regex.Replace(s, "[^X0-9]", ""); //s.Trim().Replace(" ", "").Replace("-", "");

                if (S.Length >= 13)
                {
                    LB_Saisie_EAN.Items.Add(S.Substring(0, 13));
                }
                else if (S.Length == 10)
                {
                    LB_Saisie_EAN.Items.Add(ISBN2CAB(S));
                }
            }

            if (LB_Saisie_EAN.Items.Count > 0)
                BT_FlushListe.Enabled = true;
            else
                BT_FlushListe.Enabled = false;

            if (LB_Saisie_EAN.Items.Count>=1)
                LB_Saisie_EAN.SelectedIndex = 0;

            SetCompteurListeSaisie();
        }       

        private void BT_Coller_MouseHover(object sender, EventArgs e)
        {
            TT_Boutons.Show("Coller une liste d'EAN/ISBN", (Button)sender);
        }

        private void LB_Saisie_EAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LB_Saisie_EAN.SelectedIndex >= 0)
            {
                TB_EAN.Text = LB_Saisie_EAN.SelectedItem.ToString();
                TB_EAN.Focus();
                TB_Libelle.Focus();                
            }
        }

        #endregion

        #region Methodes MainForm coherence nouveaux genres

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

                        if (CB_Genre_0.Text != "")
                            BT_AddGenre.Enabled = true;
                        else
                            BT_AddGenre.Enabled = false;


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
                        CB_Genre_1.Items.Add( pair.Key.Split(';')[1].ToString() );
                    }
                }
            }
            else
            {
                BT_AddGenre.Enabled = false;
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
                if (pair.Value.Substring(0,TB_Genre_0.Text.Length)==TB_Genre_0.Text)
                {
                    if (pair.Key.Split( ';' )[1] == LibSelected)
                    {
                        LibSelected = pair.Key;
                        break;
                    }
                }
            }
            
            string code =  DCL_Genre_1[LibSelected];
            code = code.Substring( TB_Genre_0.Text.Length, code.Length-TB_Genre_0.Text.Length);
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
                    if (pair.Value == code0+code1)
                    {
                        CB_Genre_1.Text = pair.Key.Split( ';' )[1].ToString();
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

                    if (v == code0+code1)
                        CB_Genre_2.Items.Add(pair.Key.Split( ';' )[1].ToString());
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
            int topcodelength = (code0.Length+code1.Length);


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
                        CB_Genre_3.Text = pair.Key.Split( ';' )[1];
                        break;
                    }
                }
            }
            else
                CB_Genre_3.Text = "";
        }        

        private void LB_Genres_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = LB_Genres.SelectedIndex;
            try
            {
                if (i >= 0)
                {
                    BT_SupprGenreSecondaire.Enabled = true;

                    string selcode = ((MyListBoxItem)LB_Genres.Items[i]).Message;

                    if (selcode == TB_GenrePrincipal.Text)
                        CBX_GenrePrincipal.Checked = true;
                    else
                        CBX_GenrePrincipal.Checked = false;

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BT_SupprGenreSecondaire_Click(object sender, EventArgs e)
        {
            int i = LB_Genres.SelectedIndex;
            string selcode = ((MyListBoxItem)LB_Genres.Items[i]).Message;
            LB_Genres.Items.RemoveAt(i);

            if (selcode == TB_GenrePrincipal.Text)
            {
                TB_GenrePrincipal.Text = "";
                CBX_GenrePrincipal.Checked = false;
            }

            BT_SupprGenreSecondaire.Enabled = false;
        }

        private void BT_AddGenre_Click(object sender, EventArgs e)
        {
            string code0 = TB_Genre_0.Text;
            string code1 = TB_Genre_1.Text;
            string code2 = TB_Genre_2.Text;
            string code3 = TB_Genre_3.Text;

            while (code0.Length < 2)
                code0 = "0" + code0;
            
            while (code1.Length < 2)
                code1 = "0" + code1;
            
            while (code2.Length < 2)
                code2 = "0" + code2;
            
            while (code3.Length < 2)
                code3 = "0" + code3;

            string CodeGenre = code0 + "-" + code1 + "-"+ code2 +"-"+ code3;

            MyListBoxItem lbi = new MyListBoxItem(SystemColors.WindowText, CodeGenre, deffont);
            
            int indexexist = -1;
            List<int> idxMP = new List<int>();
            
            //int idxMP = -1;

            for (int i = 0; i < LB_Genres.Items.Count; i++)
            {                
                string M = (LB_Genres.Items[i] as MyListBoxItem).Message;

                if (M == CodeGenre)
                {
                    indexexist = i;
                    break;
                }

                //ici on verifie si un code moins précis existe (remplace aussi les codes vides 00-00-00-00)
                else if (M == "00-00-00-00")
                    idxMP.Add(i);
                                                   
                else if (M == CodeGenre.Substring(0, 9) + "00")
                    idxMP.Add(i);                

                else if (M == CodeGenre.Substring(0, 6) + "00-00")                
                    idxMP.Add(i);                

                else if (M == CodeGenre.Substring(0, 3) + "00-00-00")
                    idxMP.Add(i);
                
            }

            bool Princip = false;
            if (idxMP.Count > 0)
            {
                foreach (int idx in idxMP)
                {
                    string CurrentGenre = ((MyListBoxItem)LB_Genres.Items[idx]).Message;

                    if (CurrentGenre == TB_GenrePrincipal.Text)
                    {
                        Princip = true;
                        TB_GenrePrincipal.Text = "";
                    }

                    LB_Genres.Items.RemoveAt(idx);                    
                }
            }

            if (CodeGenre == TB_GenrePrincipal.Text)
                Princip = true;
            
            if (indexexist < 0)
            {
                if (Princip)
                {
                    TB_GenrePrincipal.Text = lbi.Message;
                    lbi = new MyListBoxItem(Color.Blue, TB_GenrePrincipal.Text, boldfont);
                    LB_Genres.Items.Add(lbi);
                }
                else
                {
                    LB_Genres.Items.Insert(0, lbi);                    
                }
                indexexist = 0;
            }


            if (LB_Genres.Items.Count == 1)
            {
                TB_GenrePrincipal.Text = lbi.Message;                                
                LB_Genres.Items.RemoveAt(indexexist);
                lbi = new MyListBoxItem(Color.Blue, CodeGenre, boldfont);
                LB_Genres.Items.Add(lbi);
                
            }

            LB_Genres.SelectedItem = lbi;
            
        }

        private void CBX_GenrePrincipal_CheckedChanged(object sender, EventArgs e)
        {
            string selected = "";
            int i = LB_Genres.SelectedIndex;

            if (i >= 0)
            {
                selected = ((MyListBoxItem)LB_Genres.Items[i]).Message;
            }

            if (((CheckBox)sender).Checked)
            {
                if (TB_GenrePrincipal.Text != "")
                {                    
                    string memo = TB_GenrePrincipal.Text;

                    if (memo != selected)
                    {
                        DialogResult DR = MessageBox.Show("le code genre principal est déjà défini : " + memo + "\r\nremplacer ?", "Confirmation", MessageBoxButtons.YesNo);
                        if (DR == DialogResult.Yes)
                        {
                            TB_GenrePrincipal.Text = selected;

                            ReplaceGenrePrincipalInListBox(selected, memo, i);
                        }

                        else
                        {
                            CBX_GenrePrincipal.Checked = false;
                            TB_GenrePrincipal.Text = memo;
                        }
                    }                    
                }
                else
                {
                    if (selected != "")
                    {
                        MyListBoxItem lbi = new MyListBoxItem(Color.Blue, selected, boldfont);
                        LB_Genres.Items.RemoveAt(i);
                        LB_Genres.Items.Add(lbi);

                        TB_GenrePrincipal.Text = selected;
                    }
                    else
                    {
                        MessageBox.Show("aucun code selectionné");
                        CBX_GenrePrincipal.Checked = false;
                    }

                }
            }
            else
            {
                if (selected == TB_GenrePrincipal.Text)
                    TB_GenrePrincipal.Text = "";
            }

        }

        #endregion

        #region Methodes MainForm raccourcis_F2

        private void TB_Libelle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
                BT_Libelle_Click(sender, e);
        }

        private void LB_Suggest_Auteurs_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TB_Auteurs.Text = TB_Auteurs.Text.Replace(LBSAut, LB_Suggest_Auteurs.Items[LB_Suggest_Auteurs.SelectedIndex].ToString());
                LB_Suggest_Auteurs.Items.Clear();
                LB_Suggest_Auteurs.Visible = false;
            }
            catch { }
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
                TB_Auteurs.Text = TB_Auteurs.Text.Replace(LBSAut, LB_Suggest_Auteurs.Items[LB_Suggest_Auteurs.SelectedIndex].ToString());
                LB_Suggest_Auteurs.Items.Clear();
                LB_Suggest_Auteurs.Visible = false;
            }
        }

        private void LB_Suggest_Auteurs_Leave(object sender, EventArgs e)
        {
            LB_Suggest_Auteurs.Items.Clear();
            LB_Suggest_Auteurs.Visible = false;
        }

        public void SuggestAuteur(ListBox LB_Suggest_Auteurs, TextBox TB_Auteurs)
        {
            LB_Suggest_Auteurs.Items.Clear();

            if (TB_Auteurs.SelectionStart < TB_Auteurs.TextLength)
            {
                LBSAut = "";
                int i = 0;
                List<int> pos = new List<int>();
                foreach (char c in TB_Auteurs.Text.ToCharArray())
                {
                    i++;
                    if (c == ';')
                    {
                        pos.Add(i);
                    }
                }

                //chercher le point virgule juste avant
                if (pos.Count == 0)
                    LBSAut = TB_Auteurs.Text;
                else if (TB_Auteurs.SelectionStart < pos[0])
                {
                    LBSAut = TB_Auteurs.Text.Substring(0, pos[0]);
                }
                else
                {
                    int begin = 0, end = 0;
                    foreach (int item in pos)
                    {
                        if (item < TB_Auteurs.SelectionStart)
                        {
                            begin = item;
                            //MessageBox.Show(begin.ToString());
                        }
                        else if (item >= TB_Auteurs.SelectionStart)
                        {
                            end = item;
                            //MessageBox.Show(end.ToString());
                            break;
                        }
                    }
                    if (end == 0)
                        end = i;

                    LBSAut = TB_Auteurs.Text.Substring(begin, end - begin);
                }
            }
            else
            {
                //on ne garde que le dernier auteur après le ;
                LBSAut = "";
                foreach (string s in TB_Auteurs.Text.Split(';'))
                    if (s != "")
                        LBSAut = s.Trim();
            }

            LBSAut = LBSAut.Replace(";", "").Trim();
            if (LBSAut!="")
            {

                string CompQuery = "";
                foreach (string s in LBSAut.Replace(",", " ").Replace("-", " ").Replace(".", "").Split(' '))
                {
                    if (s != "")
                        CompQuery += " nomauteur containing '" + s + "' and";
                }
                CompQuery = CompQuery.Substring(0, CompQuery.Length - 4);

                db.Query("select first 10 nomauteur from auteursunique where" + CompQuery);
                foreach (Row row in db.FetchAll())
                {
                    LB_Suggest_Auteurs.Items.Add(row["nomauteur"].ToString());
                }

                if (LB_Suggest_Auteurs.Items.Count > 0)
                    LB_Suggest_Auteurs.Visible = true;

                LB_Suggest_Auteurs.Focus();
            }
        }

        private void TB_Auteurs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
                CallAuteursForm();

            else if (e.KeyData == Keys.F3)            
                LBClickAlternate(TB_Auteurs, BT_Auteurs);            

            else if (e.KeyData == Keys.F4)
                SuggestAuteur(LB_Suggest_Auteurs, TB_Auteurs);

        }

        private void CB_Traducteur_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2 || e.KeyData == Keys.F3)
                BT_Trad_Click(sender, e);
        }

        private void TB_Commentaire_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                TB_Commentaire.Text = TB_Commentaire.Text.ToLower();
            }
        }

        #endregion        

        #region Methodes MainForm Recharge les listes F5

        private void CB_Editeur_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                Cursor = Cursors.WaitCursor;
                DEditeur.Clear();
                DEdiCodeEdi.Clear();
                DEdiFullToEdi.Clear();
                CB_Editeur.Items.Clear();

                LoadDicoEditeurs();
                Cursor = Cursors.Arrow;
            }
        }

        private void CB_Collection_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5 && CB_Editeur.Items.Contains(CB_Editeur.Text))
            {
                Cursor = Cursors.WaitCursor;
                SetCollectionByEditeur(CB_Editeur.Text);
                Cursor = Cursors.Arrow;
            }
            else if (e.KeyData == Keys.F2)
            {
                bool Alert = false;
                
                if (string.IsNullOrEmpty(CB_Editeur.Text))
                    Alert = true;
                
                if (!DEditeur.ContainsKey(CB_Editeur.Text))
                    Alert = true;
                
                if (Alert)
                {
                    MessageBox.Show("impossible de créer une collection sans préciser d'éditeur.");
                }

                else
                {
                    //ancienne collectionform
                    /*CollectionForm CF = New CollectionForm(this, db, CB_Editeur.Text, 
                                                             DCL_Genre_0, DCL_Genre_1, DCL_Genre_2, DCL_Genre_3,
                                                             DSupport, DLectorat, DOBCode, DEdiFullToEdi, CB_Collection.Text);
                    */
                    SaisieLivre.Forms.NewCollectionForm CF = new SaisieLivre.Forms.NewCollectionForm(this, "C", Properties.Resources.SelectFromCollectionsForNewCollection);

                    if (DEdiFullToEdi.ContainsKey(CB_Editeur.Text))
                        CF.TB_Editeur.Text = DEdiFullToEdi[CB_Editeur.Text];
                    else
                        CF.TB_Editeur.Text = CB_Editeur.Text;

                    CF.TB_Libelle.Text = CB_Collection.Text;

                    DialogResult DR = CF.ShowDialog();

                    if (DR == DialogResult.OK)
                    {                       
                        //recharge la liste
                        Cursor = Cursors.WaitCursor;
                        SetCollectionByEditeur(CB_Editeur.Text);
                        Cursor = Cursors.Arrow;

                        //remet les libelles en place après modif
                        CB_Collection.Text = "";                        
                        CB_Collection.Text = CF.TB_Libelle.Text;
                        CB_Collection.SelectedIndex = CB_Collection.Items.IndexOf(CF.TB_Libelle.Text);
                        
                        TB_IDCollection.Text = CF.ID;                        
                        //TB_Lectorat.Text = CF.codeLectorat;


                        RewriteTitreFromSerieColl();
                        TB_Nocoll.Focus();
                    }                                        
                }
            }
        }

        #endregion        

        #region Methodes MainForm Enregistrement de la configuration

        /// <summary>
        /// Sauvegarde de la config (BDD)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_SaveCfg_Base_Click(object sender, EventArgs e)
        {
            ini.IniWriteValue("BDD", "HOST", TB_Parametre_Host.Text);
            ini.IniWriteValue("BDD", "NAME", TB_Parametre_Name.Text);
            ini.IniWriteValue("BDD", "USER", TB_Parametre_User.Text);
            ini.IniWriteValue("BDD", "PASS", TB_Parametre_Pass.Text);
            db.Close();

            Cursor = Cursors.WaitCursor;
            
            db = new FDB(TB_Parametre_User.Text + ":" + TB_Parametre_Pass.Text + "@" + TB_Parametre_Host.Text + ":" + TB_Parametre_Name.Text);
            LoadDefaultDicos();

            Cursor = Cursors.Default;
            MessageBox.Show("Base connectée sur " + db.ConnexionString + "!");            
        }

        private void BT_SaveCfg_Visuel_Click(object sender, EventArgs e)
        {
            BaseURL = TB_Parametre_Image.Text.Trim();
            ini.IniWriteValue("IMAGES", "URL", BaseURL);
            MessageBox.Show("Modifications enregistrées.");
        }
        
        private void CBX_AncrageDilicom_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_AncrageDilicom.Checked)
            {
                MainForm_LocationChanged(sender, e);
                ini.IniWriteValue("DILICOM", "ANCRAGE", "1");
            }
            else
            {
                ini.IniWriteValue("DILICOM", "ANCRAGE", "0");
                SD.CanMove = true;
            }
        }

        private void CBX_AffichageInfoColl_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_AffichageInfoColl.Checked)
            {
                ini.IniWriteValue("COLLECTION", "DISPLAY", "1");                
            }
            else
            {
                ini.IniWriteValue("COLLECTION", "DISPLAY", "0");                
                CD.Hide();
            }
        }

        #endregion                       
                
        #region Methodes MainForm checkboxes

        
        private void CBX_Coll_Coffret_CheckedChanged(object sender, EventArgs e)
        {
            //CheckEnabledNoColl2();

            if (CBX_Coll_Coffret.Checked)
            {
                TB_Coll_Coffret.Text = "1";

                TB_Coll_Contenu_1.Enabled = true;
                TB_Coll_Contenu_2.Enabled = true;
            }
            else
            {
                TB_Coll_Coffret.Text = "0";
                
                TB_Coll_Contenu_1.Enabled = false;
                TB_Coll_Contenu_2.Enabled = false;
            }

            RewriteTitreFromSerieColl();
        }

        private void CBX_Coll_HorsSerie_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_Coll_HorsSerie.Checked)
                TB_Coll_HorsSerie.Text = "1";
            else
                TB_Coll_HorsSerie.Text = "0";

            RewriteTitreFromSerieColl();
        }

        private void CBX_Serie_Integrale_CheckedChanged(object sender, EventArgs e)
        {
            //CheckEnabledNoSerie2();

            if (CBX_Serie_Integrale.Checked)
            {
                TB_Serie_Integrale.Text = "1";
                
                TB_Serie_Contenu_1.Enabled = true;
                TB_Serie_Contenu_2.Enabled = true;
            }
            else
            {
                TB_Serie_Integrale.Text = "0";

                if (!CBX_Serie_Coffret.Checked)
                {
                    TB_Serie_Contenu_1.Enabled = true;
                    TB_Serie_Contenu_2.Enabled = true;
                }
            }

            RewriteTitreFromSerieColl();
        }

        private void CBX_Serie_Coffret_CheckedChanged(object sender, EventArgs e)
        {
            //CheckEnabledNoSerie2();

            if (CBX_Serie_Coffret.Checked)
            {
                TB_Serie_Coffret.Text = "1";

                TB_Serie_Contenu_1.Enabled = true;
                TB_Serie_Contenu_2.Enabled = true;
            }
            else
            {
                TB_Serie_Coffret.Text = "0";

                if (!CBX_Serie_Integrale.Checked)
                {
                    TB_Serie_Contenu_1.Enabled = false;
                    TB_Serie_Contenu_2.Enabled = false;
                }
            
            }

            RewriteTitreFromSerieColl();
        }

        private void CBX_Serie_HorsSerie_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_Serie_HorsSerie.Checked)
                TB_Serie_HorsSerie.Text = "1";
            else
                TB_Serie_HorsSerie.Text = "0";

            RewriteTitreFromSerieColl();
        }


        private void TB_IAD_TextChanged(object sender, EventArgs e)
        {
            if (TB_IAD.Text == "1")
                CBX_IAD.Checked = true;
            else
                CBX_IAD.Checked = false;

            AlternateValues();
        }

        private void CBX_IAD_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_IAD.Checked)
                TB_IAD.Text = "1";
            else
                TB_IAD.Text = "0";
        }

        private void TB_Relié_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                CBX_relie.Checked = true;
            else
                CBX_relie.Checked = false;

            AlternateValues();
        }

        private void TB_Broché_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                CBX_broche.Checked = true;
            else
                CBX_broche.Checked = false;

            AlternateValues();
        }

        private void TB_cartonne_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                cbx_cartonne.Checked = true;
            else
                cbx_cartonne.Checked = false;

            AlternateValues();
        }

        private void TB_LivreLu_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                CBX_livre_lu.Checked = true;
            else
                CBX_livre_lu.Checked = false;

            AlternateValues();
        }

        private void TB_GrandsCara_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                CBX_grandcaractere.Checked = true;
            else
                CBX_grandcaractere.Checked = false;

            AlternateValues();
        }


        private void TB_Luxe_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                CBX_Luxe.Checked = true;
            else
                CBX_Luxe.Checked = false;

            AlternateValues();
        }

        private void TB_Illustre_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                CBX_Illustre.Checked = true;
            else
                CBX_Illustre.Checked = false;

            AlternateValues();
        }


        private void CBX_relie_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked)
                TB_Relié.Text = "0";
            else
            {
                TB_Relié.Text = "1";
                CBX_broche.Checked = false;
            }
        }

        private void CBX_cartonne_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked)
                TB_Cartonne.Text = "0";
            else            
                TB_Cartonne.Text = "1";            
        }

        private void CBX_broche_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked)
                TB_Broché.Text = "0";
            else
            {
                TB_Broché.Text = "1";
                CBX_relie.Checked = false;
            }
        }

        private void CBX_livre_lu_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked)
                TB_LivreLu.Text = "0";
            else
                TB_LivreLu.Text = "1";            
        }

        private void CBX_grandcaractere_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked)
                TB_GrandsCara.Text = "0";
            else
                TB_GrandsCara.Text = "1";
        }

        private void CBX_desactivervisuel_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox3.Image = null;
        }

        private void CBX_Multilingue_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_Multilingue.Checked)
                TB_Multilingue.Text = "1";
            else
                TB_Multilingue.Text = "0";
        }

        private void TB_Multilingue_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1")
                CBX_Multilingue.Checked = true;
            else
                CBX_Multilingue.Checked = false;

            AlternateValues();
        }

        private void CBX_AffichageDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_AffichageDebug.Checked)
            {
                ini.IniWriteValue("DEBUG", "VIEW", "1");
                this.MaximumSize = new Size(1183, this.Height);
                this.MinimumSize = new Size(1183, this.Height);
                this.Width = 1183;
            }
            else
            {
                ini.IniWriteValue("DEBUG", "VIEW", "0");
                this.MaximumSize = new Size(884, this.Height);
                this.MinimumSize = new Size(884, this.Height);
                this.Width = 884;
            }
        }

        private void CBX_Illustre_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_Illustre.Checked)
                TB_Illustre.Text = "1";
            else
                TB_Illustre.Text = "0";
        }

        private void CBX_Luxe_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_Luxe.Checked)
                TB_Luxe.Text = "1";
            else
                TB_Luxe.Text = "0";
        }

        #endregion        

        #region Methodes Autres

        // Charge une fiche à partir du gencod
        public void LoadFiche()
        {

            ClearFiche(true);

            if (AutoCopyEan)
                Clipboard.SetText(TB_EAN.Text.Substring(0, 13));

            //on nettoie les affichages précédents.            
            ResetMDLABELColors();

            if (TB_EAN.Text.Length >= 13 && (CheckEAN(TB_EAN.Text.Substring(0, 13)) != TB_EAN.Text.Substring(0, 13)))
            {
                MessageBox.Show(string.Format("Gencod incorrect !\r\n --> {0}\r\n\r\nremplacé par\r\n --> {1}", TB_EAN.Text.Substring(0, 13), CheckEAN(TB_EAN.Text.Substring(0, 13))));
                TB_EAN.Text = CheckEAN(TB_EAN.Text.Substring(0, 13));
            }

            if (TB_EAN.Text != "*")
            {
                string QuerySelectFromLivre = string.Format(QueryLivre, TB_EAN.Text.Substring(0, 13));
                
                //Clipboard.SetText(QuerySelectFromLivre);
                //MessageBox.Show(QuerySelectFromLivre);

                if (MultiDistri)
                {
                    /* voir le ddl de la table dans T_MUTLIDISTRI.sql */
                    DMDistri.Clear();

                    db.Query(string.Format("SELECT distributeur, id From T_MULTIDISTRI where Gencod='{0}' " +
                                           "union all " +
                                           "SELECT distributeur, 0 as id From LIVRE where Gencod='{0}'", TB_EAN.Text.Substring(0, 13)));

                    foreach (var row in db.FetchAll())
                    {
                        DMDistri.Add(Convert.ToInt32(row["id"]), row["distributeur"].ToString());

                        if (row["id"].ToString() == "0")
                            TB_MainDistri.Text = row["distributeur"].ToString();
                    }

                    if (DMDistri.Count > 1)
                    {
                        LoadMultiDistriForm LMDF = new LoadMultiDistriForm(DMDistri, TB_MultiDistri_Selected, TB_MultiDistri_ID);
                        LMDF.ShowDialog();
                    }

                    if (TB_MultiDistri_Selected.Text != "" && TB_MultiDistri_Selected.Text != TB_MainDistri.Text)
                    {
                        ResetMDLABELColors();

                        QuerySelectFromLivre = string.Format(QueryLivreAlternate,
                                                             TB_EAN.Text.Substring(0, 13),
                                                             TB_MultiDistri_ID.Text);

                    }

                }

                //Requete principale ==> fichier SQL dans les ressources               
                db.Query(QuerySelectFromLivre);

                //Clipboard.SetText(QuerySelectFromLivre);

                Found = false;
                BT_AddMultiDistri.Enabled = false;

                foreach (Row Row in db.FetchAll())
                {
                    Found = true;
                    LoadFicheRow(Row);
                    LoadArtFunc(TB_EAN.Text);
                    BT_AddMultiDistri.Enabled = true;
                    break;
                }

                if (!Found)
                {
                    //affecte certaines valeurs par défaut
                    TB_Langue_Concat.Text = "1";
                    TB_CodeTVA.Text = "2";
                    TB_Poids.Text = "0";
                    TB_NBPages.Text = "0";
                    TB_Hauteur.Text = "0";
                    TB_Largeur.Text = "0";
                    TB_PrixEuro.Text = "0";
                    TB_CodeSupport.Text = "T";
                    TB_Dispo.Text = "1";
                    TB_Lectorat.Text = "0";
                    TB_Style.Text = "0";
                    TB_Langue.Text = "0";
                    TB_IDTraduitEn.Text = "0";
                    TB_IDTraducteur.Text = "0";
                    TB_IDCollection.Text = "0";
                    pictureBox1.Image = Properties.Resources.image_a_venir;
                }

                //Clipboard.SetText(string.Format(QueryLivreBis, TB_EAN.Text.Substring(0, 13)));

                //Requete dilicom ==> idem
                db.Query(string.Format(QueryLivreBis, TB_EAN.Text.Substring(0, 13)));
                
                Dictionary<string, string> Dilidico = new Dictionary<string, string>();
                
                string result = string.Empty;
                string result2 = string.Empty;
                string TypeProd = string.Empty;
                
                foreach (var row in db.FetchAll())
                {                                       
                    foreach (var pair in row)
                    {
                        if (pair.Key.Length <= 5 || pair.Key.Substring(0, 5) != "test_")
                        {                            
                            if (Dilidico.ContainsKey(pair.Key))
                            {
                                Dilidico[pair.Key] = pair.Value.ToString();
                            }
                            else
                                Dilidico.Add( pair.Key , pair.Value.ToString() );
                        }
                        else
                        {
                            if (pair.Key == "test_typeprod")
                            {
                                try
                                {
                                    TypeProd = Convert.ToInt32(pair.Value).ToString();
                                }
                                catch { TypeProd = "0"; }
                            }
                        }
                    }
                }

                if (Dilidico.ContainsKey("supportclil"))
                {

                    if ((Dilidico["supportclil"] == "J" || Dilidico["supportclil"] == "D" || Dilidico["supportclil"] == "LD") && Dilidico["tva"] == "1")
                        Dilidico["supporttl"] = "R";

                    else if ((Dilidico["supportclil"] == "J" || Dilidico["supportclil"] == "D" || Dilidico["supportclil"] == "LD") && Dilidico["tva"] == "2")
                        Dilidico["supporttl"] = "T";

                    else if (Dilidico["supporttl"] == "" && Dilidico["tva"] == "1")
                        Dilidico["supporttl"] = "R";

                    else if (Dilidico["supporttl"] == "" && Dilidico["tva"] == "2")
                        Dilidico["supporttl"] = "T";

                    if (TypeProd == "3")
                        Dilidico["supporttl"] = "A";
                }

                /*
                Suppression suite au mail de Florent 26/10/2016
                 * 
                if (((Convert.ToInt32(Dilidico["hauteur"]) > 0 && Convert.ToInt32(Dilidico["hauteur"]) < 200) &&
                    (Convert.ToInt32(Dilidico["largeur"]) > 0 && Convert.ToInt32(Dilidico["largeur"]) < 130)) &&
                    (Dilidico["supporttl"] == "T" || Dilidico["supporttl"] == "BL"))
                    Dilidico["supporttl"] = "P";
                */                

                foreach (var pair in Dilidico)
                {
                    result += pair.Key + "~" + pair.Value + "\r\n";
                }

                SD.TB_Dilicom.Text = result;
                SD.TB_Dilicom_ToLV();
                
                SD.StartPosition = FormStartPosition.Manual;
                SD.Font = LoadedFont;
                SD.Show();

                BT_Valider.Enabled = true;
                BT_Annuler.Enabled = true;
            }
            else
            {
                MessageBox.Show("Gencod invalide");
                TB_EAN.Text = "";
                TB_EAN.Focus();
            }

            this.Focus();
            TB_Libelle.Focus();

            CheckProductVersion();
        }

        //charge les artistes/fonctions
        private void LoadArtFunc(string EAN)
        {
            Dictionary<string, List<string>> D = new Dictionary<string, List<string>>();

            db.Query(string.Format("select * from auteurs_fonctions where gencod='{0}'", EAN));
            foreach (var Row in db.FetchAll())
            {
                if (!D.ContainsKey(Row["auteur"].ToString()))
                {
                    List<string> l = new List<string>();
                    l.Add(Row["fonction"].ToString());
                    D.Add(Row["auteur"].ToString(), l);
                }
                else
                    D[Row["auteur"].ToString()].Add(Row["fonction"].ToString());

            }

            foreach (var pair in D)
            {
                string codes = "";
                foreach (string c in pair.Value)
                    codes += c + ",";

                codes = codes.Substring(0, codes.Length - 1);

                LB_AutFunc.Items.Add(pair.Key);
                LB_AutFuncComp.Items.Add(codes);                
            }
        }

        public void LoadGenres(string codesgenres)
        {
            LB_Genres.Items.Clear();

            if (codesgenres == "")
                codesgenres = "00000000:1";

            PreviousGenre = SetPreviousGenreToAllDigits(codesgenres);

            string Principal = "";
            int Pindex = 0;

            foreach (string s in codesgenres.Split(';'))
            {
                string CodeGenre = RewriteGenre(s);

                if (CodeGenre.Split(':')[1] == "1")
                {
                    Principal = CodeGenre.Split(':')[0];
                    TB_GenrePrincipal.Text = Principal;
                }

                MyListBoxItem lbi = new MyListBoxItem(SystemColors.WindowText, CodeGenre.Split(':')[0], deffont);

                if (CodeGenre.Split(':')[0] == TB_GenrePrincipal.Text)
                    lbi = new MyListBoxItem(Color.Blue, CodeGenre.Split(':')[0], boldfont);

                LB_Genres.Items.Add(lbi);
                Pindex = LB_Genres.Items.IndexOf(lbi);
            }

            LB_Genres.SelectedIndex = Pindex; //Principal;

        }

        // charge un enregistrement SQL dans le panel P_Saisie + quelques champs supplémentaires...
        private void LoadFicheRow(Row row)
        {
            //genres
            LoadGenres(row["returngenre"].ToString());

            string coll_libelle = "";                       

            foreach (Control c in P_Saisie.Controls["groupBox1"].Controls)
            {
                if (c.Tag != null)
                    if (row.ContainsKey(c.Tag.ToString()))
                        c.Text = row[c.Tag.ToString()].ToString();
            }
            
            coll_libelle = row["collection"].ToString();

            if (!CB_Collection.Items.Contains(coll_libelle))
            {
                CB_Collection.ForeColor = Color.Red;
            }

            //remplis la groupbox a part
            foreach (Control c in P_Saisie.Controls["GBSupport"].Controls)
            {
                if (c.Tag != null)
                    if (row.ContainsKey(c.Tag.ToString()))
                        c.Text = row[c.Tag.ToString()].ToString();
            }

            //remplis le panel caché
            foreach (Control c in P_Hidden.Controls)
            {
                if (c.Tag != null)
                    if (row.ContainsKey(c.Tag.ToString()))
                        c.Text = row[c.Tag.ToString()].ToString();
            }


            //remplis le reste du panel
            foreach (Control c in P_Saisie.Controls)
            {
                if (c.Tag != null)
                    if (row.ContainsKey(c.Tag.ToString()))
                        c.Text = row[c.Tag.ToString()].ToString();
            }

            //codelangue (masque les 0000/00 des niveaux 2 et 3 facultatif                                
            string XCode = row["codelangue"].ToString();

            TB_Langue_2.Text = XCode;

            if (XCode.Length >= 3)
            {
                TB_Langue_1.Text = XCode.Substring(0, 3);
            }

            if (XCode.Length >= 1)
            {
                TB_Langue_0.Text = XCode.Substring(0, 1);
            }

            if (XCode.Length >= 5)
            {

                if (XCode.Substring(3, 2) == "00")
                {
                    XCode = XCode.Substring(0, 3);
                }
            }

            if (XCode.Length >= 3)
            {
                if (XCode.Substring(1, 2) == "00")
                {
                    XCode = XCode.Substring(0, 1);
                }
            }

            TB_Langue_Concat.Text = XCode;

            LoadedAuteurs = TB_Auteurs.Text;
            
            PreviousResume = row["presentation"].ToString();
            PreviousSommaire = row["sommaire"].ToString();

            TSSL_DMD.Text = row["datemajdispo"].ToString();
            TSSL_DM.Text = row["datemaj"].ToString();
            TSSL_DMI.Text = row["image_date"].ToString();
            TSSL_DTCREA.Text = row["datecreation"].ToString();


            if (PreviousSommaire != "")
                TB_Sommaire.Text = PreviousSommaire;

            if (PreviousResume != "")
                TB_Resume.Text = PreviousResume;

            SetTPResumeText();

        }

        // nettoyage de la fiche
        private void ClearFiche(bool saveEAN = false, bool closecoll = false)
        {
            LoadedAuteurs = string.Empty;
                          
            CB_Collection.Items.Clear();
            CB_Collection.SelectedIndex = -1;

            CB_Editeur.SelectedIndex = -1;

            TB_Libelle.ForeColor = SystemColors.ControlText;

            //doit être vidé en premier pour empecher la fonction SetTitreFromPresentTitreColl d'ajouter un ; dans le champ titre vide
            TB_PresentTitre.Text = string.Empty;

            //vide les controles du groupbox 1
            foreach (Control c in P_Saisie.Controls["groupBox1"].Controls)
            {
                if (c.Tag != null)
                    c.Text = string.Empty;
            }

            //vide les controles du groupbox 2
            foreach (Control c in P_Saisie.Controls["GBSupport"].Controls)
            {
                if (c.Tag != null)
                    c.Text = string.Empty;
            }

            //remplis le reste du panel
            foreach (Control c in P_Saisie.Controls)
            {
                if (c.Tag != null)
                    c.Text = string.Empty;
            }

            //remplis le reste du panel
            foreach (Control c in P_Hidden.Controls)
            {
                if (c.Tag != null)
                {
                    if (c.Tag.ToString() == "GENCOD" && saveEAN)
                    {
                        //on conserve le gencod
                    }
                    else
                        c.Text = string.Empty;
                }
            }

            foreach (var pair in DPSaisieDefault)
            {
                (pair.Key as Control).Text = pair.Value;
            }

            pictureBox1.Image = SaisieLivre.Properties.Resources.image_a_venir;
            pictureBox2.Image = null;

            TSSL_DM.Text = "";
            TSSL_DMD.Text = "";
            TSSL_DMI.Text = "";
            TSSL_DTCREA.Text = "";

            CB_Langue_0.Text = string.Empty;
            CB_Langue_1.Text = string.Empty;
            CB_Langue_2.Text = string.Empty;

            CB_Langue.Text = "";
            CB_TraduitEn.Text = "";

            CB_Traducteur.Text = string.Empty;

            TB_Resume.Text = "";
            TP_Resume.Text = "Resume";

            TB_Sommaire.Text = "";

            LB_RefsChainees.Items.Clear();
            BT_SupprGenreSecondaire.Enabled = false;

            LB_Genres.Items.Clear();

            PreviousGenre = string.Empty;
            PreviousSommaire = string.Empty;
            PreviousResume = string.Empty;

            TB_MultiDistri_Selected.Text = string.Empty;
            TB_MainDistri.Text = string.Empty;

            AddMultiDistri = false;

            CB_Collection.ForeColor = SystemColors.WindowText;
            CB_SousCollection.ForeColor = SystemColors.WindowText;
            TB_SousCollection.Text = "";

            CBX_SupprChainage.Checked = false;
            CBX_GenrePrincipal.Checked = false;
            CBX_Coll_Coffret.Checked = false;
            CBX_Coll_HorsSerie.Checked = false;
            //CBX_Coll_Integrale.Checked = false;

            CBX_Serie_Coffret.Checked = false;
            CBX_Serie_HorsSerie.Checked = false;
            CBX_Serie_Integrale.Checked = false;

            TB_IDLibSerie.Text = "0";
            
            CB_CodeSupport.Text = "";

            CB_Dispo.Text = "";
            CB_TVA.Text = "";
            CB_Style.Text = "";
            CB_Langue.Text = "";
            CB_TraduitEn.Text = "";

            TB_Edition1.Text = "";
            TB_Edition2.Text = "";
            TB_NewTitre.Text = "";

            LB_AutFunc.Items.Clear();
            LB_AutFuncComp.Items.Clear();

            if (IF != null)
            {
                IF.Close();
                IF = null;
            }

            TB_IDCollection.Text = "";

            if (CBX_AffichageInfoColl.Checked && closecoll)
            {
                CD.Hide();
            }

            CollectionRow = null;
            SerieRow = null;

            ResetMDLABELColors();

            ReloadSerie();

            TB_EAN.Focus();
        }

        // Generation des dictionnaires              
        public Dictionary<string, Dictionary<int, string>> LoadDistrib(string query)
        {
            CB_Editeur.Items.Clear();

            var source = new AutoCompleteStringCollection();

            Dictionary<string, Dictionary<int, string>> Liste = new Dictionary<string, Dictionary<int, string>>();
            db.Query(query);
            foreach (Row row in db.FetchAll())
            {
                Dictionary<int, string> E = new Dictionary<int, string>();
                E.Add(0, row["distributeur"].ToString());

                if (!string.IsNullOrEmpty(row["distributeur2"].ToString()))
                {
                    E.Add(1, row["distributeur2"].ToString());
                }

                if (!string.IsNullOrEmpty(row["distributeur3"].ToString()))
                    E.Add(2, row["distributeur3"].ToString());

                if (!Liste.ContainsKey(row["editeur"].ToString()))
                {
                    Liste.Add(row["editeur"].ToString(), E);

                    CB_Editeur.Items.Add(row["editeur"].ToString());
                    source.Add(row["editeur"].ToString());
                }
            }
            CB_Editeur.AutoCompleteCustomSource = source;

            return Liste;
        }

        // Generation des dictionnaires              
        public Dictionary<string, string> LoadListe(string query, string DValue, string DKey, ComboBox CB = null, ComboBox CB2 = null)
        {
            if (CB != null)
                CB.Items.Clear();

            var source = new AutoCompleteStringCollection();

            Dictionary<string, string> Liste = new Dictionary<string, string>();
            db.Query(query);
            foreach (Row row in db.FetchAll())
            {
                if (!Liste.ContainsKey(row[DKey].ToString()))
                {
                    Liste.Add(row[DKey].ToString(), row[DValue].ToString());
                    if (CB != null)
                    {
                        CB.Items.Add(row[DKey].ToString());
                        source.Add(row[DKey].ToString());
                    }

                    if (CB2 != null)
                    {
                        CB2.Items.Add(row[DKey].ToString());
                    }
                }
            }

            if (CB != null)
                CB.AutoCompleteCustomSource = source;

            if (CB2 != null)
                CB2.AutoCompleteCustomSource = source;

            return Liste;
        }

        private void LoadDicoEditeurs()
        {
            DEditeur = LoadDistrib("SELECT                                                                                                                    " +
                                       "  CAST(                                                                                                                   " +
                                       "     trim( editeur ) ||                                                                                                   " +
                                       "     case                                                                                                                 " +
                                       "        when ( COALESCE( libelle_complet, '' ) != '' ) then ' (' || cast( trim( libelle_complet ) as varchar(80)) || ')'  " +
                                       "        else ''                                                                                                           " +
                                       "     end                                                                                                                  " +
                                       "     as varchar(130)) as editeur,                                                                                         " +
                                       "     distributeur, distributeur2, distributeur3                                                                           " +
                                       "from EDITEURS                                                                                                             " +
                                       "where editeur!=''                                                                                                         " +
                                       "order by editeur");

            DEdiCodeEdi = LoadListe("SELECT                                                                                                                   " +
                                    "  CAST(                                                                                                                  " +
                                    "    trim( editeur ) ||                                                                                                   " +
                                    "    case                                                                                                                 " +
                                    "        when ( COALESCE( libelle_complet, '' ) != '' ) then ' (' || cast( trim( libelle_complet ) as varchar(80)) || ')' " +
                                    "        else ''                                                                                                          " +
                                    "    end                                                                                                                  " +
                                    "    as varchar(130)) as editeur,                                                                                         " +
                                    "    codeediteur                                                                                                          " +
                                    "from EDITEURS                                                                                                            " +
                                    "where editeur!=''                                                                                                         ",
                                    "codeediteur", "editeur");

            DEdiFullToEdi = LoadListe("SELECT                                                                                                                 " +
                                      "  CAST(                                                                                                                " +
                                      "    trim( editeur ) ||                                                                                                 " +
                                      "    case                                                                                                               " +
                                      "      when ( COALESCE( libelle_complet, '' ) != '' ) then ' (' || cast( trim( libelle_complet ) as varchar(80)) || ')' " +
                                      "        else ''                                                                                                        " +
                                      "    end                                                                                                                " +
                                      "    as varchar(130)) as editeurfull,                                                                                   " +
                                      "    editeur                                                                                                            " +
                                      "from EDITEURS                                                                                                          " +
                                      "where COALESCE( libelle_complet, '' )!= '' and editeur!=''                                                             ",
                                      "editeur", "editeurfull");
        }

        public string FindIdByLib(Dictionary<string, string> Dico, string Value, ComboBox CB = null)
        {
            //CB.Text = "";
            foreach (var pair in Dico)
            {
                if (pair.Value == Value)
                {
                    if (CB != null)
                    {
                        CB.SelectedValue = pair.Key;
                        CB.Text = pair.Key;
                    }
                    return pair.Key;
                }
            }
            return "";
        }

        private void ResetMDLABELColors()
        {            
            foreach (var pair in CollectionData)
            {
                FData FD = pair.Value;
                
                switch (FD.Control.Name)
                {
                    case "TB_Libelle":
                    case "TB_Auteurs":
                    case "CB_Collection":
                    case "CB_Serie":
                    case "TB_Commentaire":
                    case "CB_Traducteur":
                        FD.Label.ForeColor = Color.Blue;                        
                        break;

                    default:
                        FD.Label.ForeColor = SystemColors.ControlText;
                        break;
                }
                
                FD.Control.BackColor = SystemColors.Window;

                if (FD.LabelText != FD.LabelTextOrigine)
                    FD.Label.Text = FD.LabelTextOrigine;

                if (FD.Control2 != null)
                    FD.Control2.BackColor = SystemColors.Window;
                if (FD.Control3 != null)
                    FD.Control3.BackColor = SystemColors.Window;
                if (FD.Control4 != null)
                    FD.Control4.BackColor = SystemColors.Window;
            }
        }

        public void SetCollectionByEditeur(string Editeur)
        {
            string MColl = CB_Collection.Text;

            CB_Collection.Items.Clear();
            DCollection.Clear();

            //TB_IDCollection.Text = "0";
            CB_Collection.Text = "0";

            Dictionary<string, string> DSC = new Dictionary<string, string>();

            try
            {
                var source = new AutoCompleteStringCollection();

                string qselcol = string.Format(Properties.Resources.QuerySelectCollection, Editeur.Replace("'", "''"));

                db.Query(qselcol);

                foreach (var row in db.FetchAll())
                {
                    if (!CB_Collection.Items.Contains(row["collection"].ToString()))
                    {
                        if (!DCollection.ContainsKey(row["collection"].ToString()))
                        {
                            CB_Collection.Items.Add(row["collection"].ToString());
                            source.Add(row["collection"].ToString());
                            DCollection.Add(row["collection"].ToString(), row);
                        }
                    }

                }
                CB_Collection.Sorted = true;
                CB_Collection.AutoCompleteCustomSource = source;

            }
            catch
            {
            }

            CB_Collection.Text = MColl;
        }

        public string RemoveDiacritics(string input)
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
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        private void ForceUpperTB(TextBox TB)
        {
            if (TB.Text != TB.Text.ToUpper())
            {
                TB.Text = TB.Text.ToUpper();
                TB.SelectionStart = TB.Text.Length;
            }
        }

        public string utf8_to_iso(string Message)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-15");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(Message);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            string msg = iso.GetString(isoBytes);
            return msg;
        }

        //equivalent Coalesce SQL;
        private string Coalesce(string Value, string Default)
        {
            if (Value != string.Empty)
                return Value;

            return Default;
        }

        private void SetTPResumeText()
        {
            string S = "";
            if (TB_Resume.Text != "")
                S += "R";
            if (TB_Sommaire.Text != "")
                S += "S";

            if (S != "")
                S = " (" + S + ")";

            TP_Resume.Text = "Resume" + S;
        }

        public string ParseCodeGenre(string s)
        {
            while (s.Length < 8)
            {
                s = "0" + s;
            }
            return s.Substring(0, 2) + "-" + s.Substring(2, 2) + "-" + s.Substring(4, 2) + "-" + s.Substring(6, 2);
        }

        private void ReplaceOfficeQuotes(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ReplaceOfficeChars(((TextBox)sender).Text);
        }

        private string ReplaceOfficeChars(string s)
        {
            //ATTENTION les 2 apostrophes remplacés ci dessous sont différents...
            return s.Replace("ʼ", "'").Replace("’", "'").Replace("…", "...");
        }

        private void SetDistriComboBox()
        {
            try
            {
                if (DEditeur.ContainsKey(CB_Editeur.Text.Trim()))
                {
                    CB_Distri.Items.Clear();
                    foreach (var pair in DEditeur[CB_Editeur.Text.Trim()])
                    {
                        CB_Distri.Items.Add(pair.Value);
                    }

                    if (!DEditeur[CB_Editeur.Text].ContainsValue(CB_Distri.Text))
                        CB_Distri.Text = DEditeur[CB_Editeur.Text][0];
                }

                if (CB_Distri.Items.Contains(TB_LIBDISTRI.Text))
                    CB_Distri.Text = TB_LIBDISTRI.Text;
            }
            catch
            { }
        }

        private void CheckProductVersion()
        {
            string SLCheckVersion = string.Empty;

            if (File.Exists(@"C:\Soft\Updater\Updater.exe"))
            {
                FDB UserDB = new FDB(ini.IniReadValue("USERBDD", "USER") + ":" +
                                     ini.IniReadValue("USERBDD", "PASS") + "@" +
                                     ini.IniReadValue("USERBDD", "HOST") + ":" +
                                     ini.IniReadValue("USERBDD", "NAME"));

                bool beta = false;
                UserDB.Query("select * from t_logiciels where application='" + Application.ProductName.Replace("'", "''") + "'");
                foreach (Row row in UserDB.FetchAll())
                {
                    if (row["betaversion"].ToString() == "1" && row["currentversion"].ToString() == ProductVersion)
                        beta = true;

                    if (row["betaversion"].ToString() != "1")
                        SLCheckVersion = row["currentversion"].ToString();
                }

                if (!beta && !string.IsNullOrEmpty(SLCheckVersion) && SLCheckVersion != ProductVersion)
                {
                    DialogResult dr = MessageBox.Show("Une nouvelle version du logiciel est disponible\r\n" +
                                                      "Voulez vous lancer la mise à jour ?", "confirmation", MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Exclamation);

                    //Application.Exit();
                    if (dr == DialogResult.Yes)
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("updater.exe");
                        psi.WorkingDirectory = @"c:\soft\updater";
                        psi.Arguments = string.Format("\"{0}\" \"{1}\" {2}", Application.ExecutablePath, Application.ProductName, Application.ProductVersion);
                        Process.Start(psi);
                        Application.Exit();
                    }
                }
                UserDB.Close();
            }
        }

        private void ReplaceGenrePrincipalInListBox(string selected, string memo, int i)
        {
            MyListBoxItem lbi = new MyListBoxItem(Color.Blue, selected, boldfont);

            LB_Genres.Items.RemoveAt(i);
            LB_Genres.Items.Add(lbi);


            int oi = 0;
            foreach (var item in LB_Genres.Items)
            {
                if (((MyListBoxItem)item).Message == memo)
                    break;
                oi++;
            }

            MyListBoxItem oldlbi2 = new MyListBoxItem(SystemColors.WindowText, memo, deffont);

            LB_Genres.Items.RemoveAt(oi);
            LB_Genres.Items.Add(oldlbi2);
        }

        private void SetCompteurListeSaisie()
        {
            string s = "";
            int c = LB_Saisie_EAN.Items.Count;
            if (c > 1)
                s = "s";
            lb_count_a_faire.Text = c.ToString() + " référence" + s;
        }

        private void SetNewTitre()
        {
            TB_NewTitre.Text = TB_Titre.Text;

            string Edition = string.Empty;
            if (TB_Edition1.Text != "")
            {
                if (TB_Edition1.TextLength > 3)
                {
                    Edition = string.Format(" (edition {0}", TB_Edition1.Text);
                    if (TB_Edition2.Text != "")
                        Edition += "/" + TB_Edition2.Text + ")";
                    else
                        Edition += ")";
                }
                else if (TB_Edition1.Text == "1")
                    Edition = " (1re edition)";
                else
                    Edition = string.Format(" ({0}e edition)", TB_Edition1.Text);
            }

            if(!string.IsNullOrEmpty(Edition))
                TB_NewTitre.Text += Edition;
        }

        #region GTL

        private string SetPreviousGenreToAllDigits(string s)
        {
            string DefaultCode = "00000000:1";
            string n = "";
            foreach (string x in s.Split(';'))
            {
                string X = x;
                while (X.Length < DefaultCode.Length)
                {
                    X = "0" + X;
                }
                n += X + ";";
            }
            return n.Substring(0, n.Length - 1);
        }

        private string RewriteGenre(string code)
        {
            string ret = "";

            while (code.Length < 10)
                code = "0" + code;

            string C = code.Split(':')[0];
            string T = code.Split(':')[1];


            for (int i = 0; i < C.Length; i++)
            {
                ret += C[i];
                if (i % 2 > 0)
                    ret += "-";
            }

            //00-00-00-00:1
            ret = ret.Substring(0, ret.Length - 1) + ":" + T;

            return ret;
        }

        #endregion

        #region Memo_F12

        /// <summary>
        /// utilisation de la classe Memorizer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F12)
            {
                //traitement spécial pour les langues
                if (((Control)sender).Name == "TB_Langue_Concat")
                {
                    TB_Langue_Concat.Text = "";

                    //remise à zero des champs et combobox
                    TB_Langue_0.Text = "0";
                    CB_Langue_0.Text = "";
                    TB_Langue_1.Text = "0";
                    CB_Langue_1.Text = "";
                    TB_Langue_2.Text = "0";
                    CB_Langue_2.Text = "";
                }

                M.Get((Control)sender);

                //pour tous les combobox, on change l'index
                if (((Control)sender).Name == "CB_Collection")
                {
                    ChoixCollection(sender);
                }
            }
        }

        private void MemorizerSet(object sender, EventArgs e)
        {
            M.Set((Control)sender);
        }

        #endregion

        #region controle integrité d'isbn 10/13

        /// <summary>
        /// Controle la validité d'un code barres
        /// </summary>
        /// <param name="EAN"></param>
        /// <returns></returns>
        public static string CheckEAN(string EAN)
        {
            string EAN13 = string.Empty;
            try
            {
                int Total = 0;
                string EAN12 = EAN.Substring(0, 12);
                int fact = 3;

                for (int i = 12; i > 0; i--)
                {
                    string subEan = EAN12.Substring(i - 1, 1);
                    Total += (Convert.ToInt32(subEan) * fact);
                    fact = 4 - fact;
                }
                int key = 10 - Convert.ToInt32(Total.ToString().Substring(Total.ToString().Length - 1, 1));
                EAN13 = EAN12 + key.ToString().Substring(key.ToString().Length - 1, 1);

            }
            catch
            {
                EAN13 = "*";
            }

            return EAN13;
        }

        public static string CheckISBN(string ISBN)
        {
            ISBN = ISBN.Trim();

            if (ISBN.Length == 13)
                ISBN = ISBN.Substring(3, 9);

            else if (ISBN.Length == 10)
                ISBN = ISBN.Substring(0, 9);

            int coef = 10;
            int calc = 0;
            string cle = "0";
            string ISBN10 = string.Empty;

            for (int i = 1; i <= 10; i++)
            {
                calc = Convert.ToInt32(ISBN.Substring(i - 1, 1)) * coef;
                coef--;
            }
            cle = (11 - (calc % 11)).ToString();
            if (cle == "10")
                cle = "X";
            else if (cle == "11")
                cle = "0";

            ISBN10 = ISBN + cle;
            return ISBN10;
        }

        public static string ISBN2CAB(string isbn)
        {
            return CheckEAN("978" + isbn);
        }

        private string StripEAN(string ean)
        {
            string Ean = ean.ToUpper();
            Regex cabOnly = new Regex(@"[^\d|X]");
            return cabOnly.Replace(Ean, "");
        }

        #endregion 

        #region traitement des images

        private Image SetImageToPictureBox(PictureBox PB, int ImageType)
        {
            Image Img;
            string tempfile = Path.Combine(Directory.GetCurrentDirectory(), TB_EAN.Text + "_" + ImageType.ToString() + "_75.jpg");

            if (TB_Image.Text == "1" || TB_Image.Text == "2")
            {
                try
                {
                    wc.DownloadFile(SetURL(ImageType), tempfile);
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }

            //on essaye d'ouvrir l'image. si TB_Image.Text = 0, pas d'image --> erreur --> visuel par défaut.
            try
            {
                FileStream stream = new FileStream(tempfile, FileMode.Open, FileAccess.Read);
                Img = Image.FromStream(stream);
                PB.Image = Img;
                stream.Close();
                stream.Dispose();
                File.Delete(tempfile);
                wc.Dispose();
            }
            catch
            {
                Img = SaisieLivre.Properties.Resources.image_a_venir;
                PB.Image = Img;
            }
            return Img;
        }

        private string SetURL(int ImageType)
        {

            string cab = TB_EAN.Text;
            if (cab.Length > 13)
            {
                cab = cab.Substring(0, 13);
            }


            string Directory = "";
            if (cab.Length >= 13)
                Directory = cab.Substring(10, 3);
            else
                Directory = "000";

            string URL = BaseURL.Replace("#", Directory);
            URL = URL.Replace("@", cab);
            URL = URL.Replace("$", ImageType.ToString());

            return URL;
        }

        #endregion


        private void RewriteTitreFromSerieColl()
        {
            RemoveCollectionFromTitre();
            RemoveSerieFromTitre();

            if (TB_Present_Titre_Serie.Text == "1")
                SetTitreFromPresentTitreSerie();

            if (TB_PresentTitre.Text == "1")
            {
                SetTitreFromPresentTitreColl(true);
            }

            if (TB_Titre.Text == TB_Info_Coll.Text + " ; " + TB_Info_Serie.Text && TB_Libelle.Text != "")
                TB_Titre.Text += TB_Titre.Text + " ; " + RemoveDiacritics(TB_Libelle.Text).ToUpper();
            
        }


        #region Series
        
        private void SetTitreFromPresentTitreSerie()
        {
            if (TB_Present_Titre_Serie.Text == "1")
            {
                string _Titre = RemoveDiacritics(TB_Libelle.Text).ToUpper(); //TB_Titre.Text;

                string SerieTom = RemoveDiacritics( TB_LibAffichageSerie.Text ).ToUpper();

                if (CBX_Serie_HorsSerie.Checked)
                    SerieTom += " HORS-SERIE";
                

                if (CBX_Serie_Coffret.Checked)
                {
                    SerieTom += " ; COFFRET";
                    if (!CBX_Serie_Integrale.Checked)
                    {
                        if (!TB_Serie_Contenu_1.Enabled && !TB_Serie_Contenu_2.Enabled)
                        {
                            TB_Serie_Contenu_1.Enabled = true;
                            TB_Serie_Contenu_2.Enabled = true;
                        }

                        if (!string.IsNullOrEmpty(TB_NoSerie.Text))
                        {
                            if (TB_NoSerie.Text!="0")
                            {
                                SerieTom += " VOL." + TB_NoSerie.Text;

                                if (TB_Serie_Contenu_1.Text != "0" && TB_Serie_Contenu_1.Text != "" && 
                                    TB_Serie_Contenu_2.Text != "0" && TB_Serie_Contenu_2.Text != "" && 
                                    Convert.ToInt32(TB_Serie_Contenu_2.Text) > Convert.ToInt32(TB_Serie_Contenu_1.Text))
                                    SerieTom += " ;";
                            }
                        }
                    }
                }

                if (CBX_Serie_Integrale.Checked)
                {
                    if (!TB_Serie_Contenu_1.Enabled && !TB_Serie_Contenu_2.Enabled)
                    {
                        TB_Serie_Contenu_1.Enabled = true;
                        TB_Serie_Contenu_2.Enabled = true;
                    }

                    if (CBX_Serie_Coffret.Checked)
                        SerieTom += " INTEGRALE";
                    else
                        SerieTom += " ; INTEGRALE";

                    if (!string.IsNullOrEmpty(TB_NoSerie.Text))
                    {
                        if (TB_NoSerie.Text!="0")
                        {
                            SerieTom += " VOL." + TB_NoSerie.Text;

                            if (TB_Serie_Contenu_1.Text != "0" && TB_Serie_Contenu_1.Text != "" && 
                                TB_Serie_Contenu_2.Text != "0" && TB_Serie_Contenu_2.Text != "" &&
                                Convert.ToInt32(TB_Serie_Contenu_2.Text) > Convert.ToInt32(TB_Serie_Contenu_1.Text))
                                SerieTom += " ;";
                        }
                    }
                }
                               
                string SerieTomSansPointVirgule = SerieTom;

                if ((CBX_Serie_Coffret.Checked || CBX_Serie_Integrale.Checked) &&
                     TB_Serie_Contenu_1.Text != "0" && TB_Serie_Contenu_2.Text != "0" && TB_Serie_Contenu_1.Text != "" &&
                     TB_Serie_Contenu_2.Text != "" && Convert.ToInt32(TB_Serie_Contenu_2.Text) > Convert.ToInt32(TB_Serie_Contenu_1.Text))
                {

                    /*if (string.IsNullOrEmpty(TB_Volume_Serie.Text) && CBX_Serie_Coffret.Checked)
                    {
                        SerieTom += " ;";
                    }*/

                    string SepTom = " T.";
                    if (TB_CodeSupport.Text == "C" || TB_CodeSupport.Text == "R")
                        SepTom = " N.";

                    if (TB_Serie_Contenu_1.Text != "0" && TB_Serie_Contenu_1.Text != "")
                    {
                        SerieTom += SepTom + TB_Serie_Contenu_1.Text;

                        if (TB_Serie_Contenu_2.Enabled && TB_Serie_Contenu_2.Text != "0" && TB_Serie_Contenu_2.Text != "")
                        {
                            try
                            {
                                if (Convert.ToInt32(TB_Serie_Contenu_2.Text) == Convert.ToInt32(TB_Serie_Contenu_1.Text) + 1)
                                {
                                    SerieTom += " ET" + SepTom + TB_Serie_Contenu_2.Text;
                                }
                                else if (Convert.ToInt32(TB_Serie_Contenu_2.Text) > Convert.ToInt32(TB_Serie_Contenu_1.Text) + 1)
                                {
                                    SerieTom += " A" + SepTom + TB_Serie_Contenu_2.Text;
                                }
                            }
                            catch
                            { }
                        }
                    }                                       
                }

                if (!CBX_Serie_Coffret.Checked && !CBX_Serie_Integrale.Checked)
                {
                    string ST = " T.";
                    if (TB_CodeSupport.Text == "C" || TB_CodeSupport.Text == "R")
                        ST = " N.";

                    if (!string.IsNullOrEmpty(TB_NoSerie.Text) && TB_NoSerie.Text != "0")
                    {
                        SerieTom += ST + TB_NoSerie.Text;
                        
                        if (TB_Serie_Contenu_1.Enabled && TB_Serie_Contenu_1.Text != "0" && TB_Serie_Contenu_1.Text != "")
                        {
                            try
                            {
                                if (Convert.ToInt32(TB_Serie_Contenu_1.Text) == Convert.ToInt32(TB_NoSerie.Text) + 1)
                                {
                                    SerieTom += " ET" + ST + TB_Serie_Contenu_1.Text;
                                }
                                else if (Convert.ToInt32(TB_Serie_Contenu_1.Text) > Convert.ToInt32(TB_NoSerie.Text) + 1)
                                {
                                    SerieTom += " A" + ST + TB_Serie_Contenu_1.Text;
                                }
                            }
                            catch
                            { }
                        }

                    }


                }

                if (SerieTom != "")
                {
                    SerieTomSansPointVirgule = SerieTom;
                    SerieTom += " ; ";
                }


                //on teste le titre pour savoir si il contient déjà l'info, si c'est le cas, on ne va pas plus loin.
                if (SerieTom.Length <= _Titre.Length && _Titre.Substring(0, SerieTom.Length) == SerieTom)
                {                    
                    return;
                }

                //si le titre contient uniquement collection + tomaison : 
                else if (SerieTomSansPointVirgule.Length <= _Titre.Length && _Titre.Substring(0, SerieTomSansPointVirgule.Length) == SerieTomSansPointVirgule)
                {                    
                    return;
                }

                else
                {
                    TB_Titre.Text = SerieTom + _Titre;
                    TB_Info_Serie.Text = SerieTom;

                    if (string.IsNullOrEmpty(_Titre))
                    {
                        TB_Titre.Text = SerieTomSansPointVirgule;
                        TB_Info_Serie.Text = SerieTomSansPointVirgule;
                    }
                }               
            }
        }
    
        
        private void RemoveSerieFromTitre()
        {
            if (TB_Info_Serie.Text!="")
                TB_Titre.Text = TB_Titre.Text.Replace(TB_Info_Serie.Text, "");           
        }
        
        #endregion

        #region Collection

        private void SetTitreFromPresentTitreColl(bool GetPreviousTitre=false)
        {
            if (TB_PresentTitre.Text == "1")
            {
                string _Titre = RemoveDiacritics(TB_Libelle.Text).ToUpper(); //TB_Titre.Text;
                if (GetPreviousTitre)
                    _Titre = TB_Titre.Text;
                
                //on reconstruit l'info COLLECTION T.Tomaison ;
                string CollTom = RemoveDiacritics(CB_Collection.Text).ToUpper();

                if (CBX_Coll_HorsSerie.Checked)
                    CollTom += " HORS-SERIE";

                if (CBX_Coll_Coffret.Checked)
                {
                    if (!TB_Coll_Contenu_1.Enabled && !TB_Coll_Contenu_2.Enabled)
                    {
                        TB_Coll_Contenu_1.Enabled = true;
                        TB_Coll_Contenu_2.Enabled = true;
                    }

                    CollTom += " ; COFFRET";
                    if (!string.IsNullOrEmpty(TB_Nocoll.Text))
                    {
                        if (TB_Nocoll.Text!="0")
                        {
                            CollTom += " VOL." + TB_Nocoll.Text;
                            
                            if (TB_Coll_Contenu_1.Text != "0" && TB_Coll_Contenu_1.Text != "" &&
                                TB_Coll_Contenu_2.Text != "0" && TB_Coll_Contenu_2.Text != "" &&
                                Convert.ToInt32(TB_Coll_Contenu_2.Text) > Convert.ToInt32(TB_Coll_Contenu_1.Text))
                            {
                                CollTom += " ;";
                            }
                        }
                    }                    
                }

                string CollTomSansPointVirgule = CollTom;

                if ( CBX_Coll_Coffret.Checked && TB_Coll_Contenu_1.Text != "0" &&
                     TB_Coll_Contenu_2.Text != "0" && TB_Coll_Contenu_1.Text != "" &&
                     TB_Coll_Contenu_2.Text != "" &&
                     Convert.ToInt32(TB_Coll_Contenu_2.Text) > Convert.ToInt32(TB_Coll_Contenu_1.Text))
                {

                    /*if (string.IsNullOrEmpty(TB_Volume_Collection.Text) && CBX_Coll_Coffret.Checked)
                    {
                        CollTom += " ;";
                    }*/

                    string SepTom = " T.";

                    if (TB_CodeSupport.Text == "C" || TB_CodeSupport.Text == "R")
                        SepTom = " N.";

                    if (TB_Coll_Contenu_1.Text != "0" && TB_Coll_Contenu_1.Text != "")
                    {
                        CollTom += SepTom + TB_Coll_Contenu_1.Text;

                        if (TB_Coll_Contenu_2.Enabled && TB_Coll_Contenu_2.Text != "0" && TB_Coll_Contenu_2.Text != "")
                        {
                            try
                            {
                                if (Convert.ToInt32(TB_Coll_Contenu_2.Text) == Convert.ToInt32(TB_Coll_Contenu_1.Text) + 1)
                                {
                                    CollTom += " ET" + SepTom + TB_Coll_Contenu_2.Text;
                                }
                                else if (Convert.ToInt32(TB_Coll_Contenu_2.Text) > Convert.ToInt32(TB_Coll_Contenu_1.Text) + 1)
                                {
                                    CollTom += " A" + SepTom + TB_Coll_Contenu_2.Text;
                                }
                            }
                            catch 
                            {  }
                        }
                    }
                }

                if (!CBX_Coll_Coffret.Checked)
                {
                    string ST = " T.";
                    if (TB_CodeSupport.Text == "C" || TB_CodeSupport.Text == "R")
                        ST = " N.";

                    if (!string.IsNullOrEmpty(TB_Nocoll.Text) && TB_Nocoll.Text != "0")
                    {
                        CollTom += ST + TB_Nocoll.Text;

                        if (TB_Coll_Contenu_1.Enabled && TB_Coll_Contenu_1.Text != "0" && TB_Coll_Contenu_1.Text != "")
                        {
                            try
                            {
                                if (Convert.ToInt32(TB_Coll_Contenu_1.Text) == Convert.ToInt32(TB_Nocoll.Text) + 1)
                                {
                                    CollTom += " ET" + ST + TB_Coll_Contenu_1.Text;
                                }
                                else if (Convert.ToInt32(TB_Coll_Contenu_1.Text) > Convert.ToInt32(TB_Nocoll.Text) + 1)
                                {
                                    CollTom += " A" + ST + TB_Coll_Contenu_1.Text;
                                }
                            }
                            catch
                            { }
                        }

                    }
                }


                if (CollTom != "")
                {
                    CollTomSansPointVirgule = CollTom;
                    CollTom += " ; ";
                }

                //on teste le titre pour savoir si il contient déjà l'info, si c'est le cas, on ne va pas plus loin.
                if (CollTom.Length <= _Titre.Length && _Titre.Substring(0, CollTom.Length) == CollTom)
                    return;

                //si le titre contient uniquement collection + tomaison : 
                else if (CollTomSansPointVirgule.Length <= _Titre.Length && _Titre.Substring(0, CollTomSansPointVirgule.Length) == CollTomSansPointVirgule)
                    return;

                else
                {
                    TB_Titre.Text = CollTom + _Titre;
                    TB_Info_Coll.Text = CollTom;

                    if (string.IsNullOrEmpty(_Titre))
                    {
                        TB_Titre.Text = CollTomSansPointVirgule;
                        TB_Info_Coll.Text = CollTomSansPointVirgule;
                    }
                }
            }
        }      

        private void RemoveCollectionFromTitre(string MemoCollection="")
        {
            if (TB_Info_Coll.Text!="")
                TB_Titre.Text = TB_Titre.Text.Replace(TB_Info_Coll.Text, "");
        }

        private void DisplayCollection(Row CollectionRow, object sender)
        {
            string result = "", result2 = "";
            string resultserie = "";
            TB_LibAffichageSerie.Text = "";

            List<string> LS = new List<string>();
            LS.Add("livre_lu");
            LS.Add("grands_cara");
            LS.Add("multilingue");
            LS.Add("illustre");
            LS.Add("iad");
            LS.Add("luxe");
            LS.Add("cartonne");
            LS.Add("relie");
            LS.Add("broche");
            LS.Add("nbrpage");
            LS.Add("hauteur");
            LS.Add("largeur");

            if (CollectionRow != null)
            {
                foreach (var row in CollectionRow)
                {
                    if (row.Key != "id" && row.Key != "test_gtl_second" && row.Key != "id_lectorat")
                    {
                        if (LS.Contains(row.Key.ToString().Trim()))
                        {
                            //MessageBox.Show(row.Key);
                            result2 += row.Key + "~" + row.Value.ToString() + "\r\n";
                        }
                        else
                            result += row.Key + "~" + row.Value.ToString() + "\r\n";                                                                       
                    }

                    else if (row.Key == "test_gtl_second")
                    {
                        string gtl2 = "genre_secondaire_";
                        int i = 1;
                        foreach (string s in row.Value.ToString().Split(';'))
                        {
                            if (s != "")
                                result += gtl2 + i.ToString() + "~" + s + "\r\n";
                            i++;
                        }
                    }
                }
                
                
            }

            if (TB_IDLibSerie.Text != "")
            {
                db.Query(string.Format(Properties.Resources.QuerySelectSeries, TB_IDLibSerie.Text));
                foreach (Row row in db.FetchAll())
                {                    
                    SerieRow = row;

                    foreach (var pair in row)
                    {                        
                        if (pair.Key != "id" && pair.Key != "test_gtl_second" && pair.Key != "id_lectorat")
                        {
                            resultserie += pair.Key + "~" + pair.Value.ToString() + "\r\n";

                            if (pair.Key == "present_titre")
                                TB_Present_Titre_Serie.Text = pair.Value.ToString();

                            if (pair.Key == "libelle_affichage")
                                TB_LibAffichageSerie.Text = pair.Value.ToString();

                        }
                        else if (pair.Key.ToString() == "test_gtl_second")
                        {
                            string gtl2 = "genre_secondaire_";
                            int i = 1;
                            foreach (string s in pair.Value.ToString().Split(';'))
                            {
                                if (s != "")
                                    resultserie += gtl2 + i.ToString() + "~" + s + "\r\n";
                                i++;
                            }
                        }
                    }
                }              
            }

            if (CBX_AffichageInfoColl.Checked)
            {
                CD.TB_Serie.Text = resultserie;
                CD.TB_Dilicom_ToLV(1);

                CD.TB_Dilicom.Text = result;
                CD.TB_Coll2.Text = result2;

                CD.TB_Dilicom_ToLV();
                CD.StartPosition = FormStartPosition.Manual;
                CD.Text = "Collection & Série";
                CD.Font = LoadedFont;
                CD.Show();

                if (resultserie != "")
                {
                    CD.tabControl1.SelectedIndex = 1;
                    CD.tabControl1.Select();
                }
                else
                {
                    CD.tabControl1.SelectedIndex = 0;
                    CD.tabControl1.Select();
                }
                //rend le focus au champ collection
                this.Focus();

                ((Control)sender).Focus();
            }

            if (resultserie!="")
                AlternateValues();
        }




        private void AlternateValues()
        {
            bool AltWork = false;

            if (SerieRow != null)
            {
                foreach (var pair in CollectionData)
                {
                    string Field = pair.Key;

                    if (SerieData.ContainsKey(Field) && SerieRow.ContainsKey(Field))
                    {
                        string FieldValue = SerieRow[Field].ToString();

                        FData fd = SerieData[Field];

                        if (fd.Label.Name == "l_genreprincipal")
                        {
                            string cgp = FieldValue;
                            while (cgp.Length < 8)
                                cgp = "0" + cgp;

                            FieldValue = cgp.Substring(0, 2) + "-" + cgp.Substring(2, 2) + "-" + cgp.Substring(4, 2) + "-" + cgp.Substring(6, 2);

                            if (FieldValue == "00-00-00-00")
                                FieldValue = "";
                        }

                        if (fd.Control.Text != FieldValue && !string.IsNullOrEmpty(FieldValue))
                        {

                            fd.Control.BackColor = Seri_Diff_Back;

                            fd.Label.ForeColor = Seri_Diff_Fore;

                            if (fd.LabelText != "")
                                fd.Label.Text = fd.LabelText;

                            if (fd.Control2 != null)
                                fd.Control2.BackColor = Seri_Diff_Back;

                            if (fd.Control3 != null)
                                fd.Control3.BackColor = Seri_Diff_Back;

                            if (fd.Control4 != null)
                                fd.Control4.BackColor = Seri_Diff_Back;

                        }

                        else if ((!SerieRow.ContainsKey(fd.Field) || string.IsNullOrEmpty(SerieRow[fd.Field].ToString())) &&
                                  CollectionRow != null && CollectionRow.ContainsKey(fd.Field) && !string.IsNullOrEmpty(CollectionRow[fd.Field].ToString()))
                        {
                            bool work = false;

                            if (!SerieData.ContainsKey(fd.Field))
                                work = true;
                            else
                            {
                                FData fds = SerieData[fd.Field];
                                if (fds.Control.BackColor != Seri_Diff_Back)
                                    work = true;
                            }

                            if (work)
                            {
                                FData fdc = CollectionData[fd.Field];

                                fdc.Control.BackColor = Coll_Diff_Back;

                                fdc.Label.ForeColor = Coll_Diff_Fore;

                                if (fdc.LabelText != "")
                                    fdc.Label.Text = fd.LabelText;

                                if (fdc.Control2 != null)
                                    fdc.Control2.BackColor = Coll_Diff_Back;

                                if (fdc.Control3 != null)
                                    fdc.Control3.BackColor = Coll_Diff_Back;

                                if (fdc.Control4 != null)
                                    fdc.Control4.BackColor = Coll_Diff_Back;
                            }
                        }
                        else
                        {
                            fd.Control.BackColor = SystemColors.Window;

                            switch (fd.Control.Name)
                            {
                                case "TB_Libelle":
                                case "TB_Auteurs":
                                case "CB_Collection":
                                case "TB_Commentaire":
                                case "CB_Traducteur":
                                    fd.Label.ForeColor = Color.Blue;
                                    break;

                                default:
                                    fd.Label.ForeColor = SystemColors.ControlText;
                                    break;
                            }

                            if (fd.LabelTextOrigine != "")
                                fd.Label.Text = fd.LabelTextOrigine;

                            if (fd.Control2 != null)
                                fd.Control2.BackColor = SystemColors.Window;

                            if (fd.Control3 != null)
                                fd.Control3.BackColor = SystemColors.Window;

                            if (fd.Control4 != null)
                                fd.Control4.BackColor = SystemColors.Window;
                        }
                    }
                    else
                    {
                        AltWork = true;
                    }
                }
            }
            else
                AltWork = true;


            // pas de série, on test la collection
            if (CollectionRow != null && AltWork)
            {
                foreach (var pair in CollectionData)
                {
                    string Field = pair.Key;
                    string FieldValue = CollectionRow[Field].ToString();
                    FData fd = pair.Value;
                    if (fd.Label.Name == "l_genreprincipal")
                    {
                        string cgp = FieldValue;
                        while (cgp.Length < 8)
                            cgp = "0" + cgp;

                        FieldValue = cgp.Substring(0, 2) + "-" + cgp.Substring(2, 2) + "-" + cgp.Substring(4, 2) + "-" + cgp.Substring(6, 2);

                        if (FieldValue == "00-00-00-00")
                            FieldValue = "";
                    }

                    //if (!string.IsNullOrEmpty(CollectionRow[fd.Field].ToString()) && fd.Control.Text != FieldValue && !string.IsNullOrEmpty(FieldValue))
                    if ((SerieRow == null || !SerieRow.ContainsKey(fd.Field) || string.IsNullOrEmpty(SerieRow[fd.Field].ToString())) &&
                        (CollectionRow.ContainsKey(fd.Field) && fd.Control.Text != FieldValue && !string.IsNullOrEmpty(FieldValue)))
                    {
                        fd.Control.BackColor = Coll_Diff_Back;

                        fd.Label.ForeColor = Coll_Diff_Fore;

                        if (fd.LabelText != "")
                            fd.Label.Text = fd.LabelText;

                        if (fd.Control2 != null)
                            fd.Control2.BackColor = Coll_Diff_Back;

                        if (fd.Control3 != null)
                            fd.Control3.BackColor = Coll_Diff_Back;

                        if (fd.Control4 != null)
                            fd.Control4.BackColor = Coll_Diff_Back;
                    }

                    else if (fd.Control.BackColor != Seri_Diff_Back) 
                    {
                        fd.Control.BackColor = SystemColors.Window;

                        switch (fd.Control.Name)
                        {
                            case "TB_Libelle":
                            case "TB_Auteurs":
                            case "CB_Collection":
                            case "TB_Commentaire":
                            case "CB_Traducteur":
                                fd.Label.ForeColor = Color.Blue;
                                break;

                            default:
                                fd.Label.ForeColor = SystemColors.ControlText;
                                break;
                        }

                        if (fd.LabelTextOrigine != "")
                            fd.Label.Text = fd.LabelTextOrigine;

                        if (fd.Control2 != null)
                            fd.Control2.BackColor = SystemColors.Window;

                        if (fd.Control3 != null)
                            fd.Control3.BackColor = SystemColors.Window;

                        if (fd.Control4 != null)
                            fd.Control4.BackColor = SystemColors.Window;
                    }
                }                
            }            
            SD.TB_Dilicom_ToLV();
            CD.TB_Dilicom_ToLV();
        }

        private void ChoixCollection(object sender)
        {
            RemoveCollectionFromTitre(MemoCollection);

            MemoCollection = "";

            CB_Collection.ForeColor = SystemColors.ControlText;

            if (DCollection.ContainsKey(CB_Collection.Text))
            {
                MemoCollection = CB_Collection.Text;

                CollectionRow = DCollection[CB_Collection.Text];

                AlternateValues();
                
                DisplayCollection(CollectionRow, CB_Collection);

                CB_SousCollection.Items.Clear();

                bool FoundSousColl = false;

                if (TB_SousCollection.Text == "")
                    TB_SousCollection.Text = "0";

                //ici on force la sous collection "0 NON PRECISE" pour qu'elle n'apparaisse pas en tant qu'erreur. 
                if (!DSousCollByColl.ContainsKey(TB_IDCollection.Text))
                {
                    List<string> LS = new List<string>();
                    LS.Add("0");
                    DSousCollByColl.Add(TB_IDCollection.Text, LS);
                }

                if (DSousCollByColl.ContainsKey(TB_IDCollection.Text))
                {
                    foreach (string S in DSousCollByColl[TB_IDCollection.Text])
                    {
                        CB_SousCollection.Items.Add(FindIdByLib(DSousCollection, S));
                        if (S == TB_SousCollection.Text)
                            FoundSousColl = true;
                    }
                    if (!FoundSousColl && TB_SousCollection.Text == "0")
                    {
                        FoundSousColl = true;
                    }
                }

                if (!FoundSousColl)
                {
                    CB_SousCollection.ForeColor = Color.Red;
                }
                else
                    CB_SousCollection.ForeColor = SystemColors.WindowText;

                string CollName = CollectionRow["collection"].ToString();

                //Present Titre
                TB_PresentTitre.Text = CollectionRow["present_titre"].ToString();

                //on applique quoi qu'il arrive
                if (TB_PresentTitre.Text == "1")
                {
                    RewriteTitreFromSerieColl();
                }

                #region pré remplissage  du lectorat depuis la collection (désactivé)
                // tester le champ lectorat et mettre a jour si 0 ou vide (9782010183928)
                /*if (CollectionRow["id_lectorat"].ToString() != "0" &&
                    !string.IsNullOrEmpty(CollectionRow["id_lectorat"].ToString()))
                {
                    TB_AltLectorat.Text = CollectionRow["id_lectorat"].ToString();

                    if (TB_Lectorat.Text == "" || TB_Lectorat.Text == "0")
                    {
                        TB_Lectorat.Text = TB_AltLectorat.Text;
                    }
                    else
                    {
                        if (TB_Lectorat.Text != TB_AltLectorat.Text)
                            CB_Lectorat.BackColor = Color.Aquamarine;
                    }
                }*/
                #endregion

                //en cas de création, on applique certaines valeurs par défaut en fonction de la collection selectionnée.
                if (!Found)
                {
                    foreach (var pair in CollFields)
                    {
                        string collfield = pair.Key;

                        if (collfield == "present_titre")
                        {
                            if (CollectionRow[collfield].ToString() == "1")
                            {
                                if (TB_Titre.Text.Length < CollName.Length || TB_Titre.Text.Substring(0, CollName.Length) != CollName)
                                {
                                    TB_Titre.Text = CollName.Trim() + " ; " + TB_Titre.Text.Trim();
                                }
                            }
                        }

                        else
                        {

                            if (CollectionRow[collfield].ToString() != "" && (pair.Value.Text == "" ||
                                                                              pair.Value.Text == "0" ||
                                                                              collfield == "codesupport"))
                            {
                                pair.Value.Text = CollectionRow[collfield].ToString();

                                if (collfield == "presentation")
                                {
                                    SetTPResumeText();
                                }

                            }
                        }
                    }
                }


                //mecanisme de vérification de la présence du libelle collection dans le champ libellé.
                //si c'est le cas, on affiche le contenu du champ en rouge.

                if ((!string.IsNullOrEmpty(CB_Collection.Text)) && (TB_Libelle.TextLength >= CB_Collection.Text.Length))
                {
                    if (RemoveDiacritics(TB_Libelle.Text.ToUpper().Substring(0, CB_Collection.Text.Length)) == CB_Collection.Text)
                    {
                        TB_Libelle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                //DisplayCollectionvide comme dilicom, on masque la fenetre si le champ id coll==string.Empty;
                if (TB_IDCollection.Text != "")
                    DisplayCollection(null, CB_Collection);

                MemoCollection = CB_Collection.Text;
            }
        }

        #endregion

        #region Validation

        private string GetGenresFromListBox()
        {
            string result = string.Empty;

            for (int i = 0; i < LB_Genres.Items.Count; i++)
            {
                object item = LB_Genres.Items[i];
                string s = ((MyListBoxItem)item).Message.Replace("-", "");
                string p = "0";
                if (s == TB_GenrePrincipal.Text)
                    p = "1";

                result += s + ":" + p + ";";
            }

            result = result.Substring(0, result.Length - 1);
            return result;
        }

        //sauvegarde les fonctions par auteurs
        private void WorkAuteurFonctions()
        {
            db.Query(string.Format("delete from AUTEURS_FONCTIONS where gencod='{0}'", TB_EAN.Text.Substring(0, 13)));

            for (int i = 0; i < LB_AutFunc.Items.Count; i++)
            {
                string codes = LB_AutFuncComp.Items[i].ToString();
                foreach (string code in codes.Split(','))
                {
                    string _code = code.Trim();

                    if (_code != "")
                        db.Query(string.Format("INSERT INTO AUTEURS_FONCTIONS values ( '{0}', '{1}', '{2}' )",
                                                  TB_EAN.Text,
                                                  LB_AutFunc.Items[i].ToString().Replace("'", "''"),
                                                  _code));
                }
            }
        }

        //sauvegarde des présentations / résumés
        private void maj_table_by_idlivre(string idlivre, string field, string NewText)
        {
            db.Query(string.Format("delete from {0}s where idlivre='{1}'", field, idlivre));

            if (NewText != "")
                db.Query(string.Format("insert into {0}s ( idlivre, {0}, datum, mbexport ) " +
                                       "values ( '{1}', '{2}', 'now', '1' )", field, idlivre, ReplaceOfficeChars(NewText).Replace("'", "''")));
        }

        //sauvegarde des genres
        private void WorkGenres(string PreviousGenre, string IDLivre, bool delete = true)
        {
            string GenreString = "";
            foreach (MyListBoxItem s in LB_Genres.Items)
            {
                string p = "0";
                if (s.Message == TB_GenrePrincipal.Text)
                    p = "1";

                GenreString += s.Message.Replace("-", "") + ":" + p + ";";
            }


            if (GenreString != "")
            {
                GenreString = GenreString.Substring(0, GenreString.Length - 1);
            }

            if (GenreString != "00000000:1;")
            {
                //on met à jour le genre uniquement si nécessaire.
                if (PreviousGenre != GenreString)
                {
                    //MessageBox.Show(PreviousGenre + "\r\n" + GenreString);
                    if (delete)
                        db.Query(string.Format("delete from T_GENRES where idlivre={0}", IDLivre));

                    foreach (string scode in GenreString.Split(';'))
                        if (scode != "")
                            SaveGenres(scode, IDLivre);
                }
            }
        }

        //sauvegarde des genres
        private void SaveGenres(string scode, string idlivre)
        {
            string cj = scode.Split(':')[0];
            string p = scode.Split(':')[1];

            string QMajGenres = string.Format("insert into T_GENRES ( idlivre, codegenre, principal ) Values ( {0}, {1}, {2} )",
                                               idlivre, cj, p);

            db.Query(QMajGenres);
        }

        #endregion       

        private void CB_Serie_TextChanged(object sender, EventArgs e)
        {            
            RemoveSerieFromTitre();
            TB_Info_Serie.Text = string.Empty;
            TB_Present_Titre_Serie.Text = "0";
            
            TB_Serie_Contenu_1.Enabled = false;
            TB_Serie_Contenu_2.Enabled = false;

            SerieRow = null;
            AlternateValues();

            if (DSeries.ContainsKey(CB_Serie.Text))
            {                
                CB_Serie.ForeColor = SystemColors.WindowText;
                TB_Present_Titre_Serie.Text = "";
                
                if (TB_IDLibSerie.Text != DSeries[CB_Serie.Text])
                    TB_IDLibSerie.Text = DSeries[CB_Serie.Text];

                if (CB_Serie.Text != "") //remplace non précisé par chaine vide 
                {
                    TB_NoSerie.Enabled = true;
                    CBX_Serie_Coffret.Enabled = true;
                    CBX_Serie_HorsSerie.Enabled = true;
                    CBX_Serie_Integrale.Enabled = true;

                    MemoSerie = CB_Serie.Text;

                    LoadArtFuncID(DSeries[CB_Serie.Text], "serie", true);


                    DisplayCollection(CollectionRow, CB_Serie);                    
                }
                else
                {
                    ResetMDLABELColors();

                    TB_NoSerie.Enabled = false;
                    CBX_Serie_Coffret.Enabled = false;
                    CBX_Serie_HorsSerie.Enabled = false;
                    CBX_Serie_Integrale.Enabled = false;                    

                    int Cindex = 0;
                    foreach (Control c in CD.tabControl1.TabPages[1].Controls)
                    {
                        if (c.Name == "LV_Series")
                        {
                            Cindex = CD.tabControl1.TabPages[1].Controls.IndexOf(c);
                            break;
                        }
                    }
                    ((ListView)CD.tabControl1.TabPages[1].Controls[Cindex]).Items.Clear();
                    CD.tabControl1.SelectedIndex = 0;
                    CD.tabControl1.Select();
                }                
                //RewriteTitreFromSerieColl();             
            }
            else
            {
                SerieRow = null;                
                CB_Serie.ForeColor = Color.OrangeRed;
                TB_NoSerie.Enabled = false;
                CBX_Serie_Coffret.Enabled = false;
                CBX_Serie_HorsSerie.Enabled = false;
                CBX_Serie_Integrale.Enabled = false;
                //TB_IDLibSerie.Text = "0";
                ResetMDLABELColors();
            }

            if (TB_Present_Titre_Serie.Text == "1" && TB_Libelle.Text.Trim() == "")
            {
                TB_Libelle_Leave(this, e);
                CB_Serie.Focus();
            }

            RewriteTitreFromSerieColl(); 
            AlternateValues();
            
        }

        private void TB_IDLibSerie_TextChanged(object sender, EventArgs e)
        {
            foreach (var pair in DSeries)
            {
                if (pair.Value == TB_IDLibSerie.Text)
                {
                    CB_Serie.Text = pair.Key;
                    break;
                }
            }
        }

        private void CheckEnabledNoSerie2()
        {
            if ((TB_NoSerie.Text != "" && TB_NoSerie.Text != "0") && (CBX_Serie_Coffret.Checked || CBX_Serie_Integrale.Checked))
                TB_Serie_Contenu_1.Enabled = true;
            else
                TB_Serie_Contenu_1.Enabled = false;
        }

        private void CheckEnabledNoColl2()
        {
            if ((TB_Nocoll.Text != "" && TB_Nocoll.Text != "0") && (CBX_Coll_Coffret.Checked))
                TB_Coll_Contenu_1.Enabled = true;
            else
                TB_Coll_Contenu_1.Enabled = false;
        }

        #endregion                     
       

        private void TB_Present_Titre_Serie_TextChanged(object sender, EventArgs e)
        {            
            RewriteTitreFromSerieColl();
        }

        private void TB_Nocoll_2_TextChanged(object sender, EventArgs e)
        {
            RewriteTitreFromSerieColl();
        }

        private void TB_NoSerie2_TextChanged(object sender, EventArgs e)
        {
            RewriteTitreFromSerieColl();
        }

        private void CBX_Ancrage_Collection_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_Ancrage_Collection.Checked)
            {
                MainForm_LocationChanged(sender, e);
                ini.IniWriteValue("COLLECTION", "ANCRAGE", "1");
            }
            else
            {
                ini.IniWriteValue("COLLECTION", "ANCRAGE", "0");
                CD.CanMove = true;
            }
        }

        private void ReloadSerie()
        {
            Cursor = Cursors.WaitCursor;
            CB_Serie.Items.Clear();
            DSeries.Clear();
            DSeries = LoadListe("select id, case when (id=0) then '' else cast(libelle as varchar(80)) end as libelle from libseries", "id", "libelle", CB_Serie);
            CB_Serie.Sorted = true;
            Cursor = Cursors.Arrow;
        }

        private void CB_Serie_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                ReloadSerie();
            }

            else if (e.KeyData == Keys.F2)
            {

                SaisieLivre.Forms.NewCollectionForm SF = new SaisieLivre.Forms.NewCollectionForm(this, "S", Properties.Resources.SelectFromLibseriesForNewCollection);
                    
                SF.TB_Libelle.Text = CB_Serie.Text;

                DialogResult DR = SF.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    //recharge la liste
                    Cursor = Cursors.WaitCursor;
                        
                    CB_Serie.Items.Clear();
                    DSeries.Clear();

                    db.Query("select id, case when (id=0) then '' else libelle end as libelle from libseries");
                    foreach (Row row in db.FetchAll())
                    {
                        if (!DSeries.ContainsKey(row["libelle"].ToString()))
                            DSeries.Add(row["libelle"].ToString(), row["id"].ToString());
                        else
                            DSeries[row["libelle"].ToString()] = row["id"].ToString();

                        CB_Serie.Items.Add(row["libelle"].ToString());
                    }    
                                        
                    CB_Serie.Sorted = true;
                    //remet les libelles en place après modif
                    CB_Serie.Text = "";
                    CB_Serie.Text = SF.TB_Libelle.Text;
                    CB_Serie.SelectedIndex = CB_Serie.Items.IndexOf(SF.TB_Libelle.Text);

                    TB_IDLibSerie.Text = SF.ID;

                    RewriteTitreFromSerieColl();

                    Cursor = Cursors.Arrow;

                    TB_NoSerie.Focus();
                }
             
            }
        }

        private void TB_Volume_Collection_TextChanged(object sender, EventArgs e)
        {
            RewriteTitreFromSerieColl();
        }

        private void TB_Volume_Serie_TextChanged(object sender, EventArgs e)
        {
            RewriteTitreFromSerieColl();
        }

        public string SetGTL(string input)
        {
            string Val = input;
            while (Val.Length < 8)
                Val = "0" + Val;
            Val = Val.Substring(0, 2) + "-" + Val.Substring(2, 2) + "-" + Val.Substring(4, 2) + "-" + Val.Substring(6, 2);
            return Val;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {            
            if (CBX_OnTopDilicom.Checked)
            {               
                if (((Form)sender).Top > 0 && !IsActivated)
                {                    
                    IsActivated = true;
                    if (SD != null)
                    {                        
                        SD.TopMost = true;
                        SD.TopMost = false;
                        this.Focus();
                    }

                    if (CD != null)
                    {                        
                        CD.TopMost = true;
                        CD.TopMost = false;
                        this.Focus();
                    }
                }
                else
                    IsActivated = false;
            }
        }

        private void LibGenreTabStop(bool state)
        {
            CB_Genre_0.TabStop = state;
            CB_Genre_1.TabStop = state;
            CB_Genre_2.TabStop = state;
            CB_Genre_3.TabStop = state;
        }

        private void CBX_Param_TabStop_LibGenres_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_Param_TabStop_LibGenres.Checked)
            {
                LibGenreTabStop(false);
                ini.IniWriteValue("TABSTOP", "LIBGENRES", "False");
            }
            else
            {
                LibGenreTabStop(true);
                ini.IniWriteValue("TABSTOP", "LIBGENRES", "True");
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CBX_OnTopDilicom.Checked)
                ini.IniWriteValue("ONTOP", "DILICOLL", "True");
            else
                ini.IniWriteValue("ONTOP", "DILICOLL", "False");
        }

        private void BT_SetRefChainToList_Click(object sender, EventArgs e)
        {
            foreach (string cab in LB_RefsChainees.Items)
                LB_Saisie_EAN.Items.Add(cab);

            SetCompteurListeSaisie();
        }


        private void LoadArtFuncFromSerieColl(ComboBox cb, string serie_coll = "coll")
        {
            string id = "";
            string SC = serie_coll;

            if (SC == "serie")
            {
                SC = "series";
                id = DSeries[cb.Text];
            }
            else
                id = DCollection[cb.Text]["id"].ToString();

            LB_AutFunc.Items.Clear();
            LB_AutFuncComp.Items.Clear();
            
            Dictionary<string, List<string>> D = new Dictionary<string, List<string>>();

            db.Query(string.Format("select * from auteurs_fonctions_{0} where id{2}='{1}'", SC, id, serie_coll));
            foreach (var Row in db.FetchAll())
            {
                if (!D.ContainsKey(Row["auteur"].ToString()))
                {
                    List<string> l = new List<string>();
                    l.Add(Row["fonction"].ToString());
                    D.Add(Row["auteur"].ToString(), l);
                }
                else
                    D[Row["auteur"].ToString()].Add(Row["fonction"].ToString());
            }

            foreach (var pair in D)
            {
                string codes = "";
                foreach (string c in pair.Value)
                    codes += c + ",";

                codes = codes.Substring(0, codes.Length - 1);

                LB_AutFunc.Items.Add(pair.Key);
                LB_AutFuncComp.Items.Add(codes);

                if (!LB_AutFunc.Items.Contains(pair.Key))
                {
                    LB_AutFunc.Items.Add(pair.Key);
                    LB_AutFuncComp.Items.Add(codes);
                }


            }
        }


        public void LoadArtFuncID(string id, string serie_coll = "coll", bool Test=false)
        {
            string SC = serie_coll;
            if (SC == "serie")
                SC = "series";

            LB_AutFunc.Items.Clear();
            LB_AutFuncComp.Items.Clear();

            Dictionary<string, List<string>> D = new Dictionary<string, List<string>>();

            db.Query(string.Format("select * from auteurs_fonctions_{0} where id{2}='{1}'", SC, id, serie_coll));
            foreach (var Row in db.FetchAll())
            {
                if (!D.ContainsKey(Row["auteur"].ToString()))
                {
                    List<string> l = new List<string>();
                    l.Add(Row["fonction"].ToString());
                    D.Add(Row["auteur"].ToString(), l);
                }
                else
                    D[Row["auteur"].ToString()].Add(Row["fonction"].ToString());
            }

            foreach (var pair in D)
            {
                string codes = "";
                foreach (string c in pair.Value)
                    codes += c + ",";

                codes = codes.Substring(0, codes.Length - 1);

                if (Test)
                {
                    if (TB_Auteurs.Text.Contains(pair.Key))
                    {
                        LB_AutFunc.Items.Add(pair.Key);
                        LB_AutFuncComp.Items.Add(codes);

                        if (!LB_AutFunc.Items.Contains(pair.Key))
                        {
                            LB_AutFunc.Items.Add(pair.Key);
                            LB_AutFuncComp.Items.Add(codes);
                        }
                    }
                }
                else
                {
                    LB_AutFunc.Items.Add(pair.Key);
                    LB_AutFuncComp.Items.Add(codes);

                    if (!LB_AutFunc.Items.Contains(pair.Key))
                    {
                        LB_AutFunc.Items.Add(pair.Key);
                        LB_AutFuncComp.Items.Add(codes);
                    }
                }


            }
        }



    }
    

    public class FData
    {        
        public FData(string FieldName, Control C, Control L, Control C2=null, string LText="", string LTextOriginal="", Control C3=null, Control C4=null)
        {
            Field = FieldName;
            Control = C;            
            Label = L;

            LabelText = LText;
            LabelTextOrigine = LTextOriginal;
      
            if (C2!=null) 
                Control2 = C2;
            if (C3 != null)
                Control3 = C3;
            if (C4 != null)
                Control4 = C4;
        }
        public string Field { get; set; }
        public Control Control { get; set; }
        public Control Control2 { get; set; }
        public Control Control3 { get; set; }
        public Control Control4 { get; set; }
        public Control Label { get; set; }        
        public string LabelText { get; set; }
        public string LabelTextOrigine { get; set; }
    }

    
    public class MyListBoxItem
    {

        public MyListBoxItem(Color c, string m, Font f  )
        {
            ItemColor = c;
            Message = m;
            Fontstyle = f;
        }        

        public Color ItemColor { get; set; }
        public string Message { get; set; }
        public Font Fontstyle { get; set; }
    }

}
