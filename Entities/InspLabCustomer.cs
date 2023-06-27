using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_LAB_Customer")]
    public partial class InspLabCustomer
    {
        [Column("Lab_Id")]
        public int LabId { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        [StringLength(200)]
        public string Code { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("InspLabCustomers")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("LabId")]
        [InverseProperty("InspLabCustomers")]
        public virtual InspLabDetail Lab { get; set; }
    }
}