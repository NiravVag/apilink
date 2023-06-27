using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Eaqf
{
    public class SaveQuotationEaqfRequest
    {
        [Required]
        public int BookingId { get; set; }
        [Required]
        public int Service { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PaymentMode { get; set; }
        [Required]
        public string PaymentRef { get; set; }
        public List<EAQFOrderDetails> OrderDetails { get; set; }
    }
    public class EAQFOrderDetails
    {        
        public string Description { get; set; }
        public double Amount { get; set; }
        public double Manday { get; set; }
        public string OrderType { get; set; }
    }
}
