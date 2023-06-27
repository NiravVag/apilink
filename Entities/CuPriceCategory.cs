using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PriceCategory")]
    public partial class CuPriceCategory
    {
        public CuPriceCategory()
        {
            CuPrPriceCategories = new HashSet<CuPrPriceCategory>();
            CuPriceCategoryPcsub2S = new HashSet<CuPriceCategoryPcsub2>();
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuPriceCategories")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuPriceCategories")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("PriceCategory")]
        public virtual ICollection<CuPrPriceCategory> CuPrPriceCategories { get; set; }
        [InverseProperty("PriceCategory")]
        public virtual ICollection<CuPriceCategoryPcsub2> CuPriceCategoryPcsub2S { get; set; }
        [InverseProperty("PriceCategory")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}