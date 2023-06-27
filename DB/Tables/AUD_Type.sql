CREATE TABLE [dbo].[AUD_Type]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL, 
    [Active] BIT NOT NULL, 
    [Entity_Id] INT NULL,
	FOREIGN KEY([Entity_Id]) REFERENCES [dbo].[AP_Entity](Id)
)
