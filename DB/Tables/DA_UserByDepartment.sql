CREATE TABLE [dbo].[DA_UserByDepartment]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[DepartmentId] INT NULL, 
[EntityId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByDepartment_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByDepartment_DepartmentId FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[CU_Department](Id),
CONSTRAINT FK_DAUserByDepartment_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id),
CONSTRAINT FK_DAUserByDepartment_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)

