using DTO.Helper;
using DTO.HumanResource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Customer
{

    public class SaveCustomerContactDetails
    {
        public long? ZohoContactId { get; set; }

        [Required]
        public string Name { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        [Required]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Others { get; set; }
        public int Office { get; set; }
        public string Comments { get; set; }
        public bool? PromotionalEmail { get; set; }
        public List<int> ContactTypes { get; set; }
    }

    public class SaveZohoCrmCustomerContactDetails
    {
        public long? ZohoContactId { get; set; }
        [Required]
        public string Name { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        [Required]
        //[RegularExpression(@"[A-Za-z0-9.+]{2,}@[a-zA-Z]{2,}.[A-Za-z]{2,}$", ErrorMessage = "Invalid Email")]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Comments { get; set; }
        public bool? PromotionalEmail { get; set; }
        public string Company { get; set; }
    }

    public class CustomerContactDetails
    {
        public int Id { get; set; }
        public long? ZohoContactId { get; set; }
        public long? ZohoCustomerId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Others { get; set; }
        public int CustomerID { get; set; }
        public int? Office { get; set; }
        public IEnumerable<CustomerAddressData> CustomerAddressList { get; set; }
        public string Comments { get; set; }
        public List<ContactType> ContactTypeList { get; set; }
        public IEnumerable<int> ContactTypes { get; set; }
        public bool? PromotionalEmail { get; set; }

		public IEnumerable<int> ContactBrandList { get; set; }

		public IEnumerable<int> ContactDepartmentList { get; set; }

		public IEnumerable<int> ContactServiceList { get; set; }

        public IEnumerable<int> ApiEntityIds { get; set; }

        public IEnumerable<EntityService> EntityServiceIds { get; set; }

        public int PrimaryEntity { get; set; }
        public int? ReportTo { get; set; }
        public IEnumerable<int> ContactSisterCompanyIds { get; set; }
    }



    public class ContactType
    {
        public int id { get; set; }
        public string Type { get; set; }
    }

    public class CustomerAddressData
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }

    public class CustomerContactData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Others { get; set; }
        public int CustomerID { get; set; }
        public int? Office { get; set; }
        public string Comments { get; set; }
        public bool? PromotionalEmail { get; set; }
        public string ZohoCustomerId { get; set; }
        public string ZohoContactId { get; set; }
    }


}

