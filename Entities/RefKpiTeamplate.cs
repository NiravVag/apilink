using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_KPI_Teamplate")]
    public partial class RefKpiTeamplate
    {
        public RefKpiTeamplate()
        {
            RefKpiTeamplateCustomers = new HashSet<RefKpiTeamplateCustomer>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? TypeId { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsDefaultCustomer { get; set; }

        [ForeignKey("TypeId")]
        [InverseProperty("RefKpiTeamplates")]
        public virtual RefKpiTemplateType Type { get; set; }
        [InverseProperty("Teamplate")]
        public virtual ICollection<RefKpiTeamplateCustomer> RefKpiTeamplateCustomers { get; set; }
    }
}