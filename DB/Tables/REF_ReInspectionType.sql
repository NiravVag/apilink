CREATE TABLE [dbo].[REF_ReInspectionType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50), 
    [Active] BIT NULL
)
