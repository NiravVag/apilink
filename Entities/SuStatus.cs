using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Status")]
    public partial class SuStatus
    {
        public SuStatus()
        {
            SuSuppliers = new HashSet<SuSupplier>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(1000)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SuStatusCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SuStatusDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<SuSupplier> SuSuppliers { get; set; }
    }
}