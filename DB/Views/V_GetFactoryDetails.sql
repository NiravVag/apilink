CREATE VIEW [dbo].[V_GetFactoryDetails]
AS 
	SELECT 
	SU.Id,
	SU.Supplier_Name as Factory_Name,
	SU_CUS.Customer_Id
	from SU_Supplier SU inner join SU_Supplier_Customer SU_CUS on SU_CUS.Supplier_Id=SU.Id
	where SU.Type_id=1 and SU.Active=1
