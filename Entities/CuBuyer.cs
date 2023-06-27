using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Buyer")]
    public partial class CuBuyer
    {
        public CuBuyer()
        {
            CuBuyerApiServices = new HashSet<CuBuyerApiService>();
            CuPrBuyers = new HashSet<CuPrBuyer>();
            DaUserByBuyers = new HashSet<DaUserByBuyer>();
            EsCuConfigs = new HashSet<EsCuConfig>();
            InspTranCuBuyers = new HashSet<InspTranCuBuyer>();
            InvTranInvoiceRequests = new HashSet<InvTranInvoiceRequest>();
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
        [InverseProperty("CuBuyerCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuBuyers")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuBuyerDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuBuyers")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuBuyerUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Buyer")]
        public virtual ICollection<CuBuyerApiService> CuBuyerApiServices { get; set; }
        [InverseProperty("Buyer")]
        public virtual ICollection<CuPrBuyer> CuPrBuyers { get; set; }
        [InverseProperty("Buyer")]
        public virtual ICollection<DaUserByBuyer> DaUserByBuyers { get; set; }
        [InverseProperty("Buyer")]
        public virtual ICollection<EsCuConfig> EsCuConfigs { get; set; }
        [InverseProperty("Buyer")]
        public virtual ICollection<InspTranCuBuyer> InspTranCuBuyers { get; set; }
        [InverseProperty("Buyer")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequests { get; set; }
    }
}