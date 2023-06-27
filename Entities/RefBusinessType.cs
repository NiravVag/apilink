using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_BusinessType")]
    public partial class RefBusinessType
    {
        public RefBusinessType()
        {
            CuCustomers = new HashSet<CuCustomer>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefBusinessTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("BusinessTypeNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
    }
}