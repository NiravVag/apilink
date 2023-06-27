CREATE TABLE [dbo].[INSP_Cancel_Reasons]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Reason] NVARCHAR(500) NOT NULL, 
    [IsDefault] BIT NOT NULL, 
    [IsAPI] BIT NOT NULL, 
    [Customer_Id] INT NULL, 
    [Active] BIT NOT NULL, 
    [EntityId] INT NULL,
	FOREIGN KEY(EntityId) REFERENCES [AP_Entity](Id),
FOREIGN KEY(Customer_Id) REFERENCES [CU_Customer](Id)
)