CREATE TABLE [dbo].[REF_Pick2]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Value] FLOAT NOT NULL, 
    [Active] BIT NOT NULL,
	[EntityId] INT NULL,
	CONSTRAINT REF_Pick2_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
