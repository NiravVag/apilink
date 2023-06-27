/*
Ne changez pas les variables de nom ou de chemin d'accès.
Toutes variables sqlcmd seront correctement substituées durant 
la génération et le déploiement.
*/
ALTER DATABASE [$(DatabaseName)]
	ADD FILEGROUP [ApiFileGroup] CONTAINS FILESTREAM
