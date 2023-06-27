using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AP_Entity")]
    public partial class ApEntity
    {
        public ApEntity()
        {
            AudBookingContacts = new HashSet<AudBookingContact>();
            AudBookingEmailConfigurations = new HashSet<AudBookingEmailConfiguration>();
            AudBookingRules = new HashSet<AudBookingRule>();
            AudCancelRescheduleReasons = new HashSet<AudCancelRescheduleReason>();
            AudCuProductCategories = new HashSet<AudCuProductCategory>();
            AudEvaluationRounds = new HashSet<AudEvaluationRound>();
            AudStatuses = new HashSet<AudStatus>();
            AudTranStatusLogs = new HashSet<AudTranStatusLog>();
            AudTransactions = new HashSet<AudTransaction>();
            AudTypes = new HashSet<AudType>();
            AudWorkProcesses = new HashSet<AudWorkProcess>();
            ClmTranAttachments = new HashSet<ClmTranAttachment>();
            ClmTransactions = new HashSet<ClmTransaction>();
            CuAddresses = new HashSet<CuAddress>();
            CuApiServices = new HashSet<CuApiService>();
            CuBrands = new HashSet<CuBrand>();
            CuBuyerApiServices = new HashSet<CuBuyerApiService>();
            CuBuyers = new HashSet<CuBuyer>();
            CuCheckPointTypes = new HashSet<CuCheckPointType>();
            CuCheckPoints = new HashSet<CuCheckPoint>();
            CuCheckPointsBrands = new HashSet<CuCheckPointsBrand>();
            CuCheckPointsCountries = new HashSet<CuCheckPointsCountry>();
            CuCheckPointsDepartments = new HashSet<CuCheckPointsDepartment>();
            CuCheckPointsServiceTypes = new HashSet<CuCheckPointsServiceType>();
            CuCollections = new HashSet<CuCollection>();
            CuContactEntityMaps = new HashSet<CuContactEntityMap>();
            CuContactEntityServiceMaps = new HashSet<CuContactEntityServiceMap>();
            CuContactSisterCompanies = new HashSet<CuContactSisterCompany>();
            CuContactTypes = new HashSet<CuContactType>();
            CuContacts = new HashSet<CuContact>();
            CuCsConfigurations = new HashSet<CuCsConfiguration>();
            CuCustomerGroups = new HashSet<CuCustomerGroup>();
            CuCustomers = new HashSet<CuCustomer>();
            CuDepartments = new HashSet<CuDepartment>();
            CuEntities = new HashSet<CuEntity>();
            CuPrCities = new HashSet<CuPrCity>();
            CuPrDetails = new HashSet<CuPrDetail>();
            CuPriceCategories = new HashSet<CuPriceCategory>();
            CuProductCategories = new HashSet<CuProductCategory>();
            CuProductFileAttachments = new HashSet<CuProductFileAttachment>();
            CuProductTypes = new HashSet<CuProductType>();
            CuProducts = new HashSet<CuProduct>();
            CuPurchaseOrderDetails = new HashSet<CuPurchaseOrderDetail>();
            CuPurchaseOrders = new HashSet<CuPurchaseOrder>();
            CuSeasonConfigs = new HashSet<CuSeasonConfig>();
            CuSeasons = new HashSet<CuSeason>();
            CuServiceTypes = new HashSet<CuServiceType>();
            CuSisterCompanies = new HashSet<CuSisterCompany>();
            DaUserByBrands = new HashSet<DaUserByBrand>();
            DaUserByDepartments = new HashSet<DaUserByDepartment>();
            DaUserByFactoryCountries = new HashSet<DaUserByFactoryCountry>();
            DaUserByProductCategories = new HashSet<DaUserByProductCategory>();
            DaUserByRoles = new HashSet<DaUserByRole>();
            DaUserByServices = new HashSet<DaUserByService>();
            DaUserCustomers = new HashSet<DaUserCustomer>();
            DaUserRoleNotificationByOffices = new HashSet<DaUserRoleNotificationByOffice>();
            DfCuConfigurations = new HashSet<DfCuConfiguration>();
            DmDetails = new HashSet<DmDetail>();
            DmFiles = new HashSet<DmFile>();
            DmRefModules = new HashSet<DmRefModule>();
            DmRights = new HashSet<DmRight>();
            DmRoles = new HashSet<DmRole>();
            EcAutRefStartPorts = new HashSet<EcAutRefStartPort>();
            EcAutTravelTariffs = new HashSet<EcAutTravelTariff>();
            EcExpClaimStatuses = new HashSet<EcExpClaimStatus>();
            EcExpencesClaims = new HashSet<EcExpencesClaim>();
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
            EcExpensesTypes = new HashSet<EcExpensesType>();
            EcFoodAllowances = new HashSet<EcFoodAllowance>();
            EcPaymenTypes = new HashSet<EcPaymenType>();
            EmExchangeRateTypes = new HashSet<EmExchangeRateType>();
            EmExchangeRates = new HashSet<EmExchangeRate>();
            EntFeatureDetails = new HashSet<EntFeatureDetail>();
            EntPagesFields = new HashSet<EntPagesField>();
            EsApiDefaultContacts = new HashSet<EsApiDefaultContact>();
            EsDetails = new HashSet<EsDetail>();
            EsRefReportSendTypes = new HashSet<EsRefReportSendType>();
            EsRefSpecialRules = new HashSet<EsRefSpecialRule>();
            EsSuPreDefinedFields = new HashSet<EsSuPreDefinedField>();
            EsSuTemplateMasters = new HashSet<EsSuTemplateMaster>();
            EsTranFiles = new HashSet<EsTranFile>();
            EventBookingLogs = new HashSet<EventBookingLog>();
            FbBookingRequestLogs = new HashSet<FbBookingRequestLog>();
            FbReportManualLogs = new HashSet<FbReportManualLog>();
            FbReportTemplates = new HashSet<FbReportTemplate>();
            HrDepartments = new HashSet<HrDepartment>();
            HrEmployeeTypes = new HashSet<HrEmployeeType>();
            HrEntityMaps = new HashSet<HrEntityMap>();
            HrFileTypes = new HashSet<HrFileType>();
            HrHolidays = new HashSet<HrHoliday>();
            HrLeaveStatuses = new HashSet<HrLeaveStatus>();
            HrLeaveTypes = new HashSet<HrLeaveType>();
            HrLeaves = new HashSet<HrLeave>();
            HrOfficeControls = new HashSet<HrOfficeControl>();
            HrOutSourceCompanies = new HashSet<HrOutSourceCompany>();
            HrPayrollCompanies = new HashSet<HrPayrollCompany>();
            HrPositions = new HashSet<HrPosition>();
            HrProfiles = new HashSet<HrProfile>();
            HrQualifications = new HashSet<HrQualification>();
            HrRenews = new HashSet<HrRenew>();
            HrStaffCompanies = new HashSet<HrStaff>();
            HrStaffEntityServiceMaps = new HashSet<HrStaffEntityServiceMap>();
            HrStaffPrimaryEntityNavigations = new HashSet<HrStaff>();
            InspBookingEmailConfigurations = new HashSet<InspBookingEmailConfiguration>();
            InspBookingRules = new HashSet<InspBookingRule>();
            InspCancelReasons = new HashSet<InspCancelReason>();
            InspContainerTransactions = new HashSet<InspContainerTransaction>();
            InspIcTransactions = new HashSet<InspIcTransaction>();
            InspLabAddressTypes = new HashSet<InspLabAddressType>();
            InspLabDetails = new HashSet<InspLabDetail>();
            InspLabTypes = new HashSet<InspLabType>();
            InspProductTransactions = new HashSet<InspProductTransaction>();
            InspPurchaseOrderColorTransactions = new HashSet<InspPurchaseOrderColorTransaction>();
            InspPurchaseOrderTransactions = new HashSet<InspPurchaseOrderTransaction>();
            InspRepCusDecisionTemplates = new HashSet<InspRepCusDecisionTemplate>();
            InspRescheduleReasons = new HashSet<InspRescheduleReason>();
            InspStatuses = new HashSet<InspStatus>();
            InspTranStatusLogs = new HashSet<InspTranStatusLog>();
            InspTransactionDrafts = new HashSet<InspTransactionDraft>();
            InspTransactions = new HashSet<InspTransaction>();
            InvAutTranCommunications = new HashSet<InvAutTranCommunication>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvAutTranStatusLogs = new HashSet<InvAutTranStatusLog>();
            InvCreTransactions = new HashSet<InvCreTransaction>();
            InvDaTransactions = new HashSet<InvDaTransaction>();
            InvDisTranDetails = new HashSet<InvDisTranDetail>();
            InvExfTranStatusLogs = new HashSet<InvExfTranStatusLog>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            InvRefOffices = new HashSet<InvRefOffice>();
            InvTmDetails = new HashSet<InvTmDetail>();
            InvTranInvoiceRequestContacts = new HashSet<InvTranInvoiceRequestContact>();
            ItRightEntities = new HashSet<ItRightEntity>();
            ItRights = new HashSet<ItRight>();
            ItRoles = new HashSet<ItRole>();
            ItUserRoles = new HashSet<ItUserRole>();
            JobScheduleConfigurations = new HashSet<JobScheduleConfiguration>();
            JobScheduleLogs = new HashSet<JobScheduleLog>();
            LogBookingFbQueues = new HashSet<LogBookingFbQueue>();
            LogBookingReportEmailQueues = new HashSet<LogBookingReportEmailQueue>();
            LogEmailQueueAttachments = new HashSet<LogEmailQueueAttachment>();
            LogEmailQueues = new HashSet<LogEmailQueue>();
            MidNotificationTypes = new HashSet<MidNotificationType>();
            MidNotifications = new HashSet<MidNotification>();
            MidTaskTypes = new HashSet<MidTaskType>();
            MidTasks = new HashSet<MidTask>();
            OmDetails = new HashSet<OmDetail>();
            QcBlockLists = new HashSet<QcBlockList>();
            QuQuotations = new HashSet<QuQuotation>();
            QuTranStatusLogs = new HashSet<QuTranStatusLog>();
            QuWorkLoadMatrices = new HashSet<QuWorkLoadMatrix>();
            RefAddressTypes = new HashSet<RefAddressType>();
            RefBillingEntities = new HashSet<RefBillingEntity>();
            RefBudgetForecasts = new HashSet<RefBudgetForecast>();
            RefBusinessTypes = new HashSet<RefBusinessType>();
            RefCityDetails = new HashSet<RefCityDetail>();
            RefDefectClassifications = new HashSet<RefDefectClassification>();
            RefExpertises = new HashSet<RefExpertise>();
            RefInvoiceTypes = new HashSet<RefInvoiceType>();
            RefLocationTypes = new HashSet<RefLocationType>();
            RefLocations = new HashSet<RefLocation>();
            RefMarketSegments = new HashSet<RefMarketSegment>();
            RefProductCategories = new HashSet<RefProductCategory>();
            RefProductCategorySub2S = new HashSet<RefProductCategorySub2>();
            RefProductCategorySub3S = new HashSet<RefProductCategorySub3>();
            RefProductCategorySubs = new HashSet<RefProductCategorySub>();
            RefProductUnits = new HashSet<RefProductUnit>();
            RefSeasons = new HashSet<RefSeason>();
            RefServiceTypeXeros = new HashSet<RefServiceTypeXero>();
            RefServiceTypes = new HashSet<RefServiceType>();
            RefServices = new HashSet<RefService>();
            RefUnits = new HashSet<RefUnit>();
            RefZones = new HashSet<RefZone>();
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
            RestApiLogs = new HashSet<RestApiLog>();
            SuAddressTypes = new HashSet<SuAddressType>();
            SuContactEntityMaps = new HashSet<SuContactEntityMap>();
            SuContactEntityServiceMaps = new HashSet<SuContactEntityServiceMap>();
            SuContacts = new HashSet<SuContact>();
            SuEntities = new HashSet<SuEntity>();
            SuGrades = new HashSet<SuGrade>();
            SuOwnlerShips = new HashSet<SuOwnlerShip>();
            SuSuppliers = new HashSet<SuSupplier>();
            SuTypes = new HashSet<SuType>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        [Column("FB_ID")]
        public int? FbId { get; set; }

        [InverseProperty("Entity")]
        public virtual ICollection<AudBookingContact> AudBookingContacts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudBookingEmailConfiguration> AudBookingEmailConfigurations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudBookingRule> AudBookingRules { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudCancelRescheduleReason> AudCancelRescheduleReasons { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudCuProductCategory> AudCuProductCategories { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudEvaluationRound> AudEvaluationRounds { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudStatus> AudStatuses { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudTranStatusLog> AudTranStatusLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudType> AudTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<AudWorkProcess> AudWorkProcesses { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<ClmTranAttachment> ClmTranAttachments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<ClmTransaction> ClmTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuAddress> CuAddresses { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuApiService> CuApiServices { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuBrand> CuBrands { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuBuyerApiService> CuBuyerApiServices { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuBuyer> CuBuyers { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCheckPointType> CuCheckPointTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCheckPoint> CuCheckPoints { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCheckPointsBrand> CuCheckPointsBrands { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCheckPointsCountry> CuCheckPointsCountries { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCheckPointsDepartment> CuCheckPointsDepartments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCheckPointsServiceType> CuCheckPointsServiceTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCollection> CuCollections { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuContactEntityMap> CuContactEntityMaps { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuContactEntityServiceMap> CuContactEntityServiceMaps { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuContactSisterCompany> CuContactSisterCompanies { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuContactType> CuContactTypes { get; set; }
        [InverseProperty("PrimaryEntityNavigation")]
        public virtual ICollection<CuContact> CuContacts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCsConfiguration> CuCsConfigurations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuCustomerGroup> CuCustomerGroups { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuDepartment> CuDepartments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuEntity> CuEntities { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuPrCity> CuPrCities { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuPriceCategory> CuPriceCategories { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuProductCategory> CuProductCategories { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuProductFileAttachment> CuProductFileAttachments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuProductType> CuProductTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuProduct> CuProducts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuPurchaseOrderDetail> CuPurchaseOrderDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrders { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuSeasonConfig> CuSeasonConfigs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuSeason> CuSeasons { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<CuSisterCompany> CuSisterCompanies { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserByBrand> DaUserByBrands { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserByDepartment> DaUserByDepartments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserByFactoryCountry> DaUserByFactoryCountries { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserByProductCategory> DaUserByProductCategories { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserByRole> DaUserByRoles { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserByService> DaUserByServices { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserCustomer> DaUserCustomers { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DaUserRoleNotificationByOffice> DaUserRoleNotificationByOffices { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DmDetail> DmDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DmFile> DmFiles { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DmRefModule> DmRefModules { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DmRight> DmRights { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<DmRole> DmRoles { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcAutRefStartPort> EcAutRefStartPorts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcAutTravelTariff> EcAutTravelTariffs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcExpClaimStatus> EcExpClaimStatuses { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaims { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcExpensesType> EcExpensesTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcFoodAllowance> EcFoodAllowances { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EcPaymenType> EcPaymenTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EmExchangeRateType> EmExchangeRateTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EmExchangeRate> EmExchangeRates { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EntFeatureDetail> EntFeatureDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EntPagesField> EntPagesFields { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EsApiDefaultContact> EsApiDefaultContacts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EsRefReportSendType> EsRefReportSendTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EsRefSpecialRule> EsRefSpecialRules { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EsSuPreDefinedField> EsSuPreDefinedFields { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasters { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EsTranFile> EsTranFiles { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<EventBookingLog> EventBookingLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<FbBookingRequestLog> FbBookingRequestLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<FbReportManualLog> FbReportManualLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<FbReportTemplate> FbReportTemplates { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrDepartment> HrDepartments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrEmployeeType> HrEmployeeTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrEntityMap> HrEntityMaps { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrFileType> HrFileTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrHoliday> HrHolidays { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrLeaveStatus> HrLeaveStatuses { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrLeaveType> HrLeaveTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrLeave> HrLeaves { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrOfficeControl> HrOfficeControls { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrOutSourceCompany> HrOutSourceCompanies { get; set; }
        [InverseProperty("EntityNavigation")]
        public virtual ICollection<HrPayrollCompany> HrPayrollCompanies { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrPosition> HrPositions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrProfile> HrProfiles { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrQualification> HrQualifications { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrRenew> HrRenews { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<HrStaff> HrStaffCompanies { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<HrStaffEntityServiceMap> HrStaffEntityServiceMaps { get; set; }
        [InverseProperty("PrimaryEntityNavigation")]
        public virtual ICollection<HrStaff> HrStaffPrimaryEntityNavigations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspBookingEmailConfiguration> InspBookingEmailConfigurations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspBookingRule> InspBookingRules { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspCancelReason> InspCancelReasons { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspContainerTransaction> InspContainerTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspIcTransaction> InspIcTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspLabAddressType> InspLabAddressTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspLabDetail> InspLabDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspLabType> InspLabTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspPurchaseOrderColorTransaction> InspPurchaseOrderColorTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspRepCusDecisionTemplate> InspRepCusDecisionTemplates { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspRescheduleReason> InspRescheduleReasons { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspStatus> InspStatuses { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspTranStatusLog> InspTranStatusLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDrafts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvAutTranCommunication> InvAutTranCommunications { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvAutTranStatusLog> InvAutTranStatusLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvCreTransaction> InvCreTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvDaTransaction> InvDaTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvDisTranDetail> InvDisTranDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvExfTranStatusLog> InvExfTranStatusLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvRefOffice> InvRefOffices { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvTmDetail> InvTmDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<InvTranInvoiceRequestContact> InvTranInvoiceRequestContacts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<ItRightEntity> ItRightEntities { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<ItRight> ItRights { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<ItRole> ItRoles { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<ItUserRole> ItUserRoles { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<JobScheduleConfiguration> JobScheduleConfigurations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<JobScheduleLog> JobScheduleLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<LogBookingFbQueue> LogBookingFbQueues { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<LogBookingReportEmailQueue> LogBookingReportEmailQueues { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<LogEmailQueueAttachment> LogEmailQueueAttachments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<LogEmailQueue> LogEmailQueues { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<MidNotificationType> MidNotificationTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<MidNotification> MidNotifications { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<MidTaskType> MidTaskTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<MidTask> MidTasks { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<OmDetail> OmDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<QcBlockList> QcBlockLists { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<QuTranStatusLog> QuTranStatusLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<QuWorkLoadMatrix> QuWorkLoadMatrices { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefAddressType> RefAddressTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefBillingEntity> RefBillingEntities { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefBudgetForecast> RefBudgetForecasts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefBusinessType> RefBusinessTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefCityDetail> RefCityDetails { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefDefectClassification> RefDefectClassifications { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefExpertise> RefExpertises { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefInvoiceType> RefInvoiceTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefLocationType> RefLocationTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefLocation> RefLocations { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefMarketSegment> RefMarketSegments { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefProductCategory> RefProductCategories { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefProductCategorySub2> RefProductCategorySub2S { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefProductCategorySub3> RefProductCategorySub3S { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefProductCategorySub> RefProductCategorySubs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefProductUnit> RefProductUnits { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefSeason> RefSeasons { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefServiceTypeXero> RefServiceTypeXeros { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefServiceType> RefServiceTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefService> RefServices { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefUnit> RefUnits { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RefZone> RefZones { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<RestApiLog> RestApiLogs { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<SuAddressType> SuAddressTypes { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<SuContactEntityMap> SuContactEntityMaps { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<SuContactEntityServiceMap> SuContactEntityServiceMaps { get; set; }
        [InverseProperty("PrimaryEntityNavigation")]
        public virtual ICollection<SuContact> SuContacts { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<SuEntity> SuEntities { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<SuGrade> SuGrades { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<SuOwnlerShip> SuOwnlerShips { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<SuSupplier> SuSuppliers { get; set; }
        [InverseProperty("Entity")]
        public virtual ICollection<SuType> SuTypes { get; set; }
    }
}