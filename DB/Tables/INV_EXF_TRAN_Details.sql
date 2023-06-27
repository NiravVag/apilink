CREATE TABLE [dbo].[INV_EXF_TRAN_Details]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[EXFTransactionId] int null,
	ExtraFeeType int null, 
	ExtraFees float null,
	Remarks nvarchar(max),
	CreatedOn datetime not null default getdate(), 
	CreatedBy int null,
	UpdatedBy int null, 
	UpdatedOn datetime  null,
	DeletedBy int null,
	DeletedOn datetime  null, 
	Active bit
	CONSTRAINT INV_EXF_TRAN_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_TRAN_Details_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_TRAN_Details_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_TRAN_Details_ExtraFeeType FOREIGN KEY (ExtraFeeType) REFERENCES [dbo].[INV_EXF_Type](Id),
	CONSTRAINT INV_EXF_TRAN_Details_EXFTransactionId FOREIGN KEY (EXFTransactionId) REFERENCES [dbo].[inv_exf_transaction](Id),
)