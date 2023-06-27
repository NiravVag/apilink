using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_ServiceType_Config")]
    public partial class EsServiceTypeConfig
    {
        public int Id { get; set; }
        public int ServiceTypeId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsServiceTypeConfigs")]
        public virtual EsDetail EsDetails { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("EsServiceTypeConfigs")]
        public virtual RefServiceType ServiceType { get; set; }
    }
}