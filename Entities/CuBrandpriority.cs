using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Brandpriority")]
    public partial class CuBrandpriority
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? BrandpriorityId { get; set; }
        public int? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("BrandpriorityId")]
        [InverseProperty("CuBrandpriorities")]
        public virtual CuRefBrandPriority Brandpriority { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuBrandpriorities")]
        public virtual CuCustomer Customer { get; set; }
    }
}