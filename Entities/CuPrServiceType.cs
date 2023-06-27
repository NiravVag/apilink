using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_ServiceType")]
    public partial class CuPrServiceType
    {
        public int Id { get; set; }
        [Column("CU_PR_Id")]
        public int CuPrId { get; set; }
        public int ServiceTypeId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrServiceTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPrId")]
        [InverseProperty("CuPrServiceTypes")]
        public virtual CuPrDetail CuPr { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrServiceTypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("CuPrServiceTypes")]
        public virtual RefServiceType ServiceType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrServiceTypeUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}