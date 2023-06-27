using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_TRAN_DefectFamily")]
    public partial class ClmTranDefectFamily
    {
        public int Id { get; set; }
        public int? ClaimId { get; set; }
        public int? DefectFamilyId { get; set; }
        public int? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("ClaimId")]
        [InverseProperty("ClmTranDefectFamilies")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTranDefectFamilyCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DefectFamilyId")]
        [InverseProperty("ClmTranDefectFamilies")]
        public virtual ClmRefDefectFamily DefectFamily { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTranDefectFamilyDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTranDefectFamilyUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}