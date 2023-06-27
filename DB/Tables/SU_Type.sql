CREATE TABLE [dbo].[SU_Type](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Type] [nvarchar](50) NOT NULL,
	[TypeTransId] INT NULL,
	[EntityId] INT NULL, 
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)