
CREATE TABLE [dbo].[REF_Currency](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[CurrencyCodeA] [nvarchar](3) NOT NULL,
	[CurrencyCodeN] [int] NULL,
	[MinorUnit] [smallint] NULL,
	[CurrencyName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL
	)

