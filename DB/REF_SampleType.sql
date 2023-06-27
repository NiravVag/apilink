CREATE TABLE [dbo].[REF_SampleType]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SampleType] NVARCHAR(100) NULL,
	[SampleSize] NVARCHAR(100) NULL,	
	[Active] BIT NULL
)
