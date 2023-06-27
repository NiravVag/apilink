CREATE TABLE [dbo].[QU_TRAN_Status_Log]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [QuotationId] INT NOT NULL, 
    [BookingId] INT NULL, 
    [AuditId] INT NULL, 
    [StatusId] INT NOT NULL, 
    [StatusChangeDate] DATETIME NOT NULL, 
    [CreatedBy] INT NULL,
	[CreatedOn] DATETIME NOT NULL, 
    [EntityId] INT NULL, 
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY([StatusId]) REFERENCES [dbo].[QU_Status](Id),
	FOREIGN KEY([QuotationId]) REFERENCES [QU_Quotation](Id),
    Constraint FK_QU_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
