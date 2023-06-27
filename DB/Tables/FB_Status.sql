CREATE TABLE [dbo].[FB_Status]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Type] [int] Not NULL, 
	[StatusName] NVARCHAR(200) NULL, 
	[FBStatusName] NVARCHAR(200) NULL, 
	[Active] BIT NULL,
	FOREIGN KEY([Type]) REFERENCES [dbo].[FB_Status_Type](Id)
)
