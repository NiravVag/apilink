
CREATE TABLE [dbo].[REF_LocationType](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[SGT_Location_Type] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[EntityId] INT NULL, 
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
	)

 
