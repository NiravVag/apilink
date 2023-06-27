
CREATE TABLE [dbo].[ES_REF_Report_Send_Type]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_Report_Send_Type_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
