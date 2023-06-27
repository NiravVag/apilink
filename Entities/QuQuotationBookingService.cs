using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_Booking_Service")]
    public partial class QuQuotationBookingService
    {
        public int IdQuotation { get; set; }
        public int IdBooking { get; set; }
        public int IdService { get; set; }
        public int SampleQty { get; set; }
        [StringLength(600)]
        public string AqlLevelDesc { get; set; }

        [ForeignKey("IdQuotation")]
        [InverseProperty("QuQuotationBookingServices")]
        public virtual QuQuotation IdQuotationNavigation { get; set; }
        [ForeignKey("IdService")]
        [InverseProperty("QuQuotationBookingServices")]
        public virtual RefService IdServiceNavigation { get; set; }
    }
}