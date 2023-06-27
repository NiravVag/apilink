using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("COMP_REF_Recipient_Type")]
    public partial class CompRefRecipientType
    {
        public CompRefRecipientType()
        {
            CompComplaints = new HashSet<CompComplaint>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Sort { get; set; }

        [InverseProperty("RecipientTypeNavigation")]
        public virtual ICollection<CompComplaint> CompComplaints { get; set; }
    }
}