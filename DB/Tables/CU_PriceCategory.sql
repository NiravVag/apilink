CREATE TABLE [dbo].[CU_PriceCategory]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NULL, 
    [CustomerId] INT NULL, 
    [Active] BIT NULL,
	[EntityId] INT NULL,
	CONSTRAINT CU_PriceCategory_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY(CustomerId) REFERENCES [Cu_Customer](Id)
)
