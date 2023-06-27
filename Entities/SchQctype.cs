using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SCH_QCType")]
    public partial class SchQctype
    {
        public SchQctype()
        {
            SchScheduleQcs = new HashSet<SchScheduleQc>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Type { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SchQctypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SchQctypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("SchQctypeModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [InverseProperty("QctypeNavigation")]
        public virtual ICollection<SchScheduleQc> SchScheduleQcs { get; set; }
    }
}