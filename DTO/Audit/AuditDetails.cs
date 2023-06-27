using System;
using System.Collections.Generic;
using System.Text;
using DTO.References;
using DTO.Customer;
using DTO.Supplier;
using DTO.HumanResource;
using DTO.Common;
namespace DTO.Audit
{
   public class AuditDetails
    {
        public int AuditId { get; set; }

        public string AuditBookingNo { get; set; }

        public int CustomerId { get; set; }

        public int? BrandId { get; set; }

        public int? DepartmentId { get; set; }

        public int ServiceTypeId { get; set; }

        public DateObject ServiceDateFrom { get; set; }

        public DateObject ServiceDateTo { get; set; }

        public int? SeasonId { get; set; }

        public int? SeasonYearId { get; set; }

        public int? EvaluationRoundId { get; set; }

        public string ApplicantName { get; set; }

        public string ApplicantEmail { get; set; }

        public string ApplicantPhNo { get; set; }

        public int SupplierId { get; set; }

        public int FactoryId { get; set; }

        public DateObject FactoryCreationDate { get; set; }

        public string NoOfCustomers { get; set; }

        public string NoOfSuppliers { get; set; }

        public string AnnualProduction { get; set; }

        public string MaximumCapacity { get; set; }

        public string FactorySurfaceArea { get; set; }

        public string TotalCapacityByCustomer { get; set; }

        public string FactoryExtension { get; set; }

        public string ManufactureProducts { get; set; }

        public string BrandsProduced { get; set; }

        public string NumberOfSites { get; set; }

        public string OpenHour { get; set; }

        public string AnnualHolidays { get; set; }

        public int ProductionStaff { get; set; }
        
        public int AdminStaff { get; set; }

        public int QualityStaff { get; set; }

        public int TotalStaff { get; set; }

        public int SalesStaff { get; set; }

        public string Investment { get; set; }

        public string Liability { get; set; }

        public string TradeAssociation { get; set; }

        public string Accreditations { get; set; }

        public string CustomerComments { get; set; }

        public string APIComments { get; set; }

        public bool IsEmailRequired { get; set; }

        public string PoNumber { get; set; }

        public string ReportNo { get; set; }

        public int OfficeId { get; set; }

        public string InternalComments { get; set; }

        public IEnumerable<int> CustomerContactListItems { get; set; }

        public IEnumerable<int> Auditors { get; set; }

        public IEnumerable<int> AuditCS { get; set; }

        public IEnumerable<int> SupplierContactListItems { get; set; }

        public IEnumerable<int> FactoryContactListItems { get; set; }

        public int StatusId { get; set; }

        public IEnumerable<AuditAttachment> Attachments { get; set; }

        public int? CreatedbyUserType { get; set; }

        public int AuditTypeid { get; set; }

        public IEnumerable<int> AuditworkprocessItems { get; set; }

        public string CustomerBookingNo { get; set; }
        public int? AuditProductCategoryId { get; set; }
        public bool IsEaqf { get; set; }
        public bool IsSupplierOrFactoryEmailSend { get; set; }
        public bool IsCustomerEmailSend { get; set; }
        public int UserId { get; set; }
    }
}
