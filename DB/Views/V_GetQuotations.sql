CREATE VIEW V_GetQuotations
	AS
	SELECT Q.*, RC.Country_Name, S.Supplier_Name, C.Customer_Name, QQINSP.IdBooking
	FROM QU_QUOTATION Q
	INNER JOIN QU_Quotation_Insp QQINSP ON QQINSP.IdQuotation = Q.Id
	INNER JOIN REF_Country RC  ON RC.Id = Q.CountryId
	INNER JOIN SU_Supplier S ON S.Id = Q.SupplierId
	INNER JOIN CU_CUSTOMER C ON C.Id = Q.CustomerId