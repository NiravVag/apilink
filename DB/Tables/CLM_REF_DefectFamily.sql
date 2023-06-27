CREATE TABLE [dbo].[CLM_REF_DefectFamily]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Active] BIT NOT NULL, 
    [Sort] INT NULL
)
