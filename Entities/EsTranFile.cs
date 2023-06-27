using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_TRAN_Files")]
    public partial class EsTranFile
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int? InspectionId { get; set; }
        [Column("Audit_Id")]
        public int? AuditId { get; set; }
        [Column("Report_Id")]
        public int? ReportId { get; set; }
        [Column("File_Type_Id")]
        public int? FileTypeId { get; set; }
        [Column("File_Link")]
        [StringLength(3000)]
        public string FileLink { get; set; }
        [Column("File_Name")]
        [StringLength(1000)]
        public string FileName { get; set; }
        [Column("Unique_Id")]
        [StringLength(1000)]
        public string UniqueId { get; set; }
        public bool? Active { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }
        [Column("Created_On", TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column("Created_By")]
        public int? CreatedBy { get; set; }
        [Column("Deleted_On", TypeName = "datetime")]
        public DateTime DeletedOn { get; set; }
        [Column("Deleted_By")]
        public int? DeletedBy { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("EsTranFiles")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("EsTranFileCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EsTranFileDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EsTranFiles")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FileTypeId")]
        [InverseProperty("EsTranFiles")]
        public virtual EsRefFileType FileType { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("EsTranFiles")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty("EsTranFiles")]
        public virtual FbReportDetail Report { get; set; }
    }
}