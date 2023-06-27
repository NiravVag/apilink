using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserByBuyer")]
    public partial class DaUserByBuyer
    {
        public int Id { get; set; }
        [Column("DAUserCustomerId")]
        public int DauserCustomerId { get; set; }
        public int? BuyerId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("DaUserByBuyers")]
        public virtual CuBuyer Buyer { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserByBuyers")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DauserCustomerId")]
        [InverseProperty("DaUserByBuyers")]
        public virtual DaUserCustomer DauserCustomer { get; set; }
    }
}