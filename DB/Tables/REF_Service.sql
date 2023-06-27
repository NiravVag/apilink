CREATE TABLE [dbo].[REF_Service]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](200) NULL,
	[Active] [bit] NOT NULL,
	[EntityId] INT NULL, 
	[Fb_Service_Id] INT NULL, 
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
