using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_ProductSubCategory")]
    public partial class MidEmailRecipientsProductSubCategory
    {
        public int Id { get; set; }
        public int EmailConfigId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("MidEmailRecipientsProductSubCategoryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsProductSubCategoryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailConfigId")]
        [InverseProperty("MidEmailRecipientsProductSubCategories")]
        public virtual MidEmailRecipientsConfiguration EmailConfig { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsProductSubCategoryModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("ProductSubCategoryId")]
        [InverseProperty("MidEmailRecipientsProductSubCategories")]
        public virtual RefProductCategorySub ProductSubCategory { get; set; }
    }
}