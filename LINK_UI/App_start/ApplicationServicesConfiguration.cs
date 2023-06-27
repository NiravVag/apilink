using BI;
using BI.Cache;
using BI.Maps;
using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DAL.Repositories;
using DTO.Common;
using EmailComponent;
using Entities;
using Entities.Enums;
using FileGenerationComponent;
using LINK_UI.FileModels;
using LoggerComponent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LINK_UI.App_start
{
    /// <summary>
    /// This class is used to manage dependancy injection
    /// </summary>
    public static class ApplicationServicesConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            // Repositories
            ConfigureRepositories(services);

            // Managers
            ConfigureManagers(services);

        }

        public static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IHumanResourceRepository, HumanResourceRespository>();
            services.AddTransient<ITranslationRepository, TranslationRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerContactRepository, CustomerContactRepository>();
            services.AddTransient<ICustomerDepartmentRepository, CustomerDepartmentRepository>();
            services.AddTransient<ICustomerBrandRepository, CustomerBrandRepository>();
            services.AddTransient<ICustomerServiceConfigRepository, CustomerServiceConfigRepository>();
            services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddTransient<IReferenceRepository, ReferenceRepository>();
            services.AddTransient<IAuditRepository, AuditRepository>();
            services.AddTransient<IAuditDashboardRepository, AuditDashboardRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IOfficeLocationRepository, OfficeLocationRepository>();
            services.AddTransient<ICustomerProductRepository, CustomerProductRepository>();
            services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddTransient<IProductManagementRepository, ProductManagementRepository>();
            services.AddTransient<IInspectionBookingRepository, InspectionBookingRepository>();
            services.AddTransient<ICustomerBuyerRepository, CustomerBuyerRepository>();
            services.AddTransient<IUserAccountRepository, UserAccountRepository>();
            services.AddTransient<ICombineOrdersRepository, CombineOrdersRepository>();
            services.AddTransient<ICSConfigRepository, CSConfigRepository>();
            services.AddTransient<ILabRepository, LabRepository>();
            services.AddTransient<IInspectionPickingRepository, InspectionPickingRepository>();
            services.AddTransient<IQuotationRepository, QuotationRepository>();
            services.AddTransient<ICancelBookingRepository, CancelBookingRepository>();
            services.AddTransient<IInspBookingRuleContactRepository, InspBookingRuleContactRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IEventBookingLogRepository, EventBookingLogRepository>();
            services.AddTransient<ICustomerCheckPointRepository, CustomerCheckPointRepository>();
            services.AddTransient<IScheduleRepository, ScheduleRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IEmailScheduleRepository, EmailScheduleRepository>();
            services.AddTransient<IFullBridgeRepository, FullBridgeRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IInspectionCertificateRepository, InspectionCertificateRepository>();
            services.AddTransient<IFBInternalReportRepository, FBInternalReportRepository>();
            services.AddTransient<IAuditCusReportRepository, AuditCusReportRepository>();
            services.AddTransient<IInspectionCustomerDecisionRepository, InspectionCustomerDecisionRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<IDynamicFieldRepository, DynamicFieldRepository>();
            services.AddTransient<IEmailLogQueueRepository, EmailLogQueueRepository>();
            services.AddTransient<IUserConfigRepository, UserConfigRepository>();
            services.AddTransient<IKpiRepository, KpiRepository>();
            services.AddTransient<ICustomerPriceCardRepository, CustomerPriceCardRepository>();
            services.AddTransient<IKpiCustomRepository, KpiCustomRepository>();
            services.AddTransient<ITravelMatrixRepository, TravelMatrixRepository>();
            services.AddTransient<IMandayRepository, MandayRepository>();
            services.AddTransient<ITCFRepository, TCFRepository>();
            services.AddTransient<IInvoiceBankRepository, InvoiceBankRepository>();
            services.AddTransient<IUtilizationDashboardRepository, UtilizationDashboardRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<ISaleInvoiceRepository, SaleInvoiceRepository>();
            services.AddTransient<ICustomerCollectionRepository, CustomerCollectionRepository>();
            services.AddTransient<IInvoicePreivewRepository, InvoicePreivewRepository>();
            services.AddTransient<IExtraFeesRepository, ExtraFeesRepository>();
            services.AddTransient<IQcDashboardRepository, QcDashboardRepository>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<IQCBlockRepository, QCBlockRepository>();
            services.AddTransient<ISupFactDashboardRepository, SupFactDashboardRepository>();
            services.AddTransient<IEmailSendingDetailsRepository, EmailSendingDetailsRepository>();
            services.AddTransient<IEmailSubjectRepository, EmailSubjectRepository>();
            services.AddTransient<IManagementDashboardRepository, ManagementDashboardRepository>();
            services.AddTransient<IDefectDashboardRepository, DefectDashboardRepository>();
            services.AddTransient<IQuantitativeDashboardRepository, QuantitativeDashboardRepository>();
            services.AddTransient<IEmailConfigurationRepository, EmailConfigRepository>();
            services.AddTransient<IRejectionDashboardRepository, RejectionDashboardRepository>();
            services.AddTransient<IEmailSendRepository, EmailSendRepository>();
            services.AddTransient<IFinanceDashboardRepository, FinanceDashboardRepository>();
            services.AddTransient<ICSDashboardRepository, CSDashboardRepository>();
            services.AddTransient<ISharedInspectionRepo, SharedInspectionRepo>();
            services.AddTransient<ISharedFullBridgeRepository, SharedFullBridgeRepository>();
            services.AddTransient<ICustomerComplaintRepository, CustomerComplaintRepository>();
            services.AddTransient<IInvoiceStatusRepository, InvoiceStatusRepository>();
            services.AddTransient<IGlobalFilterRepository, GlobalFilterRepository>();
            services.AddTransient<ITravelTariffRepository, TravelTariffRepository>();
            services.AddTransient<IFoodAllowanceRepository, FoodAllowanceRepository>();
            services.AddTransient<IScheduleJobRepository, ScheduleJobRepository>();

            services.AddTransient<IStartingPortRepository, StartingPortRepository>();
            services.AddTransient<IDataManagementRepository, DataManagementRepository>();
            services.AddTransient<IWorkLoadMatrixRepository, WorkLoadMatrixRepository>();
            services.AddTransient<IOtherMandayRepository, OtherMandayRepository>();
            services.AddTransient<IManualInvoiceRepository, ManualInvoiceRepository>();
            services.AddTransient<IClaimRepository, ClaimRepository>();
            services.AddTransient<IInspectionCustomReportRepository, InspectionCustomReportRepository>();
            services.AddTransient<IInvoiceDataAccessRepository, InvoiceDataAccessRepository>();
            services.AddTransient<IBookingFbQueueRepository, BookingFbQueueRepository>();
        }

        public static void ConfigureManagers(IServiceCollection services)
        {
            services.AddTransient<IAPIUserContext, APIUserContext>();
            services.AddTransient<IUserRightsManager, UserRightsManager>();
            services.AddTransient<ICacheManager, CacheManager>();
            services.AddTransient<IHumanResourceManager, HumanResourceManager>();
            services.AddTransient<ILocationManager, LocationManager>();
            services.AddTransient<ITranslationManager, TranslationManager>();
            services.AddTransient<ISupplierManager, SupplierManager>();
            services.AddTransient<ICustomerManager, CustomerManager>();
            services.AddTransient<ICustomerContactManager, CustomerContactManager>();
            services.AddTransient<ICustomerDepartmentManager, CustomerDepartmentManager>();
            services.AddTransient<ICustomerServiceConfigManager, CustomerServiceConfigManager>();
            services.AddTransient<IExchangeRateManager, ExchangeRateManager>();
            services.AddTransient<IReferenceManager, ReferenceManager>();
            services.AddTransient<IAuditManager, AuditManager>();
            services.AddTransient<IAuditDashboardManager, AuditDashboardManager>();
            services.AddTransient<IExpenseManager, ExpenseManager>();
            services.AddTransient<ICustomerBrandManager, CustomerBrandManager>();
            services.AddTransient<ICustomerProductManager, CustomerProductManager>();
            services.AddTransient<IPurchaseOrderManager, PurchaseOrderManager>();
            services.AddTransient<IInspectionBookingManager, InspectionBookingManager>();
            services.AddTransient<ICombineOrdersManager, CombineOrdersManager>();
            services.AddTransient<IEmailsManager, EmailsManager>();
            services.AddTransient<IQuotationManager, QuotationManager>();
            services.AddTransient<ITCFManager, TCFManager>();
            services.AddTransient<IFileManager, FileManager>((provider) =>
            {
                var environnment = provider.GetService<IHostingEnvironment>();

                return new FileManager(environnment.ContentRootPath, provider.GetService<IConfiguration>());
            });

            services.AddTransient<IAPILogger, APILogger>((provider) =>
            {
                return new APILogger(provider.GetService<IConfiguration>());
            });

            services.AddTransient<IOfficeLocationManager, OfficeLocationManager>();
            services.AddTransient<IProductManagementManager, ProductManagementManager>();
            services.AddTransient<IInspectionBookingManager, InspectionBookingManager>();
            services.AddTransient<IEmailManager, EmailManager>();
            services.AddTransient<ICustomerBuyerManager, CustomerBuyerManager>();
            services.AddTransient<IUserAccountManager, UserAccountManager>();
            services.AddTransient<IRoleRightManager, RoleRightManager>();
            services.AddTransient<ILabManager, LabManager>();
            services.AddTransient<ICSConfigManager, CSConfigManager>();
            services.AddTransient<ICancelBookingManager, CancelBookingManager>();
            services.AddTransient<IInspectionPickingManager, InspectionPickingManager>();
            services.AddTransient<IInspBookingRuleContactManager, InspBookingRuleContactManager>();
            services.AddTransient<IFBReportManager, FBReportManager>();
            services.AddTransient<IEventBookingLogManager, EventBookingLogManager>();
            services.AddTransient<ICustomerCheckPointManager, CustomerCheckPointManager>();
            services.AddTransient<IQuotationPDF, QuotationPreview>();

            services.AddTransient<IScheduleManager, ScheduleManager>();
            services.AddTransient<IHttpClientProviders, HttpClientProviders>();
            services.AddTransient<IHelper, Helper>();
            services.AddTransient<IDocumentManager, DocumentManager>();
            services.AddTransient<IEmailScheduleManager, EmailSchedulerManager>();
            services.AddTransient<IFullBridgeManager, FullBridgeManager>();
            services.AddTransient<INotificationManager, NotificationManager>();
            services.AddTransient<IReportManager, ReportManager>();
            services.AddTransient<IInspectionCertificateManager, InspectionCertificateManager>();
            services.AddTransient<IPDF, InspectionCertificateModelPDF>();
            services.AddTransient<IFBInternalReportManager, FBInternalReportManager>();
            services.AddTransient<IQCInspectionDetailPDF, QCInspectionDetailPDF>();
            services.AddTransient<IAuditCusReportManager, AuditCusReportManager>();
            services.AddTransient<IPickingPDF, InspectionPickingPDF>();
            services.AddTransient<IDashboardManager, DashboardManager>();
            services.AddTransient<IInspectionCustomerDecisionManager, InspectionCustomerDecisionManager>();
            services.AddTransient<IDynamicFieldManager, DynamicFieldManager>();

            services.AddTransient<IEmailLogQueueManager, EmailLogQueueManager>();
            services.AddTransient<IUserConfigManager, UserConfigManager>();

            services.AddTransient<IKpiManager, KpiManager>();

            services.AddTransient<ICustomerPriceCardManager, CustomerPriceCardManager>();
            services.AddTransient<IKpiCustomManager, KpiCustomManager>();
            services.AddTransient<ITravelMatrixManager, TravelMatrixManager>();
            services.AddTransient<IMandayManager, MandayManager>();
            services.AddTransient<IInvoiceBankManager, InvoiceBankManager>();
            services.AddTransient<IUtilizationDashboardManager, UtilizationDashboardManager>();
            services.AddTransient<IInvoiceManager, InvoiceManager>();
            services.AddTransient<ISaleInvoiceManager, SaleInvoiceManager>();
            services.AddTransient<ICustomerCollectionManager, CustomerCollectionManager>();
            services.AddTransient<IInvoicePreviewManager, InvoicePreviewManager>();
            services.AddTransient<IAPIGatewayManager, APIGatewayManager>();

            services.AddTransient<IExtraFeesManager, ExtraFeesManager>();
            services.AddTransient<IQcDashboardManager, QcDashboardManager>();
            services.AddTransient<IUserProfileManager, UserProfileManager>();
            services.AddTransient<IQCBlockManager, QCBlockManager>();
            services.AddTransient<ISupFactDashboardManager, SupFactDashboardManager>();
            services.AddTransient<IEmailSendingDetailsManager, EmailSendingDetailsManager>();
            services.AddTransient<IEmailSubjectManager, EmailSubjectManager>();
            services.AddTransient<IManagementDashboardManager, ManagementDashboardManager>();
            services.AddTransient<IDefectDashboardManager, DefectDashboardManager>();
            services.AddTransient<IQuantitativeDashboardManager, QuantitativeDashboardManager>();
            services.AddTransient<IEmailConfigurationManager, EmailConfigManager>();
            services.AddTransient<IRejectionDashboardManager, RejectionDashboardManager>();
            services.AddTransient<IEmailSendManager, EmailSendManager>();
            services.AddTransient<ITestManager, TestManager>();
            services.AddTransient<IFinanceDashboardManager, FinanceDashboardManager>();
            services.AddTransient<ICSDashboardManager, CSDashboardManager>();
            services.AddTransient<IUserGuideManager, UserGuideManager>();
            services.AddTransient<IUserGuideRepository, UserGuideRepository>();
            services.AddTransient<ISharedInspectionManager, SharedInspectionManager>();
            services.AddTransient<ISharedFullBridgeManager, SharedFullBridgeManager>();
            services.AddTransient<ICustomerComplaintManager, CustomerComplaintManager>();
            services.AddTransient<IInvoiceStatusManager, InvoiceStatusManager>();
            services.AddTransient<IInspectionCustomReportManager, InspectionCustomReportManager>();
            services.AddTransient<ITenantProvider, TenantProvider>();
            services.AddTransient<ITravelTariffManager, TravelTariffManager>();
            services.AddTransient<IFoodAllowanceManager, FoodAllowanceManager>();
            services.AddTransient<IScheduleJobManager, ScheduleJobManager>();
            services.AddTransient<IStartingPortManager, StartingPortManager>();
            services.AddTransient<IDataManagementManager, DataManagementManager>();
            services.AddTransient<IWorkLoadMatrixManager, WorkLoadMatrixManager>();
            services.AddTransient<IOtherMandayManager, OtherMandayManager>();

            services.AddTransient<IInvoiceDiscountManager, InvoiceDiscountManager>();
            services.AddTransient<IInvoiceDiscountRepository, InvoiceDiscountRepository>();

            services.AddTransient<IManualInvoiceManager, ManualInvoiceManager>();
            services.AddTransient<IClaimManager, ClaimManager>();

            services.AddTransient<IReportFastTransactionRepository, ReportFastTransactionRepository>();

            services.AddTransient<IInvoiceDataAccessManager, InvoiceDataAccessManager>();
            services.AddTransient<IEaqfEventUpdateManager, EaqfEventUpdateManager>();
            
            services.AddTransient<IBookingEmailLogQueueManager, BookingEmailLogQueueManager>();
        }
        public static T GetClaim<T>(ClaimsPrincipal user, string key)
        {
            var elements = user.Claims.Where(x => x.Type == key)
                .Select(x => x.Value);

            if (elements == null || !elements.Any())
                return default(T);

            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), elements.First(), true);

            //if (typeof(T).GetTypeInfo().IsEnum)
            //    return (T)Enum.Parse(typeof(T), elements.First());
            return (T)Convert.ChangeType(elements.First(), typeof(T));

        }

    }
}

