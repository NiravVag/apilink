CREATE TABLE [dbo].[INSP_LAB_Customer]
(
	[Lab_Id] INT NOT NULL,
	[Customer_Id] INT NOT NULL,
	[Code] [nvarchar](200) NULL,
	PRIMARY KEY([Lab_Id], [Customer_Id]),
	FOREIGN KEY([Lab_Id]) REFERENCES [dbo].[INSP_LAB_Details](Id),
	FOREIGN KEY ([Customer_Id]) REFERENCES [dbo].[CU_Customer](Id)
)
