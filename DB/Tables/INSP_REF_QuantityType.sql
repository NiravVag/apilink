CREATE TABLE [dbo].[INSP_REF_QuantityType]
(
	[Id] int identity(1,1) primary key,
	[Name] NVARCHAR(50),
	[Active] BIT,
	[Sort] int
)
