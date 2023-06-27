using DTO.Common;
using DTO.Customer;
using DTO.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LocLanguageName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Mobile { get; set; }

        public string WebSite { get; set; }

        public int? TypeId { get; set; }

        public int? LevelId { get; set; }

        public string DailyProduction { get; set; }

        public int? OwnerId { get; set; }

        public string TotalStaff { get; set; }

        public string LegalName { get; set; }

        public string GlCode { get; set; }

        public string ContactPersonName { get; set; }

        public string Comment { get; set; }

        public bool IsNewSupplier { get; set; }

        public int? CreditTerm { get; set; }

        public int Status { get; set; }

        public string VatNo { get; set; }

        public IEnumerable<Address> AddressList { get; set; }

        public IEnumerable<suppliercontact> SupplierContactList { get; set; }

        public IEnumerable<int?> ApiServiceIds { get; set; }

        public IEnumerable<SupplierMappedCustomer> CustomerList { get; set; }

        public IEnumerable<SupplierItem> SupplierParentList { get; set; }

        public bool IsFromBookingPage { get; set; }
        [Required]

        public List<int> SupplierEntityIds { get; set; }
        public bool MapAllSupplierContacts { get; set; }
        public int? CompanyId { get; set; }
        public IEnumerable<SupplierGrade> GradeList { get; set; }
        public bool? IsEAQF { get; set; }
        public int? UserId { get; set; }
    }

    public class SupplierMappedCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? CreditTerm { get; set; }
        public bool IsStatisticsVisibility { get; set; }
    }

    public class SupplierGradeRepo
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string Level { get; set; }
        public string CustomName { get; set; }
        public int LevelId { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
    }
    public class SupplierGrade
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Level { get; set; }
        public int LevelId { get; set; }
        public DateObject PeriodFrom { get; set; }
        public DateObject PeriodTo { get; set; }
    }

    public class SuLevelCustomDto
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string CustomName { get; set; }
    }

    public class SupplierGradeRequest
    {
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public List<int> BookingIds { get; set; }
    }
}
