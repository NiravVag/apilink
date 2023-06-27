using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_DA_office")]
    public partial class InvDaOffice
    {
        public int Id { get; set; }
        public int InvDaId { get; set; }
        public int OfficeId { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvDaOfficeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvDaOfficeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InvDaId")]
        [InverseProperty("InvDaOffices")]
        public virtual InvDaTransaction InvDa { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("InvDaOffices")]
        public virtual RefLocation Office { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvDaOfficeUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}