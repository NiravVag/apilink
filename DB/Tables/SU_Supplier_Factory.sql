CREATE TABLE [dbo].[SU_Supplier_Factory](
	[Parent_Id] INT NOT NULL,
	[Supplier_Id] INT NOT NULL,
	PRIMARY KEY([Parent_Id],[Supplier_Id]),
	FOREIGN KEY([Supplier_Id]) REFERENCES [SU_Supplier]([Id]),
	FOREIGN KEY([Parent_Id]) REFERENCES [SU_Supplier]([Id])
	)