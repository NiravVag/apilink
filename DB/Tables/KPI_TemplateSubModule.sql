	CREATE TABLE KPI_TemplateSubModule(
		IdTemplate INT NOT NULL, 
		IdSubModule INT NOT NULL,
		PRIMARY KEY(IdTemplate, IdSubModule),
		FOREIGN  KEY ([IdSubModule]) REFERENCES [AP_SubModule](Id),
		FOREIGN  KEY ([IdTemplate]) REFERENCES [KPI_Template](Id)
	)