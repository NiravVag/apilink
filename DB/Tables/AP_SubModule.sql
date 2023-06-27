	CREATE TABLE [AP_SubModule] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY, 
		[Name] NVARCHAR(300) NOT NULL, 
		[IdModule] INT NOT NULL,
		[DataSourceName] VARCHAR(200) NOT NULL,
		[DataSourceTypeId] INT NOT NULL,   
		[Active] BIT NOT NULL,
		FOREIGN KEY ([IdModule]) REFERENCES AP_Module(Id),
		FOREIGN KEY ([DataSourceTypeId]) REFERENCES [REF_DataSourceType](Id)
	)