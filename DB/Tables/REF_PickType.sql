CREATE TABLE [dbo].[REF_PickType]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Value] NVARCHAR(50) NULL, 
    [Active] BIT NOT NULL,
	[EntityId] INT NULL,
	CONSTRAINT REF_PickType_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
