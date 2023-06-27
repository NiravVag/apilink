using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_AUT_TRAN_ContactDetails")]
    public partial class InvAutTranContactDetail
    {
        public int Id { get; set; }
        [Column("Invoice_Id")]
        public int? InvoiceId { get; set; }
        [Column("Customer_Contact_Id")]
        public int? CustomerContactId { get; set; }
        [Column("Supplier_Contact_Id")]
        public int? SupplierContactId { get; set; }
        [Column("Factory_Contact_Id")]
        public int? FactoryContactId { get; set; }

        [ForeignKey("CustomerContactId")]
        [InverseProperty("InvAutTranContactDetails")]
        public virtual CuContact CustomerContact { get; set; }
        [ForeignKey("FactoryContactId")]
        [InverseProperty("InvAutTranContactDetailFactoryContacts")]
        public virtual SuContact FactoryContact { get; set; }
        [ForeignKey("InvoiceId")]
        [InverseProperty("InvAutTranContactDetails")]
        public virtual InvAutTranDetail Invoice { get; set; }
        [ForeignKey("SupplierContactId")]
        [InverseProperty("InvAutTranContactDetailSupplierContacts")]
        public virtual SuContact SupplierContact { get; set; }
    }
}