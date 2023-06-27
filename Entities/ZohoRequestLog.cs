using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ZOHO_RequestLog")]
    public partial class ZohoRequestLog
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("CustomerID")]
        public long? CustomerId { get; set; }
        [StringLength(500)]
        public string RequestUrl { get; set; }
        public string LogInformation { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
    }
}