CREATE TABLE [dbo].[INV_REF_BillingFreequency]
(
	[Id] int identity(1,1) primary key,
	[Name] NVARCHAR(50),
	[Active] BIT,
	[Sort] int
)
