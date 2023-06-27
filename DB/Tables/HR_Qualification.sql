CREATE TABLE [dbo].[HR_Qualification](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Qualification_Name] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[EntityId] INT NULL, 
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
	)


