using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_REF_RelationshipStatus")]
    public partial class CuRefRelationshipStatus
    {
        public CuRefRelationshipStatus()
        {
            CuCustomers = new HashSet<CuCustomer>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("RelationshipStatus")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
    }
}