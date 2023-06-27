using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_AQL_Pick_SampleSize_Acce_Code")]
    public partial class RefAqlPickSampleSizeAcceCode
    {
        public int Id { get; set; }
        [Column("Sample_Size_Code")]
        [StringLength(10)]
        public string SampleSizeCode { get; set; }
        public double? PickValue { get; set; }
        [Column("Acc_sample_Size_Code")]
        [StringLength(10)]
        public string AccSampleSizeCode { get; set; }
        public int? Accepted { get; set; }
        public int? Rejected { get; set; }
    }
}