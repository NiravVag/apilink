using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_Transaction_Draft")]
    public partial class InspTransactionDraft
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDateFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDateTo { get; set; }
        public int? BrandId { get; set; }
        public int? DepartmentId { get; set; }
        public int? InspectionId { get; set; }
        public string BookingInfo { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsReInspectionDraft { get; set; }
        public bool? IsReBookingDraft { get; set; }
        public int? PreviousBookingNo { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("InspTransactionDrafts")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTransactionDraftCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InspTransactionDrafts")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTransactionDraftDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("InspTransactionDrafts")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspTransactionDrafts")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryId")]
        [InverseProperty("InspTransactionDraftFactories")]
        public virtual SuSupplier Factory { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTransactionDraftInspections")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("PreviousBookingNo")]
        [InverseProperty("InspTransactionDraftPreviousBookingNoNavigations")]
        public virtual InspTransaction PreviousBookingNoNavigation { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("InspTransactionDraftSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspTransactionDraftUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}