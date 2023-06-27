CREATE TABLE [dbo].[RestApiLog]
(
	[ID] int identity primary key,  
	[RequestMethod] NVARCHAR(200),
    [RequestPath] NVARCHAR(2000),
	[RequestQuery] NVARCHAR(2000),   
	[RequestBody] NVARCHAR(max),   
    [RequestTime] datetime,
	[ResponseTime] datetime,
	[ResponseInMilliSeconds] INT,
	[ResponseStatus] NVARCHAR(2000),   
	[EntityId] INT NULL
)
