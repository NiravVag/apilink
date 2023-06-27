CREATE TABLE [dbo].[INSP_Reschedule_Reasons]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Reason] [nvarchar](500) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsAPI] [bit] NOT NULL,
	[Customer_Id] [int] NULL,
	[Active] [bit] NOT NULL,
	[EntityId] [int] NULL,
CONSTRAINT FK_EntityId FOREIGN KEY(EntityId) REFERENCES [AP_Entity](Id),
CONSTRAINT FK_CustomerId FOREIGN KEY(Customer_Id) REFERENCES [CU_Customer](Id)
)
