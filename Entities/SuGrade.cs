using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Grade")]
    public partial class SuGrade
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int CustomerId { get; set; }
        [Column("Level_Id")]
        public int LevelId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PeriodFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PeriodTo { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SuGradeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("SuGrades")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SuGradeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("SuGrades")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LevelId")]
        [InverseProperty("SuGrades")]
        public virtual SuLevelCustom Level { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuGrades")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("SuGradeUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}