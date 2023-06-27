CREATE TABLE [dbo].[REF_Product_Category_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Product_Category_Id] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[CustomName] NVARCHAR(50) NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY([Product_Category_Id]) REFERENCES [dbo].[REF_ProductCategory](Id),
	FOREIGN KEY([ServiceId]) REFERENCES [dbo].[REF_API_Services](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
