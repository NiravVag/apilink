using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Details")]
    public partial class FbReportDetail
    {
        public FbReportDetail()
        {
            ClmTranReports = new HashSet<ClmTranReport>();
            EsTranFiles = new HashSet<EsTranFile>();
            FbReportAdditionalPhotos = new HashSet<FbReportAdditionalPhoto>();
            FbReportComments = new HashSet<FbReportComment>();
            FbReportFabricControlmadeWiths = new HashSet<FbReportFabricControlmadeWith>();
            FbReportFabricDefects = new HashSet<FbReportFabricDefect>();
            FbReportInspDefects = new HashSet<FbReportInspDefect>();
            FbReportInspSummaries = new HashSet<FbReportInspSummary>();
            FbReportManualLogs = new HashSet<FbReportManualLog>();
            FbReportOtherInformations = new HashSet<FbReportOtherInformation>();
            FbReportPackingBatteryInfos = new HashSet<FbReportPackingBatteryInfo>();
            FbReportPackingDimentions = new HashSet<FbReportPackingDimention>();
            FbReportPackingInfos = new HashSet<FbReportPackingInfo>();
            FbReportPackingPackagingLabellingProducts = new HashSet<FbReportPackingPackagingLabellingProduct>();
            FbReportPackingWeights = new HashSet<FbReportPackingWeight>();
            FbReportProductBarcodesInfos = new HashSet<FbReportProductBarcodesInfo>();
            FbReportProductDimentions = new HashSet<FbReportProductDimention>();
            FbReportProductWeights = new HashSet<FbReportProductWeight>();
            FbReportQcdetails = new HashSet<FbReportQcdetail>();            
            FbReportQuantityDetails = new HashSet<FbReportQuantityDetail>();
            FbReportRdnumbers = new HashSet<FbReportRdnumber>();
            FbReportReviewers = new HashSet<FbReportReviewer>();
            FbReportSamplePickings = new HashSet<FbReportSamplePicking>();
            FbReportSampleTypes = new HashSet<FbReportSampleType>();
            InspContainerTransactions = new HashSet<InspContainerTransaction>();
            InspProductTransactions = new HashSet<InspProductTransaction>();
            InspRepCusDecisions = new HashSet<InspRepCusDecision>();
            JobScheduleLogs = new HashSet<JobScheduleLog>();
            LogBookingReportEmailQueues = new HashSet<LogBookingReportEmailQueue>();
            RepFastTranLogs = new HashSet<RepFastTranLog>();
            RepFastTransactions = new HashSet<RepFastTransaction>();
        }

        public int Id { get; set; }
        [StringLength(1500)]
        public string MissionTitle { get; set; }
        [StringLength(1500)]
        public string ReportTitle { get; set; }
        [StringLength(1000)]
        public string MainProductPhoto { get; set; }
        [StringLength(1000)]
        public string FinalReportPath { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceFromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceToDate { get; set; }
        [Column("Fb_Filling_Status")]
        public int? FbFillingStatus { get; set; }
        [Column("Fb_Review_Status")]
        public int? FbReviewStatus { get; set; }
        [Column("Fb_Report_Status")]
        public int? FbReportStatus { get; set; }
        [StringLength(1500)]
        public string OverAllResult { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int? ResultId { get; set; }
        [StringLength(4000)]
        public string ReportSummaryLink { get; set; }
        [StringLength(100)]
        public string InspectionStartTime { get; set; }
        [StringLength(100)]
        public string InspectionEndTime { get; set; }
        public int? CriticalMax { get; set; }
        public int? MajorMax { get; set; }
        public int? MinorMax { get; set; }
        public double? ProductionStatus { get; set; }
        public double? PackingStatus { get; set; }
        [Column("FB_Report_Map_Id")]
        public int? FbReportMapId { get; set; }
        [Column("Inspection_Id")]
        public int? InspectionId { get; set; }
        [StringLength(1000)]
        public string FinalManualReportPath { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column("Aql_Level")]
        public int? AqlLevel { get; set; }
        public int? SampleSize { get; set; }
        [Column("Found_Critical")]
        public int? FoundCritical { get; set; }
        [Column("Found_Major")]
        public int? FoundMajor { get; set; }
        [Column("Found_Minor")]
        public int? FoundMinor { get; set; }
        [Column("Aql_Critical")]
        public double? AqlCritical { get; set; }
        [Column("Aql_Major")]
        public double? AqlMajor { get; set; }
        [Column("Aql_Minor")]
        public double? AqlMinor { get; set; }
        public double? OrderQty { get; set; }
        public double? InspectedQty { get; set; }
        public double? PresentedQty { get; set; }
        [Column("Report_Picture_Path")]
        [StringLength(1000)]
        public string ReportPicturePath { get; set; }
        [Column("Main_Observations")]
        public string MainObservations { get; set; }
        public int? ReportVersion { get; set; }
        public int? ReportRevision { get; set; }
        public int? RequestedReportRevision { get; set; }
        [Column("Fabric_NoOfRollsPresented")]
        public double? FabricNoOfRollsPresented { get; set; }
        [Column("Fabric_NoOfLotsPresented")]
        public double? FabricNoOfLotsPresented { get; set; }
        [Column("Fabric_ProducedQtyRoll")]
        public double? FabricProducedQtyRoll { get; set; }
        [Column("Fabric_PresentedQtyRoll")]
        public double? FabricPresentedQtyRoll { get; set; }
        [Column("Fabric_InspectedQtyRoll")]
        public double? FabricInspectedQtyRoll { get; set; }
        [Column("Fabric_AcceptedQtyRoll")]
        public double? FabricAcceptedQtyRoll { get; set; }
        [Column("Fabric_RejectedQtyRoll")]
        public double? FabricRejectedQtyRoll { get; set; }
        [Column("Fabric_MachineSpeed")]
        [StringLength(200)]
        public string FabricMachineSpeed { get; set; }
        [Column("Fabric_Type")]
        [StringLength(200)]
        public string FabricType { get; set; }
        [Column("Fabric_TypeCheck")]
        [StringLength(200)]
        public string FabricTypeCheck { get; set; }
        [Column("Fabric_factoryType")]
        [StringLength(200)]
        public string FabricFactoryType { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InspectionStartedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InspectionSubmittedDate { get; set; }
        public int? QtyInspected { get; set; }
        [StringLength(500)]
        public string ProductCategory { get; set; }
        [StringLength(500)]
        public string KeyStyleHighRisk { get; set; }
        [StringLength(500)]
        public string MasterCartonPackedQuantityCtns { get; set; }
        [StringLength(500)]
        public string Region { get; set; }
        [StringLength(500)]
        public string InspectionDurationMins { get; set; }
        [Column("NumberPOMMeasured")]
        public int? NumberPommeasured { get; set; }
        [Column("DACorrelation_Enabled")]
        public bool? DacorrelationEnabled { get; set; }
        [Column("DACorrelationEmail")]
        [StringLength(500)]
        public string DacorrelationEmail { get; set; }
        [Column("DACorrelationInspectionSampling")]
        public int? DacorrelationInspectionSampling { get; set; }
        [Column("DACorrelationRate")]
        [StringLength(500)]
        public string DacorrelationRate { get; set; }
        [Column("DACorrelationResult")]
        [StringLength(500)]
        public string DacorrelationResult { get; set; }
        public bool? FactoryTourEnabled { get; set; }
        [StringLength(500)]
        public string FactoryTourBottleneckProductionStage { get; set; }
        [StringLength(500)]
        public string FactoryTourNotConductedReason { get; set; }
        [StringLength(500)]
        public string FactoryTourIrregularitiesIdentified { get; set; }
        [StringLength(100)]
        public string FillingValidatedFirstTime { get; set; }
        [StringLength(100)]
        public string ReviewValidatedFirstTime { get; set; }
        [Column("DACorrelationDone")]
        [StringLength(100)]
        public string DacorrelationDone { get; set; }
        [StringLength(100)]
        public string FactoryTourDone { get; set; }
        [StringLength(100)]
        public string FactoryTourResult { get; set; }
        [StringLength(200)]
        public string ExternalReportNumber { get; set; }
        [StringLength(200)]
        public string ReportType { get; set; }
        [StringLength(100)]
        public string Origin { get; set; }
        [StringLength(100)]
        public string ShipMode { get; set; }
        [StringLength(200)]
        public string Othercategory { get; set; }
        [StringLength(200)]
        public string Market { get; set; }
        [StringLength(100)]
        public string TotalScore { get; set; }
        [StringLength(100)]
        public string Grade { get; set; }
        [StringLength(100)]
        public string LastAuditScore { get; set; }

        [ForeignKey("AqlLevel")]
        [InverseProperty("FbReportDetails")]
        public virtual RefLevelPick1 AqlLevelNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("FbReportDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("FbReportDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FbFillingStatus")]
        [InverseProperty("FbReportDetailFbFillingStatusNavigations")]
        public virtual FbStatus FbFillingStatusNavigation { get; set; }
        [ForeignKey("FbReportStatus")]
        [InverseProperty("FbReportDetailFbReportStatusNavigations")]
        public virtual FbStatus FbReportStatusNavigation { get; set; }
        [ForeignKey("FbReviewStatus")]
        [InverseProperty("FbReportDetailFbReviewStatusNavigations")]
        public virtual FbStatus FbReviewStatusNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("FbReportDetails")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ResultId")]
        [InverseProperty("FbReportDetails")]
        public virtual FbReportResult Result { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("FbReportDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<ClmTranReport> ClmTranReports { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<EsTranFile> EsTranFiles { get; set; }
        [InverseProperty("FbReportDetail")]
        public virtual ICollection<FbReportAdditionalPhoto> FbReportAdditionalPhotos { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportComment> FbReportComments { get; set; }
        [InverseProperty("ReportDetails")]
        public virtual ICollection<FbReportFabricControlmadeWith> FbReportFabricControlmadeWiths { get; set; }
        [InverseProperty("Fbreportdetails")]
        public virtual ICollection<FbReportFabricDefect> FbReportFabricDefects { get; set; }
        [InverseProperty("FbReportDetail")]
        public virtual ICollection<FbReportInspDefect> FbReportInspDefects { get; set; }
        [InverseProperty("FbReportDetail")]
        public virtual ICollection<FbReportInspSummary> FbReportInspSummaries { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportManualLog> FbReportManualLogs { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportOtherInformation> FbReportOtherInformations { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportPackingBatteryInfo> FbReportPackingBatteryInfos { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportPackingDimention> FbReportPackingDimentions { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportPackingInfo> FbReportPackingInfos { get; set; }
        [InverseProperty("FbReportdetails")]
        public virtual ICollection<FbReportPackingPackagingLabellingProduct> FbReportPackingPackagingLabellingProducts { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportPackingWeight> FbReportPackingWeights { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportProductBarcodesInfo> FbReportProductBarcodesInfos { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportProductDimention> FbReportProductDimentions { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportProductWeight> FbReportProductWeights { get; set; }
        [InverseProperty("FbReportDetail")]
        public virtual ICollection<FbReportQcdetail> FbReportQcdetails { get; set; }    
        [InverseProperty("FbReportDetail")]
        public virtual ICollection<FbReportQuantityDetail> FbReportQuantityDetails { get; set; }
        [InverseProperty("FbReportDetails")]
        public virtual ICollection<FbReportQualityPlan> FbReportQualityPlans { get; set; }
        [InverseProperty("Reportdetails")]
        public virtual ICollection<FbReportRdnumber> FbReportRdnumbers { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportReviewer> FbReportReviewers { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportSamplePicking> FbReportSamplePickings { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<FbReportSampleType> FbReportSampleTypes { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<InspContainerTransaction> InspContainerTransactions { get; set; }
        [InverseProperty("FbReport")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<InspRepCusDecision> InspRepCusDecisions { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<JobScheduleLog> JobScheduleLogs { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<LogBookingReportEmailQueue> LogBookingReportEmailQueues { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<RepFastTranLog> RepFastTranLogs { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<RepFastTransaction> RepFastTransactions { get; set; }
    }
}