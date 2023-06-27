CREATE VIEW V_Suppliers
	AS
	SELECT S.Id, S.Supplier_Name, SC.Customer_Id
	FROM SU_Supplier S
	INNER JOIN  SU_Supplier_Customer SC ON SC.Supplier_Id = S.Id