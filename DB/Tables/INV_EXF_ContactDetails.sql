CREATE TABLE [dbo].[INV_EXF_ContactDetails]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ExtraFeeId] INT NULL, 
    [CustomerContactId] INT NULL, 
    [SupplierContactId] INT NULL, 
    [FactoryContactId] INT NULL,
	CONSTRAINT INV_EXF_ContactDetails_ExtraFeeId FOREIGN KEY ([ExtraFeeId]) REFERENCES [dbo].[INV_EXF_Transaction](Id),
	CONSTRAINT INV_EXF_ContactDetails_CustomerContactId FOREIGN KEY ([CustomerContactId]) REFERENCES [dbo].[CU_Contact](Id),
	CONSTRAINT INV_EXF_ContactDetails_SupContactId FOREIGN KEY ([SupplierContactId]) REFERENCES [dbo].[SU_Contact](Id),
	CONSTRAINT INV_EXF_ContactDetails_FactContactId FOREIGN KEY ([FactoryContactId]) REFERENCES [dbo].[SU_Contact](Id)
)
