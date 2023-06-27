CREATE TABLE [dbo].[SU_Supplier_Customer]
(
	[Supplier_Id] INT NOT NULL, 
	[Customer_Id] INT NOT NULL, 
	[Code] NVARCHAR(100) NULL,
	[Credit_Term] INT,
	PRIMARY KEY([Supplier_Id],[Customer_Id]),
	FOREIGN KEY([Supplier_Id]) REFERENCES [dbo].[SU_Supplier](Id),
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT FK_SU_Supplier_Customer_Credit_Term FOREIGN KEY (Credit_Term) REFERENCES SU_CreditTerm(Id)
)