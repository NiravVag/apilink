CREATE TABLE [dbo].[AUD_TRAN_Status_Log]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Status_Id] INT NOT NULL, 
	[CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] INT NOT NULL, 
	[Audit_Id] INT NOT NULL, 
    [EntityId] INT NULL, 
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY([Status_Id]) REFERENCES [dbo].[AUD_Status](Id),
	FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id),
	CONSTRAINT FK_AUD_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
