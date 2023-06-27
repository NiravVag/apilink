
CREATE VIEW [dbo].[V_GetInspectionDetails]
AS
    SELECT 
    INSPECTION.Id,
	INSPECTION.ServiceDate_From,
	INSPECTION.ServiceDate_To,

	INSPECTION.FirstServiceDate_From,
	INSPECTION.FirstServiceDate_To,

	INSPSTATUS.Id AS Status_Id,
	INSPSTATUS.Status,

	INSPECTION.CreatedOn,

	CUSTOMER.Id AS Customer_Id,
	CUSTOMER.Customer_Name,

	CUADDRESS.Address AS Customer_Address,
	CUCOUNTRY.Country_Name AS Customer_Country,
	CUCITY.City_Name AS Customer_City,

	CUBRAND.Id AS Brand_Id,
	CUBRAND.Name AS Brand_Name,

	CUDEPT.Id AS Department_Id,
	CUDEPT.Name AS Department_Name,

	CUMERCH.Id AS Merchandiser_Id,
	CUMERCH.Contact_name AS Merchandiser_Name,

	CUBUYER.Id AS Buyer_Id,
	CUBUYER.Name AS Buyer_Name,


	SUPPLIER.Id AS Supplier_Id,
	SUPPLIER.Supplier_Name AS Supplier_Name,
	SUADDRESS.Address AS Supplier_Address,
	SUADDRESS.LocalLanguage AS Supplier_Regional_Address,
	SUCOUNTRY.Country_Name AS Supplier_Country,
	SUPROVINCE.Province_Name AS Supplier_Province,
	SUCITY.City_Name AS Supplier_City,
	SUCOUNTY.County_Name AS Supplier_County,

	FACTORY.Id AS Factory_Id,
	FACTORY.Supplier_Name AS Factory_Name,
	FAADDRESS.Address AS Factory_Address,
	FAADDRESS.LocalLanguage AS Factory_Regional_Address,
	FACOUNTRY.Country_Name AS Factory_Country,
	FAPROVINCE.Province_Name AS Factory_Province,
	FACITY.City_Name AS Factory_City,
	FACOUNTY.County_Name AS Factory_County,
	FACTOWN.TownName AS Factory_Town,

	CUCONTACT.Contact_name AS Customer_Contact,
	SUCONTACT.Contact_name AS Supplier_Contact,
	FACONTACT.Contact_name AS Factory_Contact,

	INSPOFFICE.Id AS Location_Id,
	INSPOFFICE.Location_Name AS Office,

	SERVICETYPE.Id As ServiceTypeId,
	SERVICETYPE.Name As ServiceType,
	

	-- PRODUCTS
	CUPRODUCT.ProductID AS Product_Name,
	CUPRODUCT.[Product Description] AS Product_Description,
	PRODUCTCAT.Name AS Product_Category,
	PRODUCTSUBCAT.Name AS Product_Sub_Category,

	--po
	CUPO.PONO AS PoName,
	INSPPO.BookingQuantity AS PoQuantity,

	--Container
	'Container - ' +CONVERT(VARCHAR(20), INSPCONTAINER.Container_Id)  AS Container_Name  ,

	-- QC

	STAFFQC.Person_Name AS Schedule_QCName,

	-- Reports

	REPORT.ReportTitle AS Report_Title,
	REPORT.ServiceFromDate AS Report_FromDate,
	REPORT.ServiceToDate AS Report_ToDate,
	REPORT.ResultId AS Report_ResultId,
	REPROTRES.ResultName AS Report_Result,
	REPORT.CriticalMax As Defect_Critical_Max,
	REPORT.MajorMax As Defect_Major_Max,
	REPORT.MinorMax As Defect_Minor_Max,

	REPORTSUMMARY.Name AS Report_Criteria_Name,
	REPROTSUMMARYRES.ResultName AS Report_Criteria_Result,
	REPORTSUMMARY.Remarks AS Report_Criteria_Remarks,

	REPORTDEFECT.CategoryName AS Defect_CategoryName,
	REPORTDEFECT.Description AS Defect_Description,
	REPORTDEFECT.Critical AS Defect_Found_Critical,
	REPORTDEFECT.Major AS Defect_Found_Major,
	REPORTDEFECT.Minor AS Defect_Found_Minor,

	REPORTQTY.InspectedQuantity AS Inspected_Quantity,
	REPORTQTY.PresentedQuantity AS Shipment_Quantity

	FROM INSP_Transaction INSPECTION  


	-- Customer and Customer Address
	INNER JOIN CU_Customer CUSTOMER  ON CUSTOMER.Id = INSPECTION.Customer_Id
	INNER JOIN CU_Address CUADDRESS  ON CUADDRESS.Customer_Id = INSPECTION.Customer_Id and CUADDRESS.Address_Type=1 -- only head office
	LEFT JOIN REF_Country CUCOUNTRY ON CUCOUNTRY.Id=CUADDRESS.Country_Id
	LEFT JOIN REF_City CUCITY ON CUCITY.Id=CUADDRESS.City_Id

	-- Brand
	LEFT JOIN INSP_TRAN_CU_Brand INSPBRAND  ON INSPBRAND.Inspection_Id = INSPECTION.Id and INSPBRAND.Active=1
	LEFT JOIN CU_Brand CUBRAND ON CUBRAND.Id=INSPBRAND.Brand_Id

	-- Department
	LEFT JOIN INSP_TRAN_CU_Department INSPDEPT  ON INSPDEPT.Inspection_Id = INSPECTION.Id and INSPDEPT.Active=1
	LEFT JOIN CU_Department CUDEPT ON CUDEPT.Id=INSPDEPT.Department_Id

	-- Buyer
	LEFT JOIN INSP_TRAN_CU_Buyer INSPBUYER  ON INSPBUYER.Inspection_Id = INSPECTION.Id and INSPBUYER.Active=1
	LEFT JOIN CU_Buyer CUBUYER ON CUBUYER.Id=INSPBUYER.Buyer_Id

	-- Merchandiser
	LEFT JOIN INSP_TRAN_CU_Merchandiser INSPMERCH  ON INSPMERCH.Inspection_Id = INSPECTION.Id and INSPMERCH.Active=1
	LEFT JOIN CU_Contact CUMERCH ON CUMERCH.Id=INSPMERCH.Merchandiser_Id


	-- Supplier and Supplier Address 
	INNER JOIN SU_Supplier SUPPLIER  ON SUPPLIER.Id = INSPECTION.Supplier_Id
	INNER JOIN SU_Address SUADDRESS  ON SUADDRESS.Supplier_Id = SUPPLIER.Id and SUADDRESS.AddressTypeId=1  -- only head office
	INNER JOIN REF_Country SUCOUNTRY ON SUCOUNTRY.Id=SUADDRESS.CountryId
	INNER JOIN REF_Province SUPROVINCE ON  SUPROVINCE.Id=SUADDRESS.RegionId
	INNER JOIN REF_City SUCITY ON SUCITY.Id=SUADDRESS.CityId
    LEFT JOIN REF_County SUCOUNTY ON SUCOUNTY.Id=SUADDRESS.CountyId 

	-- Factory and Factory Address 

	INNER JOIN SU_Supplier FACTORY  ON FACTORY.Id = INSPECTION.Factory_Id
	INNER JOIN SU_Address FAADDRESS  ON FAADDRESS.Supplier_Id = FACTORY.Id and FAADDRESS.AddressTypeId=1 -- only head office
	INNER JOIN REF_Country FACOUNTRY ON FACOUNTRY.Id=FAADDRESS.CountryId
	INNER JOIN REF_Province FAPROVINCE ON  FAPROVINCE.Id=FAADDRESS.RegionId
	INNER JOIN REF_City FACITY ON FACITY.Id=FAADDRESS.CityId
    LEFT JOIN  REF_County FACOUNTY ON FACOUNTY.Id=FAADDRESS.CountyId 	
	LEFT JOIN  REF_Town FACTOWN ON FACTOWN.Id=FAADDRESS.TownId 	

	-- Customer Contact and fetch only active contacts
	INNER JOIN INSP_TRAN_CU_Contact INSPCUCONTACT ON INSPCUCONTACT.Inspection_Id=INSPECTION.Id and INSPCUCONTACT.Active=1
	INNER JOIN CU_Contact CUCONTACT ON CUCONTACT.Id=INSPCUCONTACT.Contact_Id

	-- Supplier Contact and fetch only active contacts
	INNER JOIN INSP_TRAN_SU_Contact INSPSUCONTACT ON INSPSUCONTACT.Inspection_Id=INSPECTION.Id and INSPSUCONTACT.Active=1
	INNER JOIN SU_Contact SUCONTACT ON SUCONTACT.Id=INSPSUCONTACT.Contact_Id

	-- Factory Contact and fetch only active contacts
	INNER JOIN INSP_TRAN_FA_Contact INSPFACONTACT ON INSPFACONTACT.Inspection_Id=INSPECTION.Id and INSPFACONTACT.Active=1
	INNER JOIN SU_Contact FACONTACT ON FACONTACT.Id=INSPFACONTACT.Contact_Id

	-- Inspection status 
	INNER JOIN INSP_Status INSPSTATUS ON INSPSTATUS.Id=INSPECTION.Status_Id

	-- Inspection Office
	LEFT JOIN REF_Location INSPOFFICE ON INSPOFFICE.Id=INSPECTION.Office_Id

	-- Inspection Service fetch only for active service
	INNER JOIN INSP_TRAN_ServiceType INSPSERVICETYPE ON INSPSERVICETYPE.Inspection_Id=INSPECTION.Id and INSPSERVICETYPE.Active=1
	INNER JOIN REF_ServiceType SERVICETYPE ON SERVICETYPE.Id=INSPSERVICETYPE.ServiceType_Id

	-- Inspection Products
	INNER JOIN INSP_Product_Transaction INSPPRODUCT on INSPPRODUCT.Inspection_Id=INSPECTION.Id and INSPPRODUCT.Active=1
	INNER JOIN CU_Products CUPRODUCT on CUPRODUCT.Id=INSPPRODUCT.Product_Id 
	INNER JOIN REF_ProductCategory PRODUCTCAT on PRODUCTCAT.Id=CUPRODUCT.ProductCategory
	INNER JOIN REF_ProductCategory_Sub PRODUCTSUBCAT on PRODUCTSUBCAT.Id=CUPRODUCT.ProductSubCategory

	-- Report related information

	LEFT JOIN FB_Report_Details REPORT on REPORT.Id=INSPPRODUCT.Fb_Report_Id and REPORT.Active=1
	LEFT JOIN FB_Report_Result REPROTRES on REPROTRES.Id =REPORT.ResultId 

	LEFT JOIN FB_Report_InspSummary REPORTSUMMARY on REPORTSUMMARY.FbReportDetailId=INSPPRODUCT.Fb_Report_Id and REPORTSUMMARY.Active=1	
	LEFT JOIN FB_Report_Result REPROTSUMMARYRES on REPROTSUMMARYRES.Id =REPORTSUMMARY.ResultId 

	LEFT JOIN FB_Report_InspDefects REPORTDEFECT on REPORTDEFECT.FbReportDetailId=INSPPRODUCT.Fb_Report_Id and REPORTDEFECT.Active=1
	LEFT JOIN FB_Report_Quantity_Details REPORTQTY on REPORTQTY.FbReportDetailId=INSPPRODUCT.Fb_Report_Id and REPORTQTY.Active=1

	-- Inspection Purchase order
	INNER JOIN INSP_PurchaseOrder_Transaction INSPPO on INSPPO.Inspection_Id=INSPECTION.Id and INSPPO.Active=1
	INNER JOIN CU_PurchaseOrder CUPO on CUPO.Id=INSPPO.PO_Id 

	-- Inspection Container
	LEFT JOIN INSP_Container_Transaction INSPCONTAINER ON INSPCONTAINER.Inspection_Id=INSPECTION.Id and INSPCONTAINER.Active=1

	-- Schedule QC 

	LEFT JOIN SCH_Schedule_QC SCHQC on SCHQC.BookingId=INSPECTION.Id and SCHQC.Active=1
	INNER JOIN HR_Staff STAFFQC on STAFFQC.Id=SCHQC.QCId