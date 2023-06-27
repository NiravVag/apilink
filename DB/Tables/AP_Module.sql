	CREATE TABLE [AP_Module] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY, 
		[Name] NVARCHAR(300) NOT NULL, 
		[DataSourceName] VARCHAR(200) NOT NULL,
		[DataSourceTypeId] INT NOT NULL,   
		[Active] BIT NOT NULL,
		FOREIGN KEY ([DataSourceTypeId]) REFERENCES [REF_DataSourceType](Id)
	)