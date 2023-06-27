CREATE TABLE [dbo].[ES_Special_Rule]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[Special_Rule_Id] int null,
	[Es_Details_Id] int null,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
	CONSTRAINT FK_ES_Special_Rule_Es_Details_Id FOREIGN KEY ([Es_Details_Id]) REFERENCES [dbo].[ES_Details](Id),
	CONSTRAINT FK_ES_Special_Rule_Special_Rule_Id FOREIGN KEY ([Special_Rule_Id]) REFERENCES [dbo].[ES_REF_Special_Rule](Id),
	CONSTRAINT FK_ES_Special_Rule_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
