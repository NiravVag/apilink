using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_LAB_Contact")]
    public partial class InspLabContact
    {
        public InspLabContact()
        {
            InspLabCustomerContacts = new HashSet<InspLabCustomerContact>();
            InspTranPickingContacts = new HashSet<InspTranPickingContact>();
        }

        public int Id { get; set; }
        [Column("Lab_Id")]
        public int LabId { get; set; }
        [Column("Contact_Name")]
        [StringLength(200)]
        public string ContactName { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(200)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string Mail { get; set; }
        [Required]
        public bool? Active { get; set; }
        [StringLength(250)]
        public string JobTitle { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        [StringLength(200)]
        public string Comment { get; set; }

        [ForeignKey("LabId")]
        [InverseProperty("InspLabContacts")]
        public virtual InspLabDetail Lab { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<InspLabCustomerContact> InspLabCustomerContacts { get; set; }
        [InverseProperty("LabContact")]
        public virtual ICollection<InspTranPickingContact> InspTranPickingContacts { get; set; }
    }
}