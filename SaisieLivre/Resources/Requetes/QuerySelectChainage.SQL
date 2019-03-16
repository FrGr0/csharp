Select 
	l.titre,
	l.libelle,
	l.auteurs,
	l.id,
	l.CodeLangue,
	(select returngenres from GETFROM_T_GENRES( l.id )) as returngenre,
	Bloblength(s.sommaire) as lensommaire,
	Bloblength(p.presentation) as lenpresentation,
	s.sommaire,
	p.presentation,
	coalesce( l.id_style, 0 ) as id_style,
	coalesce( l.idlectorat, 0 ) as idlectorat
from livre l 
left join presentations p on p.idlivre = l.id
left join sommaires s on s.idlivre = l.id
where l.gencod='{0}' and
	  coalesce( l.livreancien, 0 ) = 0 and
	  coalesce( l.codesupport, 'T') != 'LA'