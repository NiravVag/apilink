using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_KAM")]
    public partial class CuKam
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        [Column("KAM_Id")]
        public int? KamId { get; set; }
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

        [ForeignKey("CustomerId")]
        [InverseProperty("CuKams")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("KamId")]
        [InverseProperty("CuKams")]
        public virtual HrStaff Kam { get; set; }
    }
}