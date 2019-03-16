update T_MULTIDISTRI
set 
    distributeur='{0}',
    codeediteur='{1}',
    idcollection='{2}',
	collection='{8}',
    nocoll='{3}',
    prix='{4}',
    dateparution='{5}',
    dispo='{6}',
    datemaj='now'    
where 
    id = '{7}';