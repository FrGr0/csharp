SELECT 
	l.gencod, 
	Cast(Trim( l.titre ) as varchar(250)) as titre, 
	
	Cast(Trim( l.libelle ) as varchar(250)) as libelle,

	Cast(Trim( l.auteurs ) as varchar(120)) as auteurs, 
		
	CAST(
        trim( e.editeur ) ||                                                                                                   
        case                                                                                                                 
			when ( COALESCE( e.libelle_complet, '' ) != '' ) then ' (' || cast( trim( e.libelle_complet ) as varchar(80)) || ')'  
            else ''                                                                                                           
        end                                                                                                                  
    as varchar(130)) as editeur,   

	l.idlectorat, 

	/* collection */
	Case
	  when (coalesce( l.idcollection, 0 )>0 ) then l.idcollection 
	  when (coalesce( l.collection, '' )!='') then ( select co.id from collections co where co.collection=l.collection and co.editeur=l.editeur ) 
	else cast( '0' as smallint ) end as idcollection,

	Case when ( coalesce( l.idcollection, 0 )>0 ) then c.collection else Cast( Trim( l.collection ) as varchar(40)) end as collection, 
	
	case when (l.nocol is null or l.nocol='' ) then 0 else l.nocol end as nocoll,
	
	/* sous collection 
	l.idsouscollection,	 
	coalesce( l.noscoll, 0 ) as noscoll,
	*/

	l.serie, 
	
	Cast( Trim( l.codesupport ) as varchar(2)) as codesupport, 	
			
	l.idsupport,

	roundcent( l.prixeuro ) as prixeuro, 
	
	l.codetva, 
	
	l.distributeur, 
	
	
	Cast( case when( l.dateparution<cast('01/01/1900' as timestamp)) then '01/01/2070' else date2str( l.dateparution, 'D/M/Y' ) end as varchar(10))  as dateparution, 
	
	l.idtraducteur, 
	coalesce( l.idlangue, 0) as idlangue, 
	coalesce( l.idtraduiten, 0 ) as idtraduiten, 
	Coalesce( l.id_style, 0 ) as id_style,
	Coalesce( l.id_theme, 0 ) as id_theme,
	l.OBCode,
	
	Coalesce( ( select code_furet from get_new_codefuret(l.gencod) ), '0000' ) as CSRFuret,
 	( SELECT first 1 cfvt.LIBELLE From LIBGENRE_FURET cfvt where cfvt.ID = Coalesce(( select code_furet from get_new_codefuret(l.gencod) ), '0000') ) As LibCSRFuret, 

	l.CodeCSR,
	l.Dispo,
	Coalesce( l.IAD, 0) as IAD,
	
	case when( l.commentaire is null ) then '' else l.commentaire end as commentaire,

	coalesce( l.poids, 0 ) as poids,
	coalesce( l.longueur, 0 ) as longueur,
	coalesce( l.largeur, 0 ) as largeur,
	coalesce( l.epaiseur, 0 ) as epaiseur,
	coalesce( l.nbrpages, 0 ) as nbrpages,

	Case when (i.image_type = 1) then 2 else Coalesce( l.image, 0 ) end as image,

	Coalesce( l.image_4, 0 ) as image_4,
	l.id,
	(select code_furet from Get_New_CodeFuret(l.gencod)) as CSRFuret,
	p.presentation,
	s.sommaire,	

	
	--test	
	Coalesce( l.codelangue, 1 ) as codelangue,
	
	case 
		when exists (select first 1 t.idlivre from t_genres t where t.idlivre=l.id) then (select returngenres from GETFROM_T_GENRES( l.id ) ) 
		when ((select CodeGenre from GET_TL_FROM_CLIL( l.gencod )) is not null) then (select CodeGenre from GET_TL_FROM_CLIL( l.gencod )) || ':1'
		else (select Newgenre from GET_NEWGENRE_FROM_CSR( l.codecsr, l.id_theme, l.idlectorat )) || ':1'			
	end as returngenre,

	case when exists (select first 1 t.idlivre from t_genres t where t.idlivre=l.id) then 0 else 1 end as Force_Genre,

	cast( DATE2STR( coalesce( l.datemajdispo, l.datemaj ), 'D/M/Y' ) as varchar(10)) as datemajdispo,
	cast( DATE2STR( coalesce( l.datemaj, '01/01/2070' ), 'D/M/Y' ) as varchar(10)) as datemaj,
	cast( DATE2STR( coalesce( l.image_date, '01/01/2070' ), 'D/M/Y' ) as varchar(10)) as image_date,
	cast( DATE2STR( coalesce( l.datecreation, '01/01/2070' ), 'D/M/Y' ) as varchar(10)) as datecreation,

	coalesce( l.cartonne, 0 ) as cartonne,
	coalesce( l.relie, 0 ) as relie,
	coalesce( l.broche, 0 ) as broche,
	coalesce( l.livrelu, 0 ) as lu,
	coalesce( l.grandscaracteres, 0 ) as bigletters,
	coalesce( l.multilingue, 0 ) as multilingue,
	coalesce( l.luxe, 0 ) as luxe,
	coalesce( l.illustre, 0 ) as illustre,
	coalesce( l.millesime, 0 ) as millesime,
	coalesce( l.col_hs, 0 ) as col_hs,
	coalesce( l.col_coffret, 0 ) as col_coffret,
	coalesce( l.serie_integrale, 0 ) as serie_integrale,
	coalesce( l.serie_hs, 0 ) as serie_hs,
	coalesce( l.serie_coffret, 0 ) as serie_coffret,
	coalesce( l.idlibserie, 0 ) as idlibserie,
	coalesce( l.noserie, 0 ) as noserie,
	
	coalesce( l.coll_contenu_1, 0) as coll_contenu_1,
	coalesce( l.coll_contenu_2, 0) as coll_contenu_2,
	coalesce( l.serie_contenu_1, 0) as serie_contenu_1,
	coalesce( l.serie_contenu_2, 0) as serie_contenu_2

from livre l 

left join EDITEURS e  on trim(e.editeur) = trim(l.editeur)
left join PRESENTATIONS p on p.idlivre=l.id
left join SOMMAIRES s on s.idlivre=l.id
left join COLLECTIONS c on c.id=l.idcollection
left join IMAGETEMP i on i.cab=l.gencod
where l.gencod='{0}'