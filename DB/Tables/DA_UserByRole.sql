
CREATE TABLE [dbo].[DA_UserByRole]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[RoleId] INT NOT NULL, 
[EntityId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByRole_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByRole_RoleId FOREIGN KEY ([RoleId]) REFERENCES [dbo].[IT_Role](Id),
CONSTRAINT FK_DAUserByRole_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id),
CONSTRAINT FK_DAUserByRole_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)