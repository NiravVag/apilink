using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ENT_Fields")]
    public partial class EntField
    {
        public EntField()
        {
            EntPagesFields = new HashSet<EntPagesField>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column("ENTPageId")]
        public int? EntpageId { get; set; }

        [ForeignKey("EntpageId")]
        [InverseProperty("EntFields")]
        public virtual EntPage Entpage { get; set; }
        [InverseProperty("Entfield")]
        public virtual ICollection<EntPagesField> EntPagesFields { get; set; }
    }
}