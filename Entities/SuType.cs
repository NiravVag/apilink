using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Type")]
    public partial class SuType
    {
        public SuType()
        {
            SuSuppliers = new HashSet<SuSupplier>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        public int? TypeTransId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("SuTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Type")]
        public virtual ICollection<SuSupplier> SuSuppliers { get; set; }
    }
}