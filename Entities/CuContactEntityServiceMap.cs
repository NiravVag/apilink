using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Contact_Entity_Service_Map")]
    public partial class CuContactEntityServiceMap
    {
        public int Id { get; set; }
        public int? ContactId { get; set; }
        public int? ServiceId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("CuContactEntityServiceMaps")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuContactEntityServiceMaps")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuContactEntityServiceMaps")]
        public virtual RefService Service { get; set; }
    }
}