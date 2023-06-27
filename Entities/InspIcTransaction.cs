using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_IC_Transaction")]
    public partial class InspIcTransaction
    {
        public InspIcTransaction()
        {
            InspIcTranProducts = new HashSet<InspIcTranProduct>();
        }

        public int Id { get; set; }
        [Column("ICNO")]
        [StringLength(2000)]
        public string Icno { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        [StringLength(500)]
        public string BeneficiaryName { get; set; }
        [StringLength(2000)]
        public string SupplierAddress { get; set; }
        [Column("ICTitleId")]
        public int IctitleId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovalDate { get; set; }
        [Column("ICStatus")]
        public int Icstatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        [StringLength(2000)]
        public string Comment { get; set; }
        [Column("Buyer_Name")]
        [StringLength(1000)]
        public string BuyerName { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspIcTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InspIcTransactions")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspIcTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspIcTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("Icstatus")]
        [InverseProperty("InspIcTransactions")]
        public virtual InspIcStatus IcstatusNavigation { get; set; }
        [ForeignKey("IctitleId")]
        [InverseProperty("InspIcTransactions")]
        public virtual InspIcTitle Ictitle { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("InspIcTransactions")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspIcTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Ic")]
        public virtual ICollection<InspIcTranProduct> InspIcTranProducts { get; set; }
    }
}