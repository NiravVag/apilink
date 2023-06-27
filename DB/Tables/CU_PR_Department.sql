CREATE TABLE [dbo].[CU_PR_Department]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Cu_Price_Id] INT NOT NULL,
	[Department_Id] INT,
	[Active] BIT,
	[CreatedOn] DATETIME,
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	FOREIGN KEY([Department_Id]) REFERENCES [dbo].[CU_Department](Id),
	FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
