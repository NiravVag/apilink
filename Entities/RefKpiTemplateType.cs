using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_KPI_Template_Type")]
    public partial class RefKpiTemplateType
    {
        public RefKpiTemplateType()
        {
            RefKpiTeamplates = new HashSet<RefKpiTeamplate>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("Type")]
        public virtual ICollection<RefKpiTeamplate> RefKpiTeamplates { get; set; }
    }
}