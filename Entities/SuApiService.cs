using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_API_Services")]
    public partial class SuApiService
    {
        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public int? ServiceId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SuApiServiceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SuApiServiceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("SuApiServices")]
        public virtual RefService Service { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuApiServices")]
        public virtual SuSupplier Supplier { get; set; }
    }
}