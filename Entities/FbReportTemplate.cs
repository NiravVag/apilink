using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Template")]
    public partial class FbReportTemplate
    {
        public FbReportTemplate()
        {
            InspProductTransactions = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        public int? FbTemplateId { get; set; }
        public bool? Active { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("FbReportTemplates")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Fbtemplate")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
    }
}