CREATE TABLE [dbo].[JOB_Schedule_Job_Type]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] [nvarchar](200) NOT NULL, 
    [Active] BIT NOT NULL, 
    [Sort] INT NULL
)
