using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_Office")]
    public partial class InvRefOffice
    {
        public InvRefOffice()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Address { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(100)]
        public string Mail { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("InvRefOffices")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("InvoiceOfficeNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("OfficeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
    }
}