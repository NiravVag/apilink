using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_Transaction")]
    public partial class InspTransaction
    {
        public InspTransaction()
        {
            ClmTransactions = new HashSet<ClmTransaction>();
            CompComplaints = new HashSet<CompComplaint>();
            CuProductFileAttachments = new HashSet<CuProductFileAttachment>();
            EcAutQcFoodExpenses = new HashSet<EcAutQcFoodExpense>();
            EcAutQcTravelExpenses = new HashSet<EcAutQcTravelExpense>();
            EcExpenseClaimsInspections = new HashSet<EcExpenseClaimsInspection>();
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
            EsTranFiles = new HashSet<EsTranFile>();
            FbReportDetails = new HashSet<FbReportDetail>();
            InspContainerTransactions = new HashSet<InspContainerTransaction>();
            InspDfTransactions = new HashSet<InspDfTransaction>();
            InspProductTransactions = new HashSet<InspProductTransaction>();
            InspPurchaseOrderTransactions = new HashSet<InspPurchaseOrderTransaction>();
            InspTranCS = new HashSet<InspTranC>();
            InspTranCancels = new HashSet<InspTranCancel>();
            InspTranCuBrands = new HashSet<InspTranCuBrand>();
            InspTranCuBuyers = new HashSet<InspTranCuBuyer>();
            InspTranCuContacts = new HashSet<InspTranCuContact>();
            InspTranCuDepartments = new HashSet<InspTranCuDepartment>();
            InspTranCuMerchandisers = new HashSet<InspTranCuMerchandiser>();
            InspTranFaContacts = new HashSet<InspTranFaContact>();
            InspTranFileAttachmentZips = new HashSet<InspTranFileAttachmentZip>();
            InspTranFileAttachments = new HashSet<InspTranFileAttachment>();
            InspTranHoldReasons = new HashSet<InspTranHoldReason>();
            InspTranPickings = new HashSet<InspTranPicking>();
            InspTranReschedules = new HashSet<InspTranReschedule>();
            InspTranServiceTypes = new HashSet<InspTranServiceType>();
            InspTranShipmentTypes = new HashSet<InspTranShipmentType>();
            InspTranStatusLogs = new HashSet<InspTranStatusLog>();
            InspTranSuContacts = new HashSet<InspTranSuContact>();
            InspTransactionDraftInspections = new HashSet<InspTransactionDraft>();
            InspTransactionDraftPreviousBookingNoNavigations = new HashSet<InspTransactionDraft>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvAutTranStatusLogs = new HashSet<InvAutTranStatusLog>();
            InvCreTranClaimDetails = new HashSet<InvCreTranClaimDetail>();
            InvExfTranStatusLogs = new HashSet<InvExfTranStatusLog>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            JobScheduleLogs = new HashSet<JobScheduleLog>();
            LogBookingReportEmailQueues = new HashSet<LogBookingReportEmailQueue>();
            QuQuotationInspMandays = new HashSet<QuQuotationInspManday>();
            QuQuotationInsps = new HashSet<QuQuotationInsp>();
            RepFastTransactions = new HashSet<RepFastTransaction>();
            SchScheduleCS = new HashSet<SchScheduleC>();
            SchScheduleQcs = new HashSet<SchScheduleQc>();
        }

        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [Column("InternalReferencePO")]
        [StringLength(1500)]
        public string InternalReferencePo { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        [Column("Supplier_Id")]
        public int SupplierId { get; set; }
        [Column("Factory_Id")]
        public int? FactoryId { get; set; }
        [Column("Status_Id")]
        public int StatusId { get; set; }
        [Column("SeasonYear_Id")]
        public int? SeasonYearId { get; set; }
        [Column("ServiceDate_From", TypeName = "datetime")]
        public DateTime ServiceDateFrom { get; set; }
        [Column("ServiceDate_To", TypeName = "datetime")]
        public DateTime ServiceDateTo { get; set; }
        [Column("Cus_Booking_Comments")]
        public string CusBookingComments { get; set; }
        [Column("API_Booking_Comments")]
        public string ApiBookingComments { get; set; }
        [Column("Internal_Comments")]
        public string InternalComments { get; set; }
        [Column("Office_Id")]
        public int? OfficeId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("Entity_Id")]
        public int EntityId { get; set; }
        [Column("Applicant_Name")]
        [StringLength(200)]
        public string ApplicantName { get; set; }
        [Column("Applicant_Email")]
        [StringLength(200)]
        public string ApplicantEmail { get; set; }
        [Column("Applicant_PhoneNo")]
        [StringLength(200)]
        public string ApplicantPhoneNo { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column("Flexible_Inspection_Date")]
        public bool? FlexibleInspectionDate { get; set; }
        public int? PreviousBookingNo { get; set; }
        public int? SplitPreviousBookingNo { get; set; }
        public int? ReInspectionType { get; set; }
        [StringLength(2000)]
        public string CustomerBookingNo { get; set; }
        public bool? IsPickingRequired { get; set; }
        [Column("Schedule_Comments")]
        public string ScheduleComments { get; set; }
        [Column("Fb_Mission_Id")]
        public int? FbMissionId { get; set; }
        [Column("Fb_Mission_Status")]
        public int? FbMissionStatus { get; set; }
        [Column("QCBookingComments")]
        public string QcbookingComments { get; set; }
        [Column("FirstServiceDate_From", TypeName = "datetime")]
        public DateTime? FirstServiceDateFrom { get; set; }
        [Column("FirstServiceDate_To", TypeName = "datetime")]
        public DateTime? FirstServiceDateTo { get; set; }
        public int? CollectionId { get; set; }
        public int? PriceCategoryId { get; set; }
        public bool? IsProcessing { get; set; }
        [StringLength(1500)]
        public string CompassBookingNo { get; set; }
        public bool? IsCombineRequired { get; set; }
        public int? BusinessLine { get; set; }
        public int? CuProductCategory { get; set; }
        public int? InspectionLocation { get; set; }
        [StringLength(500)]
        public string ShipmentPort { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShipmentDate { get; set; }
        [Column("EAN")]
        [StringLength(500)]
        public string Ean { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public int? ProductCategoryId { get; set; }
        [Column("Season_Id")]
        public int? SeasonId { get; set; }
        public int? BookingType { get; set; }
        public int? PaymentOptions { get; set; }
        public int? ReportRequest { get; set; }
        public bool? IsSameDayReport { get; set; }
        public bool? IsInspectionCertificate { get; set; }
        [Column("IsEAQF")]
        public bool? IsEaqf { get; set; }
        [Column("GAPDACorrelation")]
        public bool? Gapdacorrelation { get; set; }
        [Column("GAPDAName")]
        [StringLength(500)]
        public string Gapdaname { get; set; }
        [Column("GAPDAEmail")]
        [StringLength(500)]
        public string Gapdaemail { get; set; }

        [ForeignKey("BookingType")]
        [InverseProperty("InspTransactions")]
        public virtual InspRefBookingType BookingTypeNavigation { get; set; }

        [ForeignKey("BusinessLine")]
        [InverseProperty("InspTransactions")]
        public virtual RefBusinessLine BusinessLineNavigation { get; set; }
        [ForeignKey("CollectionId")]
        [InverseProperty("InspTransactions")]
        public virtual CuCollection Collection { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuProductCategory")]
        [InverseProperty("InspTransactions")]
        public virtual CuProductCategory CuProductCategoryNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InspTransactions")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryId")]
        [InverseProperty("InspTransactionFactories")]
        public virtual SuSupplier Factory { get; set; }
        [ForeignKey("FbMissionStatus")]
        [InverseProperty("InspTransactions")]
        public virtual FbStatus FbMissionStatusNavigation { get; set; }
        [ForeignKey("InspectionLocation")]
        [InverseProperty("InspTransactions")]
        public virtual InspRefInspectionLocation InspectionLocationNavigation { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("InspTransactions")]
        public virtual RefLocation Office { get; set; }
        [ForeignKey("PaymentOptions")]
        [InverseProperty("InspTransactions")]
        public virtual InspRefPaymentOption PaymentOptionsNavigation { get; set; }
        [ForeignKey("PriceCategoryId")]
        [InverseProperty("InspTransactions")]
        public virtual CuPriceCategory PriceCategory { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("InspTransactions")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [ForeignKey("ProductSubCategoryId")]
        [InverseProperty("InspTransactions")]
        public virtual RefProductCategorySub ProductSubCategory { get; set; }
        [ForeignKey("ProductSubCategory2Id")]
        [InverseProperty("InspTransactions")]
        public virtual RefProductCategorySub2 ProductSubCategory2 { get; set; }
        [ForeignKey("ReInspectionType")]
        [InverseProperty("InspTransactions")]
        public virtual RefReInspectionType ReInspectionTypeNavigation { get; set; }
        [ForeignKey("ReportRequest")]
        [InverseProperty("InspTransactions")]
        public virtual InspRefReportRequest ReportRequestNavigation { get; set; }
        [ForeignKey("SeasonId")]
        [InverseProperty("InspTransactions")]
        public virtual CuSeasonConfig Season { get; set; }
        [ForeignKey("SeasonYearId")]
        [InverseProperty("InspTransactions")]
        public virtual RefSeasonYear SeasonYear { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("InspTransactions")]
        public virtual InspStatus Status { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("InspTransactionSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("InspectionNoNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactions { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<CompComplaint> CompComplaints { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<CuProductFileAttachment> CuProductFileAttachments { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<EcAutQcFoodExpense> EcAutQcFoodExpenses { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenses { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<EcExpenseClaimsInspection> EcExpenseClaimsInspections { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<EsTranFile> EsTranFiles { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<FbReportDetail> FbReportDetails { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspContainerTransaction> InspContainerTransactions { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<InspDfTransaction> InspDfTransactions { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactions { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranC> InspTranCS { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranCancel> InspTranCancels { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranCuBrand> InspTranCuBrands { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranCuBuyer> InspTranCuBuyers { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranCuContact> InspTranCuContacts { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranCuDepartment> InspTranCuDepartments { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranCuMerchandiser> InspTranCuMerchandisers { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranFaContact> InspTranFaContacts { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranFileAttachmentZip> InspTranFileAttachmentZips { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranFileAttachment> InspTranFileAttachments { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranHoldReason> InspTranHoldReasons { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<InspTranPicking> InspTranPickings { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranReschedule> InspTranReschedules { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranServiceType> InspTranServiceTypes { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranShipmentType> InspTranShipmentTypes { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<InspTranStatusLog> InspTranStatusLogs { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTranSuContact> InspTranSuContacts { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDraftInspections { get; set; }
        [InverseProperty("PreviousBookingNoNavigation")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDraftPreviousBookingNoNavigations { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InvAutTranStatusLog> InvAutTranStatusLogs { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InvCreTranClaimDetail> InvCreTranClaimDetails { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InvExfTranStatusLog> InvExfTranStatusLogs { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("BookingNoNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<JobScheduleLog> JobScheduleLogs { get; set; }
        [InverseProperty("Inspection")]
        public virtual ICollection<LogBookingReportEmailQueue> LogBookingReportEmailQueues { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<QuQuotationInspManday> QuQuotationInspMandays { get; set; }
        [InverseProperty("IdBookingNavigation")]
        public virtual ICollection<QuQuotationInsp> QuQuotationInsps { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<RepFastTransaction> RepFastTransactions { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<SchScheduleC> SchScheduleCS { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<SchScheduleQc> SchScheduleQcs { get; set; }
    }
}