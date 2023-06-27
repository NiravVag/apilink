CREATE TABLE [dbo].[INV_EXT_TRAN_Tax]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
[ExtraFee_Id] INT NULL,
[TaxId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DateTime default getdate()
CONSTRAINT INV_EXT_TRAN_Tax_ExtraFee_Id FOREIGN KEY ([ExtraFee_Id]) REFERENCES [dbo].[INV_EXF_Transaction](Id),
CONSTRAINT INV_EXT_TRAN_Tax_TaxId FOREIGN KEY ([TaxId]) REFERENCES [dbo].[INV_TRAN_Bank_Tax](Id),
CONSTRAINT INV_EXT_TRAN_Tax_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
