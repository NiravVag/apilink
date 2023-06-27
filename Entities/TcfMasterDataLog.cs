using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("TCF_Master_DataLog")]
    public partial class TcfMasterDataLog
    {
        [Column("ID")]
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? DataType { get; set; }
        [StringLength(500)]
        public string RequestUrl { get; set; }
        public string LogInformation { get; set; }
        public string ResponseMessage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}