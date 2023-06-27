CREATE TABLE CU_Product_MSChart_OCR_MAP (
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CustomerId int NOT NULL,
	OCR_CustomerName nvarchar(500) NULL,
	OCR_FileFormat nvarchar(500) NULL,
	Active bit NOT NULL,
	CreatedBy int NOT NULL,
	CreatedOn datetime NOT NULL,
	DeletedBy int NULL,
	DeletedOn datetime NULL,
	CONSTRAINT FK_CU_Product_MSChart_OCR_MAP_CustomerId FOREIGN KEY(CustomerId) REFERENCES CU_Customer (Id),
	CONSTRAINT FK_CU_Product_MSChart_OCR_MAP_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster (Id),
	CONSTRAINT FK_CU_Product_MSChart_OCR_MAP_DeletedBy FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster (Id)
)