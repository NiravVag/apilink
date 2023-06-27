using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CS_Onsite_Email")]
    public partial class CuCsOnsiteEmail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public string EmailId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuCsOnsiteEmails")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("CuCsOnsiteEmails")]
        public virtual ItUserMaster User { get; set; }
    }
}