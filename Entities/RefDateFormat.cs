using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_DateFormat")]
    public partial class RefDateFormat
    {
        public RefDateFormat()
        {
            EsSuTemplateDetails = new HashSet<EsSuTemplateDetail>();
        }

        public int Id { get; set; }
        [StringLength(20)]
        public string DateFormat { get; set; }

        [InverseProperty("DateFormatNavigation")]
        public virtual ICollection<EsSuTemplateDetail> EsSuTemplateDetails { get; set; }
    }
}