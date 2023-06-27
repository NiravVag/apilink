using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_Container_Transaction")]
    public partial class InspContainerTransaction
    {
        public InspContainerTransaction()
        {
            InspPurchaseOrderTransactions = new HashSet<InspPurchaseOrderTransaction>();
        }

        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("Container_Id")]
        public int ContainerId { get; set; }
        public int TotalBookingQuantity { get; set; }
        public string Remarks { get; set; }
        public int? ContainerSize { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        [Column("Fb_Report_Id")]
        public int? FbReportId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ContainerSize")]
        [InverseProperty("InspContainerTransactions")]
        public virtual RefContainerSize ContainerSizeNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspContainerTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspContainerTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspContainerTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FbReportId")]
        [InverseProperty("InspContainerTransactions")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspContainerTransactions")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspContainerTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("ContainerRef")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactions { get; set; }
    }
}