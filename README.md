DropboxConflictsSolver
======================

Descripci�n:
------------

�ste sencillo programa permite eliminar de manera r�pida y sencilla los archivos conflictivos que genera dropbox durante la sincronizaci�n.
Aunque ese es el cometido original para lo que fue desarrollado, el programa permite tanto la b�squeda como la eliminaci�n de archivos 
utilizando una expresi�n regular para filtrarlos (Por nombre).

Utilizaci�n:
------------

El programa est� dise�ado para ser ejecutado a trav�s de la l�nea de comandos con una serie de par�metros. La sintaxis es la siguiente:

DropboxConflictsSolver [directorio] [regex] [acci�n] [opciones]

directorio: Directorio donde buscar los archivos.
regex: Expresi�n regular utilizada para filtrar los archivos. Dicha regex se aplica al nombre del archivo (Extensi�n incluida), no a su 
       path completo.
acci�n: Acci�n a ejecutar por el programa.
		-s Muestra los archivos cuyo nombre se corresponde con la regex especificada.
		-d Elimina los archivos cuyo nombre se corresponde con la regex especificada.
		-h Muestra la ayuda del programa.
opciones: Opciones de la acci�n a ejecutar.
          -r Buscar recursivamente en los subdirectorios del directorio especificado.
		  -n Buscar �nicamente en el directorio especificado.
		  
C�digo fuente:
--------------

El programa es un sencillo proyecto de consola desarrollado con C# (.NET Framework 4.5). El c�digo no est� licenciado, pero cualquiera que 
desee utilizarlo para sus propios fines es libre de hacerlo.