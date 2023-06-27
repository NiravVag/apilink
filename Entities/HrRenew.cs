using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Renew")]
    public partial class HrRenew
    {
        public int Id { get; set; }
        [Column("staff_id")]
        public int StaffId { get; set; }
        public int Number { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrRenews")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrRenews")]
        public virtual HrStaff Staff { get; set; }
    }
}