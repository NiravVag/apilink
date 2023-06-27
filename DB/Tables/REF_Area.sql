CREATE TABLE [dbo].[REF_Area](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Area_Name] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL 
)
