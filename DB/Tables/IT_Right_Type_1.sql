CREATE TABLE [dbo].[IT_Right_Type]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NULL, 
    [Active] BIT NULL,
    [Sort] INT,
    [Service] INT FOREIGN KEY ([Service]) REFERENCES [REF_Service]([Id])
)