using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CustomerGroup")]
    public partial class CuCustomerGroup
    {
        public CuCustomerGroup()
        {
            CuCustomers = new HashSet<CuCustomer>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("CuCustomerGroups")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("GroupNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
    }
}