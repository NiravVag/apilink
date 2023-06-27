using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("COMP_Complaints")]
    public partial class CompComplaint
    {
        public CompComplaint()
        {
            CompTranComplaintsDetails = new HashSet<CompTranComplaintsDetail>();
            CompTranPersonInCharges = new HashSet<CompTranPersonInCharge>();
        }

        public int Id { get; set; }
        public int Type { get; set; }
        public int? Service { get; set; }
        [Column("Inspection_Id")]
        public int? InspectionId { get; set; }
        [Column("Audit_Id")]
        public int? AuditId { get; set; }
        [Column("Complaint_Date", TypeName = "datetime")]
        public DateTime? ComplaintDate { get; set; }
        [Column("Recipient_Type")]
        public int RecipientType { get; set; }
        public int Department { get; set; }
        public string Remarks { get; set; }
        public int? CustomerId { get; set; }
        public int? Country { get; set; }
        public int? Office { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("CompComplaints")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("Country")]
        [InverseProperty("CompComplaints")]
        public virtual RefCountry CountryNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CompComplaintCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CompComplaints")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CompComplaintDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("Department")]
        [InverseProperty("CompComplaints")]
        public virtual CompRefDepartment DepartmentNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("CompComplaints")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("Office")]
        [InverseProperty("CompComplaints")]
        public virtual RefLocation OfficeNavigation { get; set; }
        [ForeignKey("RecipientType")]
        [InverseProperty("CompComplaints")]
        public virtual CompRefRecipientType RecipientTypeNavigation { get; set; }
        [ForeignKey("Service")]
        [InverseProperty("CompComplaints")]
        public virtual RefService ServiceNavigation { get; set; }
        [ForeignKey("Type")]
        [InverseProperty("CompComplaints")]
        public virtual CompRefType TypeNavigation { get; set; }
        [InverseProperty("Complaint")]
        public virtual ICollection<CompTranComplaintsDetail> CompTranComplaintsDetails { get; set; }
        [InverseProperty("Complaint")]
        public virtual ICollection<CompTranPersonInCharge> CompTranPersonInCharges { get; set; }
    }
}