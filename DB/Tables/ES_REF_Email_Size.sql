CREATE TABLE [dbo].[ES_REF_Email_Size]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[Value] float null,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_Email_Size_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
