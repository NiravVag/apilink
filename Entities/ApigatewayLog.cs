using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("APIGateway_Log")]
    public partial class ApigatewayLog
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(500)]
        public string RequestUrl { get; set; }
        public string LogInformation { get; set; }
        public string ResponseMessage { get; set; }
        [StringLength(500)]
        public string RequestBaseUrl { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}