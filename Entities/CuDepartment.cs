using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Department")]
    public partial class CuDepartment
    {
        public CuDepartment()
        {
            AudTransactions = new HashSet<AudTransaction>();
            CuCheckPointsDepartments = new HashSet<CuCheckPointsDepartment>();
            CuContactDepartments = new HashSet<CuContactDepartment>();
            CuPrDepartments = new HashSet<CuPrDepartment>();
            CuPurchaseOrders = new HashSet<CuPurchaseOrder>();
            DaUserByDepartments = new HashSet<DaUserByDepartment>();
            DmDepartments = new HashSet<DmDepartment>();
            EsCuConfigs = new HashSet<EsCuConfig>();
            InspTranCuDepartments = new HashSet<InspTranCuDepartment>();
            InspTransactionDrafts = new HashSet<InspTransactionDraft>();
            InvTranInvoiceRequests = new HashSet<InvTranInvoiceRequest>();
            ItUserCuDepartments = new HashSet<ItUserCuDepartment>();
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        public bool Active { get; set; }
        [Required]
        [StringLength(100)]
        public string Code { get; set; }
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

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuDepartmentCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuDepartments")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuDepartmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuDepartments")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuDepartmentUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("Dept")]
        public virtual ICollection<CuCheckPointsDepartment> CuCheckPointsDepartments { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<CuContactDepartment> CuContactDepartments { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<CuPrDepartment> CuPrDepartments { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrders { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<DaUserByDepartment> DaUserByDepartments { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<DmDepartment> DmDepartments { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<EsCuConfig> EsCuConfigs { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<InspTranCuDepartment> InspTranCuDepartments { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDrafts { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequests { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<ItUserCuDepartment> ItUserCuDepartments { get; set; }
        [InverseProperty("Depart")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
    }
}