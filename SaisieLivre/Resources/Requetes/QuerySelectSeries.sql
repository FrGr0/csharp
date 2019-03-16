SELECT    
	case when (s2.id = 0 ) then '' else s2.libelle end as serie_mere,	
    coalesce( s.Present_Titre, 0) as present_titre,    
	s.Libelle as Serie,
	case when ( s.libelle_affichage is null or s.libelle_affichage='') then s.libelle else s.libelle_affichage end as libelle_affichage,
    cast( coalesce( s.Auteurs, '' ) as varchar(80)) as auteurs,
	case when (lc.id=0) then '' else cast( lower(lc.libelle) as varchar(50)) end as lectorat,
    cast( case when (l.col0=0) then '' else coalesce( l.col1, '' ) end as varchar(60)) as style,
	case when (s.codelangue is null or s.codelangue=0) then '' else s.codelangue end as codelangue,
    
	case when ( t.id = 0 ) then '' else
	cast( coalesce( t.nom, '' ) ||
        case when ( t.prenom is null or t.prenom='' ) 
             then '' 
             else ', ' || t.prenom  
        end 
    as varchar(150)) end as traducteur,	
    CASE WHEN ( ll.libelle = 'NON DEFINI' ) then '' else ll.libelle end as langue_de,
    CASE WHEN ( ll2.libelle = 'NON DEFINI' ) then '' else ll2.libelle end as langue_en,

	coalesce( g.gtl, '' ) as genre_principal,
	(select strgtl from GET_LIBSERIES_GTL_SEC( s.id )) as test_gtl_second,
	case when ( s.obcode is null ) then '' else s.obcode end as obcode,
	 s.presentation
FROM LIBSERIES s
JOIN LIBSERIES s2 on s2.id=coalesce(s.id_serie_mere, 0)
LEFT JOIN LIBSTYLES l on l.col0=s.idstyle
LEFT JOIN LIBLECTORAT lc on lc.id=coalesce(s.idlectorat, 0)
LEFT JOIN TRADUCTEURS t on t.id=s.idtraducteur
LEFT JOIN LIBLANGUES ll on ll.id=coalesce( s.idlangue_de, 0)
LEFT JOIN LIBLANGUES ll2 on ll2.id=coalesce( s.idlangue_en, 0)
left join LibSeries_Gtl g on g.idserie=s.id and g.principal=1
WHERE
    s.id='{0}' and s.id>0