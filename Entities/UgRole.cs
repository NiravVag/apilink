using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("UG_Role")]
    public partial class UgRole
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? UserGuideId { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("UgRoles")]
        public virtual ItRole Role { get; set; }
        [ForeignKey("UserGuideId")]
        [InverseProperty("UgRoles")]
        public virtual UgUserGuideDetail UserGuide { get; set; }
    }
}