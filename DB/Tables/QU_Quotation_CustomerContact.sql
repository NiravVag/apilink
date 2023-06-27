CREATE TABLE QU_Quotation_CustomerContact(
	IdQuotation INT NOT NULL, 
	IdContact INT NOT NULL,
	Quotation BIT NOT NULL DEFAULT(0),
	Email BIT NOT NULL DEFAULT(0),
	[InvoiceEmail] BIT NOT NULL DEFAULT 0,  
    PRIMARY KEY(IdQuotation, IdContact),
	FOREIGN KEY(IdQuotation) REFERENCES [dbo].[QU_Quotation](Id),
	FOREIGN KEY(IdContact) REFERENCES [dbo].[CU_Contact](Id)
)