using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_Transaction")]
    public partial class AudTransaction
    {
        public AudTransaction()
        {
            AudFbReportCheckpoints = new HashSet<AudFbReportCheckpoint>();
            AudTranAuditors = new HashSet<AudTranAuditor>();
            AudTranCS = new HashSet<AudTranC>();
            AudTranCancelReschedules = new HashSet<AudTranCancelReschedule>();
            AudTranCuContacts = new HashSet<AudTranCuContact>();
            AudTranFaContacts = new HashSet<AudTranFaContact>();
            AudTranFaProfiles = new HashSet<AudTranFaProfile>();
            AudTranFileAttachments = new HashSet<AudTranFileAttachment>();
            AudTranReport1S = new HashSet<AudTranReport1>();
            AudTranReportDetails = new HashSet<AudTranReportDetail>();
            AudTranReports = new HashSet<AudTranReport>();
            AudTranServiceTypes = new HashSet<AudTranServiceType>();
            AudTranStatusLogs = new HashSet<AudTranStatusLog>();
            AudTranSuContacts = new HashSet<AudTranSuContact>();
            AudTranWorkProcesses = new HashSet<AudTranWorkProcess>();
            CompComplaints = new HashSet<CompComplaint>();
            EcExpenseClaimsAudits = new HashSet<EcExpenseClaimsAudit>();
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
            EsTranFiles = new HashSet<EsTranFile>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvAutTranStatusLogs = new HashSet<InvAutTranStatusLog>();
            InvExfTranStatusLogs = new HashSet<InvExfTranStatusLog>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            LogBookingReportEmailQueues = new HashSet<LogBookingReportEmailQueue>();
            QuQuotationAudMandays = new HashSet<QuQuotationAudManday>();
            QuQuotationAudits = new HashSet<QuQuotationAudit>();
        }

        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [Required]
        [StringLength(500)]
        public string ReportNo { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        [Column("Status_Id")]
        public int StatusId { get; set; }
        [Column("Brand_Id")]
        public int? BrandId { get; set; }
        [Column("Department_Id")]
        public int? DepartmentId { get; set; }
        [Column("Season_Id")]
        public int? SeasonId { get; set; }
        [Column("SeasonYear_Id")]
        public int? SeasonYearId { get; set; }
        [Column("Supplier_Id")]
        public int SupplierId { get; set; }
        [Column("Factory_Id")]
        public int FactoryId { get; set; }
        [Column("AuditType_Id")]
        public int AuditTypeId { get; set; }
        [Column("ServiceDate_From", TypeName = "datetime")]
        public DateTime ServiceDateFrom { get; set; }
        [Column("ServiceDate_To", TypeName = "datetime")]
        public DateTime ServiceDateTo { get; set; }
        [Column("Evalution_Id")]
        public int? EvalutionId { get; set; }
        [Column("Cus_Booking_Comments")]
        [StringLength(1500)]
        public string CusBookingComments { get; set; }
        [Column("API_Booking_Comments")]
        [StringLength(1500)]
        public string ApiBookingComments { get; set; }
        [Column("Internal_Comments")]
        [StringLength(1500)]
        public string InternalComments { get; set; }
        [Column("Office_Id")]
        public int? OfficeId { get; set; }
        [Column("PO_Number")]
        [StringLength(1000)]
        public string PoNumber { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }
        [StringLength(200)]
        public string ApplicantEmail { get; set; }
        [StringLength(200)]
        public string ApplicantName { get; set; }
        [StringLength(200)]
        public string ApplicantPhNo { get; set; }
        [StringLength(1000)]
        public string CustomerBookingNo { get; set; }
        [Column("FBMissionId")]
        public int? FbmissionId { get; set; }
        [Column("FBReportId")]
        public int? FbreportId { get; set; }
        public int? FillingStatus { get; set; }
        [Column("FBMissionTitle")]
        [StringLength(3000)]
        public string FbmissionTitle { get; set; }
        [Column("FBReportTitle")]
        [StringLength(3000)]
        public string FbreportTitle { get; set; }
        [Column("FBFillingStatus")]
        public int? FbfillingStatus { get; set; }        
        [Column("FBReviewStatus")]
        public int? FbreviewStatus { get; set; }
        [Column("FBReportStatus")]
        public int? FbreportStatus { get; set; }
        public bool? IsEaqf { get; set; }
        [StringLength(100)]
        public string ScoreValue { get; set; }
        [StringLength(100)]
        public string Scorepercentage { get; set; }
        [StringLength(100)]
        public string Grade { get; set; }
        [StringLength(4000)]
        public string ReportRemarks { get; set; }
        [StringLength(500)]
        public string FinalReportPath { get; set; }
        [StringLength(500)]
        public string PictureReportPath { get; set; }
        [Column("CU_ProductCategory")]
        public int? CuProductCategory { get; set; }
        [StringLength(100)]
        public string ExternalReportNo { get; set; }
        [StringLength(50)]
        public string AuditStartTime { get; set; }
        [StringLength(50)]
        public string AuditEndTime { get; set; }
        [StringLength(100)]
        public string MainCategory { get; set; }
        [StringLength(100)]
        public string OtherCategory { get; set; }
        [StringLength(200)]
        public string Market { get; set; }
        [StringLength(100)]
        public string FillingValidatedFirstTime { get; set; }
        [StringLength(100)]
        public string ReviewValidatedFirstTime { get; set; }
        [StringLength(100)]
        public string LastAuditScore { get; set; }

        [ForeignKey("AuditTypeId")]
        [InverseProperty("AudTransactions")]
        public virtual AudType AuditType { get; set; }
        [ForeignKey("BrandId")]
        [InverseProperty("AudTransactions")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTransactions")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuProductCategory")]
        [InverseProperty("AudTransactions")]
        public virtual AudCuProductCategory CuProductCategoryNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("AudTransactions")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("AudTransactions")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("AudTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("EvalutionId")]
        [InverseProperty("AudTransactions")]
        public virtual AudEvaluationRound Evalution { get; set; }
        [ForeignKey("FactoryId")]
        [InverseProperty("AudTransactionFactories")]
        public virtual SuSupplier Factory { get; set; }
        [ForeignKey("FbfillingStatus")]
        [InverseProperty("AudTransactionFbfillingStatusNavigations")]
        public virtual FbStatus FbfillingStatusNavigation { get; set; }
        [ForeignKey("FbreportStatus")]
        [InverseProperty("AudTransactionFbreportStatusNavigations")]
        public virtual FbStatus FbreportStatusNavigation { get; set; }
        [ForeignKey("FbreviewStatus")]
        [InverseProperty("AudTransactionFbreviewStatusNavigations")]
        public virtual FbStatus FbreviewStatusNavigation { get; set; }
        [ForeignKey("FillingStatus")]
        [InverseProperty("AudTransactionFillingStatusNavigations")]
        public virtual FbStatus FillingStatusNavigation { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("AudTransactions")]
        public virtual RefLocation Office { get; set; }
        [ForeignKey("SeasonId")]
        [InverseProperty("AudTransactions")]
        public virtual RefSeason Season { get; set; }
        [ForeignKey("SeasonYearId")]
        [InverseProperty("AudTransactions")]
        public virtual RefSeasonYear SeasonYear { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("AudTransactions")]
        public virtual AudStatus Status { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("AudTransactionSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudFbReportCheckpoint> AudFbReportCheckpoints { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranAuditor> AudTranAuditors { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranC> AudTranCS { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranCancelReschedule> AudTranCancelReschedules { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranCuContact> AudTranCuContacts { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranFaContact> AudTranFaContacts { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranFaProfile> AudTranFaProfiles { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranFileAttachment> AudTranFileAttachments { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranReport1> AudTranReport1S { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranReportDetail> AudTranReportDetails { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranReport> AudTranReports { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranServiceType> AudTranServiceTypes { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranStatusLog> AudTranStatusLogs { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranSuContact> AudTranSuContacts { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<AudTranWorkProcess> AudTranWorkProcesses { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<CompComplaint> CompComplaints { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<EcExpenseClaimsAudit> EcExpenseClaimsAudits { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<EsTranFile> EsTranFiles { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<InvAutTranStatusLog> InvAutTranStatusLogs { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<InvExfTranStatusLog> InvExfTranStatusLogs { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("Audit")]
        public virtual ICollection<LogBookingReportEmailQueue> LogBookingReportEmailQueues { get; set; }
        [InverseProperty("Booking")]
        public virtual ICollection<QuQuotationAudManday> QuQuotationAudMandays { get; set; }
        [InverseProperty("IdBookingNavigation")]
        public virtual ICollection<QuQuotationAudit> QuQuotationAudits { get; set; }
    }
}