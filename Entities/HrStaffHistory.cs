using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_staffHistory")]
    public partial class HrStaffHistory
    {
        public int Id { get; set; }
        [Column("Staff_Id")]
        public int? StaffId { get; set; }
        [StringLength(100)]
        public string Company { get; set; }
        [Column("Sgt_Location_ID")]
        public int? SgtLocationId { get; set; }
        public double? Salary { get; set; }
        [Column("Currency_Id")]
        public int? CurrencyId { get; set; }
        [StringLength(100)]
        public string Position { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Datebegin { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateEnd { get; set; }
        [StringLength(100)]
        public string Comments { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("HrStaffHistories")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffHistories")]
        public virtual HrStaff Staff { get; set; }
    }
}