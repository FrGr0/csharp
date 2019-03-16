SELECT
	s.id,
	case when(s2.id=0) then '' else s2.libelle end as serie_mere,
	coalesce( s.present_titre, 0 ) as present_titre,
	s.libelle as serie,
	case when ( s.libelle_affichage is null or s.libelle_affichage='') then s.libelle else s.libelle_affichage end as libelle_affichage,
	s.auteurs,
	case when (l.id=0) then '' else cast( lower(l.libelle) as varchar(50)) end as lectorat,
	case when (st.col0=0) then '' else st.col1 end as style,
	coalesce( s.codelangue, 0) as codelangue,
	/* s.codelangue, */
	case when (t.id=0) then '' else cast( t.nom || case when (t.prenom!='' or t.prenom is not null) then ', ' || t.prenom else '' end as varchar(80)) end as traducteur,
	case when (la.id=0) then '' else la.libelle end as langue_de,
	case when (la2.id=0) then '' else la2.libelle end as langue_en,	
	coalesce( g.gtl, '' ) as genre_principal,
	(select strgtl from GET_LIBSERIES_GTL_SEC( s.id )) as test_gtl_second,
	s.obcode,
	s.presentation
	

FROM LibSeries s
JOIN LibSeries s2 on s2.id = coalesce(s.id_serie_mere,0)
LEFT JOIN LibLectorat l on l.id=coalesce( s.idlectorat,0 )
LEFt JOIN Traducteurs t on t.id=coalesce( s.idtraducteur, 0 )
left join Liblangues la on la.id=coalesce( s.idlangue_de, 0 )
left join Liblangues la2 on la2.id=coalesce( s.idlangue_en, 0 )
left join LibStyles st on st.col0=coalesce( s.idstyle, 0 )
left join LibSeries_Gtl g on g.idserie=s.id and g.principal=1
WHERE s.id>0 {0}