CREATE TABLE [dbo].[ES_REF_Special_Rule]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_Special_Rule_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)