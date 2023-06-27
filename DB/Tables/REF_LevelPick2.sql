CREATE TABLE [dbo].[REF_LevelPick2]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Value] NVARCHAR(20) NULL, 
    [Active] BIT NOT NULL,
	[EntityId] INT NULL,
	CONSTRAINT REF_LevelPick2_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
