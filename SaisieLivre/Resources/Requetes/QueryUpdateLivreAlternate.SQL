UPDATE LIVRE
SET 
    datemaj      = 'now',
    mbexport     = 1,       
    
    titre        = '{0}',
    libelle      = '{1}',
    idlectorat   = '{2}',
    
    -- editeur      = '',    
    -- distributeur = '',
    -- collection   = '',
    -- idcollection = '',
    -- prixeuro     = '', 

    codeTVA      = '{3}', 
    auteurs      = '{4}',

    -- dateparution = '',

    codesupport  = '{5}',
    idtraducteur = '{6}',
    id_style     = '{7}',
    idlangue     = '{8}',
    idtraduiten  = '{9}',
        
    codelangue   = '{10}',      
    -- l'ancien code CSR est recalculé par rapport aux nouveaux genres
    
    -- dispo        = '{17}',

    iad          = '{11}',
    commentaire  = '{12}',
    poids        = '{13}', 
    nbrpages     = '{14}',
    longueur     = '{15}', 
    largeur      = '{16}',
    origine      = '{17}',
	obcode       = '{18}'
    
WHERE
    id = '{19}'

-- Tous les champs commentés sont à mettre à jour sur la table T_MultiDistri