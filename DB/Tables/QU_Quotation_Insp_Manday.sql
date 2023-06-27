CREATE TABLE [dbo].[QU_Quotation_Insp_Manday]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[BookingId] INT NOT NULL,
	[QuotationId] INT NOT NULL,
	[NoOfManday] FLOAT NULL,
	[Remarks] NVARCHAR(MAX),
	[ServiceDate] DATETIME NOT NULL,
	[CreatedDate] DATETIME DEFAULT GETDATE(),
	[CreatedBy] INT NULL,
	[UpdatedDate] DATETIME NULL,
	[UpdatedBy] INT NULL,
	[DeletedDate] DATETIME NULL,
	[DeletedBy] INT NULL,
	[Active] BIT NULL,
	CONSTRAINT FK_QU_Insp_Manday_BookingId FOREIGN KEY (BookingId) REFERENCES [dbo].[INSP_Transaction](Id),
	CONSTRAINT FK_QU_Insp_Manday_QuotationId FOREIGN KEY (QuotationId) REFERENCES [dbo].[QU_QUOTATION](Id),
	CONSTRAINT FK_QU_Insp_Manday_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QU_Insp_Manday_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QU_Insp_Manday_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)
