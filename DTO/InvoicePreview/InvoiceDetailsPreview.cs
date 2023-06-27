using DTO.Inspection;
using DTO.Schedule;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DTO.InvoicePreview
{
    public class InvoiceDetailsPreview
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceDateTime { get; set; }
        public string PostDate { get; set; }
        public string UnitPrice { get; set; }
        public string CustomerContacts { get; set; }
        public string Merchandiser { get; set; }
        public string InspFee { get; set; }
        public string AirCost { get; set; }
        public string LandCost { get; set; }
        public string HotelCost { get; set; }
        public string OtherCost { get; set; }
        public string Discount { get; set; }

        public string ManDay { get; set; }
        public string PaymentTerm { get; set; }
        public string PaymentDuration { get; set; }
        public string Subject { get; set; }
        public string Remarks { get; set; }
        public string Currency { get; set; }

        public string CustomerName { get; set; }
        public string CustomerDepartment { get; set; }
        public string CustomerBuyer { get; set; }
        public string CustomerCollection { get; set; }

        public string BookingNo { get; set; }
        public string CustomerBookingNo { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string FactoryName { get; set; }

        public string FactoryCountry { get; set; }
        public string FactoryProvince { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryCounty { get; set; }
        public string FactoryTown { get; set; }
        public string ServiceType { get; set; }

        public string ServiceDateFromToDate { get; set; }
        public string TotalbookingQty { get; set; }
        public string TotalInspectedQty { get; set; }

        public string ProductRef { get; set; }
        public string ProductDesc { get; set; }
        public string ProductBookingQty { get; set; }
        public string ProductPO { get; set; }
        public string BookingPO { get; set; }

        public string CustomerProductCategory { get; set; }

        public string BilledContacts { get; set; }
        public string BilledAddress { get; set; }
        public string BillingMethod { get; set; }
        public string BilledName { get; set; }

        public string QuotationNo { get; set; }

        public string Description { get; set; }
        public string DueDate { get; set; }
        public string CurrentDate { get; set; }


        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeFax { get; set; }
        public string OfficeWebsite { get; set; }
        public string OfficeMail { get; set; }

        public string BankId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankSwiftCode { get; set; }
        public string DestinationCountry { get; set; }
        public string ProductBarCode { get; set; }
        public string FactoryReference { get; set; }
        public string CustomerPriceCategory { get; set; }
        public string TravelTotalCost { get; set; }
        public string TravelHotelCost { get; set; }

        public string TotalInvoiceFees { get; set; }
        public string TaxValue { get; set; }
        public string TravelOtherFees { get; set; }
        public string CustomerBrand { get; set; }
        public string BankTaxId { get; set; }
        public string ETD { get; set; }

        public string ExtrafeeTotal { get; set; }

        public string ExtraFeeType { get; set; }
        public string ExtraFeeTypeRemarks { get; set; }
        public string ExtraFee { get; set; }
        public string ExtraFeeRemarks { get; set; }
        public string ExtraFeeTypeBooking { get; set; }
        public string ProductInspectedQty { get; set; }
        public string AQLQty { get; set; }

        public string ManualDescription { get; set; }
        public string ManualServiceFee { get; set; }
        public string ManualChargeBack { get; set; }
        public string ManualOtherCost { get; set; }
        public string ManualRemarks { get; set; }

        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public string Colors { get; set; }
        public string PresentedQty { get; set; }
        public string InvoiceManday { get; set; }
        public string InspectionLocation { get; set; }
        public string Inspectors { get; set; }
        public string Season { get; set; }
        public string Report { get; set; }
        public string TotalStaff { get; set; }
        public string ProductPresentedQty { get; set; }
        public string CreditRefundAmount { get; set; }
        public string CreditSortAmount { get; set; }
        public string CreditDate { get; set; }
        public string CreditNumber { get; set; }
        public string CreditRemarks { get; set; }
        public string ReportResult { get; set; }
        public string TaxDate { get; set; }
        public string Country { get; set; }
        public string Attention { get; set; }
        public string InvoiceCode { get; set; }
        public InvoiceDetailsPreview()
        {
            BankSwiftCode = string.Empty;
            InvoiceNumber = string.Empty;
            InvoiceDate = string.Empty;
            PostDate = string.Empty;
            UnitPrice = string.Empty;

            InspFee = string.Empty;
            AirCost = string.Empty;
            LandCost = string.Empty;
            HotelCost = string.Empty;
            OtherCost = string.Empty;
            Discount = string.Empty;

            ManDay = string.Empty;
            PaymentTerm = string.Empty;
            PaymentDuration = string.Empty;
            Subject = string.Empty;
            Remarks = string.Empty;
            Currency = string.Empty;

            CustomerName = string.Empty;
            CustomerDepartment = string.Empty;
            CustomerBuyer = string.Empty;
            CustomerCollection = string.Empty;

            BookingNo = string.Empty;
            CustomerBookingNo = string.Empty;
            SupplierName = string.Empty;
            FactoryName = string.Empty;

            FactoryCountry = string.Empty;
            FactoryProvince = string.Empty;
            FactoryCity = string.Empty;
            FactoryCounty = string.Empty;
            FactoryTown = string.Empty;
            ServiceType = string.Empty;

            ServiceDateFromToDate = string.Empty;
            TotalbookingQty = string.Empty;
            TotalInspectedQty = string.Empty;

            ProductRef = string.Empty;
            ProductDesc = string.Empty;
            ProductBookingQty = string.Empty;
            ProductPO = string.Empty;
            BookingPO = string.Empty;

            CustomerProductCategory = string.Empty;

            BilledContacts = string.Empty;
            BilledAddress = string.Empty;
            BillingMethod = string.Empty;
            BilledName = string.Empty;

            QuotationNo = string.Empty;

            Description = string.Empty;
            DueDate = string.Empty;
            CurrentDate = string.Empty;


            OfficeName = string.Empty;
            OfficeAddress = string.Empty;
            OfficePhone = string.Empty;
            OfficeFax = string.Empty;
            OfficeWebsite = string.Empty;
            OfficeMail = string.Empty;

            BankId = string.Empty;
            AccountName = string.Empty;
            AccountNumber = string.Empty;
            BankName = string.Empty;
            BankAddress = string.Empty;
            BankSwiftCode = string.Empty;
            DestinationCountry = string.Empty;
            ProductBarCode = string.Empty;
            FactoryReference = string.Empty;
            CustomerPriceCategory = string.Empty;
            TravelTotalCost = string.Empty;
            TravelHotelCost = string.Empty;

            TotalInvoiceFees = string.Empty;
            TaxValue = string.Empty;
            TravelOtherFees = string.Empty;
            CustomerBrand = string.Empty;
            BankTaxId = string.Empty;
            ETD = string.Empty;
            ExtrafeeTotal = string.Empty;
            ExtraFeeType = string.Empty;
            ExtraFeeTypeRemarks = string.Empty;
            ExtraFee = string.Empty;
            ExtraFeeRemarks = string.Empty;
            ExtraFeeTypeBooking = string.Empty;
            ProductInspectedQty = string.Empty;
            AQLQty = string.Empty;
            ProductSubCategory = string.Empty;
            Colors = string.Empty;
            ProductSubCategory2 = string.Empty;
            PresentedQty = string.Empty;
            ProductPresentedQty = string.Empty;
            InvoiceManday = string.Empty;
            InspectionLocation = string.Empty;
            Inspectors = string.Empty;
            Season = string.Empty;
            Report = string.Empty;
            TotalStaff = string.Empty;
            CustomerContacts = string.Empty;
            Merchandiser = string.Empty;

            CreditDate = string.Empty;
            CreditNumber = string.Empty;
            CreditRefundAmount = string.Empty;
            CreditRemarks = string.Empty;
            CreditSortAmount = string.Empty;

            ReportResult = string.Empty;

            Country = string.Empty;
            Attention = string.Empty;
        }
    }

    public class InvoiceTaxData
    {
        public int? InvoiceId { get; set; }
        public int? TaxId { get; set; }
    }
    public class InvoiceDetailsRepo
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string CreditNumber { get; set; }
        public DateTime? CreditDate { get; set; }
        public DateTime? PostDate { get; set; }
        public double? UnitPrice { get; set; }
        public double? InspFee { get; set; }
        public double? AirCost { get; set; }
        public double? LandCost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double? Discount { get; set; }
        public double? ManDay { get; set; }
        public string PaymentTerm { get; set; }
        public string PaymentDuration { get; set; }
        public int? PaymentDurations { get; set; }
        public string Subject { get; set; }
        public string Remarks { get; set; }
        public string Currency { get; set; }
        public int? InspectionId { get; set; }
        public int? AuditId { get; set; }
        public string BilledAddress { get; set; }
        public int? BilledMethod { get; set; }
        public string BilledName { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeFax { get; set; }
        public string OfficeWebsite { get; set; }
        public string OfficeMail { get; set; }

        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankSwiftCode { get; set; }
        public int? InvoiceTo { get; set; }
        public double? TaxValue { get; set; }
        public double? TotalTravelFee { get; set; }
        public double? TotalInvoiceFees { get; set; }
        public double? TravelOtherFees { get; set; }
        public List<int?> BankTaxId { get; set; }
        public int? ServiceId { get; set; }
        public string ManualDescription { get; set; }
        public double? ManualServiceFee { get; set; }
        public double? ManualChargeBack { get; set; }
        public double? ManualOtherCost { get; set; }
        public string ManualRemarks { get; set; }
        public string ServiceType { get; set; }
        public string Service { get; set; }
        public DateTime? ServiceFromDate { get; set; }
        public DateTime? ServiceToDate { get; set; }
        public decimal? CreditRefundAmount { get; set; }
        public decimal? CreditSortAmount { get; set; }
        public string CreditRemarks { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Country { get; set; }
        public string Attention { get; set; }
    }

    public class InvoiceBookingPDFDetail
    {
        public IEnumerable<string> CustomerBrand { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<string> CustomerDepartment { get; set; }
        public IEnumerable<string> CustomerBuyer { get; set; }
        public IEnumerable<string> CustomerContact { get; set; }
        public IEnumerable<string> Merchandiser { get; set; }
        public IEnumerable<InspectionPOColorTransaction> InspPurchaseOrderColors { get; set; }
        public IEnumerable<InvoicePreviewReportResult> ReportResults { get; set; }
        public IEnumerable<ScheduleStaffItem> ScheduleStaffItems { get; set; }
        public string CustomerDept { get; set; }
        public string Brand { get; set; }

        public string CustomerCollection { get; set; }
        public string CustomerPriceCategory { get; set; }

        public int BookingNo { get; set; }
        public int? FactoryId { get; set; }
        public string CustomerBookingNo { get; set; }
        public string SupplierName { get; set; }

        public string SupplierCode { get; set; }
        public string FactoryName { get; set; }

        public string FactoryCountry { get; set; }
        public string FactoryProvince { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryCounty { get; set; }
        public string FactoryTown { get; set; }
        public IEnumerable<string> ServiceType { get; set; }

        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }

        public IEnumerable<string> CustomerProductCategory { get; set; }

        public string TotalbookingQty { get; set; }
        public string TotalInspectedQty { get; set; }

        public string ProductRef { get; set; }
        public string ProductDesc { get; set; }
        public string ProductBookingQty { get; set; }
        public string ProductPO { get; set; }
        public IEnumerable<string> BookingPO { get; set; }
        public IEnumerable<string> DestinationCountry { get; set; }
        public int customerid { get; set; }
        public int supplierid { get; set; }
        public int? factoryid { get; set; }
        public string InspectionLocation { get; set; }
        public string Season { get; set; }
        public int? SeasonYear { get; set; }

    }

    public class BilledContacts
    {
        public int InvoiceId { get; set; }
        public int CustomerContactId { get; set; }
        public int SupplierContactId { get; set; }
        public int FactoryContactId { get; set; }
    }

    public class BilledContactsName
    {
        public int? InvoiceId { get; set; }
        public string CustomerContactName { get; set; }
        public string SupplierContactName { get; set; }
        public string FactoryContactName { get; set; }
    }

    public class SaveInvoicePdfUrl
    {
        public string FilePath { get; set; }
        public string InvoiceNo { get; set; }
        public int FileType { get; set; }
        public string UniqueId { get; set; }
        public int? CreatedBy { get; set; }
    }
    public class SaveInvoicePdfResponse
    {
        public SaveInvoicePdfResult Result { get; set; }
    }
    public enum SaveInvoicePdfResult
    {
        Success = 1,
        InvoiceNumberNotFound = 2,
        Error = 3,
        InvoiceNotFound = 4
    }

    public class InvoiceDownloadResponse
    {
        public byte[] Invoice { get; set; }
        public string FileName { get; set; }
        public InvoiceDownloadResult Result { get; set; }
        public string Error { get; set; }
    }
    public enum InvoiceDownloadResult
    {
        Success = 1,
        InvoiceNoRequired = 2,
        InvoiceNotFound = 3,
        InvoicePdfNotAvailable = 4
    }

    public class SaveEAQFInvoicePDFResponse
    {
        public string invoicePdfUrl { get; set; }
        public string invoiceNo { get; set; }

    }

    public class InvoiceCreditDetails
    {
        public int? BookingId { get; set; }
        public string CreditNumber { get; set; }
    }
}
