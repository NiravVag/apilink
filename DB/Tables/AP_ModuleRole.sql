	CREATE TABLE AP_ModuleRole (
		IdModule INT NOT NULL, 
		IdRole INT NOT NULL,
		PRIMARY KEY (IdModule, IdRole),
		FOREIGN KEY ([IdModule]) REFERENCES AP_Module(Id),
		FOREIGN KEY ([IdRole]) REFERENCES it_role(Id)
	)