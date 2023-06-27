using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_UserType")]
    public partial class ItUserType
    {
        public ItUserType()
        {
            EntPagesFields = new HashSet<EntPagesField>();
            ItUserMasters = new HashSet<ItUserMaster>();
            RefKpiTeamplateCustomers = new HashSet<RefKpiTeamplateCustomer>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Label { get; set; }

        [InverseProperty("UserType")]
        public virtual ICollection<EntPagesField> EntPagesFields { get; set; }
        [InverseProperty("UserType")]
        public virtual ICollection<ItUserMaster> ItUserMasters { get; set; }
        [InverseProperty("UserType")]
        public virtual ICollection<RefKpiTeamplateCustomer> RefKpiTeamplateCustomers { get; set; }
    }
}