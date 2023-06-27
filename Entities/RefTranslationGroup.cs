using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_TranslationGroup")]
    public partial class RefTranslationGroup
    {
        public RefTranslationGroup()
        {
            RefTranslations = new HashSet<RefTranslation>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }

        [InverseProperty("TranslationGroup")]
        public virtual ICollection<RefTranslation> RefTranslations { get; set; }
    }
}