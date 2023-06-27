using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_KPI_Teamplate_Customer")]
    public partial class RefKpiTeamplateCustomer
    {
        public int Id { get; set; }
        public int? TeamplateId { get; set; }
        public int? CustomerId { get; set; }
        public bool Active { get; set; }
        public int? UserTypeId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("RefKpiTeamplateCustomers")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("TeamplateId")]
        [InverseProperty("RefKpiTeamplateCustomers")]
        public virtual RefKpiTeamplate Teamplate { get; set; }
        [ForeignKey("UserTypeId")]
        [InverseProperty("RefKpiTeamplateCustomers")]
        public virtual ItUserType UserType { get; set; }
    }
}