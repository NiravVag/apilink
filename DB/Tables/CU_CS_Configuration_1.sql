
	CREATE TABLE [dbo].[CU_CS_Configuration]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Customer_Id] INT NULL,
	[User_Id] INT NOT NULL,
	[Active] BIT NOT NULL,
	[EntityId] INT NULL,
    [Office_Location_Id] INT NOT NULL, 
    [Service_Id] INT NULL, 
    [Product_category_Id] INT NULL,
	CONSTRAINT CU_CS_Configuration_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY(Customer_Id) REFERENCES [CU_Customer](Id),
	FOREIGN KEY(User_Id) REFERENCES [HR_Staff](Id) ,
	FOREIGN KEY(Office_Location_Id) REFERENCES [REF_Location](Id),
	FOREIGN KEY(Service_Id) REFERENCES [REF_Service](Id),
	FOREIGN KEY(Product_category_Id) REFERENCES [REF_ProductCategory](Id), 
)

