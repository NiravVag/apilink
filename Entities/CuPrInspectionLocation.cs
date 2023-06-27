using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_InspectionLocation")]
    public partial class CuPrInspectionLocation
    {
        public int Id { get; set; }
        [Column("Cu_Price_Id")]
        public int? CuPriceId { get; set; }
        public int? InspectionLocationId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrInspectionLocationCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceId")]
        [InverseProperty("CuPrInspectionLocations")]
        public virtual CuPrDetail CuPrice { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrInspectionLocationDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionLocationId")]
        [InverseProperty("CuPrInspectionLocations")]
        public virtual InspRefInspectionLocation InspectionLocation { get; set; }
    }
}