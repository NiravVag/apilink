using DTO.Common;
using DTO.Helper;
using Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Customer
{

    public class SaveCustomerDetails
    {
        public long ZohoCustomerId { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        public int? ComplexityLevel { get; set; }
        public int? Group { get; set; }
        public string StartDate { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(100)]
        public string Others { get; set; }
        public int? ProspectStatus { get; set; }
        public int? SkillsRequired { get; set; }
        public int? Kam { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        public int? Category { get; set; }
        public int? MargetSegment { get; set; }
        public IEnumerable<int> BusinessCountry { get; set; }
        public IEnumerable<string> SalesCountry { get; set; }
        [StringLength(100)]
        public string OtherPhone { get; set; }
        public int? Language { get; set; }
        public int? BusinessType { get; set; }
        public int? InvoiceType { get; set; }
        [StringLength(500)]
        public string QuatationName { get; set; }
        public bool? IcRequired { get; set; }
        [Required]
        [StringLength(500)]
        public string GlCode { get; set; }
        [StringLength(1000)]
        public string Comments { get; set; }
        [StringLength(3000)]
        public string BookingDefaultComments { get; set; }
        public int? FbCusId { get; set; }
        public IEnumerable<CustomerAddress> CustomerAddresses { get; set; } 
    }             

    public class SaveCustomerCrmRequest
    {
        public long ZohoCustomerId { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        //[RegularExpression(@"[A-Za-z0-9.+]{2,}@[a-zA-Z]{2,}.[A-Za-z]{2,}$", ErrorMessage = "Invalid Email")]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        public string StartDate { get; set; }

        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        public IEnumerable<string> SalesCountry { get; set; }  
        [Required]
        [StringLength(500)]
        public string GlCode { get; set; }
        [StringLength(1000)]
        public string Comments { get; set; }
        public string Company { get; set; }
        public IEnumerable<CrmCustomerAddress> CustomerAddresses { get; set; }
    }

    public class CustomerDetails
    {
        public long? ZohoCustomerId { get; set; }
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public string Code { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        public int? ComplexityLevel { get; set; }
        public int? Group { get; set; }
        public DateObject StartDate { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(100)]
        public string Others { get; set; }
        public int? ProspectStatus { get; set; }
        public int? SkillsRequired { get; set; }
        public IEnumerable<int?> Kam { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        public int? Category { get; set; }
        public int? MargetSegment { get; set; }
        public IEnumerable<int> BusinessCountry { get; set; }
        public IEnumerable<int?> ApiServiceIds { get; set; }
        public List<int> SalesCountry { get; set; }
        [StringLength(100)]
        public string OtherPhone { get; set; }
        public int? Language { get; set; }
        public int? BusinessType { get; set; }
        public int? InvoiceType { get; set; }
        [StringLength(500)]
        public string QuatationName { get; set; }
        public bool? IcRequired { get; set; }
        [StringLength(500)]
        public string GlCode { get; set; }
        // [StringLength(1000)]
        public string Comments { get; set; }
        // [StringLength(3000)]
        public string BookingDefaultComments { get; set; }
        public int? AccountingLeader { get; set; }
        public IEnumerable<int?> SalesIncharge { get; set; }
        public int? ActivitiesLevel { get; set; }
        public int? RelationshipStatus { get; set; }
        public IEnumerable<int?> BrandPriority { get; set; }
        public string DirectCompetitor { get; set; }
        public int? FbCusId { get; set; }
        public int? isCallFrom { get; set; }
        public IEnumerable<CustomerAddress> CustomerAddresses { get; set; }
        public IEnumerable<int> CustomerEntityIds { get; set; }
        public IEnumerable<int> MapCustomerContactEntityIds { get; set; }
        public int? CompanyId { get; set; }
        public IEnumerable<int> SisterCompanyIds { get; set; }
  
    }

    public class UserAccount
    {
        public bool client { get; set; }
        public bool factory { get; set; }
        public bool vendor { get; set; }
        public string title { get; set; }
        public string status { get; set; }

    }

    public class CustomerContactEntityData
    {
        public IEnumerable<CuContact> CustomerContacts { get; set; }
        public IEnumerable<CuContactEntityMap> CustomerContactEntityMapList { get; set; }
        public IEnumerable<CuContactEntityServiceMap> CustomerContactEntityServiceMapList { get; set; }
        public IEnumerable<ItUserMaster> CustomerContactUserList { get; set; }
        public IEnumerable<ItUserRole> CustomerContactUserRoleList { get; set; }
        
    }

}
