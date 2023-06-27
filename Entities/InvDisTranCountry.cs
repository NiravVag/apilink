using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_DIS_TRAN_Country")]
    public partial class InvDisTranCountry
    {
        public int Id { get; set; }
        public int DiscountId { get; set; }
        public int CountryId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public bool? Active { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("InvDisTranCountries")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvDisTranCountryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvDisTranCountryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DiscountId")]
        [InverseProperty("InvDisTranCountries")]
        public virtual InvDisTranDetail Discount { get; set; }
    }
}