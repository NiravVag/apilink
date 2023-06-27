using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_CU_Status")]
    public partial class InspCuStatus
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomStatusName { get; set; }
        public bool Active { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("InspCuStatuses")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("InspCuStatuses")]
        public virtual InspStatus Status { get; set; }
    }
}