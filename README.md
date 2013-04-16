DropboxConflictsSolver
======================

Descripción:
------------

Éste sencillo programa permite eliminar de manera rápida y sencilla los archivos conflictivos que genera dropbox durante la sincronización.
Aunque ese es el cometido original para lo que fue desarrollado, el programa permite tanto la búsqueda como la eliminación de archivos utilizando una expresión regular para filtrarlos.

Utilización:
------------

El programa está diseñado para ser ejecutado a través de la línea de comandos con una serie de parámetros. La sintaxis es la siguiente:

DropboxConflictsSolver [directorio] [regex] [acción] [opciones]

 - directorio: Directorio donde buscar los archivos.
 - regex: Expresión regular utilizada para filtrar los archivos. Dicha regex se aplica al nombre del archivo (Extensión          
          incluida), no a su path completo.
 - acción: Acción a ejecutar por el programa.
	   **-s** Muestra los archivos cuyo nombre se corresponde con la regex especificada.
	   **-d** Elimina los archivos cuyo nombre se corresponde con la regex especificada.
	   **-h** Muestra la ayuda del programa.
 - opciones: Opciones de la acción a ejecutar.
             **-r** Buscar recursivamente en los subdirectorios del directorio especificado.
             **-n** Buscar únicamente en el directorio especificado.
		  
Código fuente:
--------------

El programa es un sencillo proyecto de consola desarrollado con C# (.NET Framework 4.5). El código no está licenciado, pero cualquiera que desee utilizarlo para sus propios fines es libre de hacerlo.
