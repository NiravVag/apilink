CREATE TABLE [dbo].[FB_Report_InspSummary_Type]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Type] NVARCHAR(100) NULL, 
	[Active] BIT NULL
)