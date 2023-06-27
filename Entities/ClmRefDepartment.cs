using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_REF_Department")]
    public partial class ClmRefDepartment
    {
        public ClmRefDepartment()
        {
            ClmTranDepartments = new HashSet<ClmTranDepartment>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("Department")]
        public virtual ICollection<ClmTranDepartment> ClmTranDepartments { get; set; }
    }
}