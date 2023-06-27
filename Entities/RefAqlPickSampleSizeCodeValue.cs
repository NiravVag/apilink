using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_AQL_Pick_SampleSize_CodeValue")]
    public partial class RefAqlPickSampleSizeCodeValue
    {
        public RefAqlPickSampleSizeCodeValue()
        {
            RefAqlSampleCodeLevelISampleSizeCodeNavigations = new HashSet<RefAqlSampleCode>();
            RefAqlSampleCodeLevelIiSampleSizeCodeNavigations = new HashSet<RefAqlSampleCode>();
            RefAqlSampleCodeLevelIiiSampleSizeCodeNavigations = new HashSet<RefAqlSampleCode>();
            RefAqlSampleCodeLevelS1SampleSizeCodeNavigations = new HashSet<RefAqlSampleCode>();
            RefAqlSampleCodeLevelS2SampleSizeCodeNavigations = new HashSet<RefAqlSampleCode>();
            RefAqlSampleCodeLevelS3SampleSizeCodeNavigations = new HashSet<RefAqlSampleCode>();
            RefAqlSampleCodeLevelS4SampleSizeCodeNavigations = new HashSet<RefAqlSampleCode>();
        }

        [Key]
        [Column("Sample_Size_Code")]
        [StringLength(1)]
        public string SampleSizeCode { get; set; }
        [Column("Sample_Size")]
        public short SampleSize { get; set; }

        [InverseProperty("LevelISampleSizeCodeNavigation")]
        public virtual ICollection<RefAqlSampleCode> RefAqlSampleCodeLevelISampleSizeCodeNavigations { get; set; }
        [InverseProperty("LevelIiSampleSizeCodeNavigation")]
        public virtual ICollection<RefAqlSampleCode> RefAqlSampleCodeLevelIiSampleSizeCodeNavigations { get; set; }
        [InverseProperty("LevelIiiSampleSizeCodeNavigation")]
        public virtual ICollection<RefAqlSampleCode> RefAqlSampleCodeLevelIiiSampleSizeCodeNavigations { get; set; }
        [InverseProperty("LevelS1SampleSizeCodeNavigation")]
        public virtual ICollection<RefAqlSampleCode> RefAqlSampleCodeLevelS1SampleSizeCodeNavigations { get; set; }
        [InverseProperty("LevelS2SampleSizeCodeNavigation")]
        public virtual ICollection<RefAqlSampleCode> RefAqlSampleCodeLevelS2SampleSizeCodeNavigations { get; set; }
        [InverseProperty("LevelS3SampleSizeCodeNavigation")]
        public virtual ICollection<RefAqlSampleCode> RefAqlSampleCodeLevelS3SampleSizeCodeNavigations { get; set; }
        [InverseProperty("LevelS4SampleSizeCodeNavigation")]
        public virtual ICollection<RefAqlSampleCode> RefAqlSampleCodeLevelS4SampleSizeCodeNavigations { get; set; }
    }
}