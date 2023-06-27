using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DM_details")]
    public partial class DmDetail
    {
        public DmDetail()
        {
            DmFiles = new HashSet<DmFile>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        [StringLength(400)]
        public string Description { get; set; }
        public int? ModuleId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public bool? Active { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DmDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("DmDetails")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("DmDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DmDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("DmDetails")]
        public virtual DmRefModule Module { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("DmDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Dmdetails")]
        public virtual ICollection<DmFile> DmFiles { get; set; }
    }
}