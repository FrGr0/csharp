UPDATE LIVRE
SET 
    datemaj      = 'now',
    mbexport     = 1,       
    
    titre        = '{0}',
    libelle      = '{1}',
    idlectorat   = '{2}',
    
    editeur      = '{3}',    
    distributeur = '{4}',
    collection   =  cast( copy( '{5}', 1, 40 ) as varchar(40)),
    idcollection = '{6}',
    prixeuro     = '{7}', 
    codeTVA      = '{8}', 
    auteurs      = '{9}',
    dateparution = '{10}',
    codesupport  = '{11}',
    idtraducteur = '{12}',
    id_style     = '{13}',
    idlangue     = '{14}',
    idtraduiten  = '{15}',
        
    codelangue   = '{16}',      
    -- l'ancien code CSR est recalculé par rapport aux nouveaux genres
    
    dispo        = '{17}',
    iad          = case when ( '{18}' = '' ) then  0 else '{18}' end,
    commentaire  = '{19}',
    poids        = case when ( '{20}' = '' ) then  0 else '{20}' end,
    nbrpages     = case when ( '{21}' = '' ) then  0 else '{21}' end,
    longueur     = case when ( '{22}' = '' ) then  0 else '{22}' end, 
    largeur      = case when ( '{23}' = '' ) then  0 else '{23}' end,
    origine      = case when ( '{24}' = '' ) then  0 else '{24}' end,
	relie        = case when ( '{25}' = '' ) then  0 else '{25}' end,
	broche       = case when ( '{26}' = '' ) then  0 else '{26}' end,
	livrelu      = case when ( '{27}' = '' ) then  0 else '{27}' end,
	grandscaracteres = case when ( '{28}' = '' ) then  0 else '{28}' end,
	multilingue = case when ( '{29}' = '' ) then  0 else '{29}' end,
	obcode = '{30}',
	idsouscollection = case when ( '{31}' = '' ) then  0 else '{31}' end,
	noscoll = case when ( '{32}' = '' ) then 0 else '{32}' end,
	nocol = case when ( '{33}' = '' ) then 0 else '{33}' end,
	luxe = '{35}', 
	illustre = '{36}',
	millesime = '{37}',
	idlibserie='{38}',
	noserie='{39}',
	serie_contenu_1='{40}',
	serie_integrale='{41}',
	serie_coffret='{42}',
	serie_hs='{43}',
	col_coffret='{44}',
	col_hs='{45}',
	coll_contenu_1='{46}',
	coll_contenu_2='{47}',
	serie_contenu_2='{48}',
	cartonne=case when ( '{49}' = '' ) then  0 else '{49}' end

    
WHERE
    id = '{34}'
