using DTO.Helper;
using System.ComponentModel.DataAnnotations;

namespace DTO.Customer
{
    public class SaveEaqfCustomerRequest
    {
        [Required]
        [StringLength(500)]
        public string CompanyName { get; set; }
        [Required]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateEaqfCustomerRequest
    {
        [Required]
        [StringLength(500)]
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
    }

    public class SaveEaqfCustomerContactRequest
    {
        [Required]
        [StringLength(1200)]
        public string FirstName { get; set; }

        [StringLength(500)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateEaqfCustomerContactRequest
    {
        [Required]
        [StringLength(1200)]
        public string FirstName { get; set; }
        [StringLength(500)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
    }

    public class SaveEaqfCustomerAddressRequest
    {
        [Required]
        public int AddressTypeId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [StringLength(2)]
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string BoxPost { get; set; }
    }

    public class UpdateEaqfCustomerAddressRequest
    {
        [Required]
        public int AddressTypeId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [StringLength(2)]
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string BoxPost { get; set; }
    }

    public class GetEaqfCustomerAddressData
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AddressTypeId { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}
