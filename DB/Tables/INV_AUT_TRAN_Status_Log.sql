CREATE TABLE [dbo].[INV_AUT_TRAN_Status_Log]
(
	 [Id] INT IDENTITY(1,1) PRIMARY KEY,	 
	 [Invoice_Id] INT NULL,
	 [Inspection_Id] INT NULL, 
     [Audit_Id] INT NULL,
	 [Status_Id] INT NULL,
     [CreatedBy] INT NULL,
     [CreatedOn] DATETIME NULL,

	 [EntityId] INT NULL, 
    CONSTRAINT INV_AUT_TRAN_Status_Log_Invoice_Id FOREIGN KEY ([Invoice_Id]) REFERENCES [dbo].[INV_AUT_TRAN_Details](Id),
	 CONSTRAINT INV_AUT_TRAN_Status_Log_Inspection_Id FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	 CONSTRAINT INV_AUT_TRAN_Status_Log_Audit_Id FOREIGN KEY ([Audit_Id]) REFERENCES [dbo].[AUD_Transaction](Id),
	 CONSTRAINT INV_AUT_TRAN_Status_Log_Status_Id FOREIGN KEY ([Status_Id]) REFERENCES [dbo].[INV_Status](Id),
	 CONSTRAINT INV_AUT_TRAN_Status_Log_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	 Constraint FK_INV_AUT_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
