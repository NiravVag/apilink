CREATE VIEW [dbo].[V_GetCustomerDetails]
AS 
	SELECT 
	Id,
	Customer_Name
	from CU_Customer where Active=1