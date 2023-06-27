	CREATE TABLE KPI_Template(
		Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(200) NOT NULL,
		[UserId] INT NOT NULL,
		[CreatedDate] DATETIME NOT NULL, 
		[UpdatedDate] DATETIME NULL,
		[IsShared] BIT NOT NULL,
		[UseXlsFormulas] BIT NOT NULL, 
		[IdModule] INT NOT NULL,
		FOREIGN  KEY ([UserId]) REFERENCES [IT_UserMaster](Id),
		FOREIGN  KEY ([IdModule]) REFERENCES [AP_Module](Id)
	)
