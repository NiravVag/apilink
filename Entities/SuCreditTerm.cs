using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_CreditTerm")]
    public partial class SuCreditTerm
    {
        public SuCreditTerm()
        {
            SuSupplierCustomers = new HashSet<SuSupplierCustomer>();
            SuSuppliers = new HashSet<SuSupplier>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(1000)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SuCreditTermCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SuCreditTermDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [InverseProperty("CreditTermNavigation")]
        public virtual ICollection<SuSupplierCustomer> SuSupplierCustomers { get; set; }
        [InverseProperty("CreditTerm")]
        public virtual ICollection<SuSupplier> SuSuppliers { get; set; }
    }
}