	CREATE TABLE AP_SubModuleRole (
		IdSubModule INT NOT NULL, 
		IdRole INT NOT NULL,
		PRIMARY KEY (IdSubModule, IdRole),
		FOREIGN KEY ([IdSubModule]) REFERENCES AP_SubModule(Id),
		FOREIGN KEY ([IdRole]) REFERENCES it_role(Id)
	)