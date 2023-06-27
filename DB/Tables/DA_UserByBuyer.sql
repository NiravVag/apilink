CREATE TABLE [dbo].[DA_UserByBuyer]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[BuyerId] INT NULL, 
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByBuyer_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByBuyer_BuyerId FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[CU_Buyer](Id),
CONSTRAINT FK_DAUserByBuyer_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id)
)
