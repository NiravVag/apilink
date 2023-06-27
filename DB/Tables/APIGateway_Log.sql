CREATE TABLE [dbo].[APIGateway_Log]
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
RequestUrl NVARCHAR(500),
LogInformation NVARCHAR(MAX),
ResponseMessage NVARCHAR(MAX),
RequestBaseUrl NVARCHAR(500),
CreatedOn DATETIME,
CreatedBy INT
)
