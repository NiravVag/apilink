CREATE TABLE [dbo].[Ref_Delimiter]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Is_File] bit,
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_Ref_Delimiter_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)