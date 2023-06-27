using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Contact_Entity_Map")]
    public partial class CuContactEntityMap
    {
        public int ContactId { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("CuContactEntityMaps")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuContactEntityMaps")]
        public virtual ApEntity Entity { get; set; }
    }
}