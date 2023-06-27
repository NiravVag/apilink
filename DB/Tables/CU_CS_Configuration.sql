CREATE TABLE [dbo].[CU_CS_Configuration]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Customer_Id] INT NOT NULL,
	[User_Id] INT NOT NULL,
	[Active] BIT NOT NULL
	FOREIGN KEY(Customer_Id) REFERENCES [CU_Customer](Id)
	FOREIGN KEY(User_Id) REFERENCES [HR_Staff](Id)
)
