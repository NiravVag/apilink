using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("COMP_TRAN_ComplaintsDetails")]
    public partial class CompTranComplaintsDetail
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public int? ProductId { get; set; }
        [Column("Complaint_Category")]
        public int ComplaintCategory { get; set; }
        [Column("Complaint_Description")]
        public string ComplaintDescription { get; set; }
        public string CorrectiveAction { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AnswerDate { get; set; }
        public string Remarks { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("ComplaintId")]
        [InverseProperty("CompTranComplaintsDetails")]
        public virtual CompComplaint Complaint { get; set; }
        [ForeignKey("ComplaintCategory")]
        [InverseProperty("CompTranComplaintsDetails")]
        public virtual CompRefCategory ComplaintCategoryNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CompTranComplaintsDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CompTranComplaintsDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("CompTranComplaintsDetails")]
        public virtual CuProduct Product { get; set; }
    }
}