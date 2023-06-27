using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_CU_Merchandiser")]
    public partial class InspTranCuMerchandiser
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("Merchandiser_Id")]
        public int MerchandiserId { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranCuMerchandiserCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranCuMerchandiserDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranCuMerchandisers")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("MerchandiserId")]
        [InverseProperty("InspTranCuMerchandisers")]
        public virtual CuContact Merchandiser { get; set; }
    }
}