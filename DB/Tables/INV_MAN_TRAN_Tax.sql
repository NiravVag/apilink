﻿CREATE TABLE INV_MAN_TRAN_TAX(
Id int PRIMARY KEY NOT NULL IDENTITY(1,1),
Man_InvoiceId INT,
TaxId INT,
CreatedOn DATETIME,
CreatedBy INT,
constraint FK_INV_MAN_TRAN_TAX_Man_Invoiceid FOREIGN KEY (Man_InvoiceId) REFERENCES INV_MAN_Transaction(Id),
constraint FK_INV_MAN_TRAN_TAX_TaxId FOREIGN KEY (TaxId) REFERENCES INV_TRAN_Bank_Tax(Id),
constraint FK_INV_MAN_TRAN_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id)
);