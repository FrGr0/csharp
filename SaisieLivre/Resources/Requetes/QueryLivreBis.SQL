SELECT 
	b.titre, 
	b.auteurs, 
	b.editeur as EditeurDilicom, 
	coalesce( e.col1, b.editeur ) as EditeurTL,
	d.distributeur,
	COALESCE( b.collection, '' ) as CollectionDilicom,
	COALESCE( c.collection, cc.collection, '' ) as CollectionTL,
	b.nocoll, 	
	Cast( date2str( b.dateparution, 'D/M/Y' ) as varchar(10)) as dateparution,
	b.dispo, 	
	roundcent( b.prixeuro ) as prixeuro,
	roundcent( t.tauxtva ) as TVA,
	COALESCE( b.codesupport, '' ) as SupportClil,
	COALESCE( s.codesupportinterne, b.codesupport, '' ) as SupportTL,

	COALESCE( b.IAD,0 ) as IAD,
	case when ( COALESCE( b.poids, 0 ) = 0 ) then 0 else b.POIDS end as Poids,
	case when ( COALESCE( b.LARGEUR,  0)=0 ) then 0 else b.LARGEUR  end as Largeur, 	
	case when ( COALESCE( b.LONGUEUR, 0)=0 ) then 0 else b.LONGUEUR end as Hauteur,
	
	coalesce((select CodeLangue from GET_LANGUE_FROM_CLIL(b.theme)), '') as CodeLangue,
		
	b.Theme as CodeCLIL,
	coalesce( l.libelle, '' ) as LibClil, 	

	COALESCE( (select CodeGenre from GET_TL_FROM_CLIL(b.Gencod)), '' ) as Codegenre,

	coalesce(case when ( trim(b.typeproduit )='' ) then 0 else b.typeproduit end, 0) as test_typeprod,
	COALESCE( b.SCOLAIRE, 0) as Scolaire

from livrebis b 

left join LibClil				l on cast( b.theme as smallint )=l.id
left join Distributeurs			d on d.gencodedilectre=b.distributeur
left join EquivalentDilicom		e on e.col0=b.editeur
left join SupportClil			s on s.codesupportclil=b.codesupport
left join TVA                   t on t.codetva=b.codetva
left join EquivalentDilicomColl	c on c.editeur=coalesce(e.col0, b.editeur) and c.collectionbis=b.collection
left join Collections          cc on cc.editeur=coalesce(e.col1, b.editeur) and cc.collection=b.collection

where b.gencod='{0}'