CREATE TABLE [dbo].[INSP_REF_PaymentOption](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](100),
	[CustomerId] [int],
	[Active] [bit],
	[Sort] [int],
	CONSTRAINT [FK_INSP_REF_PAYOPTION_CUSTOMERID] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[CU_Customer] ([Id])
	)