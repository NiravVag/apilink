CREATE TABLE [dbo].[INV_REF_InterventionType]
(
	[Id] int identity(1,1) primary key,
	[Name] NVARCHAR(50),
	[Active] BIT,
	[Sort] int
)
