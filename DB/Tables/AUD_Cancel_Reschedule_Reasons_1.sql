CREATE TABLE [dbo].[AUD_Cancel_Reschedule_Reasons]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Reason] NVARCHAR(500) NOT NULL, 
    [IsCancel] BIT NOT NULL, 
    [IsReschedule] BIT NOT NULL, 
    [IsDefault] BIT NOT NULL, 
    [IsSgT] BIT NOT NULL, 
    [Customer_Id] INT NULL, 
    [Active] BIT NOT NULL, 
	 [EntityId] INT NULL, 
	FOREIGN KEY ([Customer_Id]) REFERENCES [dbo].[CU_Customer](Id),
	 FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id)
)
