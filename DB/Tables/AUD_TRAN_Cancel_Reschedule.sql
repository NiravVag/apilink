CREATE TABLE [dbo].[AUD_TRAN_Cancel_Reschedule]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ReasonTypeId] INT NOT NULL, 
    [TimeTypeId] INT NULL, 
    [TravellingExpense] DECIMAL(18, 2) NULL, 
    [CurrencyId] INT NULL, 
    [Comments] NVARCHAR(500) NULL, 
    [InternalComments] NVARCHAR(500) NULL, 
	[Audit_Id] INT NOT NULL, 
    [OperationTypeId] INT NOT NULL, 
    FOREIGN KEY([ReasonTypeId]) REFERENCES [dbo].[AUD_Cancel_Reschedule_Reasons](Id),
	FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id)
)
