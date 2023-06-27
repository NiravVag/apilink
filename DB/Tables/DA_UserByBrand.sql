CREATE TABLE [dbo].[DA_UserByBrand]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[BrandId] INT NULL, 
[EntityId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByBrand_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByBrand_BrandId FOREIGN KEY ([BrandId]) REFERENCES [dbo].[CU_Brand](Id),
CONSTRAINT FK_DAUserByBrand_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id),
CONSTRAINT FK_DAUserByBrand_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
