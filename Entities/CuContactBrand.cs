using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Contact_Brand")]
    public partial class CuContactBrand
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int BrandId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("CuContactBrands")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("ContactId")]
        [InverseProperty("CuContactBrands")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuContactBrandCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuContactBrandDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}