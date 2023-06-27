using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_Control_Attributes")]
    public partial class DfControlAttribute
    {
        public int Id { get; set; }
        public int ControlAttributeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Value { get; set; }
        [Column("ControlConfigurationID")]
        public int ControlConfigurationId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("ControlAttributeId")]
        [InverseProperty("DfControlAttributes")]
        public virtual DfControlTypeAttribute ControlAttribute { get; set; }
        [ForeignKey("ControlConfigurationId")]
        [InverseProperty("DfControlAttributes")]
        public virtual DfCuConfiguration ControlConfiguration { get; set; }
    }
}