using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_AQL_Sample_Code")]
    public partial class RefAqlSampleCode
    {
        [Key]
        [Column("Sample_Size_range_Code_Id")]
        public int SampleSizeRangeCodeId { get; set; }
        [Column("Min_size")]
        public int MinSize { get; set; }
        [Column("Max_size")]
        public int MaxSize { get; set; }
        [Required]
        [Column("Level_I_Sample_Size_Code")]
        [StringLength(1)]
        public string LevelISampleSizeCode { get; set; }
        [Required]
        [Column("Level_II_Sample_Size_Code")]
        [StringLength(1)]
        public string LevelIiSampleSizeCode { get; set; }
        [Required]
        [Column("Level_III_Sample_Size_Code")]
        [StringLength(1)]
        public string LevelIiiSampleSizeCode { get; set; }
        [Required]
        [Column("LEVEL_S1_SAMPLE_SIZE_CODE")]
        [StringLength(1)]
        public string LevelS1SampleSizeCode { get; set; }
        [Required]
        [Column("LEVEL_S2_SAMPLE_SIZE_CODE")]
        [StringLength(1)]
        public string LevelS2SampleSizeCode { get; set; }
        [Required]
        [Column("LEVEL_S3_SAMPLE_SIZE_CODE")]
        [StringLength(1)]
        public string LevelS3SampleSizeCode { get; set; }
        [Required]
        [Column("LEVEL_S4_SAMPLE_SIZE_CODE")]
        [StringLength(1)]
        public string LevelS4SampleSizeCode { get; set; }

        [ForeignKey("LevelISampleSizeCode")]
        [InverseProperty("RefAqlSampleCodeLevelISampleSizeCodeNavigations")]
        public virtual RefAqlPickSampleSizeCodeValue LevelISampleSizeCodeNavigation { get; set; }
        [ForeignKey("LevelIiSampleSizeCode")]
        [InverseProperty("RefAqlSampleCodeLevelIiSampleSizeCodeNavigations")]
        public virtual RefAqlPickSampleSizeCodeValue LevelIiSampleSizeCodeNavigation { get; set; }
        [ForeignKey("LevelIiiSampleSizeCode")]
        [InverseProperty("RefAqlSampleCodeLevelIiiSampleSizeCodeNavigations")]
        public virtual RefAqlPickSampleSizeCodeValue LevelIiiSampleSizeCodeNavigation { get; set; }
        [ForeignKey("LevelS1SampleSizeCode")]
        [InverseProperty("RefAqlSampleCodeLevelS1SampleSizeCodeNavigations")]
        public virtual RefAqlPickSampleSizeCodeValue LevelS1SampleSizeCodeNavigation { get; set; }
        [ForeignKey("LevelS2SampleSizeCode")]
        [InverseProperty("RefAqlSampleCodeLevelS2SampleSizeCodeNavigations")]
        public virtual RefAqlPickSampleSizeCodeValue LevelS2SampleSizeCodeNavigation { get; set; }
        [ForeignKey("LevelS3SampleSizeCode")]
        [InverseProperty("RefAqlSampleCodeLevelS3SampleSizeCodeNavigations")]
        public virtual RefAqlPickSampleSizeCodeValue LevelS3SampleSizeCodeNavigation { get; set; }
        [ForeignKey("LevelS4SampleSizeCode")]
        [InverseProperty("RefAqlSampleCodeLevelS4SampleSizeCodeNavigations")]
        public virtual RefAqlPickSampleSizeCodeValue LevelS4SampleSizeCodeNavigation { get; set; }
    }
}