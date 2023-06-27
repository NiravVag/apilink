CREATE TABLE [dbo].[DA_UserByProductCategory]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[ProductCategoryId] INT NULL,
[EntityId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByProductCategory_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByProductCategory_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[REF_ProductCategory](Id),
CONSTRAINT FK_DAUserByProductCategory_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id),
CONSTRAINT FK_DAUserByProductCategory_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)