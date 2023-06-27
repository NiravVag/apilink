using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_Picking")]
    public partial class InspTranPicking
    {
        public InspTranPicking()
        {
            InspTranPickingContacts = new HashSet<InspTranPickingContact>();
        }

        public int Id { get; set; }
        [Column("Lab_Id")]
        public int? LabId { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        [Column("Lab_Address_Id")]
        public int? LabAddressId { get; set; }
        [Column("Cus_Address_Id")]
        public int? CusAddressId { get; set; }
        [Column("PO_Tran_Id")]
        public int PoTranId { get; set; }
        [Column("Picking_Qty")]
        public int PickingQty { get; set; }
        [StringLength(2000)]
        public string Remarks { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdationDate { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletionDate { get; set; }
        public int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("InspTranPickings")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranPickingCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CusAddressId")]
        [InverseProperty("InspTranPickings")]
        public virtual CuAddress CusAddress { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InspTranPickings")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranPickingDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("LabId")]
        [InverseProperty("InspTranPickings")]
        public virtual InspLabDetail Lab { get; set; }
        [ForeignKey("LabAddressId")]
        [InverseProperty("InspTranPickings")]
        public virtual InspLabAddress LabAddress { get; set; }
        [ForeignKey("PoTranId")]
        [InverseProperty("InspTranPickings")]
        public virtual InspPurchaseOrderTransaction PoTran { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspTranPickingUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("PickingTran")]
        public virtual ICollection<InspTranPickingContact> InspTranPickingContacts { get; set; }
    }
}