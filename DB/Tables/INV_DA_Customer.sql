CREATE TABLE [dbo].[INV_DA_Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[InvDaId] INT NOT NULL,
	[CustomerId] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	CONSTRAINT FK_INV_DA_Customer_InvDaId FOREIGN KEY ([InvDaId]) REFERENCES [dbo].[INV_DA_Transaction](Id),	
	CONSTRAINT FK_INV_DA_Customer_CustomerId FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CU_Customer](Id),	
    CONSTRAINT FK_INV_DA_Customer_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
    CONSTRAINT FK_INV_DA_Customer_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_INV_DA_Customer_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
