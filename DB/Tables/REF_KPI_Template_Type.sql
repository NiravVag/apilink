CREATE TABLE [dbo].[REF_KPI_Template_Type]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Active] BIT NOT NULL
)
