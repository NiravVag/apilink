using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_LAB_Details")]
    public partial class InspLabDetail
    {
        public InspLabDetail()
        {
            InspLabAddresses = new HashSet<InspLabAddress>();
            InspLabContacts = new HashSet<InspLabContact>();
            InspLabCustomerContacts = new HashSet<InspLabCustomerContact>();
            InspLabCustomers = new HashSet<InspLabCustomer>();
            InspTranPickings = new HashSet<InspTranPicking>();
        }

        public int Id { get; set; }
        [Column("Lab_Name")]
        [StringLength(200)]
        public string LabName { get; set; }
        [StringLength(200)]
        public string Comments { get; set; }
        [Required]
        public bool? Active { get; set; }
        [Column("Type_Id")]
        public int? TypeId { get; set; }
        [StringLength(200)]
        public string LegalName { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(200)]
        public string Fax { get; set; }
        [StringLength(200)]
        public string Website { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        [StringLength(200)]
        public string RegionalName { get; set; }
        [StringLength(200)]
        public string ContactPerson { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        [StringLength(500)]
        public string GlCode { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("InspLabDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("TypeId")]
        [InverseProperty("InspLabDetails")]
        public virtual InspLabType Type { get; set; }
        [InverseProperty("Lab")]
        public virtual ICollection<InspLabAddress> InspLabAddresses { get; set; }
        [InverseProperty("Lab")]
        public virtual ICollection<InspLabContact> InspLabContacts { get; set; }
        [InverseProperty("Lab")]
        public virtual ICollection<InspLabCustomerContact> InspLabCustomerContacts { get; set; }
        [InverseProperty("Lab")]
        public virtual ICollection<InspLabCustomer> InspLabCustomers { get; set; }
        [InverseProperty("Lab")]
        public virtual ICollection<InspTranPicking> InspTranPickings { get; set; }
    }
}