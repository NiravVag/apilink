using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Cu_SalesIncharge")]
    public partial class CuSalesIncharge
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? StaffId { get; set; }
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
        [InverseProperty("CuSalesIncharges")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("CuSalesIncharges")]
        public virtual HrStaff Staff { get; set; }
    }
}