CREATE TABLE [dbo].[CU_Season]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Customer_Id] INT NOT NULL , 
    [Season_Id] INT NOT NULL, 
    [Active] BIT NOT NULL, 
	[EntityId] INT NULL,
	CONSTRAINT CU_Season_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY(Customer_Id) REFERENCES [CU_Customer](Id),
	FOREIGN KEY(Season_Id) REFERENCES [REF_Season](Id)
)
