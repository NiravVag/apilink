using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Contact_API_Services")]
    public partial class SuContactApiService
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
        [InverseProperty("SuContactApiServices")]
        public virtual SuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("SuContactApiServiceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SuContactApiServiceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("SuContactApiServices")]
        public virtual RefService Service { get; set; }
    }
}