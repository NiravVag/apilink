CREATE TABLE [dbo].[INV_DA_Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[StaffId] INT NOT NULL,
    [EntityId] INT,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	CONSTRAINT FK_INV_DA_Transaction_StaffId FOREIGN KEY ([StaffId]) REFERENCES [dbo].[HR_Staff](Id),	
    CONSTRAINT FK_INV_DA_Transaction_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id),
    CONSTRAINT FK_INV_DA_Transaction_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
    CONSTRAINT FK_INV_DA_Transactions_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_INV_DA_Transaction_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
