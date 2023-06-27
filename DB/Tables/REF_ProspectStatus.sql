CREATE TABLE [dbo].[REF_ProspectStatus]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(100) NULL,
	[EntityId] INT NULL,
	CONSTRAINT REF_ProspectStatus_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
