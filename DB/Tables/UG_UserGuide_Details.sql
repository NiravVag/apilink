CREATE TABLE [dbo].[UG_UserGuide_Details]
(
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](50),
	[FileUrl] [nvarchar](500),
	[VideoUrl] [nvarchar](500),
	[TotalPage] [int],
	[Is_Customer] [bit],
	[Is_Supplier] [bit],
	[Is_Factory] [bit],
	[Is_Internal] [bit],
	[EntityId] [int],
	[Active] [bit]
)
