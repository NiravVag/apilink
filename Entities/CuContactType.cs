using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_ContactType")]
    public partial class CuContactType
    {
        public CuContactType()
        {
            CuCustomerContactTypes = new HashSet<CuCustomerContactType>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string ContactType { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("CuContactTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ContactType")]
        public virtual ICollection<CuCustomerContactType> CuCustomerContactTypes { get; set; }
    }
}