CREATE TABLE [dbo].[REF_Pick1]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Value] FLOAT NULL, 
    [Active] BIT NOT NULL,
	[EntityId] INT NULL,
	CONSTRAINT REF_Pick1_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
