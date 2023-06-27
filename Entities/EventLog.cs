using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EventLog")]
    public partial class EventLog
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(2000)]
        public string Name { get; set; }
        [Column("EventID")]
        public int? EventId { get; set; }
        [StringLength(50)]
        public string LogLevel { get; set; }
        [StringLength(4000)]
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? EntityId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ResponseTime { get; set; }
    }
}