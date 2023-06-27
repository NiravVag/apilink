CREATE TABLE [dbo].[AUD_Status]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Status] NVARCHAR(200) NULL, 
    [Active] BIT NULL, 
    [Entity_Id] INT NULL,
	FOREIGN KEY([Entity_Id]) REFERENCES [dbo].[AP_Entity](Id)
)
