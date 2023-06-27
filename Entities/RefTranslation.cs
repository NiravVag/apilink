using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Translation")]
    public partial class RefTranslation
    {
        public int Id { get; set; }
        [Column("Text_FR")]
        [StringLength(2000)]
        public string TextFr { get; set; }
        [Column("Text_CH")]
        [StringLength(2000)]
        public string TextCh { get; set; }
        public int? TranslationGroupId { get; set; }

        [ForeignKey("TranslationGroupId")]
        [InverseProperty("RefTranslations")]
        public virtual RefTranslationGroup TranslationGroup { get; set; }
    }
}