using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Cu_Contact_Service")]
    public partial class CuContactService
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int ServiceId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("CuContactServices")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuContactServiceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuContactServiceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuContactServices")]
        public virtual RefService Service { get; set; }
    }
}