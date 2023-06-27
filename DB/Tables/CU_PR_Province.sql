CREATE TABLE [dbo].[CU_PR_Province]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[FactoryProvinceId] int not null,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL,
[EntityId] INT NULL,
CONSTRAINT CU_PR_Province_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
CONSTRAINT FK_CU_PR_Province_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_Province_FactoryProvinceId FOREIGN KEY ([FactoryProvinceId]) REFERENCES [dbo].[REF_Province](Id),	
CONSTRAINT FK_CU_PR_Province_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_Province_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_Province_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
