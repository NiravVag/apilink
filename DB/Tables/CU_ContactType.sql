CREATE TABLE [dbo].[CU_ContactType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactType] NVARCHAR(200) NULL,
	[EntityId] INT NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
