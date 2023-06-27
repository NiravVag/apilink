using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("COMP_TRAN_PersonInCharge")]
    public partial class CompTranPersonInCharge
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public int PsersonInCharge { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool Active { get; set; }

        [ForeignKey("ComplaintId")]
        [InverseProperty("CompTranPersonInCharges")]
        public virtual CompComplaint Complaint { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CompTranPersonInChargeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CompTranPersonInChargeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("PsersonInCharge")]
        [InverseProperty("CompTranPersonInCharges")]
        public virtual HrStaff PsersonInChargeNavigation { get; set; }
    }
}