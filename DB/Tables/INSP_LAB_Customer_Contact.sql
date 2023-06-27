CREATE TABLE [dbo].[INSP_LAB_Customer_Contact]
(
	[Lab_Id] [int] NOT NULL,
	[Customer_Id] [int] NOT NULL,
	[Contact_Id] [int] NOT NULL,
	PRIMARY KEY([Lab_Id], [Customer_Id], [Contact_Id]),
	FOREIGN KEY([Lab_Id]) REFERENCES [dbo].[INSP_LAB_Details](Id),
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[CU_Customer](Id),
	FOREIGN KEY([Contact_Id]) REFERENCES [dbo].[INSP_LAB_Contact](Id)
)
