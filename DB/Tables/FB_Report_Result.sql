CREATE TABLE [dbo].[FB_Report_Result]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[ResultName] NVARCHAR(200) NULL, 
	[Active] BIT NULL
)
