using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Status_Type")]
    public partial class FbStatusType
    {
        public FbStatusType()
        {
            FbStatuses = new HashSet<FbStatus>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("TypeNavigation")]
        public virtual ICollection<FbStatus> FbStatuses { get; set; }
    }
}