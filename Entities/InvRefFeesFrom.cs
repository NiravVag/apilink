using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_Fees_From")]
    public partial class InvRefFeesFrom
    {
        public InvRefFeesFrom()
        {
            CuPrDetailInvoiceDiscountFeeFromNavigations = new HashSet<CuPrDetail>();
            CuPrDetailInvoiceHotelFeeFromNavigations = new HashSet<CuPrDetail>();
            CuPrDetailInvoiceInspFeeFromNavigations = new HashSet<CuPrDetail>();
            CuPrDetailInvoiceOtherFeeFromNavigations = new HashSet<CuPrDetail>();
            CuPrDetailInvoiceTmfeeFromNavigations = new HashSet<CuPrDetail>();
            InvAutTranDetailCalculateDiscountFeeNavigations = new HashSet<InvAutTranDetail>();
            InvAutTranDetailCalculateHotelFeeNavigations = new HashSet<InvAutTranDetail>();
            InvAutTranDetailCalculateInspectionFeeNavigations = new HashSet<InvAutTranDetail>();
            InvAutTranDetailCalculateOtherFeeNavigations = new HashSet<InvAutTranDetail>();
            InvAutTranDetailCalculateTravelExpenseNavigations = new HashSet<InvAutTranDetail>();
        }

        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("InvoiceDiscountFeeFromNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailInvoiceDiscountFeeFromNavigations { get; set; }
        [InverseProperty("InvoiceHotelFeeFromNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailInvoiceHotelFeeFromNavigations { get; set; }
        [InverseProperty("InvoiceInspFeeFromNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailInvoiceInspFeeFromNavigations { get; set; }
        [InverseProperty("InvoiceOtherFeeFromNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailInvoiceOtherFeeFromNavigations { get; set; }
        [InverseProperty("InvoiceTmfeeFromNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailInvoiceTmfeeFromNavigations { get; set; }
        [InverseProperty("CalculateDiscountFeeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetailCalculateDiscountFeeNavigations { get; set; }
        [InverseProperty("CalculateHotelFeeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetailCalculateHotelFeeNavigations { get; set; }
        [InverseProperty("CalculateInspectionFeeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetailCalculateInspectionFeeNavigations { get; set; }
        [InverseProperty("CalculateOtherFeeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetailCalculateOtherFeeNavigations { get; set; }
        [InverseProperty("CalculateTravelExpenseNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetailCalculateTravelExpenseNavigations { get; set; }
    }
}