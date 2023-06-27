using DTO.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Supplier
{
    public class EaqfSupplierDetails
    {
        public int Id { get; set; }
               
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        //[RegularExpression(@"[A-Za-z0-9.+]{2,}@[a-zA-Z]{2,}.[A-Za-z]{2,}$", ErrorMessage = "Supplier email is invalid")]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
        public string Email { get; set; }
        public List<EaqfSupplierContact> eaqfSupplierContacts { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SUP_COUNTRY_REQ")]
        [StringLength(2)]
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }

    public class EaqfSupplierContact
    {
        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SUP_CONTACT_REQ")]
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
    }
}

