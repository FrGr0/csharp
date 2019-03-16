update livre
set 
	titre      = '{0}', 
	auteurs    = '{1}', 
	libelle    = '{2}', 
	codelangue = '{3}', 
	idlectorat = '{4}',
	id_style   = '{5}',
	datemaj    = 'now', 
	mbexport   = 1 
where 
	gencod='{6}' and 
	( titre     != '{0}' or 
	  auteurs   != '{1}' or 
	  libelle   != '{2}' or 
	  codelangue!= '{3}' or 
	  idlectorat!= '{4}' or
	  id_style  != '{5}' )