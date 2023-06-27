using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ENT_Master_Config")]
    public partial class EntMasterConfig
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? CountryId { get; set; }
        public int? Type { get; set; }
        public string Value { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("Type")]
        [InverseProperty("EntMasterConfigs")]
        public virtual EntMasterType TypeNavigation { get; set; }
    }
}