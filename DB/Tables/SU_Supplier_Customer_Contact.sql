
CREATE TABLE [dbo].[SU_Supplier_Customer_Contact]
(
	[Supplier_Id] INT NOT NULL, 
	[Customer_Id] INT NOT NULL, 
	[Contact_Id] INT NOT NULL,
	PRIMARY KEY([Supplier_Id],[Customer_Id],[Contact_Id]),
	FOREIGN KEY([Supplier_Id]) REFERENCES [dbo].[SU_Supplier](Id),
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[CU_Customer](Id),
	FOREIGN KEY([Contact_Id]) REFERENCES  [dbo].[SU_Contact](Id)
)