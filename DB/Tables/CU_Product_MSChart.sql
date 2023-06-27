CREATE TABLE [dbo].[CU_Product_MSChart]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

  [Product_Id] int NOT NULL, 
 
  [Product_File_Id] int NULL,
 
  [Code] nvarchar(1000) NULL,
 
  [Description] nvarchar(2000) NULL,
 
  [MPCode] nvarchar(500) NULL,
 
  [Required] float NULL,
 
  [Tolerance1_Up] float NULL,
 
  [Tolerance1_Down] float NULL,
 
  [Tolerance2_Up] float NULL,
 
  [Tolerance2_Down] float NULL,
 
  [Sort] INT NULL,
 
  [CreatedOn] DATETIME NULL,
 
  [CreatedBy] INT NULL,
 
  [UpdatedBy] INT NULL,
 
  [UpdatedOn] DATETIME NULL,

  CONSTRAINT FK_CU_Product_MSChart_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	

  CONSTRAINT FK_CU_Product_MSChart_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	

  CONSTRAINT FK_CU_Product_MSChart_Product_Id FOREIGN KEY ([Product_Id]) REFERENCES [dbo].[CU_Products](Id),

  CONSTRAINT FK_CU_Product_MSChart_Product_File_Id FOREIGN KEY ([Product_File_Id]) REFERENCES [dbo].[CU_Product_File_Attachment](Id)
)
