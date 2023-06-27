using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_DA_Transaction")]
    public partial class InvDaTransaction
    {
        public InvDaTransaction()
        {
            InvDaCustomers = new HashSet<InvDaCustomer>();
            InvDaInvoiceTypes = new HashSet<InvDaInvoiceType>();
            InvDaOffices = new HashSet<InvDaOffice>();
        }

        public int Id { get; set; }
        public int StaffId { get; set; }
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
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvDaTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvDaTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvDaTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("InvDaTransactions")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvDaTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("InvDa")]
        public virtual ICollection<InvDaCustomer> InvDaCustomers { get; set; }
        [InverseProperty("InvDa")]
        public virtual ICollection<InvDaInvoiceType> InvDaInvoiceTypes { get; set; }
        [InverseProperty("InvDa")]
        public virtual ICollection<InvDaOffice> InvDaOffices { get; set; }
    }
}