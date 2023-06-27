CREATE TABLE [dbo].[INSP_LAB_Details]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Lab_Name] [nvarchar](200) NULL,
	[Comments] [nvarchar](200) NULL,
	[Active] BIT NOT NULL DEFAULT 1,
	[Type_Id] [int] NULL,
	[LegalName] [nvarchar](200) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](100) NULL,
	[Fax] [nvarchar](200) NULL,
	[Website] [nvarchar](200) NULL,
	[Mobile] [nvarchar](20) NULL,
	[RegionalName] [nvarchar](200) NULL,
	[ContactPerson] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[GlCode] [nvarchar](500) NULL,
	[EntityId] INT NULL,
	FOREIGN KEY([Type_Id]) REFERENCES [INSP_LAB_Type](Id),
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)


)
