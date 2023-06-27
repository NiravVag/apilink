CREATE TABLE [dbo].[FB_Status_Type]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Name] NVARCHAR(200) NULL, 
    [Active] BIT NULL
)
