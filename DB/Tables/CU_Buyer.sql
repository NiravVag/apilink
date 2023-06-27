CREATE TABLE [dbo].[CU_Buyer]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] nvarchar(200) NOT NULL, 
    [Customer_Id] INT NOT NULL, 
    [Active] BIT NOT NULL,
    [Code] NVARCHAR(200) NULL,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[EntityId] INT NULL,
	CONSTRAINT CU_Buyer_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY(Customer_Id) REFERENCES [CU_Customer](Id), 
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
