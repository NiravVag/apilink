using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Contact_Entity_Service_Map")]
    public partial class SuContactEntityServiceMap
    {
        public int Id { get; set; }
        public int? ContactId { get; set; }
        public int? ServiceId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("SuContactEntityServiceMaps")]
        public virtual SuContact Contact { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("SuContactEntityServiceMaps")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("SuContactEntityServiceMaps")]
        public virtual RefService Service { get; set; }
    }
}