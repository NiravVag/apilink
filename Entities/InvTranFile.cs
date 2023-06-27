using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_TRAN_Files")]
    public partial class InvTranFile
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        [StringLength(1000)]
        public string FileName { get; set; }
        public int? FileType { get; set; }
        public string UniqueId { get; set; }
        [StringLength(1000)]
        public string FilePath { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        [StringLength(1000)]
        public string InvoiceNo { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvTranFileCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvTranFileDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FileType")]
        [InverseProperty("InvTranFiles")]
        public virtual InvRefFileType FileTypeNavigation { get; set; }
        [ForeignKey("InvoiceId")]
        [InverseProperty("InvTranFiles")]
        public virtual InvAutTranDetail Invoice { get; set; }
    }
}