using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ProspectStatus")]
    public partial class RefProspectStatus
    {
        public RefProspectStatus()
        {
            CuCustomers = new HashSet<CuCustomer>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("ProspectStatusNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
    }
}