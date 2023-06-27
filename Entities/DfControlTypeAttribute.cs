using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_ControlType_Attributes")]
    public partial class DfControlTypeAttribute
    {
        public DfControlTypeAttribute()
        {
            DfControlAttributes = new HashSet<DfControlAttribute>();
        }

        public int Id { get; set; }
        public int AttributeId { get; set; }
        public int ControlTypeId { get; set; }
        [StringLength(50)]
        public string DefaultValue { get; set; }
        public bool Active { get; set; }

        [ForeignKey("AttributeId")]
        [InverseProperty("DfControlTypeAttributes")]
        public virtual DfAttribute Attribute { get; set; }
        [ForeignKey("ControlTypeId")]
        [InverseProperty("DfControlTypeAttributes")]
        public virtual DfControlType ControlType { get; set; }
        [InverseProperty("ControlAttribute")]
        public virtual ICollection<DfControlAttribute> DfControlAttributes { get; set; }
    }
}