using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Supplier_Customer_Contact")]
    public partial class SuSupplierCustomerContact
    {
        [Column("Supplier_Id")]
        public int SupplierId { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        [Column("Contact_Id")]
        public int ContactId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("SuSupplierCustomerContacts")]
        public virtual SuContact Contact { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("SuSupplierCustomerContacts")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuSupplierCustomerContacts")]
        public virtual SuSupplier Supplier { get; set; }
    }
}