﻿CREATE TABLE [dbo].[CU_ProductCategory]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(500),
	Code NVARCHAR(500),
	Active BIT,
	CustomerId INT,
	Sector NVARCHAR(500),
	Sort INT,
	EntityId INT,
	CONSTRAINT FK_CUSTOMER_CU_PRODUCT_CATEGORY FOREIGN KEY(CustomerId) REFERENCES [dbo].[CU_Customer],
	CONSTRAINT FK_CU_PRODUCT_CATEGORY_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]
)
