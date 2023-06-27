CREATE TABLE [dbo].[ES_SU_Module]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_SU_Module_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)