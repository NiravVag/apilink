CREATE VIEW [dbo].[V_GetSupplierDetails]

AS 

	SELECT 
	SU.Id,
	SU.Supplier_Name,
	SU_CUS.Customer_Id
	from SU_Supplier SU inner join SU_Supplier_Customer SU_CUS on SU_CUS.Supplier_Id=SU.Id
	where Type_id=2 and Active=1