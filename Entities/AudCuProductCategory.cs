using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_CU_ProductCategory")]
    public partial class AudCuProductCategory
    {
        public AudCuProductCategory()
        {
            AudTransactions = new HashSet<AudTransaction>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public int? ServiceType { get; set; }
        public int? CustomerId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }
        [Column("FB_Name")]
        [StringLength(200)]
        public string FbName { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("AudCuProductCategoryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("AudCuProductCategories")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudCuProductCategoryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("AudCuProductCategories")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceType")]
        [InverseProperty("AudCuProductCategories")]
        public virtual RefServiceType ServiceTypeNavigation { get; set; }
        [InverseProperty("CuProductCategoryNavigation")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
    }
}