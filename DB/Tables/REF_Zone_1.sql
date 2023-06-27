CREATE TABLE [dbo].[REF_Zone]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(500) NULL, 
    [Active] BIT NULL,
	[LocationId] INT NULL,
	[EntityId] INT NULL,
	CONSTRAINT REF_Zone_LocationId  FOREIGN KEY(LocationId) REFERENCES [REF_Location](Id),
	CONSTRAINT FK_REF_Zone_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)