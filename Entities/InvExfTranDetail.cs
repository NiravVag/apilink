using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_EXF_TRAN_Details")]
    public partial class InvExfTranDetail
    {
        public int Id { get; set; }
        [Column("EXFTransactionId")]
        public int? ExftransactionId { get; set; }
        public int? ExtraFeeType { get; set; }
        public double? ExtraFees { get; set; }
        public string Remarks { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvExfTranDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvExfTranDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ExftransactionId")]
        [InverseProperty("InvExfTranDetails")]
        public virtual InvExfTransaction Exftransaction { get; set; }
        [ForeignKey("ExtraFeeType")]
        [InverseProperty("InvExfTranDetails")]
        public virtual InvExfType ExtraFeeTypeNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvExfTranDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}