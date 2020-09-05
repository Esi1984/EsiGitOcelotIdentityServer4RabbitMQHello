1. nuget: EntityFrameWorkCore.tools and ..sqlserver
2/. create modelClass
3. create DBContext Class
4. add service provider dependency injection
	4.1 set Connection String in to the appsettings.json
5. enablemigrations
6. add-migration  / remove-migration
7. update-database

ERR
	at MS2 when try add-migration
	message:
		Could not load assembly 'MS2'. Ensure it is referenced by the startup project 'MS1'.
		Solved:
		make default project MS2 in sloution explorar!!

Tip:
	add-migration newmigration -project MsReporter
