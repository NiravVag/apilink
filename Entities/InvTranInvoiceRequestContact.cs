using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_TRAN_Invoice_Request_Contact")]
    public partial class InvTranInvoiceRequestContact
    {
        public int Id { get; set; }
        public int? CuPriceCardId { get; set; }
        public int? InvoiceRequestId { get; set; }
        public int? ContactId { get; set; }
        public bool? IsCommon { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("InvTranInvoiceRequestContacts")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvTranInvoiceRequestContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceCardId")]
        [InverseProperty("InvTranInvoiceRequestContacts")]
        public virtual CuPrDetail CuPriceCard { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvTranInvoiceRequestContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvTranInvoiceRequestContacts")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InvoiceRequestId")]
        [InverseProperty("InvTranInvoiceRequestContacts")]
        public virtual InvTranInvoiceRequest InvoiceRequest { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvTranInvoiceRequestContactUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}