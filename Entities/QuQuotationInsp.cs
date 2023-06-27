using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_Insp")]
    public partial class QuQuotationInsp
    {
        public int IdQuotation { get; set; }
        public int IdBooking { get; set; }
        public double? UnitPrice { get; set; }
        public double? NoOfManDay { get; set; }
        public double? InspFees { get; set; }
        public double? TravelLand { get; set; }
        public double? TravelAir { get; set; }
        public double? TravelHotel { get; set; }
        public double? TotalCost { get; set; }
        [StringLength(1000)]
        public string InvoiceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvoiceDate { get; set; }
        [StringLength(2000)]
        public string InvoiceRemarks { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public double? NoOfTravelManDay { get; set; }
        public double? TravelDistance { get; set; }
        public double? TravelTime { get; set; }
        public double? CalculatedWorkingHrs { get; set; }
        public double? CalculatedWorkingManDay { get; set; }
        public int? Quantity { get; set; }
        public int? BilledQtyType { get; set; }
        public double? SuggestedManday { get; set; }

        [ForeignKey("BilledQtyType")]
        [InverseProperty("QuQuotationInsps")]
        public virtual InspRefQuantityType BilledQtyTypeNavigation { get; set; }
        [ForeignKey("IdBooking")]
        [InverseProperty("QuQuotationInsps")]
        public virtual InspTransaction IdBookingNavigation { get; set; }
        [ForeignKey("IdQuotation")]
        [InverseProperty("QuQuotationInsps")]
        public virtual QuQuotation IdQuotationNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("QuQuotationInsps")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}