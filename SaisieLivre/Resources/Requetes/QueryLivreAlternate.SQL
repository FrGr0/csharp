SELECT 
	l.gencod, 
	l.titre, 
	l.libelle, 
	l.serie, 
	l.codesupport, 	
	l.idlectorat, 
	
	l.idsupport,

	-- Case when ( coalesce( l.idcollection, 0 )>0 ) then c.collection else l.collection end as collection, 
    c.collection,
    
	coalesce( m.nocoll, 0 ) as nocoll,
    
	--e.editeur, 

	CAST(
        trim( e.editeur ) ||                                                                                                   
        case                                                                                                                 
			when ( COALESCE( e.libelle_complet, '' ) != '' ) then ' (' || cast( trim( e.libelle_complet ) as varchar(80)) || ')'  
            else ''                                                                                                           
        end                                                                                                                  
    as varchar(130)) as editeur,  


	l.auteurs, 
	
	roundcent( m.prix ) as prixeuro, 
	l.codetva, 
	m.distributeur, 
	Cast( date2str( m.dateparution, 'D/M/Y' ) as varchar(10))  as dateparution, 
	
    l.idtraducteur, 
	coalesce( l.idlangue, 0) as idlangue, 
	coalesce( l.idtraduiten, 0 ) as idtraduiten, 
	Coalesce( l.id_style, 0 ) as id_style,
	Coalesce( l.id_theme, 0 ) as id_theme,
	l.OBCode,
	cf.CodeCSR as CSRFuret,
	lcf.LibCSR as LibCSRFuret,
	l.CodeCSR,
    m.Dispo,
	Coalesce( l.IAD, 0) as IAD,
	l.commentaire,
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

	m.idcollection,

	--test	
	l.codelangue,
	
	case 
		when (t.idlivre is not null) then (select returngenres from GETFROM_T_GENRES( l.id ) ) 
		-- when (a.codecsr is not null) then
		else (select Newgenre from GET_NEWGENRE_FROM_CSR( l.codecsr, l.id_theme, l.idlectorat )) || ':1'	
		-- else ''
	end as returngenre


from T_MULTIDISTRI m
join LIVRE l on m.gencod=l.gencod
join EDITEURS e on m.codeediteur = e.codeediteur

left join T_GENRES t on t.idlivre=l.id

left join CSRFuret cf on cf.gencod=l.gencod
left join CSR_LIB_NEW_FURET lcf on cf.CodeCSR = lcf.CodeCSR
left join PRESENTATIONS p on p.idlivre=l.id
left join SOMMAIRES s on s.idlivre=l.id
left join COLLECTIONS c on c.id=m.idcollection
left join IMAGETEMP i on i.cab=l.gencod

where m.gencod='{0}' and m.id='{1}'