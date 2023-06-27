using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Supplier_Customer")]
    public partial class SuSupplierCustomer
    {
        [Column("Supplier_Id")]
        public int SupplierId { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        [StringLength(100)]
        public string Code { get; set; }
        [Column("Credit_Term")]
        public int? CreditTerm { get; set; }
        public bool? IsStatisticsVisibility { get; set; }

        [ForeignKey("CreditTerm")]
        [InverseProperty("SuSupplierCustomers")]
        public virtual SuCreditTerm CreditTermNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("SuSupplierCustomers")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuSupplierCustomers")]
        public virtual SuSupplier Supplier { get; set; }
    }
}