using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_DIS_TRAN_Details")]
    public partial class InvDisTranDetail
    {
        public InvDisTranDetail()
        {
            InvDisTranCountries = new HashSet<InvDisTranCountry>();
            InvDisTranPeriodInfos = new HashSet<InvDisTranPeriodInfo>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DiscountType { get; set; }
        public bool? SelectAllCountry { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PeriodFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PeriodTo { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public bool? Active { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvDisTranDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InvDisTranDetails")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvDisTranDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DiscountType")]
        [InverseProperty("InvDisTranDetails")]
        public virtual InvDisRefType DiscountTypeNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvDisTranDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvDisTranDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Discount")]
        public virtual ICollection<InvDisTranCountry> InvDisTranCountries { get; set; }
        [InverseProperty("Discount")]
        public virtual ICollection<InvDisTranPeriodInfo> InvDisTranPeriodInfos { get; set; }
    }
}