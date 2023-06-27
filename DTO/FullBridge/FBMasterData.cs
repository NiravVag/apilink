using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.FullBridge
{
    public class FBMasterData
    {
    }

    public static class FBConstants
    {
        public const string ReviewerRole = "14";
        public const string InspectorRole = "15";
        public const string Active = "active";
        public const string Classification_Reporter = "main_reporter";
        public const string Additional_Reporter = "additional_reporter";
        public const string Classification_Reviewer = "reviewer";
        public const string Status_Draft = "draft";
        public const string Status_Completed = "completed";
    }

    public class FbReportDataRequest : FbStatusRequest
    {
        public int FbReportId { get; set; }
        public string MissionTitle { get; set; }
        public string ReportTitle { get; set; }
        public string MainProductPhoto { get; set; }
        public string FinalReportPath { get; set; }
        public string InspectionFromDate { get; set; }
        public string InspectionToDate { get; set; }
        public string OverAllResult { get; set; }
        public string ReportSummaryLink { get; set; }
        public string ProductionStatus { get; set; }
        public string PackingStatus { get; set; }
        public string FactoryCoordinates { get; set; }
        public string FactoryGps { get; set; }
        public string PicturesReportPath { get; set; }
        public string MainObservations { get; set; }
        public FbDefectMax DefectMax { get; set; }
        public FbAql Aql { get; set; }
        public List<FbReportInspectionSummary> InspectionSummary { get; set; }
        public List<FbQuantityInfo> QuantityDetails { get; set; }
        public List<FbDefectInfo> Defects { get; set; }
        public List<int?> QcList { get; set; }
        public List<int?> ReviewerList { get; set; }
        public List<FbReportAdditionalPicture> AdditionalPictures { get; set; }
        public List<FbProductInfo> ProductInfo { get; set; }
        public List<FbProductDimention> ProductDimensions { get; set; }
        public List<FbProductWeight> ProductWeight { get; set; }
        public List<FbPackingDimention> PackingDimensions { get; set; }
        public List<FbPackingWeight> PackingWeight { get; set; }
        public List<FbProductPackingInfo> ProductPackingInfo { get; set; }
        public List<FbProductBatteriesInfo> ProductBatteriesInfo { get; set; }
        public List<FbSamplePickings> SamplePickings { get; set; }
        public List<FbSampleTypes> SampleTypes { get; set; }
        public List<FbOtherInformations> OtherInformations { get; set; }
        public List<FbProductBarcodeInfo> ProductBarcodesInfo { get; set; }
        public int? ReportVersion { get; set; }
        public int? ReportRevision { get; set; }
        public List<string> ControlMadeWith { get; set; }
        public double? NumberRollsPresented { get; set; }
        public double? NumberLotsPresented { get; set; }
        public double? ProducedQtyRoll { get; set; }
        public double? PresentedQtyRoll { get; set; }
        public double? InspectedQtyRoll { get; set; }
        public double? AcceptedQtyRoll { get; set; }
        public double? RejectedQtyRoll { get; set; }
        public string SpeedFabricMachine { get; set; }
        public string TypeOfFabric { get; set; }
        public string TypeOfCheck { get; set; }
        public string TypeOfFactory { get; set; }
        public DateTime? InspectionStartedDate { get; set; }
        public string InspectionStartedTime { get; set; }
        public DateTime? InspectionSubmittedDate { get; set; }
        public string InspectionSubmittedTime { get; set; }
        public int? QtyInspected { get; set; }
        public string ProductCategory { get; set; }
        public string KeyStyleHighRisk { get; set; }
        public string MasterCartonPackedQuantityCtns { get; set; }
        public string Region { get; set; }
        public string InspectionDurationMins { get; set; }
        public int? NumberPOMMeasured { get; set; }
        public DACorrelation DACorrelation { get; set; }
        public FactoryTour FactoryTour { get; set; }
        public List<FbReportRDNumber> RDNumbers { get; set; }
        public PackingPackagingLabelling PackingPackagingLabelling { get; set; }
        public List<QualityPlan> QualityPlans { get; set; }
        public Product Product { get; set; }
        public string FillingValidatedFirstTime { get; set; }
        public string ReviewValidatedFirstTime { get; set; }
        public string DACorrelationDone { get; set; }
        public string FactoryTourDone { get; set; }
        public string ExternalReportNumber { get; set; }
        public FbAuditDetails Audit { get; set; }
        public List<EvaluationDetails> Evaluation { get; set; }        
        public string AuditStartedDate { get; set; }
        public string AuditStartedTime { get; set; }
        public string AuditSubmittedDate { get; set; }
        public string AuditSubmittedTime { get; set; }
        public string MainCategory { get; set; }
        public string OtherCategory { get; set; }
        public string Market { get; set; }
        public string LastAuditScore { get; set; }
        public string ReportType { get; set; }
        public string Origin { get; set; }
        public string ShipMode { get; set; }
        public string Score { get; set; }
        public string Grade { get; set; }        

    }

    public enum FbReportPackageType
    {
        Workmanship = 1,
        Packing = 2,
        Measurement = 3
    }

    public class Product
    {
        public int? SampleSize { get; set; }
        public TotalDefects TotalDefects { get; set; }
        public int? TotalDefectiveUnits { get; set; }
        public int? CartonQty { get; set; }
        public List<Defect> Defects { get; set; }
    }

    public class FbProductInfo
    {
        public int? ProductId { get; set; }
        public string Remarks { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Subcategory2 { get; set; }
        public string CustomerRemarkCode { get; set; }
    }

    public class FbProductDimention
    {
        public int? ProductId { get; set; }
        public string SpecClientValuesL { get; set; }
        public string SpecClientValuesW { get; set; }
        public string SpecClientValuesH { get; set; }
        public string DimensionPackValuesL { get; set; }
        public string DimensionPackValuesW { get; set; }
        public string DimensionPackValuesH { get; set; }
        public string Tolerance { get; set; }
        public double? NoPcs { get; set; }
        public string MeasuredValuesL { get; set; }
        public string MeasuredValuesW { get; set; }
        public string MeasuredValuesH { get; set; }
        public string DiscrepancyToSpec { get; set; }
        public string DiscrepancyToPack { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
    }

    public class FbProductWeight
    {
        public int? ProductId { get; set; }
        public string SpecClientValues { get; set; }
        public string WeightPackValues { get; set; }
        public string Tolerance { get; set; }
        public double? NoPcs { get; set; }
        public string MeasuredValues { get; set; }
        public string DiscrepancyToSpec { get; set; }
        public string DiscrepancyToPack { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
    }

    public class FbPackingDimention
    {
        public int? ProductId { get; set; }
        public string PackingPackaging { get; set; }//packaging type
        public string SpecClientValuesL { get; set; }
        public string SpecClientValuesW { get; set; }
        public string SpecClientValuesH { get; set; }
        public string DimensionPackValuesL { get; set; }//printedPackValuesL
        public string DimensionPackValuesW { get; set; }//printedPackValuesW
        public string DimensionPackValuesH { get; set; }//printedPackValuesH
        public string Tolerance { get; set; }
        public double? NoPcs { get; set; }
        public string MeasuredValuesL { get; set; }
        public string MeasuredValuesW { get; set; }
        public string MeasuredValuesH { get; set; }
        public string DiscrepancyToSpec { get; set; }
        public string DiscrepancyToPacking { get; set; }
        public string Unit { get; set; }
    }

    public class FbPackingWeight
    {
        public int? ProductId { get; set; }
        public string SpecClientValues { get; set; }
        public string WeightPackInfoValues { get; set; }
        public string Tolerance { get; set; }
        public double? NoPcs { get; set; }
        public string MeasuredValues { get; set; }
        public string DiscrepancyToSpec { get; set; }
        public string DiscrepancyToPacking { get; set; }
        public string Result { get; set; }
        public string PackingPackaging { get; set; }
        public string Unit { get; set; }
    }

    public class FbProductPackingInfo
    {
        public int? ProductId { get; set; }
        public string MaterialType { get; set; }
        public string PackagingDesc { get; set; }
        public double? PieceNo { get; set; }
        public string Quantity { get; set; }
        public string Location { get; set; }
        public string NetWeightPerQty { get; set; }
    }

    public class FbProductBatteriesInfo
    {
        public int? ProductId { get; set; }
        public string BatteryType { get; set; }
        public string BatteryModel { get; set; }
        public string Quantity { get; set; }
        public string Location { get; set; }
        public string NetWeightPerQty { get; set; }
    }

    public class FbSamplePickings
    {
        public int? ProductId { get; set; }
        public string SampleType { get; set; }
        public string Destination { get; set; }
        public string Quantity { get; set; }
        public string Comments { get; set; }
    }

    public class FbSampleTypes
    {
        public int? ProductId { get; set; }
        public string SampleType { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Comments { get; set; }
    }

    public class FbOtherInformations
    {
        public int? ProductId { get; set; }
        public string SubCategory { get; set; }
        public string SubCategory2 { get; set; }
        public string Remarks { get; set; }
        public string Result { get; set; }
    }

    public class FbProductBarcodeInfo
    {
        public int? ProductId { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
    }


    public class FbDefectMax
    {
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
    }

    public class FbAql
    {
        public int Sample_Size { get; set; }
        public string AQL_Level { get; set; }
        public int Found_Critical { get; set; }
        public int Found_Major { get; set; }
        public int Found_Minor { get; set; }
        public string Aql_Critical { get; set; }
        public string Aql_Major { get; set; }
        public string Aql_Minor { get; set; }
    }

    public class FbQuantityInfo
    {
        public int ProductId { get; set; }
        public string Pono { get; set; }
        public string Color { get; set; }
        public double? InspectedQuantity { get; set; }
        public double? ShipmentQuantity { get; set; }
        public double? OrderQuantity { get; set; }
        public double? PresentedQuantity { get; set; }
        public int? ContainerId { get; set; }
        public string ContainerSize { get; set; }
        public string ProductionStatus { get; set; }
        public string PackingStatus { get; set; }
        public double? TotalUnits { get; set; }
        public string TotalCartons { get; set; }
        public string FinishedPackedUnits { get; set; }
        public string FinishedUnpackedUnits { get; set; }
        public string NotFinishedUnits { get; set; }
        public string SelectCtnQty { get; set; }
        public string SelectCtnNO { get; set; }
        public double? Points100Sqy { get; set; }
        public double? AcceptanceCriteria { get; set; }
        public double? ProducedQuantity { get; set; }
        public string OverLessProducedQty { get; set; }
        public double? RejectedQuantity { get; set; }
        public double? RejectedRolls { get; set; }
        public double? DemeritPts { get; set; }
        public double? Tolerance { get; set; }
        public string Rating { get; set; }
    }

    public class FbDefectInfo
    {
        public int ProductId { get; set; }
        public string Pono { get; set; }
        public string Color { get; set; }
        public int? ContainerId { get; set; }
        public List<FbDefect> DefectDetails { get; set; }
        public string LengthUnit { get; set; }
        public string DyeLot { get; set; }
        public string RollNumber { get; set; }
        public string Result { get; set; }
        public string AcceptanceCriteria { get; set; }
        public string Points100Sqy { get; set; }
        public string LengthOriginal { get; set; }
        public string LengthActual { get; set; }
        public string WeightOriginal { get; set; }
        public string WeightActual { get; set; }
        public string WidthOriginal { get; set; }
        public string WidthActual { get; set; }
    }

    public class FbDefect
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public FbDefectClassification DefectFound { get; set; }
        public string Position { get; set; }
        public string Code { get; set; }
        public string Zone { get; set; }
        public string Size { get; set; }
        public string Reparability { get; set; }
        public string Garment_grade { get; set; }
        public List<FbDefectPicture> Pictures { get; set; }
        public string Location { get; set; }
        public string Point { get; set; }
        public string DefectInfo { get; set; }
    }

    public class FbDefectPicture
    {
        public string Path { get; set; }
        public string Description { get; set; }
    }

    public class FbReportInspectionSummary
    {
        public string Name { get; set; }
        public string Result { get; set; }
        //public string Remarks { get; set; }
        public int? Sort { get; set; }
        public List<FbReportInspectionSummarySub> SubCheckPoints { get; set; }
        public List<string> Photos { get; set; }
        public List<FbReportInspectionSummaryPhoto> Pictures { get; set; }
        public List<FbReportProblematicRemarks> Remarks { get; set; }
        public string ScoreValue { get; set; }
        public string ScorePercentage { get; set; }
    }

    public class FbReportInspectionSummaryPhoto
    {
        public int? ProductId { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
    }

    public class FbReportAdditionalPicture
    {
        public int? ProductId { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
    }

    public class FbReportInspectionSummarySub
    {
        public string Name { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }
        public int? Sort { get; set; }
        public List<FbReportInspectionSummarySub> SubCheckPoints { get; set; }
        public List<string> Photos { get; set; }
        public List<FbReportInspectionSummaryPhoto> Pictures { get; set; }
    }

    public class FbReportProblematicRemarks
    {
        public int? ProductId { get; set; }
        public string Remarks { get; set; }
        public string Result { get; set; }
        public string SubCategory { get; set; }
        public string SubCategory2 { get; set; }
        public string CustomerRemarkCode { get; set; }
    }

    public enum FbReportInspectionSummaryType
    {
        Main = 1,
        Sub = 2
    }

    public class FbDefectClassification
    {
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? Qty_Reworked { get; set; }
        public int? Qty_Replaced { get; set; }
        public int? Qty_Rejected { get; set; }
    }

    public class FbStatusRequest
    {
        public string FillingStatus { get; set; }
        public string ReviewStatus { get; set; }
        public string ReportStatus { get; set; }
        public string MissionStatus { get; set; }
        public int? ServiceId { get; set; }
    }

    public class FbStatusResponse
    {
        public int ReportId { get; set; }
        public bool IsNewReportFormatCheckPoint { get; set; }
        public int? InspectionId { get; set; }
        public int? ServiceId { get; set; }
        public int? EntityId { get; set; }
        public FbStatusResponseResult Result { get; set; }
    }

    public enum FbStatusResponseResult
    {
        success = 1,
        failure = 2,
        WrongServiceType = 3
    }

    public enum FbStatusResult
    {
        Success = 1,
        StatusUpdateFailure = 2,
        ReportIdNotExist = 3,
    }

    public enum FBFailure
    {
        CustomerSave = 1,
        SupplierSave = 2,
        FactorySave = 3,
        ProductSave = 4,
        MissionSave = 5,
        UserSave = 6,
        ReportSave = 7,
        ReportProcessDone = 8,
        MissionCompleted = 9,
        MissionUrl = 10,
        CustomerNotFound = 11,
        SupplierNotFound = 12,
        FactoryNotFound = 13,
        DeleteMission = 14,
        InspectionDataNotFound = 15,
        AuditDataNotFound = 16,
    }

    public class LogFbBookingRequest
    {
        public int? BookingId { get; set; }
        public int? MissionId { get; set; }
        public int ServiceId { get; set; }
    }

    public class FBUserAccountData
    {
        public bool client { get; set; }
        public bool factory { get; set; }
        public bool vendor { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string reference { get; set; }
        public string status { get; set; }
        public int? country { get; set; }
        public int? countrySubdivision { get; set; }
        public string city { get; set; }
        public string cn_address { get; set; }
    }

    public class FBProductData
    {
        public string title { get; set; }
        public string reference { get; set; }
        public int productCategory { get; set; }
        public int sampleSizePerManday { get; set; }
        public string status { get; set; }
        public int? client { get; set; }
        public int? factory { get; set; }
        public int? vendor { get; set; }
        public string measurementChart { get; set; }
    }

    public class FBMissionProduct
    {
        public int? product { get; set; }
        public int quantity { get; set; }
        public int sampleSizePerManday { get; set; }
        public int purchaseOrderNumber { get; set; }
        public bool createReport { get; set; }
    }

    public class FBMissionProductReportUser
    {
        public int? product { get; set; }
        public int quantity { get; set; }
        public string purchaseOrderNumber { get; set; }
        public string color { get; set; }
        public string aqlLevel { get; set; }
        public string aqlCritical { get; set; }
        public string aqlMajor { get; set; }
        public string aqlMinor { get; set; }
        public string customerReferenceNo { get; set; }
        public int? containerId { get; set; }
        public int? addTemplate { get; set; }
        public bool createReport { get; set; }
        public bool copyUsersFromMission { get; set; }
        public bool isEcopack { get; set; }
        public bool isDisplay { get; set; }
        public string ean { get; set; }
        public string etd { get; set; }
        public string destinationCountry { get; set; }
        public string factoryReference { get; set; }
        public string samplingSizeCustom { get; set; }
        //public string auditProductCategory { get; set; }

    }

    public class FBMissionProducts
    {
        public int? product { get; set; }
        public int quantity { get; set; }
        public string purchaseOrderNumber { get; set; }
        public string customerReferenceNo { get; set; }
        public int? containerId { get; set; }
        public bool addInMission { get; set; }
        public bool isEcopack { get; set; }
        public bool isDisplay { get; set; }
        public int? sampleSizePerManday { get; set; }
        public string ean { get; set; }
        public string etd { get; set; }
        public string destinationCountry { get; set; }
        public string factoryReference { get; set; }
        public string color { get; set; }
        // public string samplingSizeCustom { get; set; }
    }

    public class FBMissionData
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int client { get; set; }
        public int factory { get; set; }
        public int vendor { get; set; }
        public int? service { get; set; }
        public string reference { get; set; }
        public string qcResponsible { get; set; }
        public string qc_comment { get; set; }
        public string purchaseOrderNumber { get; set; }
        public string inspectionOffice { get; set; }
        public string destinationCountry { get; set; }
    }

    public class FBMissionPatchRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? service { get; set; }
        public int client { get; set; }
        public int factory { get; set; }
        public int vendor { get; set; }
        public string qc_comment { get; set; }
        public string inspectionOffice { get; set; }
        public string destinationCountry { get; set; }
    }
    public class FBMissionPatchClient
    {
        public int client { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? service { get; set; }
    }

    public class FBMissionPatchFactory
    {
        public int factory { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? service { get; set; }
    }

    public class FBMissionPatchSupplier
    {
        public int vendor { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? service { get; set; }
    }

    public class FBUserData
    {
        public int account { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string status { get; set; }
    }

    public class FBMapUserReport
    {
        public string user { get; set; }
        public string classification { get; set; }
        public string status { get; set; }
        public bool addInReports { get; set; }
    }

    public class FBMapProductReport
    {
        public int? product { get; set; }
        public int? poDetailId { get; set; }
        public int quantity { get; set; }
        public int? addTemplate { get; set; }
        public string poNumber { get; set; }
        public string customerReferenceNo { get; set; }
        public int? containerId { get; set; }
        public string aqlLevel { get; set; }
        public string aqlCritical { get; set; }
        public string aqlMajor { get; set; }
        public string aqlMinor { get; set; }
        public bool createReport { get; set; }
        public bool copyUsersFromMission { get; set; }
        public bool isEcopack { get; set; }
        public bool isDisplay { get; set; }
        public string ean { get; set; }
        public string etd { get; set; }
        public string destinationCountry { get; set; }
        public string factoryReference { get; set; }
        public string samplingSizeCustom { get; set; }
        public string color { get; set; }
    }

    public class FBReportProductMap
    {
        public int? product { get; set; }
        public int quantity { get; set; }
        public string purchaseOrderNumber { get; set; }
        public string customerReferenceNo { get; set; }
        public int? containerId { get; set; }
    }

    public class FBDeleteUserFromReport
    {
        public bool deleteInReports { get; set; }
    }

    public class FBDeleteProductFromMission
    {
        public bool deleteReport { get; set; }
    }

    public class FBMapUserAccounts
    {
        public string account { get; set; }
        public string classification { get; set; }
        public string status { get; set; }
    }

    public class FBProductCategory
    {
        public string title { get; set; }
        public string status { get; set; }
    }

    public class FBProductSubCategory
    {
        public string title { get; set; }
        public int parent { get; set; }
        public string status { get; set; }
    }

    public class FBSettings
    {
        public string BaseUrl { get; set; }
        public string UserRequestUrl { get; set; }
        public string AccountRequestUrl { get; set; }
        public string MissionRequestUrl { get; set; }
        public string MissionPatchRequestUrl { get; set; }
        public string ProductRequestUrl { get; set; }
        public string ProductUpdateUrl { get; set; }
        public string ReportRequestUrl { get; set; }
        public string MissionReportRequestUrl { get; set; }
        public string ReportUserRequestUrl { get; set; }
        public string MissionDeleteRequestUrl { get; set; }
        public string UserFBAccontRequestUrl { get; set; }
        public string ProductFBReportRequestUrl { get; set; }
        public string ReportDeleteRequestUrl { get; set; }
        public string AccountDeleteRequestUrl { get; set; }
        public string AccountUpdateRequestUrl { get; set; }
        public string ReportUrl { get; set; }
        public string ReportsUrl { get; set; }
        public string MissionUrl { get; set; }
        public string ReportInfo { get; set; }
        public string ProductCategoryUrl { get; set; }
        public string ProductCategoryUpdateUrl { get; set; }
        public string MissionUrlRequestUrl { get; set; }
        public string MissionUrlDeleteUrl { get; set; }
        public string FbReportDataRequestUrl { get; set; }
        public string ReportUpdateUrl { get; set; }
        public string MeasurementRequestUrl { get; set; }
    }

    public class SaveMissionResponse
    {
        public SaveMissionResponseResult Result { get; set; }
    }

    public enum SaveMissionResponseResult
    {
        Success = 1,
        Failure = 2,
        FBReportAlreadyProcessed = 3,
        MissionCompleted = 4
    }


    public class DeleteReportResponse
    {
        public DeleteReportResponseResult Result { get; set; }
    }

    public enum DeleteReportResponseResult
    {
        Success = 1,
        Failure = 2,
        ReportFilledByQC = 3,
        MissionNotExist = 4
    }

    public class UpdateReportResponse
    {
        public UpdateReportResponseResult Result { get; set; }
        public List<ReportIdData> ReportIds { get; set; }
        public List<ReportIdData> FastReportIds { get; set; }
        public bool IsNewReportFormatCheckPoint { get; set; }
        public int? InspectionId { get; set; }
        public int? EntityId { get; set; }
    }

    public class ReportIdData
    {
        public int FbReportId { get; set; }
        public int? InspectionId { get; set; }
        public int ApiReportId { get; set; }
    }

    public enum UpdateReportResponseResult
    {
        Success = 1,
        ReportIsNotValidated = 2,
        ReportNotExist = 3,
        ReportIdNotValid = 4,
        Failure = 5,
        BookingIdIsNotValid = 6,
        ReportFetchMax = 7,
        ReportSyncShortly = 8
    }

    public class FBBulkReportResponse
    {
        public string Result { get; set; }
    }

    public class ReportProductsAndPo
    {
        public int productId { get; set; }
        public int? fbProductId { get; set; }
        public string poNumber { get; set; }
        public string colorName { get; set; }
        public int poTrnsactionId { get; set; }
        public int? colorTransactionId { get; set; }
        public int? containerRefId { get; set; }
        public int? reportId { get; set; }

        public int productTransactionId { get; set; }
    }

    public class FBProductMasterData
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string MsChartFileUrl { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductSub2CategoryId { get; set; }
        public int? FBCustomerId { get; set; }
        public int? FBProducId { get; set; }

    }

    public class FBCustomerMasterData
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CityName { get; set; }
        public int? FbCountryId { get; set; }
        public int? FbCusId { get; set; }
    }

    public class CustomerPriceData
    {
        public int CustomerId { get; set; }
        public string CountryName { get; set; }
        public string CustomerSegment { get; set; }
    }

    public class FBSupplierMasterData
    {
        public int SupplierId { get; set; }
        public int? TypeId { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string RegionalAddress { get; set; }
        public int? FbCountryId { get; set; }
        public int? FbProvinceId { get; set; }
        public string CityName { get; set; }
        public int? FbFactSupId { get; set; }
    }

    public class FBMissionUrlData
    {
        public string title { get; set; }
        public string url { get; set; }
        public string classification { get; set; }
        public string description { get; set; }
    }

    public class FBMissionUrlClassification
    {
        public const string Booking = "booking";
        public const string MissionAttachment = "mission_attachment";
        public const string GeneralInstructions = "general_instructions";
    }

    public class UpdateFbReportData
    {
        public string external_url { get; set; }
    }

    public class UpdateFbReportRevision
    {
        public int revision { get; set; }
    }

    public class FastReportRequest
    {
        public List<int> ReportIds { get; set; }
        public int? BookingId { get; set; }
        public int? EntityId { get; set; }

    }

    public class FbReportRDNumber
    {
        public int ProductId { get; set; }
        public string Color { get; set; }
        public string Pono { get; set; }
        public string RDNumber { get; set; }
    }

    public class FactoryTour
    {
        public string Result { get; set; }
        public string BottleneckProductionStage { get; set; }
        public string NotConductedReason { get; set; }
        public string IrregularitiesIdentified { get; set; }
    }

    public class MeasurementDefectsPOM
    {
        public string codePOM { get; set; }
        public string POM { get; set; }
        public string CriticalPOM { get; set; }
        public int? Quantity { get; set; }
        public string SpecZone { get; set; }
    }

    public class MeasurementDefectsSize
    {
        public string Size { get; set; }
        public int? Quantity { get; set; }
    }

    public class DACorrelation
    {
        public string DAEmail { get; set; }
        public int? InspectionSampling { get; set; }
        public string CorrelationRate { get; set; }
        public string Result { get; set; }
    }

    public class Defect
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Classification { get; set; }
        public string Severity { get; set; }
        public int? Quantity { get; set; }
        public string RDNumber { get; set; }
        public List<Picture> Pictures { get; set; }
    }

    public class Picture
    {
        public string Description { get; set; }
        public string Path { get; set; }
    }

    public class TotalDefects
    {
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
    }

    public class PackingPackagingLabelling
    {
        public int? SampleSizeCtns { get; set; }
        public TotalDefects TotalDefects { get; set; }
        public List<Defect> Defects { get; set; }
        public int? CartonQty { get; set; }
    }

    public class QualityPlan
    {
        public string Title { get; set; }
        public int? TotalDefectiveUnits { get; set; }
        public string Result { get; set; }
        public int? TotalQtyMeasurmentDefects { get; set; }
        public List<MeasurementDefectsPOM> MeasurementDefectsPOM { get; set; }
        public List<MeasurementDefectsSize> MeasurementDefectsSize { get; set; }
        public string TotalPiecesMeasurmentDefects { get; set; }
        public string SampleInspected { get; set; }
        public string ActualMeasuredSampleSize { get; set; }
    }
    public class SaveMissionRequest
    {
        public InspTransaction Inspection { get; set; }
        public AudTransaction Audit { get; set; }
        public int ServiceType { get; set; }
        public int? FbMissionId { get; set; }
    }
    public class ReportRequest
    {
        public int? FbMissionId { get; set; }
    }
    public class FbBookingRequest
    {
        public Guid Id { get; set; }
        public int TryCount { get; set; }
        public int ResultId { get; set; }
    }
    public enum FbBookingQueueStatus
    {
        NotStarted = 1,
        Successs = 2,
        Failure = 3
    }
    public enum FbBookingSyncType
    {
        AuditCreation = 1,
        AuditUpdation = 2,
        AuditBookingCancellation = 3,
        InspectionCreation = 4,
        InspectionUpdation = 5,
        InspectionBookingCancellation = 6
    }

    public class FbAuditData
    {
        public int? FbCustomerId { get; set; }
        public int? FbSupplierId { get; set; }
        public int? FbFactoryId { get; set; }
        public int? FbServiceId { get; set; }
        public DateTime ServiceFromDate { get; set; }
        public DateTime ServiceToDate { get; set; }
        public int AuditId { get; set; }
        public string Office { get; set; }
        public string AuditType { get; set; }
        public string Evalution { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
    }

    public class FbAuditDetails
    {
        public string ScoreValue { get; set; }
        public string Scorepercentage { get; set; }
        public string Grade { get; set; }
        public string Critical { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ZeroTolerance { get; set; }
    }
    public class EvaluationDetails
    {
        public string Title { get; set; }
        public string ScoreValue { get; set; }
        public string ScorePercentage { get; set; }
        public string MaxPoints { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ZeroTolerance { get; set; }
        public string Remarks { get; set; }
        public string Critical { get; set; }
        public string Grade { get; set; }
    }
    public class AudTranCSDetail
    {
        public int StaffId { get; set; }
        public string PersonName { get; set; }
        public string CompanyEmail { get; set; }
        public IEnumerable<ItUserMaster> ItUserMasters { get; set; }
    }
    public class FBProductMschartData
    {
        public string code { get; set; }
        public string description { get; set; }
        public string requiredValue { get; set; }
        public string size { get; set; }
        public string tolerancePlus { get; set; }
        public string toleranceMinus { get; set; }
    }
}