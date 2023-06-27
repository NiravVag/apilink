CREATE TABLE [dbo].[CU_CustomerGroup]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
 
    [Name] NVARCHAR(100) NOT NULL,

	[EntityId] INT NULL,

	CONSTRAINT CU_CustomerGroup_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

)
