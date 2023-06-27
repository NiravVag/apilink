CREATE TABLE [dbo].[INV_DA_InvoiceType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[InvDaId] INT NOT NULL,
	[InvoiceTypeId] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	CONSTRAINT FK_INV_DA_InvoiceType_InvDaId FOREIGN KEY ([InvDaId]) REFERENCES [dbo].[INV_DA_Transaction](Id),	
	CONSTRAINT FK_INV_DA_InvoiceType_InvoiceTypeId FOREIGN KEY ([InvoiceTypeId]) REFERENCES [dbo].[REF_InvoiceType](Id),	
    CONSTRAINT FK_INV_DA_InvoiceType_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
    CONSTRAINT FK_INV_DA_InvoiceType_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_INV_DA_InvoiceType_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
