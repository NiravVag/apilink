using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_Picking_Contacts")]
    public partial class InspTranPickingContact
    {
        public int Id { get; set; }
        [Column("Picking_Tran_Id")]
        public int PickingTranId { get; set; }
        [Column("Lab_Contact_Id")]
        public int? LabContactId { get; set; }
        [Column("Cus_Contact_Id")]
        public int? CusContactId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranPickingContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CusContactId")]
        [InverseProperty("InspTranPickingContacts")]
        public virtual CuContact CusContact { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranPickingContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("LabContactId")]
        [InverseProperty("InspTranPickingContacts")]
        public virtual InspLabContact LabContact { get; set; }
        [ForeignKey("PickingTranId")]
        [InverseProperty("InspTranPickingContacts")]
        public virtual InspTranPicking PickingTran { get; set; }
    }
}