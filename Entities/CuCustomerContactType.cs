using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CustomerContactTypes")]
    public partial class CuCustomerContactType
    {
        [Column("contact_id")]
        public int ContactId { get; set; }
        [Column("contact_type_id")]
        public int ContactTypeId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("CuCustomerContactTypes")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("ContactTypeId")]
        [InverseProperty("CuCustomerContactTypes")]
        public virtual CuContactType ContactType { get; set; }
    }
}