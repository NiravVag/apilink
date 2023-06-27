using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Entities;
using Contracts;
namespace DAL
{
    public partial class API_DBContext : DbContext
    {
        public virtual DbSet<ApEntity> ApEntities { get; set; }
        public virtual DbSet<ApModule> ApModules { get; set; }
        public virtual DbSet<ApModuleRole> ApModuleRoles { get; set; }
        public virtual DbSet<ApSubModule> ApSubModules { get; set; }
        public virtual DbSet<ApSubModuleRole> ApSubModuleRoles { get; set; }
        public virtual DbSet<ApigatewayLog> ApigatewayLogs { get; set; }
        public virtual DbSet<AudBookingContact> AudBookingContacts { get; set; }
        public virtual DbSet<AudBookingEmailConfiguration> AudBookingEmailConfigurations { get; set; }
        public virtual DbSet<AudBookingRule> AudBookingRules { get; set; }
        public virtual DbSet<AudCancelRescheduleReason> AudCancelRescheduleReasons { get; set; }
        public virtual DbSet<AudCuProductCategory> AudCuProductCategories { get; set; }
        public virtual DbSet<AudEvaluationRound> AudEvaluationRounds { get; set; }
        public virtual DbSet<AudFbReportCheckpoint> AudFbReportCheckpoints { get; set; }
        public virtual DbSet<AudStatus> AudStatuses { get; set; }
        public virtual DbSet<AudTranAuditor> AudTranAuditors { get; set; }
        public virtual DbSet<AudTranC> AudTranCs { get; set; }
        public virtual DbSet<AudTranCancelReschedule> AudTranCancelReschedules { get; set; }
        public virtual DbSet<AudTranCuContact> AudTranCuContacts { get; set; }
        public virtual DbSet<AudTranFaContact> AudTranFaContacts { get; set; }
        public virtual DbSet<AudTranFaProfile> AudTranFaProfiles { get; set; }
        public virtual DbSet<AudTranFileAttachment> AudTranFileAttachments { get; set; }
        public virtual DbSet<AudTranReport> AudTranReports { get; set; }
        public virtual DbSet<AudTranReport1> AudTranReports1 { get; set; }
        public virtual DbSet<AudTranReportDetail> AudTranReportDetails { get; set; }
        public virtual DbSet<AudTranServiceType> AudTranServiceTypes { get; set; }
        public virtual DbSet<AudTranStatusLog> AudTranStatusLogs { get; set; }
        public virtual DbSet<AudTranSuContact> AudTranSuContacts { get; set; }
        public virtual DbSet<AudTranWorkProcess> AudTranWorkProcesses { get; set; }
        public virtual DbSet<AudTransaction> AudTransactions { get; set; }
        public virtual DbSet<AudType> AudTypes { get; set; }
        public virtual DbSet<AudWorkProcess> AudWorkProcesses { get; set; }
        public virtual DbSet<ClmRefCustomerRequest> ClmRefCustomerRequests { get; set; }
        public virtual DbSet<ClmRefDefectDistribution> ClmRefDefectDistributions { get; set; }
        public virtual DbSet<ClmRefDefectFamily> ClmRefDefectFamilies { get; set; }
        public virtual DbSet<ClmRefDepartment> ClmRefDepartments { get; set; }
        public virtual DbSet<ClmRefFileType> ClmRefFileTypes { get; set; }
        public virtual DbSet<ClmRefFrom> ClmRefFroms { get; set; }
        public virtual DbSet<ClmRefPriority> ClmRefPriorities { get; set; }
        public virtual DbSet<ClmRefReceivedFrom> ClmRefReceivedFroms { get; set; }
        public virtual DbSet<ClmRefRefundType> ClmRefRefundTypes { get; set; }
        public virtual DbSet<ClmRefResult> ClmRefResults { get; set; }
        public virtual DbSet<ClmRefSource> ClmRefSources { get; set; }
        public virtual DbSet<ClmRefStatus> ClmRefStatuses { get; set; }
        public virtual DbSet<ClmTranAttachment> ClmTranAttachments { get; set; }
        public virtual DbSet<ClmTranClaimRefund> ClmTranClaimRefunds { get; set; }
        public virtual DbSet<ClmTranCustomerRequest> ClmTranCustomerRequests { get; set; }
        public virtual DbSet<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefunds { get; set; }
        public virtual DbSet<ClmTranDefectFamily> ClmTranDefectFamilies { get; set; }
        public virtual DbSet<ClmTranDepartment> ClmTranDepartments { get; set; }
        public virtual DbSet<ClmTranFinalDecision> ClmTranFinalDecisions { get; set; }
        public virtual DbSet<ClmTranReport> ClmTranReports { get; set; }
        public virtual DbSet<ClmTransaction> ClmTransactions { get; set; }
        public virtual DbSet<CompComplaint> CompComplaints { get; set; }
        public virtual DbSet<CompRefCategory> CompRefCategories { get; set; }
        public virtual DbSet<CompRefDepartment> CompRefDepartments { get; set; }
        public virtual DbSet<CompRefRecipientType> CompRefRecipientTypes { get; set; }
        public virtual DbSet<CompRefType> CompRefTypes { get; set; }
        public virtual DbSet<CompTranComplaintsDetail> CompTranComplaintsDetails { get; set; }
        public virtual DbSet<CompTranPersonInCharge> CompTranPersonInCharges { get; set; }
        public virtual DbSet<CuAddress> CuAddresses { get; set; }
        public virtual DbSet<CuApiService> CuApiServices { get; set; }
        public virtual DbSet<CuBrand> CuBrands { get; set; }
        public virtual DbSet<CuBrandpriority> CuBrandpriorities { get; set; }
        public virtual DbSet<CuBuyer> CuBuyers { get; set; }
        public virtual DbSet<CuBuyerApiService> CuBuyerApiServices { get; set; }
        public virtual DbSet<CuCheckPoint> CuCheckPoints { get; set; }
        public virtual DbSet<CuCheckPointType> CuCheckPointTypes { get; set; }
        public virtual DbSet<CuCheckPointsBrand> CuCheckPointsBrands { get; set; }
        public virtual DbSet<CuCheckPointsCountry> CuCheckPointsCountries { get; set; }
        public virtual DbSet<CuCheckPointsDepartment> CuCheckPointsDepartments { get; set; }
        public virtual DbSet<CuCheckPointsServiceType> CuCheckPointsServiceTypes { get; set; }
        public virtual DbSet<CuCollection> CuCollections { get; set; }
        public virtual DbSet<CuContact> CuContacts { get; set; }
        public virtual DbSet<CuContactBrand> CuContactBrands { get; set; }        
        public virtual DbSet<CuContactDepartment> CuContactDepartments { get; set; }
        public virtual DbSet<CuContactEntityMap> CuContactEntityMaps { get; set; }
        public virtual DbSet<CuContactEntityServiceMap> CuContactEntityServiceMaps { get; set; }
        public virtual DbSet<CuContactService> CuContactServices { get; set; }
        public virtual DbSet<CuContactSisterCompany> CuContactSisterCompanies { get; set; }
        public virtual DbSet<CuContactType> CuContactTypes { get; set; }
        public virtual DbSet<CuCsConfiguration> CuCsConfigurations { get; set; }
        public virtual DbSet<CuCsOnsiteEmail> CuCsOnsiteEmails { get; set; }
        public virtual DbSet<CuCustomer> CuCustomers { get; set; }
        public virtual DbSet<CuCustomerBusinessCountry> CuCustomerBusinessCountries { get; set; }
        public virtual DbSet<CuCustomerContactType> CuCustomerContactTypes { get; set; }
        public virtual DbSet<CuCustomerGroup> CuCustomerGroups { get; set; }
        public virtual DbSet<CuCustomerSalesCountry> CuCustomerSalesCountries { get; set; }
        public virtual DbSet<CuDepartment> CuDepartments { get; set; }
        public virtual DbSet<CuEntity> CuEntities { get; set; }
        public virtual DbSet<CuKam> CuKams { get; set; }
        public virtual DbSet<CuPoFactory> CuPoFactories { get; set; }
        public virtual DbSet<CuPoSupplier> CuPoSuppliers { get; set; }
        public virtual DbSet<CuPrBrand> CuPrBrands { get; set; }
        public virtual DbSet<CuPrBuyer> CuPrBuyers { get; set; }
        public virtual DbSet<CuPrCity> CuPrCities { get; set; }
        public virtual DbSet<CuPrCountry> CuPrCountries { get; set; }
        public virtual DbSet<CuPrDepartment> CuPrDepartments { get; set; }
        public virtual DbSet<CuPrDetail> CuPrDetails { get; set; }
        public virtual DbSet<CuPrHolidayInfo> CuPrHolidayInfos { get; set; }
        public virtual DbSet<CuPrHolidayType> CuPrHolidayTypes { get; set; }
        public virtual DbSet<CuPrInspectionLocation> CuPrInspectionLocations { get; set; }
        public virtual DbSet<CuPrPriceCategory> CuPrPriceCategories { get; set; }
        public virtual DbSet<CuPrProductCategory> CuPrProductCategories { get; set; }
        public virtual DbSet<CuPrProductSubCategory> CuPrProductSubCategories { get; set; }
        public virtual DbSet<CuPrProvince> CuPrProvinces { get; set; }
        public virtual DbSet<CuPrRefComplexType> CuPrRefComplexTypes { get; set; }
        public virtual DbSet<CuPrServiceType> CuPrServiceTypes { get; set; }
        public virtual DbSet<CuPrSupplier> CuPrSuppliers { get; set; }
        public virtual DbSet<CuPrTranSpecialRule> CuPrTranSpecialRules { get; set; }
        public virtual DbSet<CuPrTranSubcategory> CuPrTranSubcategories { get; set; }
        public virtual DbSet<CuPriceCategory> CuPriceCategories { get; set; }
        public virtual DbSet<CuPriceCategoryPcsub2> CuPriceCategoryPcsub2S { get; set; }
        public virtual DbSet<CuProduct> CuProducts { get; set; }
        public virtual DbSet<CuProductApiService> CuProductApiServices { get; set; }
        public virtual DbSet<CuProductCategory> CuProductCategories { get; set; }
        public virtual DbSet<CuProductFileAttachment> CuProductFileAttachments { get; set; }
        public virtual DbSet<CuProductFileType> CuProductFileTypes { get; set; }
        public virtual DbSet<CuProductMschart> CuProductMscharts { get; set; }
        public virtual DbSet<CuProductMschartOcrMap> CuProductMschartOcrMaps { get; set; }
        public virtual DbSet<CuProductType> CuProductTypes { get; set; }
        public virtual DbSet<CuPurchaseOrder> CuPurchaseOrders { get; set; }
        public virtual DbSet<CuPurchaseOrderAttachment> CuPurchaseOrderAttachments { get; set; }
        public virtual DbSet<CuPurchaseOrderDetail> CuPurchaseOrderDetails { get; set; }
        public virtual DbSet<CuRefAccountingLeader> CuRefAccountingLeaders { get; set; }
        public virtual DbSet<CuRefActivitiesLevel> CuRefActivitiesLevels { get; set; }
        public virtual DbSet<CuRefBrandPriority> CuRefBrandPriorities { get; set; }
        public virtual DbSet<CuRefRelationshipStatus> CuRefRelationshipStatuses { get; set; }
        public virtual DbSet<CuReportCustomerDecisionComment> CuReportCustomerDecisionComments { get; set; }
        public virtual DbSet<CuSalesIncharge> CuSalesIncharges { get; set; }
        public virtual DbSet<CuSeason> CuSeasons { get; set; }
        public virtual DbSet<CuSeasonConfig> CuSeasonConfigs { get; set; }
        public virtual DbSet<CuServiceType> CuServiceTypes { get; set; }
        public virtual DbSet<CuSisterCompany> CuSisterCompanies { get; set; }
        public virtual DbSet<DaUserByBrand> DaUserByBrands { get; set; }
        public virtual DbSet<DaUserByBuyer> DaUserByBuyers { get; set; }
        public virtual DbSet<DaUserByDepartment> DaUserByDepartments { get; set; }
        public virtual DbSet<DaUserByFactoryCountry> DaUserByFactoryCountries { get; set; }
        public virtual DbSet<DaUserByProductCategory> DaUserByProductCategories { get; set; }
        public virtual DbSet<DaUserByRole> DaUserByRoles { get; set; }
        public virtual DbSet<DaUserByService> DaUserByServices { get; set; }
        public virtual DbSet<DaUserCustomer> DaUserCustomers { get; set; }
        public virtual DbSet<DaUserRoleNotificationByOffice> DaUserRoleNotificationByOffices { get; set; }
        public virtual DbSet<DfAttribute> DfAttributes { get; set; }
        public virtual DbSet<DfControlAttribute> DfControlAttributes { get; set; }
        public virtual DbSet<DfControlType> DfControlTypes { get; set; }
        public virtual DbSet<DfControlTypeAttribute> DfControlTypeAttributes { get; set; }
        public virtual DbSet<DfCuConfiguration> DfCuConfigurations { get; set; }
        public virtual DbSet<DfCuDdlSourceType> DfCuDdlSourceTypes { get; set; }
        public virtual DbSet<DfDdlSource> DfDdlSources { get; set; }
        public virtual DbSet<DfDdlSourceType> DfDdlSourceTypes { get; set; }
        public virtual DbSet<DmBrand> DmBrands { get; set; }
        public virtual DbSet<DmDepartment> DmDepartments { get; set; }
        public virtual DbSet<DmDetail> DmDetails { get; set; }
        public virtual DbSet<DmFile> DmFiles { get; set; }
        public virtual DbSet<DmRefModule> DmRefModules { get; set; }
        public virtual DbSet<DmRight> DmRights { get; set; }
        public virtual DbSet<DmRole> DmRoles { get; set; }
        public virtual DbSet<EcAutQcFoodExpense> EcAutQcFoodExpenses { get; set; }
        public virtual DbSet<EcAutQcTravelExpense> EcAutQcTravelExpenses { get; set; }
        public virtual DbSet<EcAutRefStartPort> EcAutRefStartPorts { get; set; }
        public virtual DbSet<EcAutRefTripType> EcAutRefTripTypes { get; set; }
        public virtual DbSet<EcAutTravelTariff> EcAutTravelTariffs { get; set; }
        public virtual DbSet<EcExpClaimStatus> EcExpClaimStatuses { get; set; }
        public virtual DbSet<EcExpencesClaim> EcExpencesClaims { get; set; }
        public virtual DbSet<EcExpenseClaimsAudit> EcExpenseClaimsAudits { get; set; }
        public virtual DbSet<EcExpenseClaimsInspection> EcExpenseClaimsInspections { get; set; }
        public virtual DbSet<EcExpenseClaimtype> EcExpenseClaimtypes { get; set; }
        public virtual DbSet<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
        public virtual DbSet<EcExpensesType> EcExpensesTypes { get; set; }
        public virtual DbSet<EcFoodAllowance> EcFoodAllowances { get; set; }
        public virtual DbSet<EcPaymenType> EcPaymenTypes { get; set; }
        public virtual DbSet<EcReceiptFile> EcReceiptFiles { get; set; }
        public virtual DbSet<EcReceiptFileAttachment> EcReceiptFileAttachments { get; set; }
        public virtual DbSet<EcStatusRole> EcStatusRoles { get; set; }
        public virtual DbSet<EmExchangeRate> EmExchangeRates { get; set; }
        public virtual DbSet<EmExchangeRateType> EmExchangeRateTypes { get; set; }
        public virtual DbSet<EntFeatureDetail> EntFeatureDetails { get; set; }
        public virtual DbSet<EntField> EntFields { get; set; }
        public virtual DbSet<EntMasterConfig> EntMasterConfigs { get; set; }
        public virtual DbSet<EntMasterType> EntMasterTypes { get; set; }
        public virtual DbSet<EntPage> EntPages { get; set; }
        public virtual DbSet<EntPagesField> EntPagesFields { get; set; }
        public virtual DbSet<EntRefFeature> EntRefFeatures { get; set; }
        public virtual DbSet<EsAdditionalRecipient> EsAdditionalRecipients { get; set; }
        public virtual DbSet<EsApiContact> EsApiContacts { get; set; }
        public virtual DbSet<EsApiDefaultContact> EsApiDefaultContacts { get; set; }
        public virtual DbSet<EsCuConfig> EsCuConfigs { get; set; }
        public virtual DbSet<EsCuContact> EsCuContacts { get; set; }
        public virtual DbSet<EsDetail> EsDetails { get; set; }
        public virtual DbSet<EsEmailReportTypeMap> EsEmailReportTypeMaps { get; set; }
        public virtual DbSet<EsFaCountryConfig> EsFaCountryConfigs { get; set; }
        public virtual DbSet<EsOfficeConfig> EsOfficeConfigs { get; set; }
        public virtual DbSet<EsProductCategoryConfig> EsProductCategoryConfigs { get; set; }
        public virtual DbSet<EsRecipientType> EsRecipientTypes { get; set; }
        public virtual DbSet<EsRefEmailSize> EsRefEmailSizes { get; set; }
        public virtual DbSet<EsRefFileType> EsRefFileTypes { get; set; }
        public virtual DbSet<EsRefRecipient> EsRefRecipients { get; set; }
        public virtual DbSet<EsRefRecipientType> EsRefRecipientTypes { get; set; }
        public virtual DbSet<EsRefReportInEmail> EsRefReportInEmails { get; set; }
        public virtual DbSet<EsRefReportSendType> EsRefReportSendTypes { get; set; }
        public virtual DbSet<EsRefSpecialRule> EsRefSpecialRules { get; set; }
        public virtual DbSet<EsResultConfig> EsResultConfigs { get; set; }
        public virtual DbSet<EsRuleRecipientEmailTypeMap> EsRuleRecipientEmailTypeMaps { get; set; }
        public virtual DbSet<EsServiceTypeConfig> EsServiceTypeConfigs { get; set; }
        public virtual DbSet<EsSpecialRule> EsSpecialRules { get; set; }
        public virtual DbSet<EsSuDataType> EsSuDataTypes { get; set; }
        public virtual DbSet<EsSuModule> EsSuModules { get; set; }
        public virtual DbSet<EsSuPreDefinedField> EsSuPreDefinedFields { get; set; }
        public virtual DbSet<EsSuTemplateDetail> EsSuTemplateDetails { get; set; }
        public virtual DbSet<EsSuTemplateMaster> EsSuTemplateMasters { get; set; }
        public virtual DbSet<EsSupFactConfig> EsSupFactConfigs { get; set; }
        public virtual DbSet<EsTranFile> EsTranFiles { get; set; }
        public virtual DbSet<EsType> EsTypes { get; set; }
        public virtual DbSet<EventBookingLog> EventBookingLogs { get; set; }
        public virtual DbSet<EventLog> EventLogs { get; set; }
        public virtual DbSet<FbBookingRequestLog> FbBookingRequestLogs { get; set; }
        public virtual DbSet<FbReportAdditionalPhoto> FbReportAdditionalPhotos { get; set; }
        public virtual DbSet<FbReportComment> FbReportComments { get; set; }
        public virtual DbSet<FbReportDefectPhoto> FbReportDefectPhotos { get; set; }
        public virtual DbSet<FbReportDetail> FbReportDetails { get; set; }
        public virtual DbSet<FbReportFabricControlmadeWith> FbReportFabricControlmadeWiths { get; set; }
        public virtual DbSet<FbReportFabricDefect> FbReportFabricDefects { get; set; }
        public virtual DbSet<FbReportInspDefect> FbReportInspDefects { get; set; }
        public virtual DbSet<FbReportInspSubSummary> FbReportInspSubSummaries { get; set; }
        public virtual DbSet<FbReportInspSummary> FbReportInspSummaries { get; set; }
        public virtual DbSet<FbReportInspSummaryPhoto> FbReportInspSummaryPhotos { get; set; }
        public virtual DbSet<FbReportInspSummaryType> FbReportInspSummaryTypes { get; set; }
        public virtual DbSet<FbReportManualLog> FbReportManualLogs { get; set; }
        public virtual DbSet<FbReportOtherInformation> FbReportOtherInformations { get; set; }
        public virtual DbSet<FbReportPackingBatteryInfo> FbReportPackingBatteryInfos { get; set; }
        public virtual DbSet<FbReportPackingDimention> FbReportPackingDimentions { get; set; }
        public virtual DbSet<FbReportPackingInfo> FbReportPackingInfos { get; set; }
        public virtual DbSet<FbReportPackingPackagingLabellingProduct> FbReportPackingPackagingLabellingProducts { get; set; }    
        public virtual DbSet<FbReportPackingWeight> FbReportPackingWeights { get; set; }
        public virtual DbSet<FbReportProblematicRemark> FbReportProblematicRemarks { get; set; }
        public virtual DbSet<FbReportProductBarcodesInfo> FbReportProductBarcodesInfos { get; set; }
        public virtual DbSet<FbReportProductDimention> FbReportProductDimentions { get; set; }
        public virtual DbSet<FbReportProductWeight> FbReportProductWeights { get; set; }
        public virtual DbSet<FbReportQcdetail> FbReportQcdetails { get; set; }        
        public virtual DbSet<FbReportQuantityDetail> FbReportQuantityDetails { get; set; }
        public virtual DbSet<FbReportQualityPlan> FbReportQuantityPlans { get; set; }
        public virtual DbSet<FbReportRdnumber> FbReportRdnumbers { get; set; }
        public virtual DbSet<FbReportResult> FbReportResults { get; set; }
        public virtual DbSet<FbReportReviewer> FbReportReviewers { get; set; }
        public virtual DbSet<FbReportSamplePicking> FbReportSamplePickings { get; set; }
        public virtual DbSet<FbReportSampleType> FbReportSampleTypes { get; set; }
        public virtual DbSet<FbReportTemplate> FbReportTemplates { get; set; }
        public virtual DbSet<FbStatus> FbStatuses { get; set; }
        public virtual DbSet<FbStatusType> FbStatusTypes { get; set; }
        public virtual DbSet<HrAttachment> HrAttachments { get; set; }
        public virtual DbSet<HrDepartment> HrDepartments { get; set; }
        public virtual DbSet<HrEmployeeType> HrEmployeeTypes { get; set; }
        public virtual DbSet<HrEntityMap> HrEntityMaps { get; set; }
        public virtual DbSet<HrFileAttachment> HrFileAttachments { get; set; }
        public virtual DbSet<HrFileType> HrFileTypes { get; set; }
        public virtual DbSet<HrHoliday> HrHolidays { get; set; }
        public virtual DbSet<HrHolidayDayType> HrHolidayDayTypes { get; set; }
        public virtual DbSet<HrLeave> HrLeaves { get; set; }
        public virtual DbSet<HrLeaveStatus> HrLeaveStatuses { get; set; }
        public virtual DbSet<HrLeaveType> HrLeaveTypes { get; set; }
        public virtual DbSet<HrOfficeControl> HrOfficeControls { get; set; }
        public virtual DbSet<HrOutSourceCompany> HrOutSourceCompanies { get; set; }
        public virtual DbSet<HrPayrollCompany> HrPayrollCompanies { get; set; }
        public virtual DbSet<HrPhoto> HrPhotos { get; set; }
        public virtual DbSet<HrPosition> HrPositions { get; set; }
        public virtual DbSet<HrProfile> HrProfiles { get; set; }
        public virtual DbSet<HrQualification> HrQualifications { get; set; }
        public virtual DbSet<HrRefBand> HrRefBands { get; set; }
        public virtual DbSet<HrRefSocialInsuranceType> HrRefSocialInsuranceTypes { get; set; }
        public virtual DbSet<HrRefStatus> HrRefStatuses { get; set; }
        public virtual DbSet<HrRenew> HrRenews { get; set; }
        public virtual DbSet<HrStaff> HrStaffs { get; set; }
        public virtual DbSet<HrStaffEntityServiceMap> HrStaffEntityServiceMaps { get; set; }
        public virtual DbSet<HrStaffExpertise> HrStaffExpertises { get; set; }
        public virtual DbSet<HrStaffHistory> HrStaffHistories { get; set; }
        public virtual DbSet<HrStaffMarketSegment> HrStaffMarketSegments { get; set; }
        public virtual DbSet<HrStaffOpCountry> HrStaffOpCountries { get; set; }
        public virtual DbSet<HrStaffPhoto> HrStaffPhotos { get; set; }
        public virtual DbSet<HrStaffProductCategory> HrStaffProductCategories { get; set; }
        public virtual DbSet<HrStaffProfile> HrStaffProfiles { get; set; }
        public virtual DbSet<HrStaffService> HrStaffServices { get; set; }
        public virtual DbSet<HrStaffTraining> HrStaffTrainings { get; set; }
        public virtual DbSet<InspBookingEmailConfiguration> InspBookingEmailConfigurations { get; set; }
        public virtual DbSet<InspBookingRule> InspBookingRules { get; set; }
        public virtual DbSet<InspCancelReason> InspCancelReasons { get; set; }
        public virtual DbSet<InspContainerTransaction> InspContainerTransactions { get; set; }
        public virtual DbSet<InspCuStatus> InspCuStatuses { get; set; }
        public virtual DbSet<InspDfTransaction> InspDfTransactions { get; set; }
        public virtual DbSet<InspIcStatus> InspIcStatuses { get; set; }
        public virtual DbSet<InspIcTitle> InspIcTitles { get; set; }
        public virtual DbSet<InspIcTranProduct> InspIcTranProducts { get; set; }
        public virtual DbSet<InspIcTransaction> InspIcTransactions { get; set; }
        public virtual DbSet<InspLabAddress> InspLabAddresses { get; set; }
        public virtual DbSet<InspLabAddressType> InspLabAddressTypes { get; set; }
        public virtual DbSet<InspLabContact> InspLabContacts { get; set; }
        public virtual DbSet<InspLabCustomer> InspLabCustomers { get; set; }
        public virtual DbSet<InspLabCustomerContact> InspLabCustomerContacts { get; set; }
        public virtual DbSet<InspLabDetail> InspLabDetails { get; set; }
        public virtual DbSet<InspLabType> InspLabTypes { get; set; }
        public virtual DbSet<InspProductTransaction> InspProductTransactions { get; set; }
        public virtual DbSet<InspPurchaseOrderColorTransaction> InspPurchaseOrderColorTransactions { get; set; }
        public virtual DbSet<InspPurchaseOrderTransaction> InspPurchaseOrderTransactions { get; set; }
        public virtual DbSet<InspRefBookingType> InspRefBookingTypes { get; set; }
        public virtual DbSet<InspRefDpPoint> InspRefDpPoints { get; set; }
        public virtual DbSet<InspRefFileAttachmentCategory> InspRefFileAttachmentCategories { get; set; }
        public virtual DbSet<InspRefHoldReason> InspRefHoldReasons { get; set; }
        public virtual DbSet<InspRefInspectionLocation> InspRefInspectionLocations { get; set; }
        public virtual DbSet<InspRefPackingStatus> InspRefPackingStatuses { get; set; }
        public virtual DbSet<InspRefPaymentOption> InspRefPaymentOptions { get; set; }
        public virtual DbSet<InspRefProductionStatus> InspRefProductionStatuses { get; set; }
        public virtual DbSet<InspRefQuantityType> InspRefQuantityTypes { get; set; }
        public virtual DbSet<InspRefReportRequest> InspRefReportRequests { get; set; }
        public virtual DbSet<InspRefShipmentType> InspRefShipmentTypes { get; set; }
        public virtual DbSet<InspRepCusDecision> InspRepCusDecisions { get; set; }
        public virtual DbSet<InspRepCusDecisionTemplate> InspRepCusDecisionTemplates { get; set; }
        public virtual DbSet<InspRescheduleReason> InspRescheduleReasons { get; set; }
        public virtual DbSet<InspStatus> InspStatuses { get; set; }
        public virtual DbSet<InspTranC> InspTranCs { get; set; }
        public virtual DbSet<InspTranCancel> InspTranCancels { get; set; }
        public virtual DbSet<InspTranCuBrand> InspTranCuBrands { get; set; }
        public virtual DbSet<InspTranCuBuyer> InspTranCuBuyers { get; set; }
        public virtual DbSet<InspTranCuContact> InspTranCuContacts { get; set; }
        public virtual DbSet<InspTranCuDepartment> InspTranCuDepartments { get; set; }
        public virtual DbSet<InspTranCuMerchandiser> InspTranCuMerchandisers { get; set; }
        public virtual DbSet<InspTranFaContact> InspTranFaContacts { get; set; }
        public virtual DbSet<InspTranFileAttachment> InspTranFileAttachments { get; set; }
        public virtual DbSet<InspTranFileAttachmentZip> InspTranFileAttachmentZips { get; set; }
        public virtual DbSet<InspTranHoldReason> InspTranHoldReasons { get; set; }
        public virtual DbSet<InspTranPicking> InspTranPickings { get; set; }
        public virtual DbSet<InspTranPickingContact> InspTranPickingContacts { get; set; }
        public virtual DbSet<InspTranReschedule> InspTranReschedules { get; set; }
        public virtual DbSet<InspTranServiceType> InspTranServiceTypes { get; set; }
        public virtual DbSet<InspTranShipmentType> InspTranShipmentTypes { get; set; }
        public virtual DbSet<InspTranStatusLog> InspTranStatusLogs { get; set; }
        public virtual DbSet<InspTranSuContact> InspTranSuContacts { get; set; }
        public virtual DbSet<InspTransaction> InspTransactions { get; set; }
        public virtual DbSet<InspTransactionDraft> InspTransactionDrafts { get; set; }
        public virtual DbSet<InvAutTranCommunication> InvAutTranCommunications { get; set; }
        public virtual DbSet<InvAutTranContactDetail> InvAutTranContactDetails { get; set; }
        public virtual DbSet<InvAutTranDetail> InvAutTranDetails { get; set; }
        public virtual DbSet<InvAutTranStatusLog> InvAutTranStatusLogs { get; set; }
        public virtual DbSet<InvAutTranTax> InvAutTranTaxes { get; set; }
        public virtual DbSet<InvCreRefCreditType> InvCreRefCreditTypes { get; set; }
        public virtual DbSet<InvCreTranClaimDetail> InvCreTranClaimDetails { get; set; }
        public virtual DbSet<InvCreTranContact> InvCreTranContacts { get; set; }
        public virtual DbSet<InvCreTransaction> InvCreTransactions { get; set; }
        public virtual DbSet<InvDaCustomer> InvDaCustomers { get; set; }
        public virtual DbSet<InvDaInvoiceType> InvDaInvoiceTypes { get; set; }
        public virtual DbSet<InvDaOffice> InvDaOffices { get; set; }
        public virtual DbSet<InvDaTransaction> InvDaTransactions { get; set; }
        public virtual DbSet<InvDisRefType> InvDisRefTypes { get; set; }
        public virtual DbSet<InvDisTranCountry> InvDisTranCountries { get; set; }
        public virtual DbSet<InvDisTranDetail> InvDisTranDetails { get; set; }
        public virtual DbSet<InvDisTranPeriodInfo> InvDisTranPeriodInfos { get; set; }
        public virtual DbSet<InvExfContactDetail> InvExfContactDetails { get; set; }
        public virtual DbSet<InvExfStatus> InvExfStatuses { get; set; }
        public virtual DbSet<InvExfTranDetail> InvExfTranDetails { get; set; }
        public virtual DbSet<InvExfTranStatusLog> InvExfTranStatusLogs { get; set; }
        public virtual DbSet<InvExfTransaction> InvExfTransactions { get; set; }
        public virtual DbSet<InvExfType> InvExfTypes { get; set; }
        public virtual DbSet<InvExtTranTax> InvExtTranTaxes { get; set; }
        public virtual DbSet<InvManTranDetail> InvManTranDetails { get; set; }
        public virtual DbSet<InvManTranTax> InvManTranTaxes { get; set; }
        public virtual DbSet<InvManTransaction> InvManTransactions { get; set; }
        public virtual DbSet<InvPaymentStatus> InvPaymentStatuses { get; set; }
        public virtual DbSet<InvRefBank> InvRefBanks { get; set; }
        public virtual DbSet<InvRefBillingFreequency> InvRefBillingFreequencies { get; set; }
        public virtual DbSet<InvRefFeesFrom> InvRefFeesFroms { get; set; }
        public virtual DbSet<InvRefFileType> InvRefFileTypes { get; set; }
        public virtual DbSet<InvRefInterventionType> InvRefInterventionTypes { get; set; }
        public virtual DbSet<InvRefOffice> InvRefOffices { get; set; }
        public virtual DbSet<InvRefPaymentMode> InvRefPaymentModes { get; set; }
        public virtual DbSet<InvRefPaymentTerm> InvRefPaymentTerms { get; set; }
        public virtual DbSet<InvRefPriceCalculationType> InvRefPriceCalculationTypes { get; set; }
        public virtual DbSet<InvRefRequestType> InvRefRequestTypes { get; set; }
        public virtual DbSet<InvStatus> InvStatuses { get; set; }
        public virtual DbSet<InvTmDetail> InvTmDetails { get; set; }
        public virtual DbSet<InvTmType> InvTmTypes { get; set; }
        public virtual DbSet<InvTranBankTax> InvTranBankTaxes { get; set; }
        public virtual DbSet<InvTranFile> InvTranFiles { get; set; }
        public virtual DbSet<InvTranInvoiceRequest> InvTranInvoiceRequests { get; set; }
        public virtual DbSet<InvTranInvoiceRequestContact> InvTranInvoiceRequestContacts { get; set; }
        public virtual DbSet<ItLoginLog> ItLoginLogs { get; set; }
        public virtual DbSet<ItRight> ItRights { get; set; }
        public virtual DbSet<ItRightEntity> ItRightEntities { get; set; }
        public virtual DbSet<ItRightMap> ItRightMaps { get; set; }
        public virtual DbSet<ItRightType> ItRightTypes { get; set; }
        public virtual DbSet<ItRole> ItRoles { get; set; }
        public virtual DbSet<ItRoleRight> ItRoleRights { get; set; }
        public virtual DbSet<ItUserCuBrand> ItUserCuBrands { get; set; }
        public virtual DbSet<ItUserCuDepartment> ItUserCuDepartments { get; set; }
        public virtual DbSet<ItUserMaster> ItUserMasters { get; set; }
        public virtual DbSet<ItUserRole> ItUserRoles { get; set; }
        public virtual DbSet<ItUserType> ItUserTypes { get; set; }
        public virtual DbSet<JobScheduleConfiguration> JobScheduleConfigurations { get; set; }
        public virtual DbSet<JobScheduleJobType> JobScheduleJobTypes { get; set; }
        public virtual DbSet<JobScheduleLog> JobScheduleLogs { get; set; }
        public virtual DbSet<KpiColumn> KpiColumns { get; set; }
        public virtual DbSet<KpiTemplate> KpiTemplates { get; set; }
        public virtual DbSet<KpiTemplateColumn> KpiTemplateColumns { get; set; }
        public virtual DbSet<KpiTemplateSubModule> KpiTemplateSubModules { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LogBookingFbQueue> LogBookingFbQueues { get; set; }
        public virtual DbSet<LogBookingReportEmailQueue> LogBookingReportEmailQueues { get; set; }
        public virtual DbSet<LogEmailQueue> LogEmailQueues { get; set; }
        public virtual DbSet<LogEmailQueueAttachment> LogEmailQueueAttachments { get; set; }
        public virtual DbSet<MidNotification> MidNotifications { get; set; }
        public virtual DbSet<MidNotificationMessage> MidNotificationMessages { get; set; }
        public virtual DbSet<MidNotificationType> MidNotificationTypes { get; set; }
        public virtual DbSet<MidTask> MidTasks { get; set; }
        public virtual DbSet<MidTaskType> MidTaskTypes { get; set; }
        public virtual DbSet<OmDetail> OmDetails { get; set; }
        public virtual DbSet<OmRefPurpose> OmRefPurposes { get; set; }
        public virtual DbSet<QcBlCustomer> QcBlCustomers { get; set; }
        public virtual DbSet<QcBlProductCatgeory> QcBlProductCatgeories { get; set; }
        public virtual DbSet<QcBlProductSubCategory> QcBlProductSubCategories { get; set; }
        public virtual DbSet<QcBlProductSubCategory2> QcBlProductSubCategory2S { get; set; }
        public virtual DbSet<QcBlSupplierFactory> QcBlSupplierFactories { get; set; }
        public virtual DbSet<QcBlockList> QcBlockLists { get; set; }
        public virtual DbSet<QuBillMethod> QuBillMethods { get; set; }
        public virtual DbSet<QuInspProduct> QuInspProducts { get; set; }
        public virtual DbSet<QuPaidBy> QuPaidBies { get; set; }
        public virtual DbSet<QuPdfversion> QuPdfversions { get; set; }
        public virtual DbSet<QuQuotation> QuQuotations { get; set; }
        public virtual DbSet<QuQuotationAudManday> QuQuotationAudMandays { get; set; }
        public virtual DbSet<QuQuotationAudit> QuQuotationAudits { get; set; }
        public virtual DbSet<QuQuotationContact> QuQuotationContacts { get; set; }
        public virtual DbSet<QuQuotationCustomerContact> QuQuotationCustomerContacts { get; set; }
        public virtual DbSet<QuQuotationFactoryContact> QuQuotationFactoryContacts { get; set; }
        public virtual DbSet<QuQuotationInsp> QuQuotationInsps { get; set; }
        public virtual DbSet<QuQuotationInspManday> QuQuotationInspMandays { get; set; }
        public virtual DbSet<QuQuotationPdfVersion> QuQuotationPdfVersions { get; set; }
        public virtual DbSet<QuQuotationSupplierContact> QuQuotationSupplierContacts { get; set; }
        public virtual DbSet<QuStatus> QuStatuses { get; set; }
        public virtual DbSet<QuTranStatusLog> QuTranStatusLogs { get; set; }
        public virtual DbSet<QuWorkLoadMatrix> QuWorkLoadMatrices { get; set; }
        public virtual DbSet<RefAddressType> RefAddressTypes { get; set; }
        public virtual DbSet<RefAqlPickSampleSizeAcceCode> RefAqlPickSampleSizeAcceCodes { get; set; }
        public virtual DbSet<RefAqlPickSampleSizeCodeValue> RefAqlPickSampleSizeCodeValues { get; set; }
        public virtual DbSet<RefAqlSampleCode> RefAqlSampleCodes { get; set; }
        public virtual DbSet<RefArea> RefAreas { get; set; }
        public virtual DbSet<RefBillingEntity> RefBillingEntities { get; set; }
        public virtual DbSet<RefBudgetForecast> RefBudgetForecasts { get; set; }
        public virtual DbSet<RefBusinessLine> RefBusinessLines { get; set; }
        public virtual DbSet<RefBusinessType> RefBusinessTypes { get; set; }
        public virtual DbSet<RefCity> RefCities { get; set; }
        public virtual DbSet<RefCityDetail> RefCityDetails { get; set; }
        public virtual DbSet<RefContainerSize> RefContainerSizes { get; set; }
        public virtual DbSet<RefCountry> RefCountries { get; set; }
        public virtual DbSet<RefCountryLocation> RefCountryLocations { get; set; }
        public virtual DbSet<RefCounty> RefCounties { get; set; }
        public virtual DbSet<RefCurrency> RefCurrencies { get; set; }
        public virtual DbSet<RefDataSourceType> RefDataSourceTypes { get; set; }
        public virtual DbSet<RefDateFormat> RefDateFormats { get; set; }
        public virtual DbSet<RefDefectClassification> RefDefectClassifications { get; set; }
        public virtual DbSet<RefDelimiter> RefDelimiters { get; set; }
        public virtual DbSet<RefExpertise> RefExpertises { get; set; }
        public virtual DbSet<RefFileExtension> RefFileExtensions { get; set; }
        public virtual DbSet<RefInspCusDecision> RefInspCusDecisions { get; set; }
        public virtual DbSet<RefInspCusDecisionConfig> RefInspCusDecisionConfigs { get; set; }
        public virtual DbSet<RefInvoiceType> RefInvoiceTypes { get; set; }
        public virtual DbSet<RefKpiTeamplate> RefKpiTeamplates { get; set; }
        public virtual DbSet<RefKpiTeamplateCustomer> RefKpiTeamplateCustomers { get; set; }
        public virtual DbSet<RefKpiTemplateType> RefKpiTemplateTypes { get; set; }
        public virtual DbSet<RefLevelPick1> RefLevelPick1S { get; set; }
        public virtual DbSet<RefLevelPick2> RefLevelPick2S { get; set; }
        public virtual DbSet<RefLocation> RefLocations { get; set; }
        public virtual DbSet<RefLocationCountry> RefLocationCountries { get; set; }
        public virtual DbSet<RefLocationType> RefLocationTypes { get; set; }
        public virtual DbSet<RefMarketSegment> RefMarketSegments { get; set; }
        public virtual DbSet<RefModule> RefModules { get; set; }
        public virtual DbSet<RefPick1> RefPick1S { get; set; }
        public virtual DbSet<RefPick2> RefPick2S { get; set; }
        public virtual DbSet<RefPickType> RefPickTypes { get; set; }
        public virtual DbSet<RefProductCategory> RefProductCategories { get; set; }
        public virtual DbSet<RefProductCategoryApiService> RefProductCategoryApiServices { get; set; }
        public virtual DbSet<RefProductCategorySub> RefProductCategorySubs { get; set; }
        public virtual DbSet<RefProductCategorySub2> RefProductCategorySub2S { get; set; }
        public virtual DbSet<RefProductCategorySub3> RefProductCategorySub3S { get; set; }
        public virtual DbSet<RefProductUnit> RefProductUnits { get; set; }
        public virtual DbSet<RefProspectStatus> RefProspectStatuses { get; set; }
        public virtual DbSet<RefProvince> RefProvinces { get; set; }
        public virtual DbSet<RefReInspectionType> RefReInspectionTypes { get; set; }
        public virtual DbSet<RefReportUnit> RefReportUnits { get; set; }
        public virtual DbSet<RefSampleType> RefSampleTypes { get; set; }
        public virtual DbSet<RefSeason> RefSeasons { get; set; }
        public virtual DbSet<RefSeasonYear> RefSeasonYears { get; set; }
        public virtual DbSet<RefService> RefServices { get; set; }
        public virtual DbSet<RefServiceType> RefServiceTypes { get; set; }
        public virtual DbSet<RefServiceTypeXero> RefServiceTypeXeros { get; set; }
        public virtual DbSet<RefSignEquality> RefSignEqualities { get; set; }
        public virtual DbSet<RefTown> RefTowns { get; set; }
        public virtual DbSet<RefTranslation> RefTranslations { get; set; }
        public virtual DbSet<RefTranslationGroup> RefTranslationGroups { get; set; }
        public virtual DbSet<RefUnit> RefUnits { get; set; }
        public virtual DbSet<RefZone> RefZones { get; set; }
        public virtual DbSet<RepFastRefStatus> RepFastRefStatuses { get; set; }
        public virtual DbSet<RepFastTemplate> RepFastTemplates { get; set; }
        public virtual DbSet<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
        public virtual DbSet<RepFastTranLog> RepFastTranLogs { get; set; }
        public virtual DbSet<RepFastTransaction> RepFastTransactions { get; set; }
        public virtual DbSet<RepRefToolType> RepRefToolTypes { get; set; }
        public virtual DbSet<RestApiLog> RestApiLogs { get; set; }
        public virtual DbSet<SchQctype> SchQctypes { get; set; }
        public virtual DbSet<SchScheduleC> SchScheduleCs { get; set; }
        public virtual DbSet<SchScheduleQc> SchScheduleQcs { get; set; }
        public virtual DbSet<SuAddress> SuAddresses { get; set; }
        public virtual DbSet<SuAddressType> SuAddressTypes { get; set; }
        public virtual DbSet<SuApiService> SuApiServices { get; set; }
        public virtual DbSet<SuContact> SuContacts { get; set; }
        public virtual DbSet<SuContactApiService> SuContactApiServices { get; set; }
        public virtual DbSet<SuContactEntityMap> SuContactEntityMaps { get; set; }
        public virtual DbSet<SuContactEntityServiceMap> SuContactEntityServiceMaps { get; set; }
        public virtual DbSet<SuCreditTerm> SuCreditTerms { get; set; }
        public virtual DbSet<SuEntity> SuEntities { get; set; }
        public virtual DbSet<SuGrade> SuGrades { get; set; }
        public virtual DbSet<SuLevel> SuLevels { get; set; }
        public virtual DbSet<SuLevelCustom> SuLevelCustoms { get; set; }
        public virtual DbSet<SuOwnlerShip> SuOwnlerShips { get; set; }
        public virtual DbSet<SuStatus> SuStatuses { get; set; }
        public virtual DbSet<SuSupplier> SuSuppliers { get; set; }
        public virtual DbSet<SuSupplierCustomer> SuSupplierCustomers { get; set; }
        public virtual DbSet<SuSupplierCustomerContact> SuSupplierCustomerContacts { get; set; }
        public virtual DbSet<SuSupplierFactory> SuSupplierFactories { get; set; }
        public virtual DbSet<SuType> SuTypes { get; set; }
        public virtual DbSet<TcfMasterDataLog> TcfMasterDataLogs { get; set; }
        public virtual DbSet<UgRole> UgRoles { get; set; }
        public virtual DbSet<UgUserGuideDetail> UgUserGuideDetails { get; set; }
        public virtual DbSet<ZohoRequestLog> ZohoRequestLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<ApEntity>(entity =>
            {
                entity.ToTable("AP_Entity");

                entity.Property(e => e.FbId).HasColumnName("FB_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ApModule>(entity =>
            {
                entity.ToTable("AP_Module");

                entity.Property(e => e.DataSourceName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.DataSourceType)
                    .WithMany(p => p.ApModules)
                    .HasForeignKey(d => d.DataSourceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AP_Module__DataS__381CA9CD");
            });

            modelBuilder.Entity<ApModuleRole>(entity =>
            {
                entity.HasKey(e => new { e.IdModule, e.IdRole })
                    .HasName("PK__AP_Modul__82B23A0600118D6D");

                entity.ToTable("AP_ModuleRole");

                entity.HasOne(d => d.IdModuleNavigation)
                    .WithMany(p => p.ApModuleRoles)
                    .HasForeignKey(d => d.IdModule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AP_Module__IdMod__3AF91678");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.ApModuleRoles)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AP_Module__IdRol__3BED3AB1");
            });

            modelBuilder.Entity<ApSubModule>(entity =>
            {
                entity.ToTable("AP_SubModule");

                entity.Property(e => e.DataSourceName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.DataSourceType)
                    .WithMany(p => p.ApSubModules)
                    .HasForeignKey(d => d.DataSourceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AP_SubMod__DataS__3FBDCB95");

                entity.HasOne(d => d.IdModuleNavigation)
                    .WithMany(p => p.ApSubModules)
                    .HasForeignKey(d => d.IdModule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AP_SubMod__IdMod__3EC9A75C");
            });

            modelBuilder.Entity<ApSubModuleRole>(entity =>
            {
                entity.HasKey(e => new { e.IdSubModule, e.IdRole })
                    .HasName("PK__AP_SubMo__FFD8C6D4EA773A7F");

                entity.ToTable("AP_SubModuleRole");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.ApSubModuleRoles)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AP_SubMod__IdRol__438E5C79");

                entity.HasOne(d => d.IdSubModuleNavigation)
                    .WithMany(p => p.ApSubModuleRoles)
                    .HasForeignKey(d => d.IdSubModule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AP_SubMod__IdSub__429A3840");
            });

            modelBuilder.Entity<ApigatewayLog>(entity =>
            {
                entity.ToTable("APIGateway_Log");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.RequestBaseUrl).HasMaxLength(500);

                entity.Property(e => e.RequestUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<AudBookingContact>(entity =>
            {
                entity.ToTable("AUD_BookingContact");

                entity.Property(e => e.BookingEmailCc)
                    .HasColumnName("BookingEmailCC")
                    .HasMaxLength(500);

                entity.Property(e => e.BookingEmailTo)
                    .HasColumnName("Booking_EmailTo")
                    .HasMaxLength(500);

                entity.Property(e => e.ContactInformation).HasMaxLength(2500);

                entity.Property(e => e.FactoryCountryId).HasColumnName("Factory_Country_Id");

                entity.Property(e => e.OfficeId).HasColumnName("Office_Id");

                entity.Property(e => e.PenaltyEmail).HasMaxLength(500);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudBookingContacts)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_Booki__Entit__436373F0");

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.AudBookingContacts)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .HasConstraintName("FK__AUD_Booki__Facto__426F4FB7");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.AudBookingContacts)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_Booki__Offic__417B2B7E");
            });

            modelBuilder.Entity<AudBookingEmailConfiguration>(entity =>
            {
                entity.ToTable("AUD_BookingEmailConfiguration");

                entity.Property(e => e.Email).IsRequired();

                entity.HasOne(d => d.AuditStatus)
                    .WithMany(p => p.AudBookingEmailConfigurations)
                    .HasForeignKey(d => d.AuditStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AUD_BookingEmailConfiguration_AuditStatusId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AudBookingEmailConfigurations)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AUD_BookingEmailConfiguration_CustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudBookingEmailConfigurations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_AUD_BookingEmailConfiguration_EntityId");

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.AudBookingEmailConfigurations)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AUD_BookingEmailConfiguration_FactoryCountryId");
            });

            modelBuilder.Entity<AudBookingRule>(entity =>
            {
                entity.ToTable("AUD_BookingRules");

                entity.Property(e => e.BookingRule)
                    .HasColumnName("Booking_Rule")
                    .HasMaxLength(3000);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.FactoryCountryId).HasColumnName("Factory_CountryId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AudBookingRules)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__AUD_Booki__Custo__44579829");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudBookingRules)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_Booki__Entit__463FE09B");

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.AudBookingRules)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .HasConstraintName("FK__AUD_Booki__Facto__454BBC62");
            });

            modelBuilder.Entity<AudCancelRescheduleReason>(entity =>
            {
                entity.ToTable("AUD_Cancel_Reschedule_Reasons");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AudCancelRescheduleReasons)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__AUD_Cance__Custo__473404D4");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudCancelRescheduleReasons)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_Cance__Entit__4828290D");
            });

            modelBuilder.Entity<AudCuProductCategory>(entity =>
            {
                entity.ToTable("AUD_CU_ProductCategory");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FbName)
                    .HasColumnName("FB_Name")
                    .HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudCuProductCategoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_AUD_CU_ProductCategory_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AudCuProductCategories)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_AUD_CU_ProductCategory_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudCuProductCategoryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_AUD_CU_ProductCategory_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudCuProductCategories)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_AUD_CU_ProductCategory_EntityId");

                entity.HasOne(d => d.ServiceTypeNavigation)
                    .WithMany(p => p.AudCuProductCategories)
                    .HasForeignKey(d => d.ServiceType)
                    .HasConstraintName("FK_AUD_CU_ProductCategory_ServiceType");
            });

            modelBuilder.Entity<AudEvaluationRound>(entity =>
            {
                entity.ToTable("AUD_EvaluationRound");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudEvaluationRounds)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_Evalu__Entit__491C4D46");
            });

            modelBuilder.Entity<AudFbReportCheckpoint>(entity =>
            {
                entity.ToTable("Aud_FB_Report_Checkpoints");

                entity.Property(e => e.ChekPointName).HasMaxLength(1000);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Critical).HasMaxLength(500);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Grade).HasMaxLength(100);

                entity.Property(e => e.Major).HasMaxLength(500);

                entity.Property(e => e.MaxPoint).HasMaxLength(500);

                entity.Property(e => e.Minor).HasMaxLength(500);

                entity.Property(e => e.Remarks).HasMaxLength(4000);

                entity.Property(e => e.ScorePercentage).HasMaxLength(100);

                entity.Property(e => e.ScoreValue).HasMaxLength(100);

                entity.Property(e => e.ZeroTolerance).HasMaxLength(500);

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudFbReportCheckpoints)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("FK_Aud_FB_Report_Checkpoints_AuditId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudFbReportCheckpointCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Aud_FB_Report_Checkpoints_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudFbReportCheckpointDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Aud_FB_Report_Checkpoints_DeletedBy");
            });

            modelBuilder.Entity<AudStatus>(entity =>
            {
                entity.ToTable("AUD_Status");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.Status).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudStatuses)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_Statu__Entit__4A10717F");
            });

            modelBuilder.Entity<AudTranAuditor>(entity =>
            {
                entity.ToTable("AUD_TRAN_Auditors");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.StaffId).HasColumnName("Staff_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranAuditors)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__4B0495B8");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranAuditorCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_AUD_TRAN_Auditors_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranAuditorDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_AUD_TRAN_Auditors_DeletedBy");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.AudTranAuditors)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Staff__4BF8B9F1");
            });

            modelBuilder.Entity<AudTranC>(entity =>
            {
                entity.ToTable("AUD_TRAN_CS");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.StaffId).HasColumnName("Staff_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranCS)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__51B19347");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranCCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_AUD_TRAN_Cs_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranCDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_AUD_TRAN_Cs_DeletedBy");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.AudTranCS)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Staff__52A5B780");
            });

            modelBuilder.Entity<AudTranCancelReschedule>(entity =>
            {
                entity.ToTable("AUD_TRAN_Cancel_Reschedule");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.Comments).HasMaxLength(500);

                entity.Property(e => e.InternalComments).HasMaxLength(500);

                entity.Property(e => e.TravellingExpense).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOn)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranCancelReschedules)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__50BD6F0E");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.AudTranCancelReschedules)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK__AUD_TRAN___Curre__4FC94AD5");

                entity.HasOne(d => d.ReasonType)
                    .WithMany(p => p.AudTranCancelReschedules)
                    .HasForeignKey(d => d.ReasonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Reaso__4ED5269C");



                entity.HasOne(d => d.CreatedByNavigation)
                                   .WithMany(p => p.AudTranCancelReschedules)
                                   .HasForeignKey(d => d.CreatedBy)
                                   .HasConstraintName("FK__AUD_TRAN___Creat__02C075A4");
            });

            modelBuilder.Entity<AudTranCuContact>(entity =>
            {
                entity.ToTable("AUD_TRAN_CU_Contact");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranCuContacts)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__585E90D6");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.AudTranCuContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Conta__576A6C9D");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranCuContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Creat__5582242B");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranCuContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__56764864");
            });

            modelBuilder.Entity<AudTranFaContact>(entity =>
            {
                entity.ToTable("AUD_TRAN_FA_Contact");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranFaContacts)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__5C2F21BA");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.AudTranFaContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Conta__5B3AFD81");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranFaContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Creat__5952B50F");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranFaContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__5A46D948");
            });

            modelBuilder.Entity<AudTranFaProfile>(entity =>
            {
                entity.ToTable("AUD_TRAN_FA_Profile");

                entity.Property(e => e.Accrediations).HasMaxLength(3500);

                entity.Property(e => e.AdministrativeStaff).HasColumnName("Administrative_Staff");

                entity.Property(e => e.AnnualHolidays)
                    .HasColumnName("Annual_Holidays")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.AnnualProduction)
                    .HasColumnName("Annual_production")
                    .HasMaxLength(1000);

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CompanyOpenTime)
                    .HasColumnName("Company_Open_Time")
                    .HasMaxLength(1000);

                entity.Property(e => e.CompanySurfaceArea)
                    .HasColumnName("Company_Surface_Area")
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("Created_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IndustryTradeAssociation)
                    .HasColumnName("Industry_TradeAssociation")
                    .HasMaxLength(3500);

                entity.Property(e => e.InvestmentBackground)
                    .HasColumnName("Investment_Background")
                    .HasMaxLength(3000);

                entity.Property(e => e.MaximumCapacity)
                    .HasColumnName("Maximum_Capacity")
                    .HasMaxLength(1000);

                entity.Property(e => e.NoOfCustomer)
                    .HasColumnName("No_of_Customer")
                    .HasMaxLength(1000);

                entity.Property(e => e.NoOfSuppliersComponent)
                    .HasColumnName("No_Of_Suppliers_Component")
                    .HasMaxLength(1000);

                entity.Property(e => e.NumberOfSites)
                    .HasColumnName("Number_Of_Sites")
                    .HasMaxLength(1000);

                entity.Property(e => e.PercentageCusTotalCapacity)
                    .HasColumnName("Percentage_Cus_Total_Capacity")
                    .HasMaxLength(1000);

                entity.Property(e => e.PossibilityOfExtension)
                    .HasColumnName("Possibility_Of_Extension")
                    .HasMaxLength(1000);

                entity.Property(e => e.ProductionStaff).HasColumnName("Production_Staff");

                entity.Property(e => e.PublicLiabilityInsurance)
                    .HasColumnName("Public_Liability_Insurance")
                    .HasMaxLength(3500);

                entity.Property(e => e.QualityStaff).HasColumnName("Quality_Staff");

                entity.Property(e => e.SalesStaff).HasColumnName("Sales_Staff");

                entity.Property(e => e.TotalStaff).HasColumnName("Total_Staff");

                entity.Property(e => e.TypeOfProductManufactured)
                    .HasColumnName("Type_Of_product_Manufactured")
                    .HasMaxLength(3000);

                entity.Property(e => e.TypesOfBrands)
                    .HasColumnName("Types_Of_Brands")
                    .HasMaxLength(3000);

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranFaProfiles)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__5F0B8E65");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranFaProfileCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Creat__5D2345F3");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranFaProfileDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__5E176A2C");
            });

            modelBuilder.Entity<AudTranFileAttachment>(entity =>
            {
                entity.ToTable("AUD_TRAN_File_Attachment");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranFileAttachments)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__15BD7028");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranFileAttachmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__17A5B89A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AudTranFileAttachmentUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___UserI__16B19461");
            });

            modelBuilder.Entity<AudTranReport>(entity =>
            {
                entity.ToTable("AUD_TRAN_Report");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranReports)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__100496D2");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranReportCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__AUD_TRAN___Creat__10F8BB0B");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranReportDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__11ECDF44");
            });

            modelBuilder.Entity<AudTranReport1>(entity =>
            {
                entity.ToTable("AUD_TRAN_Reports");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__AUD_TRAN__4B840D04B5FB1798")
                    .IsUnique();

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranReport1S)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__63D04382");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AudTranReport1S)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___UserI__64C467BB");
            });

            modelBuilder.Entity<AudTranReportDetail>(entity =>
            {
                entity.ToTable("AUD_TRAN_Report_Details");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.Comments).HasMaxLength(2000);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceDateFrom)
                    .HasColumnName("ServiceDate_From")
                    .HasColumnType("datetime");

                entity.Property(e => e.ServiceDateTo)
                    .HasColumnName("ServiceDate_To")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranReportDetails)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__61E7FB10");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AudTranReportDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___UserI__62DC1F49");
            });

            modelBuilder.Entity<AudTranServiceType>(entity =>
            {
                entity.ToTable("AUD_TRAN_ServiceType");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceType_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranServiceTypes)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__6894F89F");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranServiceTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Creat__65B88BF4");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranServiceTypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__66ACB02D");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.AudTranServiceTypes)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Servi__67A0D466");
            });

            modelBuilder.Entity<AudTranStatusLog>(entity =>
            {
                entity.ToTable("AUD_TRAN_Status_Log");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranStatusLogs)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__6B71654A");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranStatusLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Creat__69891CD8");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudTranStatusLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_AUD_TRAN_Status_Log_EntityId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.AudTranStatusLogs)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Statu__6A7D4111");
            });

            modelBuilder.Entity<AudTranSuContact>(entity =>
            {
                entity.ToTable("AUD_TRAN_SU_Contact");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranSuContacts)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__6F41F62E");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.AudTranSuContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Conta__6E4DD1F5");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranSuContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Creat__6C658983");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranSuContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__6D59ADBC");
            });

            modelBuilder.Entity<AudTranWorkProcess>(entity =>
            {
                entity.ToTable("AUD_TRAN_WorkProcess");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.WorkProcessId).HasColumnName("WorkProcess_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AudTranWorkProcesses)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Audit__721E62D9");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTranWorkProcessCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___Creat__70361A67");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.AudTranWorkProcessDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__AUD_TRAN___Delet__712A3EA0");

                entity.HasOne(d => d.WorkProcess)
                    .WithMany(p => p.AudTranWorkProcesses)
                    .HasForeignKey(d => d.WorkProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_TRAN___WorkP__73128712");
            });

            modelBuilder.Entity<AudTransaction>(entity =>
            {
                entity.ToTable("AUD_Transaction");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__AUD_Tran__4B840D04E3C9363F")
                    .IsUnique();

                entity.Property(e => e.ApiBookingComments)
                    .HasColumnName("API_Booking_Comments")
                    .HasMaxLength(1500);

                entity.Property(e => e.ApplicantEmail).HasMaxLength(200);

                entity.Property(e => e.ApplicantName).HasMaxLength(200);

                entity.Property(e => e.ApplicantPhNo).HasMaxLength(200);

                entity.Property(e => e.AuditTypeId).HasColumnName("AuditType_Id");

                entity.Property(e => e.BrandId).HasColumnName("Brand_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuProductCategory).HasColumnName("CU_ProductCategory");

                entity.Property(e => e.CusBookingComments)
                    .HasColumnName("Cus_Booking_Comments")
                    .HasMaxLength(1500);

                entity.Property(e => e.CustomerBookingNo).HasMaxLength(1000);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.EvalutionId).HasColumnName("Evalution_Id");

                entity.Property(e => e.FactoryId).HasColumnName("Factory_Id");

                entity.Property(e => e.FbfillingStatus).HasColumnName("FBFillingStatus");

                entity.Property(e => e.FbmissionId).HasColumnName("FBMissionId");

                entity.Property(e => e.IsEaqf).HasColumnName("IsEAQF");

                entity.Property(e => e.FbmissionTitle)
                    .HasColumnName("FBMissionTitle")
                    .HasMaxLength(3000);

                entity.Property(e => e.FbreportId).HasColumnName("FBReportId");

                entity.Property(e => e.FbreportStatus).HasColumnName("FBReportStatus");

                entity.Property(e => e.FbreportTitle)
                    .HasColumnName("FBReportTitle")
                    .HasMaxLength(3000);

                entity.Property(e => e.FbreviewStatus).HasColumnName("FBReviewStatus");

                entity.Property(e => e.FinalReportPath).HasMaxLength(500);

                entity.Property(e => e.Grade).HasMaxLength(100);

                entity.Property(e => e.InternalComments)
                    .HasColumnName("Internal_Comments")
                    .HasMaxLength(1500);

                entity.Property(e => e.OfficeId).HasColumnName("Office_Id");

                entity.Property(e => e.PictureReportPath).HasMaxLength(500);

                entity.Property(e => e.PoNumber)
                    .HasColumnName("PO_Number")
                    .HasMaxLength(1000);

                entity.Property(e => e.ReportNo)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ReportRemarks).HasMaxLength(4000);

                entity.Property(e => e.ScoreValue).HasMaxLength(100);

                entity.Property(e => e.Scorepercentage).HasMaxLength(100);

                entity.Property(e => e.SeasonId).HasColumnName("Season_Id");

                entity.Property(e => e.SeasonYearId).HasColumnName("SeasonYear_Id");

                entity.Property(e => e.ServiceDateFrom)
                    .HasColumnName("ServiceDate_From")
                    .HasColumnType("datetime");

                entity.Property(e => e.ServiceDateTo)
                    .HasColumnName("ServiceDate_To")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");

                entity.HasOne(d => d.AuditType)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.AuditTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_Trans__Audit__7F785DF7");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__AUD_Trans__Brand__74FACF84");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__AUD_Trans__Creat__7C9BF14C");

                entity.HasOne(d => d.CuProductCategoryNavigation)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.CuProductCategory)
                    .HasConstraintName("FK_AUD_Transaction_CU_ProductCategory");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_Trans__Custo__7406AB4B");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__AUD_Trans__Depar__75EEF3BD");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_Trans__Entit__7D901585");

                entity.HasOne(d => d.Evalution)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.EvalutionId)
                    .HasConstraintName("FK__AUD_Trans__Evalu__7AB3A8DA");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.AudTransactionFactories)
                    .HasForeignKey(d => d.FactoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_Trans__Facto__79BF84A1");

                entity.HasOne(d => d.FbfillingStatusNavigation)
                    .WithMany(p => p.AudTransactionFbfillingStatusNavigations)
                    .HasForeignKey(d => d.FbfillingStatus)
                    .HasConstraintName("AUD_Transaction_FBFillingStatus");

                entity.HasOne(d => d.FbreportStatusNavigation)
                    .WithMany(p => p.AudTransactionFbreportStatusNavigations)
                    .HasForeignKey(d => d.FbreportStatus)
                    .HasConstraintName("AUD_Transaction_FBReportStatus");

                entity.HasOne(d => d.FbreviewStatusNavigation)
                    .WithMany(p => p.AudTransactionFbreviewStatusNavigations)
                    .HasForeignKey(d => d.FbreviewStatus)
                    .HasConstraintName("AUD_Transaction_FBReviewStatus");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK__AUD_Trans__Offic__7BA7CD13");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.SeasonId)
                    .HasConstraintName("FK__AUD_Trans__Seaso__76E317F6");

                entity.HasOne(d => d.SeasonYear)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.SeasonYearId)
                    .HasConstraintName("FK__AUD_Trans__Seaso__77D73C2F");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.AudTransactions)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_Trans__Statu__7E8439BE");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.AudTransactionSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AUD_Trans__Suppl__78CB6068");
            });

            modelBuilder.Entity<AudType>(entity =>
            {
                entity.ToTable("AUD_Type");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_Type__Entity__006C8230");
            });

            modelBuilder.Entity<AudWorkProcess>(entity =>
            {
                entity.ToTable("AUD_WorkProcess");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.AudWorkProcesses)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__AUD_WorkP__Entit__0160A669");
            });

            modelBuilder.Entity<ClmRefCustomerRequest>(entity =>
            {
                entity.ToTable("CLM_REF_CustomerRequest");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefDefectDistribution>(entity =>
            {
                entity.ToTable("CLM_REF_DefectDistribution");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefDefectFamily>(entity =>
            {
                entity.ToTable("CLM_REF_DefectFamily");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ClmRefDepartment>(entity =>
            {
                entity.ToTable("CLM_REF_Department");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefFileType>(entity =>
            {
                entity.ToTable("CLM_REF_FileType");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefFrom>(entity =>
            {
                entity.ToTable("CLM_REF_FROM");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefPriority>(entity =>
            {
                entity.ToTable("CLM_REF_Priority");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefReceivedFrom>(entity =>
            {
                entity.ToTable("CLM_REF_ReceivedFrom");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefRefundType>(entity =>
            {
                entity.ToTable("CLM_REF_RefundType");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefResult>(entity =>
            {
                entity.ToTable("CLM_REF_Result");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefSource>(entity =>
            {
                entity.ToTable("CLM_REF_Source");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmRefStatus>(entity =>
            {
                entity.ToTable("CLM_REF_Status");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ClmTranAttachment>(entity =>
            {
                entity.ToTable("CLM_TRAN_Attachments");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(1000);

                entity.Property(e => e.UniqueId).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranAttachments)
                    .HasForeignKey(d => d.ClaimId)
                    .HasConstraintName("FK__CLM_TRAN___Claim__25A9BF2E");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranAttachmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Creat__22CD5283");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranAttachmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_TRAN___Delet__24B59AF5");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ClmTranAttachments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__CLM_TRAN___Entit__279207A0");

                entity.HasOne(d => d.FileTypeNavigation)
                    .WithMany(p => p.ClmTranAttachments)
                    .HasForeignKey(d => d.FileType)
                    .HasConstraintName("FK__CLM_TRAN___FileT__269DE367");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranAttachmentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Updat__23C176BC");
            });

            modelBuilder.Entity<ClmTranClaimRefund>(entity =>
            {
                entity.ToTable("CLM_TRAN_ClaimRefund");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranClaimRefunds)
                    .HasForeignKey(d => d.Claimid)
                    .HasConstraintName("FK_CLM_TRAN_ClaimRefund_CLM_Transaction");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranClaimRefundCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CLM_TRAN_ClaimRefund_IT_UserMaster");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranClaimRefundDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CLM_TRAN_ClaimRefund_IT_UserMaster2");

                entity.HasOne(d => d.RefundType)
                    .WithMany(p => p.ClmTranClaimRefunds)
                    .HasForeignKey(d => d.RefundTypeId)
                    .HasConstraintName("FK_CLM_TRAN_ClaimRefund_CLM_REF_RefundType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranClaimRefundUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CLM_TRAN_ClaimRefund_IT_UserMaster1");
            });

            modelBuilder.Entity<ClmTranCustomerRequest>(entity =>
            {
                entity.ToTable("CLM_TRAN_CustomerRequest");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranCustomerRequests)
                    .HasForeignKey(d => d.Claimid)
                    .HasConstraintName("FK__CLM_TRAN___Claim__0CDE1164");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranCustomerRequestCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Creat__0EC659D6");

                entity.HasOne(d => d.CustomerRequest)
                    .WithMany(p => p.ClmTranCustomerRequests)
                    .HasForeignKey(d => d.CustomerRequestId)
                    .HasConstraintName("FK__CLM_TRAN___Custo__0DD2359D");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranCustomerRequestDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_TRAN___Delet__10AEA248");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranCustomerRequestUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Updat__0FBA7E0F");
            });

            modelBuilder.Entity<ClmTranCustomerRequestRefund>(entity =>
            {
                entity.ToTable("CLM_TRAN_CustomerRequestRefund");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranCustomerRequestRefunds)
                    .HasForeignKey(d => d.Claimid)
                    .HasConstraintName("FK__CLM_TRAN___Claim__138B0EF3");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranCustomerRequestRefundCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Creat__15735765");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranCustomerRequestRefundDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_TRAN___Delet__175B9FD7");

                entity.HasOne(d => d.RefundType)
                    .WithMany(p => p.ClmTranCustomerRequestRefunds)
                    .HasForeignKey(d => d.RefundTypeId)
                    .HasConstraintName("FK__CLM_TRAN___Refun__147F332C");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranCustomerRequestRefundUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Updat__16677B9E");
            });

            modelBuilder.Entity<ClmTranDefectFamily>(entity =>
            {
                entity.ToTable("CLM_TRAN_DefectFamily");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranDefectFamilies)
                    .HasForeignKey(d => d.ClaimId)
                    .HasConstraintName("FK__CLM_TRAN___Claim__78D718B7");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranDefectFamilyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Creat__7ABF6129");

                entity.HasOne(d => d.DefectFamily)
                    .WithMany(p => p.ClmTranDefectFamilies)
                    .HasForeignKey(d => d.DefectFamilyId)
                    .HasConstraintName("FK__CLM_TRAN___Defec__79CB3CF0");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranDefectFamilyDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_TRAN___Delet__7CA7A99B");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranDefectFamilyUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Updat__7BB38562");
            });

            modelBuilder.Entity<ClmTranDepartment>(entity =>
            {
                entity.ToTable("CLM_TRAN_Department");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranDepartments)
                    .HasForeignKey(d => d.Claimid)
                    .HasConstraintName("FK__CLM_TRAN___Claim__063113D5");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranDepartmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Creat__08195C47");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranDepartmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_TRAN___Delet__0A01A4B9");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.ClmTranDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__CLM_TRAN___Depar__0725380E");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranDepartmentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Updat__090D8080");
            });

            modelBuilder.Entity<ClmTranFinalDecision>(entity =>
            {
                entity.ToTable("CLM_TRAN_FinalDecision");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranFinalDecisions)
                    .HasForeignKey(d => d.Claimid)
                    .HasConstraintName("FK__CLM_TRAN___Claim__1C2054F4");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranFinalDecisionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Creat__1E089D66");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranFinalDecisionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_TRAN___Delet__1FF0E5D8");

                entity.HasOne(d => d.FinalDecisionNavigation)
                    .WithMany(p => p.ClmTranFinalDecisions)
                    .HasForeignKey(d => d.FinalDecision)
                    .HasConstraintName("FK__CLM_TRAN___Final__1D14792D");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranFinalDecisionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Updat__1EFCC19F");
            });

            modelBuilder.Entity<ClmTranReport>(entity =>
            {
                entity.ToTable("CLM_TRAN_Reports");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.ClmTranReports)
                    .HasForeignKey(d => d.ClaimId)
                    .HasConstraintName("FK__CLM_TRAN___Claim__722A1B28");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTranReportCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Creat__7412639A");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTranReportDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_TRAN___Delet__75FAAC0C");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ClmTranReports)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK__CLM_TRAN___Repor__731E3F61");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTranReportUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_TRAN___Updat__750687D3");
            });

            modelBuilder.Entity<ClmTransaction>(entity =>
            {
                entity.ToTable("CLM_Transaction");

                entity.Property(e => e.AnalyzedOn).HasColumnType("datetime");

                entity.Property(e => e.AnalyzerFeedback).IsUnicode(false);

                entity.Property(e => e.ClaimDate).HasColumnType("datetime");

                entity.Property(e => e.ClaimNo).HasMaxLength(50);

                entity.Property(e => e.ClaimRecommendation).HasMaxLength(2000);

                entity.Property(e => e.ClaimRefundRemarks).HasMaxLength(2000);

                entity.Property(e => e.ClaimRemarks).HasMaxLength(2000);

                entity.Property(e => e.ClosedOn).HasColumnType("datetime");

                entity.Property(e => e.Color).HasMaxLength(1000);

                entity.Property(e => e.CompareToAql).HasColumnName("CompareToAQL");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DefectCartonInspected).HasMaxLength(500);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FobCurrency).HasColumnName("FOB_Currency");

                entity.Property(e => e.FobPrice).HasColumnName("FOB_Price");

                entity.Property(e => e.Qccontrol100goods).HasColumnName("QCControl_100Goods");

                entity.Property(e => e.RequestedContactName).HasMaxLength(100);

                entity.Property(e => e.RetailCurrency).HasColumnName("Retail_Currency");

                entity.Property(e => e.RetailPrice).HasColumnName("Retail_Price");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.ValidatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AnalyzedByNavigation)
                    .WithMany(p => p.ClmTransactionAnalyzedByNavigations)
                    .HasForeignKey(d => d.AnalyzedBy)
                    .HasConstraintName("FK_CLM_Transaction_IT_UserMaster");

                entity.HasOne(d => d.ClaimRefundCurrencyNavigation)
                    .WithMany(p => p.ClmTransactionClaimRefundCurrencyNavigations)
                    .HasForeignKey(d => d.ClaimRefundCurrency)
                    .HasConstraintName("FK__CLM_Trans__Claim__6B7D1D99");

                entity.HasOne(d => d.ClaimSourceNavigation)
                    .WithMany(p => p.ClmTransactions)
                    .HasForeignKey(d => d.ClaimSource)
                    .HasConstraintName("FK__CLM_Trans__Claim__66B8687C");

                entity.HasOne(d => d.ClosedByNavigation)
                    .WithMany(p => p.ClmTransactionClosedByNavigations)
                    .HasForeignKey(d => d.ClosedBy)
                    .HasConstraintName("FK_CLM_Transaction_IT_UserMaster2");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ClmTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CLM_Trans__Creat__6D65660B");

                entity.HasOne(d => d.CustomerPriorityNavigation)
                    .WithMany(p => p.ClmTransactions)
                    .HasForeignKey(d => d.CustomerPriority)
                    .HasConstraintName("FK__CLM_Trans__Custo__67AC8CB5");

                entity.HasOne(d => d.CustomerReqRefundCurrencyNavigation)
                    .WithMany(p => p.ClmTransactionCustomerReqRefundCurrencyNavigations)
                    .HasForeignKey(d => d.CustomerReqRefundCurrency)
                    .HasConstraintName("FK__CLM_Trans__Custo__68A0B0EE");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ClmTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CLM_Trans__Delet__6F4DAE7D");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ClmTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_CLM_Transaction_EntityId");

                entity.HasOne(d => d.FobCurrencyNavigation)
                    .WithMany(p => p.ClmTransactionFobCurrencyNavigations)
                    .HasForeignKey(d => d.FobCurrency)
                    .HasConstraintName("FK__CLM_Trans__FOB_C__6994D527");

                entity.HasOne(d => d.InspectionNoNavigation)
                    .WithMany(p => p.ClmTransactions)
                    .HasForeignKey(d => d.InspectionNo)
                    .HasConstraintName("FK__CLM_Trans__Inspe__63DBFBD1");

                entity.HasOne(d => d.RealInspectionFeesCurrencyNavigation)
                    .WithMany(p => p.ClmTransactionRealInspectionFeesCurrencyNavigations)
                    .HasForeignKey(d => d.RealInspectionFeesCurrency)
                    .HasConstraintName("FK__CLM_Trans__RealI__6C7141D2");

                entity.HasOne(d => d.ReceivedFromNavigation)
                    .WithMany(p => p.ClmTransactions)
                    .HasForeignKey(d => d.ReceivedFrom)
                    .HasConstraintName("FK__CLM_Trans__Recei__65C44443");

                entity.HasOne(d => d.RetailCurrencyNavigation)
                    .WithMany(p => p.ClmTransactionRetailCurrencyNavigations)
                    .HasForeignKey(d => d.RetailCurrency)
                    .HasConstraintName("FK__CLM_Trans__Retai__6A88F960");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ClmTransactions)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__CLM_Trans__Statu__64D0200A");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ClmTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CLM_Trans__Updat__6E598A44");

                entity.HasOne(d => d.ValidatedByNavigation)
                    .WithMany(p => p.ClmTransactionValidatedByNavigations)
                    .HasForeignKey(d => d.ValidatedBy)
                    .HasConstraintName("FK_CLM_Transaction_IT_UserMaster1");
            });

            modelBuilder.Entity<CompComplaint>(entity =>
            {
                entity.ToTable("COMP_Complaints");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.ComplaintDate)
                    .HasColumnName("Complaint_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.RecipientType).HasColumnName("Recipient_Type");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("FK_AUD_Transaction_Audit_Id");

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.Country)
                    .HasConstraintName("FK_REF_Country_Country");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CompComplaintCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_IT_UserMaster_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CU_Customer_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CompComplaintDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_IT_UserMaster_DeletedBy");

                entity.HasOne(d => d.DepartmentNavigation)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.Department)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMP_REF_Department_Department");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_INSP_Transaction_Inspection_Id");

                entity.HasOne(d => d.OfficeNavigation)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.Office)
                    .HasConstraintName("FK_REF_Location_Office");

                entity.HasOne(d => d.RecipientTypeNavigation)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.RecipientType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMP_REF_Recipient_Type_Recipient_Type");

                entity.HasOne(d => d.ServiceNavigation)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.Service)
                    .HasConstraintName("FK_REF_Service_Service");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.CompComplaints)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMP_REF_Type_Type");
            });

            modelBuilder.Entity<CompRefCategory>(entity =>
            {
                entity.ToTable("COMP_REF_Category");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CompRefDepartment>(entity =>
            {
                entity.ToTable("COMP_REF_Department");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CompRefRecipientType>(entity =>
            {
                entity.ToTable("COMP_REF_Recipient_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CompRefType>(entity =>
            {
                entity.ToTable("COMP_REF_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CompTranComplaintsDetail>(entity =>
            {
                entity.ToTable("COMP_TRAN_ComplaintsDetails");

                entity.Property(e => e.AnswerDate).HasColumnType("datetime");

                entity.Property(e => e.ComplaintCategory).HasColumnName("Complaint_Category");

                entity.Property(e => e.ComplaintDescription).HasColumnName("Complaint_Description");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ComplaintCategoryNavigation)
                    .WithMany(p => p.CompTranComplaintsDetails)
                    .HasForeignKey(d => d.ComplaintCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMP_REF_Category_Complaint_Category");

                entity.HasOne(d => d.Complaint)
                    .WithMany(p => p.CompTranComplaintsDetails)
                    .HasForeignKey(d => d.ComplaintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMP_Complaints_ComplaintId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CompTranComplaintsDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_TRAN_ComplaintsDetails_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CompTranComplaintsDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_TRAN_ComplaintsDetails_DeletedBy");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CompTranComplaintsDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CU_Products_ProductId");
            });

            modelBuilder.Entity<CompTranPersonInCharge>(entity =>
            {
                entity.ToTable("COMP_TRAN_PersonInCharge");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Complaint)
                    .WithMany(p => p.CompTranPersonInCharges)
                    .HasForeignKey(d => d.ComplaintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMP_Complaints_ComplaintsId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CompTranPersonInChargeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_IT_UserMaster_Comp_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CompTranPersonInChargeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_IT_UserMaster_Comp_DeletedBy");

                entity.HasOne(d => d.PsersonInChargeNavigation)
                    .WithMany(p => p.CompTranPersonInCharges)
                    .HasForeignKey(d => d.PsersonInCharge)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HR_Staff_PsersonInCharge");
            });

            modelBuilder.Entity<CuAddress>(entity =>
            {
                entity.ToTable("CU_Address");

                entity.Property(e => e.AddressType).HasColumnName("Address_Type");

                entity.Property(e => e.BoxPost)
                    .HasColumnName("Box_Post")
                    .HasMaxLength(100);

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.CountryId).HasColumnName("Country_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.ZipCode)
                    .HasColumnName("Zip_Code")
                    .HasMaxLength(20);

                entity.HasOne(d => d.AddressTypeNavigation)
                    .WithMany(p => p.CuAddresses)
                    .HasForeignKey(d => d.AddressType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Address_REF_AddressType");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CuAddresses)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__CU_Addres__City___043D1314");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CuAddresses)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK__CU_Addres__Count__0348EEDB");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuAddressCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Addres__Creat__2C35F5D5");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuAddresses)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CU_Addres__Custo__0254CAA2");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuAddressDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Addres__Delet__2D2A1A0E");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuAddresses)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Address_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuAddressUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Addres__Updat__2E1E3E47");
            });

            modelBuilder.Entity<CuApiService>(entity =>
            {
                entity.ToTable("CU_API_Services");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuApiServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_API_Se__Creat__499C3B8A");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuApiServices)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CU_API_Se__Custo__4B8483FC");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuApiServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_API_Se__Delet__4A905FC3");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuApiServices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_API_Services_EntityId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuApiServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_CU_API_Service");
            });

            modelBuilder.Entity<CuBrand>(entity =>
            {
                entity.ToTable("CU_Brand");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuBrandCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Brand__Create__1EDBFAB7");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuBrands)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Brand__Custom__0531374D");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuBrandDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Brand__Delete__1FD01EF0");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuBrands)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Brand_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuBrandUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Brand__Update__20C44329");
            });

            modelBuilder.Entity<CuBrandpriority>(entity =>
            {
                entity.ToTable("CU_Brandpriority");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Brandpriority)
                    .WithMany(p => p.CuBrandpriorities)
                    .HasForeignKey(d => d.BrandpriorityId)
                    .HasConstraintName("FK_CU_Brandpriority_Cu_REF_BrandPriority");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuBrandpriorities)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CU_Brandpriority_CU_Customer");
            });

            modelBuilder.Entity<CuBuyer>(entity =>
            {
                entity.ToTable("CU_Buyer");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuBuyerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Buyer__Create__2494D40D");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuBuyers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Buyer__Custom__3846C6FF");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuBuyerDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Buyer__Delete__2588F846");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuBuyers)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Buyer_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuBuyerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Buyer__Update__267D1C7F");
            });

            modelBuilder.Entity<CuBuyerApiService>(entity =>
            {
                entity.ToTable("CU_Buyer_API_Services");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.CuBuyerApiServices)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Buyer___Buyer__607FA0E2");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuBuyerApiServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Buyer___Creat__6267E954");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuBuyerApiServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Buyer___Delet__635C0D8D");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuBuyerApiServices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Buyer_API_Services_EntityId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuBuyerApiServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Buyer_API_Service");
            });

            modelBuilder.Entity<CuCheckPoint>(entity =>
            {
                entity.ToTable("CU_CheckPoints");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CheckpointType)
                    .WithMany(p => p.CuCheckPoints)
                    .HasForeignKey(d => d.CheckpointTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CPCheckpointTypeId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuCheckPointCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CPCreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuCheckPoints)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CPCustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuCheckPointDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CPDeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCheckPoints)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_CheckPoints_EntityId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.CuCheckPointModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_CPModifiedBy");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuCheckPoints)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CPServiceId");
            });

            modelBuilder.Entity<CuCheckPointType>(entity =>
            {
                entity.ToTable("CU_CheckPointType");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.Name).HasMaxLength(1500);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCheckPointTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__CU_CheckP__Entit__176FE319");
            });

            modelBuilder.Entity<CuCheckPointsBrand>(entity =>
            {
                entity.ToTable("CU_CheckPoints_Brand");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.CuCheckPointsBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__Brand__65645190");

                entity.HasOne(d => d.Checkpoint)
                    .WithMany(p => p.CuCheckPointsBrands)
                    .HasForeignKey(d => d.CheckpointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__Check__6287E4E5");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuCheckPointsBrandCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_CheckP__Creat__637C091E");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCheckPointsBrands)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_CheckPoints_Brand_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuCheckPointsBrandUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_CheckP__Updat__64702D57");
            });

            modelBuilder.Entity<CuCheckPointsCountry>(entity =>
            {
                entity.ToTable("CU_CheckPoints_Country");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Checkpoint)
                    .WithMany(p => p.CuCheckPointsCountries)
                    .HasForeignKey(d => d.CheckpointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__Check__179B96F9");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CuCheckPointsCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__Count__188FBB32");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCheckPointsCountries)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__CU_CheckP__Entit__1983DF6B");
            });

            modelBuilder.Entity<CuCheckPointsDepartment>(entity =>
            {
                entity.ToTable("CU_CheckPoints_Department");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Checkpoint)
                    .WithMany(p => p.CuCheckPointsDepartments)
                    .HasForeignKey(d => d.CheckpointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__Check__5CCF0B8F");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuCheckPointsDepartmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_CheckP__Creat__5DC32FC8");

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.CuCheckPointsDepartments)
                    .HasForeignKey(d => d.DeptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__DeptI__5FAB783A");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCheckPointsDepartments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_CheckPoints_Department_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuCheckPointsDepartmentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_CheckP__Updat__5EB75401");
            });

            modelBuilder.Entity<CuCheckPointsServiceType>(entity =>
            {
                entity.ToTable("CU_CheckPoints_ServiceType");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Checkpoint)
                    .WithMany(p => p.CuCheckPointsServiceTypes)
                    .HasForeignKey(d => d.CheckpointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__Check__6840BE3B");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuCheckPointsServiceTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_CheckP__Creat__6934E274");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCheckPointsServiceTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_CheckPoints_ServiceType_EntityId");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.CuCheckPointsServiceTypes)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CheckP__Servi__6B1D2AE6");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuCheckPointsServiceTypeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_CheckP__Updat__6A2906AD");
            });

            modelBuilder.Entity<CuCollection>(entity =>
            {
                entity.ToTable("CU_Collection");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuCollectionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_Collection_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuCollections)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CU_Collec__Custo__605FA551");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuCollectionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_Collection_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCollections)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Collection_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuCollectionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_Collection_UpdatedBy");
            });

            modelBuilder.Entity<CuContact>(entity =>
            {
                entity.ToTable("CU_Contact");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Comments).HasMaxLength(2200);

                entity.Property(e => e.ContactName)
                    .HasColumnName("Contact_name")
                    .HasMaxLength(1200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(200);

                entity.Property(e => e.JobTitle)
                    .HasColumnName("Job_Title")
                    .HasMaxLength(250);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Others).HasMaxLength(2200);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PromotionalEmail).HasColumnName("Promotional_Email");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Contac__Creat__182EFD28");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuContacts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Contac__Custo__06255B86");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Contac__Delet__19232161");

                entity.HasOne(d => d.OfficeNavigation)
                    .WithMany(p => p.CuContacts)
                    .HasForeignKey(d => d.Office)
                    .HasConstraintName("FK__CU_Contac__Offic__1B0B69D3");

                entity.HasOne(d => d.PrimaryEntityNavigation)
                    .WithMany(p => p.CuContacts)
                    .HasForeignKey(d => d.PrimaryEntity)
                    .HasConstraintName("FK_CU_Contact_PrimaryEntity");

                entity.HasOne(d => d.ReportToNavigation)
                    .WithMany(p => p.InverseReportToNavigation)
                    .HasForeignKey(d => d.ReportTo)
                    .HasConstraintName("FK_CU_Contact_CU_Contact");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuContactUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Contac__Updat__1A17459A");
            });

            modelBuilder.Entity<CuContactBrand>(entity =>
            {
                entity.ToTable("CU_Contact_Brand");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.CuContactBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Contac__Brand__48D23483");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.CuContactBrands)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Contac__Conta__49C658BC");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuContactBrandCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Contac__Creat__4ABA7CF5");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuContactBrandDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Contac__Delet__4BAEA12E");
            });

            modelBuilder.Entity<CuContactDepartment>(entity =>
            {
                entity.ToTable("CU_Contact_Department");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.CuContactDepartments)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Contac__Conta__4E8B0DD9");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuContactDepartmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Contac__Creat__4F7F3212");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuContactDepartmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Contac__Delet__5073564B");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.CuContactDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Contac__Depar__51677A84");
            });

            modelBuilder.Entity<CuContactEntityMap>(entity =>
            {
                entity.HasKey(e => new { e.ContactId, e.EntityId })
                    .HasName("PK__CU_Conta__95AEB7624814C9D6");

                entity.ToTable("CU_Contact_Entity_Map");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.CuContactEntityMaps)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Contact_Entity_Map_ContactId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuContactEntityMaps)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Contact_Entity_Map_EntityId");
            });

            modelBuilder.Entity<CuContactEntityServiceMap>(entity =>
            {
                entity.ToTable("CU_Contact_Entity_Service_Map");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.CuContactEntityServiceMaps)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_CU_Contact_Entity_Service_Map_ContactId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuContactEntityServiceMaps)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_CU_Contact_Entity_Service_Map_EntityId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuContactEntityServiceMaps)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_CU_Contact_Entity_Service_Map_ServiceId");
            });

            modelBuilder.Entity<CuContactService>(entity =>
            {
                entity.ToTable("Cu_Contact_Service");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.CuContactServices)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cu_Contac__Conta__5443E72F");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuContactServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Cu_Contac__Creat__55380B68");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuContactServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__Cu_Contac__Delet__562C2FA1");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuContactServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cu_Contac__Servi__572053DA");
            });

            modelBuilder.Entity<CuContactSisterCompany>(entity =>
            {
                entity.ToTable("CU_Contact_SisterCompany");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.CuContactSisterCompanies)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Contact_SisterCompany_ContactId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuContactSisterCompanyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_Contact_SisterCompany_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuContactSisterCompanyDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_Contact_SisterCompany_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuContactSisterCompanies)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Contact_SisterCompany_EntityId");

                entity.HasOne(d => d.SisterCompany)
                    .WithMany(p => p.CuContactSisterCompanies)
                    .HasForeignKey(d => d.SisterCompanyId)
                    .HasConstraintName("FK_CU_Contact_SisterCompany_SisterCompanyId");
            });

            modelBuilder.Entity<CuContactType>(entity =>
            {
                entity.ToTable("CU_ContactType");

                entity.Property(e => e.ContactType).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuContactTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__CU_Contac__Entit__07197FBF");
            });

            modelBuilder.Entity<CuCsConfiguration>(entity =>
            {
                entity.ToTable("CU_CS_Configuration");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.OfficeLocationId).HasColumnName("Office_Location_Id");

                entity.Property(e => e.ProductCategoryId).HasColumnName("Product_category_Id");

                entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuCsConfigurations)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CU_CS_Con__Custo__049218CF");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCsConfigurations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_CS_Configuration_EntityId");

                entity.HasOne(d => d.OfficeLocation)
                    .WithMany(p => p.CuCsConfigurations)
                    .HasForeignKey(d => d.OfficeLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CS_Con__Offic__05863D08");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.CuCsConfigurations)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__CU_CS_Con__Produ__067A6141");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuCsConfigurations)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__CU_CS_Con__Servi__076E857A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CuCsConfigurations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CS_Con__User___0862A9B3");
            });

            modelBuilder.Entity<CuCsOnsiteEmail>(entity =>
            {
                entity.ToTable("CU_CS_Onsite_Email");

                entity.Property(e => e.EmailId).IsRequired();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuCsOnsiteEmails)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CS_Ons__Custo__59A88621");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CuCsOnsiteEmails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_CS_Ons__UserI__58B461E8");
            });

            modelBuilder.Entity<CuCustomer>(entity =>
            {
                entity.ToTable("CU_Customer");

                entity.Property(e => e.BusinessCountry).HasColumnName("Business_Country");

                entity.Property(e => e.BusinessType).HasColumnName("Business_Type");

                entity.Property(e => e.ComplexityLevel).HasColumnName("Complexity_Level");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasColumnName("Customer_Name")
                    .HasMaxLength(500);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DirectCompetitor).HasMaxLength(2000);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(100);

                entity.Property(e => e.FbCusId).HasColumnName("Fb_Cus_Id");

                entity.Property(e => e.GlCode)
                    .HasColumnName("Gl_Code")
                    .HasMaxLength(500);

                entity.Property(e => e.GlRequired).HasColumnName("Gl_Required");

                entity.Property(e => e.IcRequired).HasColumnName("Ic_Required");

                entity.Property(e => e.InvoiceType).HasColumnName("Invoice_Type");

                entity.Property(e => e.IsEaqf).HasColumnName("IsEAQF");

                entity.Property(e => e.MargetSegment).HasColumnName("Marget_Segment");

                entity.Property(e => e.OtherPhone)
                    .HasColumnName("Other_Phone")
                    .HasMaxLength(100);

                entity.Property(e => e.Others).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.ProspectStatus).HasColumnName("Prospect_Status");

                entity.Property(e => e.QuatationName)
                    .HasColumnName("Quatation_Name")
                    .HasMaxLength(500);

                entity.Property(e => e.SkillsRequired).HasColumnName("Skills_Required");

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.Website).HasMaxLength(100);

                entity.HasOne(d => d.AccountingLeader)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.AccountingLeaderId)
                    .HasConstraintName("FK_CU_Customer_CU_REF_AccountingLeader");

                entity.HasOne(d => d.ActvitiesLevel)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.ActvitiesLevelId)
                    .HasConstraintName("FK_CU_Customer_CU_REF_ActivitiesLevel");

                entity.HasOne(d => d.BusinessCountryNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.BusinessCountry)
                    .HasConstraintName("FK__CU_Custom__Busin__0CBD4A7C");

                entity.HasOne(d => d.BusinessTypeNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.BusinessType)
                    .HasConstraintName("FK__CU_Custom__Busin__0DB16EB5");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__CU_Custom__Compa__6BF21EE5");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuCustomerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Custom__Creat__0EA592EE");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuCustomerDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Custom__Delet__0F99B727");

                entity.HasOne(d => d.GroupNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.Group)
                    .HasConstraintName("FK__CU_Custom__Group__127623D2");

                entity.HasOne(d => d.InvoiceTypeNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.InvoiceType)
                    .HasConstraintName("FK__CU_Custom__Invoi__136A480B");

                entity.HasOne(d => d.KamNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.Kam)
                    .HasConstraintName("FK__CU_Customer__Kam__173AD8EF");

                entity.HasOne(d => d.LanguageNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.Language)
                    .HasConstraintName("FK__CU_Custom__Langu__145E6C44");

                entity.HasOne(d => d.MargetSegmentNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.MargetSegment)
                    .HasConstraintName("FK__CU_Custom__Marge__1552907D");

                entity.HasOne(d => d.ProspectStatusNavigation)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.ProspectStatus)
                    .HasConstraintName("FK__CU_Custom__Prosp__1646B4B6");

                entity.HasOne(d => d.RelationshipStatus)
                    .WithMany(p => p.CuCustomers)
                    .HasForeignKey(d => d.RelationshipStatusId)
                    .HasConstraintName("FK_CU_Customer_CU_REF_RelationshipStatus");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuCustomerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Custom__Updat__108DDB60");
            });

            modelBuilder.Entity<CuCustomerBusinessCountry>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.BusinessCountryId })
                    .HasName("PK__CU_Custo__A773F411ECB4E90A");

                entity.ToTable("CU_CustomerBusinessCountry");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.BusinessCountryId).HasColumnName("business_country_id");

                entity.HasOne(d => d.BusinessCountry)
                    .WithMany(p => p.CuCustomerBusinessCountries)
                    .HasForeignKey(d => d.BusinessCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Custom__busin__0BDE34DC");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuCustomerBusinessCountries)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Custom__custo__0CD25915");
            });

            modelBuilder.Entity<CuCustomerContactType>(entity =>
            {
                entity.HasKey(e => new { e.ContactId, e.ContactTypeId })
                    .HasName("PK__CU_Custo__CADD9C812058F168");

                entity.ToTable("CU_CustomerContactTypes");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.ContactTypeId).HasColumnName("contact_type_id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.CuCustomerContactTypes)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Custom__conta__0EBAA187");

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.CuCustomerContactTypes)
                    .HasForeignKey(d => d.ContactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Custom__conta__0DC67D4E");
            });

            modelBuilder.Entity<CuCustomerGroup>(entity =>
            {
                entity.ToTable("CU_CustomerGroup");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuCustomerGroups)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_CustomerGroup_EntityId");
            });

            modelBuilder.Entity<CuCustomerSalesCountry>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.SalesCountryId })
                    .HasName("PK__CU_Custo__B8DF48571BAA3668");

                entity.ToTable("CU_CustomerSalesCountries");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.SalesCountryId).HasColumnName("sales_country_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuCustomerSalesCountries)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Custom__custo__6D649AB4");

                entity.HasOne(d => d.SalesCountry)
                    .WithMany(p => p.CuCustomerSalesCountries)
                    .HasForeignKey(d => d.SalesCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Custom__sales__6C70767B");
            });

            modelBuilder.Entity<CuDepartment>(entity =>
            {
                entity.ToTable("CU_Department");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuDepartmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Depart__Creat__1BFF8E0C");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuDepartments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Depart__Custo__0FAEC5C0");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuDepartmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Depart__Delet__1CF3B245");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuDepartments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Department_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuDepartmentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Depart__Updat__1DE7D67E");
            });

            modelBuilder.Entity<CuEntity>(entity =>
            {
                entity.ToTable("CU_Entity");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuEntityCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Cu_Entity_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuEntities)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cu_Entity_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuEntityDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("Cu_Entity_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuEntities)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cu_Entity_EntityId");
            });

            modelBuilder.Entity<CuKam>(entity =>
            {
                entity.ToTable("CU_KAM");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.KamId).HasColumnName("KAM_Id");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuKams)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CU_KAM_CU_Customer");

                entity.HasOne(d => d.Kam)
                    .WithMany(p => p.CuKams)
                    .HasForeignKey(d => d.KamId)
                    .HasConstraintName("FK_CU_KAM_HR_Staff");
            });

            modelBuilder.Entity<CuPoFactory>(entity =>
            {
                entity.ToTable("CU_PO_Factory");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPoFactoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PO_Factory_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPoFactoryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PO_Factory_Deleted_By");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.CuPoFactories)
                    .HasForeignKey(d => d.FactoryId)
                    .HasConstraintName("FK_CU_PO_Factory_FactoryId");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.CuPoFactories)
                    .HasForeignKey(d => d.PoId)
                    .HasConstraintName("FK_CU_PO_Factory_PO_Id");
            });

            modelBuilder.Entity<CuPoSupplier>(entity =>
            {
                entity.ToTable("CU_PO_Supplier");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPoSupplierCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PO_Supplier_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPoSupplierDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PO_Supplier_Deleted_By");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.CuPoSuppliers)
                    .HasForeignKey(d => d.PoId)
                    .HasConstraintName("FK_CU_PO_Supplier_PO_Id");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.CuPoSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_CU_PO_Supplier_Supplier_Id");
            });

            modelBuilder.Entity<CuPrBrand>(entity =>
            {
                entity.ToTable("CU_PR_Brand");

                entity.Property(e => e.BrandId).HasColumnName("Brand_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.CuPrBrands)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__CU_PR_Bra__Brand__71552729");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrBrandCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_PR_Bra__Creat__733D6F9B");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrBrands)
                    .HasForeignKey(d => d.CuPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_PR_Bra__Cu_Pr__72494B62");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrBrandDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_PR_Bra__Delet__743193D4");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrBrandUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_PR_Bra__Updat__7525B80D");
            });

            modelBuilder.Entity<CuPrBuyer>(entity =>
            {
                entity.ToTable("CU_PR_Buyer");

                entity.Property(e => e.BuyerId).HasColumnName("Buyer_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.CuPrBuyers)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("FK__CU_PR_Buy__Buyer__780224B8");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrBuyerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_PR_Buy__Creat__79EA6D2A");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrBuyers)
                    .HasForeignKey(d => d.CuPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_PR_Buy__Cu_Pr__78F648F1");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrBuyerDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_PR_Buy__Delet__7ADE9163");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrBuyerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_PR_Buy__Updat__7BD2B59C");
            });
            modelBuilder.Entity<CuPrCity>(entity =>
            {
                entity.ToTable("CU_PR_City");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("Created_On")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CuPrId).HasColumnName("CU_PR_Id");

                entity.Property(e => e.DeletedBy).HasColumnName("Deleted_By");

                entity.Property(e => e.DeletedOn)
                    .HasColumnName("Deleted_On")
                    .HasColumnType("datetime");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.FactoryCityId).HasColumnName("Factory_City_Id");

                entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("Updated_On")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrCityCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_City_Created_By");

                entity.HasOne(d => d.CuPr)
                    .WithMany(p => p.CuPrCities)
                    .HasForeignKey(d => d.CuPrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_City_CU_PR_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrCityDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_City_Deleted_By");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuPrCities)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_CU_PR_City_Entity_Id");

                entity.HasOne(d => d.FactoryCity)
                    .WithMany(p => p.CuPrCities)
                    .HasForeignKey(d => d.FactoryCityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_City_Factory_City_Id");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrCityUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_City_Updated_By");
            });


            modelBuilder.Entity<CuPrCountry>(entity =>
            {
                entity.ToTable("CU_PR_Country");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CuPrId).HasColumnName("CU_PR_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrCountryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_Country_CreatedBy");

                entity.HasOne(d => d.CuPr)
                    .WithMany(p => p.CuPrCountries)
                    .HasForeignKey(d => d.CuPrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Country_CU_PR_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrCountryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_Country_DeletedBy");

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.CuPrCountries)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Country_FactoryCountryId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrCountryUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_Country_UpdatedBy");
            });

            modelBuilder.Entity<CuPrDepartment>(entity =>
            {
                entity.ToTable("CU_PR_Department");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrDepartmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_PR_Dep__Creat__00976AB9");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrDepartments)
                    .HasForeignKey(d => d.CuPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_PR_Dep__Cu_Pr__7FA34680");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrDepartmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_PR_Dep__Delet__018B8EF2");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.CuPrDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__CU_PR_Dep__Depar__7EAF2247");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrDepartmentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_PR_Dep__Updat__027FB32B");
            });

            modelBuilder.Entity<CuPrDetail>(entity =>
            {
                entity.ToTable("CU_PR_Details");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FreeTravelKm).HasColumnName("FreeTravelKM");

                entity.Property(e => e.InvoiceNoDigit).HasMaxLength(100);

                entity.Property(e => e.InvoiceNoPrefix).HasMaxLength(100);

                entity.Property(e => e.InvoiceRequestBilledName).HasMaxLength(2000);

                entity.Property(e => e.InvoiceSubject).HasMaxLength(1000);

                entity.Property(e => e.InvoiceTmfeeFrom).HasColumnName("InvoiceTMFeeFrom");

                entity.Property(e => e.ManDayProductivity).HasColumnName("ManDay_Productivity");

                entity.Property(e => e.MandayBuffer).HasColumnName("Manday_Buffer");

                entity.Property(e => e.MandayReportCount).HasColumnName("Manday_ReportCount");

                entity.Property(e => e.PaymentTerms).HasMaxLength(100);

                entity.Property(e => e.PeriodFrom).HasColumnType("datetime");

                entity.Property(e => e.PeriodTo).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(3000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BankAccountNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.BankAccount)
                    .HasConstraintName("CU_PR_Details_BankAccount");

                entity.HasOne(d => d.BilledQuantityTypeNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.BilledQuantityType)
                    .HasConstraintName("CU_PR_Details_BilledQuantityType");

                entity.HasOne(d => d.BillingEntityNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.BillingEntity)
                    .HasConstraintName("CU_PR_Details_BillingEntity");

                entity.HasOne(d => d.BillingFreequencyNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.BillingFreequency)
                    .HasConstraintName("CU_PR_Details_BillingFreequency");

                entity.HasOne(d => d.BillingMethod)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.BillingMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Details_BillingMethodId");

                entity.HasOne(d => d.BillingTo)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.BillingToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Details_BillingToId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_Details_CreatedBy");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Details_CurrencyId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Details_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_Details_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_PR_Details_EntityId");

                entity.HasOne(d => d.InterventionTypeNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.InterventionType)
                    .HasConstraintName("CU_PR_Details_InterventionType");

                entity.HasOne(d => d.InvoiceDiscountFeeFromNavigation)
                    .WithMany(p => p.CuPrDetailInvoiceDiscountFeeFromNavigations)
                    .HasForeignKey(d => d.InvoiceDiscountFeeFrom)
                    .HasConstraintName("CU_PR_Details_InvoiceDiscountFeeFrom");

                entity.HasOne(d => d.InvoiceHotelFeeFromNavigation)
                    .WithMany(p => p.CuPrDetailInvoiceHotelFeeFromNavigations)
                    .HasForeignKey(d => d.InvoiceHotelFeeFrom)
                    .HasConstraintName("CU_PR_Details_InvoiceHotelFeeFrom");

                entity.HasOne(d => d.InvoiceInspFeeFromNavigation)
                    .WithMany(p => p.CuPrDetailInvoiceInspFeeFromNavigations)
                    .HasForeignKey(d => d.InvoiceInspFeeFrom)
                    .HasConstraintName("CU_PR_Details_InvoiceTMFeeFrom");

                entity.HasOne(d => d.InvoiceOfficeNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.InvoiceOffice)
                    .HasConstraintName("CU_PR_Details_InvoiceOffice");

                entity.HasOne(d => d.InvoiceOtherFeeFromNavigation)
                    .WithMany(p => p.CuPrDetailInvoiceOtherFeeFromNavigations)
                    .HasForeignKey(d => d.InvoiceOtherFeeFrom)
                    .HasConstraintName("CU_PR_Details_InvoiceOtherFeeFrom");

                entity.HasOne(d => d.InvoiceRequestTypeNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.InvoiceRequestType)
                    .HasConstraintName("CU_PR_Details_InvoiceRequestType");

                entity.HasOne(d => d.InvoiceTmfeeFromNavigation)
                    .WithMany(p => p.CuPrDetailInvoiceTmfeeFromNavigations)
                    .HasForeignKey(d => d.InvoiceTmfeeFrom)
                    .HasConstraintName("CU_PR_Details_InvoiceTravleExpenseFrom");

                entity.HasOne(d => d.PriceComplexTypeNavigation)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.PriceComplexType)
                    .HasConstraintName("CU_PR_Details_PriceComplexType");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Details_ServiceId");

                entity.HasOne(d => d.TravelMatrixType)
                    .WithMany(p => p.CuPrDetails)
                    .HasForeignKey(d => d.TravelMatrixTypeId)
                    .HasConstraintName("CU_PR_Details_TravelMatrixTypeId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_Details_UpdatedBy");
            });

            modelBuilder.Entity<CuPrHolidayInfo>(entity =>
            {
                entity.ToTable("CU_PR_HolidayInfo");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CuPrHolidayType>(entity =>
            {
                entity.ToTable("CU_PR_HolidayType");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.HolidayInfoId).HasColumnName("HolidayInfo_Id");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrHolidayTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_PR_Hol__Creat__0DF165D7");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrHolidayTypes)
                    .HasForeignKey(d => d.CuPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_PR_Hol__Cu_Pr__0CFD419E");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrHolidayTypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_PR_Hol__Delet__0EE58A10");

                entity.HasOne(d => d.HolidayInfo)
                    .WithMany(p => p.CuPrHolidayTypes)
                    .HasForeignKey(d => d.HolidayInfoId)
                    .HasConstraintName("FK__CU_PR_Hol__Holid__0C091D65");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrHolidayTypeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_PR_Hol__Updat__0FD9AE49");
            });

            modelBuilder.Entity<CuPrInspectionLocation>(entity =>
            {
                entity.ToTable("CU_PR_InspectionLocation");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrInspectionLocationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_InspectionLocation_Created_By");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrInspectionLocations)
                    .HasForeignKey(d => d.CuPriceId)
                    .HasConstraintName("FK_CU_PR_InspectionLocation_Cu_Price_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrInspectionLocationDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_InspectionLocation_Deleted_By");

                entity.HasOne(d => d.InspectionLocation)
                    .WithMany(p => p.CuPrInspectionLocations)
                    .HasForeignKey(d => d.InspectionLocationId)
                    .HasConstraintName("FK_CU_PR_InspectionLocation_InspectionLocationId");
            });

            modelBuilder.Entity<CuPrPriceCategory>(entity =>
            {
                entity.ToTable("CU_PR_PriceCategory");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.PriceCategoryId).HasColumnName("PriceCategory_Id");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrPriceCategoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_PR_Pri__Creat__07446848");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrPriceCategories)
                    .HasForeignKey(d => d.CuPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_PR_Pri__Cu_Pr__0650440F");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrPriceCategoryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_PR_Pri__Delet__08388C81");

                entity.HasOne(d => d.PriceCategory)
                    .WithMany(p => p.CuPrPriceCategories)
                    .HasForeignKey(d => d.PriceCategoryId)
                    .HasConstraintName("FK__CU_PR_Pri__Price__055C1FD6");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrPriceCategoryUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_PR_Pri__Updat__092CB0BA");
            });

            modelBuilder.Entity<CuPrProductCategory>(entity =>
            {
                entity.ToTable("CU_PR_ProductCategory");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CuPrId).HasColumnName("CU_PR_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrProductCategoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_ProductCategory_CreatedBy");

                entity.HasOne(d => d.CuPr)
                    .WithMany(p => p.CuPrProductCategories)
                    .HasForeignKey(d => d.CuPrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_ProductCategory_CU_PR_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrProductCategoryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_ProductCategory_DeletedBy");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.CuPrProductCategories)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_ProductCategory_ProductCategoryId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrProductCategoryUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_ProductCategory_UpdatedBy");
            });

            modelBuilder.Entity<CuPrProductSubCategory>(entity =>
            {
                entity.ToTable("CU_PR_ProductSubCategory");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CuPrId).HasColumnName("CU_PR_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrProductSubCategoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_ProductSubCategory_CreatedBy");

                entity.HasOne(d => d.CuPr)
                    .WithMany(p => p.CuPrProductSubCategories)
                    .HasForeignKey(d => d.CuPrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_ProductSubCategory_CU_PR_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrProductSubCategoryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_ProductSubCategory_DeletedBy");

                entity.HasOne(d => d.ProductSubCategory)
                    .WithMany(p => p.CuPrProductSubCategories)
                    .HasForeignKey(d => d.ProductSubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_ProductSubCategory_ProductSubCategoryId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrProductSubCategoryUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_ProductSubCategory_UpdatedBy");
            });

            modelBuilder.Entity<CuPrProvince>(entity =>
            {
                entity.ToTable("CU_PR_Province");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CuPrId).HasColumnName("CU_PR_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrProvinceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_Province_CreatedBy");

                entity.HasOne(d => d.CuPr)
                    .WithMany(p => p.CuPrProvinces)
                    .HasForeignKey(d => d.CuPrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Province_CU_PR_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrProvinceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_Province_DeletedBy");

                entity.HasOne(d => d.FactoryProvince)
                    .WithMany(p => p.CuPrProvinces)
                    .HasForeignKey(d => d.FactoryProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Province_FactoryProvinceId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrProvinceUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_Province_UpdatedBy");
            });

            modelBuilder.Entity<CuPrRefComplexType>(entity =>
            {
                entity.ToTable("CU_PR_RefComplexType");

                entity.Property(e => e.Name).HasMaxLength(300);
            });

            modelBuilder.Entity<CuPrServiceType>(entity =>
            {
                entity.ToTable("CU_PR_ServiceType");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CuPrId).HasColumnName("CU_PR_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrServiceTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_ServiceType_CreatedBy");

                entity.HasOne(d => d.CuPr)
                    .WithMany(p => p.CuPrServiceTypes)
                    .HasForeignKey(d => d.CuPrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_ServiceType_CU_PR_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrServiceTypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_ServiceType_DeletedBy");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.CuPrServiceTypes)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_ServiceType_ServiceTypeId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrServiceTypeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_ServiceType_UpdatedBy");
            });

            modelBuilder.Entity<CuPrSupplier>(entity =>
            {
                entity.ToTable("CU_PR_Supplier");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CuPrId).HasColumnName("CU_PR_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrSupplierCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_PR_Supplier_CreatedBy");

                entity.HasOne(d => d.CuPr)
                    .WithMany(p => p.CuPrSuppliers)
                    .HasForeignKey(d => d.CuPrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Supplier_CU_PR_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrSupplierDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_PR_Supplier_DeletedBy");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.CuPrSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PR_Supplier_SupplierId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPrSupplierUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_PR_Supplier_UpdatedBy");
            });

            modelBuilder.Entity<CuPrTranSpecialRule>(entity =>
            {
                entity.ToTable("CU_PR_TRAN_SpecialRule");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.MaxStylePerDay).HasColumnName("Max_Style_Per_Day");

                entity.Property(e => e.MaxStylePerMonth).HasColumnName("Max_Style_Per_Month");

                entity.Property(e => e.MaxStylePerWeek).HasColumnName("Max_Style_Per_Week");

                entity.Property(e => e.PieceRateBillingQStart).HasColumnName("PieceRate_Billing_Q_Start");

                entity.Property(e => e.PiecerateBillingQEnd).HasColumnName("Piecerate_Billing_Q_End");

                entity.Property(e => e.PiecerateMinBilling).HasColumnName("Piecerate_MinBilling");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrTranSpecialRuleCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("CU_PR_TRAN_SpecialRule_CreatedBy");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrTranSpecialRules)
                    .HasForeignKey(d => d.CuPriceId)
                    .HasConstraintName("CU_PR_TRAN_SpecialRule_Cu_Price_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrTranSpecialRuleDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("CU_PR_TRAN_SpecialRule_DeletedBy");
            });

            modelBuilder.Entity<CuPrTranSubcategory>(entity =>
            {
                entity.ToTable("CU_PR_TRAN_Subcategory");

                entity.Property(e => e.AqlQty125).HasColumnName("AQL_QTY_125");

                entity.Property(e => e.AqlQty1250).HasColumnName("AQL_QTY_1250");

                entity.Property(e => e.AqlQty13).HasColumnName("AQL_QTY_13");

                entity.Property(e => e.AqlQty20).HasColumnName("AQL_QTY_20");

                entity.Property(e => e.AqlQty200).HasColumnName("AQL_QTY_200");

                entity.Property(e => e.AqlQty315).HasColumnName("AQL_QTY_315");

                entity.Property(e => e.AqlQty32).HasColumnName("AQL_QTY_32");

                entity.Property(e => e.AqlQty50).HasColumnName("AQL_QTY_50");

                entity.Property(e => e.AqlQty500).HasColumnName("AQL_QTY_500");

                entity.Property(e => e.AqlQty8).HasColumnName("AQL_QTY_8");

                entity.Property(e => e.AqlQty80).HasColumnName("AQL_QTY_80");

                entity.Property(e => e.AqlQty800).HasColumnName("AQL_QTY_800");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CuPriceId).HasColumnName("Cu_Price_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.SubCategory2Id).HasColumnName("Sub_Category2Id");

                entity.Property(e => e.SubCategoryId).HasColumnName("Sub_CategoryId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPrTranSubcategoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("CU_PR_TRAN_Subcategory_CreatedBy");

                entity.HasOne(d => d.CuPrice)
                    .WithMany(p => p.CuPrTranSubcategories)
                    .HasForeignKey(d => d.CuPriceId)
                    .HasConstraintName("CU_PR_TRAN_Subcategory_Cu_Price_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPrTranSubcategoryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("CU_PR_TRAN_Subcategory_DeletedBy");

                entity.HasOne(d => d.SubCategory2)
                    .WithMany(p => p.CuPrTranSubcategories)
                    .HasForeignKey(d => d.SubCategory2Id)
                    .HasConstraintName("CU_PR_TRAN_Subcategory_Sub_Category2Id");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.CuPrTranSubcategories)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("CU_PR_TRAN_Subcategory_Sub_CategoryId");
            });

            modelBuilder.Entity<CuPriceCategory>(entity =>
            {
                entity.ToTable("CU_PriceCategory");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuPriceCategories)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CU_PriceC__Custo__633C11FC");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuPriceCategories)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_PriceCategory_EntityId");
            });

            modelBuilder.Entity<CuPriceCategoryPcsub2>(entity =>
            {
                entity.ToTable("CU_PriceCategory_PCSub2");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuPriceCategoryPcsub2S)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CU_PriceCategory_PCSub2_CustomerId");

                entity.HasOne(d => d.PriceCategory)
                    .WithMany(p => p.CuPriceCategoryPcsub2S)
                    .HasForeignKey(d => d.PriceCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CU_PriceCategory_PCSub2_PriceCategoryId");

                entity.HasOne(d => d.ProductSubCategoryId2Navigation)
                    .WithMany(p => p.CuPriceCategoryPcsub2S)
                    .HasForeignKey(d => d.ProductSubCategoryId2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CU_PriceCategory_PCSub2_ProductSubCategoryId2");
            });

            modelBuilder.Entity<CuProduct>(entity =>
            {
                entity.ToTable("CU_Products");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.FactoryReference).HasMaxLength(1000);

                entity.Property(e => e.FbCusProdId).HasColumnName("Fb_Cus_Prod_Id");

                entity.Property(e => e.IsMsChart).HasColumnName("IsMS_Chart");

                entity.Property(e => e.ItRemarks).HasColumnName("IT_Remarks");

                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasColumnName("Product Description")
                    .HasMaxLength(3500);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("ProductID")
                    .HasMaxLength(200);

                entity.Property(e => e.Remarks).HasMaxLength(1000);

                entity.Property(e => e.SampleSize8h).HasColumnName("SampleSize_8h");

                entity.Property(e => e.TechnicalComments).HasMaxLength(1000);

                entity.Property(e => e.TpAdjustmentReason)
                    .HasColumnName("Tp_AdjustmentReason")
                    .HasMaxLength(1000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuProducts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Produc__Custo__15679F16");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuProducts)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Products_EntityId");

                entity.HasOne(d => d.ProductCategoryNavigation)
                    .WithMany(p => p.CuProducts)
                    .HasForeignKey(d => d.ProductCategory)
                    .HasConstraintName("FK__CU_Produc__Produ__128B326B");

                entity.HasOne(d => d.ProductCategorySub2Navigation)
                    .WithMany(p => p.CuProducts)
                    .HasForeignKey(d => d.ProductCategorySub2)
                    .HasConstraintName("FK_ProductCategorySub2");

                entity.HasOne(d => d.ProductCategorySub3Navigation)
                    .WithMany(p => p.CuProducts)
                    .HasForeignKey(d => d.ProductCategorySub3)
                    .HasConstraintName("FK_CuProducts_ProductCategorySub3Id");

                entity.HasOne(d => d.ProductSubCategoryNavigation)
                    .WithMany(p => p.CuProducts)
                    .HasForeignKey(d => d.ProductSubCategory)
                    .HasConstraintName("FK__CU_Produc__Produ__137F56A4");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuProducts)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_Product_UpdatedBy");
            });

            modelBuilder.Entity<CuProductApiService>(entity =>
            {
                entity.ToTable("CU_Product_API_Services");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuProductApiServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Produc__Creat__6820C2AA");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuProductApiServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Produc__Delet__6914E6E3");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CuProductApiServices)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Produc__Produ__66387A38");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuProductApiServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Product_API_Service");
            });

            modelBuilder.Entity<CuProductCategory>(entity =>
            {
                entity.ToTable("CU_ProductCategory");

                entity.Property(e => e.Code).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Sector).HasMaxLength(500);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuProductCategories)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CUSTOMER_CU_PRODUCT_CATEGORY");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuProductCategories)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_CU_PRODUCT_CATEGORY_ENTITY_ID");

                entity.HasOne(d => d.LinkProductSubCategoryNavigation)
                    .WithMany(p => p.CuProductCategories)
                    .HasForeignKey(d => d.LinkProductSubCategory)
                    .HasConstraintName("FK__CU_Produc__LinkP__4E2CB1D4");
            });

            modelBuilder.Entity<CuProductFileAttachment>(entity =>
            {
                entity.ToTable("CU_Product_File_Attachment");

                entity.Property(e => e.BookingId).HasColumnName("Booking_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FileTypeId).HasColumnName("FileType_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.CuProductFileAttachments)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__CU_Produc__Booki__246AB6DB");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuProductFileAttachmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Produc__Delet__237692A2");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuProductFileAttachments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Product_File_Attach_EntityId");

                entity.HasOne(d => d.FileType)
                    .WithMany(p => p.CuProductFileAttachments)
                    .HasForeignKey(d => d.FileTypeId)
                    .HasConstraintName("FK_CU_Product_File_Attachment_FileType_Id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CuProductFileAttachments)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Produc__Produ__218E4A30");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CuProductFileAttachmentUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Produc__UserI__22826E69");
            });

            modelBuilder.Entity<CuProductFileType>(entity =>
            {
                entity.ToTable("CU_Product_FileType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<CuProductMschart>(entity =>
            {
                entity.ToTable("CU_Product_MSChart");

                entity.Property(e => e.Code).HasMaxLength(1000);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Mpcode)
                    .HasColumnName("MPCode")
                    .HasMaxLength(500);

                entity.Property(e => e.ProductFileId).HasColumnName("Product_File_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.Tolerance1Down).HasColumnName("Tolerance1_Down");

                entity.Property(e => e.Tolerance1Up).HasColumnName("Tolerance1_Up");

                entity.Property(e => e.Tolerance2Down).HasColumnName("Tolerance2_Down");

                entity.Property(e => e.Tolerance2Up).HasColumnName("Tolerance2_Up");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuProductMschartCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_Product_MSChart_CreatedBy");

                entity.HasOne(d => d.ProductFile)
                    .WithMany(p => p.CuProductMscharts)
                    .HasForeignKey(d => d.ProductFileId)
                    .HasConstraintName("FK_CU_Product_MSChart_Product_File_Id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CuProductMscharts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Product_MSChart_Product_Id");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuProductMschartUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_CU_Product_MSChart_UpdatedBy");
            });

            modelBuilder.Entity<CuProductMschartOcrMap>(entity =>
            {
                entity.ToTable("CU_Product_MSChart_OCR_MAP");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.OcrCustomerName)
                    .HasColumnName("OCR_CustomerName")
                    .HasMaxLength(500);

                entity.Property(e => e.OcrFileFormat)
                    .HasColumnName("OCR_FileFormat")
                    .HasMaxLength(500);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuProductMschartOcrMapCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Product_MSChart_OCR_MAP_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuProductMschartOcrMaps)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Product_MSChart_OCR_MAP_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuProductMschartOcrMapDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_Product_MSChart_OCR_MAP_DeletedBy");
            });

            modelBuilder.Entity<CuProductType>(entity =>
            {
                entity.ToTable("CU_ProductType");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuProductTypes)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CU_Produc__Custo__51091E7F");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuProductTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__CU_Produc__Entit__51FD42B8");

                entity.HasOne(d => d.LinkProductTypeNavigation)
                    .WithMany(p => p.CuProductTypes)
                    .HasForeignKey(d => d.LinkProductType)
                    .HasConstraintName("FK__CU_Produc__LinkP__52F166F1");
            });

            modelBuilder.Entity<CuPurchaseOrder>(entity =>
            {
                entity.ToTable("CU_PurchaseOrder");

                entity.Property(e => e.BrandId).HasColumnName("Brand_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.CustomerReferencePo).HasMaxLength(1000);

                entity.Property(e => e.CustomerRemarks).HasMaxLength(1000);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.Property(e => e.InternalRemarks).HasMaxLength(1000);

                entity.Property(e => e.OfficeId).HasColumnName("Office_Id");

                entity.Property(e => e.Pono)
                    .IsRequired()
                    .HasColumnName("PONO")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.CuPurchaseOrders)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__CU_Purcha__Brand__19382FFA");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPurchaseOrderCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Purcha__Creat__1A2C5433");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuPurchaseOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CU_Purcha__Custo__174FE788");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPurchaseOrderDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Purcha__Delet__2F126280");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.CuPurchaseOrders)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__CU_Purcha__Depar__18440BC1");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuPurchaseOrders)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_PurchaseOrder_EntityId");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.CuPurchaseOrders)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK__CU_Purcha__Offic__165BC34F");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuPurchaseOrderUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Purcha__Updat__300686B9");
            });

            modelBuilder.Entity<CuPurchaseOrderAttachment>(entity =>
            {
                entity.ToTable("CU_PurchaseOrder_Attachment");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__CU_Purch__4B840D04DD05DAEE")
                    .IsUnique();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PoId).HasColumnName("Po_Id");

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.CuPurchaseOrderAttachments)
                    .HasForeignKey(d => d.PoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Purcha__Po_Id__1C149CA5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CuPurchaseOrderAttachments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Purcha__UserI__1B20786C");
            });

            modelBuilder.Entity<CuPurchaseOrderDetail>(entity =>
            {
                entity.ToTable("CU_PurchaseOrder_Details");

                entity.Property(e => e.BookingStatus).HasColumnName("Booking_Status");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedTime).HasColumnType("datetime");

                entity.Property(e => e.DestinationCountryId).HasColumnName("Destination_Country_Id");

                entity.Property(e => e.Etd)
                    .HasColumnName("ETD")
                    .HasColumnType("datetime");

                entity.Property(e => e.FactoryReference).HasMaxLength(1000);

                entity.Property(e => e.PoId).HasColumnName("PO_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.UnitId).HasColumnName("Unit_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuPurchaseOrderDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Purcha__Creat__21CD75FB");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuPurchaseOrderDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Purcha__Delet__22C19A34");

                entity.HasOne(d => d.DestinationCountry)
                    .WithMany(p => p.CuPurchaseOrderDetails)
                    .HasForeignKey(d => d.DestinationCountryId)
                    .HasConstraintName("FK__CU_Purcha__Desti__1DFCE517");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuPurchaseOrderDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_PurchaseOrder_Details_EntityId");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.CuPurchaseOrderDetails)
                    .HasForeignKey(d => d.PoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Purcha__PO_Id__1D08C0DE");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CuPurchaseOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_PurchaseOrder_Details_CU_Products");
                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.CuPurchaseOrderDetails)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK__CU_Purcha__Unit___1EF10950");
            });

            modelBuilder.Entity<CuRefAccountingLeader>(entity =>
            {
                entity.ToTable("CU_REF_AccountingLeader");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CuRefActivitiesLevel>(entity =>
            {
                entity.ToTable("CU_REF_ActivitiesLevel");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CuRefBrandPriority>(entity =>
            {
                entity.ToTable("Cu_REF_BrandPriority");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CuRefRelationshipStatus>(entity =>
            {
                entity.ToTable("CU_REF_RelationshipStatus");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CuReportCustomerDecisionComment>(entity =>
            {
                entity.ToTable("CU_Report_CustomerDecisionComment");

                entity.Property(e => e.ReportResult).HasMaxLength(1000);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuReportCustomerDecisionComments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_Customer_CU_Report_CustomerDecisionComment_CustomerId");
            });

            modelBuilder.Entity<CuSalesIncharge>(entity =>
            {
                entity.ToTable("Cu_SalesIncharge");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuSalesIncharges)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Cu_SalesIncharge_CU_Customer");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.CuSalesIncharges)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_Cu_SalesIncharge_HR_Staff");
            });

            modelBuilder.Entity<CuSeason>(entity =>
            {
                entity.ToTable("CU_Season");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.SeasonId).HasColumnName("Season_Id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuSeasons)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Season__Custo__23B5BE6D");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuSeasons)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_Season_EntityId");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.CuSeasons)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Season__Seaso__24A9E2A6");
            });

            modelBuilder.Entity<CuSeasonConfig>(entity =>
            {
                entity.ToTable("CU_Season_Config");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuSeasonConfigs)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CUSTOMER_CU_SEASON_CONFIG");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuSeasonConfigs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_CU_SEASON_CONFIG_ENTITY_ID");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.CuSeasonConfigs)
                    .HasForeignKey(d => d.SeasonId)
                    .HasConstraintName("FK_SEASON_CU_SEASON_CONFIG");
            });

            modelBuilder.Entity<CuServiceType>(entity =>
            {
                entity.ToTable("CU_ServiceType");

                entity.Property(e => e.AllowAqlmodification).HasColumnName("AllowAQLModification");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomServiceTypeName).HasMaxLength(1500);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DpPoint).HasColumnName("DP_Point");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuServiceTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__CU_Servic__Creat__21B86762");

                entity.HasOne(d => d.CriticalPick1Navigation)
                    .WithMany(p => p.CuServiceTypeCriticalPick1Navigations)
                    .HasForeignKey(d => d.CriticalPick1)
                    .HasConstraintName("FK__CU_Servic__Criti__2B56E035");

                entity.HasOne(d => d.CriticalPick2Navigation)
                    .WithMany(p => p.CuServiceTypeCriticalPick2Navigations)
                    .HasForeignKey(d => d.CriticalPick2)
                    .HasConstraintName("FK__CU_Servic__Criti__2C4B046E");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Servic__Custo__259E06DF");

                entity.HasOne(d => d.DefectClassificationNavigation)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.DefectClassification)
                    .HasConstraintName("FK__CU_Servic__Defec__310FB98B");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuServiceTypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__CU_Servic__Delet__22AC8B9B");

                entity.HasOne(d => d.DpPointNavigation)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.DpPoint)
                    .HasConstraintName("FK_CU_ServiceType_INSP_REF_DP_Point");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("CU_ServiceType_EntityId");

                entity.HasOne(d => d.LevelPick1Navigation)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.LevelPick1)
                    .HasConstraintName("FK__CU_Servic__Level__296E97C3");

                entity.HasOne(d => d.LevelPick2Navigation)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.LevelPick2)
                    .HasConstraintName("FK__CU_Servic__Level__2A62BBFC");

                entity.HasOne(d => d.MajorTolerancePick1Navigation)
                    .WithMany(p => p.CuServiceTypeMajorTolerancePick1Navigations)
                    .HasForeignKey(d => d.MajorTolerancePick1)
                    .HasConstraintName("FK__CU_Servic__Major__2D3F28A7");

                entity.HasOne(d => d.MajorTolerancePick2Navigation)
                    .WithMany(p => p.CuServiceTypeMajorTolerancePick2Navigations)
                    .HasForeignKey(d => d.MajorTolerancePick2)
                    .HasConstraintName("FK__CU_Servic__Major__2E334CE0");

                entity.HasOne(d => d.MinorTolerancePick1Navigation)
                    .WithMany(p => p.CuServiceTypeMinorTolerancePick1Navigations)
                    .HasForeignKey(d => d.MinorTolerancePick1)
                    .HasConstraintName("FK__CU_Servic__Minor__2F277119");

                entity.HasOne(d => d.MinorTolerancePick2Navigation)
                    .WithMany(p => p.CuServiceTypeMinorTolerancePick2Navigations)
                    .HasForeignKey(d => d.MinorTolerancePick2)
                    .HasConstraintName("FK__CU_Servic__Minor__301B9552");

                entity.HasOne(d => d.PickTypeNavigation)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.PickType)
                    .HasConstraintName("FK__CU_Servic__PickT__287A738A");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__CU_Servic__Produ__32F801FD");

                entity.HasOne(d => d.ReportUnitNavigation)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.ReportUnit)
                    .HasConstraintName("FK__CU_Servic__Repor__3203DDC4");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Servic__Servi__26922B18");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.CuServiceTypes)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CU_Servic__Servi__27864F51");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.CuServiceTypeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__CU_Servic__Updat__23A0AFD4");
            });

            modelBuilder.Entity<CuSisterCompany>(entity =>
            {
                entity.ToTable("CU_SisterCompany");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CuSisterCompanyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CU_SisterCompany_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CuSisterCompanyCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_SisterCompany_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CuSisterCompanyDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_CU_SisterCompany_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.CuSisterCompanies)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CU_SisterCompany_EntityId");

                entity.HasOne(d => d.SisterCompany)
                    .WithMany(p => p.CuSisterCompanySisterCompanies)
                    .HasForeignKey(d => d.SisterCompanyId)
                    .HasConstraintName("FK_CU_SisterCompany_SisterCompanyId");
            });

            modelBuilder.Entity<DaUserByBrand>(entity =>
            {
                entity.ToTable("DA_UserByBrand");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DauserCustomerId).HasColumnName("DAUserCustomerId");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.DaUserByBrands)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_DAUserByBrand_BrandId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserByBrands)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserByBrand_CreatedBy");

                entity.HasOne(d => d.DauserCustomer)
                    .WithMany(p => p.DaUserByBrands)
                    .HasForeignKey(d => d.DauserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserByBrand_DAUserCustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DaUserByBrands)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DAUserByBrand_EntityId");
            });

            modelBuilder.Entity<DaUserByBuyer>(entity =>
            {
                entity.ToTable("DA_UserByBuyer");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DauserCustomerId).HasColumnName("DAUserCustomerId");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.DaUserByBuyers)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("FK_DAUserByBuyer_BuyerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserByBuyers)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserByBuyer_CreatedBy");

                entity.HasOne(d => d.DauserCustomer)
                    .WithMany(p => p.DaUserByBuyers)
                    .HasForeignKey(d => d.DauserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserByBuyer_DAUserCustomerId");
            });

            modelBuilder.Entity<DaUserByDepartment>(entity =>
            {
                entity.ToTable("DA_UserByDepartment");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DauserCustomerId).HasColumnName("DAUserCustomerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserByDepartments)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserByDepartment_CreatedBy");

                entity.HasOne(d => d.DauserCustomer)
                    .WithMany(p => p.DaUserByDepartments)
                    .HasForeignKey(d => d.DauserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserByDepartment_DAUserCustomerId");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DaUserByDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_DAUserByDepartment_DepartmentId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DaUserByDepartments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DAUserByDepartment_EntityId");
            });

            modelBuilder.Entity<DaUserByFactoryCountry>(entity =>
            {
                entity.ToTable("DA_UserByFactoryCountry");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserByFactoryCountries)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DaUserCustomer)
                    .WithMany(p => p.DaUserByFactoryCountries)
                    .HasForeignKey(d => d.DaUserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Entity)
                  .WithMany(p => p.DaUserByFactoryCountries)
                  .HasForeignKey(d => d.EntityId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.DaUserByFactoryCountries)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DaUserByProductCategory>(entity =>
            {
                entity.ToTable("DA_UserByProductCategory");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DauserCustomerId).HasColumnName("DAUserCustomerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserByProductCategories)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserByProductCategory_CreatedBy");

                entity.HasOne(d => d.DauserCustomer)
                    .WithMany(p => p.DaUserByProductCategories)
                    .HasForeignKey(d => d.DauserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserByProductCategory_DAUserCustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DaUserByProductCategories)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DAUserByProductCategory_EntityId");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.DaUserByProductCategories)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_DAUserByProductCategory_ProductCategoryId");
            });

            modelBuilder.Entity<DaUserByRole>(entity =>
            {
                entity.ToTable("DA_UserByRole");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DauserCustomerId).HasColumnName("DAUserCustomerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserByRoles)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserByRole_CreatedBy");

                entity.HasOne(d => d.DauserCustomer)
                    .WithMany(p => p.DaUserByRoles)
                    .HasForeignKey(d => d.DauserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserByRole_DAUserCustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DaUserByRoles)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DAUserByRole_EntityId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.DaUserByRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserByRole_RoleId");
            });

            modelBuilder.Entity<DaUserByService>(entity =>
            {
                entity.ToTable("DA_UserByService");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DauserCustomerId).HasColumnName("DAUserCustomerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserByServices)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserByService_CreatedBy");

                entity.HasOne(d => d.DauserCustomer)
                    .WithMany(p => p.DaUserByServices)
                    .HasForeignKey(d => d.DauserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserByService_DAUserCustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DaUserByServices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DAUserByService_EntityId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.DaUserByServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_DAUserByService_ServiceId");
            });

            modelBuilder.Entity<DaUserCustomer>(entity =>
            {
                entity.ToTable("DA_UserCustomer");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PrimaryCs).HasColumnName("Primary_CS");

                entity.Property(e => e.PrimaryReportChecker).HasColumnName("Primary_ReportChecker");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserCustomerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserCustomer_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DaUserCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_DAUserCustomer_CustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DaUserCustomers)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DAUserCustomer_EntityId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DaUserCustomerUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserCustomer_UserId");

                entity.HasOne(d => d.UserTypeNavigation)
                    .WithMany(p => p.DaUserCustomers)
                    .HasForeignKey(d => d.UserType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserCustomer_UserType");
            });

            modelBuilder.Entity<DaUserRoleNotificationByOffice>(entity =>
            {
                entity.ToTable("DA_UserRoleNotificationByOffice");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DauserCustomerId).HasColumnName("DAUserCustomerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DaUserRoleNotificationByOffices)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DAUserRoleNotificationByOffice_CreatedBy");

                entity.HasOne(d => d.DauserCustomer)
                    .WithMany(p => p.DaUserRoleNotificationByOffices)
                    .HasForeignKey(d => d.DauserCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAUserRoleNotificationByOffice_DAUserCustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DaUserRoleNotificationByOffices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DAUserRoleNotificationByOffice_EntityId");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DaUserRoleNotificationByOffices)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_DAUserRoleNotificationByOffice_OfficeId");
            });

            modelBuilder.Entity<DfAttribute>(entity =>
            {
                entity.ToTable("DF_Attributes");

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DfControlAttribute>(entity =>
            {
                entity.ToTable("DF_Control_Attributes");

                entity.Property(e => e.ControlConfigurationId).HasColumnName("ControlConfigurationID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ControlAttribute)
                    .WithMany(p => p.DfControlAttributes)
                    .HasForeignKey(d => d.ControlAttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_Contro__Contr__27913C49");

                entity.HasOne(d => d.ControlConfiguration)
                    .WithMany(p => p.DfControlAttributes)
                    .HasForeignKey(d => d.ControlConfigurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_Contro__Contr__269D1810");
            });

            modelBuilder.Entity<DfControlType>(entity =>
            {
                entity.ToTable("DF_ControlTypes");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DfControlTypeAttribute>(entity =>
            {
                entity.ToTable("DF_ControlType_Attributes");

                entity.Property(e => e.DefaultValue).HasMaxLength(50);

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.DfControlTypeAttributes)
                    .HasForeignKey(d => d.AttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_Contro__Attri__1666B047");

                entity.HasOne(d => d.ControlType)
                    .WithMany(p => p.DfControlTypeAttributes)
                    .HasForeignKey(d => d.ControlTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_Contro__Contr__175AD480");
            });

            modelBuilder.Entity<DfCuConfiguration>(entity =>
            {
                entity.ToTable("DF_CU_Configuration");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Fbreference)
                    .HasColumnName("FBReference")
                    .HasMaxLength(200);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Type).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ControlType)
                    .WithMany(p => p.DfCuConfigurations)
                    .HasForeignKey(d => d.ControlTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_CU_Con__Contr__1FF01A81");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DfCuConfigurationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_CU_Con__Creat__21D862F3");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DfCuConfigurations)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_CU_Con__Custo__1E07D20F");

                entity.HasOne(d => d.DataSourceTypeNavigation)
                    .WithMany(p => p.DfCuConfigurations)
                    .HasForeignKey(d => d.DataSourceType)
                    .HasConstraintName("FK__DF_CU_Con__DataS__20E43EBA");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.DfCuConfigurationDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__DF_CU_Con__Delet__23C0AB65");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DfCuConfigurations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("DF_CU_CONFIG_EntityId");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.DfCuConfigurations)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_CU_Con__Modul__1EFBF648");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.DfCuConfigurationUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__DF_CU_Con__Updat__22CC872C");
            });

            modelBuilder.Entity<DfCuDdlSourceType>(entity =>
            {
                entity.ToTable("DF_CU_DDL_SourceType");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DfCuDdlSourceTypes)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_CU_DDL__Custo__2A6DA8F4");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.DfCuDdlSourceTypes)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_CU_DDL__TypeI__2B61CD2D");
            });

            modelBuilder.Entity<DfDdlSource>(entity =>
            {
                entity.ToTable("DF_DDL_Source");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.DfDdlSources)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DF_DDL_Sou__Type__2E3E39D8");
            });

            modelBuilder.Entity<DfDdlSourceType>(entity =>
            {
                entity.ToTable("DF_DDL_SourceType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DmBrand>(entity =>
            {
                entity.ToTable("DM_Brand");

                entity.Property(e => e.DmfileId).HasColumnName("DMFileId");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.DmBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dm_Brand_BrandId");

                entity.HasOne(d => d.Dmfile)
                    .WithMany(p => p.DmBrands)
                    .HasForeignKey(d => d.DmfileId)
                    .HasConstraintName("FK_DM_Brand_DMFileId");
            });

            modelBuilder.Entity<DmDepartment>(entity =>
            {
                entity.ToTable("DM_Department");

                entity.Property(e => e.DmfileId).HasColumnName("DMFileId");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DmDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dm_Department_DepartmentId");

                entity.HasOne(d => d.Dmfile)
                    .WithMany(p => p.DmDepartments)
                    .HasForeignKey(d => d.DmfileId)
                    .HasConstraintName("FK_DM_Department_DMFileId");
            });

            modelBuilder.Entity<DmDetail>(entity =>
            {
                entity.ToTable("DM_details");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(400);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DmDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DM_DETAILS_CREATED_BY");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DmDetails)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_DM_DETAILS_CUSTOMER_ID");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.DmDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_DM_DETAIL_DELETED_BY");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DmDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DM_details_EntityId");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.DmDetails)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_DM_DETAILS_MODULE_ID");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.DmDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_DM_DETAILS_UPDATED_BY");
            });

            modelBuilder.Entity<DmFile>(entity =>
            {
                entity.ToTable("DM_File");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DmdetailsId).HasColumnName("DMDetailsId");

                entity.Property(e => e.FileId).HasMaxLength(200);

                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.FileType).HasMaxLength(200);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DmFileCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DM_FILE_CREATED_BY");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.DmFileDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_DM_FILE_DELETED_BY");

                entity.HasOne(d => d.Dmdetails)
                    .WithMany(p => p.DmFiles)
                    .HasForeignKey(d => d.DmdetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DM_DETAILS_ID");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DmFiles)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DM_ENTITY_ID");
            });

            modelBuilder.Entity<DmRefModule>(entity =>
            {
                entity.ToTable("DM_REF_MODULE");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DmLevel).HasColumnName("DM_Level");

                entity.Property(e => e.ModuleName).HasMaxLength(500);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DmRefModules)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DM_REF_MODULE_EntityId");
            });

            modelBuilder.Entity<DmRight>(entity =>
            {
                entity.ToTable("DM_RIGHT");

                entity.Property(e => e.DmRoleId).HasColumnName("DM_RoleId");

                entity.HasOne(d => d.DmRole)
                    .WithMany(p => p.DmRights)
                    .HasForeignKey(d => d.DmRoleId)
                    .HasConstraintName("FK_DM_RIGHT_DM_RoleId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DmRights)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DM_RIGHT_EntityId");

                entity.HasOne(d => d.IdModuleNavigation)
                 .WithMany(p => p.DmRights)
                 .HasForeignKey(d => d.IdModule)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK__DM_RIGHT__IdModu__37C86F69");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.DmRights)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK__DM_RIGHT__IdRole__38BC93A2");

                entity.HasOne(d => d.IdStaffNavigation)
                    .WithMany(p => p.DmRights)
                    .HasForeignKey(d => d.IdStaff)
                    .HasConstraintName("FK__DM_RIGHT__IdStaf__39B0B7DB");
            });

            modelBuilder.Entity<DmRole>(entity =>
            {
                entity.ToTable("DM_Role");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DmRoleCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DM_Role_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.DmRoleDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_DM_Role_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.DmRoles)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_DM_Role_EntityId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.DmRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_DM_Role_RoleId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.DmRoles)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_DM_Role_StaffId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.DmRoleUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_DM_Role_UpdatedBy");
            });

            modelBuilder.Entity<EcAutQcFoodExpense>(entity =>
            {
                entity.ToTable("EC_AUT_QC_FoodExpense");

                entity.Property(e => e.Comments).HasMaxLength(2500);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EcAutQcFoodExpenseCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_EC_AUT_QC_FoodExpense_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcAutQcFoodExpenseDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_EC_AUT_QC_FoodExpense_Deleted_By");

                entity.HasOne(d => d.FactoryCountryNavigation)
                    .WithMany(p => p.EcAutQcFoodExpenses)
                    .HasForeignKey(d => d.FactoryCountry)
                    .HasConstraintName("FK_EC_AUT_QC_FoodExpense_FactoryCountry");

                entity.HasOne(d => d.FoodAllowanceCurrencyNavigation)
                    .WithMany(p => p.EcAutQcFoodExpenses)
                    .HasForeignKey(d => d.FoodAllowanceCurrency)
                    .HasConstraintName("FK_EC_AUT_QC_FoodExpense_FoodAllowanceCurrency");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.EcAutQcFoodExpenses)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_EC_AUT_QC_FoodExpense_InspectionId");

                entity.HasOne(d => d.Qc)
                    .WithMany(p => p.EcAutQcFoodExpenses)
                    .HasForeignKey(d => d.QcId)
                    .HasConstraintName("FK_EC_AUT_QC_FoodExpense_QcId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EcAutQcFoodExpenseUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_EC_AUT_QC_FoodExpense_UpdatedBy");
            });

            modelBuilder.Entity<EcAutQcTravelExpense>(entity =>
            {
                entity.ToTable("EC_AUT_QC_TravelExpense");

                entity.Property(e => e.Comments).HasMaxLength(2500);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EcAutQcTravelExpenseCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcAutQcTravelExpenseDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_Deleted_By");

                entity.HasOne(d => d.FactoryTownNavigation)
                    .WithMany(p => p.EcAutQcTravelExpenses)
                    .HasForeignKey(d => d.FactoryTown)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_FactoryTown");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.EcAutQcTravelExpenses)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_InspectionId");

                entity.HasOne(d => d.Qc)
                    .WithMany(p => p.EcAutQcTravelExpenses)
                    .HasForeignKey(d => d.QcId)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_QcId");

                entity.HasOne(d => d.StartPortNavigation)
                    .WithMany(p => p.EcAutQcTravelExpenses)
                    .HasForeignKey(d => d.StartPort)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_StartPort");

                entity.HasOne(d => d.TravelTariffCurrencyNavigation)
                    .WithMany(p => p.EcAutQcTravelExpenses)
                    .HasForeignKey(d => d.TravelTariffCurrency)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_TravelTariffCurrency");

                entity.HasOne(d => d.TripTypeNavigation)
                    .WithMany(p => p.EcAutQcTravelExpenses)
                    .HasForeignKey(d => d.TripType)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_TripType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EcAutQcTravelExpenseUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_EC_AUT_QC_TravelExpense_UpdatedBy");
            });

            modelBuilder.Entity<EcAutRefStartPort>(entity =>
            {
                entity.ToTable("EC_AUT_REF_StartPort");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.StartPortName).HasMaxLength(1000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.EcAutRefStartPorts)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_EC_AUT_REF_StartPort_CityId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EcAutRefStartPortCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_EC_AUT_REF_StartPort_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcAutRefStartPortDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_EC_AUT_REF_StartPort_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcAutRefStartPorts)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_EC_AUT_REF_StartPort_Entity_Id");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EcAutRefStartPortUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_EC_AUT_REF_StartPort_UpdatedBy");
            });

            modelBuilder.Entity<EcAutRefTripType>(entity =>
            {
                entity.ToTable("EC_AUT_REF_TripType");

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<EcAutTravelTariff>(entity =>
            {
                entity.ToTable("EC_AUT_TravelTariff");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EcAutTravelTariffCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_EC_AUT_TravelTariff_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcAutTravelTariffDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_EC_AUT_TravelTariff_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcAutTravelTariffs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_EC_AUT_TravelTariff_Entity_Id");

                entity.HasOne(d => d.StartPortNavigation)
                    .WithMany(p => p.EcAutTravelTariffs)
                    .HasForeignKey(d => d.StartPort)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EC_AUT_TravelTariff_StartPort");

                entity.HasOne(d => d.Town)
                    .WithMany(p => p.EcAutTravelTariffs)
                    .HasForeignKey(d => d.TownId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EC_AUT_TravelTariff_TownId");

                entity.HasOne(d => d.TravelCurrencyNavigation)
                    .WithMany(p => p.EcAutTravelTariffs)
                    .HasForeignKey(d => d.TravelCurrency)
                    .HasConstraintName("FK_EC_AUT_TravelTariff_TravelCurrency");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EcAutTravelTariffUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_EC_AUT_TravelTariff_UpdatedBy");
            });

            modelBuilder.Entity<EcExpClaimStatus>(entity =>
            {
                entity.ToTable("EC_ExpClaimStatus");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcExpClaimStatuses)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__EC_ExpCla__Entit__33EC2636");
            });

            modelBuilder.Entity<EcExpencesClaim>(entity =>
            {
                entity.ToTable("EC_ExpencesClaims");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CancelDate).HasColumnType("datetime");

                entity.Property(e => e.CheckedDate).HasColumnType("datetime");

                entity.Property(e => e.ClaimDate).HasColumnType("datetime");

                entity.Property(e => e.ClaimNo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Comment).HasMaxLength(600);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExpensePurpose).HasMaxLength(4000);

                entity.Property(e => e.IsInsp).HasColumnName("IsINsp");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.RejectDate).HasColumnType("datetime");

                entity.HasOne(d => d.Approved)
                    .WithMany(p => p.EcExpencesClaimApproveds)
                    .HasForeignKey(d => d.ApprovedId)
                    .HasConstraintName("FK__EC_Expenc__Appro__3A9923C5");

                entity.HasOne(d => d.Cancel)
                    .WithMany(p => p.EcExpencesClaimCancels)
                    .HasForeignKey(d => d.CancelId)
                    .HasConstraintName("FK_CancelId_IT_UserMaster_Id");

                entity.HasOne(d => d.Checked)
                    .WithMany(p => p.EcExpencesClaimCheckeds)
                    .HasForeignKey(d => d.CheckedId)
                    .HasConstraintName("FK__EC_Expenc__Check__3B8D47FE");

                entity.HasOne(d => d.ClaimType)
                    .WithMany(p => p.EcExpencesClaims)
                    .HasForeignKey(d => d.ClaimTypeId)
                    .HasConstraintName("FK_ExpenseClaimType");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.EcExpencesClaims)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expenc__Count__36C892E1");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcExpencesClaims)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expenc__Entit__34E04A6F");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.EcExpencesClaims)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expenc__Locat__38B0DB53");

                entity.HasOne(d => d.Paid)
                    .WithMany(p => p.EcExpencesClaimPaids)
                    .HasForeignKey(d => d.PaidId)
                    .HasConstraintName("FK__EC_Expenc__PaidI__3C816C37");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.EcExpencesClaims)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expenc__Payme__37BCB71A");

                entity.HasOne(d => d.Reject)
                    .WithMany(p => p.EcExpencesClaimRejects)
                    .HasForeignKey(d => d.RejectId)
                    .HasConstraintName("FK_Rejected_IT_UserMaster_Id");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.EcExpencesClaims)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expenc__Staff__35D46EA8");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.EcExpencesClaims)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expenc__Statu__39A4FF8C");
            });

            modelBuilder.Entity<EcExpenseClaimsAudit>(entity =>
            {
                entity.ToTable("EC_ExpenseClaimsAudit");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.EcExpenseClaimsAudits)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expens__Booki__68D4EFC1");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EcExpenseClaimsAuditCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__EC_Expens__Creat__6ABD3833");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcExpenseClaimsAuditDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__EC_Expens__Delet__6BB15C6C");

                entity.HasOne(d => d.ExpenseClaimDetail)
                    .WithMany(p => p.EcExpenseClaimsAudits)
                    .HasForeignKey(d => d.ExpenseClaimDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expens__Expen__69C913FA");
            });

            modelBuilder.Entity<EcExpenseClaimsInspection>(entity =>
            {
                entity.ToTable("EC_ExpenseClaimsInspection");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.EcExpenseClaimsInspections)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expens__Booki__6E8DC917");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EcExpenseClaimsInspectionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__EC_Expens__Creat__70761189");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcExpenseClaimsInspectionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__EC_Expens__Delet__716A35C2");

                entity.HasOne(d => d.ExpenseClaimDetail)
                    .WithMany(p => p.EcExpenseClaimsInspections)
                    .HasForeignKey(d => d.ExpenseClaimDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expens__Expen__6F81ED50");
            });

            modelBuilder.Entity<EcExpenseClaimtype>(entity =>
            {
                entity.ToTable("EC_ExpenseClaimtype");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<EcExpensesClaimDetai>(entity =>
            {
                entity.ToTable("EC_ExpensesClaimDetais");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AmmountHk).HasColumnName("Ammount_HK");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.ExchangeRate).HasColumnName("Exchange_Rate");

                entity.Property(e => e.ExpenseDate).HasColumnType("datetime");

                entity.Property(e => e.IsManagerApproved).HasColumnName("IsManagerApproved ");

                entity.Property(e => e.QcAutoExpenseId).HasColumnName("Qc_Auto_ExpenseId");

                entity.Property(e => e.QcFoodExpenseId).HasColumnName("Qc_Food_ExpenseId");

                entity.Property(e => e.QcTravelExpenseId).HasColumnName("Qc_Travel_ExpenseId");

                entity.HasOne(d => d.ArrivalCity)
                    .WithMany(p => p.EcExpensesClaimDetaiArrivalCities)
                    .HasForeignKey(d => d.ArrivalCityId)
                    .HasConstraintName("FK__EC_Expens__Arriv__4051FD1B");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("FK_EXPENSE_AUDIT_ID");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.EcExpensesClaimDetaiCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expens__Curre__423A458D");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("EC_ExpensesClaimDetais_EntityId");

                entity.HasOne(d => d.Expense)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.ExpenseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expens__Expen__3D759070");

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Expens__Expen__3E69B4A9");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_EXPENSE_INSPECTION_ID");

                entity.HasOne(d => d.PayrollCurrency)
                    .WithMany(p => p.EcExpensesClaimDetaiPayrollCurrencies)
                    .HasForeignKey(d => d.PayrollCurrencyId)
                    .HasConstraintName("FK__EC_Expens__Payro__432E69C6");

                entity.HasOne(d => d.QcFoodExpense)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.QcFoodExpenseId)
                    .HasConstraintName("EC_ExpensesClaimDetais_Qc_Food_ExpenseId");

                entity.HasOne(d => d.QcTravelExpense)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.QcTravelExpenseId)
                    .HasConstraintName("EC_ExpensesClaimDetais_Qc_Travel_ExpenseId");

                entity.HasOne(d => d.StartCity)
                    .WithMany(p => p.EcExpensesClaimDetaiStartCities)
                    .HasForeignKey(d => d.StartCityId)
                    .HasConstraintName("FK__EC_Expens__Start__3F5DD8E2");

                entity.HasOne(d => d.TripTypeNavigation)
                    .WithMany(p => p.EcExpensesClaimDetais)
                    .HasForeignKey(d => d.TripType)
                    .HasConstraintName("EC_ExpensesClaimDetais_TripType");
            });

            modelBuilder.Entity<EcExpensesType>(entity =>
            {
                entity.ToTable("EC_ExpensesTypes");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsTravel)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcExpensesTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__EC_Expens__Entit__44228DFF");

                entity.Property(e => e.XeroAccountCode)
                .HasColumnName("Xero_AccountCode")
                .HasMaxLength(2000);

                entity.Property(e => e.XeroOutSourceAccountCode)
                    .HasColumnName("Xero_OutSource_AccountCode")
                    .HasMaxLength(2000);
            });

            modelBuilder.Entity<EcFoodAllowance>(entity =>
            {
                entity.ToTable("EC_FoodAllowance");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FoodAllowance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.EcFoodAllowances)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_FoodAl__Count__4516B238");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.EcFoodAllowances)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_FoodAl__Curre__460AD671");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcFoodAllowanceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_EC_FoodAllowance_DeletedById");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcFoodAllowances)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__EC_FoodAl__Entit__47F31EE3");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EcFoodAllowanceUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_EC_FoodAllowance_UpdatedById");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EcFoodAllowanceUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_FoodAl__UserI__46FEFAAA");
            });

            modelBuilder.Entity<EcPaymenType>(entity =>
            {
                entity.ToTable("EC_PaymenTypes");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EcPaymenTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__EC_Paymen__Entit__48E7431C");
            });

            modelBuilder.Entity<EcReceiptFile>(entity =>
            {
                entity.ToTable("EC_ReceiptFile");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__EC_Recei__4B840D04C76D22B0")
                    .IsUnique();

                entity.Property(e => e.FullFileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Expense)
                    .WithMany(p => p.EcReceiptFiles)
                    .HasForeignKey(d => d.ExpenseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Receip__Expen__49DB6755");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EcReceiptFiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Receip__UserI__4ACF8B8E");
            });

            modelBuilder.Entity<EcReceiptFileAttachment>(entity =>
            {
                entity.ToTable("EC_ReceiptFileAttachment");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.HasOne(d => d.CreatedbyNavigation)
                    .WithMany(p => p.EcReceiptFileAttachmentCreatedbyNavigations)
                    .HasForeignKey(d => d.Createdby)
                    .HasConstraintName("FK__EC_Receip__Creat__1F46DA62");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EcReceiptFileAttachmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__EC_Receip__Delet__203AFE9B");

                entity.HasOne(d => d.Expense)
                    .WithMany(p => p.EcReceiptFileAttachments)
                    .HasForeignKey(d => d.ExpenseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Receip__Expen__212F22D4");
            });

            modelBuilder.Entity<EcStatusRole>(entity =>
            {
                entity.HasKey(e => new { e.IdRole, e.IdStatus })
                    .HasName("PK__EC_Statu__0F7396171E094BD0");

                entity.ToTable("EC_Status_Role");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.EcStatusRoles)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Status__IdRol__4BC3AFC7");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.EcStatusRoles)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EC_Status__IdSta__4CB7D400");
            });

            modelBuilder.Entity<EmExchangeRate>(entity =>
            {
                entity.ToTable("EM_ExchangeRate");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.BeginDate).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.CurrencyId1Navigation)
                    .WithMany(p => p.EmExchangeRateCurrencyId1Navigations)
                    .HasForeignKey(d => d.CurrencyId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EM_Exchan__Curre__4DABF839");

                entity.HasOne(d => d.Currencyid2Navigation)
                    .WithMany(p => p.EmExchangeRateCurrencyid2Navigations)
                    .HasForeignKey(d => d.Currencyid2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EM_Exchan__Curre__4EA01C72");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EmExchangeRates)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EM_Exchan__Entit__508864E4");

                entity.HasOne(d => d.ExRateType)
                    .WithMany(p => p.EmExchangeRates)
                    .HasForeignKey(d => d.ExRateTypeId)
                    .HasConstraintName("FK__EM_Exchan__ExRat__517C891D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmExchangeRates)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EM_Exchan__User___4F9440AB");
            });

            modelBuilder.Entity<EmExchangeRateType>(entity =>
            {
                entity.ToTable("EM_ExchangeRateType");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EmExchangeRateTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__EM_Exchan__Entit__5270AD56");
            });

            modelBuilder.Entity<EntFeatureDetail>(entity =>
            {
                entity.ToTable("ENT_Feature_Details");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.EntFeatureDetails)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_ENT_Feature_Details_CountryId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EntFeatureDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ENT_Feature_Details_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EntFeatureDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_ENT_Feature_Details_Deleted_By");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EntFeatureDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ENT_Feature_Details_EntityId");

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.EntFeatureDetails)
                    .HasForeignKey(d => d.FeatureId)
                    .HasConstraintName("FK_ENT_Feature_Details_FeatureId");
            });

            modelBuilder.Entity<EntField>(entity =>
            {
                entity.ToTable("ENT_Fields");

                entity.Property(e => e.EntpageId).HasColumnName("ENTPageId");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Entpage)
                    .WithMany(p => p.EntFields)
                    .HasForeignKey(d => d.EntpageId)
                    .HasConstraintName("FK_ENT_Page_Id");
            });

            modelBuilder.Entity<EntMasterConfig>(entity =>
            {
                entity.ToTable("ENT_Master_Config");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.EntMasterConfigs)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_ENT_Master_Config_Ent_Master_Type");
            });

            modelBuilder.Entity<EntMasterType>(entity =>
            {
                entity.ToTable("ENT_Master_Type");

                entity.Property(e => e.Name).HasMaxLength(300);
            });

            modelBuilder.Entity<EntPage>(entity =>
            {
                entity.ToTable("ENT_Pages");

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.EntPages)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ENT_PAGE_SERVICE");
            });

            modelBuilder.Entity<EntPagesField>(entity =>
            {
                entity.ToTable("ENT_Pages_Fields");

                entity.Property(e => e.EntfieldId).HasColumnName("ENTFieldId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.EntPagesFields)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ENT_PAGE_FIELD_CUSTOMER_ID");

                entity.HasOne(d => d.Entfield)
                    .WithMany(p => p.EntPagesFields)
                    .HasForeignKey(d => d.EntfieldId)
                    .HasConstraintName("FK_ENT_FIELD_ID");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EntPagesFields)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ENT_PAGE_FIELD_ENTITY_ID");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.EntPagesFields)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("FK_USER_TYPE_ID");
            });

            modelBuilder.Entity<EntRefFeature>(entity =>
            {
                entity.ToTable("ENT_REF_Features");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EntRefFeatureCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ENT_REF_Features_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EntRefFeatureDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_ENT_REF_Features_Deleted_By");
            });

            modelBuilder.Entity<EsAdditionalRecipient>(entity =>
            {
                entity.ToTable("ES_AdditionalRecipients");

                entity.Property(e => e.AdditionalEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsAdditionalRecipients)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_AdditionalRecipients_CreatedBy");

                entity.HasOne(d => d.EmailDetail)
                    .WithMany(p => p.EsAdditionalRecipients)
                    .HasForeignKey(d => d.EmailDetailId)
                    .HasConstraintName("FK_ES_AdditionalRecipients_EmailDetailId");

                entity.HasOne(d => d.RecipientNavigation)
                    .WithMany(p => p.EsAdditionalRecipients)
                    .HasForeignKey(d => d.Recipient)
                    .HasConstraintName("FK_ES_AdditionalRecipients_Recipient");
            });

            modelBuilder.Entity<EsApiContact>(entity =>
            {
                entity.ToTable("ES_API_Contacts");

                entity.Property(e => e.ApiContactId).HasColumnName("Api_Contact_Id");

                entity.HasOne(d => d.ApiContact)
                    .WithMany(p => p.EsApiContacts)
                    .HasForeignKey(d => d.ApiContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_API_Contacts_Customer_Api_Contact_Id");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsApiContacts)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_API_Contacts_EsDetailsId");
            });

            modelBuilder.Entity<EsApiDefaultContact>(entity =>
            {
                entity.ToTable("ES_API_Default_Contacts");

                entity.Property(e => e.ApiContactId).HasColumnName("Api_Contact_Id");

                entity.HasOne(d => d.ApiContact)
                    .WithMany(p => p.EsApiDefaultContacts)
                    .HasForeignKey(d => d.ApiContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_API_Default_Contacts_Api_Contact_Id");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EsApiDefaultContacts)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ES_API_Default_Contacts_EntityId");
            });

            modelBuilder.Entity<EsCuConfig>(entity =>
            {
                entity.ToTable("ES_CU_Config");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.EsCuConfigs)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("ES_CU_Config_BrandId");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.EsCuConfigs)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("ES_CU_Config_BuyerId");

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.EsCuConfigs)
                    .HasForeignKey(d => d.CollectionId)
                    .HasConstraintName("ES_CU_Config_CollectionId");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.EsCuConfigs)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("ES_CU_Config_DepartmentId");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsCuConfigs)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_CU_Config_EsDetailsId");
            });

            modelBuilder.Entity<EsCuContact>(entity =>
            {
                entity.ToTable("ES_CU_Contacts");

                entity.Property(e => e.CustomerContactId).HasColumnName("Customer_Contact_Id");

                entity.HasOne(d => d.CustomerContact)
                    .WithMany(p => p.EsCuContacts)
                    .HasForeignKey(d => d.CustomerContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_CU_Contacts_Customer_Contact_Id");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsCuContacts)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_CU_Contacts_EsDetailsId");
            });

            modelBuilder.Entity<EsDetail>(entity =>
            {
                entity.ToTable("ES_Details");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.EmailSize).HasColumnName("Email_Size");

                entity.Property(e => e.EmailSubject).HasColumnName("Email_Subject");

                entity.Property(e => e.FileNameId).HasColumnName("File_Name_Id");

                entity.Property(e => e.NoOfReports).HasColumnName("No_Of_Reports");

                entity.Property(e => e.RecipientName).HasColumnName("Recipient_Name");

                entity.Property(e => e.ReportInEmail).HasColumnName("Report_In_Email");

                entity.Property(e => e.ReportSendType).HasColumnName("Report_Send_Type");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("ES_Details_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Details_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EsDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_ES_Details_DeletedBy");

                entity.HasOne(d => d.EmailSizeNavigation)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.EmailSize)
                    .HasConstraintName("FK_ES_Details_Email_Size");

                entity.HasOne(d => d.EmailSubjectNavigation)
                    .WithMany(p => p.EsDetailEmailSubjectNavigations)
                    .HasForeignKey(d => d.EmailSubject)
                    .HasConstraintName("FK_ES_Details_Email_Subject");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ES_Details_EntityId");

                entity.HasOne(d => d.FileName)
                    .WithMany(p => p.EsDetailFileNames)
                    .HasForeignKey(d => d.FileNameId)
                    .HasConstraintName("FK_ES_Details_File_Name_Id");

                entity.HasOne(d => d.ReportInEmailNavigation)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.ReportInEmail)
                    .HasConstraintName("FK_ES_Details_Report_In_Email");

                entity.HasOne(d => d.ReportSendTypeNavigation)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.ReportSendType)
                    .HasConstraintName("FK_ES_Details_Report_Send_Type");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Details_ServiceId");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.EsDetails)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Details_TypeId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EsDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_ES_Details_UpdatedBy");
            });

            modelBuilder.Entity<EsEmailReportTypeMap>(entity =>
            {
                entity.ToTable("ES_Email_Report_Type_Map");

                entity.HasOne(d => d.EmailTypeNavigation)
                    .WithMany(p => p.EsEmailReportTypeMaps)
                    .HasForeignKey(d => d.EmailType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ES_Email_Report_Type_Map_EmailType");

                entity.HasOne(d => d.ReportTypeNavigation)
                    .WithMany(p => p.EsEmailReportTypeMaps)
                    .HasForeignKey(d => d.ReportType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ES_Email_Report_Type_Map_ReportType");
            });

            modelBuilder.Entity<EsFaCountryConfig>(entity =>
            {
                entity.ToTable("ES_FA_Country_Config");

                entity.Property(e => e.FactoryCountryId).HasColumnName("Factory_CountryId");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsFaCountryConfigs)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_FA_Country_Config_EsDetailsId");

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.EsFaCountryConfigs)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_FA_Country_Config_Factory_CountryId");
            });

            modelBuilder.Entity<EsOfficeConfig>(entity =>
            {
                entity.ToTable("ES_Office_Config");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsOfficeConfigs)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Office_Config_EsDetailsId");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.EsOfficeConfigs)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("ES_Office_Config_Office_Id");
            });

            modelBuilder.Entity<EsProductCategoryConfig>(entity =>
            {
                entity.ToTable("ES_Product_Category_Config");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsProductCategoryConfigs)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Product_Category_Config_EsDetailsId");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.EsProductCategoryConfigs)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Product_Category_Config_ProductCategoryId");
            });

            modelBuilder.Entity<EsRecipientType>(entity =>
            {
                entity.ToTable("ES_Recipient_Type");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("Created_On")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EsDetailsId).HasColumnName("Es_Details_Id");

                entity.Property(e => e.IsCc).HasColumnName("IsCC");

                entity.Property(e => e.RecipientTypeId).HasColumnName("Recipient_Type_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsRecipientTypes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("ES_Recipient_Type_Created_By");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsRecipientTypes)
                    .HasForeignKey(d => d.EsDetailsId)
                    .HasConstraintName("ES_Recipient_Type_Es_Details_Id");

                entity.HasOne(d => d.RecipientType)
                    .WithMany(p => p.EsRecipientTypes)
                    .HasForeignKey(d => d.RecipientTypeId)
                    .HasConstraintName("ES_Recipient_Type_Recipient_Type_Id");
            });

            modelBuilder.Entity<EsRefEmailSize>(entity =>
            {
                entity.ToTable("ES_REF_Email_Size");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsRefEmailSizes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_REF_Email_Size_CreatedBy");
            });

            modelBuilder.Entity<EsRefFileType>(entity =>
            {
                entity.ToTable("ES_REF_File_Type");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsRefFileTypes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_REF_File_Type_CreatedBy");
            });

            modelBuilder.Entity<EsRefRecipient>(entity =>
            {
                entity.ToTable("ES_REF_Recipient");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EsRefRecipientType>(entity =>
            {
                entity.ToTable("ES_REF_RecipientType");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("Created_On")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsRefRecipientTypes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("ES_REF_RecipientType_Created_By");
            });

            modelBuilder.Entity<EsRefReportInEmail>(entity =>
            {
                entity.ToTable("ES_REF_Report_In_Email");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsRefReportInEmails)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_REF_Report_In_Email_CreatedBy");
            });

            modelBuilder.Entity<EsRefReportSendType>(entity =>
            {
                entity.ToTable("ES_REF_Report_Send_Type");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsRefReportSendTypes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_REF_Report_Send_Type_CreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EsRefReportSendTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ES_REF_Report_Send_Type_EntityId");
            });

            modelBuilder.Entity<EsRefSpecialRule>(entity =>
            {
                entity.ToTable("ES_REF_Special_Rule");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsRefSpecialRules)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_REF_Special_Rule_CreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EsRefSpecialRules)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ES_REF_Special_Rule_EntityId");
            });

            modelBuilder.Entity<EsResultConfig>(entity =>
            {
                entity.ToTable("ES_Result_Config");

                entity.Property(e => e.ApiResultId).HasColumnName("API_Result_Id");

                entity.Property(e => e.CustomerResultId).HasColumnName("Customer_Result_Id");

                entity.HasOne(d => d.ApiResult)
                    .WithMany(p => p.EsResultConfigs)
                    .HasForeignKey(d => d.ApiResultId)
                    .HasConstraintName("FK_ES_Result_Config_API_Result_Id");

                entity.HasOne(d => d.CustomerResult)
                    .WithMany(p => p.EsResultConfigs)
                    .HasForeignKey(d => d.CustomerResultId)
                    .HasConstraintName("ES_Result_Config_Customer_Result_Id");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsResultConfigs)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Result_Config_EsDetailsId");
            });

            modelBuilder.Entity<EsRuleRecipientEmailTypeMap>(entity =>
            {
                entity.ToTable("ES_RULE_Recipient_EmailType_Map");

                entity.HasOne(d => d.EmailType)
                    .WithMany(p => p.EsRuleRecipientEmailTypeMaps)
                    .HasForeignKey(d => d.EmailTypeId)
                    .HasConstraintName("FK_ES_RULE_Recipient_EmailType_Map_TypeId");

                entity.HasOne(d => d.RecipientType)
                    .WithMany(p => p.EsRuleRecipientEmailTypeMaps)
                    .HasForeignKey(d => d.RecipientTypeId)
                    .HasConstraintName("FK_ES_RULE_Recipient_EmailType_Map_RecipientTypeId");
            });

            modelBuilder.Entity<EsServiceTypeConfig>(entity =>
            {
                entity.ToTable("ES_ServiceType_Config");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsServiceTypeConfigs)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_ServiceType_Config_EsDetailsId");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.EsServiceTypeConfigs)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_ServiceType_Config_ServiceTypeId");
            });

            modelBuilder.Entity<EsSpecialRule>(entity =>
            {
                entity.ToTable("ES_Special_Rule");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EsDetailsId).HasColumnName("Es_Details_Id");

                entity.Property(e => e.SpecialRuleId).HasColumnName("Special_Rule_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsSpecialRules)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_Special_Rule_CreatedBy");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsSpecialRules)
                    .HasForeignKey(d => d.EsDetailsId)
                    .HasConstraintName("FK_ES_Special_Rule_Es_Details_Id");

                entity.HasOne(d => d.SpecialRule)
                    .WithMany(p => p.EsSpecialRules)
                    .HasForeignKey(d => d.SpecialRuleId)
                    .HasConstraintName("FK_ES_Special_Rule_Special_Rule_Id");
            });

            modelBuilder.Entity<EsSuDataType>(entity =>
            {
                entity.ToTable("ES_SU_DataType");

                entity.Property(e => e.Data).HasMaxLength(20);
            });

            modelBuilder.Entity<EsSuModule>(entity =>
            {
                entity.ToTable("ES_SU_Module");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsSuModules)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_SU_Module_CreatedBy");
            });

            modelBuilder.Entity<EsSuPreDefinedField>(entity =>
            {
                entity.ToTable("ES_SU_PreDefined_Fields");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FieldAliasName).HasColumnName("Field_Alias_Name");

                entity.Property(e => e.FieldName).HasColumnName("Field_Name");

                entity.Property(e => e.MaxChar).HasColumnName("Max_Char");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsSuPreDefinedFieldCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("ES_SU_PreDefined_Fields_CreatedBy");

                entity.HasOne(d => d.DataTypeNavigation)
                    .WithMany(p => p.EsSuPreDefinedFields)
                    .HasForeignKey(d => d.DataType)
                    .HasConstraintName("FK_SU_DataType");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EsSuPreDefinedFieldDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("ES_SU_PreDefined_Fields_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EsSuPreDefinedFields)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ES_SU_PreDefined_Field_Entity_Id");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EsSuPreDefinedFieldUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("ES_SU_PreDefined_Fields_UpdatedBy");
            });

            modelBuilder.Entity<EsSuTemplateDetail>(entity =>
            {
                entity.ToTable("ES_SU_Template_Details");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FieldId).HasColumnName("Field_Id");

                entity.Property(e => e.MaxChar).HasColumnName("Max_Char");

                entity.Property(e => e.MaxItems).HasColumnName("Max_Items");

                entity.Property(e => e.TemplateId).HasColumnName("Template_Id");

                entity.Property(e => e.TitleCustomName).HasMaxLength(400);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsSuTemplateDetails)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("ES_SU_Template_Details_CreatedBy");

                entity.HasOne(d => d.DateFormatNavigation)
                    .WithMany(p => p.EsSuTemplateDetails)
                    .HasForeignKey(d => d.DateFormat)
                    .HasConstraintName("FK_Date_Format");

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.EsSuTemplateDetails)
                    .HasForeignKey(d => d.FieldId)
                    .HasConstraintName("ES_SU_Template_Details_Field_Id");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.EsSuTemplateDetails)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("ES_SU_Template_Details_Template_Id");
            });

            modelBuilder.Entity<EsSuTemplateMaster>(entity =>
            {
                entity.ToTable("ES_SU_Template_Master");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DelimiterId).HasColumnName("Delimiter_Id");

                entity.Property(e => e.EmailTypeId).HasColumnName("Email_Type_Id");

                entity.Property(e => e.ModuleId).HasColumnName("Module_Id");

                entity.Property(e => e.TemplateDisplayName).HasColumnName("Template_Display_Name");

                entity.Property(e => e.TemplateName).HasColumnName("Template_Name");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsSuTemplateMasterCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("ES_SU_Template_Master_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.EsSuTemplateMasters)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("ES_SU_Template_Master_Customer_Id");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EsSuTemplateMasterDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("ES_SU_Template_Master_DeletedBy");

                entity.HasOne(d => d.Delimiter)
                    .WithMany(p => p.EsSuTemplateMasters)
                    .HasForeignKey(d => d.DelimiterId)
                    .HasConstraintName("FK_ES_SU_Template_Master_Delimiter_Id");

                entity.HasOne(d => d.EmailType)
                    .WithMany(p => p.EsSuTemplateMasters)
                    .HasForeignKey(d => d.EmailTypeId)
                    .HasConstraintName("FK_ES_SU_Template_Master_Email_Type_Id");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EsSuTemplateMasters)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ES_SU_Template_Master_EntityId");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.EsSuTemplateMasters)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_ES_SU_Template_Master_Module_Id");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.EsSuTemplateMasterUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("ES_SU_Template_Master_UpdatedBy");
            });

            modelBuilder.Entity<EsSupFactConfig>(entity =>
            {
                entity.ToTable("ES_Sup_Fact_Config");

                entity.Property(e => e.SupplierOrFactoryId).HasColumnName("Supplier_OR_Factory_Id");

                entity.HasOne(d => d.EsDetails)
                    .WithMany(p => p.EsSupFactConfigs)
                    .HasForeignKey(d => d.EsDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Sup_Fact_Config_EsDetailsId");

                entity.HasOne(d => d.SupplierOrFactory)
                    .WithMany(p => p.EsSupFactConfigs)
                    .HasForeignKey(d => d.SupplierOrFactoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ES_Sup_Fact_Config_Supplier_OR_Factory_Id");
            });

            modelBuilder.Entity<EsTranFile>(entity =>
            {
                entity.ToTable("ES_TRAN_Files");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("Created_On")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasColumnName("Deleted_By");

                entity.Property(e => e.DeletedOn)
                    .HasColumnName("Deleted_On")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.FileLink)
                    .HasColumnName("File_Link")
                    .HasMaxLength(3000);

                entity.Property(e => e.FileName)
                    .HasColumnName("File_Name")
                    .HasMaxLength(1000);

                entity.Property(e => e.FileTypeId).HasColumnName("File_Type_Id");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.ReportId).HasColumnName("Report_Id");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("Unique_Id")
                    .HasMaxLength(1000);

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.EsTranFiles)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("FK_ES_TRAN_Files_Auidt_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EsTranFileCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ES_TRAN_Files_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.EsTranFileDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_ES_TRAN_Files_Deleted_By");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EsTranFiles)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_ES_TRAN_Files_Entity_Id");

                entity.HasOne(d => d.FileType)
                    .WithMany(p => p.EsTranFiles)
                    .HasForeignKey(d => d.FileTypeId)
                    .HasConstraintName("FK_ES_TRAN_Files_File_Type_Id");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.EsTranFiles)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_ES_TRAN_Files_Inspection_Id");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.EsTranFiles)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ES_TRAN_Files_Report_Id");
            });

            modelBuilder.Entity<EsType>(entity =>
            {
                entity.ToTable("ES_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<EventBookingLog>(entity =>
            {
                entity.ToTable("EventBookingLog");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.BookingId).HasColumnName("Booking_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.QuotationId).HasColumnName("Quotation_Id");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EventBookingLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventBook__Creat__34CB3BD6");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EventBookingLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_EventBookingLog_EntityId");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.ToTable("EventLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.LogLevel).HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(4000);

                entity.Property(e => e.Name).HasMaxLength(2000);

                entity.Property(e => e.ResponseTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<FbBookingRequestLog>(entity =>
            {
                entity.ToTable("FB_Booking_RequestLog");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.RequestUrl).HasMaxLength(1000);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.FbBookingRequestLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_FB_Booking_RequestLog_EntityId");
            });

            modelBuilder.Entity<FbReportAdditionalPhoto>(entity =>
            {
                entity.ToTable("FB_Report_Additional_Photos");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.Property(e => e.PhotoPath).HasMaxLength(1000);

                entity.HasOne(d => d.FbReportDetail)
                    .WithMany(p => p.FbReportAdditionalPhotos)
                    .HasForeignKey(d => d.FbReportDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__5B45EA79");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportAdditionalPhotos)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_InspSummary_Additional_Photos_ProductId");
            });

            modelBuilder.Entity<FbReportComment>(entity =>
            {
                entity.ToTable("FB_Report_Comments");

                entity.Property(e => e.Category).HasMaxLength(2000);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerReferenceCode).HasMaxLength(2000);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.SubCategory)
                    .HasColumnName("Sub_Category")
                    .HasMaxLength(2000);

                entity.Property(e => e.SubCategory2)
                    .HasColumnName("Sub_Category2")
                    .HasMaxLength(2000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportComments)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__61DDD96F");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportComments)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Report_Comments_ProductId");
            });

            modelBuilder.Entity<FbReportDefectPhoto>(entity =>
            {
                entity.ToTable("FB_Report_Defect_Photos");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.Property(e => e.Path).HasMaxLength(1000);

                entity.HasOne(d => d.Defect)
                    .WithMany(p => p.FbReportDefectPhotos)
                    .HasForeignKey(d => d.DefectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FB_Report_Defect_Photos_FbDefectId");
            });

            modelBuilder.Entity<FbReportDetail>(entity =>
            {
                entity.ToTable("FB_Report_Details");

                entity.Property(e => e.AqlCritical).HasColumnName("Aql_Critical");

                entity.Property(e => e.AqlLevel).HasColumnName("Aql_Level");

                entity.Property(e => e.AqlMajor).HasColumnName("Aql_Major");

                entity.Property(e => e.AqlMinor).HasColumnName("Aql_Minor");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DacorrelationDone)
                    .HasColumnName("DACorrelationDone")
                    .HasMaxLength(100);

                entity.Property(e => e.DacorrelationEmail)
                    .HasColumnName("DACorrelationEmail")
                    .HasMaxLength(500);

                entity.Property(e => e.DacorrelationEnabled).HasColumnName("DACorrelation_Enabled");

                entity.Property(e => e.DacorrelationInspectionSampling).HasColumnName("DACorrelationInspectionSampling");

                entity.Property(e => e.DacorrelationRate)
                    .HasColumnName("DACorrelationRate")
                    .HasMaxLength(500);

                entity.Property(e => e.DacorrelationResult)
                    .HasColumnName("DACorrelationResult")
                    .HasMaxLength(500);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ExternalReportNumber).HasMaxLength(200);

                entity.Property(e => e.FabricAcceptedQtyRoll).HasColumnName("Fabric_AcceptedQtyRoll");

                entity.Property(e => e.FabricFactoryType)
                    .HasColumnName("Fabric_factoryType")
                    .HasMaxLength(200);

                entity.Property(e => e.FabricInspectedQtyRoll).HasColumnName("Fabric_InspectedQtyRoll");

                entity.Property(e => e.FabricMachineSpeed)
                    .HasColumnName("Fabric_MachineSpeed")
                    .HasMaxLength(200);

                entity.Property(e => e.FabricNoOfLotsPresented).HasColumnName("Fabric_NoOfLotsPresented");

                entity.Property(e => e.FabricNoOfRollsPresented).HasColumnName("Fabric_NoOfRollsPresented");

                entity.Property(e => e.FabricPresentedQtyRoll).HasColumnName("Fabric_PresentedQtyRoll");

                entity.Property(e => e.FabricProducedQtyRoll).HasColumnName("Fabric_ProducedQtyRoll");

                entity.Property(e => e.FabricRejectedQtyRoll).HasColumnName("Fabric_RejectedQtyRoll");

                entity.Property(e => e.FabricType)
                    .HasColumnName("Fabric_Type")
                    .HasMaxLength(200);

                entity.Property(e => e.FabricTypeCheck)
                    .HasColumnName("Fabric_TypeCheck")
                    .HasMaxLength(200);

                entity.Property(e => e.FactoryTourBottleneckProductionStage).HasMaxLength(500);

                entity.Property(e => e.FactoryTourDone).HasMaxLength(100);

                entity.Property(e => e.FactoryTourIrregularitiesIdentified).HasMaxLength(500);

                entity.Property(e => e.FactoryTourNotConductedReason).HasMaxLength(500);

                entity.Property(e => e.FactoryTourResult).HasMaxLength(100);

                entity.Property(e => e.FbFillingStatus).HasColumnName("Fb_Filling_Status");

                entity.Property(e => e.FbReportMapId).HasColumnName("FB_Report_Map_Id");

                entity.Property(e => e.FbReportStatus).HasColumnName("Fb_Report_Status");

                entity.Property(e => e.FbReviewStatus).HasColumnName("Fb_Review_Status");

                entity.Property(e => e.FillingValidatedFirstTime).HasMaxLength(100);

                entity.Property(e => e.FinalManualReportPath).HasMaxLength(1000);

                entity.Property(e => e.FinalReportPath).HasMaxLength(1000);

                entity.Property(e => e.FoundCritical).HasColumnName("Found_Critical");

                entity.Property(e => e.FoundMajor).HasColumnName("Found_Major");

                entity.Property(e => e.FoundMinor).HasColumnName("Found_Minor");

                entity.Property(e => e.InspectionDurationMins).HasMaxLength(500);

                entity.Property(e => e.InspectionEndTime).HasMaxLength(100);

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.InspectionStartTime).HasMaxLength(100);

                entity.Property(e => e.InspectionStartedDate).HasColumnType("date");

                entity.Property(e => e.InspectionSubmittedDate).HasColumnType("date");

                entity.Property(e => e.KeyStyleHighRisk).HasMaxLength(500);

                entity.Property(e => e.MainObservations).HasColumnName("Main_Observations");

                entity.Property(e => e.MainProductPhoto).HasMaxLength(1000);

                entity.Property(e => e.MasterCartonPackedQuantityCtns).HasMaxLength(500);

                entity.Property(e => e.MissionTitle).HasMaxLength(1500);

                entity.Property(e => e.NumberPommeasured).HasColumnName("NumberPOMMeasured");

                entity.Property(e => e.OverAllResult).HasMaxLength(1500);

                entity.Property(e => e.ProductCategory).HasMaxLength(500);

                entity.Property(e => e.Region).HasMaxLength(500);

                entity.Property(e => e.ReportPicturePath)
                    .HasColumnName("Report_Picture_Path")
                    .HasMaxLength(1000);

                entity.Property(e => e.ReportSummaryLink).HasMaxLength(4000);

                entity.Property(e => e.ReportTitle).HasMaxLength(1500);

                entity.Property(e => e.ReviewValidatedFirstTime).HasMaxLength(100);

                entity.Property(e => e.ServiceFromDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceToDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AqlLevelNavigation)
                    .WithMany(p => p.FbReportDetails)
                    .HasForeignKey(d => d.AqlLevel)
                    .HasConstraintName("FK_FB_Report_Details_Aql_Level");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FbReportDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FB_REPORT_DETAIL_CREATED_BY");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.FbReportDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FB_REPORT_DETAIL_DELETED_BY");

                entity.HasOne(d => d.FbFillingStatusNavigation)
                    .WithMany(p => p.FbReportDetailFbFillingStatusNavigations)
                    .HasForeignKey(d => d.FbFillingStatus)
                    .HasConstraintName("FK__FB_Report__Fb_Fi__7AF39FFC");

                entity.HasOne(d => d.FbReportStatusNavigation)
                    .WithMany(p => p.FbReportDetailFbReportStatusNavigations)
                    .HasForeignKey(d => d.FbReportStatus)
                    .HasConstraintName("FK__FB_Report__Fb_Re__7CDBE86E");

                entity.HasOne(d => d.FbReviewStatusNavigation)
                    .WithMany(p => p.FbReportDetailFbReviewStatusNavigations)
                    .HasForeignKey(d => d.FbReviewStatus)
                    .HasConstraintName("FK__FB_Report__Fb_Re__7BE7C435");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.FbReportDetails)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FB_Report_Details_Inspection_Id");

                entity.HasOne(d => d.Result)
                    .WithMany(p => p.FbReportDetails)
                    .HasForeignKey(d => d.ResultId)
                    .HasConstraintName("FK__FB_Report__Resul__52B0A478");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.FbReportDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FB_REPORT_DETAIL_UPDATED_BY");
            });

            modelBuilder.Entity<FbReportFabricControlmadeWith>(entity =>
            {
                entity.ToTable("FB_Report_FabricControlmadeWith");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.HasOne(d => d.ReportDetails)
                    .WithMany(p => p.FbReportFabricControlmadeWiths)
                    .HasForeignKey(d => d.ReportDetailsId)
                    .HasConstraintName("FB_Report_FabricControlmadeWith_ReportDetailsId");
            });

            modelBuilder.Entity<FbReportFabricDefect>(entity =>
            {
                entity.ToTable("FB_Report_FabricDefects");

                entity.Property(e => e.AcceptanceCriteria).HasMaxLength(200);

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DyeLot).HasMaxLength(200);

                entity.Property(e => e.LengthActual).HasMaxLength(200);

                entity.Property(e => e.LengthOriginal).HasMaxLength(200);

                entity.Property(e => e.LengthUnit).HasMaxLength(100);

                entity.Property(e => e.Location).HasMaxLength(200);

                entity.Property(e => e.Point).HasMaxLength(200);

                entity.Property(e => e.Points100Sqy).HasMaxLength(200);

                entity.Property(e => e.Result).HasMaxLength(100);

                entity.Property(e => e.RollNumber).HasMaxLength(100);

                entity.Property(e => e.WeightActual).HasMaxLength(200);

                entity.Property(e => e.WeightOriginal).HasMaxLength(200);

                entity.Property(e => e.WidthActual).HasMaxLength(200);

                entity.Property(e => e.WidthOriginal).HasMaxLength(200);

                entity.HasOne(d => d.Fbreportdetails)
                    .WithMany(p => p.FbReportFabricDefects)
                    .HasForeignKey(d => d.FbreportdetailsId)
                    .HasConstraintName("FB_Report_FabricDefects_FbreportdetailsId");

                entity.HasOne(d => d.InspColorTransaction)
                    .WithMany(p => p.FbReportFabricDefects)
                    .HasForeignKey(d => d.InspColorTransactionId)
                    .HasConstraintName("FB_Report_FabricDefects_InspColorTransactionId");

                entity.HasOne(d => d.InspPoTransaction)
                    .WithMany(p => p.FbReportFabricDefects)
                    .HasForeignKey(d => d.InspPoTransactionId)
                    .HasConstraintName("FB_Report_FabricDefects_InspPoTransactionId");
            });

            modelBuilder.Entity<FbReportInspDefect>(entity =>
            {
                entity.ToTable("FB_Report_InspDefects");

                entity.Property(e => e.CategoryName).HasMaxLength(3000);

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DefectInfo).HasMaxLength(2000);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.Property(e => e.GarmentGrade)
                    .HasColumnName("Garment_Grade")
                    .HasMaxLength(100);

                entity.Property(e => e.Position).HasMaxLength(100);

                entity.Property(e => e.QtyRejected).HasColumnName("Qty_Rejected");

                entity.Property(e => e.QtyReplaced).HasColumnName("Qty_Replaced");

                entity.Property(e => e.QtyReworked).HasColumnName("Qty_Reworked");

                entity.Property(e => e.Reparability).HasMaxLength(100);

                entity.Property(e => e.Size).HasMaxLength(100);

                entity.Property(e => e.Zone).HasMaxLength(100);

                entity.HasOne(d => d.FbReportDetail)
                    .WithMany(p => p.FbReportInspDefects)
                    .HasForeignKey(d => d.FbReportDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__0941BF53");

                entity.HasOne(d => d.InspColorTransaction)
                    .WithMany(p => p.FbReportInspDefects)
                    .HasForeignKey(d => d.InspColorTransactionId)
                    .HasConstraintName("FK_FB_RPT_DEFECT_COLOR_TRANSACTION_ID");

                entity.HasOne(d => d.InspPoTransaction)
                    .WithMany(p => p.FbReportInspDefects)
                    .HasForeignKey(d => d.InspPoTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__InspP__122C0B0F");
            });

            modelBuilder.Entity<FbReportInspSubSummary>(entity =>
            {
                entity.ToTable("FB_Report_InspSub_Summary");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(1000);

                entity.Property(e => e.Result).HasMaxLength(1000);

                entity.HasOne(d => d.FbReportSummary)
                    .WithMany(p => p.FbReportInspSubSummaries)
                    .HasForeignKey(d => d.FbReportSummaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__1745C5E7");

                entity.HasOne(d => d.ResultNavigation)
                    .WithMany(p => p.FbReportInspSubSummaries)
                    .HasForeignKey(d => d.ResultId)
                    .HasConstraintName("FK__FB_Report__Resul__1839EA20");
            });

            modelBuilder.Entity<FbReportInspSummary>(entity =>
            {
                entity.ToTable("FB_Report_InspSummary");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(1000);

                entity.Property(e => e.Result).HasMaxLength(1000);

                entity.HasOne(d => d.FbReportDetail)
                  .WithMany(p => p.FbReportInspSummaries)
                  .HasForeignKey(d => d.FbReportDetailId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_FB_Report_InspSummary_FB_Report_Details");

                entity.HasOne(d => d.FbReportInspsumType)
                    .WithMany(p => p.FbReportInspSummaries)
                    .HasForeignKey(d => d.FbReportInspsumTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FB_Report_InspSummary_FB_Report_InspSummary_Type");

                entity.HasOne(d => d.ResultNavigation)
                    .WithMany(p => p.FbReportInspSummaries)
                    .HasForeignKey(d => d.ResultId)
                    .HasConstraintName("FK_FB_Report_InspSummary_FB_Report_Result");
            });

            modelBuilder.Entity<FbReportInspSummaryPhoto>(entity =>
            {
                entity.ToTable("FB_Report_InspSummary_Photo");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.Property(e => e.Photo).HasMaxLength(1000);

                entity.HasOne(d => d.FbReportSummary)
                    .WithMany(p => p.FbReportInspSummaryPhotos)
                    .HasForeignKey(d => d.FbReportSummaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__0D125037");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportInspSummaryPhotos)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_InspSummary_Photo_ProductId");
            });

            modelBuilder.Entity<FbReportInspSummaryType>(entity =>
            {
                entity.ToTable("FB_Report_InspSummary_Type");

                entity.Property(e => e.Type).HasMaxLength(100);
            });

            modelBuilder.Entity<FbReportManualLog>(entity =>
            {
                entity.ToTable("FB_Report_Manual_Log");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FbReportManualLogCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_FB_Report_Manual_Log_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.FbReportManualLogDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_FB_Report_Manual_Log_Deleted_By");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.FbReportManualLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_FB_Report_Manual_Log_EntityId");

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportManualLogs)
                    .HasForeignKey(d => d.FbReportId)
                    .HasConstraintName("FK_FB_Report_Manual_Log_Fb_Report_Id");
            });

            modelBuilder.Entity<FbReportOtherInformation>(entity =>
            {
                entity.ToTable("FB_Report_OtherInformation");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Result).HasMaxLength(2000);

                entity.Property(e => e.SubCategory).HasMaxLength(1000);

                entity.Property(e => e.SubCategory2)
                    .HasColumnName("Sub_Category2")
                    .HasMaxLength(1000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportOtherInformations)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__78C13EC7");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportOtherInformations)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OtherInformation_ProductId");
            });

            modelBuilder.Entity<FbReportPackingBatteryInfo>(entity =>
            {
                entity.ToTable("FB_Report_Packing_BatteryInfo");

                entity.Property(e => e.BatteryModel).HasMaxLength(2000);

                entity.Property(e => e.BatteryType).HasMaxLength(2000);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(2000);

                entity.Property(e => e.NetWeightPerQty).HasMaxLength(1000);

                entity.Property(e => e.Quantity).HasMaxLength(1000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportPackingBatteryInfos)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__64BA461A");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportPackingBatteryInfos)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Packing_BatteryInfo_ProductId");
            });

            modelBuilder.Entity<FbReportPackingDimention>(entity =>
            {
                entity.ToTable("FB_Report_Packing_Dimention");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DiscrepancyToPacking).HasMaxLength(1000);

                entity.Property(e => e.DiscrepancyToSpec).HasMaxLength(1000);

                entity.Property(e => e.MeasuredValuesH).HasMaxLength(2000);

                entity.Property(e => e.MeasuredValuesL).HasMaxLength(2000);

                entity.Property(e => e.MeasuredValuesW).HasMaxLength(2000);

                entity.Property(e => e.PackingType).HasMaxLength(2000);

                entity.Property(e => e.PrintedPackValuesH).HasMaxLength(2000);

                entity.Property(e => e.PrintedPackValuesL).HasMaxLength(2000);

                entity.Property(e => e.PrintedPackValuesW).HasMaxLength(2000);

                entity.Property(e => e.Result).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValuesH).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValuesL).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValuesW).HasMaxLength(2000);

                entity.Property(e => e.Tolerance).HasMaxLength(2000);

                entity.Property(e => e.Unit).HasMaxLength(100);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportPackingDimentions)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__6796B2C5");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportPackingDimentions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Packing_Dimention_ProductId");
            });

            modelBuilder.Entity<FbReportPackingInfo>(entity =>
            {
                entity.ToTable("FB_Report_PackingInfo");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(2000);

                entity.Property(e => e.MaterialType).HasMaxLength(2000);

                entity.Property(e => e.NetWeightPerQty).HasMaxLength(1000);

                entity.Property(e => e.Quantity).HasMaxLength(1000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportPackingInfos)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__08038257");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportPackingInfos)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_PackingInfo_ProductId");
            });

            modelBuilder.Entity<FbReportPackingPackagingLabellingProduct>(entity =>
            {
                entity.ToTable("FB_Report_PackingPackagingLabelling_Product");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.FbReportdetails)
                    .WithMany(p => p.FbReportPackingPackagingLabellingProducts)
                    .HasForeignKey(d => d.FbReportdetailsId)
                    .HasConstraintName("FK_FB_Report_PackingPackagingLabelling_Product_FbReportdetailsId");
            });

            modelBuilder.Entity<FbReportPackingWeight>(entity =>
            {
                entity.ToTable("FB_Report_Packing_Weight");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DiscrepancyToPacking).HasMaxLength(1000);

                entity.Property(e => e.DiscrepancyToSpec).HasMaxLength(1000);

                entity.Property(e => e.MeasuredValues).HasMaxLength(2000);

                entity.Property(e => e.PackingType).HasMaxLength(500);

                entity.Property(e => e.Result).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValues).HasMaxLength(2000);

                entity.Property(e => e.Tolerance).HasMaxLength(2000);

                entity.Property(e => e.Unit).HasMaxLength(100);

                entity.Property(e => e.WeightPackValues).HasMaxLength(2000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportPackingWeights)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__6A731F70");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportPackingWeights)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Packing_Weight_ProductId");
            });

            modelBuilder.Entity<FbReportProblematicRemark>(entity =>
            {
                entity.ToTable("FB_Report_Problematic_Remarks");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerRemarkCode).HasMaxLength(1000);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Result).HasMaxLength(2000);

                entity.Property(e => e.SubCategory)
                    .HasColumnName("Sub_Category")
                    .HasMaxLength(1000);

                entity.Property(e => e.SubCategory2)
                    .HasColumnName("Sub_Category2")
                    .HasMaxLength(1000);

                entity.HasOne(d => d.FbReportSummary)
                    .WithMany(p => p.FbReportProblematicRemarks)
                    .HasForeignKey(d => d.FbReportSummaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__6D4F8C1B");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportProblematicRemarks)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Problematic_Remarks_ProductId");
            });

            modelBuilder.Entity<FbReportProductBarcodesInfo>(entity =>
            {
                entity.ToTable("FB_Report_ProductBarcodesInfo");

                entity.Property(e => e.BarCode).HasMaxLength(500);

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportProductBarcodesInfos)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FB_Report_ProductBarcodesInfo_FbReportId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportProductBarcodesInfos)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FB_Report_ProductBarcodesInfo_ProductId");
            });

            modelBuilder.Entity<FbReportProductDimention>(entity =>
            {
                entity.ToTable("FB_Report_Product_Dimention");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DimensionPackValuesH).HasMaxLength(2000);

                entity.Property(e => e.DimensionPackValuesL).HasMaxLength(2000);

                entity.Property(e => e.DimensionPackValuesW).HasMaxLength(2000);

                entity.Property(e => e.DiscrepancyToPack).HasMaxLength(1000);

                entity.Property(e => e.DiscrepancyToSpec).HasMaxLength(1000);

                entity.Property(e => e.MeasuredValuesH).HasMaxLength(2000);

                entity.Property(e => e.MeasuredValuesL).HasMaxLength(2000);

                entity.Property(e => e.MeasuredValuesW).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValuesH).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValuesL).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValuesW).HasMaxLength(2000);

                entity.Property(e => e.Tolerance).HasMaxLength(2000);

                entity.Property(e => e.Unit).HasMaxLength(100);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportProductDimentions)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__702BF8C6");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportProductDimentions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Product_Dimention_ProductId");
            });

            modelBuilder.Entity<FbReportProductWeight>(entity =>
            {
                entity.ToTable("FB_Report_Product_Weight");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DiscrepancyToPack).HasMaxLength(1000);

                entity.Property(e => e.DiscrepancyToSpec).HasMaxLength(1000);

                entity.Property(e => e.MeasuredValues).HasMaxLength(2000);

                entity.Property(e => e.SpecClientValues).HasMaxLength(2000);

                entity.Property(e => e.Tolerance).HasMaxLength(2000);

                entity.Property(e => e.Unit).HasMaxLength(100);

                entity.Property(e => e.WeightPackValues).HasMaxLength(2000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportProductWeights)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__73086571");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportProductWeights)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Product_Weight_ProductId");
            });

            modelBuilder.Entity<FbReportQcdetail>(entity =>
            {
                entity.ToTable("FB_Report_QCDetails");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.FbReportDetail)
                    .WithMany(p => p.FbReportQcdetails)
                    .HasForeignKey(d => d.FbReportDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__0FEEBCE2");
            });

            modelBuilder.Entity<FbReportQuantityDetail>(entity =>
            {
                entity.ToTable("FB_Report_Quantity_Details");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FabricAcceptanceCriteria).HasColumnName("Fabric_AcceptanceCriteria");

                entity.Property(e => e.FabricDemeritPts).HasColumnName("Fabric_DemeritPts");

                entity.Property(e => e.FabricOverLessProducedQty)
                    .HasColumnName("Fabric_OverLessProducedQty")
                    .HasMaxLength(100);

                entity.Property(e => e.FabricPoints100Sqy).HasColumnName("Fabric_Points100Sqy");

                entity.Property(e => e.FabricRating)
                    .HasColumnName("Fabric_Rating")
                    .HasMaxLength(100);

                entity.Property(e => e.FabricRejectedQuantity).HasColumnName("Fabric_RejectedQuantity");

                entity.Property(e => e.FabricRejectedRolls).HasColumnName("Fabric_RejectedRolls");

                entity.Property(e => e.FabricTolerance).HasColumnName("Fabric_Tolerance");

                entity.Property(e => e.SelectCtnNo).HasColumnName("SelectCtnNO");

                entity.HasOne(d => d.FbReportDetail)
                    .WithMany(p => p.FbReportQuantityDetails)
                    .HasForeignKey(d => d.FbReportDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__05712E6F");

                entity.HasOne(d => d.InspColorTransaction)
                    .WithMany(p => p.FbReportQuantityDetails)
                    .HasForeignKey(d => d.InspColorTransactionId)
                    .HasConstraintName("FK_FB_RPT_DTL_COLOR_TRANSACTION_ID");

                entity.HasOne(d => d.InspPoTransaction)
                    .WithMany(p => p.FbReportQuantityDetails)
                    .HasForeignKey(d => d.InspPoTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__InspP__1137E6D6");
            });

            modelBuilder.Entity<FbReportRdnumber>(entity =>
            {
                entity.ToTable("FB_Report_RDNumbers");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Rdnumber)
                    .HasColumnName("RDNumber")
                    .HasMaxLength(500);

                entity.HasOne(d => d.PoColor)
                    .WithMany(p => p.FbReportRdnumbers)
                    .HasForeignKey(d => d.PoColorId)
                    .HasConstraintName("FK_FB_Report_RDNumbers_PoColorId");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.FbReportRdnumbers)
                    .HasForeignKey(d => d.PoId)
                    .HasConstraintName("FK_FB_Report_RDNumbers_PoId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportRdnumbers)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_FB_Report_RDNumbers_ProductId");

                entity.HasOne(d => d.Reportdetails)
                    .WithMany(p => p.FbReportRdnumbers)
                    .HasForeignKey(d => d.ReportdetailsId)
                    .HasConstraintName("FK_FB_Report_RDNumbers_ReportdetailsId");
            });

            modelBuilder.Entity<FbReportResult>(entity =>
            {
                entity.ToTable("FB_Report_Result");

                entity.Property(e => e.ResultName).HasMaxLength(200);
            });

            modelBuilder.Entity<FbReportReviewer>(entity =>
            {
                entity.ToTable("FB_Report_Reviewer");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportReviewers)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__7B9DAB72");
            });

            modelBuilder.Entity<FbReportSamplePicking>(entity =>
            {
                entity.ToTable("FB_Report_Sample_Pickings");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Destination).HasMaxLength(2000);

                entity.Property(e => e.Quantity).HasMaxLength(1000);

                entity.Property(e => e.SampleType).HasMaxLength(2000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportSamplePickings)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__75E4D21C");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportSamplePickings)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Sample_Pickings_ProductId");
            });

            modelBuilder.Entity<FbReportSampleType>(entity =>
            {
                entity.ToTable("FB_Report_SampleTypes");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Quantity).HasMaxLength(1000);

                entity.Property(e => e.SampleType).HasMaxLength(1000);

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.FbReportSampleTypes)
                    .HasForeignKey(d => d.FbReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Report__FbRep__7E7A181D");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FbReportSampleTypes)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_SampleTypeProductId");
            });

            modelBuilder.Entity<FbReportTemplate>(entity =>
            {
                entity.ToTable("FB_Report_Template");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.Name).HasMaxLength(1000);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.FbReportTemplates)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_FB_Report_Template_Entity_Id");
            });

            modelBuilder.Entity<FbStatus>(entity =>
            {
                entity.ToTable("FB_Status");

                entity.Property(e => e.FbstatusName)
                    .HasColumnName("FBStatusName")
                    .HasMaxLength(200);

                entity.Property(e => e.StatusName).HasMaxLength(200);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.FbStatuses)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FB_Status__Type__762EEADF");
            });

            modelBuilder.Entity<FbStatusType>(entity =>
            {
                entity.ToTable("FB_Status_Type");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<HrAttachment>(entity =>
            {
                entity.ToTable("HR_Attachment");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__HR_FileA__4B840D04218F17D7")
                    .IsUnique();

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.StaffId).HasColumnName("Staff_Id");

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.HrAttachmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__HR_FileAt__Delet__2653CAA4");

                entity.HasOne(d => d.FileType)
                    .WithMany(p => p.HrAttachments)
                    .HasForeignKey(d => d.FileTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_FileAt__FileT__23775DF9");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrAttachments)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_FileAt__Staff__246B8232");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HrAttachmentUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_FileAt__UserI__255FA66B");
            });

            modelBuilder.Entity<HrDepartment>(entity =>
            {
                entity.ToTable("HR_Department");

                entity.Property(e => e.DepartmentCode)
                    .HasColumnName("Department_Code")
                    .HasMaxLength(50);

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasColumnName("Department_Name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.DeptParent)
                    .WithMany(p => p.InverseDeptParent)
                    .HasForeignKey(d => d.DeptParentId)
                    .HasConstraintName("FK__HR_Depart__DeptP__5458F5C8");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrDepartments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Depart__Entit__5364D18F");
            });

            modelBuilder.Entity<HrEmployeeType>(entity =>
            {
                entity.ToTable("HR_EMployeeType");

                entity.Property(e => e.EmployeeTypeName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrEmployeeTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_EMploy__Entit__554D1A01");
            });

            modelBuilder.Entity<HrEntityMap>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.EntityId })
                    .HasName("PK__HR_Entit__5F1C39EE6D71E3CC");

                entity.ToTable("HR_Entity_Map");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrEntityMaps)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HR_Entity_Map_EntityId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrEntityMaps)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HR_Entity_Map_ContactId");
            });

            modelBuilder.Entity<HrFileAttachment>(entity =>
            {
                entity.ToTable("HR_FileAttachment");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__HR_FileA__4B840D0480B2EEB9")
                    .IsUnique();

                entity.Property(e => e.FullFileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.FileType)
                    .WithMany(p => p.HrFileAttachments)
                    .HasForeignKey(d => d.FileTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_FileAt__FileT__582986AC");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrFileAttachments)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_FileAt__staff__57356273");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HrFileAttachments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_FileAt__UserI__56413E3A");
            });

            modelBuilder.Entity<HrFileType>(entity =>
            {
                entity.ToTable("HR_FileType");

                entity.Property(e => e.FileTypeName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrFileTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_FileTy__Entit__591DAAE5");
            });

            modelBuilder.Entity<HrHoliday>(entity =>
            {
                entity.ToTable("HR_Holiday");

                entity.Property(e => e.CountryId).HasColumnName("Country_Id");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.HolidayName)
                    .IsRequired()
                    .HasColumnName("Holiday_Name")
                    .HasMaxLength(200);

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.RecurrenceType).HasColumnName("recurrence_type");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.HrHolidays)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Holida__Count__5A11CF1E");

                entity.HasOne(d => d.EndDateTypeNavigation)
                    .WithMany(p => p.HrHolidayEndDateTypeNavigations)
                    .HasForeignKey(d => d.EndDateType)
                    .HasConstraintName("FK__HR_Holida__EndDa__5DE26002");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrHolidays)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Holida__Entit__5BFA1790");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.HrHolidays)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__HR_Holida__locat__5B05F357");

                entity.HasOne(d => d.StartDateTypeNavigation)
                    .WithMany(p => p.HrHolidayStartDateTypeNavigations)
                    .HasForeignKey(d => d.StartDateType)
                    .HasConstraintName("FK__HR_Holida__Start__5CEE3BC9");
            });

            modelBuilder.Entity<HrHolidayDayType>(entity =>
            {
                entity.ToTable("HR_HolidayDayType");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<HrLeave>(entity =>
            {
                entity.ToTable("HR_Leave");

                entity.Property(e => e.ApprovedId).HasColumnName("Approved_Id");

                entity.Property(e => e.ApprovedLeaveDays).HasColumnName("Approved_Leave_days");

                entity.Property(e => e.ApprovedOn).HasColumnType("datetime");

                entity.Property(e => e.CancelledOn).HasColumnType("datetime");

                entity.Property(e => e.CheckedId).HasColumnName("Checked_Id");

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.Comments1).HasMaxLength(100);

                entity.Property(e => e.Comments2).HasMaxLength(100);

                entity.Property(e => e.DateBegin)
                    .HasColumnName("Date_Begin")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateEnd)
                    .HasColumnName("Date_End")
                    .HasColumnType("datetime");

                entity.Property(e => e.LeaveApplicationDate)
                    .HasColumnName("Leave_application_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveType_Id");

                entity.Property(e => e.NumberOfDays).HasColumnName("Number_Of_Days");

                entity.Property(e => e.RejectedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Approved)
                    .WithMany(p => p.HrLeaveApproveds)
                    .HasForeignKey(d => d.ApprovedId)
                    .HasConstraintName("FK__HR_Leave__Approv__648F5D91");

                entity.HasOne(d => d.Checked)
                    .WithMany(p => p.HrLeaveCheckeds)
                    .HasForeignKey(d => d.CheckedId)
                    .HasConstraintName("FK__HR_Leave__Checke__639B3958");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrLeaves)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("HR_Leave_EntityId");

                entity.HasOne(d => d.IdTypeEndDateNavigation)
                    .WithMany(p => p.HrLeaveIdTypeEndDateNavigations)
                    .HasForeignKey(d => d.IdTypeEndDate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Leave__IdType__5FCAA874");

                entity.HasOne(d => d.IdTypeStartDateNavigation)
                    .WithMany(p => p.HrLeaveIdTypeStartDateNavigations)
                    .HasForeignKey(d => d.IdTypeStartDate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Leave__IdType__5ED6843B");

                entity.HasOne(d => d.LeaveType)
                    .WithMany(p => p.HrLeaves)
                    .HasForeignKey(d => d.LeaveTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Leave__LeaveT__62A7151F");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrLeaves)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Leave__StaffI__60BECCAD");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.HrLeaves)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK__HR_Leave__Status__61B2F0E6");
            });

            modelBuilder.Entity<HrLeaveStatus>(entity =>
            {
                entity.ToTable("HR_Leave_Status");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrLeaveStatuses)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Leave___Entit__658381CA");
            });

            modelBuilder.Entity<HrLeaveType>(entity =>
            {
                entity.ToTable("HR_Leave_Type");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalDays).HasColumnName("Total_Days");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrLeaveTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Leave___Entit__6677A603");
            });

            modelBuilder.Entity<HrOfficeControl>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.LocationId })
                    .HasName("PK__HR_Offic__F8AB415E047DB442");

                entity.ToTable("HR_OfficeControl");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrOfficeControls)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_HR_OfficeControl_ENTITY_ID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.HrOfficeControls)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Office__Locat__676BCA3C");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrOfficeControls)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Office__Staff__685FEE75");
            });

            modelBuilder.Entity<HrOutSourceCompany>(entity =>
            {
                entity.ToTable("HR_OutSource_Company");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.HrOutSourceCompanyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_HR_OUTSOURCE_COMPANY_CREATEDBY");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.HrOutSourceCompanyDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_HR_OUTSOURCE_COMPANY_DELETEDBY");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrOutSourceCompanies)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_HR_OUTSOURCE_COMPANY_ENTITYID");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.HrOutSourceCompanyUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_HR_OUTSOURCE_COMPANY_UPDATEDBY");
            });

            modelBuilder.Entity<HrPayrollCompany>(entity =>
            {
                entity.ToTable("HR_PayrollCompany");

                entity.Property(e => e.AccountEmail).HasMaxLength(1000);

                entity.Property(e => e.CompanyName).HasMaxLength(200);

                entity.HasOne(d => d.EntityNavigation)
                    .WithMany(p => p.HrPayrollCompanies)
                    .HasForeignKey(d => d.Entity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HR_Payrol_Entity");
            });

            modelBuilder.Entity<HrPhoto>(entity =>
            {
                entity.HasKey(e => e.GuidId)
                    .HasName("PK__HR_Staff__4B840D05BD3BC01B");

                entity.ToTable("HR_Photo");

                entity.Property(e => e.GuidId).ValueGeneratedNever();

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.HrPhotoDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__HR_StaffP__Delet__2C0CA3FA");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrPhotos)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_StaffP__Staff__2B187FC1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HrPhotoUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_StaffP__UserI__2D00C833");
            });

            modelBuilder.Entity<HrPosition>(entity =>
            {
                entity.ToTable("HR_Position");

                entity.Property(e => e.PositionName)
                    .IsRequired()
                    .HasColumnName("Position_Name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrPositions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Positi__Entit__695412AE");
            });

            modelBuilder.Entity<HrProfile>(entity =>
            {
                entity.ToTable("HR_Profile");

                entity.Property(e => e.ProfileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrProfiles)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Profil__Entit__6A4836E7");
            });

            modelBuilder.Entity<HrQualification>(entity =>
            {
                entity.ToTable("HR_Qualification");

                entity.Property(e => e.QualificationName)
                    .IsRequired()
                    .HasColumnName("Qualification_Name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrQualifications)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Qualif__Entit__6B3C5B20");
            });

            modelBuilder.Entity<HrRefBand>(entity =>
            {
                entity.ToTable("HR_REF_Band");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HrRefSocialInsuranceType>(entity =>
            {
                entity.ToTable("HR_REF_Social_Insurance_type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HrRefStatus>(entity =>
            {
                entity.ToTable("HR_REF_Status");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HrRenew>(entity =>
            {
                entity.ToTable("HR_Renew");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrRenews)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__HR_Renew__Entity__6C307F59");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrRenews)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Renew__staff___6D24A392");
            });

            modelBuilder.Entity<HrStaff>(entity =>
            {
                entity.ToTable("HR_Staff");

                entity.Property(e => e.AnnualLeave).HasMaxLength(10);

                entity.Property(e => e.BankAccountNo)
                    .HasColumnName("Bank_Account_No")
                    .HasMaxLength(50);

                entity.Property(e => e.BankName)
                    .HasColumnName("Bank_Name")
                    .HasMaxLength(100);

                entity.Property(e => e.BirthDate)
                    .HasColumnName("Birth_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Comments).HasMaxLength(400);

                entity.Property(e => e.CompanyEmail).HasMaxLength(100);

                entity.Property(e => e.CompanyMobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_At")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("Created_By")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.CurrentAddress)
                    .HasColumnName("Current_Address")
                    .HasMaxLength(200);

                entity.Property(e => e.CurrentCityId).HasColumnName("Current_CityId");

                entity.Property(e => e.CurrentCountyId).HasColumnName("Current_CountyId");

                entity.Property(e => e.CurrentTown)
                    .HasColumnName("Current_Town")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentZipCode)
                    .HasColumnName("Current_ZipCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.Property(e => e.EmaiLaddress)
                    .HasColumnName("EmaiLAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyCall)
                    .HasColumnName("Emergency_Call")
                    .HasMaxLength(50);

                entity.Property(e => e.EmergencyContactName).HasMaxLength(200);

                entity.Property(e => e.EmergencyContactPhone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyContactRelationship).HasMaxLength(500);

                entity.Property(e => e.EmpNo)
                    .HasColumnName("Emp_no")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.GlCode)
                    .HasColumnName("GL_Code")
                    .HasMaxLength(50);

                entity.Property(e => e.GlobalGrading).HasMaxLength(500);

                entity.Property(e => e.GraduateDate).HasColumnType("datetime");

                entity.Property(e => e.GraduateSchool).HasMaxLength(200);

                entity.Property(e => e.HomeAddress)
                    .HasColumnName("Home_Address")
                    .HasMaxLength(200);

                entity.Property(e => e.HomeCityId).HasColumnName("Home_CityId");

                entity.Property(e => e.HousingFuncard)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HroutSourceCompanyId).HasColumnName("HROutSourceCompanyId");

                entity.Property(e => e.JoinDate)
                    .HasColumnName("Join_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LaborContractExpiredDate)
                    .HasColumnName("Labor_Contract_Expired_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LaborContractPeriod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeaveDate)
                    .HasColumnName("Leave_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LocalLanguage).HasMaxLength(10);

                entity.Property(e => e.LocationId).HasColumnName("Location_Id");

                entity.Property(e => e.MajorSubject).HasMaxLength(500);

                entity.Property(e => e.MaritalStatus)
                    .HasColumnName("Marital_Status")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt)
                    .HasColumnName("Modified_At")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("Modified_By")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.NameChinese)
                    .HasColumnName("Name_Chinese")
                    .HasMaxLength(50);

                entity.Property(e => e.NationalityCountryId).HasColumnName("Nationality_Country_Id");

                entity.Property(e => e.PaidHolidayDaysPerYear).HasColumnName("Paid_Holiday_Days_Per_Year");

                entity.Property(e => e.ParentStaffId).HasColumnName("Parent_Staff_Id");

                entity.Property(e => e.PassportNo)
                    .HasColumnName("Passport_no")
                    .HasMaxLength(100);

                entity.Property(e => e.PersonName)
                    .HasColumnName("Person_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.PlaceOfBirth)
                    .HasColumnName("place_of_Birth")
                    .HasMaxLength(100);

                entity.Property(e => e.PlacePurchasingSihf)
                    .HasColumnName("PlacePurchasingSIHF")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PositionId).HasColumnName("Position_Id");

                entity.Property(e => e.PreferCurrencyId).HasColumnName("Prefer_Currency_Id");

                entity.Property(e => e.ProbationExpiredDate)
                    .HasColumnName("Probation_Expired_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProbationPeriod).HasColumnName("Probation_Period");

                entity.Property(e => e.QualificationId).HasColumnName("Qualification_Id");

                entity.Property(e => e.SalaryCurrencyId).HasColumnName("salary_Currency_Id");

                entity.Property(e => e.SkypeId).HasMaxLength(10);

                entity.Property(e => e.SocialInsuranceCardNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartPort).HasColumnName("Start_Port");

                entity.Property(e => e.StartWorkingDate).HasColumnType("datetime");

                entity.Property(e => e.WorkingDaysOfWeek).HasColumnName("Working_Days_Of_Week");

                entity.Property(e => e.XeroDeptId).HasColumnName("Xero_DeptId");

                entity.HasOne(d => d.Band)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.BandId)
                    .HasConstraintName("FK_HR_Staff_BandId");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.HrStaffCompanies)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__HR_Staff__Compan__6A09D673");

                entity.HasOne(d => d.CurrentCity)
                    .WithMany(p => p.HrStaffCurrentCities)
                    .HasForeignKey(d => d.CurrentCityId)
                    .HasConstraintName("FK__HR_Staff__Curren__77A23205");

                entity.HasOne(d => d.CurrentCounty)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.CurrentCountyId)
                    .HasConstraintName("HR_Staff_Current_CountyId");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__HR_Staff__Depart__7896563E");

                entity.HasOne(d => d.EmployeeType)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.EmployeeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff__Employ__798A7A77");

                entity.HasOne(d => d.HomeCity)
                    .WithMany(p => p.HrStaffHomeCities)
                    .HasForeignKey(d => d.HomeCityId)
                    .HasConstraintName("FK__HR_Staff__Home_C__76AE0DCC");

                entity.HasOne(d => d.HroutSourceCompany)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.HroutSourceCompanyId)
                    .HasConstraintName("FK_HR_STaff_Outsource_Company_Id");

                entity.HasOne(d => d.HukoLocation)
                    .WithMany(p => p.HrStaffHukoLocations)
                    .HasForeignKey(d => d.HukoLocationId)
                    .HasConstraintName("FK_HR_Staff_HukoLocationId");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__HR_Staff__Locati__72DD7CE8");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__HR_Staff__Manage__70F53476");

                entity.HasOne(d => d.NationalityCountry)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.NationalityCountryId)
                    .HasConstraintName("FK__HR_Staff__Nation__73D1A121");

                entity.HasOne(d => d.ParentStaff)
                    .WithMany(p => p.InverseParentStaff)
                    .HasForeignKey(d => d.ParentStaffId)
                    .HasConstraintName("FK__HR_Staff__Parent__7001103D");

                entity.HasOne(d => d.PayrollCompanyNavigation)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.PayrollCompany)
                    .HasConstraintName("FK__HR_Staff__Payrol__4D388D9B");

                entity.HasOne(d => d.PayrollCurrency)
                    .WithMany(p => p.HrStaffPayrollCurrencies)
                    .HasForeignKey(d => d.PayrollCurrencyId)
                    .HasConstraintName("FK__HR_Staff__Payrol__75B9E993");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK__HR_Staff__Positi__6E18C7CB");

                entity.HasOne(d => d.PreferCurrency)
                    .WithMany(p => p.HrStaffPreferCurrencies)
                    .HasForeignKey(d => d.PreferCurrencyId)
                    .HasConstraintName("FK__HR_Staff__Prefer__74C5C55A");

                entity.HasOne(d => d.PrimaryEntityNavigation)
                    .WithMany(p => p.HrStaffPrimaryEntityNavigations)
                    .HasForeignKey(d => d.PrimaryEntity)
                    .HasConstraintName("FK_Hr_Staff_PrimaryEntity");

                entity.HasOne(d => d.Qualification)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.QualificationId)
                    .HasConstraintName("FK__HR_Staff__Qualif__71E958AF");

                entity.HasOne(d => d.SalaryCurrency)
                    .WithMany(p => p.HrStaffSalaryCurrencies)
                    .HasForeignKey(d => d.SalaryCurrencyId)
                    .HasConstraintName("FK__HR_Staff__salary__6F0CEC04");

                entity.HasOne(d => d.SocialInsuranceType)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.SocialInsuranceTypeId)
                    .HasConstraintName("FK_HR_Staff_SocialInsuranceTypeId");

                entity.HasOne(d => d.StartPortNavigation)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.StartPortId)
                    .HasConstraintName("FK_HR_Staff_StartPortId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.HrStaffs)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_HR_Staff_StatusId");

                entity.HasOne(d => d.XeroDept)
                   .WithMany(p => p.HrStaffs)
                   .HasForeignKey(d => d.XeroDeptId)
                   .HasConstraintName("HR_Staff_Xero_DeptId");
            });

            modelBuilder.Entity<HrStaffEntityServiceMap>(entity =>
            {
                entity.ToTable("HR_Staff_Entity_Service_Map");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.HrStaffEntityServiceMaps)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_HR_Staff_Entity_Service_Map_EntityId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.HrStaffEntityServiceMaps)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_HR_Staff_Entity_Service_Map_ServiceId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffEntityServiceMaps)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_HR_Staff_Entity_Service_Map_StaffId");
            });

            modelBuilder.Entity<HrStaffExpertise>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.ExpertiseId })
                    .HasName("PK__HR_Staff__106A0F763FC5330C");

                entity.ToTable("HR_Staff_Expertise");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.HasOne(d => d.Expertise)
                    .WithMany(p => p.HrStaffExpertises)
                    .HasForeignKey(d => d.ExpertiseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___Exper__7C66E722");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffExpertises)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___staff__7B72C2E9");
            });

            modelBuilder.Entity<HrStaffHistory>(entity =>
            {
                entity.ToTable("HR_staffHistory");

                entity.Property(e => e.Comments).HasMaxLength(100);

                entity.Property(e => e.Company).HasMaxLength(100);

                entity.Property(e => e.CurrencyId).HasColumnName("Currency_Id");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.Datebegin).HasColumnType("datetime");

                entity.Property(e => e.Position).HasMaxLength(100);

                entity.Property(e => e.SgtLocationId).HasColumnName("Sgt_Location_ID");

                entity.Property(e => e.StaffId).HasColumnName("Staff_Id");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.HrStaffHistories)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK__HR_staffH__Curre__05F0515C");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffHistories)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__HR_staffH__Staff__04FC2D23");
            });

            modelBuilder.Entity<HrStaffMarketSegment>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.MarketSegmentId })
                    .HasName("PK__HR_Staff__5709AD8FB1C291D3");

                entity.ToTable("HR_Staff_MarketSegment");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.HasOne(d => d.MarketSegment)
                    .WithMany(p => p.HrStaffMarketSegments)
                    .HasForeignKey(d => d.MarketSegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___Marke__7D5B0B5B");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffMarketSegments)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___staff__7E4F2F94");
            });

            modelBuilder.Entity<HrStaffOpCountry>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.CountryId })
                    .HasName("PK__HR_Staff__5E8B1099C8DFED01");

                entity.ToTable("HR_Staff_OpCountry");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.HrStaffOpCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___count__7F4353CD");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffOpCountries)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___staff__00377806");
            });

            modelBuilder.Entity<HrStaffPhoto>(entity =>
            {
                entity.HasKey(e => e.GuidId)
                    .HasName("PK__HR_Staff__4B840D053CD1BF4E");

                entity.ToTable("HR_StaffPhoto");

                entity.Property(e => e.GuidId).ValueGeneratedNever();

                entity.Property(e => e.PhotoMType)
                    .HasColumnName("Photo_mType")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffPhotos)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_StaffP__Staff__06E47595");
            });

            modelBuilder.Entity<HrStaffProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.ProductCategoryId })
                    .HasName("PK__HR_Staff__EA41935032538813");

                entity.ToTable("HR_Staff_ProductCategory");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.HrStaffProductCategories)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___Produ__021FC078");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffProductCategories)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___staff__012B9C3F");
            });

            modelBuilder.Entity<HrStaffProfile>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.ProfileId })
                    .HasName("PK__HR_Staff__F3886A9D3243C48E");

                entity.ToTable("HR_Staff_Profile");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.ProfileId).HasColumnName("profile_id");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.HrStaffProfiles)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___profi__040808EA");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffProfiles)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HR_Staff___staff__0313E4B1");
            });

            modelBuilder.Entity<HrStaffService>(entity =>
            {
                entity.ToTable("HR_Staff_Services");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.HrStaffServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_HR_Staff_Services_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.HrStaffServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_HR_Staff_Services_DeletedBy");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.HrStaffServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Staff_API_ServiceId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffServices)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_HR_Staff_Services_StaffId");
            });

            modelBuilder.Entity<HrStaffTraining>(entity =>
            {
                entity.ToTable("HR_StaffTraining");

                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.StaffId).HasColumnName("Staff_Id");

                entity.Property(e => e.Trainer).HasMaxLength(200);

                entity.Property(e => e.TrainingTopic)
                    .HasColumnName("Training_Topic")
                    .HasMaxLength(200);

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.HrStaffTrainings)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__HR_StaffT__Staff__07D899CE");
            });

            modelBuilder.Entity<HrStaffXeroDept>(entity =>
            {
                entity.ToTable("Hr_Staff_XeroDept");

                entity.Property(e => e.DeptName).HasMaxLength(250);
            });

            modelBuilder.Entity<InspBookingEmailConfiguration>(entity =>
            {
                entity.ToTable("INSP_BookingEmailConfiguration");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.BookingStatus)
                    .WithMany(p => p.InspBookingEmailConfigurations)
                    .HasForeignKey(d => d.BookingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INSP_BookingEmailConfiguration_BookingStatusId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspBookingEmailConfigurations)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INSP_BookingEmailConfiguration_CustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspBookingEmailConfigurations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_BookingEmailConfiguration_EntityId");

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.InspBookingEmailConfigurations)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .HasConstraintName("FK_INSP_BookingEmailConfiguration_FactoryCountryId");
            });

            modelBuilder.Entity<InspBookingRule>(entity =>
            {
                entity.ToTable("INSP_BookingRules");

                entity.Property(e => e.BookingRule)
                    .HasColumnName("Booking_Rule")
                    .HasMaxLength(3000);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.FactoryCountryId).HasColumnName("Factory_CountryId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspBookingRules)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__INSP_Book__Custo__014B97D0");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspBookingRules)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__INSP_Book__Entit__023FBC09");

                entity.HasOne(d => d.FactoryCountry)
                    .WithMany(p => p.InspBookingRules)
                    .HasForeignKey(d => d.FactoryCountryId)
                    .HasConstraintName("FK__INSP_Book__Facto__0333E042");
            });

            modelBuilder.Entity<InspCancelReason>(entity =>
            {
                entity.ToTable("INSP_Cancel_Reasons");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.IsApi).HasColumnName("IsAPI");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspCancelReasons)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__INSP_Canc__Custo__50DD6A9F");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspCancelReasons)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__INSP_Canc__Entit__51D18ED8");
            });

            modelBuilder.Entity<InspContainerTransaction>(entity =>
            {
                entity.ToTable("INSP_Container_Transaction");

                entity.Property(e => e.ContainerId).HasColumnName("Container_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FbReportId).HasColumnName("Fb_Report_Id");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ContainerSizeNavigation)
                    .WithMany(p => p.InspContainerTransactions)
                    .HasForeignKey(d => d.ContainerSize)
                    .HasConstraintName("FK__INSP_Cont__Conta__5D8338A6");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspContainerTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_Cont__Creat__57CA5F50");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspContainerTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_Cont__Delet__59B2A7C2");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspContainerTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_Container_Transaction_EntityId");

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.InspContainerTransactions)
                    .HasForeignKey(d => d.FbReportId)
                    .HasConstraintName("FK__INSP_Cont__Fb_Re__5AA6CBFB");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspContainerTransactions)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Cont__Inspe__56D63B17");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspContainerTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__INSP_Cont__Updat__58BE8389");
            });

            modelBuilder.Entity<InspCuStatus>(entity =>
            {
                entity.ToTable("INSP_CU_Status");

                entity.Property(e => e.CustomStatusName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspCuStatuses)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_CU_S__Custo__664D88D1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InspCuStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_CU_S__Statu__6741AD0A");
            });

            modelBuilder.Entity<InspDfTransaction>(entity =>
            {
                entity.ToTable("INSP_DF_Transaction");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.Value).HasMaxLength(100);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.InspDfTransactions)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_DF_T__Booki__311AA683");

                entity.HasOne(d => d.ControlConfiguration)
                    .WithMany(p => p.InspDfTransactions)
                    .HasForeignKey(d => d.ControlConfigurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_DF_T__Contr__320ECABC");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspDfTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_DF_T__Creat__3302EEF5");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspDfTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_DF_T__Delet__34EB3767");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspDfTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__INSP_DF_T__Updat__33F7132E");
            });

            modelBuilder.Entity<InspIcStatus>(entity =>
            {
                entity.ToTable("INSP_IC_Status");

                entity.Property(e => e.StatusName).HasMaxLength(200);
            });

            modelBuilder.Entity<InspIcTitle>(entity =>
            {
                entity.ToTable("INSP_IC_Title");

                entity.Property(e => e.Name).HasMaxLength(1000);
            });

            modelBuilder.Entity<InspIcTranProduct>(entity =>
            {
                entity.ToTable("INSP_IC_TRAN_Products");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Icid).HasColumnName("ICId");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BookingProduct)
                    .WithMany(p => p.InspIcTranProducts)
                    .HasForeignKey(d => d.BookingProductId)
                    .HasConstraintName("FK_INSP_IC_TRAN_Products_BookingProductId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspIcTranProductCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INSP_IC_TRAN_Products_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspIcTranProductDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INSP_IC_TRAN_Products_DeletedBy");

                entity.HasOne(d => d.Ic)
                    .WithMany(p => p.InspIcTranProducts)
                    .HasForeignKey(d => d.Icid)
                    .HasConstraintName("FK_INSP_IC_TRAN_Products_ICId");

                entity.HasOne(d => d.PoColor)
                    .WithMany(p => p.InspIcTranProducts)
                    .HasForeignKey(d => d.PoColorId)
                    .HasConstraintName("FK_INSP_IC_TRAN_Products_PoColorId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspIcTranProductUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INSP_IC_TRAN_Products_UpdatedBy");
            });

            modelBuilder.Entity<InspIcTransaction>(entity =>
            {
                entity.ToTable("INSP_IC_Transaction");

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.BeneficiaryName).HasMaxLength(500);

                entity.Property(e => e.BuyerName)
                    .HasColumnName("Buyer_Name")
                    .HasMaxLength(1000);

                entity.Property(e => e.Comment).HasMaxLength(2000);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Icno)
                    .HasColumnName("ICNO")
                    .HasMaxLength(2000);

                entity.Property(e => e.Icstatus).HasColumnName("ICStatus");

                entity.Property(e => e.IctitleId).HasColumnName("ICTitleId");

                entity.Property(e => e.SupplierAddress).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspIcTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INSP_IC_Transaction_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspIcTransactions)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_INSP_IC_Transaction_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspIcTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INSP_IC_Transaction_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspIcTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_IC_Transaction_EntityId");

                entity.HasOne(d => d.IcstatusNavigation)
                    .WithMany(p => p.InspIcTransactions)
                    .HasForeignKey(d => d.Icstatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INSP_IC_Transaction_ICStatus");

                entity.HasOne(d => d.Ictitle)
                    .WithMany(p => p.InspIcTransactions)
                    .HasForeignKey(d => d.IctitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INSP_IC_Transaction_ICTitle");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.InspIcTransactions)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_INSP_IC_Transaction_SupplierId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspIcTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INSP_IC_Transaction_UpdatedBy");
            });

            modelBuilder.Entity<InspLabAddress>(entity =>
            {
                entity.ToTable("INSP_LAB_Address");

                entity.Property(e => e.Address).HasMaxLength(2000);

                entity.Property(e => e.LabId).HasColumnName("Lab_Id");

                entity.Property(e => e.RegionalLanguage).HasMaxLength(2000);

                entity.Property(e => e.ZipCode).HasMaxLength(20);

                entity.HasOne(d => d.AddressType)
                    .WithMany(p => p.InspLabAddresses)
                    .HasForeignKey(d => d.AddressTypeId)
                    .HasConstraintName("FK__INSP_LAB___Addre__18640752");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.InspLabAddresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___CityI__19582B8B");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.InspLabAddresses)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Count__1A4C4FC4");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.InspLabAddresses)
                    .HasForeignKey(d => d.LabId)
                    .HasConstraintName("FK__INSP_LAB___Lab_I__1B4073FD");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.InspLabAddresses)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Provi__1C349836");
            });

            modelBuilder.Entity<InspLabAddressType>(entity =>
            {
                entity.ToTable("INSP_LAB_AddressType");

                entity.Property(e => e.AddressType)
                    .IsRequired()
                    .HasColumnName("Address_type")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspLabAddressTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__INSP_LAB___Entit__1D28BC6F");
            });

            modelBuilder.Entity<InspLabContact>(entity =>
            {
                entity.ToTable("INSP_LAB_Contact");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.Property(e => e.ContactName)
                    .HasColumnName("Contact_Name")
                    .HasMaxLength(200);

                entity.Property(e => e.Fax).HasMaxLength(200);

                entity.Property(e => e.JobTitle).HasMaxLength(250);

                entity.Property(e => e.LabId).HasColumnName("Lab_Id");

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.InspLabContacts)
                    .HasForeignKey(d => d.LabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Lab_I__1E1CE0A8");
            });

            modelBuilder.Entity<InspLabCustomer>(entity =>
            {
                entity.HasKey(e => new { e.LabId, e.CustomerId })
                    .HasName("PK__INSP_LAB__962D5A06ABAD2235");

                entity.ToTable("INSP_LAB_Customer");

                entity.Property(e => e.LabId).HasColumnName("Lab_Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspLabCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Custo__1F1104E1");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.InspLabCustomers)
                    .HasForeignKey(d => d.LabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Lab_I__2005291A");
            });

            modelBuilder.Entity<InspLabCustomerContact>(entity =>
            {
                entity.HasKey(e => new { e.LabId, e.CustomerId, e.ContactId })
                    .HasName("PK__INSP_LAB__7AAFF6C7DDFEEA72");

                entity.ToTable("INSP_LAB_Customer_Contact");

                entity.Property(e => e.LabId).HasColumnName("Lab_Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.InspLabCustomerContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Conta__20F94D53");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspLabCustomerContacts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Custo__21ED718C");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.InspLabCustomerContacts)
                    .HasForeignKey(d => d.LabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_LAB___Lab_I__22E195C5");
            });

            modelBuilder.Entity<InspLabDetail>(entity =>
            {
                entity.ToTable("INSP_LAB_Details");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.ContactPerson).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(200);

                entity.Property(e => e.GlCode).HasMaxLength(500);

                entity.Property(e => e.LabName)
                    .HasColumnName("Lab_Name")
                    .HasMaxLength(200);

                entity.Property(e => e.LegalName).HasMaxLength(200);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.RegionalName).HasMaxLength(200);

                entity.Property(e => e.TypeId).HasColumnName("Type_Id");

                entity.Property(e => e.Website).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspLabDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__INSP_LAB___Entit__23D5B9FE");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.InspLabDetails)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__INSP_LAB___Type___24C9DE37");
            });

            modelBuilder.Entity<InspLabType>(entity =>
            {
                entity.ToTable("INSP_LAB_Type");

                entity.Property(e => e.Type).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspLabTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__INSP_LAB___Entit__25BE0270");
            });

            modelBuilder.Entity<InspProductTransaction>(entity =>
            {
                entity.ToTable("INSP_Product_Transaction");

                entity.Property(e => e.Aql).HasColumnName("AQL");

                entity.Property(e => e.AqlQuantity).HasColumnName("AQL_Quantity");

                entity.Property(e => e.AsReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.CombineAqlQuantity).HasColumnName("Combine_AQL_Quantity");

                entity.Property(e => e.CombineProductId).HasColumnName("Combine_Product_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FbReportId).HasColumnName("Fb_Report_Id");

                entity.Property(e => e.FbtemplateId).HasColumnName("FBTemplateId");

                entity.Property(e => e.GoldenSampleComments).HasMaxLength(500);

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.ParentProductId).HasColumnName("Parent_Product_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.SampleCollectionComments).HasMaxLength(500);

                entity.Property(e => e.TfReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AqlNavigation)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.Aql)
                    .HasConstraintName("FK__INSP_Produc__AQL__79605D45");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspProductTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_Prod__Creat__7E251262");

                entity.HasOne(d => d.CriticalNavigation)
                    .WithMany(p => p.InspProductTransactionCriticalNavigations)
                    .HasForeignKey(d => d.Critical)
                    .HasConstraintName("FK__INSP_Prod__Criti__7A54817E");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspProductTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_Prod__Delet__000D5AD4");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_Product_Transaction_EntityId");

                entity.HasOne(d => d.FbReport)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.FbReportId)
                    .HasConstraintName("FK__INSP_Prod__Fb_Re__01017F0D");

                entity.HasOne(d => d.Fbtemplate)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.FbtemplateId)
                    .HasConstraintName("FK__INSP_Prod__FBTem__6800C719");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Prod__Inspe__786C390C");

                entity.HasOne(d => d.MajorNavigation)
                    .WithMany(p => p.InspProductTransactionMajorNavigations)
                    .HasForeignKey(d => d.Major)
                    .HasConstraintName("FK__INSP_Prod__Major__7B48A5B7");

                entity.HasOne(d => d.MinorNavigation)
                    .WithMany(p => p.InspProductTransactionMinorNavigations)
                    .HasForeignKey(d => d.Minor)
                    .HasConstraintName("FK__INSP_Prod__Minor__7C3CC9F0");

                entity.HasOne(d => d.PackingStatusNavigation)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.PackingStatus)
                    .HasConstraintName("FK__INSP_Prod__Packing__Status");

                entity.HasOne(d => d.ParentProduct)
                    .WithMany(p => p.InspProductTransactionParentProducts)
                    .HasForeignKey(d => d.ParentProductId)
                    .HasConstraintName("FK_INSP_Prd_Trans_ParentProduct");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InspProductTransactionProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Prod__Produ__01F5A346");

                entity.HasOne(d => d.ProductionStatusNavigation)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.ProductionStatus)
                    .HasConstraintName("FK__INSP_Prod__Production__Status");

                entity.HasOne(d => d.SampleTypeNavigation)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.SampleType)
                    .HasConstraintName("FK__INSP_Prod__Sampl__65796029");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.InspProductTransactions)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Produ__Unit__7D30EE29");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspProductTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__INSP_Prod__Updat__7F19369B");
            });

            modelBuilder.Entity<InspPurchaseOrderColorTransaction>(entity =>
            {
                entity.ToTable("INSP_PurchaseOrder_Color_Transaction");

                entity.Property(e => e.ColorCode).HasMaxLength(50);

                entity.Property(e => e.ColorName).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspPurchaseOrderColorTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Color_Trans_Created_By");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspPurchaseOrderColorTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Color_Trans_Deleted_By");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspPurchaseOrderColorTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Color_Trans_EntityId");

                entity.HasOne(d => d.PoTrans)
                    .WithMany(p => p.InspPurchaseOrderColorTransactions)
                    .HasForeignKey(d => d.PoTransId)
                    .HasConstraintName("FK_Color_Trans_Po_Trans_Id");

                entity.HasOne(d => d.ProductRef)
                    .WithMany(p => p.InspPurchaseOrderColorTransactions)
                    .HasForeignKey(d => d.ProductRefId)
                    .HasConstraintName("FK_Color_Trans_Product_Trans_Id");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspPurchaseOrderColorTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Color_Trans_Updated_By");
            });

            modelBuilder.Entity<InspPurchaseOrderTransaction>(entity =>
            {
                entity.ToTable("INSP_PurchaseOrder_Transaction");

                entity.HasIndex(e => e.ProductRefId)
                    .HasName("NCI_INSPPT_ProductRef");

                entity.Property(e => e.ContainerRefId).HasColumnName("Container_Ref_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerReferencePo).HasMaxLength(1000);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DestinationCountryId).HasColumnName("Destination_Country_Id");

                entity.Property(e => e.Etd)
                    .HasColumnName("ETD")
                    .HasColumnType("datetime");

                entity.Property(e => e.FbMissionProductId).HasColumnName("Fb_Mission_Product_Id");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.PoId).HasColumnName("PO_Id");

                entity.Property(e => e.ProductRefId).HasColumnName("Product_Ref_Id");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ContainerRef)
                    .WithMany(p => p.InspPurchaseOrderTransactions)
                    .HasForeignKey(d => d.ContainerRefId)
                    .HasConstraintName("FK__INSP_Purc__Conta__5C8F146D");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspPurchaseOrderTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_Purc__Creat__07AE7C9C");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspPurchaseOrderTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_Purc__Delet__0996C50E");

                entity.HasOne(d => d.DestinationCountry)
                    .WithMany(p => p.InspPurchaseOrderTransactions)
                    .HasForeignKey(d => d.DestinationCountryId)
                    .HasConstraintName("FK__INSP_Purc__Desti__0A8AE947");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspPurchaseOrderTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_PurchaseOrder_Transaction_EntityId");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspPurchaseOrderTransactions)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Purc__Inspe__05C6342A");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.InspPurchaseOrderTransactions)
                    .HasForeignKey(d => d.PoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Purc__PO_Id__06BA5863");

                entity.HasOne(d => d.ProductRef)
                    .WithMany(p => p.InspPurchaseOrderTransactions)
                    .HasForeignKey(d => d.ProductRefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Purc__Produ__04D20FF1");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspPurchaseOrderTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__INSP_Purc__Updat__08A2A0D5");
            });

            modelBuilder.Entity<InspRefBookingType>(entity =>
            {
                entity.ToTable("INSP_REF_BookingType");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<InspRefDpPoint>(entity =>
            {
                entity.ToTable("INSP_REF_DP_Point");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InspRefFileAttachmentCategory>(entity =>
            {
                entity.ToTable("INSP_REF_FileAttachment_Category");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<InspRefHoldReason>(entity =>
            {
                entity.ToTable("INSP_REF_Hold_Reasons");

                entity.Property(e => e.Reason).HasMaxLength(500);
            });

            modelBuilder.Entity<InspRefInspectionLocation>(entity =>
            {
                entity.ToTable("INSP_REF_InspectionLocation");

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<InspRefPackingStatus>(entity =>
            {
                entity.ToTable("INSP_REF_PackingStatus");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<InspRefPaymentOption>(entity =>
            {
                entity.ToTable("INSP_REF_PaymentOption");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspRefPaymentOptions)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_INSP_REF_PAYOPTION_CUSTOMERID");
            });

            modelBuilder.Entity<InspRefProductionStatus>(entity =>
            {
                entity.ToTable("INSP_REF_ProductionStatus");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<InspRefQuantityType>(entity =>
            {
                entity.ToTable("INSP_REF_QuantityType");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<InspRefReportRequest>(entity =>
            {
                entity.ToTable("INSP_REF_ReportRequest");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<InspRefShipmentType>(entity =>
            {
                entity.ToTable("INSP_REF_ShipmentType");

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<InspRepCusDecision>(entity =>
            {
                entity.ToTable("INSP_REP_CUS_Decision");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspRepCusDecisions)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CDCreatedBy");

                entity.HasOne(d => d.CustomerResult)
                    .WithMany(p => p.InspRepCusDecisions)
                    .HasForeignKey(d => d.CustomerResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CDCustomerResultId");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.InspRepCusDecisions)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CDReportId");
            });

            modelBuilder.Entity<InspRepCusDecisionTemplate>(entity =>
            {
                entity.ToTable("INSP_REP_CUS_Decision_Template");

                entity.Property(e => e.TemplatePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspRepCusDecisionTemplates)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_INSP_REP_CUS_Decision_Template_CU_Customer");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspRepCusDecisionTemplates)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_REP_CUS_Decision_Template_AP_Entity");
                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.InspRepCusDecisionTemplates)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_INSP_REP_CUS_Decision_Template_REF_ServiceType");
            });

            modelBuilder.Entity<InspRescheduleReason>(entity =>
            {
                entity.ToTable("INSP_Reschedule_Reasons");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.IsApi).HasColumnName("IsAPI");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspRescheduleReasons)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspRescheduleReasons)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_EntityId");
            });

            modelBuilder.Entity<InspStatus>(entity =>
            {
                entity.ToTable("INSP_Status");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.Status).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspStatuses)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__INSP_Stat__Entit__37DCB2AB");
            });

            modelBuilder.Entity<InspTranC>(entity =>
            {
                entity.ToTable("INSP_TRAN_CS");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranCCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INSP_TRAN_CS_CreatedBy");

                entity.HasOne(d => d.Cs)
                    .WithMany(p => p.InspTranCCs)
                    .HasForeignKey(d => d.CsId)
                    .HasConstraintName("FK_INSP_TRAN_CS_CsId");
                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranCDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INSP_TRAN_CS_DeletedBy");
                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranCS)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_INSP_TRAN_CS_InspectionId");
            });

            modelBuilder.Entity<InspTranCancel>(entity =>
            {
                entity.ToTable("INSP_TRAN_Cancel");

                entity.Property(e => e.Comments).HasMaxLength(500);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.InternalComments).HasMaxLength(500);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.TravellingExpense).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranCancelCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__38D0D6E4");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.InspTranCancels)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK__INSP_TRAN__Curre__39C4FB1D");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranCancels)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__3AB91F56");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.InspTranCancelModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__INSP_TRAN__Modif__3BAD438F");

                entity.HasOne(d => d.ReasonType)
                    .WithMany(p => p.InspTranCancels)
                    .HasForeignKey(d => d.ReasonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Reaso__3CA167C8");
            });

            modelBuilder.Entity<InspTranCuBrand>(entity =>
            {
                entity.ToTable("INSP_TRAN_CU_Brand");

                entity.Property(e => e.BrandId).HasColumnName("Brand_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.InspTranCuBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Brand__3D958C01");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranCuBrandCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__3F7DD473");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranCuBrandDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__4071F8AC");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranCuBrands)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__3E89B03A");
            });

            modelBuilder.Entity<InspTranCuBuyer>(entity =>
            {
                entity.ToTable("INSP_TRAN_CU_Buyer");

                entity.Property(e => e.BuyerId).HasColumnName("Buyer_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.InspTranCuBuyers)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Buyer__41661CE5");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranCuBuyerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__434E6557");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranCuBuyerDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__44428990");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranCuBuyers)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__425A411E");
            });

            modelBuilder.Entity<InspTranCuContact>(entity =>
            {
                entity.ToTable("INSP_TRAN_CU_Contact");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.InspTranCuContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Conta__4536ADC9");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranCuContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__462AD202");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranCuContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__471EF63B");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranCuContacts)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__48131A74");
            });

            modelBuilder.Entity<InspTranCuDepartment>(entity =>
            {
                entity.ToTable("INSP_TRAN_CU_Department");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranCuDepartmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__4AEF871F");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranCuDepartmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__4BE3AB58");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.InspTranCuDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Depar__49073EAD");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranCuDepartments)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__49FB62E6");
            });

            modelBuilder.Entity<InspTranCuMerchandiser>(entity =>
            {
                entity.ToTable("INSP_TRAN_CU_Merchandiser");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.MerchandiserId).HasColumnName("Merchandiser_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranCuMerchandiserCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__668292FB");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranCuMerchandiserDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__6776B734");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranCuMerchandisers)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__686ADB6D");
                entity.HasOne(d => d.Merchandiser)
                    .WithMany(p => p.InspTranCuMerchandisers)
                    .HasForeignKey(d => d.MerchandiserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INSP_TRAN_CU_Merchandiser_CU_Contact");
            });

            modelBuilder.Entity<InspTranFaContact>(entity =>
            {
                entity.ToTable("INSP_TRAN_FA_Contact");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.InspTranFaContacts)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK__INSP_TRAN__Conta__4CD7CF91");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranFaContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__4DCBF3CA");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranFaContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__4EC01803");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranFaContacts)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__4FB43C3C");
            });

            modelBuilder.Entity<InspTranFileAttachment>(entity =>
            {
                entity.ToTable("INSP_TRAN_File_Attachment");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FbId).HasColumnName("Fb_Id");

                entity.Property(e => e.FileAttachmentCategoryId).HasColumnName("FileAttachment_CategoryId");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.IsReportSendToFb).HasColumnName("IsReportSendToFB");

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranFileAttachmentDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__359542DD");

                entity.HasOne(d => d.FileAttachmentCategory)
                    .WithMany(p => p.InspTranFileAttachments)
                    .HasForeignKey(d => d.FileAttachmentCategoryId)
                    .HasConstraintName("FK_INSP_TRAN_FILE_ATTACHMENT_CATEGORY_ID");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranFileAttachments)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__33ACFA6B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InspTranFileAttachmentUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__UserI__34A11EA4");
            });

            modelBuilder.Entity<InspTranFileAttachmentZip>(entity =>
            {
                entity.ToTable("INSP_TRAN_File_Attachment_zip");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.FileUrl).HasMaxLength(500);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranFileAttachmentZipCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INSP_TRAN_FILE_ATTACH_ZIP_CREATED_BY");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranFileAttachmentZipDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INSP_TRAN_FILE_ATTACH_ZIP_DELETED_BY");

                entity.HasOne(d => d.FileAttachmentCategory)
                    .WithMany(p => p.InspTranFileAttachmentZips)
                    .HasForeignKey(d => d.FileAttachmentCategoryId)
                    .HasConstraintName("FK_INSP_TRAN_FILE_ATTACH_CATEGORY_ID");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranFileAttachmentZips)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_INSP_TRAN_FILE_ATTACH_ZIP_INSPECTION_ID");
            });

            modelBuilder.Entity<InspTranHoldReason>(entity =>
            {
                entity.ToTable("INSP_TRAN_Hold_reason");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranHoldReasons)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_INSPECTION_ID");

                entity.HasOne(d => d.ReasonTypeNavigation)
                    .WithMany(p => p.InspTranHoldReasons)
                    .HasForeignKey(d => d.ReasonType)
                    .HasConstraintName("FK_HOLD_REASON");
            });

            modelBuilder.Entity<InspTranPicking>(entity =>
            {
                entity.ToTable("INSP_TRAN_Picking");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.CusAddressId).HasColumnName("Cus_Address_Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DeletionDate).HasColumnType("datetime");

                entity.Property(e => e.LabAddressId).HasColumnName("Lab_Address_Id");

                entity.Property(e => e.LabId).HasColumnName("Lab_Id");

                entity.Property(e => e.PickingQty).HasColumnName("Picking_Qty");

                entity.Property(e => e.PoTranId).HasColumnName("PO_Tran_Id");

                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.Property(e => e.UpdationDate).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.InspTranPickings)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_INSP_PICKING_BOOKING_ID");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranPickingCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__556D1592");

                entity.HasOne(d => d.CusAddress)
                    .WithMany(p => p.InspTranPickings)
                    .HasForeignKey(d => d.CusAddressId)
                    .HasConstraintName("FK_INSP_PICKING_CUST_ADDRESS");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspTranPickings)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__INSP_TRAN__Custo__5384CD20");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranPickingDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__566139CB");

                entity.HasOne(d => d.LabAddress)
                    .WithMany(p => p.InspTranPickings)
                    .HasForeignKey(d => d.LabAddressId)
                    .HasConstraintName("FK_INSP_PICKING_LAB_ADDRESS_VALUE");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.InspTranPickings)
                    .HasForeignKey(d => d.LabId)
                    .HasConstraintName("FK_INSP_TRAN_Picking_INSP_LAB_Details");

                entity.HasOne(d => d.PoTran)
                    .WithMany(p => p.InspTranPickings)
                    .HasForeignKey(d => d.PoTranId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__PO_Tr__1043C29D");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspTranPickingUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Updat__57555E04");
            });

            modelBuilder.Entity<InspTranPickingContact>(entity =>
            {
                entity.ToTable("INSP_TRAN_Picking_Contacts");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CusContactId).HasColumnName("Cus_Contact_Id");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.LabContactId).HasColumnName("Lab_Contact_Id");

                entity.Property(e => e.PickingTranId).HasColumnName("Picking_Tran_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranPickingContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__5A31CAAF");

                entity.HasOne(d => d.CusContact)
                    .WithMany(p => p.InspTranPickingContacts)
                    .HasForeignKey(d => d.CusContactId)
                    .HasConstraintName("FK__INSP_TRAN__Cus_C__5849823D");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranPickingContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__5B25EEE8");

                entity.HasOne(d => d.LabContact)
                    .WithMany(p => p.InspTranPickingContacts)
                    .HasForeignKey(d => d.LabContactId)
                    .HasConstraintName("FK_INSP_PICKING_LAB_CONTACT");

                entity.HasOne(d => d.PickingTran)
                    .WithMany(p => p.InspTranPickingContacts)
                    .HasForeignKey(d => d.PickingTranId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Picki__593DA676");
            });

            modelBuilder.Entity<InspTranReschedule>(entity =>
            {
                entity.ToTable("INSP_TRAN_Reschedule");

                entity.Property(e => e.Comments).HasMaxLength(500);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.InternalComments).HasMaxLength(500);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceFromDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceToDate).HasColumnType("datetime");

                entity.Property(e => e.TravellingExpense).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranRescheduleCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_CreatedBy");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.InspTranReschedules)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_CurrencyId");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranReschedules)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InspectionId");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.InspTranRescheduleModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_ModifiedBy");

                entity.HasOne(d => d.ReasonType)
                    .WithMany(p => p.InspTranReschedules)
                    .HasForeignKey(d => d.ReasonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReasonTypeId");
            });

            modelBuilder.Entity<InspTranServiceType>(entity =>
            {
                entity.ToTable("INSP_TRAN_ServiceType");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceType_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranServiceTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__60DEC83E");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranServiceTypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__61D2EC77");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranServiceTypes)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__62C710B0");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.InspTranServiceTypes)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Servi__63BB34E9");
            });

            modelBuilder.Entity<InspTranShipmentType>(entity =>
            {
                entity.ToTable("INSP_TRAN_ShipmentType");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranShipmentTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INSP_SHIPMENT_CREATED_BY");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranShipmentTypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INSP_SHIPMENT_DELETED_BY");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranShipmentTypes)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_INSP_TRANSACTION");

                entity.HasOne(d => d.ShipmentType)
                    .WithMany(p => p.InspTranShipmentTypes)
                    .HasForeignKey(d => d.ShipmentTypeId)
                    .HasConstraintName("FK_INSP_SHIPMENT_TYPE");
            });

            modelBuilder.Entity<InspTranStatusLog>(entity =>
            {
                entity.ToTable("INSP_TRAN_Status_Log");

                entity.Property(e => e.BookingId).HasColumnName("Booking_Id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ServiceDateFrom).HasColumnType("datetime");

                entity.Property(e => e.ServiceDateTo).HasColumnType("datetime");

                entity.Property(e => e.StatusChangeDate).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.InspTranStatusLogs)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Booki__2959892A");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranStatusLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__2A4DAD63");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspTranStatusLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_TRAN_Status_Log_EntityId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InspTranStatusLogs)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Statu__2B41D19C");
            });

            modelBuilder.Entity<InspTranSuContact>(entity =>
            {
                entity.ToTable("INSP_TRAN_SU_Contact");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.InspTranSuContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Conta__64AF5922");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTranSuContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_TRAN__Creat__65A37D5B");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTranSuContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__INSP_TRAN__Delet__6697A194");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTranSuContacts)
                    .HasForeignKey(d => d.InspectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_TRAN__Inspe__678BC5CD");
            });

            modelBuilder.Entity<InspTransaction>(entity =>
            {
                entity.ToTable("INSP_Transaction");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__INSP_Tra__4B840D045AB88976")
                    .IsUnique();

                entity.Property(e => e.ApiBookingComments).HasColumnName("API_Booking_Comments");

                entity.Property(e => e.ApplicantEmail)
                    .HasColumnName("Applicant_Email")
                    .HasMaxLength(200);

                entity.Property(e => e.ApplicantName)
                    .HasColumnName("Applicant_Name")
                    .HasMaxLength(200);

                entity.Property(e => e.ApplicantPhoneNo)
                    .HasColumnName("Applicant_PhoneNo")
                    .HasMaxLength(200);

                entity.Property(e => e.CompassBookingNo).HasMaxLength(1500);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CusBookingComments).HasColumnName("Cus_Booking_Comments");

                entity.Property(e => e.CustomerBookingNo).HasMaxLength(2000);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.Ean)
                    .HasColumnName("EAN")
                    .HasMaxLength(500);

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.FactoryId).HasColumnName("Factory_Id");

                entity.Property(e => e.FbMissionId).HasColumnName("Fb_Mission_Id");

                entity.Property(e => e.FbMissionStatus).HasColumnName("Fb_Mission_Status");

                entity.Property(e => e.FirstServiceDateFrom)
                    .HasColumnName("FirstServiceDate_From")
                    .HasColumnType("datetime");

                entity.Property(e => e.FirstServiceDateTo)
                    .HasColumnName("FirstServiceDate_To")
                    .HasColumnType("datetime");

                entity.Property(e => e.FlexibleInspectionDate).HasColumnName("Flexible_Inspection_Date");

                entity.Property(e => e.Gapdacorrelation).HasColumnName("GAPDACorrelation");

                entity.Property(e => e.Gapdaemail)
                    .HasColumnName("GAPDAEmail")
                    .HasMaxLength(500);

                entity.Property(e => e.Gapdaname)
                    .HasColumnName("GAPDAName")
                    .HasMaxLength(500);

                entity.Property(e => e.InternalComments).HasColumnName("Internal_Comments");

                entity.Property(e => e.InternalReferencePo)
                    .HasColumnName("InternalReferencePO")
                    .HasMaxLength(1500);

                entity.Property(e => e.IsEaqf).HasColumnName("IsEAQF");

                entity.Property(e => e.IsPickingRequired).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsProcessing).HasDefaultValueSql("((0))");

                entity.Property(e => e.OfficeId).HasColumnName("Office_Id");

                entity.Property(e => e.QcbookingComments).HasColumnName("QCBookingComments");

                entity.Property(e => e.ScheduleComments).HasColumnName("Schedule_Comments");

                entity.Property(e => e.SeasonId).HasColumnName("Season_Id");

                entity.Property(e => e.SeasonYearId).HasColumnName("SeasonYear_Id");

                entity.Property(e => e.ServiceDateFrom)
                    .HasColumnName("ServiceDate_From")
                    .HasColumnType("datetime");

                entity.Property(e => e.ServiceDateTo)
                    .HasColumnName("ServiceDate_To")
                    .HasColumnType("datetime");

                entity.Property(e => e.ShipmentDate).HasColumnType("datetime");

                entity.Property(e => e.ShipmentPort).HasMaxLength(500);

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BookingTypeNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.BookingType)
                    .HasConstraintName("FK_INSP_BOOKING_TYPE");

                entity.HasOne(d => d.BusinessLineNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.BusinessLine)
                    .HasConstraintName("FK_INSP_Business_Line");

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.CollectionId)
                    .HasConstraintName("FK__INSP_Tran__Colle__66187EA7");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__INSP_Tran__Creat__72095440");

                entity.HasOne(d => d.CuProductCategoryNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.CuProductCategory)
                    .HasConstraintName("FK_INSP_CU_PRODUCT_CATEGORY");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Tran__Custo__69740E3F");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Tran__Entit__6A683278");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.InspTransactionFactories)
                    .HasForeignKey(d => d.FactoryId)
                    .HasConstraintName("FK__INSP_Tran__Facto__6B5C56B1");

                entity.HasOne(d => d.FbMissionStatusNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.FbMissionStatus)
                    .HasConstraintName("FK_INSP_Transaction_FB_Status");

                entity.HasOne(d => d.InspectionLocationNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.InspectionLocation)
                    .HasConstraintName("FK_INSP_INSPECTION_LOCATION");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK__INSP_Tran__Offic__6C507AEA");

                entity.HasOne(d => d.PaymentOptionsNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.PaymentOptions)
                    .HasConstraintName("FK_INSP_PAYMENT_OPTIONS");

                entity.HasOne(d => d.PriceCategory)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.PriceCategoryId)
                    .HasConstraintName("FK__INSP_Tran__Price__670CA2E0");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_INSP_TRAN_ProductCategoryId");

                entity.HasOne(d => d.ProductSubCategory2)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.ProductSubCategory2Id)
                    .HasConstraintName("FK_INSP_TRAN_ProductSubCategory2Id");

                entity.HasOne(d => d.ProductSubCategory)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.ProductSubCategoryId)
                    .HasConstraintName("FK_INSP_TRAN_ProductSubCategoryId");

                entity.HasOne(d => d.ReInspectionTypeNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.ReInspectionType)
                    .HasConstraintName("FK_INSP_TRANSACTION_RE_INSPECTION_TYPE");

                entity.HasOne(d => d.ReportRequestNavigation)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.ReportRequest)
                    .HasConstraintName("FK__INSP_Tran__Report_Request");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.SeasonId)
                    .HasConstraintName("FK_INSP_SEASON_CONFIG");

                entity.HasOne(d => d.SeasonYear)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.SeasonYearId)
                    .HasConstraintName("FK__INSP_Tran__Seaso__6F2CE795");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InspTransactions)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Tran__Statu__70210BCE");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.InspTransactionSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INSP_Tran__Suppl__71153007");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__INSP_Tran__Updat__72FD7879");
            });

            modelBuilder.Entity<InspTransactionDraft>(entity =>
            {
                entity.ToTable("INSP_Transaction_Draft");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceDateFrom).HasColumnType("datetime");

                entity.Property(e => e.ServiceDateTo).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.InspTransactionDrafts)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_INSP_Transaction_Draft_BrandId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InspTransactionDraftCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INSP_Transaction_Draft_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InspTransactionDrafts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_INSP_Transaction_Draft_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InspTransactionDraftDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INSP_Transaction_Draft_DeletedBy");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.InspTransactionDrafts)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_INSP_Transaction_Draft_DepartmentId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InspTransactionDrafts)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INSP_TRANSACTION_DRAFT_ENTITYId");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.InspTransactionDraftFactories)
                    .HasForeignKey(d => d.FactoryId)
                    .HasConstraintName("FK_INSP_Transaction_Draft_FactoryId");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InspTransactionDraftInspections)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_INSP_Transaction_Draft_InspectionId");

                entity.HasOne(d => d.PreviousBookingNoNavigation)
                    .WithMany(p => p.InspTransactionDraftPreviousBookingNoNavigations)
                    .HasForeignKey(d => d.PreviousBookingNo)
                    .HasConstraintName("FK_DRAFT_PREV_BOOKING_NO");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.InspTransactionDraftSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_INSP_Transaction_Draft_SupplierId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InspTransactionDraftUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INSP_Transaction_Draft_UpdatedBy");
            });

            modelBuilder.Entity<InvAutTranContactDetail>(entity =>
            {
                entity.ToTable("INV_AUT_TRAN_ContactDetails");

                entity.Property(e => e.CustomerContactId).HasColumnName("Customer_Contact_Id");

                entity.Property(e => e.FactoryContactId).HasColumnName("Factory_Contact_Id");

                entity.Property(e => e.InvoiceId).HasColumnName("Invoice_Id");

                entity.Property(e => e.SupplierContactId).HasColumnName("Supplier_Contact_Id");

                entity.HasOne(d => d.CustomerContact)
                    .WithMany(p => p.InvAutTranContactDetails)
                    .HasForeignKey(d => d.CustomerContactId)
                    .HasConstraintName("INV_AUT_TRAN_ContactDetails_Customer_Contact_Id");

                entity.HasOne(d => d.FactoryContact)
                    .WithMany(p => p.InvAutTranContactDetailFactoryContacts)
                    .HasForeignKey(d => d.FactoryContactId)
                    .HasConstraintName("INV_AUT_TRAN_ContactDetails_Factory_Contact_Id");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvAutTranContactDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("INV_AUT_TRAN_ContactDetails_Invoice_Id");

                entity.HasOne(d => d.SupplierContact)
                    .WithMany(p => p.InvAutTranContactDetailSupplierContacts)
                    .HasForeignKey(d => d.SupplierContactId)
                    .HasConstraintName("INV_AUT_TRAN_ContactDetails_Supplier_Contact_Id");
            });

            modelBuilder.Entity<InvAutTranDetail>(entity =>
            {
                entity.ToTable("INV_AUT_TRAN_Details");

                entity.Property(e => e.AdditionalBdTax).HasColumnName("Additional_BD_Tax");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo).HasMaxLength(1000);

                entity.Property(e => e.InvoicePaymentDate).HasColumnType("datetime");

                entity.Property(e => e.InvoicedAddress).HasMaxLength(2000);

                entity.Property(e => e.InvoicedName).HasMaxLength(1000);

                entity.Property(e => e.PaymentDuration).HasMaxLength(1000);

                entity.Property(e => e.PaymentTerms).HasMaxLength(2000);

                entity.Property(e => e.PostedDate).HasColumnType("datetime");

                entity.Property(e => e.ProrateBookingNumbers).HasMaxLength(1000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("INV_AUT_TRAN_Details_AuditId");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("INV_AUT_TRAN_Details_BankId");

                entity.HasOne(d => d.CalculateDiscountFeeNavigation)
                    .WithMany(p => p.InvAutTranDetailCalculateDiscountFeeNavigations)
                    .HasForeignKey(d => d.CalculateDiscountFee)
                    .HasConstraintName("INV_AUT_TRAN_Details_CalculateDiscountFee");

                entity.HasOne(d => d.CalculateHotelFeeNavigation)
                    .WithMany(p => p.InvAutTranDetailCalculateHotelFeeNavigations)
                    .HasForeignKey(d => d.CalculateHotelFee)
                    .HasConstraintName("INV_AUT_TRAN_Details_CalculateHotelFee");

                entity.HasOne(d => d.CalculateInspectionFeeNavigation)
                    .WithMany(p => p.InvAutTranDetailCalculateInspectionFeeNavigations)
                    .HasForeignKey(d => d.CalculateInspectionFee)
                    .HasConstraintName("INV_AUT_TRAN_Details_CalculateInspectionFee");

                entity.HasOne(d => d.CalculateOtherFeeNavigation)
                    .WithMany(p => p.InvAutTranDetailCalculateOtherFeeNavigations)
                    .HasForeignKey(d => d.CalculateOtherFee)
                    .HasConstraintName("INV_AUT_TRAN_Details_CalculateOtherFee");

                entity.HasOne(d => d.CalculateTravelExpenseNavigation)
                    .WithMany(p => p.InvAutTranDetailCalculateTravelExpenseNavigations)
                    .HasForeignKey(d => d.CalculateTravelExpense)
                    .HasConstraintName("INV_AUT_TRAN_Details_CalculateTravelExpense");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_AUT_TRAN_Details_CreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_AUT_TRAN_Details_EntityId");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("INV_AUT_TRAN_Details_InspectionId");

                entity.HasOne(d => d.InvoiceCurrencyNavigation)
                    .WithMany(p => p.InvAutTranDetailInvoiceCurrencyNavigations)
                    .HasForeignKey(d => d.InvoiceCurrency)
                    .HasConstraintName("INV_AUT_TRAN_Details_InvoiceCurrency");

                entity.HasOne(d => d.InvoiceMethodNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.InvoiceMethod)
                    .HasConstraintName("INV_AUT_TRAN_Details_InvoiceMethod");

                entity.HasOne(d => d.InvoicePaymentStatusNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.InvoicePaymentStatus)
                    .HasConstraintName("INV_AUT_TRAN_Details_InvoicePaymentStatus");

                entity.HasOne(d => d.InvoiceStatusNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.InvoiceStatus)
                    .HasConstraintName("INV_AUT_TRAN_Details_InvoiceStatus");

                entity.HasOne(d => d.InvoiceToNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.InvoiceTo)
                    .HasConstraintName("INV_AUT_TRAN_Details_InvoiceTo");

                entity.HasOne(d => d.InvoiceTypeNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.InvoiceType)
                    .HasConstraintName("FK_InvoiceType");

                entity.HasOne(d => d.OfficeNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.Office)
                    .HasConstraintName("INV_AUT_TRAN_Details_Office");

                entity.HasOne(d => d.PriceCalculationTypeNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.PriceCalculationType)
                    .HasConstraintName("INV_AUT_TRAN_Details_PriceCalculationType");

                entity.HasOne(d => d.PriceCardCurrencyNavigation)
                    .WithMany(p => p.InvAutTranDetailPriceCardCurrencyNavigations)
                    .HasForeignKey(d => d.PriceCardCurrency)
                    .HasConstraintName("INV_AUT_TRAN_Details_PriceCardCurrency");

                entity.HasOne(d => d.Rule)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.RuleId)
                    .HasConstraintName("INV_AUT_TRAN_Details_RuleId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("INV_AUT_TRAN_Details_ServiceId");

                entity.HasOne(d => d.TravelMatrixTypeNavigation)
                    .WithMany(p => p.InvAutTranDetails)
                    .HasForeignKey(d => d.TravelMatrixType)
                    .HasConstraintName("INV_AUT_TRAN_Details_TravelMatrixType");
            });

            modelBuilder.Entity<InvAutTranStatusLog>(entity =>
            {
                entity.ToTable("INV_AUT_TRAN_Status_Log");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.InvoiceId).HasColumnName("Invoice_Id");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.InvAutTranStatusLogs)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("INV_AUT_TRAN_Status_Log_Audit_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvAutTranStatusLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_AUT_TRAN_Status_Log_CreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvAutTranStatusLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_AUT_TRAN_Status_Log_EntityId");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InvAutTranStatusLogs)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("INV_AUT_TRAN_Status_Log_Inspection_Id");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvAutTranStatusLogs)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("INV_AUT_TRAN_Status_Log_Invoice_Id");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InvAutTranStatusLogs)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("INV_AUT_TRAN_Status_Log_Status_Id");
            });

            modelBuilder.Entity<InvAutTranTax>(entity =>
            {
                entity.ToTable("INV_AUT_TRAN_Tax");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.InvoiceId).HasColumnName("Invoice_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvAutTranTaxes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_AUT_TRAN_Tax_CreatedBy");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvAutTranTaxes)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("INV_AUT_TRAN_Tax_Invoice_Id");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.InvAutTranTaxes)
                    .HasForeignKey(d => d.TaxId)
                    .HasConstraintName("INV_AUT_TRAN_Tax_TaxId");
            });

            modelBuilder.Entity<InvCreRefCreditType>(entity =>
            {
                entity.ToTable("INV_CRE_REF_CreditType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvCreTranClaimDetail>(entity =>
            {
                entity.ToTable("INV_CRE_TRAN_ClaimDetails");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.RefundAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.Property(e => e.SortAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.InvCreTranClaimDetails)
                    .HasForeignKey(d => d.ClaimId)
                    .HasConstraintName("INV_CRE_TRAN_ClaimDetails_ClaimId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvCreTranClaimDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_CRE_TRAN_ClaimDetails_CreatedBy");

                entity.HasOne(d => d.Credit)
                    .WithMany(p => p.InvCreTranClaimDetails)
                    .HasForeignKey(d => d.CreditId)
                    .HasConstraintName("INV_CRE_TRAN_ClaimDetails_CreditId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvCreTranClaimDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_CRE_TRAN_ClaimDetails_DeletedBy");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InvCreTranClaimDetails)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("INV_CRE_TRAN_ClaimDetails_InspectionId");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvCreTranClaimDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("INV_CRE_TRAN_ClaimDetails_InvoiceId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvCreTranClaimDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_CRE_TRAN_ClaimDetails_UpdatedBy");
            });

            modelBuilder.Entity<InvCreTranContact>(entity =>
            {
                entity.ToTable("INV_CRE_TRAN_Contacts");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvCreTranContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_CRE_TRAN_Contacts_CreatedBy");

                entity.HasOne(d => d.CreditedNavigation)
                    .WithMany(p => p.InvCreTranContacts)
                    .HasForeignKey(d => d.Credited)
                    .HasConstraintName("INV_CRE_TRAN_Contacts_Credited");

                entity.HasOne(d => d.CustomerContact)
                    .WithMany(p => p.InvCreTranContacts)
                    .HasForeignKey(d => d.CustomerContactId)
                    .HasConstraintName("INV_CRE_TRAN_Contacts_CustomerContactId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvCreTranContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_CRE_TRAN_Contacts_DeletedBy");
            });

            modelBuilder.Entity<InvCreTransaction>(entity =>
            {
                entity.ToTable("INV_CRE_Transaction");

                entity.Property(e => e.BillTo).HasMaxLength(1000);

                entity.Property(e => e.BilledAddress).HasMaxLength(2000);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreditDate).HasColumnType("datetime");

                entity.Property(e => e.CreditNo).HasMaxLength(1000);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.InvoiceAdrress).HasMaxLength(2000);

                entity.Property(e => e.PaymentTerms).HasMaxLength(1000);

                entity.Property(e => e.PostDate).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.InvCreTransactions)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("INV_CRE_Transaction_BankId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvCreTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_CRE_Transaction_CreatedBy");

                entity.HasOne(d => d.CreditType)
                    .WithMany(p => p.InvCreTransactions)
                    .HasForeignKey(d => d.CreditTypeId)
                    .HasConstraintName("INV_CRE_Transaction_CreditTypeId");

                entity.HasOne(d => d.CurrencyNavigation)
                    .WithMany(p => p.InvCreTransactions)
                    .HasForeignKey(d => d.Currency)
                    .HasConstraintName("INV_CRE_Transaction_Currency");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvCreTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_CRE_Transaction_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvCreTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_CRE_Transaction_EntityId");

                entity.HasOne(d => d.OfficeNavigation)
                    .WithMany(p => p.InvCreTransactions)
                    .HasForeignKey(d => d.Office)
                    .HasConstraintName("INV_CRE_Transaction_Office");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvCreTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_CRE_Transaction_UpdatedBy");
            });

            modelBuilder.Entity<InvDaCustomer>(entity =>
            {
                entity.ToTable("INV_DA_Customer");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvDaCustomerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_Customer_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InvDaCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_Customer_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvDaCustomerDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INV_DA_Customer_DeletedBy");

                entity.HasOne(d => d.InvDa)
                    .WithMany(p => p.InvDaCustomers)
                    .HasForeignKey(d => d.InvDaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_Customer_InvDaId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvDaCustomerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INV_DA_Customer_UpdatedBy");
            });

            modelBuilder.Entity<InvDaInvoiceType>(entity =>
            {
                entity.ToTable("INV_DA_InvoiceType");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvDaInvoiceTypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_InvoiceType_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvDaInvoiceTypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INV_DA_InvoiceType_DeletedBy");

                entity.HasOne(d => d.InvDa)
                    .WithMany(p => p.InvDaInvoiceTypes)
                    .HasForeignKey(d => d.InvDaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_InvoiceType_InvDaId");

                entity.HasOne(d => d.InvoiceType)
                    .WithMany(p => p.InvDaInvoiceTypes)
                    .HasForeignKey(d => d.InvoiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_InvoiceType_InvoiceTypeId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvDaInvoiceTypeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INV_DA_InvoiceType_UpdatedBy");
            });

            modelBuilder.Entity<InvDaOffice>(entity =>
            {
                entity.ToTable("INV_DA_office");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvDaOfficeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_office_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvDaOfficeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INV_DA_office_DeletedBy");

                entity.HasOne(d => d.InvDa)
                    .WithMany(p => p.InvDaOffices)
                    .HasForeignKey(d => d.InvDaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_office_InvDaId");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.InvDaOffices)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_office_OfficeId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvDaOfficeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INV_DA_office_UpdatedBy");
            });

            modelBuilder.Entity<InvDaTransaction>(entity =>
            {
                entity.ToTable("INV_DA_Transaction");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvDaTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_Transaction_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvDaTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INV_DA_Transactions_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvDaTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_DA_Transaction_EntityId");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.InvDaTransactions)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_DA_Transaction_StaffId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvDaTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INV_DA_Transaction_UpdatedBy");
            });

            modelBuilder.Entity<InvDisRefType>(entity =>
            {
                entity.ToTable("INV_DIS_REF_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvDisTranCountry>(entity =>
            {
                entity.ToTable("INV_DIS_TRAN_Country");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.InvDisTranCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_DIS_TRAN_Country_CountryId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvDisTranCountryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_DIS_TRAN_Country_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvDisTranCountryDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_DIS_TRAN_Country_DeletedBy");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.InvDisTranCountries)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_DIS_TRAN_Country_DiscountId");
            });

            modelBuilder.Entity<InvDisTranDetail>(entity =>
            {
                entity.ToTable("INV_DIS_TRAN_Details");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.PeriodFrom).HasColumnType("date");

                entity.Property(e => e.PeriodTo).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvDisTranDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_DIS_TRAN_Details_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InvDisTranDetails)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_DIS_TRAN_Details_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvDisTranDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_DIS_TRAN_Details_DeletedBy");

                entity.HasOne(d => d.DiscountTypeNavigation)
                    .WithMany(p => p.InvDisTranDetails)
                    .HasForeignKey(d => d.DiscountType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_DIS_TRAN_Details_DiscountType");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvDisTranDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_DIS_TRAN_Details_EntityId");
                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvDisTranDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_DIS_TRAN_Details_UpdatedBy");
            });

            modelBuilder.Entity<InvDisTranPeriodInfo>(entity =>
            {
                entity.ToTable("INV_DIS_TRAN_PeriodInfo");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.LimitFrom).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.LimitTo).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvDisTranPeriodInfoCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_DIS_TRAN_PeriodInfo_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvDisTranPeriodInfoDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_DIS_TRAN_PeriodInfo_DeletedBy");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.InvDisTranPeriodInfos)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_DIS_TRAN_PeriodInfo_DiscountId");
            });

            modelBuilder.Entity<InvExfContactDetail>(entity =>
            {
                entity.ToTable("INV_EXF_ContactDetails");

                entity.HasOne(d => d.CustomerContact)
                    .WithMany(p => p.InvExfContactDetails)
                    .HasForeignKey(d => d.CustomerContactId)
                    .HasConstraintName("INV_EXF_ContactDetails_CustomerContactId");

                entity.HasOne(d => d.ExtraFee)
                    .WithMany(p => p.InvExfContactDetails)
                    .HasForeignKey(d => d.ExtraFeeId)
                    .HasConstraintName("INV_EXF_ContactDetails_ExtraFeeId");

                entity.HasOne(d => d.FactoryContact)
                    .WithMany(p => p.InvExfContactDetailFactoryContacts)
                    .HasForeignKey(d => d.FactoryContactId)
                    .HasConstraintName("INV_EXF_ContactDetails_FactContactId");

                entity.HasOne(d => d.SupplierContact)
                    .WithMany(p => p.InvExfContactDetailSupplierContacts)
                    .HasForeignKey(d => d.SupplierContactId)
                    .HasConstraintName("INV_EXF_ContactDetails_SupContactId");
            });

            modelBuilder.Entity<InvExfStatus>(entity =>
            {
                entity.ToTable("INV_EXF_Status");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<InvExfTranDetail>(entity =>
            {
                entity.ToTable("INV_EXF_TRAN_Details");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ExftransactionId).HasColumnName("EXFTransactionId");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvExfTranDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_EXF_TRAN_Details_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvExfTranDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_EXF_TRAN_Details_DeletedBy");

                entity.HasOne(d => d.Exftransaction)
                    .WithMany(p => p.InvExfTranDetails)
                    .HasForeignKey(d => d.ExftransactionId)
                    .HasConstraintName("INV_EXF_TRAN_Details_EXFTransactionId");

                entity.HasOne(d => d.ExtraFeeTypeNavigation)
                    .WithMany(p => p.InvExfTranDetails)
                    .HasForeignKey(d => d.ExtraFeeType)
                    .HasConstraintName("INV_EXF_TRAN_Details_ExtraFeeType");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvExfTranDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_EXF_TRAN_Details_UpdatedBy");
            });

            modelBuilder.Entity<InvExfTranStatusLog>(entity =>
            {
                entity.ToTable("INV_EXF_TRAN_Status_Log");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExtraFeeId).HasColumnName("ExtraFee_Id");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.InvExfTranStatusLogs)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("INV_EXF_TRAN_Status_Log_Audit_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvExfTranStatusLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_EXF_TRAN_Status_Log_CreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvExfTranStatusLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_EXF_TRAN_Status_Log_EntityId");

                entity.HasOne(d => d.ExtraFee)
                    .WithMany(p => p.InvExfTranStatusLogs)
                    .HasForeignKey(d => d.ExtraFeeId)
                    .HasConstraintName("INV_EXF_TRAN_Status_Log_ExtraFee_Id");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InvExfTranStatusLogs)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("INV_EXF_TRAN_Status_Log_Inspection_Id");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InvExfTranStatusLogs)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("INV_EXF_TRAN_Status_Log_Status_Id");
            });

            modelBuilder.Entity<InvExfTransaction>(entity =>
            {
                entity.ToTable("INV_EXF_Transaction");

                entity.Property(e => e.BilledAddress).HasMaxLength(1000);

                entity.Property(e => e.BilledName).HasMaxLength(500);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ExtraFeeInvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentTerms).HasMaxLength(500);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("INV_EXF_Transaction_AuditId");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("INV_EXF_Transaction_BankId");

                entity.HasOne(d => d.BilledToNavigation)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.BilledTo)
                    .HasConstraintName("INV_EXF_Transaction_BilledTo");

                entity.HasOne(d => d.BillingEntity)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.BillingEntityId)
                    .HasConstraintName("INV_EXF_Transaction_BillingEntityId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvExfTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_EXF_Transaction_CreatedBy");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.InvExfTransactionCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("INV_EXF_Transaction_CurrencyId");

                entity.HasOne(d => d.InvoiceCurrency)
            .WithMany(p => p.InvExfTransactionInvoiceCurrencies)
            .HasForeignKey(d => d.InvoiceCurrencyId)
            .HasConstraintName("FK_INV_EXF_Transaction_InvoiceCurrencyId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("INV_EXF_Transaction_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvExfTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_EXF_Transaction_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_EXF_Transaction_EntityId");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.InvExfTransactionFactories)
                    .HasForeignKey(d => d.FactoryId)
                    .HasConstraintName("INV_EXF_Transaction_FactoryId");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("INV_EXF_Transaction_InspectionId");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("INV_EXF_Transaction_InvoiceId");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("INV_EXF_Transaction_OfficeId");

                entity.HasOne(d => d.PaymentStatusNavigation)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.PaymentStatus)
                    .HasConstraintName("INV_EXF_Transaction_PaymentStatus");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("INV_EXF_Transaction_ServiceId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InvExfTransactions)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("INV_EXF_Transaction_StatusId");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.InvExfTransactionSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("INV_EXF_Transaction_SupplierId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvExfTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_EXF_Transaction_UpdatedBy");
            });

            modelBuilder.Entity<InvExfType>(entity =>
            {
                entity.ToTable("INV_EXF_Type");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<InvExtTranTax>(entity =>
            {
                entity.ToTable("INV_EXT_TRAN_Tax");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExtraFeeId).HasColumnName("ExtraFee_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvExtTranTaxes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_EXT_TRAN_Tax_CreatedBy");

                entity.HasOne(d => d.ExtraFee)
                    .WithMany(p => p.InvExtTranTaxes)
                    .HasForeignKey(d => d.ExtraFeeId)
                    .HasConstraintName("INV_EXT_TRAN_Tax_ExtraFee_Id");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.InvExtTranTaxes)
                    .HasForeignKey(d => d.TaxId)
                    .HasConstraintName("INV_EXT_TRAN_Tax_TaxId");
            });

            modelBuilder.Entity<InvManTranDetail>(entity =>
            {
                entity.ToTable("INV_MAN_TRAN_Details");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.InvManualId).HasColumnName("Inv_ManualId");

                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvManTranDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_MAN_TRAN_Details_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvManTranDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_MAN_TRAN_Details_DeletedBy");

                entity.HasOne(d => d.InvManual)
                    .WithMany(p => p.InvManTranDetails)
                    .HasForeignKey(d => d.InvManualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_MAN_TRAN_Details_Inv_ManualId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvManTranDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_MAN_TRAN_Details_UpdatedBy");
            });

            modelBuilder.Entity<InvManTranTax>(entity =>
            {
                entity.ToTable("INV_MAN_TRAN_TAX");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ManInvoiceId).HasColumnName("Man_InvoiceId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvManTranTaxes)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INV_MAN_TRAN_CreatedBy");

                entity.HasOne(d => d.ManInvoice)
                    .WithMany(p => p.InvManTranTaxes)
                    .HasForeignKey(d => d.ManInvoiceId)
                    .HasConstraintName("FK_INV_MAN_TRAN_TAX_Man_Invoiceid");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.InvManTranTaxes)
                    .HasForeignKey(d => d.TaxId)
                    .HasConstraintName("FK_INV_MAN_TRAN_TAX_TaxId");
            });

            modelBuilder.Entity<InvManTransaction>(entity =>
            {
                entity.ToTable("INV_MAN_Transaction");

                entity.Property(e => e.Attn).HasMaxLength(1000);

                entity.Property(e => e.BilledAddress).HasMaxLength(2000);

                entity.Property(e => e.BilledName).HasMaxLength(500);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(2000);

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.InvoiceDate).HasColumnType("date");

                entity.Property(e => e.InvoiceNo).HasMaxLength(100);

                entity.Property(e => e.IsEaqf).HasColumnName("IsEAQF");

                entity.Property(e => e.PaymentRef).HasMaxLength(200);

                entity.Property(e => e.PaymentTerms).HasMaxLength(1000);

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("FK_INV_MAN_Transaction_AuditId");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_MAN_Transaction_BankId");

                entity.HasOne(d => d.BookingNoNavigation)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.BookingNo)
                    .HasConstraintName("FK_INV_MAN_Transaction_BookingNo");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_INV_MAN_Transaction_Country");
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvManTransactionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_MAN_Transaction_CreatedBy");

                entity.HasOne(d => d.CurrencyNavigation)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.Currency)
                    .HasConstraintName("INV_MAN_Transaction_Currency");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_MAN_Transaction_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvManTransactionDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_MAN_Transaction_Deletedby");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_MAN_Transaction_EntityId");
                entity.HasOne(d => d.InvoiceTo)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.InvoiceToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_MAN_Transaction_InvoiceToId");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_MAN_Transaction_OfficeId");

                entity.HasOne(d => d.PaymentModeNavigation)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.PaymentMode)
                    .HasConstraintName("FK_INV_MAN_Transaction_PaymentMode");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("INV_MAN_Transaction_ServiceId");

                entity.HasOne(d => d.ServiceTypeNavigation)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.ServiceType)
                    .HasConstraintName("INV_MAN_Transaction_ServiceType");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_MAN_Transaction_Status");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.InvManTransactions)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("INV_MAN_Transaction_SupplierId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvManTransactionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_MAN_Transaction_UpdatedBy");
            });

            modelBuilder.Entity<InvPaymentStatus>(entity =>
            {
                entity.ToTable("INV_Payment_Status");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<InvRefBank>(entity =>
            {
                entity.ToTable("INV_REF_Bank");

                entity.Property(e => e.AccountName).HasMaxLength(500);

                entity.Property(e => e.AccountNumber).HasMaxLength(500);

                entity.Property(e => e.BankAddress).HasMaxLength(1000);

                entity.Property(e => e.BankName).HasMaxLength(500);

                entity.Property(e => e.ChopFileUniqueId).HasMaxLength(255);

                entity.Property(e => e.ChopFilename).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.Property(e => e.SignatureFileUniqueId).HasMaxLength(255);

                entity.Property(e => e.SignatureFilename).HasMaxLength(255);

                entity.Property(e => e.SwiftCode).HasMaxLength(500);

                entity.Property(e => e.TaxNameInXero).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AccountCurrencyNavigation)
                    .WithMany(p => p.InvRefBanks)
                    .HasForeignKey(d => d.AccountCurrency)
                    .HasConstraintName("FK_INV_Bank_AccountCurrency");

                entity.HasOne(d => d.BillingEntityNavigation)
                    .WithMany(p => p.InvRefBanks)
                    .HasForeignKey(d => d.BillingEntity)
                    .HasConstraintName("FK_INV_Bank_BillingEntity");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvRefBankCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INV_Bank_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvRefBankDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INV_Bank_DeletedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvRefBankUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INV_Bank_UpdatedBy");
            });

            modelBuilder.Entity<InvRefBillingFreequency>(entity =>
            {
                entity.ToTable("INV_REF_BillingFreequency");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<InvRefFeesFrom>(entity =>
            {
                entity.ToTable("INV_REF_Fees_From");

                entity.Property(e => e.Name).HasMaxLength(1000);
            });

            modelBuilder.Entity<InvRefFileType>(entity =>
            {
                entity.ToTable("INV_REF_FileType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<InvRefInterventionType>(entity =>
            {
                entity.ToTable("INV_REF_InterventionType");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<InvRefOffice>(entity =>
            {
                entity.ToTable("INV_REF_Office");

                entity.Property(e => e.Fax).HasMaxLength(100);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.Website).HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvRefOffices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("INV_REF_Office_EntityId");
            });

            modelBuilder.Entity<InvRefPaymentMode>(entity =>
            {
                entity.ToTable("INV_REF_PaymentMode");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<InvRefPaymentTerm>(entity =>
            {
                entity.ToTable("INV_REF_PaymentTerms");

                entity.Property(e => e.Name).HasMaxLength(1000);
            });

            modelBuilder.Entity<InvRefPriceCalculationType>(entity =>
            {
                entity.ToTable("INV_REF_PriceCalculationType");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvRefRequestType>(entity =>
            {
                entity.ToTable("INV_REF_Request_Type");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<InvStatus>(entity =>
            {
                entity.ToTable("INV_Status");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<InvTmDetail>(entity =>
            {
                entity.ToTable("INV_TM_Details");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DistanceKm).HasColumnName("DistanceKM");

                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.InvTmDetailCities)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("INV_TM_Details_CityId");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.InvTmDetails)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_TM_Details_CountryId");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.InvTmDetailCounties)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("INV_TM_Details_CountyId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvTmDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_TM_Details_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.InvTmDetails)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("INV_TM_Details_CustomerId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvTmDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_TM_Details_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvTmDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_INV_TM_Details_EntityId");

                entity.HasOne(d => d.InspPortCity)
                    .WithMany(p => p.InvTmDetailInspPortCities)
                    .HasForeignKey(d => d.InspPortCityId)
                    .HasConstraintName("FK_TRAVEL_MATRIX_PORT_CITY_ID");

                entity.HasOne(d => d.InspPortCounty)
                    .WithMany(p => p.InvTmDetailInspPortCounties)
                    .HasForeignKey(d => d.InspPortCountyId)
                    .HasConstraintName("INV_TM_Details_InspPortCountyId");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.InvTmDetails)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_TM_Details_ProvinceId");

                entity.HasOne(d => d.SourceCurrency)
                    .WithMany(p => p.InvTmDetailSourceCurrencies)
                    .HasForeignKey(d => d.SourceCurrencyId)
                    .HasConstraintName("INV_TM_Details_SourceCurrencyId");

                entity.HasOne(d => d.TravelCurrency)
                    .WithMany(p => p.InvTmDetailTravelCurrencies)
                    .HasForeignKey(d => d.TravelCurrencyId)
                    .HasConstraintName("INV_TM_Details_TravelCurrencyId");

                entity.HasOne(d => d.TravelMatrixType)
                    .WithMany(p => p.InvTmDetails)
                    .HasForeignKey(d => d.TravelMatrixTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INV_TM_Details_TravelMatrixTypeId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvTmDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_TM_Details_UpdatedBy");
            });

            modelBuilder.Entity<InvTmType>(entity =>
            {
                entity.ToTable("INV_TM_Type");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<InvTranBankTax>(entity =>
            {
                entity.ToTable("INV_TRAN_Bank_Tax");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FromDate)
                    .HasColumnName("From_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaxName)
                    .IsRequired()
                    .HasColumnName("Tax_name")
                    .HasMaxLength(500);

                entity.Property(e => e.TaxValue)
                    .HasColumnName("Tax_Value")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ToDate)
                    .HasColumnName("To_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.InvTranBankTaxes)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INV_Bank_Tax_BankId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvTranBankTaxCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INV_Bank_Tax_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvTranBankTaxDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INV_Bank_Tax_DeletedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvTranBankTaxUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_INV_Bank_Tax_UpdatedBy");
            });

            modelBuilder.Entity<InvTranFile>(entity =>
            {
                entity.ToTable("INV_TRAN_Files");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(1000);

                entity.Property(e => e.FilePath).HasMaxLength(1000);

                entity.Property(e => e.InvoiceNo).HasMaxLength(1000);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvTranFileCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_INV_TRAN_Files_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvTranFileDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_INV_TRAN_Files_DeletedBy");

                entity.HasOne(d => d.FileTypeNavigation)
                    .WithMany(p => p.InvTranFiles)
                    .HasForeignKey(d => d.FileType)
                    .HasConstraintName("FK_INV_TRAN_Files_FileType");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvTranFiles)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_INV_TRAN_Files_InvoiceId");
            });

            modelBuilder.Entity<InvTranInvoiceRequest>(entity =>
            {
                entity.ToTable("INV_TRAN_Invoice_Request");

                entity.Property(e => e.BilledName).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.InvTranInvoiceRequests)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_BrandId");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.InvTranInvoiceRequests)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_BuyerId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvTranInvoiceRequestCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_TRAN_Invoice_Request_CreatedBy");

                entity.HasOne(d => d.CuPriceCard)
                    .WithMany(p => p.InvTranInvoiceRequests)
                    .HasForeignKey(d => d.CuPriceCardId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_CuPriceCardId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvTranInvoiceRequestDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_TRAN_Invoice_Request_DeletedBy");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.InvTranInvoiceRequests)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_DepartmentId");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.InvTranInvoiceRequests)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_ProductCategoryId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvTranInvoiceRequestUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_TRAN_Invoice_Request_UpdatedBy");
            });

            modelBuilder.Entity<InvTranInvoiceRequestContact>(entity =>
            {
                entity.ToTable("INV_TRAN_Invoice_Request_Contact");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.InvTranInvoiceRequestContacts)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_Contact_ContactId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InvTranInvoiceRequestContactCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("INV_TRAN_Invoice_Request_Contact_CreatedBy");

                entity.HasOne(d => d.CuPriceCard)
                    .WithMany(p => p.InvTranInvoiceRequestContacts)
                    .HasForeignKey(d => d.CuPriceCardId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_Contact_CuPriceCardId");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InvTranInvoiceRequestContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("INV_TRAN_Invoice_Request_Contact_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.InvTranInvoiceRequestContacts)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_Contact_EntityId");

                entity.HasOne(d => d.InvoiceRequest)
                    .WithMany(p => p.InvTranInvoiceRequestContacts)
                    .HasForeignKey(d => d.InvoiceRequestId)
                    .HasConstraintName("INV_TRAN_Invoice_Request_Contact_InvoiceRequestId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InvTranInvoiceRequestContactUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("INV_TRAN_Invoice_Request_Contact_UpdatedBy");
            });

            modelBuilder.Entity<ItLoginLog>(entity =>
            {
                entity.ToTable("IT_login_Log");

                entity.Property(e => e.BrowserType).HasMaxLength(1000);

                entity.Property(e => e.DeviceType).HasMaxLength(1000);

                entity.Property(e => e.IpAddress).HasMaxLength(1000);

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.LogInTime).HasColumnType("datetime");

                entity.Property(e => e.LogOutTime).HasColumnType("datetime");

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.HasOne(d => d.UserIt)
                    .WithMany(p => p.ItLoginLogs)
                    .HasForeignKey(d => d.UserItId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_login___UserI__77ED161F");
            });

            modelBuilder.Entity<ItRight>(entity =>
            {
                entity.ToTable("IT_Right");

                entity.Property(e => e.Glyphicons)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MenuNameIdTran).HasColumnName("MenuName_IdTran");

                entity.Property(e => e.Path)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ShowMenu)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TitleName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TitleNameIdTran).HasColumnName("TitleName_IdTran");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ItRights)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__IT_Right__Entity__2C15FA44");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__IT_Right__Parent__2B21D60B");

                entity.HasOne(d => d.RightTypeNavigation)
                    .WithMany(p => p.ItRights)
                    .HasForeignKey(d => d.RightType)
                    .HasConstraintName("FK__IT_Right__RightT__13BF4DC6");
            });

            modelBuilder.Entity<ItRightEntity>(entity =>
            {
                entity.ToTable("IT_Right_Entity");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ItRightEntities)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_IT_Right_Entity_CU_Customer");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ItRightEntities)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_IT_Right_Entity_AP_Entity");

                entity.HasOne(d => d.Right)
                    .WithMany(p => p.ItRightEntities)
                    .HasForeignKey(d => d.RightId)
                    .HasConstraintName("FK_IT_Right_Entity_IT_Right");
            });

            modelBuilder.Entity<ItRightMap>(entity =>
            {
                entity.ToTable("IT_Right_Map");

                entity.HasOne(d => d.Right)
                    .WithMany(p => p.ItRightMaps)
                    .HasForeignKey(d => d.RightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_Right___Right__5E2E0F73");

                entity.HasOne(d => d.RightType)
                    .WithMany(p => p.ItRightMaps)
                    .HasForeignKey(d => d.RightTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_Right___Right__5F2233AC");
            });

            modelBuilder.Entity<ItRightType>(entity =>
            {
                entity.ToTable("IT_Right_Type");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.ServiceNavigation)
                    .WithMany(p => p.ItRightTypes)
                    .HasForeignKey(d => d.Service)
                    .HasConstraintName("FK__IT_Right___Servi__601657E5");
            });

            modelBuilder.Entity<ItRole>(entity =>
            {
                entity.ToTable("IT_Role");

                entity.Property(e => e.PrimaryRole)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ItRoles)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__IT_Role__EntityI__2D0A1E7D");
            });

            modelBuilder.Entity<ItRoleRight>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.RightId })
                    .HasName("PK__IT_Role___FE9F269FEB49D108");

                entity.ToTable("IT_Role_Right");

                entity.HasOne(d => d.Right)
                    .WithMany(p => p.ItRoleRights)
                    .HasForeignKey(d => d.RightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_Role_R__Right__2EF266EF");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ItRoleRights)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_Role_R__RoleI__2DFE42B6");
            });

            modelBuilder.Entity<ItUserCuBrand>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BrandId })
                    .HasName("PK__IT_User___CAC6A364D9725CD8");

                entity.ToTable("IT_User_CU_Brand");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.BrandId).HasColumnName("Brand_Id");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ItUserCuBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_User_C__Brand__30DAAF61");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ItUserCuBrands)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_User_C__User___2FE68B28");
            });

            modelBuilder.Entity<ItUserCuDepartment>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.DepartmentId })
                    .HasName("PK__IT_User___213CF62F3C9F146F");

                entity.ToTable("IT_User_CU_Department");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.ItUserCuDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_User_C__Depar__32C2F7D3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ItUserCuDepartments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_User_C__User___31CED39A");
            });

            modelBuilder.Entity<ItUserMaster>(entity =>
            {
                entity.ToTable("IT_UserMaster");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.FbUserId).HasColumnName("Fb_User_Id");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.FullName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasColumnName("Login_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TcfuserId).HasColumnName("TCFUserID");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InverseCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__IT_UserMa__Creat__5681355C");

                entity.HasOne(d => d.CustomerContact)
                    .WithMany(p => p.ItUserMasters)
                    .HasForeignKey(d => d.CustomerContactId)
                    .HasConstraintName("FK__IT_UserMa__Custo__53A4C8B1");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ItUserMasters)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__IT_UserMa__Custo__359F647E");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InverseDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__IT_UserMa__Delet__57755995");

                entity.HasOne(d => d.FactoryContact)
                    .WithMany(p => p.ItUserMasterFactoryContacts)
                    .HasForeignKey(d => d.FactoryContactId)
                    .HasConstraintName("FK__IT_UserMa__Facto__558D1123");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.ItUserMasterFactories)
                    .HasForeignKey(d => d.FactoryId)
                    .HasConstraintName("FK__IT_UserMa__Facto__3787ACF0");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.ItUserMasters)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__IT_UserMa__Staff__34AB4045");

                entity.HasOne(d => d.SupplierContact)
                    .WithMany(p => p.ItUserMasterSupplierContacts)
                    .HasForeignKey(d => d.SupplierContactId)
                    .HasConstraintName("FK__IT_UserMa__Suppl__5498ECEA");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ItUserMasterSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__IT_UserMa__Suppl__369388B7");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InverseUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__IT_UserMa__Updat__58697DCE");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.ItUserMasters)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_UserMa__UserT__387BD129");
            });

            modelBuilder.Entity<ItUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId, e.EntityId })
                    .HasName("pk_user_role_entity");

                entity.ToTable("IT_UserRole");

                entity.Property(e => e.EntityId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.ItUserRoles)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IT_UserRole_EntityId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ItUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_UserRo__RoleI__3A64199B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ItUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IT_UserRo__UserI__396FF562");
            });

            modelBuilder.Entity<ItUserType>(entity =>
            {
                entity.ToTable("IT_UserType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<JobScheduleConfiguration>(entity =>
            {
                entity.ToTable("JOB_Schedule_Configuration");

                entity.Property(e => e.Cc)
                    .HasColumnName("CC")
                    .HasMaxLength(1500);

                entity.Property(e => e.CustomerId).HasMaxLength(1500);

                entity.Property(e => e.FileName).HasMaxLength(1500);

                entity.Property(e => e.FolderPath).HasMaxLength(1500);

                entity.Property(e => e.Name).HasMaxLength(1000);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.To).HasMaxLength(1500);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.JobScheduleConfigurations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_JOB_Schedule_Configuration_ENTITY_ID");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.JobScheduleConfigurations)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_JOB_Schedule_Configuration_Type");
            });

            modelBuilder.Entity<JobScheduleJobType>(entity =>
            {
                entity.ToTable("JOB_Schedule_Job_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<JobScheduleLog>(entity =>
            {
                entity.ToTable("JOB_Schedule_Log");

                entity.Property(e => e.BookingId).HasColumnName("Booking_Id");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.ReportId).HasColumnName("Report_Id");

                entity.Property(e => e.ScheduleType).HasColumnName("Schedule_Type");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.JobScheduleLogs)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_JOB_Schedule_Log_Booking_Id");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.JobScheduleLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_JOB_Schedule_Log_EntityId");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.JobScheduleLogs)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_JOB_Schedule_Log_Report_Id");
            });

            modelBuilder.Entity<KpiColumn>(entity =>
            {
                entity.ToTable("KPI_Column");

                entity.Property(e => e.FieldLabel)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FieldType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FilterDataSourceFieldCondition)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilterDataSourceFieldConditionValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilterDataSourceFieldName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilterDataSourceFieldValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilterDataSourceName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FilterDataSourceType)
                    .WithMany(p => p.KpiColumns)
                    .HasForeignKey(d => d.FilterDataSourceTypeId)
                    .HasConstraintName("FK__KPI_Colum__Filte__4A3B5A08");

                entity.HasOne(d => d.FilterSignEquality)
                    .WithMany(p => p.KpiColumns)
                    .HasForeignKey(d => d.FilterSignEqualityId)
                    .HasConstraintName("FK__KPI_Colum__Filte__4B2F7E41");

                entity.HasOne(d => d.IdModuleNavigation)
                    .WithMany(p => p.KpiColumns)
                    .HasForeignKey(d => d.IdModule)
                    .HasConstraintName("FK__KPI_Colum__IdMod__494735CF");

                entity.HasOne(d => d.IdSubModuleNavigation)
                    .WithMany(p => p.KpiColumns)
                    .HasForeignKey(d => d.IdSubModule)
                    .HasConstraintName("FK__KPI_Colum__IdSub__48531196");
            });

            modelBuilder.Entity<KpiTemplate>(entity =>
            {
                entity.ToTable("KPI_Template");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdModuleNavigation)
                    .WithMany(p => p.KpiTemplates)
                    .HasForeignKey(d => d.IdModule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KPI_Templ__IdMod__4F000F25");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.KpiTemplates)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KPI_Templ__UserI__4E0BEAEC");
            });

            modelBuilder.Entity<KpiTemplateColumn>(entity =>
            {
                entity.ToTable("KPI_TemplateColumn");

                entity.Property(e => e.ColumnName).HasMaxLength(200);

                entity.Property(e => e.Valuecolumn).HasMaxLength(300);

                entity.HasOne(d => d.IdColumnNavigation)
                    .WithMany(p => p.KpiTemplateColumns)
                    .HasForeignKey(d => d.IdColumn)
                    .HasConstraintName("FK__KPI_Templ__IdCol__52D0A009");

                entity.HasOne(d => d.IdTemplateNavigation)
                    .WithMany(p => p.KpiTemplateColumns)
                    .HasForeignKey(d => d.IdTemplate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KPI_Templ__IdTem__51DC7BD0");
            });

            modelBuilder.Entity<KpiTemplateSubModule>(entity =>
            {
                entity.HasKey(e => new { e.IdTemplate, e.IdSubModule })
                    .HasName("PK__KPI_Temp__95758743E563F92D");

                entity.ToTable("KPI_TemplateSubModule");

                entity.HasOne(d => d.IdSubModuleNavigation)
                    .WithMany(p => p.KpiTemplateSubModules)
                    .HasForeignKey(d => d.IdSubModule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KPI_Templ__IdSub__55AD0CB4");

                entity.HasOne(d => d.IdTemplateNavigation)
                    .WithMany(p => p.KpiTemplateSubModules)
                    .HasForeignKey(d => d.IdTemplate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KPI_Templ__IdTem__56A130ED");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogBookingFbQueue>(entity =>
            {
                entity.ToTable("LOG_Booking_FB_Queue");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.LogBookingFbQueues)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_LOG_Booking_FB_Queue_CreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.LogBookingFbQueues)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOG_Booking_FB_Queue_EntityId");
            });

            modelBuilder.Entity<LogBookingReportEmailQueue>(entity =>
            {
                entity.ToTable("LOG_Booking_Report_Email_Queue");

                entity.Property(e => e.AuditId).HasColumnName("Audit_Id");

                entity.Property(e => e.EmailLogId).HasColumnName("Email_Log_Id");

                entity.Property(e => e.EsTypeId).HasColumnName("Es_Type_Id");

                entity.Property(e => e.InspectionId).HasColumnName("Inspection_Id");

                entity.Property(e => e.ReportId).HasColumnName("Report_Id");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.LogBookingReportEmailQueues)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("FK_LOG_Booking_Report_Email_Queue_Audit_Id");

                entity.HasOne(d => d.EmailLog)
                    .WithMany(p => p.LogBookingReportEmailQueues)
                    .HasForeignKey(d => d.EmailLogId)
                    .HasConstraintName("FK_LOG_Booking_Report_Email_Queue_Email_Log_Id");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.LogBookingReportEmailQueues)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_LOG_Booking_Report_Email_Queue_EntityId");

                entity.HasOne(d => d.EsType)
                    .WithMany(p => p.LogBookingReportEmailQueues)
                    .HasForeignKey(d => d.EsTypeId)
                    .HasConstraintName("FK_LOG_Booking_Report_Email_Queue_Es_Type_Id");

                entity.HasOne(d => d.Inspection)
                    .WithMany(p => p.LogBookingReportEmailQueues)
                    .HasForeignKey(d => d.InspectionId)
                    .HasConstraintName("FK_LOG_Booking_Report_Email_Queue_Inspection_Id");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.LogBookingReportEmailQueues)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_LOG_Booking_Report_Email_Queue_Report_Id");
            });

            modelBuilder.Entity<LogEmailQueue>(entity =>
            {
                entity.ToTable("LOG_Email_Queue");

                entity.Property(e => e.Bcclist)
                    .HasColumnName("BCCList")
                    .HasMaxLength(2000);

                entity.Property(e => e.Cclist)
                    .HasColumnName("CCList")
                    .HasMaxLength(2000);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.SendOn).HasColumnType("datetime");

                entity.Property(e => e.SourceName).HasMaxLength(200);

                entity.Property(e => e.Subject).HasMaxLength(2000);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.LogEmailQueues)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LOG_Email__Creat__37C7A412");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.LogEmailQueues)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_LOG_Email_Queue_ENTITY_ID");
            });

            modelBuilder.Entity<LogEmailQueueAttachment>(entity =>
            {
                entity.ToTable("LOG_Email_Queue_Attachments");

                entity.HasIndex(e => e.GuidId)
                    .HasName("UQ__LOG_Emai__4B840D04101CD1D7")
                    .IsUnique();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmailQueueId).HasColumnName("Email_Queue_Id");

                entity.Property(e => e.FileLink).HasMaxLength(2000);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FileUniqueId).HasMaxLength(2000);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.LogEmailQueueAttachments)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LOG_Email__Creat__3C8C592F");

                entity.HasOne(d => d.EmailQueue)
                    .WithMany(p => p.LogEmailQueueAttachments)
                    .HasForeignKey(d => d.EmailQueueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LOG_Email__Email__3B9834F6");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.LogEmailQueueAttachments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_LOG_Email_Queue_Attachments_EntityId");
            });

            modelBuilder.Entity<MidNotification>(entity =>
            {
                entity.ToTable("MID_Notification");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NotificationMessage).HasMaxLength(1000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.MidNotifications)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_MID_Notification_EntityId");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MidNotifications)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_MID_Notification_MessageId");

                entity.HasOne(d => d.NotifType)
                    .WithMany(p => p.MidNotifications)
                    .HasForeignKey(d => d.NotifTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MID_Notif__Notif__3B583DD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MidNotifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MID_Notif__UserI__3C4C620D");
            });

            modelBuilder.Entity<MidNotificationMessage>(entity =>
            {
                entity.ToTable("MID_Notification_Message");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<MidNotificationType>(entity =>
            {
                entity.ToTable("MID_NotificationType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.MidNotificationTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__MID_Notif__Entit__3D408646");
            });

            modelBuilder.Entity<MidTask>(entity =>
            {
                entity.ToTable("MID_Task");

                entity.HasIndex(e => e.ReportTo);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.MidTasks)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_MID_Task_EntityId");

                entity.HasOne(d => d.TaskType)
                    .WithMany(p => p.MidTasks)
                    .HasForeignKey(d => d.TaskTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MID_Task__TaskTy__3E34AA7F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MidTasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MID_Task__UserId__60C9B9A5");
            });

            modelBuilder.Entity<MidTaskType>(entity =>
            {
                entity.ToTable("MID_TaskType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.MidTaskTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__MID_TaskT__Entit__401CF2F1");
            });

            modelBuilder.Entity<OmDetail>(entity =>
            {
                entity.ToTable("OM_Details");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(1000);

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.OmDetailCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__OM_Detail__Creat__2B97A2AE");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.OmDetails)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__OM_Detail__Custo__26D2ED91");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.OmDetailDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__OM_Detail__Delet__2D7FEB20");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.OmDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__OM_Detail__Entit__2E740F59");

                entity.HasOne(d => d.OfficeCountry)
                    .WithMany(p => p.OmDetailOfficeCountries)
                    .HasForeignKey(d => d.OfficeCountryId)
                    .HasConstraintName("FK__OM_Detail__Offic__27C711CA");

                entity.HasOne(d => d.OperationalCountry)
                    .WithMany(p => p.OmDetailOperationalCountries)
                    .HasForeignKey(d => d.OperationalCountryId)
                    .HasConstraintName("FK__OM_Detail__Opera__29AF5A3C");

                entity.HasOne(d => d.Purpose)
                    .WithMany(p => p.OmDetails)
                    .HasForeignKey(d => d.PurposeId)
                    .HasConstraintName("FK__OM_Detail__Purpo__2AA37E75");

                entity.HasOne(d => d.Qc)
                    .WithMany(p => p.OmDetails)
                    .HasForeignKey(d => d.QcId)
                    .HasConstraintName("FK__OM_Details__QcId__28BB3603");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.OmDetailUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__OM_Detail__Updat__2C8BC6E7");
            });

            modelBuilder.Entity<OmRefPurpose>(entity =>
            {
                entity.ToTable("OM_REF_Purpose");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<QcBlCustomer>(entity =>
            {
                entity.ToTable("QC_BL_Customer");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.Qcblid).HasColumnName("QCBLId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QcBlCustomers)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QC_BL_Customer_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.QcBlCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_QC_BL_Customer_Customer_Id");

                entity.HasOne(d => d.Qcbl)
                    .WithMany(p => p.QcBlCustomers)
                    .HasForeignKey(d => d.Qcblid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QC_BL_Customer_QCBLId");
            });

            modelBuilder.Entity<QcBlProductCatgeory>(entity =>
            {
                entity.ToTable("QC_BL_ProductCatgeory");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Qcblid).HasColumnName("QCBLId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QcBlProductCatgeories)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QC_BL_ProductCatgeory_CreatedBy");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.QcBlProductCatgeories)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_QC_BL_ProductCatgeory_ProductCategoryId");

                entity.HasOne(d => d.Qcbl)
                    .WithMany(p => p.QcBlProductCatgeories)
                    .HasForeignKey(d => d.Qcblid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QC_BL_ProductCatgeory_QCBLId");
            });

            modelBuilder.Entity<QcBlProductSubCategory>(entity =>
            {
                entity.ToTable("QC_BL_ProductSubCategory");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Qcblid).HasColumnName("QCBLId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QcBlProductSubCategories)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QC_BL_ProductSubCatgeory_CreatedBy");

                entity.HasOne(d => d.ProductSubCategory)
                    .WithMany(p => p.QcBlProductSubCategories)
                    .HasForeignKey(d => d.ProductSubCategoryId)
                    .HasConstraintName("FK_QC_BL_ProductSubCatgeory_ProductSubCategoryId");

                entity.HasOne(d => d.Qcbl)
                    .WithMany(p => p.QcBlProductSubCategories)
                    .HasForeignKey(d => d.Qcblid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QC_BL_ProductSubCatgeory_QCBLId");
            });

            modelBuilder.Entity<QcBlProductSubCategory2>(entity =>
            {
                entity.ToTable("QC_BL_ProductSubCategory2");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Qcblid).HasColumnName("QCBLId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QcBlProductSubCategory2S)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QC_BL_ProductSubCategory2_CreatedBy");

                entity.HasOne(d => d.ProductSubCategory2)
                    .WithMany(p => p.QcBlProductSubCategory2S)
                    .HasForeignKey(d => d.ProductSubCategory2Id)
                    .HasConstraintName("FK_QC_BL_ProductSubCategory2_ProductCategoryId");

                entity.HasOne(d => d.Qcbl)
                    .WithMany(p => p.QcBlProductSubCategory2S)
                    .HasForeignKey(d => d.Qcblid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QC_BL_ProductSubCategory2_QCBLId");
            });

            modelBuilder.Entity<QcBlSupplierFactory>(entity =>
            {
                entity.ToTable("QC_BL_Supplier_Factory");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Qcblid).HasColumnName("QCBLId");

                entity.Property(e => e.SupplierFactoryId).HasColumnName("Supplier_FactoryId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QcBlSupplierFactories)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QC_BL_Supplier_Factory_CreatedBy");

                entity.HasOne(d => d.Qcbl)
                    .WithMany(p => p.QcBlSupplierFactories)
                    .HasForeignKey(d => d.Qcblid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QC_BL_Supplier_Factory_QCBLId");

                entity.HasOne(d => d.SupplierFactory)
                    .WithMany(p => p.QcBlSupplierFactories)
                    .HasForeignKey(d => d.SupplierFactoryId)
                    .HasConstraintName("FK_QC_BL_Supplier_Factory_Supplier_Id");
            });

            modelBuilder.Entity<QcBlockList>(entity =>
            {
                entity.ToTable("QC_BlockList");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Qcid).HasColumnName("QCId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QcBlockListCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QC_BlockList_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.QcBlockListDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_QC_BlockList_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.QcBlockLists)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_QC_BlockList_EntityId");

                entity.HasOne(d => d.Qc)
                    .WithMany(p => p.QcBlockLists)
                    .HasForeignKey(d => d.Qcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QC_BlockList_QCId");
            });

            modelBuilder.Entity<QuBillMethod>(entity =>
            {
                entity.ToTable("QU_BillMethod");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<QuInspProduct>(entity =>
            {
                entity.HasKey(e => new { e.IdQuotation, e.ProductTranId })
                    .HasName("PK__QU_INSP___0A820C6387951DE7");

                entity.ToTable("QU_INSP_Product");

                entity.Property(e => e.AqlLevelDesc).HasMaxLength(600);

                entity.HasOne(d => d.IdQuotationNavigation)
                    .WithMany(p => p.QuInspProducts)
                    .HasForeignKey(d => d.IdQuotation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_INSP_P__IdQuo__0D6755F2");

                entity.HasOne(d => d.ProductTran)
                    .WithMany(p => p.QuInspProducts)
                    .HasForeignKey(d => d.ProductTranId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_INSP_P__Produ__0E5B7A2B");
            });

            modelBuilder.Entity<QuPaidBy>(entity =>
            {
                entity.ToTable("QU_PaidBy");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Label).HasMaxLength(200);
            });

            modelBuilder.Entity<QuPdfversion>(entity =>
            {
                entity.HasKey(e => e.GuidId)
                    .HasName("PK__QU_PDFVe__4B840D0539D2CA16");

                entity.ToTable("QU_PDFVersion");

                entity.Property(e => e.GuidId).ValueGeneratedNever();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.GenerateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuPdfversions)
                    .HasForeignKey(d => d.QuotationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_PDFVer__Quota__22778171");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.QuPdfversions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_PDFVer__UserI__0E7088C4");
            });

            modelBuilder.Entity<QuQuotation>(entity =>
            {
                entity.ToTable("QU_QUOTATION");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLegalName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FactoryAddress)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.IdStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.LegalFactoryName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PaymentTermsValue).HasMaxLength(200);

                entity.Property(e => e.SupplierLegalName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ValidatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BillingEntityNavigation)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.BillingEntity)
                    .HasConstraintName("FK__QU_QUOTAT__Billi__05FB3E54");

                entity.HasOne(d => d.BillingMethod)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.BillingMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Billi__0F64ACFD");

                entity.HasOne(d => d.BillingPaidBy)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.BillingPaidById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Billi__1058D136");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Count__114CF56F");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Curre__124119A8");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Custo__13353DE1");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_QU_QUOTATION_EntityId");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.QuQuotationFactories)
                    .HasForeignKey(d => d.FactoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Facto__1429621A");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__IdSta__151D8653");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Offic__1611AA8C");

                entity.HasOne(d => d.PaymentTermsNavigation)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.PaymentTerms)
                    .HasConstraintName("FK__QU_QUOTAT__Payme__7B139B8D");

                entity.HasOne(d => d.Rule)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.RuleId)
                    .HasConstraintName("FK_Qu_Quotation_RuleId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Servi__1705CEC5");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.QuQuotationSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_QUOTAT__Suppl__17F9F2FE");

                entity.HasOne(d => d.ValidatedByNavigation)
                    .WithMany(p => p.QuQuotations)
                    .HasForeignKey(d => d.ValidatedBy)
                    .HasConstraintName("FK_QU_QUOTATION_ValidatedBy");
            });

            modelBuilder.Entity<QuQuotationAudManday>(entity =>
            {
                entity.ToTable("QU_Quotation_Aud_Manday");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.QuQuotationAudMandays)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QU_Audit_Manday_BookingId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QuQuotationAudMandayCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QU_Audit_Manday_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.QuQuotationAudMandayDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_QU_Audit_Manday_DeletedBy");

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuQuotationAudMandays)
                    .HasForeignKey(d => d.QuotationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QU_Audit_Manday_QuotationId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.QuQuotationAudMandayUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_QU_Audit_Manday_UpdatedBy");
            });

            modelBuilder.Entity<QuQuotationAudit>(entity =>
            {
                entity.HasKey(e => new { e.IdQuotation, e.IdBooking })
                    .HasName("PK__QU_Quota__1BC33443CC7F8419");

                entity.ToTable("QU_Quotation_Audit");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo).HasMaxLength(1000);

                entity.Property(e => e.InvoiceRemarks).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.IdBookingNavigation)
                    .WithMany(p => p.QuQuotationAudits)
                    .HasForeignKey(d => d.IdBooking)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdBoo__18EE1737");

                entity.HasOne(d => d.IdQuotationNavigation)
                    .WithMany(p => p.QuQuotationAudits)
                    .HasForeignKey(d => d.IdQuotation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdQuo__19E23B70");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.QuQuotationAudits)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_QU_Quotation_Audit_IT_UserMaster");
            });

            modelBuilder.Entity<QuQuotationContact>(entity =>
            {
                entity.HasKey(e => new { e.IdQuotation, e.IdContact })
                    .HasName("PK__QU_Quota__1E487E7BD7FF92C0");

                entity.ToTable("QU_Quotation_Contact");

                entity.HasOne(d => d.IdContactNavigation)
                    .WithMany(p => p.QuQuotationContacts)
                    .HasForeignKey(d => d.IdContact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdCon__1AD65FA9");

                entity.HasOne(d => d.IdQuotationNavigation)
                    .WithMany(p => p.QuQuotationContacts)
                    .HasForeignKey(d => d.IdQuotation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdQuo__1BCA83E2");
            });

            modelBuilder.Entity<QuQuotationCustomerContact>(entity =>
            {
                entity.HasKey(e => new { e.IdQuotation, e.IdContact })
                    .HasName("PK__QU_Quota__1E487E7BD074A73C");

                entity.ToTable("QU_Quotation_CustomerContact");

                entity.HasOne(d => d.IdContactNavigation)
                    .WithMany(p => p.QuQuotationCustomerContacts)
                    .HasForeignKey(d => d.IdContact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdCon__1CBEA81B");

                entity.HasOne(d => d.IdQuotationNavigation)
                    .WithMany(p => p.QuQuotationCustomerContacts)
                    .HasForeignKey(d => d.IdQuotation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdQuo__1DB2CC54");
            });

            modelBuilder.Entity<QuQuotationFactoryContact>(entity =>
            {
                entity.HasKey(e => new { e.IdQuotation, e.IdContact })
                    .HasName("PK__QU_Quota__1E487E7B45670537");

                entity.ToTable("QU_Quotation_FactoryContact");

                entity.HasOne(d => d.IdContactNavigation)
                    .WithMany(p => p.QuQuotationFactoryContacts)
                    .HasForeignKey(d => d.IdContact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdCon__1EA6F08D");

                entity.HasOne(d => d.IdQuotationNavigation)
                    .WithMany(p => p.QuQuotationFactoryContacts)
                    .HasForeignKey(d => d.IdQuotation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdQuo__1F9B14C6");
            });

            modelBuilder.Entity<QuQuotationInsp>(entity =>
            {
                entity.HasKey(e => new { e.IdQuotation, e.IdBooking })
                    .HasName("PK__QU_Quota__1BC33443CEFC03F1");

                entity.ToTable("QU_Quotation_Insp");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo).HasMaxLength(1000);

                entity.Property(e => e.InvoiceRemarks).HasMaxLength(2000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BilledQtyTypeNavigation)
                    .WithMany(p => p.QuQuotationInsps)
                    .HasForeignKey(d => d.BilledQtyType)
                    .HasConstraintName("FK__QU_Quotat__Bille__57B61C0E");

                entity.HasOne(d => d.IdBookingNavigation)
                    .WithMany(p => p.QuQuotationInsps)
                    .HasForeignKey(d => d.IdBooking)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdBoo__3B432F3B");

                entity.HasOne(d => d.IdQuotationNavigation)
                    .WithMany(p => p.QuQuotationInsps)
                    .HasForeignKey(d => d.IdQuotation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdQuo__3C375374");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.QuQuotationInsps)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_QU_Quotation_Insp_IT_UserMaster");
            });

            modelBuilder.Entity<QuQuotationInspManday>(entity =>
            {
                entity.ToTable("QU_Quotation_Insp_Manday");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.QuQuotationInspMandays)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QU_Insp_Manday_BookingId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QuQuotationInspMandayCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_QU_Insp_Manday_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.QuQuotationInspMandayDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_QU_Insp_Manday_DeletedBy");

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuQuotationInspMandays)
                    .HasForeignKey(d => d.QuotationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QU_Insp_Manday_QuotationId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.QuQuotationInspMandayUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_QU_Insp_Manday_UpdatedBy");
            });

            modelBuilder.Entity<QuQuotationPdfVersion>(entity =>
            {
                entity.ToTable("QU_Quotation_Pdf_Version");

                entity.Property(e => e.FileName).HasMaxLength(500);

                entity.Property(e => e.QuotationId).HasColumnName("Quotation_Id");

                entity.Property(e => e.UniqueId).HasMaxLength(1000);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuQuotationPdfVersions)
                    .HasForeignKey(d => d.QuotationId)
                    .HasConstraintName("FK_QU_Quotation_Pdf_Version_Quotation_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.QuQuotationPdfVersions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_QU_Quotation_Pdf_Version_UserId");
            });

            modelBuilder.Entity<QuQuotationSupplierContact>(entity =>
            {
                entity.HasKey(e => new { e.IdQuotation, e.IdContact })
                    .HasName("PK__QU_Quota__1E487E7B7C8AAFA3");

                entity.ToTable("QU_Quotation_SupplierContact");

                entity.HasOne(d => d.IdContactNavigation)
                    .WithMany(p => p.QuQuotationSupplierContacts)
                    .HasForeignKey(d => d.IdContact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdCon__208F38FF");

                entity.HasOne(d => d.IdQuotationNavigation)
                    .WithMany(p => p.QuQuotationSupplierContacts)
                    .HasForeignKey(d => d.IdQuotation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_Quotat__IdQuo__21835D38");
            });

            modelBuilder.Entity<QuStatus>(entity =>
            {
                entity.ToTable("QU_Status");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<QuTranStatusLog>(entity =>
            {
                entity.ToTable("QU_TRAN_Status_Log");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.StatusChangeDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QuTranStatusLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__QU_TRAN_S__Creat__51526BEB");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.QuTranStatusLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_QU_TRAN_Status_Log_EntityId");

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuTranStatusLogs)
                    .HasForeignKey(d => d.QuotationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_TRAN_S__Quota__533AB45D");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.QuTranStatusLogs)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QU_TRAN_S__Statu__52469024");
            });

            modelBuilder.Entity<QuWorkLoadMatrix>(entity =>
            {
                entity.ToTable("QU_WorkLoadMatrix");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.SampleSize8h).HasColumnName("SampleSize_8h");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.QuWorkLoadMatrixCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__QU_WorkLo__Creat__4868EB86");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.QuWorkLoadMatrixDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__QU_WorkLo__Delet__4A5133F8");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.QuWorkLoadMatrices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__QU_WorkLo__Entit__4B455831");

                entity.HasOne(d => d.ProductSubCategory3)
                    .WithMany(p => p.QuWorkLoadMatrices)
                    .HasForeignKey(d => d.ProductSubCategory3Id)
                    .HasConstraintName("FK__QU_WorkLo__Produ__4774C74D");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.QuWorkLoadMatrixUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__QU_WorkLo__Updat__495D0FBF");
            });

            modelBuilder.Entity<RefAddressType>(entity =>
            {
                entity.ToTable("REF_AddressType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefAddressTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Addre__Entit__4111172A");
            });

            modelBuilder.Entity<RefAqlPickSampleSizeAcceCode>(entity =>
            {
                entity.ToTable("REF_AQL_Pick_SampleSize_Acce_Code");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AccSampleSizeCode)
                    .HasColumnName("Acc_sample_Size_Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SampleSizeCode)
                    .HasColumnName("Sample_Size_Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefAqlPickSampleSizeCodeValue>(entity =>
            {
                entity.HasKey(e => e.SampleSizeCode);

                entity.ToTable("REF_AQL_Pick_SampleSize_CodeValue");

                entity.Property(e => e.SampleSizeCode)
                    .HasColumnName("Sample_Size_Code")
                    .HasMaxLength(1)
                    .ValueGeneratedNever();

                entity.Property(e => e.SampleSize).HasColumnName("Sample_Size");
            });

            modelBuilder.Entity<RefAqlSampleCode>(entity =>
            {
                entity.HasKey(e => e.SampleSizeRangeCodeId);

                entity.ToTable("REF_AQL_Sample_Code");

                entity.Property(e => e.SampleSizeRangeCodeId).HasColumnName("Sample_Size_range_Code_Id");

                entity.Property(e => e.LevelISampleSizeCode)
                    .IsRequired()
                    .HasColumnName("Level_I_Sample_Size_Code")
                    .HasMaxLength(1);

                entity.Property(e => e.LevelIiSampleSizeCode)
                    .IsRequired()
                    .HasColumnName("Level_II_Sample_Size_Code")
                    .HasMaxLength(1);

                entity.Property(e => e.LevelIiiSampleSizeCode)
                    .IsRequired()
                    .HasColumnName("Level_III_Sample_Size_Code")
                    .HasMaxLength(1);

                entity.Property(e => e.LevelS1SampleSizeCode)
                    .IsRequired()
                    .HasColumnName("LEVEL_S1_SAMPLE_SIZE_CODE")
                    .HasMaxLength(1);

                entity.Property(e => e.LevelS2SampleSizeCode)
                    .IsRequired()
                    .HasColumnName("LEVEL_S2_SAMPLE_SIZE_CODE")
                    .HasMaxLength(1);

                entity.Property(e => e.LevelS3SampleSizeCode)
                    .IsRequired()
                    .HasColumnName("LEVEL_S3_SAMPLE_SIZE_CODE")
                    .HasMaxLength(1);

                entity.Property(e => e.LevelS4SampleSizeCode)
                    .IsRequired()
                    .HasColumnName("LEVEL_S4_SAMPLE_SIZE_CODE")
                    .HasMaxLength(1);

                entity.Property(e => e.MaxSize).HasColumnName("Max_size");

                entity.Property(e => e.MinSize).HasColumnName("Min_size");

                entity.HasOne(d => d.LevelISampleSizeCodeNavigation)
                    .WithMany(p => p.RefAqlSampleCodeLevelISampleSizeCodeNavigations)
                    .HasForeignKey(d => d.LevelISampleSizeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_AQL_S__Level__73F19CB2");

                entity.HasOne(d => d.LevelIiSampleSizeCodeNavigation)
                    .WithMany(p => p.RefAqlSampleCodeLevelIiSampleSizeCodeNavigations)
                    .HasForeignKey(d => d.LevelIiSampleSizeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_AQL_S__Level__74E5C0EB");

                entity.HasOne(d => d.LevelIiiSampleSizeCodeNavigation)
                    .WithMany(p => p.RefAqlSampleCodeLevelIiiSampleSizeCodeNavigations)
                    .HasForeignKey(d => d.LevelIiiSampleSizeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_AQL_S__Level__75D9E524");

                entity.HasOne(d => d.LevelS1SampleSizeCodeNavigation)
                    .WithMany(p => p.RefAqlSampleCodeLevelS1SampleSizeCodeNavigations)
                    .HasForeignKey(d => d.LevelS1SampleSizeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_AQL_S__LEVEL__76CE095D");

                entity.HasOne(d => d.LevelS2SampleSizeCodeNavigation)
                    .WithMany(p => p.RefAqlSampleCodeLevelS2SampleSizeCodeNavigations)
                    .HasForeignKey(d => d.LevelS2SampleSizeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_AQL_S__LEVEL__77C22D96");

                entity.HasOne(d => d.LevelS3SampleSizeCodeNavigation)
                    .WithMany(p => p.RefAqlSampleCodeLevelS3SampleSizeCodeNavigations)
                    .HasForeignKey(d => d.LevelS3SampleSizeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_AQL_S__LEVEL__78B651CF");

                entity.HasOne(d => d.LevelS4SampleSizeCodeNavigation)
                    .WithMany(p => p.RefAqlSampleCodeLevelS4SampleSizeCodeNavigations)
                    .HasForeignKey(d => d.LevelS4SampleSizeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_AQL_S__LEVEL__79AA7608");
            });

            modelBuilder.Entity<RefArea>(entity =>
            {
                entity.ToTable("REF_Area");

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasColumnName("Area_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RefBillingEntity>(entity =>
            {
                entity.ToTable("REF_Billing_Entity");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefBillingEntities)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_REF_BILLING_ENTITY_ENTITY_ID");
            });

            modelBuilder.Entity<RefBudgetForecast>(entity =>
            {
                entity.ToTable("REF_Budget_Forecast");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.RefBudgetForecasts)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK__REF_Budge__Count__2E9E2C8B");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RefBudgetForecastCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__REF_Budge__Creat__2F9250C4");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.RefBudgetForecasts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("budget_forecast_currency_fk");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.RefBudgetForecastDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__REF_Budge__Delet__317A9936");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefBudgetForecasts)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_REF_Budget_Forecast_EntityId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.RefBudgetForecastUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__REF_Budge__Updat__308674FD");
            });

            modelBuilder.Entity<RefBusinessLine>(entity =>
            {
                entity.ToTable("REF_BUSINESS_LINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.BusinessLine).HasMaxLength(200);
            });

            modelBuilder.Entity<RefBusinessType>(entity =>
            {
                entity.ToTable("REF_BusinessType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefBusinessTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Busin__Entit__42053B63");
            });

            modelBuilder.Entity<RefCity>(entity =>
            {
                entity.ToTable("REF_City");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasColumnName("City_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.PhCode)
                    .HasColumnName("Ph_Code")
                    .HasMaxLength(20);

                entity.Property(e => e.ProvinceId).HasColumnName("Province_Id");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.RefCities)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_City__Provin__42F95F9C");
            });

            modelBuilder.Entity<RefCityDetail>(entity =>
            {
                entity.ToTable("REF_CITY_DETAILS");

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.LocationId).HasColumnName("Location_Id");

                entity.Property(e => e.TravelTime).HasColumnName("Travel_Time");

                entity.Property(e => e.ZoneId).HasColumnName("Zone_Id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.RefCityDetails)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_CITY___City___46C9F080");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefCityDetails)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_CITY___Entit__48B238F2");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.RefCityDetails)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__REF_CITY___Locat__43ED83D5");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.RefCityDetails)
                    .HasForeignKey(d => d.ZoneId)
                    .HasConstraintName("FK__REF_CITY___Zone___47BE14B9");
            });

            modelBuilder.Entity<RefContainerSize>(entity =>
            {
                entity.ToTable("REF_Container_Size");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<RefCountry>(entity =>
            {
                entity.ToTable("REF_Country");

                entity.Property(e => e.Alpha2Code)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AreaId).HasColumnName("Area_id");

                entity.Property(e => e.CountryCode).HasColumnName("Country_Code");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("Country_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.FbCountryId).HasColumnName("FB_CountryId");

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.RefCountries)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK__REF_Count__Area___49A65D2B");
            });

            modelBuilder.Entity<RefCountryLocation>(entity =>
            {
                entity.HasKey(e => new { e.CountryId, e.LocationId })
                    .HasName("PK__REF_Coun__7EAE8AD65D612C74");

                entity.ToTable("REF_Country_Location");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.RefCountryLocations)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Count__Count__4A9A8164");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.RefCountryLocations)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Count__Locat__4B8EA59D");
            });

            modelBuilder.Entity<RefCounty>(entity =>
            {
                entity.ToTable("REF_County");

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.CountyCode)
                    .HasColumnName("County_Code")
                    .HasMaxLength(500);

                entity.Property(e => e.CountyName)
                    .IsRequired()
                    .HasColumnName("County_Name")
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("Created_On")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasColumnName("Deleted_By");

                entity.Property(e => e.DeletedOn)
                    .HasColumnName("Deleted_On")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("Modified_On")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.RefCounties)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Count__Delet__43195B2D");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RefCountyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__REF_Count__Creat__440D7F66");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.RefCountyDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__REF_Count__Delet__45F5C7D8");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.RefCountyModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__REF_Count__Modif__4501A39F");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.RefCounties)
                    .HasForeignKey(d => d.ZoneId)
                    .HasConstraintName("REF_County_ZoneId");
            });

            modelBuilder.Entity<RefCurrency>(entity =>
            {
                entity.ToTable("REF_Currency");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CurrencyCodeA)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RefDataSourceType>(entity =>
            {
                entity.ToTable("REF_DataSourceType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<RefDateFormat>(entity =>
            {
                entity.ToTable("REF_DateFormat");

                entity.Property(e => e.DateFormat).HasMaxLength(20);
            });

            modelBuilder.Entity<RefDefectClassification>(entity =>
            {
                entity.ToTable("REF_DefectClassification");

                entity.Property(e => e.Value).HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefDefectClassifications)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Defec__Entit__4C82C9D6");
            });

            modelBuilder.Entity<RefDelimiter>(entity =>
            {
                entity.ToTable("Ref_Delimiter");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsFile).HasColumnName("Is_File");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RefDelimiters)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Ref_Delimiter_CreatedBy");
            });

            modelBuilder.Entity<RefExpertise>(entity =>
            {
                entity.ToTable("REF_Expertise");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefExpertises)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Exper__Entit__4D76EE0F");
            });

            modelBuilder.Entity<RefFileExtension>(entity =>
            {
                entity.ToTable("REF_File_Extension");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.ExtensionName).HasMaxLength(50);
            });

            modelBuilder.Entity<RefInspCusDecision>(entity =>
            {
                entity.ToTable("REF_INSP_CUS_decision");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<RefInspCusDecisionConfig>(entity =>
            {
                entity.ToTable("REF_INSP_CUS_Decision_Config");

                entity.Property(e => e.CustomDecisionName).HasMaxLength(200);

                entity.HasOne(d => d.CusDec)
                    .WithMany(p => p.RefInspCusDecisionConfigs)
                    .HasForeignKey(d => d.CusDecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CDCusDecId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RefInspCusDecisionConfigs)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CDCustomerId");
            });

            modelBuilder.Entity<RefInvoiceType>(entity =>
            {
                entity.ToTable("REF_InvoiceType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefInvoiceTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Invoi__Entit__4E6B1248");
            });

            modelBuilder.Entity<RefKpiTeamplate>(entity =>
            {
                entity.ToTable("REF_KPI_Teamplate");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.RefKpiTeamplates)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_REF_KPI_TEAMPLATE_TypeId");
            });

            modelBuilder.Entity<RefKpiTeamplateCustomer>(entity =>
            {
                entity.ToTable("REF_KPI_Teamplate_Customer");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RefKpiTeamplateCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__REF_KPI_T__Custo__1C955640");

                entity.HasOne(d => d.Teamplate)
                    .WithMany(p => p.RefKpiTeamplateCustomers)
                    .HasForeignKey(d => d.TeamplateId)
                    .HasConstraintName("FK__REF_KPI_T__Teamp__1BA13207");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.RefKpiTeamplateCustomers)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("FK__REF_KPI_T__UserT__1D897A79");
            });

            modelBuilder.Entity<RefKpiTemplateType>(entity =>
            {
                entity.ToTable("REF_KPI_Template_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<RefLevelPick1>(entity =>
            {
                entity.ToTable("REF_LevelPick1");

                entity.Property(e => e.Fbvalue)
                    .HasColumnName("FBValue")
                    .HasMaxLength(100);

                entity.Property(e => e.Value).HasMaxLength(20);
            });

            modelBuilder.Entity<RefLevelPick2>(entity =>
            {
                entity.ToTable("REF_LevelPick2");

                entity.Property(e => e.Value).HasMaxLength(20);
            });

            modelBuilder.Entity<RefLocation>(entity =>
            {
                entity.ToTable("REF_Location");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(1500);

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(1500);

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.Comment).HasMaxLength(2000);

                entity.Property(e => e.DefaultCurrencyId).HasColumnName("Default_Currency_Id");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Fax).HasMaxLength(40);

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasColumnName("Location_Name")
                    .HasMaxLength(1000);

                entity.Property(e => e.LocationTypeId).HasColumnName("LocationType_Id");

                entity.Property(e => e.MasterCurrencyId).HasColumnName("Master_Currency_Id");

                entity.Property(e => e.OfficeCode).HasMaxLength(200);

                entity.Property(e => e.Tel).HasMaxLength(100);

                entity.Property(e => e.ZipCode).HasMaxLength(30);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.RefLocations)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Locat__City___50535ABA");

                entity.HasOne(d => d.DefaultCurrency)
                    .WithMany(p => p.RefLocationDefaultCurrencies)
                    .HasForeignKey(d => d.DefaultCurrencyId)
                    .HasConstraintName("FK__REF_Locat__Defau__532FC765");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefLocations)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Locat__Entit__4F5F3681");

                entity.HasOne(d => d.HeadOfficeNavigation)
                    .WithMany(p => p.InverseHeadOfficeNavigation)
                    .HasForeignKey(d => d.HeadOffice)
                    .HasConstraintName("FK__REF_Locat__HeadO__5423EB9E");

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.RefLocations)
                    .HasForeignKey(d => d.LocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Locat__Locat__51477EF3");

                entity.HasOne(d => d.MasterCurrency)
                    .WithMany(p => p.RefLocationMasterCurrencies)
                    .HasForeignKey(d => d.MasterCurrencyId)
                    .HasConstraintName("FK__REF_Locat__Maste__523BA32C");
            });

            modelBuilder.Entity<RefLocationCountry>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.CountryId })
                    .HasName("PK__REF_Loca__06F3B27C7A6FB67A");

                entity.ToTable("REF_Location_Country");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.RefLocationCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Locat__Count__560C3410");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.RefLocationCountries)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Locat__Locat__55180FD7");
            });

            modelBuilder.Entity<RefLocationType>(entity =>
            {
                entity.ToTable("REF_LocationType");

                entity.Property(e => e.SgtLocationType)
                    .IsRequired()
                    .HasColumnName("SGT_Location_Type")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefLocationTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Locat__Entit__57005849");
            });

            modelBuilder.Entity<RefMarketSegment>(entity =>
            {
                entity.ToTable("REF_MarketSegment");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefMarketSegments)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Marke__Entit__57F47C82");
            });

            modelBuilder.Entity<RefModule>(entity =>
            {
                entity.ToTable("REF_Modules");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RefPick1>(entity =>
            {
                entity.ToTable("REF_Pick1");
            });

            modelBuilder.Entity<RefPick2>(entity =>
            {
                entity.ToTable("REF_Pick2");
            });

            modelBuilder.Entity<RefPickType>(entity =>
            {
                entity.ToTable("REF_PickType");

                entity.Property(e => e.Value).HasMaxLength(50);
            });

            modelBuilder.Entity<RefProductCategory>(entity =>
            {
                entity.ToTable("REF_ProductCategory");

                entity.Property(e => e.FbProductCategoryId).HasColumnName("Fb_ProductCategory_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.BusinessLine)
                    .WithMany(p => p.RefProductCategories)
                    .HasForeignKey(d => d.BusinessLineId)
                    .HasConstraintName("FK__REF_Produ__Busin__1C604C16");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefProductCategories)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Produ__Entit__59DCC4F4");
            });

            modelBuilder.Entity<RefProductCategoryApiService>(entity =>
            {
                entity.ToTable("REF_Product_Category_API_Services");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomName).HasMaxLength(50);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ProductCategoryId).HasColumnName("Product_Category_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RefProductCategoryApiServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__REF_Produ__Creat__1CB4867A");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.RefProductCategoryApiServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__REF_Produ__Delet__1DA8AAB3");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.RefProductCategoryApiServices)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Produ__Produ__1ACC3E08");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.RefProductCategoryApiServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Product_Category_Service");
            });

            modelBuilder.Entity<RefProductCategorySub>(entity =>
            {
                entity.ToTable("REF_ProductCategory_Sub");

                entity.Property(e => e.FbProductSubCategoryId).HasColumnName("Fb_Product_SubCategory_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefProductCategorySubs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Produ__Entit__5BC50D66");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.RefProductCategorySubs)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Produ__Produ__5AD0E92D");
            });

            modelBuilder.Entity<RefProductCategorySub2>(entity =>
            {
                entity.ToTable("REF_ProductCategory_Sub2");

                entity.Property(e => e.FbProductSubCategory2Id).HasColumnName("Fb_Product_SubCategory2_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ProductSubCategoryId).HasColumnName("ProductSubCategoryID");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefProductCategorySub2S)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Produ__Entit__7A9E9A41");

                entity.HasOne(d => d.ProductSubCategory)
                    .WithMany(p => p.RefProductCategorySub2S)
                    .HasForeignKey(d => d.ProductSubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Produ__Produ__7B92BE7A");
            });

            modelBuilder.Entity<RefProductCategorySub3>(entity =>
            {
                entity.ToTable("REF_ProductCategory_Sub3");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RefProductCategorySub3CreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__REF_Produ__Creat__3B0EF068");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.RefProductCategorySub3DeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__REF_Produ__Delet__3CF738DA");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefProductCategorySub3S)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Produ__Entit__3DEB5D13");

                entity.HasOne(d => d.ProductSubCategory2)
                    .WithMany(p => p.RefProductCategorySub3S)
                    .HasForeignKey(d => d.ProductSubCategory2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Produ__Produ__3A1ACC2F");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.RefProductCategorySub3UpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__REF_Produ__Updat__3C0314A1");
            });

            modelBuilder.Entity<RefProductUnit>(entity =>
            {
                entity.ToTable("REF_Product_Units");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefProductUnits)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Produ__Entit__58E8A0BB");
            });

            modelBuilder.Entity<RefProspectStatus>(entity =>
            {
                entity.ToTable("REF_ProspectStatus");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefProvince>(entity =>
            {
                entity.ToTable("REF_Province");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CountryId).HasColumnName("Country_Id");

                entity.Property(e => e.FbProvinceId).HasColumnName("FB_ProvinceId");

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("Province_Code")
                    .HasMaxLength(50);

                entity.Property(e => e.ProvinceName)
                    .IsRequired()
                    .HasColumnName("Province_Name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.RefProvinces)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Provi__Count__5EA17A11");
            });

            modelBuilder.Entity<RefReInspectionType>(entity =>
            {
                entity.ToTable("REF_ReInspectionType");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<RefReportUnit>(entity =>
            {
                entity.ToTable("REF_ReportUnit");

                entity.Property(e => e.Active).HasColumnName("Active ");

                entity.Property(e => e.Value).HasMaxLength(20);
            });

            modelBuilder.Entity<RefSampleType>(entity =>
            {
                entity.ToTable("REF_SampleType");

                entity.Property(e => e.SampleSize).HasMaxLength(100);

                entity.Property(e => e.SampleType).HasMaxLength(100);
            });

            modelBuilder.Entity<RefSeason>(entity =>
            {
                entity.ToTable("REF_Season");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefSeasons)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Seaso__Entit__5F959E4A");
            });

            modelBuilder.Entity<RefSeasonYear>(entity =>
            {
                entity.ToTable("REF_Season_Year");
            });

            modelBuilder.Entity<RefService>(entity =>
            {
                entity.ToTable("REF_Service");

                entity.Property(e => e.FbServiceId).HasColumnName("Fb_Service_Id");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefServices)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Servi__Entit__6089C283");
            });

            modelBuilder.Entity<RefServiceType>(entity =>
            {
                entity.ToTable("REF_ServiceType");

                entity.Property(e => e.Abbreviation).HasMaxLength(50);

                entity.Property(e => e.FbServiceTypeId).HasColumnName("Fb_ServiceType_Id");

                entity.Property(e => e.IsAutoQcexpenseClaim).HasColumnName("IsAutoQCExpenseClaim");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.BusinessLine)
                    .WithMany(p => p.RefServiceTypes)
                    .HasForeignKey(d => d.BusinessLineId)
                    .HasConstraintName("FK_BUSSINESS_LINE");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefServiceTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Servi__Entit__617DE6BC");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.RefServiceTypes)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_SERVICE");
            });

            modelBuilder.Entity<RefServiceTypeXero>(entity =>
            {
                entity.ToTable("Ref_ServiceType_Xero");

                entity.Property(e => e.EntityId).HasColumnName("Entity_Id");

                entity.Property(e => e.InspectionServiceTypeId).HasColumnName("Inspection_ServiceType_Id");

                entity.Property(e => e.InspectionType)
                    .HasColumnName("Inspection_Type")
                    .HasMaxLength(500);

                entity.Property(e => e.InspectionTypeConsolidate)
                    .HasColumnName("Inspection_Type_consolidate")
                    .HasMaxLength(500);

                entity.Property(e => e.TrackingOptionName).HasMaxLength(500);

                entity.Property(e => e.TrackingOptionNameTravel)
                    .HasColumnName("TrackingOptionName_Travel")
                    .HasMaxLength(500);

                entity.Property(e => e.XeroAccount)
                    .HasColumnName("XERO_Account")
                    .HasMaxLength(500);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefServiceTypeXeros)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_Ref_ServiceType_Xero_Entity_Id");

                entity.HasOne(d => d.InspectionServiceType)
                    .WithMany(p => p.RefServiceTypeXeros)
                    .HasForeignKey(d => d.InspectionServiceTypeId)
                    .HasConstraintName("FK_Ref_ServiceType_Xero_Inspection_ServiceType_Id");
            });

            modelBuilder.Entity<RefSignEquality>(entity =>
            {
                entity.ToTable("REF_SignEquality");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefTown>(entity =>
            {
                entity.ToTable("REF_Town");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.TownCode).HasMaxLength(500);

                entity.Property(e => e.TownName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.County)
                    .WithMany(p => p.RefTowns)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REF_Town__Delete__59FCC085");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RefTownCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__REF_Town__Create__5AF0E4BE");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.RefTownDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__REF_Town__Delete__5CD92D30");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.RefTownModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__REF_Town__Modifi__5BE508F7");
            });

            modelBuilder.Entity<RefTranslation>(entity =>
            {
                entity.ToTable("REF_Translation");

                entity.Property(e => e.TextCh)
                    .HasColumnName("Text_CH")
                    .HasMaxLength(2000);

                entity.Property(e => e.TextFr)
                    .HasColumnName("Text_FR")
                    .HasMaxLength(2000);

                entity.HasOne(d => d.TranslationGroup)
                    .WithMany(p => p.RefTranslations)
                    .HasForeignKey(d => d.TranslationGroupId)
                    .HasConstraintName("FK__REF_Trans__Trans__62720AF5");
            });

            modelBuilder.Entity<RefTranslationGroup>(entity =>
            {
                entity.ToTable("REF_TranslationGroup");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<RefUnit>(entity =>
            {
                entity.ToTable("REF_Unit");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefUnits)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__REF_Unit__Entity__63662F2E");
            });

            modelBuilder.Entity<RefZone>(entity =>
            {
                entity.ToTable("REF_Zone");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RefZones)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_REF_Zone_EntityId");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.RefZones)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("REF_Zone_LocationId");
            });

            modelBuilder.Entity<RepFastRefStatus>(entity =>
            {
                entity.ToTable("REP_FAST_REF_Status");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<RepFastTemplate>(entity =>
            {
                entity.ToTable("REP_FAST_Template");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<RepFastTemplateConfig>(entity =>
            {
                entity.ToTable("REP_FAST_Template_Config");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.FileExtensionId).HasColumnName("FileExtensionID");

                entity.Property(e => e.ReportToolTypeId)
                    .HasColumnName("ReportToolTypeID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ScheduleFromDate).HasColumnType("datetime");

                entity.Property(e => e.ScheduleToDate).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_CU_Brand");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_CU_Customer");

                entity.HasOne(d => d.Depart)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.DepartId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_CU_Department");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.Entityid)
                    .HasConstraintName("FK_REP_FAST_Template_Config_AP_Entity");

                entity.HasOne(d => d.FileExtension)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.FileExtensionId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_REF_File_Extension");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_REF_ProductCategory");

                entity.HasOne(d => d.ReportToolType)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.ReportToolTypeId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_REP_REF_ToolType");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_REF_ServiceType");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.RepFastTemplateConfigs)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK_REP_FAST_Template_Config_REP_FAST_Template");
            });

            modelBuilder.Entity<RepFastTranLog>(entity =>
            {
                entity.ToTable("REP_FAST_TRAN_Log");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FastTran)
                    .WithMany(p => p.RepFastTranLogs)
                    .HasForeignKey(d => d.FastTranId)
                    .HasConstraintName("FK_REP_FAST_TRAN_Log_FastTranId");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.RepFastTranLogs)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_REP_FAST_TRAN_Log_ReportId");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.RepFastTranLogs)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_REP_FAST_TRAN_Log_Status");
            });

            modelBuilder.Entity<RepFastTransaction>(entity =>
            {
                entity.ToTable("REP_FAST_Transaction");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItNotification).HasColumnName("IT_Notification");

                entity.Property(e => e.ReportLink).HasMaxLength(2000);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.RepFastTransactions)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_REP_FAST_Transaction_BookingId");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.RepFastTransactions)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_REP_FAST_Transaction_ReportId");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.RepFastTransactions)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_REP_FAST_Transaction_Status");
            });

            modelBuilder.Entity<RepRefToolType>(entity =>
            {
                entity.ToTable("REP_REF_ToolType");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.ToolName).HasMaxLength(50);
            });

            modelBuilder.Entity<RestApiLog>(entity =>
            {
                entity.ToTable("RestApiLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RequestMethod).HasMaxLength(200);

                entity.Property(e => e.RequestPath).HasMaxLength(2000);

                entity.Property(e => e.RequestQuery).HasMaxLength(2000);

                entity.Property(e => e.RequestTime).HasColumnType("datetime");

                entity.Property(e => e.ResponseStatus).HasMaxLength(2000);

                entity.Property(e => e.ResponseTime).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RestApiLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_RestApiLog_CreatedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.RestApiLogs)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_RestApiLog_EntityId");
            });

            modelBuilder.Entity<SchQctype>(entity =>
            {
                entity.ToTable("SCH_QCType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SchQctypeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__SCH_QCTyp__Creat__503E4C21");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SchQctypeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__SCH_QCTyp__Delet__5132705A");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.SchQctypeModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__SCH_QCTyp__Modif__52269493");
            });

            modelBuilder.Entity<SchScheduleC>(entity =>
            {
                entity.ToTable("SCH_Schedule_CS");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Csid).HasColumnName("CSId");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IsReportReviewCs).HasColumnName("IsReportReviewCS");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.SchScheduleCS)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SCH_Sched__Booki__55F72577");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SchScheduleCCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__SCH_Sched__Creat__56EB49B0");

                entity.HasOne(d => d.Cs)
                    .WithMany(p => p.SchScheduleCS)
                    .HasForeignKey(d => d.Csid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SCH_Schedu__CSId__57DF6DE9");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SchScheduleCDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__SCH_Sched__Delet__58D39222");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.SchScheduleCModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__SCH_Sched__Modif__59C7B65B");
            });

            modelBuilder.Entity<SchScheduleQc>(entity =>
            {
                entity.ToTable("SCH_Schedule_QC");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IsReportFilledQc).HasColumnName("IsReportFilledQC");

                entity.Property(e => e.IsVisibleToQc).HasColumnName("IsVisibleToQC");

                entity.Property(e => e.KeepQcforTravelExpense).HasColumnName("KeepQCForTravelExpense");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.QcLeader).HasColumnName("QC_Leader");

                entity.Property(e => e.Qcid).HasColumnName("QCId");

                entity.Property(e => e.Qctype).HasColumnName("QCType");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.SchScheduleQcs)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SCH_Sched__Booki__65396907");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SchScheduleQcCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__SCH_Sched__Creat__6721B179");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SchScheduleQcDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__SCH_Sched__Delet__6815D5B2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.SchScheduleQcModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__SCH_Sched__Modif__6909F9EB");

                entity.HasOne(d => d.Qc)
                    .WithMany(p => p.SchScheduleQcs)
                    .HasForeignKey(d => d.Qcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SCH_Schedu__QCId__662D8D40");

                entity.HasOne(d => d.QctypeNavigation)
                    .WithMany(p => p.SchScheduleQcs)
                    .HasForeignKey(d => d.Qctype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SCH_Sched__QCTyp__69FE1E24");
            });

            modelBuilder.Entity<SuAddress>(entity =>
            {
                entity.ToTable("SU_Address");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.LocalLanguage).HasMaxLength(2000);

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");

                entity.Property(e => e.ZipCode).HasMaxLength(20);

                entity.HasOne(d => d.AddressType)
                    .WithMany(p => p.SuAddresses)
                    .HasForeignKey(d => d.AddressTypeId)
                    .HasConstraintName("FK__SU_Addres__Addre__682AE44B");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.SuAddresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Addres__CityI__6736C012");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.SuAddresses)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Addres__Count__654E77A0");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.SuAddresses)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("countyfk");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.SuAddresses)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Addres__Regio__66429BD9");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuAddresses)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Addres__Suppl__645A5367");

                entity.HasOne(d => d.Town)
                    .WithMany(p => p.SuAddresses)
                    .HasForeignKey(d => d.TownId)
                    .HasConstraintName("townfk");
            });

            modelBuilder.Entity<SuAddressType>(entity =>
            {
                entity.ToTable("SU_AddressType");

                entity.Property(e => e.AddressType)
                    .IsRequired()
                    .HasColumnName("Address_Type")
                    .HasMaxLength(50);

                entity.Property(e => e.AddressTypeFlag)
                    .IsRequired()
                    .HasColumnName("Address_Type_Flag")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SuAddressTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__SU_Addres__Entit__691F0884");
            });

            modelBuilder.Entity<SuApiService>(entity =>
            {
                entity.ToTable("SU_API_Services");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuApiServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__SU_API_Se__Creat__4F5514E0");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SuApiServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__SU_API_Se__Delet__50493919");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.SuApiServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_SU_API_Service");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuApiServices)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__SU_API_Se__Suppl__513D5D52");
            });

            modelBuilder.Entity<SuContact>(entity =>
            {
                entity.ToTable("SU_Contact");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.Property(e => e.ContactName)
                    .HasColumnName("Contact_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Fax).HasMaxLength(200);

                entity.Property(e => e.JobTitle).HasMaxLength(500);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.Mobile).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(200);

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuContacts)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_SU_CONTACT_CREATED_BY");

                entity.HasOne(d => d.PrimaryEntityNavigation)
                    .WithMany(p => p.SuContacts)
                    .HasForeignKey(d => d.PrimaryEntity)
                    .HasConstraintName("FK_SU_Contact_PrimaryEntity");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuContacts)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Contac__Suppl__6A132CBD");
            });

            modelBuilder.Entity<SuContactApiService>(entity =>
            {
                entity.ToTable("SU_Contact_API_Services");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.SuContactApiServices)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Contac__Conta__5AC6C78C");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuContactApiServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__SU_Contac__Creat__5CAF0FFE");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SuContactApiServiceDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__SU_Contac__Delet__5DA33437");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.SuContactApiServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_Contact_API_Service");
            });

            modelBuilder.Entity<SuContactEntityMap>(entity =>
            {
                entity.HasKey(e => new { e.ContactId, e.EntityId })
                    .HasName("PK__SU_Conta__95AEB7620AD36FCA");

                entity.ToTable("SU_Contact_Entity_Map");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.SuContactEntityMaps)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_Contact_Entity_Map_ContactId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SuContactEntityMaps)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_Contact_Entity_Map_EntityId");
            });

            modelBuilder.Entity<SuContactEntityServiceMap>(entity =>
            {
                entity.ToTable("SU_Contact_Entity_Service_Map");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.SuContactEntityServiceMaps)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_SU_Contact_Entity_Service_Map_ContactId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SuContactEntityServiceMaps)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_SU_Contact_Entity_Service_Map_EntityId");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.SuContactEntityServiceMaps)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_SU_Contact_Entity_Service_Map_ServiceId");
            });

            modelBuilder.Entity<SuCreditTerm>(entity =>
            {
                entity.ToTable("SU_CreditTerm");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuCreditTermCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__SU_Credit__Delet__5FB599DB");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SuCreditTermDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__SU_Credit__Delet__60A9BE14");
            });

            modelBuilder.Entity<SuEntity>(entity =>
            {
                entity.ToTable("SU_Entity");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuEntityCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Su_Entity_CreatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SuEntityDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("Su_Entity_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SuEntities)
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Su_Entity_EntityId");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuEntities)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Su_Entity_Supplierid");
            });

            modelBuilder.Entity<SuGrade>(entity =>
            {
                entity.ToTable("SU_Grade");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.LevelId).HasColumnName("Level_Id");

                entity.Property(e => e.PeriodFrom).HasColumnType("datetime");

                entity.Property(e => e.PeriodTo).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuGradeCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_SU_Grade_CreatedBy");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SuGrades)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_Grade_CustomerId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.SuGradeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_SU_Grade_UpdatedBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SuGradeDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_SU_Grade_DeletedBy");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SuGrades)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_SU_Grade_EntityId");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.SuGrades)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_Grade_Level_Id");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuGrades)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SU_Grade_SupplierId");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.SuGradeUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_SU_Grade_UpdatedBy");
            });

            modelBuilder.Entity<SuLevel>(entity =>
            {
                entity.ToTable("SU_Level");

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SuLevelCustom>(entity =>
            {
                entity.ToTable("SU_Level_Custom");

                entity.Property(e => e.CustomName).HasMaxLength(500);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SuLevelCustoms)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_SU_Level_Custom_CustomerId");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.SuLevelCustoms)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_SU_Level_Custom_LevelId");
            });

            modelBuilder.Entity<SuOwnlerShip>(entity =>
            {
                entity.ToTable("SU_OwnlerShip");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NameTranId).HasColumnName("Name_TranId");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SuOwnlerShips)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__SU_Ownler__Entit__6B0750F6");
            });

            modelBuilder.Entity<SuStatus>(entity =>
            {
                entity.ToTable("SU_Status");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuStatusCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__SU_Status__Delet__63862ABF");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SuStatusDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK__SU_Status__Delet__647A4EF8");
            });

            modelBuilder.Entity<SuSupplier>(entity =>
            {
                entity.ToTable("SU_Supplier");

                entity.Property(e => e.Comments).HasMaxLength(500);

                entity.Property(e => e.ContactPerson).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DailyProduction)
                    .HasColumnName("daily_production")
                    .HasMaxLength(50);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.Fax).HasMaxLength(500);

                entity.Property(e => e.FbFactSupId).HasColumnName("Fb_FactSup_Id");

                entity.Property(e => e.GlCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsEaqf).HasColumnName("IsEAQF");

                entity.Property(e => e.LegalName).HasMaxLength(500);

                entity.Property(e => e.LevelId).HasColumnName("Level_Id");

                entity.Property(e => e.LocalName).HasMaxLength(500);

                entity.Property(e => e.Mobile).HasMaxLength(500);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OwnerShipId).HasColumnName("OwnerShip_Id");

                entity.Property(e => e.Phone).HasMaxLength(500);

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasColumnName("Supplier_Name")
                    .HasMaxLength(500);

                entity.Property(e => e.TotalStaff)
                    .HasColumnName("total_staff")
                    .HasMaxLength(50);

                entity.Property(e => e.TypeId).HasColumnName("Type_id");

                entity.Property(e => e.Vatno)
                    .HasColumnName("VATNo")
                    .HasMaxLength(500);

                entity.Property(e => e.Website).HasMaxLength(500);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.SuSuppliers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__SU_Suppli__Compa__6AFDFAAC");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SuSupplierCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_SU_Supplier_IT_UserMaster");

                entity.HasOne(d => d.CreditTerm)
                    .WithMany(p => p.SuSuppliers)
                    .HasForeignKey(d => d.CreditTermId)
                    .HasConstraintName("FK_SU_CreditTerm");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.SuSupplierDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_IT_UserMaster_1");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.SuSuppliers)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK__SU_Suppli__Level__6BFB752F");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.SuSupplierModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_IT_UserMaster_2");

                entity.HasOne(d => d.OwnerShip)
                    .WithMany(p => p.SuSuppliers)
                    .HasForeignKey(d => d.OwnerShipId)
                    .HasConstraintName("FK__SU_Suppli__Owner__6DE3BDA1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SuSuppliers)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_SU_Status");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.SuSuppliers)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__SU_Suppli__Type___6CEF9968");
            });

            modelBuilder.Entity<SuSupplierCustomer>(entity =>
            {
                entity.HasKey(e => new { e.SupplierId, e.CustomerId })
                    .HasName("PK__SU_Suppl__0B5AA5D130643BC8");

                entity.ToTable("SU_Supplier_Customer");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.CreditTerm).HasColumnName("Credit_Term");

                entity.HasOne(d => d.CreditTermNavigation)
                    .WithMany(p => p.SuSupplierCustomers)
                    .HasForeignKey(d => d.CreditTerm)
                    .HasConstraintName("FK_SU_Supplier_Customer_Credit_Term");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SuSupplierCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Suppli__Custo__70C02A4C");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuSupplierCustomers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Suppli__Suppl__6FCC0613");
            });

            modelBuilder.Entity<SuSupplierCustomerContact>(entity =>
            {
                entity.HasKey(e => new { e.SupplierId, e.CustomerId, e.ContactId })
                    .HasName("PK__SU_Suppl__E7D80910BF81B384");

                entity.ToTable("SU_Supplier_Customer_Contact");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.ContactId).HasColumnName("Contact_Id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.SuSupplierCustomerContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Suppli__Conta__739C96F7");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SuSupplierCustomerContacts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Suppli__Custo__72A872BE");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuSupplierCustomerContacts)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Suppli__Suppl__71B44E85");
            });

            modelBuilder.Entity<SuSupplierFactory>(entity =>
            {
                entity.HasKey(e => new { e.ParentId, e.SupplierId })
                    .HasName("PK__SU_Suppl__80BFAE984BEE422D");

                entity.ToTable("SU_Supplier_Factory");

                entity.Property(e => e.ParentId).HasColumnName("Parent_Id");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.SuSupplierFactoryParents)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Suppli__Paren__7584DF69");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SuSupplierFactorySuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SU_Suppli__Suppl__7490BB30");
            });

            modelBuilder.Entity<SuType>(entity =>
            {
                entity.ToTable("SU_Type");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SuTypes)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK__SU_Type__EntityI__767903A2");
            });

            modelBuilder.Entity<TcfMasterDataLog>(entity =>
            {
                entity.ToTable("TCF_Master_DataLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.RequestUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<UgRole>(entity =>
            {
                entity.ToTable("UG_Role");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UgRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UG_ROLE_ID");

                entity.HasOne(d => d.UserGuide)
                    .WithMany(p => p.UgRoles)
                    .HasForeignKey(d => d.UserGuideId)
                    .HasConstraintName("FK_UG_USER_GUIDE");
            });

            modelBuilder.Entity<UgUserGuideDetail>(entity =>
            {
                entity.ToTable("UG_UserGuide_Details");

                entity.Property(e => e.FileUrl).HasMaxLength(500);

                entity.Property(e => e.IsCustomer).HasColumnName("Is_Customer");

                entity.Property(e => e.IsFactory).HasColumnName("Is_Factory");

                entity.Property(e => e.IsInternal).HasColumnName("Is_Internal");

                entity.Property(e => e.IsSupplier).HasColumnName("Is_Supplier");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.VideoUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<ZohoRequestLog>(entity =>
            {
                entity.ToTable("ZOHO_RequestLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.RequestUrl).HasMaxLength(500);
            });

            OnModelCreatingExt(modelBuilder);
        }

        partial void OnModelCreatingExt(ModelBuilder modelBuilder);
        public API_DBContext(DbContextOptions<API_DBContext> options)
        : base(options)
        {

        }
    }
}