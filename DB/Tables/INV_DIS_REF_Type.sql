
CREATE TABLE [dbo].[INV_DIS_REF_Type](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [varchar](100) NOT NULL,
	[Active] [bit] NULL,
	[Sort] [int] NOT NULL
)