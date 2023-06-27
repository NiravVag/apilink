CREATE TABLE [dbo].[INV_EXF_TRAN_Status_Log]
(
	 [Id] INT IDENTITY(1,1) PRIMARY KEY not null,	 
	 [ExtraFee_Id] INT NULL,
	 [Inspection_Id] INT NULL, 
     [Audit_Id] INT NULL,
	 [Status_Id] INT NULL,
     [CreatedBy] INT NULL,
     [CreatedOn] DATETIME default getdate()
	 CONSTRAINT INV_EXF_TRAN_Status_Log_ExtraFee_Id FOREIGN KEY ([ExtraFee_Id]) REFERENCES [dbo].[INV_EXF_Transaction](Id),
	 [EntityId] INT NULL, 
    CONSTRAINT INV_EXF_TRAN_Status_Log_Inspection_Id FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	 CONSTRAINT INV_EXF_TRAN_Status_Log_Audit_Id FOREIGN KEY ([Audit_Id]) REFERENCES [dbo].[AUD_Transaction](Id),
	 CONSTRAINT INV_EXF_TRAN_Status_Log_Status_Id FOREIGN KEY ([Status_Id]) REFERENCES [dbo].[INV_EXF_Status](Id),
	 CONSTRAINT INV_EXF_TRAN_Status_Log_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	 Constraint FK_INV_EXF_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
