using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PurchaseOrder")]
    public partial class CuPurchaseOrder
    {
        public CuPurchaseOrder()
        {
            CuPoFactories = new HashSet<CuPoFactory>();
            CuPoSuppliers = new HashSet<CuPoSupplier>();
            CuPurchaseOrderAttachments = new HashSet<CuPurchaseOrderAttachment>();
            CuPurchaseOrderDetails = new HashSet<CuPurchaseOrderDetail>();
            InspPurchaseOrderTransactions = new HashSet<InspPurchaseOrderTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [Column("PONO")]
        [StringLength(200)]
        public string Pono { get; set; }
        [Column("Office_Id")]
        public int? OfficeId { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        [Column("Department_Id")]
        public int? DepartmentId { get; set; }
        [Column("Brand_Id")]
        public int? BrandId { get; set; }
        [StringLength(1000)]
        public string InternalRemarks { get; set; }
        [StringLength(1000)]
        public string CustomerRemarks { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [StringLength(1000)]
        public string CustomerReferencePo { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("CuPurchaseOrders")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPurchaseOrderCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuPurchaseOrders")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPurchaseOrderDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("CuPurchaseOrders")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuPurchaseOrders")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("CuPurchaseOrders")]
        public virtual RefLocation Office { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPurchaseOrderUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Po")]
        public virtual ICollection<CuPoFactory> CuPoFactories { get; set; }
        [InverseProperty("Po")]
        public virtual ICollection<CuPoSupplier> CuPoSuppliers { get; set; }
        [InverseProperty("Po")]
        public virtual ICollection<CuPurchaseOrderAttachment> CuPurchaseOrderAttachments { get; set; }
        [InverseProperty("Po")]
        public virtual ICollection<CuPurchaseOrderDetail> CuPurchaseOrderDetails { get; set; }
        [InverseProperty("Po")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactions { get; set; }
    }
}