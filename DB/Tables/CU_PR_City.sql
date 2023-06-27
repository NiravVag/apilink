CREATE TABLE [dbo].[CU_PR_City]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[Factory_City_Id] int not null,
[Active] bit,
[Created_By] INT NULL, 
[Deleted_By] INT NULL, 
[Updated_By] INT NULL, 
[Updated_On] DATETIME, 
[Created_On] DATETIME NOT NULL DEFAULT GETDATE(), 
[Deleted_On] DATETIME NULL,
[Entity_Id] INT NULL,
CONSTRAINT FK_CU_PR_City_Entity_Id  FOREIGN KEY(Entity_Id) REFERENCES [dbo].[AP_Entity](Id),
CONSTRAINT FK_CU_PR_City_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_City_Factory_City_Id FOREIGN KEY ([Factory_City_Id]) REFERENCES [dbo].[REF_City](Id),	
CONSTRAINT FK_CU_PR_City_Created_By FOREIGN KEY ([Created_By]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_City_Deleted_By FOREIGN KEY ([Deleted_By]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_City_Updated_By FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[IT_UserMaster](Id)
)