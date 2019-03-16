SELECT 
	Coalesce(c.present_titre, 0) as present_titre,
    cast( trim( c.collection ) as varchar(160)) as collection,                                                          
    cast( trim( c.editeur ) as varchar(40)) as editeur,                                                                               
    c.auteurs, 
	case when ( c.codesupport is null ) then '' else c.codesupport end as codesupport,    
	case when (l.id=0) then '' else cast(lower(l.libelle) as varchar(50)) end as lectorat,
	coalesce( c.id_lectorat, 0) as id_lectorat,
	cast( case when (ls.col0=0) then '' else coalesce( ls.col1, '' ) end as varchar(60)) as style,
	coalesce( c.livre_lu, '' ) as livre_lu,
	coalesce( c.bigletters, '' ) as grands_cara,
	coalesce( c.multilingue, '' ) as multilingue,
	coalesce( c.illustre, '' ) as illustre, 
	coalesce( c.iad, '') as iad,
	coalesce( c.luxe, '') as luxe, 
	coalesce( c.cartonne, '' ) as cartonne,
	coalesce( c.relie, '') as relie,
	coalesce( c.broche, '' ) as broche,
	
	case when( c.NBRpage is null or c.nbrpage=0) then '' else c.nbrpage end as nbrpage,
	case when( c.Largeur is null or c.largeur=0 ) then '' else c.largeur end as largeur,
	case when( c.Hauteur is null or c.hauteur=0 ) then '' else c.hauteur end as hauteur,	
	case when( c.codelangue is null or c.codelangue=0) then '' else c.codelangue end as codelangue,
	
	case when ( t.id = 0 ) then '' else
	cast( coalesce( t.nom, '' ) ||
        case when ( t.prenom is null or t.prenom='' ) 
             then '' 
             else ', ' || t.prenom  
        end 
    as varchar(150)) end as traducteur,	
	
	CASE WHEN ( la.libelle = 'NON DEFINI' ) then '' else la.libelle end as langue_de,
	CASE WHEN ( la2.libelle = 'NON DEFINI' ) then '' else la2.libelle end as langue_en,		               
       
	coalesce( g.gtl, '' ) as genre_principal,
	(select strgtl from GET_COLLECTIONS_GTL_SEC( c.id )) as test_gtl_second,                                                                                             
    case when ( c.obcode is null ) then '' else c.obcode end as obcode,
	c.presentation,  
	c.id  

from COLLECTIONS          c        
JOIN EDITEURS             e on c.EDITEUR     = e.EDITEUR                                                                              
LEFT JOIN LibLectorat     l on coalesce(c.id_lectorat, 0) = l.id      
LEFT JOIN LIBSTYLES      ls on ls.col0       = coalesce( c.idstyle, 0 )
left join Collections_Gtl g on g.idcoll      = c.id and g.principal=1
left join Liblangues la on la.id=coalesce( c.idlangue_de, 0 )
left join Liblangues la2 on la2.id=coalesce( c.idlangue_en, 0 )
LEFt JOIN Traducteurs t on t.id=coalesce( c.idtraducteur, 0 )
where                                                                                                                              
    cast(
        trim( e.editeur ) ||                                                                                                              
        case when ( COALESCE( e.libelle_complet, '' ) != '' ) then ' (' || cast( trim( e.libelle_complet ) as varchar(80)) || ')' else '' end as varchar(130)) ='{0}'
        and trim(c.collection)!='' order by c.collection