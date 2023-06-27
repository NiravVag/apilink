﻿CREATE TABLE  QU_Quotation_Audit(
	IdQuotation INT NOT NULL, 
	IdBooking INT NOT NULL,
	[UnitPrice] FLOAT NULL, 
    [NoOfManDay] FLOAT NULL, 
    [InspFees] FLOAT NULL, 
    [TravelLand] FLOAT NULL, 
    [TravelAir] FLOAT NULL, 
    [TravelHotel] FLOAT NULL, 
    [TotalCost] FLOAT NULL, 
    [InvoiceNo] NVARCHAR(1000) NULL, 
    [InvoiceDate] DATETIME NULL, 
    [InvoiceRemarks] NVARCHAR(2000) NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [NoOfTravelManDay] FLOAT NULL,
    [TravelDistance] FLOAT NULL, 
    [TravelTime] FLOAT NULL, 
    PRIMARY KEY (IdQuotation, IdBooking),
	FOREIGN KEY(IdQuotation) REFERENCES [dbo].[QU_Quotation](Id),
	FOREIGN KEY(IdBooking) REFERENCES [dbo].[AUD_Transaction](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
)