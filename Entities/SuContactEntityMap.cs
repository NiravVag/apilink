using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Contact_Entity_Map")]
    public partial class SuContactEntityMap
    {
        public int ContactId { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("SuContactEntityMaps")]
        public virtual SuContact Contact { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("SuContactEntityMaps")]
        public virtual ApEntity Entity { get; set; }
    }
}