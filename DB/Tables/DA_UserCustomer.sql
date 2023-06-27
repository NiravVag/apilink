CREATE TABLE [dbo].[DA_UserCustomer]
(	
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL, 
    [CustomerId] INT NULL, 
	[EntityId] INT NULL,
	[UserType] INT NOT NULL, 
	[Email] BIT NOT NULL,     
    [CreatedBy] INT NULL,    
    [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
	[Primary_CS] [bit] NULL,
	[Primary_ReportChecker] [bit] NULL,
	CONSTRAINT FK_DAUserCustomer_UserType FOREIGN KEY ([UserType]) REFERENCES [dbo].[HR_Profile](Id),
	CONSTRAINT FK_DAUserCustomer_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	CONSTRAINT FK_DAUserCustomer_UserId FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_DAUserCustomer_CustomerId FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT DA_UserCustomer_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
