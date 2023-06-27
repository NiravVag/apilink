using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("UG_UserGuide_Details")]
    public partial class UgUserGuideDetail
    {
        public UgUserGuideDetail()
        {
            UgRoles = new HashSet<UgRole>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string FileUrl { get; set; }
        [StringLength(500)]
        public string VideoUrl { get; set; }
        public int? TotalPage { get; set; }
        [Column("Is_Customer")]
        public bool? IsCustomer { get; set; }
        [Column("Is_Supplier")]
        public bool? IsSupplier { get; set; }
        [Column("Is_Factory")]
        public bool? IsFactory { get; set; }
        [Column("Is_Internal")]
        public bool? IsInternal { get; set; }
        public int? EntityId { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("UserGuide")]
        public virtual ICollection<UgRole> UgRoles { get; set; }
    }
}