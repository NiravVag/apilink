using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Brand")]
    public partial class CuBrand
    {
        public CuBrand()
        {
            AudTransactions = new HashSet<AudTransaction>();
            CuCheckPointsBrands = new HashSet<CuCheckPointsBrand>();
            CuContactBrands = new HashSet<CuContactBrand>();
            CuPrBrands = new HashSet<CuPrBrand>();
            CuPurchaseOrders = new HashSet<CuPurchaseOrder>();
            DaUserByBrands = new HashSet<DaUserByBrand>();
            DmBrands = new HashSet<DmBrand>();
            EsCuConfigs = new HashSet<EsCuConfig>();
            InspTranCuBrands = new HashSet<InspTranCuBrand>();
            InspTransactionDrafts = new HashSet<InspTransactionDraft>();
            InvTranInvoiceRequests = new HashSet<InvTranInvoiceRequest>();
            ItUserCuBrands = new HashSet<ItUserCuBrand>();
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        public bool Active { get; set; }
        [StringLength(200)]
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
        [InverseProperty("CuBrandCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuBrands")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuBrandDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuBrands")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuBrandUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<CuCheckPointsBrand> CuCheckPointsBrands { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<CuContactBrand> CuContactBrands { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<CuPrBrand> CuPrBrands { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrders { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<DaUserByBrand> DaUserByBrands { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<DmBrand> DmBrands { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<EsCuConfig> EsCuConfigs { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<InspTranCuBrand> InspTranCuBrands { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDrafts { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequests { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<ItUserCuBrand> ItUserCuBrands { get; set; }
        [InverseProperty("Brand")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
    }
}