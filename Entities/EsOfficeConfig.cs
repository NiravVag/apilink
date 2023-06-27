using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Office_Config")]
    public partial class EsOfficeConfig
    {
        public int Id { get; set; }
        public int? OfficeId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsOfficeConfigs")]
        public virtual EsDetail EsDetails { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("EsOfficeConfigs")]
        public virtual RefLocation Office { get; set; }
    }
}