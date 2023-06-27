using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Schedule
{
    public class ScheduleQCEmail
    {
        public DateTime ServiceDate { get; set; }
        public int BookingId { get; set; }
        public int? MissionId { get; set; }
        public string MisssionUrl { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string AEName { get; set; }
        public int FactoryID { get; set; }
        public string FactoryName { get; set; }
        public string FactoryRegionalName { get; set; }
        public string FactoryPhNo { get; set; }
        public string FactoryAddressEnglish { get; set; }
        public string FactoryAddressRegional { get; set; }
        public string FactoryContactName { get; set; }
        public string FactoryContactPhone { get; set; }
        public string SupplierContactName { get; set; }
        public string SupplierContactPhone { get; set; }
        public string FactoryContactEmail { get; set; }
        public string FactoryProvince { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryCounty { get; set; }
        public string FactoryTown { get; set; }
        public string ProductCategory { get; set; }
        public string ServiceType { get; set; }
        public string CountItems { get; set; }
        public string ScheduleComments { get; set; }
        public int QCId { get; set; }
        public string QCName { get; set; }
        public string QCEmail { get; set; }
        public string QCNames { get; set; }
        public string CsNames { get; set; }
        public List<string> QCEmails { get; set; }
        public int? SampleSize { get; set; }
        public int TotalNumberofProducts { get; set; }
        public int TotalNumberofReports { get; set; }
        public string ReportLink { get; set; }
        public string BookingOfficeLocation { get; set; }
        public List<string> FactoryContactEmails { get; set; }
        public List<InsectionPOProductData> ProductList { get; set; }
        public string QCBookingComments { get; set; }
        public string ProductCode { get; set; }
        public string PONumber { get; set; }
        public string Color { get; set; }
        public int? OrderQty { get; set; }
        public string ProductSubCategory2 { get; set; }
        public string Unit { get; set; }
        public bool IsChinaCountry { get; set; }
        public string Service { get; set; }
        public int ServiceId { get; set; }
        public string ReportTitle { get; set; }
        public int? BusinessLine { get; set; }
    }

    public class InsectionPOProductData
    {
        public int? ProductID { get; set; }
        public int? AqlID { get; set; }
        public int Critical { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int OrderQuantity { get; set; }
        public int? SampleSize { get; set; }
    }

    public class QCInspectionData
    {
        public int QcID { get; set; }
        public int InspectionID { get; set; }
        public string QcEmail { get; set; }
    }

    public class ScheduleQCEntityData
    {
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public int BookingId { get; set; }
        public int? MisssionId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string SupplierName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string TownName { get; set; }
        public string ScheduleComments { get; set; }
        public string ProductCategory { get; set; }
        public string QCBookingcomments { get; set; }
        public int? BusinessLine { get; set; }

        public string FactoryName { get; set; }
        public string FactoryRegionalName { get; set; }
        public string FactoryPhNo { get; set; }
        public string FactoryAddressRegional { get; set; }
        public string FactoryContactEmail { get; set; }
        public string BookingOfficeLocation { get; set; }
        public string ReportTitle { get; set; }
    }

    public class ScheduleQCEmailTemplate
    {
        public int QCId { get; set; }
        public string QCName { get; set; }
        public string QCEmailID { get; set; }
        public string CurrentUserEmailID { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public int TotalNumberofDays { get; set; }
        public bool IsFromScheduler { get; set; }
        public List<ScheduleQcServiceDate> ScheduleQcServiceDateList { get; set; }
        
        public bool IsChinaCountry { get; set; }
    }

    public class ScheduleQcServiceDate
    {
        public DateTime ServiceDate { get; set; }
        public bool IsShowSoftLineItems { get; set; }
        public List<ScheduleQCEmail> ScheduleQCEmailDetail { get; set; }
        public List<BookingAttachmentData> BookingAttachments { get; set; }
    }

    public class BookingAttachmentData
    {
        public int BookingId { get; set; }
        public DateTime ServiceDate { get; set; }
        public IEnumerable<BookingFileAttachment> Attachments { get; set; }
        public string Service { get; set; }
    }
}
