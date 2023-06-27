using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_DA_InvoiceType")]
    public partial class InvDaInvoiceType
    {
        public int Id { get; set; }
        public int InvDaId { get; set; }
        public int InvoiceTypeId { get; set; }
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
        [InverseProperty("InvDaInvoiceTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvDaInvoiceTypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InvDaId")]
        [InverseProperty("InvDaInvoiceTypes")]
        public virtual InvDaTransaction InvDa { get; set; }
        [ForeignKey("InvoiceTypeId")]
        [InverseProperty("InvDaInvoiceTypes")]
        public virtual RefInvoiceType InvoiceType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvDaInvoiceTypeUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}