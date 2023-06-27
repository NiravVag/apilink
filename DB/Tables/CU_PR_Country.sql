CREATE TABLE [dbo].[CU_PR_Country]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[FactoryCountryId] int not null,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL,
[EntityId] INT NULL,
CONSTRAINT CU_PR_Department_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
CONSTRAINT FK_CU_PR_Country_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_Country_FactoryCountryId FOREIGN KEY ([FactoryCountryId]) REFERENCES [dbo].[REF_Country](Id),	
CONSTRAINT FK_CU_PR_Country_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_Country_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_Country_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
