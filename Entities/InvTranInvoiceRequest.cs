using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_TRAN_Invoice_Request")]
    public partial class InvTranInvoiceRequest
    {
        public InvTranInvoiceRequest()
        {
            InvTranInvoiceRequestContacts = new HashSet<InvTranInvoiceRequestContact>();
        }

        public int Id { get; set; }
        public int? CuPriceCardId { get; set; }
        [StringLength(100)]
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public int? DepartmentId { get; set; }
        public int? BrandId { get; set; }
        public int? BuyerId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? ProductCategoryId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("InvTranInvoiceRequests")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("BuyerId")]
        [InverseProperty("InvTranInvoiceRequests")]
        public virtual CuBuyer Buyer { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvTranInvoiceRequestCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceCardId")]
        [InverseProperty("InvTranInvoiceRequests")]
        public virtual CuPrDetail CuPriceCard { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvTranInvoiceRequestDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("InvTranInvoiceRequests")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("InvTranInvoiceRequests")]
        public virtual CuProductCategory ProductCategory { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvTranInvoiceRequestUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("InvoiceRequest")]
        public virtual ICollection<InvTranInvoiceRequestContact> InvTranInvoiceRequestContacts { get; set; }
    }
}