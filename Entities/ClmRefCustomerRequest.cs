using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_CustomerRequest")]
    public partial class ClmRefCustomerRequest
    {
        public ClmRefCustomerRequest()
        {
            ClmTranCustomerRequests = new HashSet<ClmTranCustomerRequest>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("CustomerRequest")]
        public virtual ICollection<ClmTranCustomerRequest> ClmTranCustomerRequests { get; set; }
    }
}