CREATE TABLE [dbo].[QU_INSP_Product]
(		
		IdQuotation INT NOT NULL, 
		ProductTranId INT NOT NULL,
		SampleQty INT NOT NULL,
		AqlLevelDesc NVARCHAR(600) NULL,
		PRIMARY KEY(IdQuotation, ProductTranId),
		FOREIGN KEY(IdQuotation) REFERENCES [dbo].[QU_Quotation](Id),
		FOREIGN KEY (ProductTranId) REFERENCES [dbo].[INSP_Product_Transaction](Id)
)
