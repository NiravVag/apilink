CREATE TABLE EventLog(  
    [ID] int identity primary key,  
	[Name] NVARCHAR(2000),
    [EventID] int,  
    [LogLevel] nvarchar(50), 
    [Message] nvarchar(4000), 
	[Exception] NVARCHAR(MAX) NULL, 
    [CreatedTime] datetime2,
	[EntityId] INT NULL
) 
