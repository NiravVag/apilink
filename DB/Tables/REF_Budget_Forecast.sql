CREATE TABLE [dbo].[REF_Budget_Forecast]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Year] INT NOT NULL, 
    [Month] INT NOT NULL, 
    [CountryId] INT NOT NULL, 
    [ManDay] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL,
	[Fees] FLOAT NULL, 
    [CurrencyId] INT NULL, 
    [EntityId] INT NULL,
    FOREIGN KEY ([CountryId]) REFERENCES [dbo].[REF_Country](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[REF_Currency](Id),
    CONSTRAINT FK_REF_Budget_Forecast_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
