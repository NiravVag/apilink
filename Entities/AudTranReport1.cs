using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_Reports")]
    public partial class AudTranReport1
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        public bool Active { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranReport1S")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("AudTranReport1S")]
        public virtual ItUserMaster User { get; set; }
    }
}