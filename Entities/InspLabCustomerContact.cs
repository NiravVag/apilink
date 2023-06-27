using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_LAB_Customer_Contact")]
    public partial class InspLabCustomerContact
    {
        [Column("Lab_Id")]
        public int LabId { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        [Column("Contact_Id")]
        public int ContactId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("InspLabCustomerContacts")]
        public virtual InspLabContact Contact { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InspLabCustomerContacts")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("LabId")]
        [InverseProperty("InspLabCustomerContacts")]
        public virtual InspLabDetail Lab { get; set; }
    }
}