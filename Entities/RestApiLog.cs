using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("RestApiLog")]
    public partial class RestApiLog
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string RequestMethod { get; set; }
        [StringLength(2000)]
        public string RequestPath { get; set; }
        [StringLength(2000)]
        public string RequestQuery { get; set; }
        public string RequestBody { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RequestTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ResponseTime { get; set; }
        public int? ResponseInMilliSeconds { get; set; }
        [StringLength(2000)]
        public string ResponseStatus { get; set; }
        public int? EntityId { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("RestApiLogs")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("RestApiLogs")]
        public virtual ApEntity Entity { get; set; }
    }
}