CREATE TABLE [dbo].[INSP_CU_Status]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [StatusId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [CustomStatusName] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL,
	FOREIGN KEY(CustomerId) REFERENCES [CU_Customer](Id), 
	FOREIGN KEY([StatusId]) REFERENCES INSP_Status(Id)
)
