using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DM_File")]
    public partial class DmFile
    {
        public DmFile()
        {
            DmBrands = new HashSet<DmBrand>();
            DmDepartments = new HashSet<DmDepartment>();
        }

        public int Id { get; set; }
        [Column("DMDetailsId")]
        public int DmdetailsId { get; set; }
        [StringLength(200)]
        public string FileId { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(200)]
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public double? FileSize { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DmFileCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("DmFileDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DmdetailsId")]
        [InverseProperty("DmFiles")]
        public virtual DmDetail Dmdetails { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DmFiles")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Dmfile")]
        public virtual ICollection<DmBrand> DmBrands { get; set; }
        [InverseProperty("Dmfile")]
        public virtual ICollection<DmDepartment> DmDepartments { get; set; }
    }
}