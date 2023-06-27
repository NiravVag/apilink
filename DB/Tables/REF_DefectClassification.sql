CREATE TABLE [dbo].[REF_DefectClassification]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Value] NVARCHAR(100) NULL, 
    [Active] BIT NOT NULL,
	[EntityId] INT NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
