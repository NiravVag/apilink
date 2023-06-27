CREATE TABLE [dbo].[CU_CheckPointType]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(1500) NULL, 
    [Active] BIT NOT NULL,
	[Entity_Id] INT NULL,	
	CONSTRAINT FK_CPEntityId FOREIGN KEY([Entity_Id]) REFERENCES [dbo].[AP_Entity](Id)
)
