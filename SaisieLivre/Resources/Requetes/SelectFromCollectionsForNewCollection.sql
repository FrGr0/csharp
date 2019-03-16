SELECT
	c.Present_titre,
	c.Collection,
	c.Editeur, 	
	c.Auteurs,	
	c.codesupport,
	case when (l.id=0) then '' else cast( lower(l.libelle) as varchar(50)) end as lectorat,
	case when (st.col0=0) then '' else st.col1 end as style,
	/*c.NewGenre, */	
	coalesce( c.codelangue, 0 ) as codelangue,
	case when (t.id=0) then '' else cast( t.nom || case when (t.prenom!='' or t.prenom is not null) then ', ' || t.prenom else '' end as varchar(80)) end as traducteur,
	case when (la.id=0) then '' else la.libelle end as langue_de,
	case when (la2.id=0) then '' else la2.libelle end as langue_en,						
	coalesce( g.gtl, '' ) as genre_principal,
	(select strgtl from GET_COLLECTIONS_GTL_SEC( c.id )) as test_gtl_second,
	c.OBCode,
	c.Presentation,		
	coalesce( c.livre_lu, 0 ) as livre_lu,
	coalesce( c.bigletters, 0 ) as grands_cara,
	coalesce( c.multilingue, 0 ) as multilingue,
	coalesce( c.illustre, 0 ) as illustre, 
	coalesce( c.iad, 0 ) as iad,
	coalesce( c.luxe, 0 ) as luxe, 
	coalesce( c.cartonne, 0 ) as cartonne, 
	coalesce( c.relie, 0 ) as relie,
	coalesce( c.broche, 0 ) as broche,
	c.NBRpage,
	c.Largeur,
	c.Hauteur,
	c.id

from COLLECTIONS c
left join LIBLECTORAT l on l.id = coalesce(c.id_lectorat, 0)
left join LibStyles st on st.col0=coalesce( c.idstyle, 0 )
LEFt JOIN Traducteurs t on t.id=coalesce( c.idtraducteur, 0 )
left join Liblangues la on la.id=coalesce( c.idlangue_de, 0 )
left join Liblangues la2 on la2.id=coalesce( c.idlangue_en, 0 )
left join Collections_Gtl g on g.idcoll=c.id and g.principal=1
where 
	editeur='{0}' 
	{1}