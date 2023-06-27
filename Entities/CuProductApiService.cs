using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Product_API_Services")]
    public partial class CuProductApiService
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ServiceId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuProductApiServiceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuProductApiServiceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("CuProductApiServices")]
        public virtual CuProduct Product { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuProductApiServices")]
        public virtual RefService Service { get; set; }
    }
}