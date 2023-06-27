using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Container_Size")]
    public partial class RefContainerSize
    {
        public RefContainerSize()
        {
            InspContainerTransactions = new HashSet<InspContainerTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("ContainerSizeNavigation")]
        public virtual ICollection<InspContainerTransaction> InspContainerTransactions { get; set; }
    }
}