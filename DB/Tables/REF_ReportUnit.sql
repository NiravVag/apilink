CREATE TABLE [dbo].[REF_ReportUnit]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Value] NVARCHAR(20) NULL, 
    [Active ] BIT NOT NULL,
	[EntityId] INT NULL,
	CONSTRAINT REF_ReportUnit_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
