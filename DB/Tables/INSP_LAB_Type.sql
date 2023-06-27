CREATE TABLE [dbo].[INSP_LAB_Type]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Type] [nvarchar](200) NULL,
	[TypeTransId] [int] NULL,
	[EntityId] [int] NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
