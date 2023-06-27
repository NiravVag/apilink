using DTO.Customer;
using DTO.Dashboard;
using DTO.ExtraFees;
using DTO.Invoice;
using DTO.Report;
using DTO.UtilizationDashboard;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace DTO.Common
{
    public abstract class ApiCommonData
    {
        public const int GapFlashProcessAuditServiceType = 128;
        public const string DefaultEAQFInvoiceNumber = "EAQF-CU-{0}-{1}";
        public const int NewOption = -11;
        public const int CountryCount = 1; // used for customer price card config - province field insert or update or verify
        public const int BookingRuleDays = 2;
        public const string StandardDateFormat = "dd/MM/yyyy";//this is used globally for the date format
        public const string StandardISO8601DateFormat = "yyyy-MM-dd";
        public const string StandardISO8601DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";
        public const string StandardDateFormat1 = "MM/dd/yyyy";
        public const string StandardDateFormat2 = "dd/MMM/yyyy";
        public const string StandardDateFormat3 = "MMM dd, yyyy";
        public const string StandardDateFormat4 = "dd_MM_yyyy";
        public const string StandardDateFormat5 = "yyyy/MM/dd";
        public const string StandardDateTimeFormat = "dd/MM/yyyy HH:mm:ss";//this is used globally for the date format
        public const string StandardDateTimeFormat1 = "dd/MM/yyyy HH:mm";
        public const string StandardTimeFormart = "hh\\:mm";
        public const int PickingCount = 3;
        public const double ProrateMaxRoundValue = 0.5;
        public const double DefaultAdditionalTax = 10;
        public const string Monday = "Monday";

        public const string Pool = "abcdefghijklmnopqrstuvwxyz0123456789";
        //[FB_Report_SampleTypes] table - sample type column
        public const string FBSampleTypeNotProvided = "not_provided";
        public const string FBSampleTypeGoldenSample = "golden_sample";
        public const string FBSampleTypeArtworkColorCard = "artwork_color_card";

        //[FB_Report_SampleTypes] table - description column
        public string[] FBDescriptionGoldenSample = { "golded", "sample", "golden", "gold" };
        public string[] FBDescriptionColorTargetCard = { "color", "target", "card" };

        //[FB_Report_OtherInformation] table - subcategory column
        public const string FBSubCategoryGoldenSample = "Golden sample";
        public const string FBSubCategoryArtworkColorCard = "Artwork / Color Card";

        //fb insp summary main name
        public const string FBInspSummaryMainNameWorkmanship = "Workmanship";
        public const string FBInspSummaryMainNameQuantity = "Quantity";
        public const string FBInspSummaryMainNameProductSpecifications = "Product specifications";
        public const string FBInspSummaryMainNamePacking = "Packing";
        public const string FBInspSummaryMainNameOnsiteTests = "Onsite tests";
        public const string FBInspSummaryMainNameOtherInformation = "Other information";

        //fb insp sub summary sub name
        public const string FBInspSummarySubNameMajor = "Major";
        public const string FBInspSummarySubNameMinor = "Minor";
        public const string FBInspSummarySubNameCritical = "Critical";
        public const double DoubleMinimumValue = 0.00001d;
        public const string CombineOrderString = "(Combine Order for different brand)";
        public const string CombineSpecialCase = "(Combine with other special price)";

        //UtilizationDashboard Manday
        public const int ActualMandayPerServiceDay = 1;
        public const int Feb_29 = 29;
        public const int ColorRangeLow = 1;
        public const int ColorRangeHigh = 8;

        public const int FetchTop5 = 5;

        public const int InvoiceRoundUpValue = 2;

        public const string Api = "API";

        public const int EpplusDefaultColumnWidth = 20;

        //invoice pdf month desc 
        public const string MonthDesc = "Inspection fee from ";
        public const string MonthWord = "st";
        public const string MonthWord1 = "th";
        public const string Month1stWord = "1st";

        //invoice carrefour template
        public const string LocationOffice = "Shanghai";
        public const string LocationOffice1 = "Hongkong";

        public const string DeptDivision = "Hardgoods";
        public const string DeptDivision1 = "Houseware";

        //user name format
        public const string supUserName = "sup_";
        public const string factUserName = "fact_";

        public readonly int splitUserNameSubString = 6;
        public readonly int fullNameTrim = 10;

        public readonly int splitSupplierNameSubString = 4;

        public const string Password = "apidemo";

        public readonly string FBRemarkResult = "pass";

        public readonly string all = "All";
        public readonly string paid = "Paid";
        public readonly string notPaid = "Not Paid";

        //extra fee invoice number format
        public readonly string InvoiceWord = "EXF";
        public readonly string InvoiceUnderScore = "_";

        public readonly string InvoiceCustomer = "CU";
        public readonly string InvoiceSupplier = "SU";
        public readonly string InvoiceFactory = "FA";

        public readonly string InvoiceInspWord = "INS";
        public readonly string InvoiceAudWord = "AUD";

        public readonly string InvalidErrorMessage = " is invalid";

        public readonly int PageSize = 10;

        public const string GuangzhouOffice = "API GUANGZHOU";

        //defect dashboard page starts
        public const string DefectMajor = "Major";
        public const string DefectMinor = "Minor";
        public const string DefectCritical = "Critical";

        public const int DefectCriticalId = 1;
        public const int DefectMajorId = 2;
        public const int DefectMinorId = 3;

        public const string DefectTotalReports = "Total Reports";

        public const int ParetoDefectCount = 5;

        public const int ParetoDefectExportCount = 25;

        public const int DefectCountBySupplierFactory = 10;

        public const int CountryDefectCount = 10;

        public const int Top10Rows = 10;

        public const int DefectDashboardCountryCount = 6;

        public const int QuantitativeDashboardCountryCount = 5;

        public const int QuantitativeDashboardMandayYearCount = 3;

        public const int NumberOne = 1;

        public const int NumberTwo = 2;

        public const int NumberThree = 3;

        public const int NumberSeven = 7;

        public const int DecLastDay = 31;

        public const int DecLastMonth = 12;

        public const int LastThirtyDays = -30;

        public const string EnglishUS = "en-US";

        public const string Mdm_Kpi_sp = "dbo.usp_Defect_KPI_MDM";

        public const string Defect_Summary_Kpi_sp = "dbo.Usp_CustomKPI_DefectSummary";

        public const string FinanceDashboardMandayRate_sp = "dbo.Usp_FinanceKPI_GetMandayRateDetails";

        public const string FinanceDashboardBilledManday_sp = "dbo.Usp_FinanceKPI_GetBilledMandayDetails";

        public const string FinanceDashboardTurnOver_sp = "dbo.Usp_FinanceKPI_GetTurnOver";

        public const string FinanceDashboardChargeBack_sp = "dbo.Usp_FinanceKPI_GetChargeBack";

        public const string FinanceDashboardQuotationCount_sp = "dbo.Usp_FinanceKPI_GetRejectQuotationDetails";

        public const string DbConnectionName = "APIConnection";

        //KPI Custom
        public const string ThirdParty = "API";
        public const string ExportDefectParetoSortDataColumns = "Inspections DESC, Reports DESC, FactoryCountryName, FactoryName, SupplierName, BrandName, DefectCount DESC";

        public const string HardLine = "Hard Line";

        //CS Dashboard SP name starts
        public const string SP_CSDashboard_GetNewDetails = "dbo.Usp_CSDashboard_GetNewDetails";

        public const string SP_CSDashboard_GetServiceType = "dbo.Usp_CSDashboard_GetServiceType";

        public const string SP_CSDashboard_GetMandayByOffice = "dbo.Usp_CSDashboard_GetMandayByOffice";

        public const string SP_CSDashboard_GetReports = "dbo.Usp_CSDashboard_GetReports";

        public const string SP_CSDashboard_GetModuleStatus = "dbo.Usp_CSDashboard_GetModuleStatus";

        public const string TWO_DIGIT_FORMAT = "d2";

        //CS Dashboard SP name ends

        public const string CSDashboard_Allocation_Pending = "Allocation Pending";

        public const string CSDashboard_Allocate = "Allocated";

        public const string CSDashboard_CS_Validate = "CS Validated";

        public const string CSDashboard_Quotation_Modify = "Modify";

        public const string CSDashboard_Created = "Created";

        public const string CSDashboard_Quotation_Approved = "Approved";

        public const string CSDashboard_Quotation_Sent = "Sent";

        public const string CSDashboard_Customer_Confirmed = "Validated";

        public const string Ecopack = "Ecopack : ";

        public readonly List<int> BookingStatusList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        public const string Customer = "Customer";

        public const string Percentage = "%";

        //defect dashboard page ends

        public const string Factory_Address_County_Town = "County, Town";
        public const string Factory_Address_County = "County";
        public const string Factory_Address_Town = "Town";

        public const string Service_Inspection = "Inspection";

        public const string Service_Audit = "Audit";

        public const string MinimumFee = "Minimum Fee: ";
        public const string MaximumFee = "Maximum Fee: ";
        public const string NumberFormat = "#.##########";
        public const string CombineOrderBrand = "(Combine Order for different brand)";
        public const string SpecialFee = "Special Fee: ";
        public const string NormalFee = "Normal Fee: ";

        public const int EmailFileNameMaxChar = 240;

        public const string ScheduleMSChartYes = "Yes";
        public const string ScheduleMSChartNo = "No";

        public const string BadRequest = "Bad Request";
        public const string EmailExists = "Email already exists";
        public const string CompanyNameExists = "Company Name already exists";
        public const string Success = "Success";
        public const string InternalServerError = "Internal ServerError";
        public const string InvalidCountry = "Invalid Country";
        public const string InvalidCity = "Invalid City";
        public const string InvalidVendor = "Invalid Vendor";
        public const string RequiredProdutDetail = "Product data required";
        public const string RequiredVendor = "Vendor data required";
        public const string FactoryNotFound = "Factory not found";
        public const string BookingNotFound = "Booking not found";
        public const string GreterThanTodate = "Service from date is greater than service to date";
        public const string InvalidserviceFromDate = "Invalid Service from date";
        public const string InvalidserviceToDate = "Invalid Service to date";

        public const string InvalidProductCategory = "Product Category not found";
        public const string InvalidProductSubCategory = "Product Sub Category not mapped with Product Category";
        public const string InvalidProductSub2Category = "Product Type not mapped with Product Sub Category";
        public const string InvalidServiceType = "Service Type not mapped with business line";
        public const string InvalidEntity = "Company not found";
        public const string InvalidBookingForCustomer = "Booking not found for this customer";
        public const string CustomerNotFound = "Customer not found";
        public const string ServiceTypeNotFound = "Service type is required";
        public const string DataNotFound = "Data not found";
        public const string CustomerNotMapped = "Customer is not mapped with this user";
        public const string ColorDataRequired = "Color code and name required for Softline";

        public const string CancelRefund = "CancelRefund";
        public const string CancelPartialRefund = "CancelPartialRefund";
        public const string CancelNoRefund = "CancelNoRefund";
        public const string DateChange = "DateChange";
        public const string DateChangePenalty = "DateChangePenalty";

        /// <summary>
        /// Added for Invoice Code dynamic field
        /// </summary>
        public const int InvoiceCodeControlConfigurationId = 20;

        //for default payment status color
        public const string DefaultPaymentStatusColor = "insp-confirm";

        public const double DefaultDecimalNumber = 0.0;

        //for default address
        public const string DefaultAddress = "N/A";

        // Garment Sub Categroy
        public const string ProductCategorySubName = "Garment";

        public const string ExpenseSumaryYes = "Yes";
        public const string ExpenseSumaryNo = "No";

        public const int BarCodeCharactersExceeded = 100;

        public readonly List<int> ActualInvoiceStatusIds = new List<int>()
                        {
                            (int) InvoiceStatus.Modified, (int) InvoiceStatus.Created
                        };

        public readonly List<int> ActualExtraFeesInvoiceStatusIds = new List<int>()
                        {
                            (int) ExtraFeeStatus.Pending, (int) ExtraFeeStatus.Invoiced
                        };


        public enum ExportPagination
        {
            PageIndex = 1,
            PageSize = 9999
        }
        //this is used for sort the table based on status in schedule page
        public enum SortTableOrder
        {
            FirstItem = 1,
            SecondItem = 2
        }
        public enum UserEmailVerification
        {
            ScheduleQCEmail = 1,
            ReportFilledToReportChecker = 2
        }
        public readonly Dictionary<int, string> AuditStatusColor = new Dictionary<int, string>()
        {
            {(int)Entities.Enums.AuditStatus.Received,"#5bc0de" },
            {(int)Entities.Enums.AuditStatus.Confirmed,"#428bca" },
            {(int)Entities.Enums.AuditStatus.Rescheduled,"#FFA8A8" },
            {(int)Entities.Enums.AuditStatus.Cancel,"#dd4b39" },
            {(int)Entities.Enums.AuditStatus.Scheduled,"#f0ad4e" },
            {(int)Entities.Enums.AuditStatus.Audited,"#10ed39" }
        };



        public readonly Dictionary<int, string> InspectionStatusColor = new Dictionary<int, string>()
        {
            {(int)BookingStatus.Received,"#5bc0de" },
            {(int)BookingStatus.Confirmed,"#428bca" },
            {(int)BookingStatus.Rescheduled,"#FFA8A8" },
            {(int)BookingStatus.Cancel,"#dd4b39" },
            {(int)BookingStatus.Scheduled,"#f0ad4e" },
            {(int)BookingStatus.Inspected,"#10ed39" },
             {(int)BookingStatus.Validated,"#10ed39" },
             {(int)BookingStatus.Verified,"#6b03a394" },
             {(int)BookingStatus.AllocateQC,"#ECA22B" },
             {(int)BookingStatus.Hold,"#E86A92" },
            {(int)BookingStatus.ReportSent,"#228b22" }
        };

        public readonly Dictionary<int, string> AuditSummaryStatusColor = new Dictionary<int, string>()
        {
            {(int)Entities.Enums.AuditStatus.Received,"insp-pending" },
            {(int)Entities.Enums.AuditStatus.Confirmed,"insp-confirm" },
            {(int)Entities.Enums.AuditStatus.Rescheduled,"insp-reschedule" },
            {(int)Entities.Enums.AuditStatus.Cancel,"insp-cancel" },
            {(int)Entities.Enums.AuditStatus.Scheduled,"insp-schedule" },
            {(int)Entities.Enums.AuditStatus.Audited,"insp-inspected" }
        };

        public readonly Dictionary<int, string> BookingSummaryInspectionStatusColor = new Dictionary<int, string>()
        {
             {(int)BookingStatus.Received,"insp-pending" },
            {(int)BookingStatus.Confirmed,"insp-confirm" },
            {(int)BookingStatus.Rescheduled,"insp-reschedule" },
            {(int)BookingStatus.Cancel,"insp-cancel" },
            {(int)BookingStatus.Scheduled,"insp-schedule" },
            {(int)BookingStatus.Inspected,"insp-inspected" },
             {(int)BookingStatus.Validated,"insp-validate" },
             {(int)BookingStatus.Verified,"insp-verify" },
             {(int)BookingStatus.AllocateQC,"insp-allocate" },
             {(int)BookingStatus.Hold,"insp-hold" },
            {(int)BookingStatus.ReportSent,"insp-send" }
        };

        /// <summary>
        /// based on the inspection occupancy status color
        /// </summary>
        public readonly Dictionary<int, string> InspectionOccupancyCategoryColor = new Dictionary<int, string>()
        {
             {(int)InspectionOccupancyCategory.High,"insp-inspected"},
             {(int)InspectionOccupancyCategory.Low,"insp-hold"},
              {(int)InspectionOccupancyCategory.Medium,"insp-reschedule"},

        };

        /// <summary>
        /// based on the inspection occupancy status label
        /// </summary>
        public readonly Dictionary<int, string> InspectionOccupancyCategoryLabel = new Dictionary<int, string>()
        {
             {(int)InspectionOccupancyCategory.High,"High (61-100%)"},
             {(int)InspectionOccupancyCategory.Low,"Low (1-30%)"},
              {(int)InspectionOccupancyCategory.Medium,"Medium (31-60%)"},

        };

        public List<string> RemoveColumn = new List<string>() { "FactoryCountryId", "SupplierId", "FactoryId", "BrandId" };

        public readonly Dictionary<int, string> ReportStatusColor = new Dictionary<int, string>()
        {
            {(int)FBStatus.ReportFillingInprogress,"#FFA500" },
            {(int)FBStatus.ReportFillingValidated,"#008000" },
            {(int)FBStatus.ReportReviewInprogress,"#FFA500" },
            {(int)FBStatus.ReportReviewValidated,"#008000" },
            {(int)FBStatus.ReportValidated,"#008000" },
            {(int)FBStatus.ReportInValidated,"#FF0000" }

        };

        public readonly Dictionary<int, string> ReportResultColor = new Dictionary<int, string>()
        {
            {(int)FBReportResult.Pass,"#32CD32" },
            {(int)FBReportResult.Fail,"#FF0000" },
            {(int)FBReportResult.Pending,"#FFA500" },
            {(int)FBReportResult.Not_Applicable,"#FFA500" },
            {(int)FBReportResult.Missing,"#FFA500" },
            {(int)FBReportResult.Conformed, "#198754" },
            {(int)FBReportResult.NotConformed, "#dc3545" },
            {(int)FBReportResult.Delay, "#fd7e14" },
            {(int)FBReportResult.Note, "#6f42c1" }
        };

        public readonly Dictionary<int, string> ExpenseStatusColor = new Dictionary<int, string>()
        {
            {(int)ExpenseClaimStatus.Pending,"#428bca" },
            {(int)ExpenseClaimStatus.Checked,"#f0ad4e" },
            {(int)ExpenseClaimStatus.Approved,"#28a745" },
            {(int)ExpenseClaimStatus.Paid,"#10ed39" },
            {(int)ExpenseClaimStatus.Cancelled,"#dd4b39" },
            {(int)ExpenseClaimStatus.Rejected,"#ef0505" }
        };



        public string DoFormat(double myNumber)
        {
            return string.Format("{0:0.00}", myNumber);
        }


        public readonly Dictionary<int, string> QuotationStatusColor = new Dictionary<int, string>()
        {
            { (int)QuotationStatus.QuotationCreated, "#5bc0de" },
            {(int)QuotationStatus.ManagerApproved, "#428bca" },
            {(int)QuotationStatus.AERejected, "#ff2222" },
            {(int)QuotationStatus.ManagerRejected, "#ff2222" },
            {(int)QuotationStatus.Canceled,"#dd4b39" },
            {(int)QuotationStatus.QuotationVerified, "#f0ad4e" },
             {(int)QuotationStatus.CustomerRejected, "#FFA8A8" },
             {(int)QuotationStatus.CustomerValidated, "#10ed39" },
            { (int)QuotationStatus.SentToClient, "#28a745" }
        };

        public int GetStatusSortForScheduleSummary(BookingStatus status)
        {
            switch (status)
            {
                case BookingStatus.Confirmed:
                    return 1;
                default:
                    return 2;
            }
        }

        public enum BookingStatusNames
        {
            Requested = 1,
            Modified = 2,
            Verified = 3,
            Confirmed = 4,
            Scheduled = 5,
            Cancelled = 6,
            Rescheduled = 7,
            Hold = 8
        }

        public readonly Dictionary<int, string> ICStatusColor = new Dictionary<int, string>()
        {
            {(int)InspectionCertificateStatus.Created,"#5bc0de" },
            {(int)InspectionCertificateStatus.Cancel,"#dd4b39" },
        };

        public Dictionary<string, string> QCInspectionDetailsPDF = new Dictionary<string, string>()
        {
            { "SerialNumber","No" },
            { "Title","Inspection Detail" },
            { "InspectionID","Insp#" },
            { "InspectionDate", "Insp. Date" },
            { "Inspector", "Inspector" },
            { "EstimatedStartTime", "Estimated Start Time" },
            { "EstimatedEndTime", "Estimated End Time" },
            { "CustomerBookingNo", "Customer Booking No" },
            { "Customer","Customer" },
            { "Supplier", "Supplier" },
            { "Factory", "Factory" },
            { "PoNumber","Po No" },
            { "Comments", "Comments" },
            { "ProductName", "Prod Name" },
            { "ProductDescription", "Description" },
            { "DestinationCountry", "DST" },
            { "FactoryReference", "Factory Reference" },
            { "BarCode", "BarCode" },
            { "AQL", "GIL" },
            { "Qty", "Qty" },
            { "SampleSize", "SS" },
            { "CombineProduct", "Combine Product" },
            { "Picking", "Pick." },
            { "Remarks", "Remarks" },
            { "NonCombineProductDetails", "Non Combined:" },
            { "CombinedGroup", "Combined Group-" },
            { "ManDays", "MD" },
            { "TotalReports", "Total Products/ Reports" },
             { "ServiceType", "Service Type" },
            { "TotalCombineProducts", "Total Combine Products" },
            { "Collection", "Collection" },
            { "Brand", "Brand" },
            { "Department", "Department" },
            {"SamplingSize", "Sampling Size : "},
            {"PickingQty", "Picking Qty : " },
            {"TotalSamplingSize", "Total Sampling Size : " },
            {"TotalPickingQty", "Total Picking Qty : " },
            {"Category", "Sub /Sub2" },
            {"Pcsctn", "Pcs/ctn" },
            {"TotalCtn", "Total ctn" },
            {"SelectCtn", "Select ctn" },
            {"CS","CS" },
            {"Color","Color" }
        };

        public readonly Dictionary<string, string> QCInspectionPickingPDF = new Dictionary<string, string>()
        {
            { "Title","Picking Form" },
            { "InspectionID","Inspection Id" },
            { "DeliveryAddress", "Delivery Address" },
            { "Samplecollection", "Sample collection" },
            { "InspectionDate", "Inspection Date" },
            { "Inspector", "Inspector" },
            { "CustomerBookingNo", "Customer Booking No" },
            { "Customer","Customer" },
            { "Supplier", "Supplier" },
            { "Factory", "Factory" },
            { "PoNumber","Po No" },
            { "Comments", "Comments" },
            { "ProductName", "Product Name" },
            { "ProductDescription", "Description" },
            { "DestinationCountry", "Destination Country" },
            { "FactoryReference", "Factory Reference" },
            { "To", "To : " },
            { "Contact", "Contact : " },
            { "Address", "Address : " },
            { "QtySealed", "Qty Sealed" },
            { "QtyReq", "Qty Required" },
            { "CartonNumbers", "Carton Numbers" },
            { "SealsNumbers", "Seals Numbers" },
            { "1", "1" },
            { "2", "2" },
            { "3", "3" },
            {"Lab", "Delivery receipt from Lab" },
            {"API","API" },
            {"InspChop","Inspector Chop No and Signature : " },
            {"APIContact", "Contact Person" },
            {"Name","Name : " },
            {"Phone", "Phone : " },
            {"DeiveryDateadvisedbyFactory","Deivery Date advised by Factory :" },
            {"FactoryRepresentative","Factory Representative" },
            {"DateReceived","Date Received : " },
            {"LabRepresentative","Lab Representative" }
        };

        public readonly Dictionary<int, string> CustomerAPIRADashboardColor = new Dictionary<int, string>()
        {
            {(int)FBReportResult.Pass, "#24C11E" },
            {(int)FBReportResult.Fail, "#F81539" },
            {(int)FBReportResult.Pending, "#F29A0C" },
            {(int)FBReportResult.Not_Applicable, "#D3D3D3" },
            {(int)FBReportResult.Missing, "#9eb9ce" },
            {(int)FBReportResult.Conformed, "#198754" },
            {(int)FBReportResult.NotConformed, "#dc3545" },
            {(int)FBReportResult.Delay, "#fd7e14" },
            {(int)FBReportResult.Note, "#6f42c1" },
        };

        public readonly Dictionary<string, string> CustomerResultDashboardColor = new Dictionary<string, string>()
        {
            { CustomerResult.Pass.ToString(), "#24C11E" },
            { CustomerResult.Fail.ToString(), "#F81539" },
            { CustomerResult.Pending.ToString(), "#F29A0C" },
            { CustomerResult.Derogated.ToString(), "#8296af" }
        };

        public readonly Dictionary<int, string> ProductCategoryDashboardColor = new Dictionary<int, string>()
        {
            { (int)ProductCategoryEnum.ElectronicAndElectrical, "#7492D7" },
            { (int)ProductCategoryEnum.Furniture, "#978AF4" },
             {(int)ProductCategoryEnum.Toys,"#C890F6" },
            {(int)ProductCategoryEnum.BabyCare,"#D690C8" },
            {(int)ProductCategoryEnum.Tools,"#9EABC9" },
            {(int)ProductCategoryEnum.SportsFitnessandCamping,"#7BD2BD" },
            {(int)ProductCategoryEnum.PersonalCare,"#4A90E2" },
            {(int)ProductCategoryEnum.HomeProducts,"#AB8ABD" },
             {(int)ProductCategoryEnum.Stationery,"#6BD0E9" },
             {(int)ProductCategoryEnum.LuggageAndBags,"#7B68EE" },
             {(int)ProductCategoryEnum.CarAccessories,"#8B0000" },
             {(int)ProductCategoryEnum.NFR,"#800000" },
             {(int)ProductCategoryEnum.TextileandFootware,"#F5DEB3" },
             {(int)ProductCategoryEnum.FUR,"#DAA520" },
              {(int)ProductCategoryEnum.Garment,"#7492D7" },
             {(int)ProductCategoryEnum.Fabric,"#978AF4" },
             {(int)ProductCategoryEnum.HomeAccessories,"#C890F6" },
              {(int)ProductCategoryEnum.HomeTextile,"#9EABC9" },
               {(int)ProductCategoryEnum.Jewelry,"#7BD2BD" },
                {(int)ProductCategoryEnum.LeatherHides,"#4A90E2" },
                {(int)ProductCategoryEnum.Shoes,"#AB8ABD" },
                {(int)ProductCategoryEnum.TextileAccessories,"#6BD0E9" },
                 {(int)ProductCategoryEnum.Sports,"#8B0000" }
        };

        public readonly Dictionary<int, string> ProductCategoryImagePath = new Dictionary<int, string>()
        {
            { (int)ProductCategoryEnum.ElectronicAndElectrical, "graph-label-electric.svg" },
            { (int)ProductCategoryEnum.Furniture, "graph-label-furniter.svg" },
             {(int)ProductCategoryEnum.Toys,"graph-label-toy.svg" },
            {(int)ProductCategoryEnum.BabyCare,"graph-label-baby.svg" },
            {(int)ProductCategoryEnum.Tools,"graph-label-tool.svg" },
            {(int)ProductCategoryEnum.SportsFitnessandCamping,"graph-label-sport.svg" },
            {(int)ProductCategoryEnum.PersonalCare,"graph-label-health.svg" },
            {(int)ProductCategoryEnum.HomeProducts,"graph-label-home.svg" },
             {(int)ProductCategoryEnum.Stationery,"graph-label-utility.svg" },
             {(int)ProductCategoryEnum.LuggageAndBags,"graph-label-bag.svg" },
             {(int)ProductCategoryEnum.CarAccessories,"graph-label-car.svg" },
             {(int)ProductCategoryEnum.NFR,"graph-label-utility.svg" },
             {(int)ProductCategoryEnum.TextileandFootware,"graph-label-textile.svg" },
             {(int)ProductCategoryEnum.FUR,"graph-label-toy.svg" },
             {(int)ProductCategoryEnum.Garment,"graph-label-garments.svg" },
             {(int)ProductCategoryEnum.Fabric,"graph-label-fabric.svg" },
             {(int)ProductCategoryEnum.HomeAccessories,"graph-label-homeaccessories.svg" },
              {(int)ProductCategoryEnum.HomeTextile,"graph-label-hometextile.svg" },
               {(int)ProductCategoryEnum.Jewelry,"graph-label-jwellary.svg" },
                {(int)ProductCategoryEnum.LeatherHides,"graph-label-leatherhides.svg" },
                {(int)ProductCategoryEnum.Shoes,"graph-label-shoes.svg" },
                {(int)ProductCategoryEnum.TextileAccessories,"graph-label-textileaccessory.svg" },
                 {(int)ProductCategoryEnum.Sports,"graph-label-sport.svg" }
        };

        public readonly Dictionary<int, string> InspectionRejectDashboardColor = new Dictionary<int, string>()
        {
            {1, "#950d22" },
            { 2, "#ad0f27" },
            { 3, "#c7112d" },
            { 4,"#df1332"},
             { 5, "#f81538" },
            { 6,"#FF4500"},
            { 7, "#e24c00" },
            { 8,"#cd5700"},
            { 9,"#FF6700"},
            { 10,"#ffa500"},
        };

        public readonly Dictionary<int, string> BookingStatusCustomList = new Dictionary<int, string>()
        {
            {1,"Booking Received"},
            {2,"Confirmed"},
            {3,"Scheduling"},
            {4,"Report Sent"},
            {5,"Hold"},
            {6,"Cancelled"},
            {7,"in_progress"},
            {8,"done"},
            {9,"not_started"},
            {10,"In Progress"},
            {11,"Inspection"},
            {12,"will update soon"}
        };

        public readonly Dictionary<int, string> MonthData = new Dictionary<int, string>()
        {
            { 1, "Jan" },
            { 2, "Feb" },
            { 3, "Mar" },
            { 4, "Apr" },
            { 5, "May" },
            { 6, "Jun" },
            { 7, "Jul" },
            { 8, "Aug" },
            { 9, "Sep" },
            { 10, "Oct" },
            { 11, "Nov" },
            { 12, "Dec" }
        };

        public readonly Dictionary<int, string> DayData = new Dictionary<int, string>()
        {
            { 1, "Day-1" },
            { 2, "Day-2" },
            { 3, "Day-3" },
            { 4, "Day-4" },
            { 5, "Day-5" },
            { 6, "Day-6" },
            { 7, "Day-7" }
        };

        public readonly Dictionary<int, string> WeekData = new Dictionary<int, string>()
        {
            { 1, "Week-1" },
            { 2, "Week-2" },
            { 3, "Week-3" },
            { 4, "Week-4" },
            { 5, "Week-5" },
            { 6, "Week-6" },
            { 7, "Week-7" }
        };

        public enum AutoCompleteTextLimit
        {
            BookingPoLimit = 3
        }

        public readonly List<int> AuditedStatusList = new List<int>()
        {
            {(int)Entities.Enums.AuditStatus.Audited}
        };

        public readonly List<int> PreInvoiceAuditStatusList = new List<int>()
        {
            {(int)Entities.Enums.AuditStatus.Confirmed},
            {(int)Entities.Enums.AuditStatus.Scheduled},
            {(int)Entities.Enums.AuditStatus.Audited}
        };

        // Default booking status list to check the booking
        public readonly List<int> InspectedStatusList = new List<int>()
        {
            {(int)BookingStatus.Inspected},
            {(int)BookingStatus.Validated},
            {(int)BookingStatus.ReportSent}
        };


        public readonly List<int> InspectionStatusList= new List<int>()
        {
            {(int)BookingStatus.Received},
            {(int)BookingStatus.Confirmed},
            {(int)BookingStatus.Rescheduled},
            {(int)BookingStatus.Cancel},
            {(int)BookingStatus.Inspected},
            {(int)BookingStatus.Verified},
            {(int)BookingStatus.AllocateQC},
            {(int)BookingStatus.Hold},
            {(int)BookingStatus.ReportSent}
        };

        public readonly List<int> AuditStatusList = new List<int>()
        {
            {(int)AuditStatus.Received},
            {(int)AuditStatus.Confirmed},
            {(int)AuditStatus.Rescheduled},
            {(int)AuditStatus.Cancel},
            {(int)AuditStatus.Scheduled},
            {(int)AuditStatus.Audited}
        };

        public readonly List<int> KPICustomerMandayStatusList = new List<int>()
        {
            {(int)BookingStatus.Inspected},
            {(int)BookingStatus.ReportSent}
        };

        public readonly List<int> QcStatusList = new List<int>()
        {
            {(int)BookingStatus.Inspected},
            {(int)BookingStatus.Validated},
            {(int)BookingStatus.ReportSent},
            {(int)BookingStatus.AllocateQC}
        };
        //use in edit schedule
        public readonly List<int> ScheduleStatusList = new List<int>()
        {
            {(int)BookingStatus.Inspected},
            {(int)BookingStatus.Validated},
            {(int)BookingStatus.ReportSent},
            {(int)BookingStatus.AllocateQC},
            {(int)BookingStatus.Confirmed}
        };

        public readonly List<int> InValidBookingStatusList = new List<int>()
        {
            {(int)BookingStatus.Hold},
            {(int)BookingStatus.Cancel} ,
             {(int)BookingStatus.Received}
        };

        public readonly List<int> InvoicedStatusList = new List<int>()
        {
            {(int)InvoiceStatus.Approved},
            {(int)InvoiceStatus.Created},
            {(int)InvoiceStatus.Modified}
        };
        public readonly Dictionary<int, string> InspectionServiceTypeData = new Dictionary<int, string>()
        {
            {(int)InspectionServiceTypeEnum.Container,"Container-" }
        };

        //booking status for QC expense claim
        public readonly List<int> ExpenseBookingStatusList = new List<int>()
        {
            {(int)BookingStatus.Inspected},
            {(int)BookingStatus.Validated},
            {(int)BookingStatus.ReportSent},
            {(int)BookingStatus.AllocateQC}
        };

        public readonly Dictionary<int, string> MandayDashboardColorList = new Dictionary<int, string>()
        {
            {1, "#48A5C6"},
            {2, "#6AD6EA"},
            {3, "#8288DE"},
            {4, "#CA48DB"},
            {5, "#9D55E5"},
            {6, "#92D05D"},
            {7, "#63D570"},
            {8, "#D3C944"},
            {9, "#F5934F"},
            {10, "#D68B6B"},
            {11, "#5860D6"}
        };

        public readonly Dictionary<int, string> KPICustomStatus = new Dictionary<int, string>()
        {
            {1, "planned" }
        };

        public enum Month
        {
            Feb = 28 //to set the date if not leap year
        }

        public GradingData[] UtilizationDashboardGraphData =  {
            new GradingData {
            Title = "Very Low",
            color= "#ee1f25",
            LowScore= 0,
            HighScore= 30
        },

        new GradingData {
            Title = "Low",
            color= "#fdae19",
            LowScore= 30,
            HighScore= 50
        },

        new GradingData {
            Title = "Medium",
            color= "#b0d136",
            LowScore= 50,
            HighScore= 70
        },

        new GradingData {
            Title = "High",
            color= "#54b947",
            LowScore= 70,
            HighScore= 90
        },

        new GradingData {
            Title = "Very High",
            color= "#0f9747",
            LowScore= 90,
            HighScore= 100
        }
        };

        public readonly Dictionary<string, string> DayNames = new Dictionary<string, string>()
        {
            { "Today", "Today" },
            { "Yesterday", "Yesterday" },
            { "Tomorrow", "Tomorrow" }
        };

        public readonly Dictionary<int, string> InvoiceStatusColor = new Dictionary<int, string>()
        {
            {(int)InvoiceStatus.Created,"#5bc0de" },
            {(int)InvoiceStatus.Modified,"#428bca" },
            {(int)InvoiceStatus.Approved,"#28a745" },
            {(int)InvoiceStatus.Cancelled,"#dd4b39" }
        };
        public readonly Dictionary<int, string> InvoicePaymentStatusColor = new Dictionary<int, string>()
        {
            {(int)InvoicePaymentStatus.NotPaid,"insp-cancel" },
            {(int)InvoicePaymentStatus.HalfPaid,"insp-reschedule" },
            {(int)InvoicePaymentStatus.Paid,"insp-send" }
        };
        public readonly Dictionary<int, string> ExtraFeeStatusColor = new Dictionary<int, string>()
        {
            {(int)ExtraFeeStatus.Pending,"#5bc0de" },
            {(int)ExtraFeeStatus.Cancelled,"#dd4b39" },
            {(int)ExtraFeeStatus.Invoiced,"#28a745" },
        };

        public readonly Dictionary<int, string> TaskText = new Dictionary<int, string>()
        {
            {(int)TaskType.QuotationCustomerConfirmed, $"Quotation to be Validated - " }
        };

        public enum TaskPageType
        {
            Booking = 1,
            Quotation = 2
        };

        public readonly Dictionary<int, string> UserType = new Dictionary<int, string>()
        {
            {1, "InternalUser" },
            {2, "Customer" },
            {3, "Supplier" },
            {4, "Factory" }
        };

        public readonly Dictionary<int, string> MobileQuotationStatus = new Dictionary<int, string>()
        {
            {(int)QuotationStatus.SentToClient, "Pending" },
            {(int)QuotationStatus.CustomerValidated, "Approved" }
        };
        public string Day_DateFormat = "ddd";

        public readonly Dictionary<int, string> ServiceTypeDashboardColor = new Dictionary<int, string>()
        {
            {(int)InspectionServiceTypeEnum.FinalRandomInspection, "#00CD00" },
            {(int)InspectionServiceTypeEnum.FinalRandomReInspection, "#2BFF2B" },
            {(int)InspectionServiceTypeEnum.Container, "#00611C" },
            {(int)InspectionServiceTypeEnum.PickingOnly, "#7EFFA4" },
            {(int)InspectionServiceTypeEnum.InitialProductionCheck ,"#0b6623"},
            {(int)InspectionServiceTypeEnum.DuringProductionInspection ,"#9dc183"},
            {(int)InspectionServiceTypeEnum.OneHundredInspection ,"#3f704d"},
             {(int)InspectionServiceTypeEnum.GoldenSampleReview ,"#4F7942"},
             {(int)InspectionServiceTypeEnum.InProductionProcessAssessment ,"#29AB87"},
              {(int)InspectionServiceTypeEnum.ProductVerification ,"#8A9A5B"},
               {(int)InspectionServiceTypeEnum.SampleCheck ,"#98FB98"},
                {(int)InspectionServiceTypeEnum.OnsiteCriticalTest ,"#01796F"},
                {(int)InspectionServiceTypeEnum.SelfInspectionTrainingClassroom ,"#D0F0C0"},
                 {(int)InspectionServiceTypeEnum.ProductionStatusCheck ,"#00A572"},
                  {(int)InspectionServiceTypeEnum.ControlPlanChecking ,"#50c878"},
                  {(int)InspectionServiceTypeEnum.DPIRE ,"#4Cbb17"},
                   {(int)InspectionServiceTypeEnum.OnSiteQualityEngineer ,"#39ff14"},
                   {(int)InspectionServiceTypeEnum.CSRTraining ,"#043927"},
                   {(int)InspectionServiceTypeEnum.SelfInspectionTrainingOnsite ,"#679267"},
                   {(int)InspectionServiceTypeEnum.DesktopReview ,"#2E8B57"},
                   {(int)InspectionServiceTypeEnum.OneHundredRE ,"#50c878"},
                   {(int)InspectionServiceTypeEnum.DedicatedTechnician ,"#3CB371"},
                    {(int)InspectionServiceTypeEnum.Qualityprocessreview ,"#2E8B57"},
                     {(int)InspectionServiceTypeEnum.RemoteInspectionmonitoring ,"#6B8E23"},
                      {(int)InspectionServiceTypeEnum.FactorySelfInspection ,"#b5e550"},
                       {(int)InspectionServiceTypeEnum.RemotePickingMonitoring ,"#607c3c"},

        };


        public readonly Dictionary<string, string> DefectAnalysisColorList = new Dictionary<string, string>()
        {
            {"Critical", "#ec4b82"},
            {"Major", "#EE82EE"},
            {"Minor", "#6673c3"},
            {"Total Reports", "#FFC75F"},
        };

        public readonly Dictionary<int, string> ParetoDefectColorList = new Dictionary<int, string>()
        {
            {1, "#E56DE5"},
            {2, "#8BC348"},
            {3, "#D3C944"},
            {4, "#577BF2"},
            {5, "#916CF0"}
        };

        public readonly Dictionary<int, string> DefectNameList = new Dictionary<int, string>()
        {
            {1, "Critical"},
            {2, "Major"},
            {3, "Minor"},
        };

        public readonly Dictionary<int, string> DefectPerCountryColorList = new Dictionary<int, string>()
        {
            {1, "#48A5C6"},
            {2, "#5860D6"},
            {3, "#52CE9A"},
            {4, "#EBCA12"},
            {5, "#64A4FC"},
            {6, "#CA48DB"},
            {7, "#8BC348"},
            {8, "#E6B36C"},
            {9, "#EAAD4C"},
            {10, "#63D570"},
            {11, "#D68B6B"}
        };

        public readonly Dictionary<int, string> BookingQuantityDashboardColor = new Dictionary<int, string>()
        {
            {(int)QuantityType.Ordered, "#64A4FC" },
            {(int)QuantityType.Presented, "#6AD6EA" },
            {(int)QuantityType.Inspected, "#577BF2" }
        };

        public readonly Dictionary<int, string> BookingQuantityTypeDashboard = new Dictionary<int, string>()
        {
            {(int)QuantityType.Ordered,  QuantityType.Ordered.ToString()},
            {(int)QuantityType.Presented, QuantityType.Presented.ToString()},
            {(int)QuantityType.Inspected, QuantityType.Inspected.ToString() }
        };

        public readonly Dictionary<int, string> CommonDashboardColor = new Dictionary<int, string>()
        {
            { 1, "#48A5C6" },
            {2 , "#3AC7D3" },
             {3,"#64A4FC" },
            {4,"#5991DD" },
            {5,"#6AD6EA" },
            {6,"#62C3D5" },
            {7,"#5860D6" },
            {8,"#577BF2" },
             {9,"#8288DE" },
             {10,"#DD5BA4" },
             {11,"#E56DE5" },
             {12,"#CA48DB" },
             {13,"#916CF0" },
             {14,"#9D55E5" },
             {15,"#52CE9A" },
             {16,"#63D570" },
             {17,"#92D05D" },
             {18,"#8BC348" },
             {19,"#74C765" },
             {20,"#B5D649" },
             {21,"#EBCA12" },
             {22,"#D3C944" },
             {23,"#EAAD4C" },
             {24,"#F5934F" },
             {25,"#D68B6B" },
             {26,"#E6B36C" },
             {27,"#FFC500" },
             {28,"#00A7FF" },
             {29,"#F29A0C" },
        };

        public readonly Dictionary<int, string> EmailSendReportResultColor = new Dictionary<int, string>()
        {
            {(int)FBReportResult.Pass, "#24C11E" },
            {(int)FBReportResult.Fail, "#F81539" },
            {(int)FBReportResult.Pending, "#F29A0C" },
            {(int)FBReportResult.Missing,"#9eb9ce" }
        };

        public readonly Dictionary<int, string> CustomerDecisionResultColor = new Dictionary<int, string>()
        {
            {(int)InspCustomerDecisionEnum.Pass, "#24C11E" },
            {(int)InspCustomerDecisionEnum.Fail, "#F81539" },
            {(int)InspCustomerDecisionEnum.Pending, "#F29A0C" },
            {(int)InspCustomerDecisionEnum.Derogated,"#8296af" }
        };

        public readonly Dictionary<string, string> KpiDbRequest = new Dictionary<string, string>()
        {
            {"CustomerId","@CustomerId" },
            {"FromDate", "@ServiceDateFrom" },
            {"ToDate", "@ServiceDateTo" },
            {"OfficeIdList", "@OfficeList" },
            {"ServiceTypeIdList", "@ServiceTypeList" },
            {"TemplateId", "@TemplateId" },
            {"InvoiceNo", "@InvoiceNo" }
        };

        public readonly List<int> EmailBookingFileds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 23,
                                                                      26,27,28,29,30,31,32,33,34,35,36,37,38,39,48,49,50,51,52,53,54,55,56,57,58,59
                                                         ,60,61,62,63,64,65,66,67,68,69,70,71,72,73,76,78,79,80,81,82,83,84,85,86,87,88};

        public readonly List<int> EmailProductFileds = new List<int> { 16, 17, 18, 19, 20, 40, 41, 42, 74, 75 };

        public readonly List<int> EmailReportFileds = new List<int> { 21, 43, 77 };

        public readonly List<int> QuotationPendingBookingStatus = new List<int> { (int)BookingStatus.AllocateQC, (int)BookingStatus.Verified, (int)BookingStatus.Confirmed, (int)BookingStatus.Rescheduled, (int)BookingStatus.Inspected, (int)BookingStatus.ReportSent };

        private readonly Regex sWhitespace = new Regex(@"\s+");
        public string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }
        //split the string with space with given charlimit
        public String SplitStringWithSpace(String value, int charLimit)
        {
            String splittedString = String.Empty;
            var length = value.Length;
            int splitLimit = length / charLimit;
            int startLimit = 0;
            int endLimit = charLimit;

            //if character length less than value length then return value
            if (length < charLimit)
                return value;

            for (int index = 0; index <= splitLimit; index++)
            {
                splittedString = (((index + 1) == 1) ? splittedString : splittedString + " ") + value.Substring(startLimit, endLimit);
                startLimit = (index + 1) * charLimit;
                endLimit = (((index + 1) * charLimit) + charLimit) > value.Length ? (value.Length - startLimit) : charLimit;
            }
            return splittedString;
        }

        //customer list 

        public enum CustomerList
        {
            Cultura = 62
        }

        public enum PriceCalculationType
        {
            Normal = 1,
            MinFee = 2,
            MaxFee = 3,
            SpecialPrice = 4
        }

        public enum InvoiceQuantityType
        {
            OrderQuantity = 1,
            InspectedQuantity = 2,
            PresentedQuantity = 3
        }

        public readonly Dictionary<int, string> ClaimStatusColor = new Dictionary<int, string>()
        {
             {(int)ClaimStatus.Registered,"#00A5CC" },
            {(int)ClaimStatus.Analyzed,"#0085CC" },
             {(int)ClaimStatus.Validated,"#997600" },
            {(int)ClaimStatus.Closed,"#167412" },
            {(int)ClaimStatus.Cancelled,"#F81538" }
        };

        public readonly Dictionary<int, string> ClaimSummaryStatusColor = new Dictionary<int, string>()
        {
             {(int)ClaimStatus.Registered,"insp-pending" },
            {(int)ClaimStatus.Analyzed,"insp-confirm" },
            {(int)ClaimStatus.Cancelled,"insp-cancel" },
             {(int)ClaimStatus.Validated,"insp-allocate" },
            {(int)ClaimStatus.Closed,"insp-send" }
        };

        public readonly Dictionary<int, string> CompanyList = new Dictionary<int, string>()
        {
            {1,"API" },
            {2,"SGT" },
            {3,"AQF" }
        };

        public class CustomerReportDetailsStatusCodes
        {
            public const string BadRequest = "400";
            public const string InternalServerError = "500";
            public const string OK = "200";
        }

        public class CustomerReportDetailsErrorMessages
        {
            public const string IsRequired = "{0} is required";
            public const string InvalidDateFormat = "Invalid {0} Date Format";
            public const string InvalidDate = "Invalid {0} Date";
            public const string InvalidDateRange = "From date is not greater than to date";
            public const string InternalServerError = "Internal server error";
        }

        public class InvoiceDownloadErrorMessages
        {
            public const string InvoiceNoRequired = "Invoice no is required";
            public const string InvoiceNotFound = "Invoice not found";
            public const string InvoiceFileNotFound = "Invoice file not found";
        }

        public List<string> OrderStatusLogRemovedColumnList = new List<string>() {
        "PONumber", "BuyerName", "EciOffice", "Bdm", "Merchandise", "Merchandise2", "QcmName", "SupplierCode", "FactoryName", "FactoryCode",
"FactoryState", "FactoryAddress", "InspectionStartDate","InspectionEndDate","ReportDate","ProductDescription","ProductSubCategory2",
"ShipmentQty","PartialShiptment","SampleSize","IsCombined","WMCode","InspectionFeePerUnit","ManDay","InspectionFee","WMDFee","Quotationcomment",
"TravellingCost","TotalInspectionFee","TotalReports","PaidBy","BookingRemarks","ConfirmDate","ReConfirmDate","IsPicking","FactoryRef",
"TMDFee", "NoOfTMD", "HotelFee", "TravelTime","ReportNo","ReportResult","ReportRemarks","FinalReportStatus","Barcode","Etd","InspectionReportDate",
"TotalReportPass","TotalReportFail","TotalReportPending","TotalReportMissing","ReportPassPercentage","Month","Year","MonthName","SupContactDate",
"SupContactDeadlineDate","ReinspectionId","TotalQty","CartonQty","FactoryContact","CustomerContact","SupContact","Email",
"Phone","ProductCount","QcCount","CombineId","CombineProductQty","bookingDFList","AQLLevelName","InspectionName","InvoiceSentDate","QuotationDate",
"CurrencyName","QuotationNumber","GifiOfficeName","GifiQAContactName","ColorTargetAvailable","ColorCheckFinding","GoldenSampleAvailable","GoldenSampleFinding",
"FBRemarkResult","FBRemarkNumber","CriticalMax","MajorMax","MinorMax","CriticalDefect","MajorDefect","MinorDefect","CriticalResult","MajorResult",
"MinorResult","CollectionName","PriceCategory","DefectDesc","DefectCategory","FbResult","SerialNo","RemarkCategory","RemarkSubCategory","RemarkSubCategory2",
"CustomerRemarkCodeReference","ProductUnitName","FbReportComments","BatteryType","BatteryModel","BatteryQuantity","BatteryNetWeight","PieceNo","MaterialGroup",
"MaterialCode","PackingLocation","PackingQuantity","PackingNetWeight","PercentageVolume","PercentageWeight","PCB","SpecClientValuesLength","SpecClientValuesWidth",
"SpecClientValuesHeight","SpecClientValuesWeight","SpecClientVolume","MeasuredValuesLength","MeasuredValuesWidth","MeasuredValuesHeight","MeasuredValuesWeight","MeasuredVolume",
"OtherRemarks","AQLQuantity","RescheduleReason","FirstServiceDateFrom","FirstServiceDateTo","InspMonthName","InspMonthNumber","OtherFee",
"DestinationCountry","DeptDivision","OfficeLocationName","ReportId","QuotationId","ProrateBookingNo","ResultList","InvoiceRemarks",
"CusDecisionName","CusDecisionDate","BookingFormSerial","ExtraFee","Invoice_OtherFee","Invoice_Discount","Invoice_TotalFee","Invoice_ExtraFee","ServiceDate",
"PoQuantity","TotalDefects","TotalQtyReworked","TotalQtyReplaced","TotalQtyRejected","ProductId","PickingProductName","PickingProductCategory","PickingProductSubCategory",
"PickingPoNumber","PickingCustomerName","PickingSupplierName","PickingFactoryName","PickingInspectionId","PickingServiceDate","PickingLabName","PickingQuantity","NewProduct",
"MajorDefects","MinorDefects","CriticalDefects","CustomerResult","ShipmentDate","CustomerDecision","CustomerDecisionComments"};


        public LinkErrorResponse BuildCommonLinkErrorResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new LinkErrorResponse()
            {
                Errors = errors,
                StatusCode = statusCode,
                Message = message
            };
        }

        public LinkSaveSuccessResponse BuildCommonLinkSuccessResponse(HttpStatusCode statusCode, string message, int id)
        {
            return new LinkSaveSuccessResponse()
            {
                StatusCode = statusCode,
                Message = message,
                Id = id
            };
        }

        public enum CustomerContactCredentialsFrom
        {
            ContactPage = 1,
            EAQF = 2
        }

        public enum CancelReasonType
        {
            RequestByEaqf = 13
        }

        public enum RescheduleReasonType
        {
            RequestByEaqf = 11
        }

        public enum CancelTimeType
        {
            LessThan24 = 1,
            TwentFourHoursTo48Hours = 2,
            After48Hours = 1,

        }

        public readonly Dictionary<int, string> AuditProductCategoryList = new Dictionary<int, string>()
        {
            {(int)TemplateProductCategory.CutNSewn,"Cut & Sewn" },
            {(int)TemplateProductCategory.Sweater,"Sweater" },
            {(int)TemplateProductCategory.Footwear, "Footwear" },
            {(int)TemplateProductCategory.AccessoryHardlines, "Accessory Hardline" }

        };

        public enum TemplateProductCategory
        {
            CutNSewn = 44,
            Sweater = 45,
            Footwear = 46,
            AccessoryHardlines = 47

        }

        public readonly Dictionary<int, string> KpiARFollowUpReportConstantList = new Dictionary<int, string>()
        {
            {(int)KpiARFollowUpReportConstant.PreInvoice,"Pre Invoice" },
            {(int)KpiARFollowUpReportConstant.Penalty,"Penalty" },
            {(int)KpiARFollowUpReportConstant.DSONotDue, "Not Due" },
            {(int)KpiARFollowUpReportConstant.DSO30, "30" },
            {(int)KpiARFollowUpReportConstant.DSO60, "60" },
            {(int)KpiARFollowUpReportConstant.DSO180, "180" },
            {(int)KpiARFollowUpReportConstant.DSOOLDER, "Older" },
            {(int)KpiARFollowUpReportConstant.Inspection, "Inspection" }

        };

        public enum KpiARFollowUpReportConstant
        {
            PreInvoice = 1,
            Penalty = 2,
            DSONotDue = 3,
            DSO30 = 30,
            DSO60 = 60,
            DSO180 = 180,
            DSOOLDER = 181,
            Inspection = 4

        }
    }
}

