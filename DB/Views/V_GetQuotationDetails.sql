CREATE VIEW [dbo].[V_GetQuotationDetails]
AS 
	SELECT 
	 QUINSP.IdBooking,
	 QUINSP.IdQuotation,
	 QUINSP.InspFees,
	 QUINSP.InvoiceDate,
	 QUINSP.InvoiceNo,
	 QUINSP.InvoiceRemarks,
	 QUINSP.TravelAir,
	 QUINSP.TravelDistance,
	 QUINSP.TravelHotel,
	 QUINSP.TravelLand,
	 QUINSP.TravelTime,
	 QUINSP.UnitPrice,
	 QUINSP.TotalCost,
	 QUINSP.NoOfManDay,
	 QUINSP.NoOfTravelManDay
	 FROM QU_Quotation_Insp QUINSP  
	 INNER JOIN QU_QUOTATION QUOTATION ON QUOTATION.Id =QUINSP.IdQuotation AND QUOTATION.IdStatus!=5
