CREATE TABLE [dbo].[CU_SisterCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CustomerId] [int] NOT NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[EntityId] [int] NOT NULL,
	[SisterCompanyId] [int] NOT NULL,
	CONSTRAINT FK_CU_SisterCompany_CustomerId FOREIGN KEY (CustomerId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_CU_SisterCompany_SisterCompanyId FOREIGN KEY (SisterCompanyId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_CU_SisterCompany_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id),
	CONSTRAINT FK_CU_SisterCompany_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_CU_SisterCompany_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster(Id)
)
