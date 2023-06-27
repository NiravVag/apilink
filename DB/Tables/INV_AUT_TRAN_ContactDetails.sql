CREATE TABLE [dbo].[INV_AUT_TRAN_ContactDetails]
(
	  [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [Invoice_Id] INT NULL,
	  [Customer_Contact_Id] INT NULL,	
	  [Supplier_Contact_Id] INT NULL,
	  [Factory_Contact_Id] INT NULL, 
	  
	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Invoice_Id FOREIGN KEY ([Invoice_Id]) REFERENCES [dbo].[INV_AUT_TRAN_Details](Id),
	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Customer_Contact_Id FOREIGN KEY ([Customer_Contact_Id]) REFERENCES [dbo].[CU_Contact](Id),
	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Supplier_Contact_Id FOREIGN KEY ([Supplier_Contact_Id]) REFERENCES [dbo].[SU_Contact](Id),
	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Factory_Contact_Id	 FOREIGN KEY ([Factory_Contact_Id]) REFERENCES [dbo].[SU_Contact](Id)
)
