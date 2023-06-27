CREATE TABLE QU_Quotation_Contact(
	IdQuotation INT NOT NULL, 
	IdContact INT NOT NULL,
	Quotation BIT NOT NULL DEFAULT(0),
	Email BIT NOT NULL DEFAULT(0),
	PRIMARY KEY(IdQuotation, IdContact),
	FOREIGN KEY(IdQuotation) REFERENCES [dbo].[QU_Quotation](Id),
	FOREIGN KEY(IdContact) REFERENCES [dbo].[HR_Staff](Id)
)