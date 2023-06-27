using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_AUT_TRAN_Communications")]
    public partial class InvAutTranCommunication
    {
        public int Id { get; set; }
        [Column("Invoice_Number")]
        [StringLength(1000)]
        public string InvoiceNumber { get; set; }
        [StringLength(2000)]
        public string Comment { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvAutTranCommunications")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvAutTranCommunications")]
        public virtual ApEntity Entity { get; set; }
    }
}