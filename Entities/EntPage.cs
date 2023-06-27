using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ENT_Pages")]
    public partial class EntPage
    {
        public EntPage()
        {
            EntFields = new HashSet<EntField>();
        }

        public int Id { get; set; }
        public int? RightId { get; set; }
        public int? ServiceId { get; set; }
        public bool? Active { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("EntPages")]
        public virtual RefService Service { get; set; }
        [InverseProperty("Entpage")]
        public virtual ICollection<EntField> EntFields { get; set; }
    }
}