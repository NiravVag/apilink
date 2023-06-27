using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Result_Config")]
    public partial class EsResultConfig
    {
        public int Id { get; set; }
        [Column("Customer_Result_Id")]
        public int? CustomerResultId { get; set; }
        public int EsDetailsId { get; set; }
        [Column("API_Result_Id")]
        public int? ApiResultId { get; set; }

        [ForeignKey("ApiResultId")]
        [InverseProperty("EsResultConfigs")]
        public virtual FbReportResult ApiResult { get; set; }
        [ForeignKey("CustomerResultId")]
        [InverseProperty("EsResultConfigs")]
        public virtual RefInspCusDecisionConfig CustomerResult { get; set; }
        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsResultConfigs")]
        public virtual EsDetail EsDetails { get; set; }
    }
}