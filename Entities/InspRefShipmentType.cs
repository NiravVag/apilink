using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_ShipmentType")]
    public partial class InspRefShipmentType
    {
        public InspRefShipmentType()
        {
            InspTranShipmentTypes = new HashSet<InspTranShipmentType>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("ShipmentType")]
        public virtual ICollection<InspTranShipmentType> InspTranShipmentTypes { get; set; }
    }
}