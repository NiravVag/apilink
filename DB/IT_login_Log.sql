CREATE TABLE [dbo].[IT_login_Log]
(
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
		[UserItId] int not null,
		[IpAddress] NVARCHAR(1000) NULL, 
		[BrowserType] NVARCHAR(1000) NULL, 	
		[DeviceType] NVARCHAR(1000) NULL, 	
		[LogInTime] DATETIME NULL,
		[LogOutTime] DATETIME NULL,
		[Latitude] DECIMAL(12, 9) NULL, 
		[Longitude] DECIMAL(12, 9) NULL, 	
		FOREIGN KEY(UserItId) REFERENCES IT_UserMaster(Id)
)
