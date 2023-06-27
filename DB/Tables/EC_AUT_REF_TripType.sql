CREATE TABLE [dbo].[EC_AUT_REF_TripType]
(
	[Id] int not null primary key identity(1,1),
	[Name] nvarchar(500),	
	[Sort] int,
	[Active] [bit] NULL
)
