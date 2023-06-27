using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_ProductCategory")]
    public partial class CuProductCategory
    {
        public CuProductCategory()
        {
            InspTransactions = new HashSet<InspTransaction>();
            InvTranInvoiceRequests = new HashSet<InvTranInvoiceRequest>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Code { get; set; }
        public bool? Active { get; set; }
        public int? CustomerId { get; set; }
        [StringLength(500)]
        public string Sector { get; set; }
        public int? Sort { get; set; }
        public int? EntityId { get; set; }
        public int? LinkProductSubCategory { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuProductCategories")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuProductCategories")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LinkProductSubCategory")]
        [InverseProperty("CuProductCategories")]
        public virtual RefProductCategorySub LinkProductSubCategoryNavigation { get; set; }
        [InverseProperty("CuProductCategoryNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequests { get; set; }
    }
}