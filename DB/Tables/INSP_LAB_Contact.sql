CREATE TABLE [dbo].[INSP_LAB_Contact]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Lab_Id] [int] NOT NULL,
	[Contact_Name] [nvarchar](200) NULL,
	[Phone] [nvarchar](100) NULL,
	[Fax] [nvarchar](200) NULL,
	[Mail] [nvarchar](100) NULL,
	[Active] BIT NOT NULL DEFAULT 1,
	[JobTitle] [nvarchar](250) NULL,
	[Mobile] [nvarchar](20) NULL,
	[Comment] [nvarchar](200) NULL,
	FOREIGN KEY ([Lab_Id]) REFERENCES [dbo].[INSP_LAB_Details](Id)
)
