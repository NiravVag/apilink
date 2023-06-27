using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.DynamicFields;
using DTO.EventBookingLog;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.RepoRequest.Audit;
using DTO.Report;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class FBReportManager : ApiCommonData, IFBReportManager
    {

        private readonly IInspectionBookingRepository _bookingRepo = null;
        private readonly IHelper _helper = null;
        private readonly FBSettings _fbSettings = null;
        private readonly ICustomerRepository _customerRepo = null;
        private readonly ISupplierRepository _supplierRepo = null;

        private readonly ICustomerProductRepository _productRepo = null;
        private readonly IHumanResourceRepository _humanRepository = null;
        private readonly IFullBridgeManager _fbReportManager = null;
        private readonly IScheduleRepository _schRepo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ILogger _logger = null;
        private readonly IDynamicFieldManager _dynamicFieldManager = null;
        private readonly IEventBookingLogManager _fbLog = null;
        private readonly IFullBridgeManager _fbManager = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IAuditRepository _auditRepository = null;

        public FBReportManager(IInspectionBookingRepository bookingRepo,
            IHelper helper,
            ICustomerRepository customerRepo,
            ISupplierRepository supplierRepo,
            ICustomerProductRepository productRepo,
            IHumanResourceRepository humanRepository,
            IEventBookingLogManager fbLog,
            IFullBridgeManager fbReportManager,
            IScheduleRepository schRepo,
            IAPIUserContext ApplicationContext,
            ILogger<FBReportManager> logger,
            IOptions<FBSettings> fbSettings,
            IFullBridgeManager fbManager,
            IDynamicFieldManager dynamicFieldManager,
            IAuditRepository auditRepository,
            ITenantProvider filterService = null)
        {
            _bookingRepo = bookingRepo;
            _helper = helper;
            _customerRepo = customerRepo;
            _supplierRepo = supplierRepo;
            _productRepo = productRepo;
            _humanRepository = humanRepository;
            _fbSettings = fbSettings.Value;
            _fbReportManager = fbReportManager;
            _schRepo = schRepo;
            _fbManager = fbManager;
            _logger = logger;
            _fbLog = fbLog;
            _ApplicationContext = ApplicationContext;
            _dynamicFieldManager = dynamicFieldManager;
            _filterService = filterService;
            _auditRepository = auditRepository;
        }

        /// <summary>
        /// Save Master Data To FB.
        /// </summary>
        /// <param name="scheduleData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<Boolean> SaveMasterDataToFB(SaveScheduleRequest scheduleData, string fbToken, bool isRecreate)
        {
            StringBuilder failureResult = new StringBuilder();
            try
            {

                var entityId = _filterService.GetCompanyId();
                _logger.LogInformation("API-FB : FB Master data start here.");

                // Get Inspection booking related information based on the booking Id.

                var entity = await _bookingRepo.GetInspectionReportDetails(scheduleData.BookingId);
                if (entity == null)
                {
                    failureResult.Append(FBFailure.AuditDataNotFound + "--failure" + "\n");
                    throw new Exception();
                }
                var fbRequestLog = new LogFbBookingRequest()
                {
                    BookingId = entity.Id,
                    MissionId = entity.FbMissionId,
                    ServiceId = (int)Service.InspectionId
                };
                // Save Customer to FB and get FB Id back then save to API.

                if (!await SaveCustomerDataToFB(entity.Customer, fbToken, fbRequestLog, entityId))
                {
                    failureResult.Append(FBFailure.CustomerSave + "--failure" + "\n");
                    failureResult.Append(FBFailure.SupplierSave + "--failure" + "\n");
                    failureResult.Append(FBFailure.FactorySave + "--failure" + "\n");
                    failureResult.Append(FBFailure.ProductSave + "--failure" + "\n");
                    failureResult.Append(FBFailure.MissionSave + "--failure" + "\n");
                    failureResult.Append(FBFailure.UserSave + "--failure" + "\n");
                    failureResult.Append(FBFailure.ReportSave + "--failure" + "\n");
                    throw new Exception();
                }

                // Save Supplier to FB and get FB Id back then save to API.

                if (!await SaveSupplierDataToFB(entity.Supplier, fbToken, fbRequestLog, entityId))
                {
                    failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.SupplierSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.FactorySave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.ProductSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.MissionSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.UserSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                    throw new Exception();
                }

                // Save Factory to FB and get FB Id back then save to API.

                if (!await SaveFactoryDataToFB(entity.Factory, fbToken, fbRequestLog, entityId))
                {
                    failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.FactorySave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.ProductSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.MissionSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.UserSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                    throw new Exception();
                }

                // Save Products to FB and get FB Id back then save to API.

                if (!await SaveProductsDataToFB(entity, fbToken))
                {
                    failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.ProductSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.MissionSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.UserSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                    throw new Exception();
                }

                // Save Mission to FB and get FB Id back then save to API.

                if (!await SaveMissionDataToFB(entity, fbToken))
                {
                    failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.ProductSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.MissionSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.UserSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                    throw new Exception();
                }

                // create user and map the user to fb mission
                if (!await CreateUserToFBMission(entity, fbToken, scheduleData))
                {
                    failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.ProductSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.MissionSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.UserSave.ToString() + "--failure" + "\n");
                    failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                    throw new Exception();
                }

                // map the products with mission and enable to create the reports
                if (!await CreateMissionProductwithReports(entity, fbToken, 0))
                {
                    failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.ProductSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.MissionSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.UserSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                    throw new Exception();
                }


                if (!await SaveMissionUrlsDataToFb(entity, fbToken))
                {
                    failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.ProductSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.MissionSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.UserSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.ReportSave.ToString() + "--done" + "\n");
                    failureResult.Append(FBFailure.MissionUrl.ToString() + "--failure" + "\n");
                    throw new Exception();
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Source = failureResult.ToString();
                throw ex;
            }
        }


        public async Task<Boolean> SaveAuditMasterDataToFB(int auditId, int entityId, string fbToken)
        {
            StringBuilder failureResult = new StringBuilder();
            try
            {
                _logger.LogInformation("API-FB : FB Master data start here.");
                int serviceId = (int)Service.AuditId;
                // Get Audit booking related information based on the booking Id.
                var entity = await _auditRepository.GetAuditData(auditId);
                var auditFbReportDetails = await _auditRepository.GetAuditForFbReportDetails(auditId);

                var auditTranFiles = await _auditRepository.GetAuditTranFiles(auditId);
                if (entity != null && auditFbReportDetails != null)
                {
                    var fbRequestLog = new LogFbBookingRequest()
                    {
                        BookingId = entity.Id,
                        MissionId = entity.FbmissionId,
                        ServiceId = (int)Service.AuditId
                    };
                    if (auditFbReportDetails.CustomerId.HasValue)
                    {
                        if (auditFbReportDetails.FbCustomerId == null)
                        {
                            var customer = await _customerRepo.GetCustomerDataByCustomerIdAndEntityId(auditFbReportDetails.CustomerId.Value, entityId);
                            if (!await SaveCustomerDataToFB(customer, fbToken, fbRequestLog, entityId))
                            {
                                failureResult.Append(FBFailure.CustomerSave + "--failure" + "\n");
                                failureResult.Append(FBFailure.SupplierSave + "--failure" + "\n");
                                failureResult.Append(FBFailure.FactorySave + "--failure" + "\n");
                                failureResult.Append(FBFailure.MissionSave + "--failure" + "\n");
                                failureResult.Append(FBFailure.ReportSave + "--failure" + "\n");
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        failureResult.Append(FBFailure.CustomerNotFound + "--failure" + "\n");
                        throw new Exception();
                    }

                    if (auditFbReportDetails.SupplierId.HasValue)
                    {
                        if (auditFbReportDetails.FbSupplierId == null)
                        {
                            var supplier = await _supplierRepo.GetSupplierDataBySupplierIdAndEntityId(auditFbReportDetails.SupplierId.Value, entityId);
                            if (!await SaveSupplierDataToFB(supplier, fbToken, fbRequestLog, entityId))
                            {
                                failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                                failureResult.Append(FBFailure.SupplierSave.ToString() + "--failure" + "\n");
                                failureResult.Append(FBFailure.FactorySave.ToString() + "--failure" + "\n");
                                failureResult.Append(FBFailure.MissionSave.ToString() + "--failure" + "\n");
                                failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        failureResult.Append(FBFailure.SupplierNotFound + "--failure" + "\n");
                        throw new Exception();
                    }

                    if (auditFbReportDetails.FactoryId.HasValue)
                    {
                        if (auditFbReportDetails.FbFactoryId == null)
                        {
                            var factory = await _supplierRepo.GetSupplierDataBySupplierIdAndEntityId(auditFbReportDetails.FactoryId.Value, entityId);
                            if (!await SaveFactoryDataToFB(entity.Factory, fbToken, fbRequestLog, entityId))
                            {
                                failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                                failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                                failureResult.Append(FBFailure.FactorySave.ToString() + "--failure" + "\n");
                                failureResult.Append(FBFailure.MissionSave.ToString() + "--failure" + "\n");
                                failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        failureResult.Append(FBFailure.FactoryNotFound + "--failure" + "\n");
                        throw new Exception();
                    }

                    var auditCustomerContactDetails = await _auditRepository.GetAuditCustomerContacts(auditId);
                    var auditSupplierContactDetails = await _auditRepository.GetAuditSupplierContacts(auditId);

                    //save mission audit data
                    if (!await SaveAuditMissionDataToFB(entity, auditFbReportDetails, auditCustomerContactDetails, auditSupplierContactDetails, fbToken))
                    {
                        failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.MissionSave.ToString() + "--failure" + "\n");
                        failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                        throw new Exception();
                    }

                    //save mission audit url data
                    if (!await SaveAuditMissionUrlsDataToFb(entity, auditTranFiles, fbToken))
                    {
                        failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.MissionSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.MissionUrl.ToString() + "--failure" + "\n");
                        failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                        throw new Exception();
                    }
                    // Create the Reports for audit
                    if (!await CreateAuditReportRequest(entity, fbToken))
                    {
                        failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.MissionSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.MissionUrl.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                        throw new Exception();
                    }

                    if (!await CreateUserToFBMissionForAudit(entity, fbToken))
                    {
                        failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.MissionSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.MissionUrl.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.ReportSave.ToString() + "--done" + "\n");
                        failureResult.Append(FBFailure.UserSave.ToString() + "--failure" + "\n");
                        throw new Exception();
                    }
                }
                else
                {
                    failureResult.Append(FBFailure.AuditDataNotFound + "--failure" + "\n");
                    throw new Exception();
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Source = failureResult.ToString();
                throw ex;
            }
        }

        /// <summary>
        /// Update booking master data with FB
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateFBBookingDetails(int bookingId, string fbToken)
        {
            var entity = await _bookingRepo.GetInspectionReportDetails(bookingId);
            var fbRequestLog = new LogFbBookingRequest()
            {
                BookingId = entity.Id,
                MissionId = entity.FbMissionId,
                ServiceId = (int)Service.InspectionId
            };

            var entityId = _filterService.GetCompanyId();

            if (entity.Customer.FbCusId == null)
                await SaveCustomerDataToFB(entity.Customer, fbToken, fbRequestLog, entityId);

            if (entity.Supplier.FbFactSupId == null)
                await SaveSupplierDataToFB(entity.Supplier, fbToken, fbRequestLog, entityId);

            if (entity.Factory.FbFactSupId == null)
                await SaveFactoryDataToFB(entity.Factory, fbToken, fbRequestLog, entityId);

            await UpdateMissionDataToFB(entity, fbToken);

            return true;
        }

        /// <summary>
        /// Update Product data with FB 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateFBProductDetails(int bookingId, string fbToken)
        {
            var entity = await _bookingRepo.GetInspectionReportDetails(bookingId);
            return await CreateMissionProductwithReports(entity, fbToken, 0);
        }

        /// <summary>
        /// Delete Mission from FB
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbMissionId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>

        public async Task<SaveMissionResponse> DeleteFBMission(int bookingId, int fbMissionId, string fbToken)
        {
            SaveMissionResponse repsonse = new SaveMissionResponse();
            try
            {
                var entity = await _bookingRepo.GetInspectionReportDetails(bookingId);
                await DeleteFBReportAndMission(entity, fbToken);
                repsonse.Result = SaveMissionResponseResult.Success;
            }
            catch (Exception ex)
            {
                if (ex.Source == FBFailure.ReportProcessDone.ToString())
                {
                    repsonse.Result = SaveMissionResponseResult.FBReportAlreadyProcessed;
                }
                else if (ex.Source == FBFailure.MissionCompleted.ToString())
                {
                    repsonse.Result = SaveMissionResponseResult.MissionCompleted;
                }
                else
                {
                    repsonse.Result = SaveMissionResponseResult.Failure;
                }
            }

            return repsonse;
        }

        /// <summary>
        /// Create Mission Data with booking details
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<bool> CreateFBMission(int bookingId, string fbToken)
        {
            var entity = await _bookingRepo.GetInspectionReportDetails(bookingId);

            if (await SaveMissionDataToFB(entity, fbToken))
            {
                await SaveMissionUrlsDataToFb(entity, fbToken);

                await CreateMissionProductwithReports(entity, fbToken, 0);

                //map the booking qc, additional qc and cs data to fb
                await MapInspectionUserToFbMission(entity, fbToken);
            }

            return true;
        }

        /// <summary>
        /// map the inspection qc, additional qc and cs data to fb mission when create the mission
        /// </summary>
        /// <param name="inspection"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<bool> MapInspectionUserToFbMission(InspTransaction inspection, string fbToken)
        {
            var qcIdList = inspection.SchScheduleQcs.Where(x => x.Active && x.Qctype == (int)QCType.QC).Select(x => x.Qcid).Distinct().ToList();
            var additionalQcIdList = inspection.SchScheduleQcs.Where(x => x.Active && x.Qctype == (int)QCType.AdditionalQC).Select(x => x.Qcid).Distinct().ToList();
            var csIdList = inspection.SchScheduleCS.Where(x => x.Active).Select(x => x.Csid).Distinct().ToList();

            var entityId = _filterService.GetCompanyId();
            return await CreateUserToFBMission(inspection.Id, inspection.FbMissionId, qcIdList, additionalQcIdList, csIdList, fbToken, entityId);
        }

        /// <summary>
        /// Create FB Report with booking details 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<bool> CreateFBReport(int bookingId, string fbToken, int productId)
        {
            var entity = await _bookingRepo.GetInspectionReportDetails(bookingId);
            await MapInspectionUserToFbMission(entity, fbToken);
            return await CreateMissionProductwithReports(entity, fbToken, productId);
        }

        /// <summary>
        /// Delete Specific FB Report
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbReportId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<DeleteReportResponse> DeleteFBReport(int bookingId, int fbReportId, string fbToken, int apiReportId)
        {
            var entity = await _bookingRepo.GetInspectionReportDetails(bookingId);

            if (entity.InspTranServiceTypes.Any(x => x.Active && x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container))
            {
                return await DeleteMissionContainers(entity, fbToken, fbReportId, apiReportId);
            }
            else
            {
                return await DeleteMissionProducts(entity, fbToken, fbReportId, apiReportId);
            }

        }

        /// <summary>
        /// Fetch FB Report details and status - update to API 
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>

        public async Task<UpdateReportResponse> FetchFBReport(int fbReportId, string fbToken, int apiReportId)
        {
            try
            {
                if (fbReportId <= 0)
                {
                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportIdNotValid };
                }

                var fbReport = await GetFullBridgeReportDetails(fbReportId, fbToken);

                if (fbReport == null)
                {
                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportNotExist };
                }

                // update to API DB from FB - Report status
                FbStatusRequest request = new FbStatusRequest()
                {
                    FillingStatus = fbReport.statusFilling,
                    MissionStatus = fbReport.missionStatus,
                    ReviewStatus = fbReport.statusReview
                };
                await _fbManager.UpdateFBFillingAndReviewStatus(apiReportId, request, false);


                if (fbReport.status.ToLower() == "validated")
                {
                    var fbReportDetails = await GetFullBridgeReportInfo(fbReportId, fbToken);

                    if (fbReportDetails == string.Empty)
                    {
                        return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportNotExist };
                    }
                    // update to API DB from FB - Report details

                    var objRequestReport = JObject.Parse(fbReportDetails);

                    var result = await _fbManager.SaveFBReportDetails(fbReportId, objRequestReport.ToObject<FbReportDataRequest>());

                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.Success, IsNewReportFormatCheckPoint = result.IsNewReportFormatCheckPoint, InspectionId = result.InspectionId };
                }
                else
                {
                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportIsNotValidated };
                }
            }
            catch (Exception ex)
            {
                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                {
                    ReportId = fbReportId,
                    LogInformation = ex.Message,
                    CreatedBy = -1
                });
                return new UpdateReportResponse() { Result = UpdateReportResponseResult.Failure };
            }
        }

        /// <summary>
        /// Fetch Fb Reports by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<UpdateReportResponse> FetchFBReportByBooking(int bookingId, int option, string fbToken)
        {
            try
            {
                List<ReportIdData> inspectedFbReportData = new List<ReportIdData>();
                List<ReportIdData> fbReportIds = null;
                int reportCount = 0;

                if (bookingId <= 0)
                {
                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.BookingIdIsNotValid };
                }

                if (option == (int)FbReportFetch.All)
                {
                    fbReportIds = await _fbReportManager.getReportIdsbyBooking(bookingId);
                }
                else if (option == (int)FbReportFetch.NotValidated)
                {
                    fbReportIds = await _fbReportManager.getNonValidatedReportIdsbyBooking(bookingId);
                }

                // check the count 
                if (fbReportIds != null && fbReportIds.Any())
                {
                    reportCount = fbReportIds.Count();
                }

                if (reportCount == 0)
                {
                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportNotExist };
                }

                if (reportCount > 2) // push to queue bcz its a time taking process
                {
                    return new UpdateReportResponse() { ReportIds = fbReportIds, Result = UpdateReportResponseResult.ReportFetchMax };
                }
                else if (fbReportIds != null)
                {
                    foreach (var report in fbReportIds)
                    {
                        var fbReport = await GetFullBridgeReportDetails(report.FbReportId, fbToken);

                        if (fbReport != null)
                        {
                            // update to API DB from FB - Report status
                            FbStatusRequest request = new FbStatusRequest()
                            {
                                FillingStatus = fbReport.statusFilling,
                                MissionStatus = fbReport.missionStatus,
                                ReviewStatus = fbReport.statusReview
                            };
                            await _fbManager.UpdateFBFillingAndReviewStatus(report.ApiReportId, request, false);

                            if (fbReport.status.ToLower() == "validated")
                            {
                                var fbReportDetails = await GetFullBridgeReportInfo(report.FbReportId, fbToken);

                                if (fbReportDetails == string.Empty)
                                {
                                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportNotExist };
                                }
                                // update to API DB from FB - Report details

                                var objRequestReport = JObject.Parse(fbReportDetails);

                                var result = await _fbManager.SaveFBReportDetails(report.FbReportId, objRequestReport.ToObject<FbReportDataRequest>());
                                if (result.IsNewReportFormatCheckPoint)
                                {
                                    inspectedFbReportData.Add(new ReportIdData() { FbReportId = report.ApiReportId, InspectionId = result.InspectionId });
                                }

                            }
                        }
                    }
                }

                return new UpdateReportResponse() { Result = UpdateReportResponseResult.Success, FastReportIds = inspectedFbReportData };
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Fetch Bulk Fb Report updates
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<UpdateReportResponse> FetchBulkFBReport(int fbReportId, int apiReportId, string fbToken)
        {
            try
            {

                var fbReport = await GetFullBridgeReportDetails(fbReportId, fbToken);

                if (fbReport == null)
                {
                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        ReportId = fbReportId,
                        LogInformation = "Report not exist",
                        CreatedBy = -1
                    });
                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportNotExist };
                }

                // update to API DB from FB - Report status
                FbStatusRequest request = new FbStatusRequest()
                {
                    FillingStatus = fbReport.statusFilling,
                    MissionStatus = fbReport.missionStatus,
                    ReviewStatus = fbReport.statusReview
                };
                await _fbManager.UpdateFBFillingAndReviewStatus(apiReportId, request, true);


                if (fbReport.status.ToLower() == "validated")
                {
                    var fbReportDetails = await GetFullBridgeReportInfo(fbReportId, fbToken);

                    if (fbReportDetails == string.Empty)
                    {
                        return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportNotExist };
                    }
                    // update to API DB from FB - Report details

                    var objRequestReport = JObject.Parse(fbReportDetails);

                    var result = await _fbManager.SaveFBReportDetails(fbReportId, objRequestReport.ToObject<FbReportDataRequest>());

                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.Success, IsNewReportFormatCheckPoint = result.IsNewReportFormatCheckPoint, InspectionId = result.InspectionId, EntityId = result.EntityId };
                }
                else
                {
                    return new UpdateReportResponse() { Result = UpdateReportResponseResult.ReportIsNotValidated };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// Save Customer Data to FB and save FB_Id back to API customer table.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveCustomerDataToFB(CuCustomer customerData, string fbToken, LogFbBookingRequest request, int entityId)
        {
            try
            {

                if (customerData.FbCusId == null) // check if customer not created in FB
                {
                    FBUserAccountData objAccount = new FBUserAccountData()
                    {
                        client = true,
                        title = customerData.CustomerName,
                        address = customerData.CuAddresses?.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice)
                                     .Select(x => x.Address).FirstOrDefault(),
                        country = customerData.CuAddresses?.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice)
                                      .Select(x => x.Country?.FbCountryId).FirstOrDefault(),
                        city = customerData.CuAddresses?.Where(y => y.AddressType == (int)RefAddressTypeEnum.HeadOffice)
                                       .Select(x => x.City?.CityName).FirstOrDefault(),
                        status = FBConstants.Active
                    };
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.AccountRequestUrl;

                    _logger.LogInformation("API-FB : FB Customer data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        ServiceId = request.ServiceId,
                        MissionId = request.MissionId,
                        BookingId = request.BookingId,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objAccount, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB Customer data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);

                        // update customer table with FB id.
                        var customerEntity = await _customerRepo.GetCustomerDataByCustomerIdAndEntityId(customerData.Id, entityId);
                        customerEntity.FbCusId = Convert.ToInt32(fbRecordId);
                        customerData.FbCusId = customerEntity.FbCusId;
                        await _customerRepo.EditCustomer(customerEntity);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Delete Customer Entry in FB.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> DeleteCustomerDataToFB(InspTransaction bookingData, string fbToken)
        {
            try
            {

                if (bookingData.Customer.FbCusId != null) // check if customer not created in FB
                {

                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.AccountDeleteRequestUrl, bookingData.Customer.FbCusId);

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {

                        // update customer table with FB id.
                        var customerEntity = await _customerRepo.GetCustomerDetails(bookingData.Customer.Id);
                        customerEntity.FbCusId = null;

                        bookingData.Customer.FbCusId = null;

                        await _customerRepo.EditCustomer(customerEntity);
                        return true;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete Supplier From FB.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> DeleteSupplierDataToFB(InspTransaction bookingData, string fbToken)
        {
            try
            {

                if (bookingData.Supplier.FbFactSupId != null) // check if customer not created in FB
                {

                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.AccountDeleteRequestUrl, bookingData.Supplier.FbFactSupId);

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {

                        // update customer table with FB id.
                        var supplierEntity = await _supplierRepo.GetSupplierDetails(bookingData.Supplier.Id);
                        supplierEntity.FbFactSupId = null;
                        bookingData.Supplier.FbFactSupId = null;
                        await _supplierRepo.EditSupplier(supplierEntity);
                        return true;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete factory from FB
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>

        private async Task<Boolean> DeleteFactoryDataToFB(InspTransaction bookingData, string fbToken)
        {
            try
            {

                if (bookingData.Factory.FbFactSupId != null) // check if customer not created in FB
                {

                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.AccountDeleteRequestUrl, bookingData.Factory.FbFactSupId);

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var factoryEntity = await _supplierRepo.GetSupplierDetails(bookingData.Factory.Id);
                        factoryEntity.FbFactSupId = null;
                        bookingData.Factory.FbFactSupId = null;
                        await _supplierRepo.EditSupplier(factoryEntity);
                        return true;

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SaveScheduleRequest getScheduleQCAndCSList(InspTransaction bookingData)
        {
            SaveScheduleRequest objQcAndCS = new SaveScheduleRequest();
            List<SaveAllocationStaff> staffData = new List<SaveAllocationStaff>();
            List<StaffSchedule> qcList = new List<StaffSchedule>();
            List<StaffSchedule> csList = new List<StaffSchedule>();

            foreach (var item in bookingData.SchScheduleQcs)
            {
                qcList.Add(new StaffSchedule
                {
                    StaffID = item.Qcid,
                    StaffName = item.Qc.PersonName
                });
            }

            foreach (var item in bookingData.SchScheduleCS)
            {
                csList.Add(new StaffSchedule
                {
                    StaffID = item.Csid,
                    StaffName = item.Cs.PersonName
                });
            }

            staffData.Add(new SaveAllocationStaff()
            {
                QC = qcList,
                CS = csList
            });

            objQcAndCS.AllocationCSQCStaff = staffData;

            return objQcAndCS;

        }

        /// <summary>
        /// Save Supplier data to Full bridge.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveSupplierDataToFB(SuSupplier supplierData, string fbToken, LogFbBookingRequest request, int entityId)
        {
            try
            {
                if (supplierData.FbFactSupId == null) // check if supplier not created in FB
                {
                    FBUserAccountData objAccount = new FBUserAccountData()
                    {
                        vendor = (supplierData.TypeId == 2) ? true : false,
                        title = supplierData.SupplierName,
                        address = supplierData.SuAddresses?.Select(x => x.Address).FirstOrDefault(),
                        cn_address = supplierData.SuAddresses?.Select(x => x.LocalLanguage).FirstOrDefault(),
                        country = supplierData.SuAddresses?.Select(x => x.Country?.FbCountryId).FirstOrDefault(),
                        countrySubdivision = supplierData.SuAddresses?.Select(x => x.Region?.FbProvinceId).FirstOrDefault(),
                        city = supplierData.SuAddresses?.Select(x => x.City?.CityName).FirstOrDefault(),
                        status = FBConstants.Active
                    };

                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.AccountRequestUrl;

                    _logger.LogInformation("API-FB : FB supplier/Factory data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        ServiceId = request.ServiceId,
                        MissionId = request.MissionId,
                        BookingId = request.BookingId,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objAccount, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB supplier/Factory data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        // update customer table with FB id.
                        var supplierEntity = await _supplierRepo.GetSupplierDataBySupplierIdAndEntityId(supplierData.Id, entityId);
                        supplierEntity.FbFactSupId = Convert.ToInt32(fbRecordId);
                        supplierData.FbFactSupId = supplierEntity.FbFactSupId;
                        await _supplierRepo.EditSupplier(supplierEntity);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveMissionUrlsDataToFb(InspTransaction bookingData, string fbToken)
        {
            try
            {
                if (bookingData.FbMissionId > 0)
                {
                    var missionUrls = await GetMissionUrlsDataToFb(bookingData.FbMissionId.GetValueOrDefault(), bookingData.Id, fbToken, (int)Service.InspectionId);
                    if (missionUrls != null)
                    {
                        if (bookingData.InspTranFileAttachments.Any(x => x.Active)) // check if supplier not created in FB
                        {

                            var inspectionAttachmentFbIds = bookingData.InspTranFileAttachments.Where(x => x.FbId.HasValue && x.Active && x.IsReportSendToFb.HasValue && x.IsReportSendToFb.Value).Select(x => x.FbId);
                            var deleteFbUrlIds = missionUrls.Where(x => x > 0 && !inspectionAttachmentFbIds.Contains(x));
                            if (deleteFbUrlIds != null && deleteFbUrlIds.Any())
                            {
                                foreach (var deleteFbUrlId in deleteFbUrlIds)
                                {
                                    if (deleteFbUrlId > 0)
                                    {
                                        if (!await DeleteMissionUrlDataToFb(bookingData, deleteFbUrlId, fbToken))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            List<FBMissionUrlData> requestMissionUrl = new List<FBMissionUrlData>();

                            string fbRecordId = string.Empty;
                            var fbBase = _fbSettings.BaseUrl;
                            var fbRequest = string.Format(_fbSettings.MissionUrlRequestUrl, bookingData.FbMissionId);

                            var inspTranFileAttachments = bookingData.InspTranFileAttachments.Where(x => x.Active && !x.FbId.HasValue && x.IsReportSendToFb.HasValue && x.IsReportSendToFb.Value).ToList();
                            if (inspTranFileAttachments.Any())
                            {
                                foreach (var item in inspTranFileAttachments)
                                {
                                    requestMissionUrl.Add(new FBMissionUrlData() { url = item.FileUrl, description = item.FileDescription, title = item.FileName, classification = FBMissionUrlClassification.MissionAttachment });
                                }

                                _logger.LogInformation("API-FB : FB mission url data request start ");
                                _logger.LogInformation(JsonConvert.SerializeObject(requestMissionUrl));

                                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                                {
                                    ServiceId = (int)Service.InspectionId,
                                    MissionId = bookingData?.FbMissionId,
                                    BookingId = bookingData?.Id,
                                    RequestUrl = fbRequest,
                                    LogInformation = JsonConvert.SerializeObject(requestMissionUrl)
                                });

                                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, requestMissionUrl, fbBase, fbToken);

                                _logger.LogInformation("API-FB : FB mission url data request end ");
                                _logger.LogInformation(httpResponse.StatusCode.ToString());
                                if (httpResponse.StatusCode == HttpStatusCode.Created)
                                {

                                    var result = getFBMissionUrlData(httpResponse);
                                    UpdateInspTranFileAttachementFbID(inspTranFileAttachments, result);
                                    await _bookingRepo.Save();
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (missionUrls.Any())
                            {
                                foreach (var missionUrl in missionUrls)
                                {
                                    if (missionUrl > 0)
                                    {
                                        await DeleteMissionUrlDataToFb(bookingData, missionUrl, fbToken);
                                    }
                                }
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateInspTranFileAttachementFbID(IEnumerable<InspTranFileAttachment> inspTranFileAttachments, List<FbMissionUrlResponse> fbMissionUrls)
        {
            if (fbMissionUrls != null)
            {
                foreach (var fbMissionUrl in fbMissionUrls)
                {
                    var inspTranFileAttachment = inspTranFileAttachments.FirstOrDefault(x => x.FileUrl == fbMissionUrl.Url);
                    if (inspTranFileAttachment != null && Int32.TryParse(fbMissionUrl.FbId, out int fbId))
                    {
                        inspTranFileAttachment.FbId = fbId;
                    }
                }
                _bookingRepo.EditEntities(inspTranFileAttachments);
            }
        }

        private async Task<List<int>> GetMissionUrlsDataToFb(int fbMissionId, int bookingId, string fbToken, int serviceId)
        {
            try
            {
                if (fbMissionId > 0)
                {
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.MissionUrlRequestUrl, fbMissionId);

                    _logger.LogInformation("API-FB : FB mission url data request start ");

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        ServiceId = serviceId,
                        MissionId = fbMissionId,
                        BookingId = bookingId,
                        RequestUrl = fbRequest
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                    List<int> fbUrlIds = new List<int>();
                    _logger.LogInformation("API-FB : FB mission url data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var result = getFBMissionUrlData(httpResponse);
                        if (result != null && result.Any())
                        {
                            return result.Select(x =>
                            {
                                if (Int32.TryParse(x.FbId, out int fbId))
                                    return fbId;
                                else return 0;
                            }).ToList();
                        }


                    }
                    return fbUrlIds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> DeleteMissionUrlDataToFb(InspTransaction bookingData, int fbUrlId, string fbToken)
        {
            try
            {
                string fbRecordId = string.Empty;
                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format(_fbSettings.MissionUrlDeleteUrl, bookingData.FbMissionId);
                fbRequest = string.Concat(fbRequest, "/", fbUrlId);

                _logger.LogInformation("API-FB : FB mission url data request start ");

                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                {
                    ServiceId = (int)Service.InspectionId,
                    MissionId = bookingData.FbMissionId,
                    BookingId = bookingData?.Id,
                    RequestUrl = fbRequest,
                });

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);

                _logger.LogInformation("API-FB : FB mission url data request end ");
                _logger.LogInformation(httpResponse.StatusCode.ToString());

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var fileAttachment = bookingData.InspTranFileAttachments.FirstOrDefault(x => x.FbId == fbUrlId);
                    if (fileAttachment != null)
                    {
                        fileAttachment.FbId = null;

                        _bookingRepo.EditEntity(fileAttachment);
                    }

                    await _bookingRepo.Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Save Factory details to FB from api 
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveFactoryDataToFB(SuSupplier factoryData, string fbToken, LogFbBookingRequest logFbBookingRequest, int entityId)
        {
            try
            {
                if (factoryData.FbFactSupId == null) // check if factory not created in FB
                {
                    FBUserAccountData objAccount = new FBUserAccountData()
                    {
                        factory = (factoryData.TypeId == 1) ? true : false,
                        title = factoryData.SupplierName,
                        address = factoryData.SuAddresses?.Select(x => x.Address).FirstOrDefault(),
                        cn_address = factoryData.SuAddresses?.Select(x => x.LocalLanguage).FirstOrDefault(),
                        country = factoryData.SuAddresses?.Select(x => x.Country?.FbCountryId).FirstOrDefault(),
                        countrySubdivision = factoryData.SuAddresses?.Select(x => x.Region?.FbProvinceId).FirstOrDefault(),
                        city = factoryData.SuAddresses?.Select(x => x.City?.CityName).FirstOrDefault(),
                        status = FBConstants.Active
                    };

                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.AccountRequestUrl;
                    _logger.LogInformation("API-FB : FB Factory data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        ServiceId = logFbBookingRequest.ServiceId,
                        MissionId = logFbBookingRequest.MissionId,
                        BookingId = logFbBookingRequest.BookingId,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objAccount, fbBase, fbToken);
                    _logger.LogInformation("API-FB : FB Factory data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        var factoryEntity = await _supplierRepo.GetSupplierDataBySupplierIdAndEntityId(factoryData.Id, entityId);
                        factoryEntity.FbFactSupId = Convert.ToInt32(fbRecordId);
                        factoryData.FbFactSupId = factoryEntity.FbFactSupId;
                        await _supplierRepo.EditSupplier(factoryEntity);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save Customer Products with FB from booking API.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveProductsDataToFB(InspTransaction bookingData, string fbToken)
        {
            try
            {
                var activeProductTransactions = bookingData.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value);

                foreach (var item in activeProductTransactions)
                {
                    if (item.Product?.FbCusProdId == null) // check if products not created in FB
                    {
                        int? productcategoryId = 0;

                        if (item.Product?.ProductCategorySub2Navigation?.FbProductSubCategory2Id != null)
                        {
                            productcategoryId = item.Product?.ProductCategorySub2Navigation?.FbProductSubCategory2Id.Value;
                        }

                        else if (item.Product?.ProductSubCategoryNavigation?.FbProductSubCategoryId != null)
                        {
                            productcategoryId = item.Product?.ProductSubCategoryNavigation?.FbProductSubCategoryId.Value;
                        }
                        else if (item.Product?.ProductCategoryNavigation?.FbProductCategoryId != null)
                        {
                            productcategoryId = item.Product?.ProductCategoryNavigation?.FbProductCategoryId.Value;
                        }

                        FBProductData objProduct = new FBProductData()
                        {
                            title = item.Product?.ProductDescription,
                            reference = item.Product?.ProductId?.Trim(),
                            client = bookingData.Customer.FbCusId,
                            vendor = bookingData.Supplier.FbFactSupId,
                            factory = bookingData.Factory.FbFactSupId,
                            productCategory = productcategoryId.GetValueOrDefault(),
                            status = FBConstants.Active
                        };

                        string fbRecordId = string.Empty;
                        var fbBase = _fbSettings.BaseUrl;
                        var fbRequest = _fbSettings.ProductRequestUrl;

                        _logger.LogInformation("API-FB : FB product data request start ");
                        _logger.LogInformation(JsonConvert.SerializeObject(objProduct));

                        await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                        {
                            ServiceId = (int)Service.InspectionId,
                            MissionId = bookingData?.FbMissionId,
                            BookingId = bookingData?.Id,
                            RequestUrl = fbRequest,
                            LogInformation = JsonConvert.SerializeObject(objProduct)
                        });

                        HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objProduct, fbBase, fbToken);

                        _logger.LogInformation("API-FB : FB product data request end ");
                        _logger.LogInformation(httpResponse.StatusCode.ToString());

                        if (httpResponse.StatusCode == HttpStatusCode.Created)
                        {
                            fbRecordId = getFbRecordIdFromResult(httpResponse);
                            var productEntity = _productRepo.GetCustomerProductByID(item.ProductId);
                            if (productEntity != null)
                            {
                                productEntity.FbCusProdId = Convert.ToInt32(fbRecordId);
                                item.Product.FbCusProdId = productEntity?.FbCusProdId;
                                await _productRepo.EditCustomerProducts(productEntity);
                            }
                            else
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }

                await _bookingRepo.EditInspectionBooking(bookingData);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// create mission object based on the dynamic request
        /// </summary>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private async Task<IDictionary<string, string>> CreateMissionObject(InspTransaction bookingData)
        {
            string service = string.Empty;
            string isEaqf = "false";
            var destinationCountry = bookingData?.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x?.DestinationCountry?.CountryName);
            if (bookingData.InspTranServiceTypes != null && bookingData.InspTranServiceTypes.Any())
            {
                var inspServiceType = bookingData.InspTranServiceTypes.Where(x => x.Active).FirstOrDefault();
                if (inspServiceType != null && inspServiceType.ServiceType != null)
                    service = inspServiceType.ServiceType.FbServiceTypeId.HasValue ? inspServiceType.ServiceType?.FbServiceTypeId.Value.ToString() : "0";
            }
            var FBCusId = bookingData.Customer.FbCusId.HasValue ? bookingData.Customer.FbCusId.ToString() : "0";
            var FBFacId = bookingData.Factory.FbFactSupId.HasValue ? bookingData.Factory.FbFactSupId.Value.ToString() : "0";
            var FBSupId = bookingData.Supplier.FbFactSupId.HasValue ? bookingData.Supplier.FbFactSupId.Value.ToString() : "0";

            var clientEmail = string.Join(",", bookingData.InspTranCuContacts.Where(x => x.Active).Select(x => x.Contact.Email).Distinct());

            var supplierEmail = string.Join(",", bookingData.InspTranSuContacts.Where(x => x.Active).Select(x => x.Contact.Mail).Distinct());

            var contact = string.Join(",", bookingData.InspTranCuContacts.Where(x => x.Active).Select(x => x.Contact.ContactName).Distinct());

            var department = string.Join(",", bookingData.InspTranCuDepartments.Where(x => x.Active).Select(x => x.Department.Name).Distinct());

            var merchandiser = string.Join(",", bookingData.InspTranCuMerchandisers.Where(x => x.Active).Select(x => x.Merchandiser.ContactName).Distinct());

            var buyer = string.Join(",", bookingData.InspTranCuBuyers.Where(x => x.Active).Select(x => x.Buyer.Name).Distinct());

            var supplierContact = string.Join(",", bookingData.InspTranSuContacts.Where(x => x.Active).Select(x => x.Contact.ContactName).Distinct());

            var brand = string.Join(",", bookingData?.InspTranCuBrands.Where(x => x.Active).Select(x => x.Brand?.Name).Distinct());

            //take fb entity id for the current entity id and push to fb
            var entityId = _filterService.GetCompanyId();

            var fbEntityId = await _schRepo.GetFBEntityId(entityId);

            var qc_company = fbEntityId != null ? Convert.ToString(fbEntityId) : string.Empty;

            if (bookingData.IsEaqf.GetValueOrDefault())
            {
                isEaqf = "true";
            }

            var objAccount = new Dictionary<string, string>
                                        {
                                            { "startDate", bookingData.ServiceDateFrom.Year + "-"
                                                            + bookingData.ServiceDateFrom.Month + "-" + bookingData.ServiceDateFrom.Day},
                                            { "endDate", bookingData.ServiceDateTo.Year + "-" + bookingData.ServiceDateTo.Month
                                                                                    + "-" + bookingData.ServiceDateTo.Day},
                                            { "service", service },
                                            { "reference", bookingData.Id.ToString()},
                                            { "client", FBCusId},
                                            { "factory", FBFacId},
                                            { "vendor", FBSupId},
                                            { "inspectionOffice", bookingData.Office?.LocationName},
                                            { "qc_comment", bookingData.QcbookingComments ?? "" + " " + bookingData.ScheduleComments ?? ""},
                                            { "destinationCountry", string.Join(",", destinationCountry.Distinct())},
                                            { "contact", contact},
                                            { "department", department},
                                            { "merchandiser", merchandiser},
                                            { "buyer", buyer},
                                            { "customerReferenceNo",bookingData.CustomerBookingNo},
                                            { "clientEmail",clientEmail},
                                            { "supplierEmail",supplierEmail},
                                            {"supplierContactName",supplierContact },
                                            {"brand", brand},
                                            {"qc_company",qc_company },
                                            {"isEaqf",isEaqf }
                                        };

            var bookingDFCustomerData = await _dynamicFieldManager.GetBookingDFDataByBookingIds(new[] { bookingData.Id });
            if (bookingDFCustomerData.Result == InspectionBookingDFDataResult.Success)
            {
                if (bookingDFCustomerData.bookingDFDataList != null && bookingDFCustomerData.bookingDFDataList.Any())
                {
                    foreach (var bookingDFData in bookingDFCustomerData.bookingDFDataList)
                    {
                        if (!string.IsNullOrEmpty(bookingDFData.FbReference))
                            objAccount.Add(bookingDFData.FbReference, bookingDFData.DFValue);
                    }
                }
            }

            //get the audit product category for the inspection
            var auditProductCategory = await _dynamicFieldManager.GetBookingAuditProductCategory(bookingData.Id,
                                                                (int)DynamicDropDownSourceType.AuditProductCategory);

            if (!string.IsNullOrEmpty(auditProductCategory) && int.TryParse(auditProductCategory, out int auditProductCategoryValue))
                objAccount.Add("auditProductCategory", AuditProductCategoryList.GetValueOrDefault(auditProductCategoryValue, ""));

            return objAccount;
        }

        private async Task<IDictionary<string, string>> CreateMissionObjectForAudit(FbAuditData auditData, int entityId, List<AuditCustomerContactData> customerContacts, List<AuditCustomerContactData> supplierContacts, string customerProductCategory)
        {
            string service = auditData.FbServiceId.HasValue ? auditData.FbServiceId.Value.ToString() : "0";
            var FBCusId = auditData.FbCustomerId.HasValue ? auditData.FbCustomerId.ToString() : "0";
            var FBFacId = auditData.FbFactoryId.HasValue ? auditData.FbFactoryId.Value.ToString() : "0";
            var FBSupId = auditData.FbSupplierId.HasValue ? auditData.FbSupplierId.Value.ToString() : "0";

            var clientEmail = string.Join(",", customerContacts?.Select(x => x.ContactEmail).Distinct().ToList());

            var supplierEmail = string.Join(",", supplierContacts.Select(x => x.ContactEmail).Distinct().ToList());

            var supplierContact = string.Join(",", supplierContacts.Select(x => x.ContactName).Distinct().ToList());

            //take fb entity id for the current entity id and push to fb            

            var fbEntityId = await _schRepo.GetFBEntityId(entityId);

            var qc_company = fbEntityId != null ? Convert.ToString(fbEntityId) : string.Empty;

            var objAccount = new Dictionary<string, string>
                                        {
                                            { "startDate", auditData.ServiceFromDate.Year + "-"
                                                            + auditData.ServiceFromDate.Month + "-" + auditData.ServiceFromDate.Day},
                                            { "endDate", auditData.ServiceToDate.Year + "-" + auditData.ServiceToDate.Month
                                                                                    + "-" + auditData.ServiceToDate.Day},
                                            { "service", service },
                                            { "reference", auditData.AuditId.ToString()},
                                            { "client", FBCusId},
                                            { "factory", FBFacId},
                                            { "vendor", FBSupId},
                                            { "inspectionOffice", auditData.Office},
                                            { "clientEmail",clientEmail},
                                            { "supplierEmail",supplierEmail},
                                            {"supplierContactName",supplierContact },
                                            {"qc_company",qc_company },
                                            {"audit_type", auditData.AuditType},
                                            {"evaluation_round", auditData.Evalution},
                                            {"auditProductCategory", customerProductCategory }
                                        };

            var bookingDFCustomerData = await _dynamicFieldManager.GetBookingDFDataByBookingIds(new[] { auditData.AuditId });
            if (bookingDFCustomerData.Result == InspectionBookingDFDataResult.Success)
            {
                if (bookingDFCustomerData.bookingDFDataList != null && bookingDFCustomerData.bookingDFDataList.Any())
                {
                    foreach (var bookingDFData in bookingDFCustomerData.bookingDFDataList)
                    {
                        if (!string.IsNullOrEmpty(bookingDFData.FbReference))
                            objAccount.Add(bookingDFData.FbReference, bookingDFData.DFValue);
                    }
                }
            }

            return objAccount;
        }

        /// <summary>
        /// Save Mission data to FB with booking details from API.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveMissionDataToFB(InspTransaction bookingData, string fbToken)
        {
            try
            {
                if (bookingData?.FbMissionId == null) // check if mission not created in FB
                {
                    //Create mission object
                    var objAccount = await CreateMissionObject(bookingData);
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.MissionRequestUrl;
                    _logger.LogInformation("API-FB : FB mission data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));
                    // Add Full bridge Log information for mission request
                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        MissionId = bookingData?.FbMissionId,
                        BookingId = bookingData?.Id,
                        RequestUrl = fbRequest,
                        ServiceId = (int)Service.InspectionId,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });
                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objAccount, fbBase, fbToken);
                    _logger.LogInformation("API-FB : FB mission data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        // update booking table with FB id.
                        bookingData.FbMissionId = Convert.ToInt32(fbRecordId);

                        await _bookingRepo.EditInspectionBooking(bookingData);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var objAccount = await CreateMissionObject(bookingData);

                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.MissionDeleteRequestUrl, bookingData.FbMissionId);

                    _logger.LogInformation("API-FB : FB mission data update request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    // Add Full bridge Log information for mission request update
                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        MissionId = bookingData?.FbMissionId,
                        BookingId = bookingData?.Id,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Patch, fbRequest, objAccount, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB mission data update request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Update Mission Details with Patch data.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> UpdateMissionDataToFB(InspTransaction bookingData, string fbToken)
        {
            try
            {
                if (bookingData.FbMissionId != null) // check if mission not created in FB
                {
                    var objAccount = await CreateMissionObject(bookingData);

                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.MissionDeleteRequestUrl, bookingData.FbMissionId);

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Patch, fbRequest, objAccount, fbBase, fbToken);
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// create mission Report 
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> CreateMissionProductwithReports(InspTransaction bookingData, string fbToken, int productId)
        {

            // container save with fb reports
            if (bookingData.InspTranServiceTypes.Any(x => x.Active && x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container))
            {
                return await CreateMissionProductbyContainerwithReports(bookingData, fbToken, productId);
            }
            else
            {
                List<int> objReportIdList = new List<int>();
                List<FBMissionProductReportUser> objMissionProductsList = new List<FBMissionProductReportUser>();
                try
                {
                    List<FBMapProductReport> combineProductList = null;
                    var activeProductTransaction = bookingData.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value);

                    if (activeProductTransaction != null && activeProductTransaction.Any())
                    {
                        // if product is exist then create only specific report
                        if (productId > 0)
                        {
                            activeProductTransaction = activeProductTransaction.Where(x => x.ProductId == productId);
                        }

                        activeProductTransaction = activeProductTransaction.OrderByDescending(x => x.Aql.HasValue).ThenBy(x => x.Aql);

                        foreach (var item in activeProductTransaction)
                        {
                            // check report is not created in FB
                            if (item.FbReportId == null)
                            {

                                FBMissionProductReportUser objMissionProduct = new FBMissionProductReportUser()
                                {
                                    purchaseOrderNumber = item.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Active.Value)?.Po?.Pono,
                                    product = item.Product?.FbCusProdId == null ? 0 : item.Product?.FbCusProdId.Value,
                                    aqlLevel = item?.AqlNavigation?.Fbvalue != null ? item?.AqlNavigation?.Fbvalue.ToString() : "",
                                    aqlCritical = item?.CriticalNavigation?.Value != null ? item?.CriticalNavigation?.Value.ToString() : "",
                                    aqlMajor = item?.MajorNavigation?.Value != null ? item?.MajorNavigation?.Value.ToString() : "",
                                    aqlMinor = item?.MinorNavigation?.Value != null ? item?.MinorNavigation?.Value.ToString() : "",
                                    addTemplate = item?.Fbtemplate?.FbTemplateId.GetValueOrDefault(),
                                    customerReferenceNo = item.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Active.Value)?.CustomerReferencePo,
                                    createReport = true,
                                    copyUsersFromMission = true,
                                    isEcopack = item.IsEcopack.GetValueOrDefault(),
                                    isDisplay = item.IsDisplayMaster.GetValueOrDefault(),
                                    ean = item.Product?.Barcode,
                                    destinationCountry = item.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Active.Value)?.DestinationCountry?.CountryName,
                                    etd = item.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Active.Value)?.Etd != null ?
                                          item.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Active.Value)?.Etd.ToString() : string.Empty,
                                    factoryReference = item.Product?.FactoryReference,
                                    samplingSizeCustom = item?.AqlNavigation?.Fbvalue != null && item?.Aql == (int)AqlType.AQLCustom ? item?.AqlQuantity?.ToString() : ""
                                };


                                //apply the color transaction details
                                if (bookingData.BusinessLine == (int)BusinessLine.SoftLine)
                                {
                                    var colorTransaction = item.InspPurchaseOrderColorTransactions.FirstOrDefault(x => x.Active.Value);

                                    if (colorTransaction != null)
                                    {
                                        objMissionProduct.color = colorTransaction.ColorName;
                                        objMissionProduct.quantity = colorTransaction.BookingQuantity.GetValueOrDefault();
                                    }

                                }
                                //apply the po transaction details
                                else
                                {
                                    objMissionProduct.quantity = item.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Active.Value).BookingQuantity;
                                }

                                // for Log purpose
                                objMissionProductsList.Add(objMissionProduct);

                                var fbBase = _fbSettings.BaseUrl;
                                var fbRequest = string.Format(_fbSettings.MissionReportRequestUrl, bookingData.FbMissionId);

                                _logger.LogInformation("API-FB : FB mission product report creation request start ");
                                _logger.LogInformation(JsonConvert.SerializeObject(objMissionProduct));

                                // Add Full bridge Log information for mission products                       

                                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                                {
                                    ServiceId = (int)Service.InspectionId,
                                    MissionId = bookingData?.FbMissionId,
                                    BookingId = bookingData?.Id,
                                    RequestUrl = fbRequest,
                                    LogInformation = JsonConvert.SerializeObject(objMissionProduct)
                                });


                                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objMissionProduct, fbBase, fbToken);

                                _logger.LogInformation("API-FB : FB mission product report creation request end ");
                                _logger.LogInformation(httpResponse.StatusCode.ToString());

                                if (httpResponse.StatusCode == HttpStatusCode.Created)
                                {
                                    // get fb Result
                                    var fbData = getFBResultData(httpResponse);

                                    _logger.LogInformation("API-FB : Report ID :" + fbData.fbReportId + "");
                                    int reportNumber;

                                    if (fbData != null && Int32.TryParse(fbData.fbReportId, out reportNumber))
                                    {
                                        // save Fb Report details in API Report table
                                        fbData.InspectionId = bookingData.Id;
                                        var reportData = await SaveFbReport(fbData);

                                        if (reportData != null && reportData.Result == FbStatusResponseResult.success)
                                        {
                                            objReportIdList.Add(Convert.ToInt32(reportData.ReportId));
                                        }

                                        // map product related po to fb Report and mission products
                                        combineProductList = new List<FBMapProductReport>();

                                        //if the businessline is softline then take the color transaction details
                                        if (bookingData.BusinessLine == (int)BusinessLine.SoftLine)
                                        {
                                            if (!await MapPoColorTransactionToReportProducts(bookingData, item, item.InspPurchaseOrderColorTransactions.ToList(), objReportIdList,
                                                                            fbToken, fbData.fbReportId, reportData.ReportId))
                                                return false;
                                        }
                                        //process through the po transactions
                                        else
                                        {
                                            if (!await MapPoProductTransactiontoReportProducts(bookingData, item, item.InspPurchaseOrderTransactions.ToList(), objReportIdList,
                                                                            fbToken, fbData.fbReportId, reportData.ReportId))
                                                return false;
                                        }

                                        if (reportData != null && reportData.Result == FbStatusResponseResult.success)
                                        {
                                            // update fb report id and fb mission product map id with inspection po transaction
                                            item.FbReportId = reportData.ReportId;

                                            // check combine order exist then map created report id with combine products
                                            if (item.CombineProductId != null)
                                            {
                                                var combineOrderList = bookingData.InspProductTransactions.Where(x =>
                                                x.Active.HasValue && x.Active.Value && x.CombineProductId == item.CombineProductId);

                                                if (combineOrderList.Any())
                                                {
                                                    //combineProductList = new List<FBMapProductReport>();
                                                    foreach (var combineProducts in combineOrderList)
                                                    {
                                                        if (item.ProductId == combineProducts.ProductId)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            if (bookingData.BusinessLine == (int)BusinessLine.SoftLine && item.InspPurchaseOrderColorTransactions != null
                                                                                    && item.InspPurchaseOrderColorTransactions.Any())
                                                            {
                                                                var combineOrderPoColorList = bookingData.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value &&
                                                                  x.ProductId == combineProducts.ProductId).SelectMany(x => x.InspPurchaseOrderColorTransactions).
                                                                                                               Where(z => z.Active.HasValue && z.Active.Value).ToList();

                                                                MapCombineColorTransactionToFBMapProductReport(item, combineOrderPoColorList, combineProductList);

                                                            }
                                                            else
                                                            {
                                                                var combineOrderPoList = bookingData.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value &&
                                                                 x.ProductId == combineProducts.ProductId).SelectMany(x => x.InspPurchaseOrderTransactions).
                                                                                                              Where(z => z.Active.HasValue && z.Active.Value).ToList();

                                                                MapCombinePODataToFBMapProductReport(item, combineOrderPoList, combineProductList);
                                                            }
                                                        }
                                                    }

                                                    // map the report id for combine prodcut list
                                                    var isMappedCombine = await MapCombineProductsToFBMission(combineProductList, Convert.ToInt32(fbData.fbReportId), fbToken, bookingData, reportData.ReportId);

                                                    // if any failure happen at the time of mapping combine products with existing report - revert back
                                                    if (!isMappedCombine)
                                                    {
                                                        await DeleteMission(bookingData, fbToken, objReportIdList, false);
                                                        return false;
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            await DeleteMission(bookingData, fbToken, objReportIdList, false);
                                            return false;
                                        }

                                    }
                                    else
                                    {
                                        await DeleteMission(bookingData, fbToken, objReportIdList, false);
                                        return false;
                                    }
                                }

                                else
                                {
                                    await DeleteMission(bookingData, fbToken, objReportIdList, false);
                                    return false;
                                }
                            }
                        }

                        await _bookingRepo.EditInspectionBooking(bookingData);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    if (bookingData.InspTranServiceTypes.Any(x => x.Active && x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container))
                    {
                        await DeleteMissionByContainer(bookingData, fbToken, objReportIdList, false);
                    }
                    else
                    {
                        await DeleteMission(bookingData, fbToken, objReportIdList, false);
                    }

                    return false;
                }
            }

        }


        private async Task<bool> MapPoColorTransactionToReportProducts(InspTransaction bookingData, InspProductTransaction productItem,
                                List<InspPurchaseOrderColorTransaction> poColorTransactions, List<int> objReportIdList,
                                string fbToken, string fbReportId, int apiReportId)
        {

            bool isMappeedSuccess = false;

            var otherThanFirstColorList = poColorTransactions.Where(x => x.Active.Value).Skip(1).ToList();
            if (otherThanFirstColorList != null && otherThanFirstColorList.Any())
            {
                var combineProductList = MapColorTransactionWithFBMapProductReport(productItem, otherThanFirstColorList);

                // map the report id 
                isMappeedSuccess = await MapCombineProductsToFBMission(combineProductList, Convert.ToInt32(fbReportId), fbToken, bookingData, apiReportId);

                // if any failure happen at the time of mapping combine products with existing report - revert back
                if (!isMappeedSuccess)
                {
                    await DeleteMission(bookingData, fbToken, objReportIdList, false);
                    isMappeedSuccess = false;
                }

            }
            else
                isMappeedSuccess = true;

            return isMappeedSuccess;
        }


        private List<FBMapProductReport> MapColorTransactionWithFBMapProductReport(InspProductTransaction productItem, List<InspPurchaseOrderColorTransaction> colorTransactions)
        {
            var combineProductList = new List<FBMapProductReport>();

            foreach (var colorTransItem in colorTransactions)
            {
                combineProductList.Add(new FBMapProductReport()
                {
                    product = productItem.Product?.FbCusProdId,
                    quantity = colorTransItem?.BookingQuantity ?? 0,
                    poDetailId = colorTransItem?.PoTrans?.Id,
                    poNumber = colorTransItem?.PoTrans?.Po?.Pono ?? "",
                    color = colorTransItem?.ColorName,
                    destinationCountry = colorTransItem?.PoTrans?.DestinationCountry?.CountryName,
                    etd = colorTransItem?.PoTrans?.Etd != null ? colorTransItem?.PoTrans?.Etd.ToString() : string.Empty,
                    addTemplate = productItem.Fbtemplate?.FbTemplateId.GetValueOrDefault(),
                    customerReferenceNo = colorTransItem?.PoTrans?.CustomerReferencePo,
                    isEcopack = productItem.IsEcopack.GetValueOrDefault(),
                    isDisplay = productItem.IsDisplayMaster.GetValueOrDefault(),
                    factoryReference = productItem.Product?.FactoryReference,
                    ean = productItem.Product?.Barcode,
                    samplingSizeCustom = productItem?.AqlNavigation?.Fbvalue != null && productItem?.Aql == (int)AqlType.AQLCustom ? productItem?.AqlQuantity?.ToString() : ""
                });
            }

            return combineProductList;
        }

        private List<FBMapProductReport> MapCombineColorTransactionToFBMapProductReport(InspProductTransaction productItem, List<InspPurchaseOrderColorTransaction> combineOrderPoColorList, List<FBMapProductReport> combineProductList)
        {

            foreach (var poColorItem in combineOrderPoColorList)
            {
                combineProductList.Add(new FBMapProductReport()
                {
                    product = poColorItem?.ProductRef?.Product?.FbCusProdId,
                    quantity = poColorItem?.BookingQuantity ?? 0,
                    poDetailId = poColorItem?.PoTrans?.Id,
                    addTemplate = poColorItem.ProductRef?.Fbtemplate?.FbTemplateId.GetValueOrDefault(),
                    poNumber = poColorItem?.PoTrans?.Po?.Pono ?? "",
                    destinationCountry = poColorItem?.PoTrans?.DestinationCountry?.CountryName,
                    etd = poColorItem?.PoTrans?.Etd != null ? poColorItem?.PoTrans?.Etd.ToString() : string.Empty,
                    customerReferenceNo = poColorItem?.PoTrans.CustomerReferencePo,
                    isEcopack = poColorItem.ProductRef.IsEcopack.GetValueOrDefault(),
                    isDisplay = poColorItem.ProductRef.IsDisplayMaster.GetValueOrDefault(),
                    factoryReference = poColorItem.ProductRef.Product?.FactoryReference,
                    ean = poColorItem.ProductRef.Product?.Barcode,
                    samplingSizeCustom = productItem?.AqlNavigation?.Fbvalue != null && productItem?.Aql == (int)AqlType.AQLCustom ? productItem?.AqlQuantity?.ToString() : "",
                    color = poColorItem.ColorName
                });
            }

            return combineProductList;
        }

        private List<FBMapProductReport> MapCombinePODataToFBMapProductReport(InspProductTransaction productItem, List<InspPurchaseOrderTransaction> combineOrderPoList, List<FBMapProductReport> combineProductList)
        {

            foreach (var poItem in combineOrderPoList)
            {
                combineProductList.Add(new FBMapProductReport()
                {
                    product = poItem.ProductRef.Product?.FbCusProdId,
                    quantity = poItem.BookingQuantity,
                    poDetailId = poItem.Id,
                    addTemplate = poItem.ProductRef?.Fbtemplate?.FbTemplateId.GetValueOrDefault(),
                    poNumber = poItem?.Po?.Pono ?? "",
                    destinationCountry = poItem?.DestinationCountry?.CountryName,
                    etd = poItem?.Etd != null ? poItem?.Etd.ToString() : string.Empty,
                    customerReferenceNo = poItem.CustomerReferencePo,
                    isEcopack = poItem.ProductRef.IsEcopack.GetValueOrDefault(),
                    isDisplay = poItem.ProductRef.IsDisplayMaster.GetValueOrDefault(),
                    factoryReference = poItem.ProductRef.Product?.FactoryReference,
                    ean = poItem.ProductRef.Product?.Barcode,
                    samplingSizeCustom = productItem?.AqlNavigation?.Fbvalue != null && productItem?.Aql == (int)AqlType.AQLCustom ? productItem?.AqlQuantity?.ToString() : ""
                });
            }

            return combineProductList;
        }




        private async Task<bool> MapPoProductTransactiontoReportProducts(InspTransaction bookingData, InspProductTransaction productItem,
                    List<InspPurchaseOrderTransaction> poTransactions, List<int> objReportIdList,
                    string fbToken, string fbReportId, int apiReportId)
        {
            var combineProductList = new List<FBMapProductReport>();
            bool isMappeedSuccess = false;

            var otherThanFirstPOList = poTransactions.Where(x => x.Active.Value).Skip(1);
            if (otherThanFirstPOList != null && otherThanFirstPOList.Any())
            {
                foreach (var poItem in otherThanFirstPOList)
                {
                    combineProductList.Add(new FBMapProductReport()
                    {
                        product = productItem.Product?.FbCusProdId,
                        quantity = poItem.BookingQuantity,
                        poDetailId = poItem.Id,
                        poNumber = poItem?.Po?.Pono ?? "",
                        destinationCountry = poItem?.DestinationCountry?.CountryName,
                        etd = poItem?.Etd != null ? poItem?.Etd.ToString() : string.Empty,
                        addTemplate = productItem.Fbtemplate?.FbTemplateId.GetValueOrDefault(),
                        customerReferenceNo = poItem.CustomerReferencePo,
                        isEcopack = productItem.IsEcopack.GetValueOrDefault(),
                        isDisplay = productItem.IsDisplayMaster.GetValueOrDefault(),
                        factoryReference = productItem.Product?.FactoryReference,
                        ean = productItem.Product?.Barcode,
                        samplingSizeCustom = productItem?.AqlNavigation?.Fbvalue != null && productItem?.Aql == (int)AqlType.AQLCustom ? productItem?.AqlQuantity?.ToString() : ""
                    });
                }
                // map the report id 
                isMappeedSuccess = await MapCombineProductsToFBMission(combineProductList, Convert.ToInt32(fbReportId), fbToken, bookingData, apiReportId);

                // if any failure happen at the time of mapping combine products with existing report - revert back
                if (!isMappeedSuccess)
                {
                    await DeleteMission(bookingData, fbToken, objReportIdList, false);
                    isMappeedSuccess = false;
                }
            }
            else
                isMappeedSuccess = true;

            return isMappeedSuccess;
        }

        /// <summary>
        /// create Mission products by container 
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        private async Task<Boolean> CreateMissionProductbyContainerwithReports(InspTransaction bookingData, string fbToken, int containerId)
        {
            List<int> objReportIdList = new List<int>();
            List<FBMissionProductReportUser> objMissionProductsList = new List<FBMissionProductReportUser>();
            try
            {
                List<FBMapProductReport> combineProductList = null;
                var activeContainerTransaction = bookingData.InspContainerTransactions.Where(x => x.Active.HasValue && x.Active.Value);

                if (activeContainerTransaction != null && activeContainerTransaction.Any())
                {
                    // if container is exist then create only specific container report - this flow is mainly from FB admin page
                    if (containerId > 0)
                    {
                        activeContainerTransaction = activeContainerTransaction.Where(x => x.ContainerId == containerId);
                    }

                    // process every container for creating fb Report
                    foreach (var item in activeContainerTransaction)
                    {
                        // check report is not created
                        if (item.FbReportId == null)
                        {

                            // Select only First Record for creating the Report
                            var containerOfFirstPO = item.InspPurchaseOrderTransactions.FirstOrDefault(x => x.Active.Value);

                            FBMissionProductReportUser objMissionProduct = new FBMissionProductReportUser()
                            {
                                purchaseOrderNumber = containerOfFirstPO?.Po?.Pono,
                                product = containerOfFirstPO?.ProductRef?.Product?.FbCusProdId == null ? 0 : containerOfFirstPO?.ProductRef?.Product?.FbCusProdId.Value,
                                quantity = containerOfFirstPO.BookingQuantity,
                                customerReferenceNo = containerOfFirstPO?.CustomerReferencePo,
                                containerId = containerOfFirstPO?.ContainerRefId.GetValueOrDefault(),
                                createReport = true,
                                addTemplate = containerOfFirstPO?.ProductRef?.Fbtemplate?.FbTemplateId.GetValueOrDefault(),
                                copyUsersFromMission = true,
                                ean = containerOfFirstPO?.ProductRef?.Product?.Barcode,
                                destinationCountry = containerOfFirstPO?.DestinationCountry?.CountryName,
                                etd = containerOfFirstPO?.Etd != null ?
                                          containerOfFirstPO?.Etd.ToString() : string.Empty,
                                factoryReference = containerOfFirstPO?.ProductRef?.Product?.FactoryReference
                            };

                            objMissionProductsList.Add(objMissionProduct);

                            var fbBase = _fbSettings.BaseUrl;
                            var fbRequest = string.Format(_fbSettings.MissionReportRequestUrl, bookingData.FbMissionId);

                            _logger.LogInformation("API-FB : FB mission product report creation request start ");
                            _logger.LogInformation(JsonConvert.SerializeObject(objMissionProduct));

                            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                            {
                                MissionId = bookingData?.FbMissionId,
                                BookingId = bookingData?.Id,
                                RequestUrl = fbRequest,
                                LogInformation = JsonConvert.SerializeObject(objMissionProduct)
                            });

                            // create mission products and reports and copyUsersFromMission
                            HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objMissionProduct, fbBase, fbToken);

                            _logger.LogInformation("API-FB : FB mission product report creation request end ");
                            _logger.LogInformation(httpResponse.StatusCode.ToString());

                            if (httpResponse.StatusCode == HttpStatusCode.Created)
                            {
                                // get the Fb report and mission details from FB Response
                                var fbData = getFBResultData(httpResponse);

                                _logger.LogInformation("API-FB : Report ID :" + fbData.fbReportId + "");
                                // _logger.LogInformation("API-FB : Product MAP ID :" + fbData.fbMissionProductId + "");
                                int reportNumber;

                                if (Int32.TryParse(fbData.fbReportId, out reportNumber))
                                {
                                    // save Fb Report details in API Report table 
                                    fbData.InspectionId = bookingData.Id;
                                    var reportData = await SaveFbReport(fbData);
                                    if (reportData != null && reportData.Result == FbStatusResponseResult.success)
                                    {
                                        objReportIdList.Add(Convert.ToInt32(reportData.ReportId));
                                    }

                                    // map product related po to fb Report and mission products
                                    combineProductList = new List<FBMapProductReport>();

                                    // skip first item which is already created on top
                                    var otherThanFirstPOList = item.InspPurchaseOrderTransactions.Where(x => x.Active.Value).Skip(1);

                                    if (otherThanFirstPOList != null && otherThanFirstPOList.Any())
                                    {
                                        // Create List of po and products for mapping to same report - just created above
                                        foreach (var poItem in otherThanFirstPOList)
                                        {
                                            combineProductList.Add(new FBMapProductReport()
                                            {
                                                product = poItem?.ProductRef?.Product?.FbCusProdId,
                                                quantity = poItem.BookingQuantity,
                                                poDetailId = poItem.Id,
                                                poNumber = poItem?.Po?.Pono ?? "",
                                                customerReferenceNo = poItem.CustomerReferencePo,
                                                containerId = poItem?.ContainerRefId.GetValueOrDefault(),

                                                ean = poItem?.ProductRef?.Product?.Barcode,
                                                destinationCountry = poItem?.DestinationCountry?.CountryName,
                                                etd = poItem?.Etd != null ? poItem?.Etd.ToString() : string.Empty,
                                                factoryReference = poItem?.ProductRef?.Product?.FactoryReference
                                            });
                                        }
                                        // Report map Request with Booking po products to FB
                                        var isMappedSuccess = await MapContainerProductsToFBMission(combineProductList, Convert.ToInt32(fbData.fbReportId), fbToken, bookingData);
                                        if (!isMappedSuccess)
                                        {
                                            await DeleteMission(bookingData, fbToken, objReportIdList, false);
                                            return false;
                                        }
                                    }

                                    if (reportData != null && reportData.Result == FbStatusResponseResult.success)
                                    {
                                        // update Report id for each item.
                                        item.FbReportId = reportData.ReportId;
                                    }
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    await _bookingRepo.EditInspectionBooking(bookingData);
                }

                return true;
            }
            catch (Exception ex)
            {
                if (bookingData.InspTranServiceTypes.Any(x => x.Active && x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container))
                {
                    await DeleteMissionByContainer(bookingData, fbToken, objReportIdList, false);
                }
                else
                {
                    await DeleteMission(bookingData, fbToken, objReportIdList, false);
                }


                return false;
            }

        }


        /// <summary>
        /// Delete QC and CS
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task RemoveQCAndCSFromSchedule(int bookingId)
        {
            try
            {
                if (bookingId > 0) // if booking is valid
                {
                    //get QC list from sch_schedule_qc table using booking id
                    var qcList = await _schRepo.GetQCDetails(bookingId);

                    //get CS list from sch_schedule_cs table using booking id
                    var csList = await _schRepo.GetCSDetails(bookingId);

                    if (qcList != null && qcList.Count() > 0)
                    {
                        foreach (var qcItem in qcList)
                        {
                            qcItem.Active = false;
                            qcItem.DeletedBy = _ApplicationContext.UserId;
                            qcItem.DeletedOn = DateTime.Now;
                        }
                        _bookingRepo.EditEntities(qcList);
                    }
                    if (csList != null && csList.Count() > 0)
                    {
                        foreach (var csItem in csList)
                        {
                            csItem.Active = false;
                            csItem.DeletedBy = _ApplicationContext.UserId;
                            csItem.DeletedOn = DateTime.Now;
                        }
                        _bookingRepo.EditEntities(csList);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save Fb Report
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        private async Task<FbStatusResponse> SaveFbReport(FbReportMainDetail reportData)
        {
            StringBuilder failureResult = new StringBuilder();
            try
            {
                if (reportData == null || string.IsNullOrEmpty(reportData.fbReportId))
                {
                    return new FbStatusResponse() { Result = FbStatusResponseResult.failure };
                }

                FbReportDataRequest objReport = new FbReportDataRequest()
                {
                    ReportTitle = reportData.reportTitle,
                    MissionTitle = reportData.missionTitle,
                    FbReportId = Convert.ToInt32(reportData.fbReportId)
                };

                return await _fbReportManager.SaveFBReportInfo(Convert.ToInt32(reportData.fbReportId), reportData.InspectionId, objReport);
            }
            catch (Exception ex)
            {
                failureResult.Append(FBFailure.CustomerSave.ToString() + "--done" + "\n");
                failureResult.Append(FBFailure.SupplierSave.ToString() + "--done" + "\n");
                failureResult.Append(FBFailure.FactorySave.ToString() + "--done" + "\n");
                failureResult.Append(FBFailure.ProductSave.ToString() + "--done" + "\n");
                failureResult.Append(FBFailure.MissionSave.ToString() + "--done" + "\n");
                failureResult.Append(FBFailure.UserSave.ToString() + "--done" + "\n");
                failureResult.Append(FBFailure.ReportSave.ToString() + "--failure" + "\n");
                ex.Source = failureResult.ToString();
                throw new Exception();
            }
        }


        /// <summary>
        /// Map user data to FB Report.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> CreateUserToFBMission(InspTransaction bookingData, string fbToken, SaveScheduleRequest scheduleData)
        {
            try
            {
                var qcList = scheduleData.AllocationCSQCStaff.SelectMany(x => x.QC).Select(x => x.StaffID).Distinct().ToList();
                var additionalQCList = scheduleData.AllocationCSQCStaff.SelectMany(x => x.AdditionalQC).Select(x => x.StaffID).Distinct().ToList();
                var cSList = scheduleData.AllocationCSQCStaff.SelectMany(x => x.CS).Select(x => x.StaffID).Distinct().ToList();
                var entityId = _filterService.GetCompanyId();
                return await CreateUserToFBMission(bookingData.Id, bookingData.FbMissionId, qcList, additionalQCList, cSList, fbToken, entityId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Boolean> CreateUserToFBMissionForAudit(AudTransaction auditData, string fbToken)
        {
            try
            {
                var auditorList = await _auditRepository.GetAuditorDetails(auditData.Id);
                var csList = await _auditRepository.GetAuditCSDetails(auditData.Id);
                if (auditorList.Any() || csList.Any())
                    return await CreateUserToFBMission(auditData.Id, auditData.FbmissionId, auditorList.Select(x => x.StaffId).Distinct().ToList(), null, csList.Select(x => x.StaffId).Distinct().ToList(), fbToken, auditData.EntityId.GetValueOrDefault());
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// map the qc, additional qc and report checker data to mission
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="fbMissionId"></param>
        /// <param name="qcIdList"></param>
        /// <param name="additionalQcList"></param>
        /// <param name="csList"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<bool> CreateUserToFBMission(int bookingId, int? fbMissionId, List<int> qcIdList, List<int> additionalQcList, List<int> csList, string fbToken, int entityId)
        {
            List<FBMapUserReport> objRequestMappedUserList = new List<FBMapUserReport>();
            if (qcIdList != null)
                // main qc added 
                await MapFbMissionUserRequestObject(objRequestMappedUserList, qcIdList, FBConstants.InspectorRole, FBConstants.Classification_Reporter, fbToken, entityId);

            if (additionalQcList != null)
                // Additional qc added 
                await MapFbMissionUserRequestObject(objRequestMappedUserList, additionalQcList, FBConstants.InspectorRole, FBConstants.Additional_Reporter, fbToken, entityId);

            if (csList != null)
                // main report checker added 
                await MapFbMissionUserRequestObject(objRequestMappedUserList, csList, FBConstants.ReviewerRole, FBConstants.Classification_Reviewer, fbToken, entityId);

            //map the user to fb mission             
            return await MapUserToFBMissionReport(fbMissionId == null ? 0 : fbMissionId.Value, objRequestMappedUserList, fbToken, bookingId);
        }

        /// <summary>
        /// common method for the map the qc, additional qc and report checker
        /// </summary>
        /// <param name="objRequestMappedUserList"></param>
        /// <param name="staffIds"></param>
        /// <param name="roleId"></param>
        /// <param name="classification"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task MapFbMissionUserRequestObject(List<FBMapUserReport> objRequestMappedUserList, List<int> staffIds, string roleId, string classification, string fbToken, int entityId)
        {
            foreach (var staffId in staffIds)
            {
                var fbUserId = await getFbUserID(staffId, roleId, fbToken, entityId);
                if (!string.IsNullOrEmpty(fbUserId))
                {
                    objRequestMappedUserList.Add(new FBMapUserReport()
                    {
                        user = fbUserId,
                        classification = classification,
                        status = FBConstants.Active,
                        addInReports = true
                    });
                }

            }
        }

        private async Task<string> getFbUserID(int staffId, string roleId, string fbToken, int entityId)
        {

            string fbUserId = "";

            var qcUserDetails = await _humanRepository.GetStaffDetailsByIdAndEntityId(staffId, entityId);

            var FBUserId = qcUserDetails?.ItUserMasters.Where(x => x.Active).Select(x => x.FbUserId).FirstOrDefault();

            if (FBUserId == null)
            {
                fbUserId = await CreateUser(fbToken, roleId, qcUserDetails);
            }

            else
            {
                fbUserId = FBUserId.ToString();
            }

            return fbUserId;
        }

        /// <summary>
        /// Create User and map to FB API account.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="fbToken"></param>
        /// <param name="objReportIdList"></param>
        /// <returns></returns>
        private async Task<string> CreateUser(string fbToken, string roleId, HrStaff staffEntity)
        {
            var fbBase = _fbSettings.BaseUrl;
            var fbRequest = _fbSettings.UserRequestUrl;

            // check the user already present 
            string fbRecordId = await GetFbUserIfEmailAlreadyExist(fbToken, staffEntity);

            // user not exist in fb then create the new fb user 
            if (string.IsNullOrEmpty(fbRecordId) && !string.IsNullOrEmpty(staffEntity?.CompanyEmail))
            {
                FBUserData objUser = new FBUserData()
                {
                    account = 2727, // default Account mapping with API.
                    firstname = staffEntity?.PersonName,
                    lastname = "",
                    email = staffEntity?.CompanyEmail != null ? staffEntity?.CompanyEmail.ToString() : "",
                    role = roleId,
                    status = FBConstants.Active
                };

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objUser, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.Created)
                {
                    fbRecordId = getFbRecordIdFromResult(httpResponse);
                    if (staffEntity != null && staffEntity.ItUserMasters.Any(x => x.Active))
                    {
                        staffEntity.ItUserMasters.FirstOrDefault(x => x.Active).FbUserId = Convert.ToInt32(fbRecordId);
                        _humanRepository.Save(staffEntity);
                    }
                }
            }

            return fbRecordId;
        }

        /// <summary>
        /// get fb user if email already exist in FB
        /// </summary>
        /// <param name="fbToken"></param>
        /// <param name="staffEntity"></param>
        /// <returns></returns>
        private async Task<string> GetFbUserIfEmailAlreadyExist(string fbToken, HrStaff staffEntity)
        {
            string fbRecordId = string.Empty;
            var fbBase = _fbSettings.BaseUrl;
            var fbRequest = _fbSettings.UserRequestUrl;

            if (!string.IsNullOrEmpty(staffEntity?.CompanyEmail.ToString()))
            {
                fbRequest = fbRequest + "?filter[email]=" + staffEntity?.CompanyEmail.ToString() + "";
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    fbRecordId = getFbUserIdIdFromResponse(httpResponse);

                    // fb user not mapped for apilink user 
                    if (!string.IsNullOrWhiteSpace(fbRecordId) && staffEntity.ItUserMasters.Any(x => x.Active) && !staffEntity.ItUserMasters.Any(x => x.Active && x.FbUserId == Convert.ToInt32(fbRecordId)))
                    {
                        staffEntity.ItUserMasters.FirstOrDefault(x => x.Active).FbUserId = Convert.ToInt32(fbRecordId);
                        _humanRepository.Save(staffEntity);
                    }
                    else
                    {
                        fbRecordId = string.Empty;
                    }
                }
            }
            return fbRecordId;
        }



        /// <summary>
        /// Delete mission product and reports based on the status check.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fbToken"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        private async Task<DeleteReportResponse> DeleteMissionProducts(InspTransaction entity, string fbToken, int fbreportId, int apiReportId)
        {
            try
            {
                DeleteReportResponse objRepsonse = new DeleteReportResponse();

                if (entity.FbMissionId.Value > 0)
                {
                    var missionInfo = await GetMissionProductsInfo(entity.FbMissionId.Value, fbToken);

                    if (missionInfo != null && missionInfo.mappedProducts != null)
                    {
                        var reportInfo = await GetReportInfo(fbreportId, fbToken);

                        if (missionInfo.status != FBConstants.Status_Completed && reportInfo.statusFilling != "validated")
                        {

                            // delete the report from fb 
                            var fbBase = _fbSettings.BaseUrl;
                            var fbRequest = string.Format(_fbSettings.ReportDeleteRequestUrl, fbreportId);

                            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                            {
                                MissionId = entity?.FbMissionId,
                                BookingId = entity?.Id,
                                RequestUrl = fbRequest,
                                ReportId = fbreportId,
                                CreatedBy = _ApplicationContext.UserId,
                                LogInformation = "API-FB : FB Report delete request start"
                            });

                            HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Send, fbRequest, null, fbBase, fbToken);

                            if (httpResponse.StatusCode == HttpStatusCode.OK)
                            {

                                if (entity.InspProductTransactions.
                                 Any(x => x.Active.HasValue && x.Active.Value && x.FbReportId == apiReportId))
                                {
                                    // update all the rows if the report is same - combine case
                                    var reportProductList = entity.InspProductTransactions.
                                        Where(x => x.Active.HasValue && x.Active.Value && x.FbReportId == apiReportId).ToList();

                                    foreach (var item in reportProductList)
                                    {
                                        item.FbReportId = null;
                                    }
                                }

                                await _bookingRepo.EditInspectionBooking(entity);
                                // Delete fb reports and related details.
                                await _fbReportManager.DeleteFBReportDetails(apiReportId);

                                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                                {
                                    MissionId = entity?.FbMissionId,
                                    BookingId = entity?.Id,
                                    RequestUrl = fbRequest,
                                    ReportId = fbreportId,
                                    CreatedBy = _ApplicationContext.UserId,
                                    LogInformation = "API-FB : FB Report delete request end"
                                });

                            }
                            else
                            {
                                objRepsonse.Result = DeleteReportResponseResult.Failure;
                                return objRepsonse;
                            }


                            objRepsonse.Result = DeleteReportResponseResult.Success;
                            return objRepsonse;

                        }
                        else
                        {
                            objRepsonse.Result = DeleteReportResponseResult.ReportFilledByQC;
                            return objRepsonse;
                        }
                    }
                    else
                    {
                        objRepsonse.Result = DeleteReportResponseResult.MissionNotExist;
                        return objRepsonse;
                    }

                }
                else
                {
                    objRepsonse.Result = DeleteReportResponseResult.MissionNotExist;
                    return objRepsonse;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete Mission Reports and containers
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fbToken"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        private async Task<DeleteReportResponse> DeleteMissionContainers(InspTransaction entity, string fbToken, int reportId, int apiReportId)
        {
            try
            {
                DeleteReportResponse objRepsonse = new DeleteReportResponse();

                if (entity.FbMissionId.Value > 0)
                {
                    var missionInfo = await GetMissionProductsInfo(entity.FbMissionId.Value, fbToken);

                    if (missionInfo != null)
                    {
                        var reportInfo = await GetReportInfo(reportId, fbToken);

                        if (missionInfo.status != FBConstants.Status_Completed && reportInfo.statusFilling != "validated")
                        {
                            // delete the report from fb 
                            var fbBase = _fbSettings.BaseUrl;
                            var fbRequest = string.Format(_fbSettings.ReportDeleteRequestUrl, reportId);


                            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                            {
                                MissionId = entity?.FbMissionId,
                                BookingId = entity?.Id,
                                RequestUrl = fbRequest,
                                ReportId = reportId,
                                CreatedBy = _ApplicationContext.UserId,
                                LogInformation = "API-FB : FB Report delete request start"
                            });

                            HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Send, fbRequest, null, fbBase, fbToken);

                            if (httpResponse.StatusCode == HttpStatusCode.OK)
                            {

                                if (entity.InspContainerTransactions.
                                   Any(x => x.Active.HasValue && x.Active.Value && x.FbReportId == apiReportId))
                                {

                                    // update all the rows if the report is same - combine case
                                    var reportProductList = entity.InspContainerTransactions.
                                        Where(x => x.Active.HasValue && x.Active.Value && x.FbReportId == apiReportId).ToList();

                                    foreach (var item in reportProductList)
                                    {
                                        item.FbReportId = null;
                                    }
                                }
                                await _bookingRepo.EditInspectionBooking(entity);

                                // Delete fb reports and related details.
                                await _fbReportManager.DeleteFBReportDetails(apiReportId);

                                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                                {
                                    MissionId = entity?.FbMissionId,
                                    BookingId = entity?.Id,
                                    RequestUrl = fbRequest,
                                    ReportId = reportId,
                                    CreatedBy = _ApplicationContext.UserId,
                                    LogInformation = "API-FB : FB Report delete request end"
                                });
                            }

                            else
                            {
                                objRepsonse.Result = DeleteReportResponseResult.Failure;
                                return objRepsonse;
                            }

                            objRepsonse.Result = DeleteReportResponseResult.Success;
                            return objRepsonse;
                        }

                        else
                        {
                            objRepsonse.Result = DeleteReportResponseResult.ReportFilledByQC;
                            return objRepsonse;
                        }
                    }
                    else
                    {
                        objRepsonse.Result = DeleteReportResponseResult.MissionNotExist;
                        return objRepsonse;
                    }
                }
                else
                {
                    objRepsonse.Result = DeleteReportResponseResult.MissionNotExist;
                    return objRepsonse;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// map the 
        /// </summary>
        /// <param name="missionId"></param>
        /// <param name="objRequestMappedUserList"></param>
        /// <param name="fbToken"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        private async Task<Boolean> MapUserToFBMissionReport(int missionId, List<FBMapUserReport> objRequestMappedUserList, string fbToken, int? bookingId)
        {
            try
            {
                List<FBMapUserReport> mappedUserListFromRequest = new List<FBMapUserReport>();
                FBDeleteUserFromReport objDeleteReports = new FBDeleteUserFromReport() { deleteInReports = true };
                var jsondata = JsonConvert.SerializeObject(objDeleteReports);


                if (missionId > 0)
                {
                    var missionInfo = await GetMissionUserInfo(missionId, fbToken);
                    if (missionInfo != null && missionInfo.mappedUsers != null)
                    {
                        var removedMappedUserList = missionInfo.mappedUsers.Where(x => !objRequestMappedUserList.Select(y => y.user).Contains(x.userId));

                        var existingMappedUsers = missionInfo.mappedUsers.Where(x => objRequestMappedUserList.Select(y => y.user).Contains(x.userId));

                        foreach (var item in removedMappedUserList)
                        {
                            if ((item.userRole == FBConstants.Classification_Reporter || item.userRole == FBConstants.Additional_Reporter)
                                && missionInfo.status != FBConstants.Status_Completed) //qc user can remove only at not completed status.
                            {
                                var fbBase1 = _fbSettings.BaseUrl;
                                var fbRequest1 = string.Format("api/api/mission/{0}/mission-user", missionId) + "/" + item.mapId;

                                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                                {
                                    MissionId = missionId,
                                    BookingId = bookingId,
                                    RequestUrl = fbRequest1,
                                    LogInformation = "Mission QC Users Delete Request Start"
                                });

                                HttpResponseMessage httpResponse1 = await _helper.SendRequestToPartnerAPI(Method.Send, fbRequest1, objDeleteReports, fbBase1, fbToken);

                                if (httpResponse1.StatusCode == HttpStatusCode.OK)
                                {

                                }
                            }

                            else if (item.userRole == FBConstants.Classification_Reviewer && missionInfo.status != FBConstants.Status_Completed) //cs user can remove only at not completed status.
                            {
                                var fbBase1 = _fbSettings.BaseUrl;
                                var fbRequest1 = string.Format("api/api/mission/{0}/mission-user", missionId) + "/" + item.mapId;

                                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                                {
                                    MissionId = missionId,
                                    BookingId = bookingId,
                                    RequestUrl = fbRequest1,
                                    LogInformation = "Mission Reviewer Users Delete Request Start"
                                });

                                HttpResponseMessage httpResponse1 = await _helper.SendRequestToPartnerAPI(Method.Send, fbRequest1, objDeleteReports, fbBase1, fbToken);

                                if (httpResponse1.StatusCode == HttpStatusCode.OK)
                                {

                                }
                            }
                        }

                        foreach (var mappedUser in objRequestMappedUserList)
                        {
                            if (!existingMappedUsers.Any() || !existingMappedUsers.Any(x => x.userId == mappedUser.user && x.userRole == mappedUser.classification))
                            {
                                mappedUserListFromRequest.Add(mappedUser);
                            }
                        }

                        if (mappedUserListFromRequest.Any())
                        {
                            var fbBase = _fbSettings.BaseUrl;
                            var fbRequest = string.Format("api/api/mission/{0}/mission-users", missionId);

                            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                            {
                                MissionId = missionId,
                                BookingId = bookingId,
                                RequestUrl = fbRequest,
                                LogInformation = JsonConvert.SerializeObject(mappedUserListFromRequest)
                            });
                            // map user to mission
                            HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, mappedUserListFromRequest, fbBase, fbToken);

                            if (httpResponse.StatusCode == HttpStatusCode.Created)
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    else // Add new users to mission 
                    {
                        foreach (var mappedUser in objRequestMappedUserList)
                        {
                            mappedUserListFromRequest.Add(mappedUser);
                        }

                        if (mappedUserListFromRequest.Count() > 0)
                        {
                            var fbBase = _fbSettings.BaseUrl;
                            var fbRequest = string.Format("api/api/mission/{0}/mission-users", missionId);

                            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                            {
                                MissionId = missionId,
                                BookingId = bookingId,
                                RequestUrl = fbRequest,
                                LogInformation = JsonConvert.SerializeObject(mappedUserListFromRequest)
                            });

                            HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, mappedUserListFromRequest, fbBase, fbToken);

                            if (httpResponse.StatusCode == HttpStatusCode.Created)
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class FbReportInfo
        {
            public string status { get; set; }
            public string statusPreparation { get; set; }
            public string statusFilling { get; set; }
            public string statusReview { get; set; }
            public List<FbUserReportMapData> mappedUsers { get; set; }
            public List<FbProdutsReportMapData> mappedProducts { get; set; }
        }


        public class FbReportMainDetail
        {
            public string fbReportId { get; set; }
            public string fbProductId { get; set; }
            public string fbMissionId { get; set; }
            // public string fbMissionProductId { get; set; }
            public string poNumber { get; set; }
            public string missionTitle { get; set; }
            public string missionStatus { get; set; }
            public string reportTitle { get; set; }
            public string status { get; set; }
            public string statusPreparation { get; set; }
            public string statusFilling { get; set; }
            public string statusReview { get; set; }
            public int InspectionId { get; set; }
        }
        public class FbMissionUrlResponse
        {
            public string FbId { get; set; }
            public string Url { get; set; }
        }
        public class FbUserReportMapData
        {
            public string userId { get; set; }
            public string userRole { get; set; }
            public string mapId { get; set; }
        }
        public class FbProdutsReportMapData
        {
            public string productId { get; set; }
            public string mapId { get; set; }
        }

        private async Task<FbReportInfo> GetMissionProductsInfo(int missionId, string fbToken)
        {
            try
            {

                List<FbProdutsReportMapData> mappedProductList = new List<FbProdutsReportMapData>();
                FbReportInfo objReport = new FbReportInfo();

                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format("api/api/mission/{0}/mission-products", missionId);
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {

                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    JObject reportDataJson = JObject.Parse(reportData.Result);
                    if (reportDataJson != null && reportDataJson.GetValue("data") != null)
                    {

                        foreach (var item in reportDataJson.GetValue("data").ToArray())
                        {
                            if (item["type"].ToString() == "MissionProduct")
                            {
                                mappedProductList.Add(new FbProdutsReportMapData()
                                {
                                    mapId = item["id"].ToString(),
                                    productId = item["relationships"]["product"]["data"]["id"].ToString()
                                });
                            }
                        }

                        foreach (var item in reportDataJson.GetValue("included").ToArray())
                        {
                            if (item["type"].ToString() == "Mission")
                            {
                                objReport.status = item["attributes"]["status"].ToString();
                            }
                        }

                        objReport.mappedProducts = mappedProductList;
                    }

                }

                return objReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<FbReportInfo> GetMissionUserInfo(int missionId, string fbToken)
        {
            try
            {

                List<FbUserReportMapData> mappedUserList = new List<FbUserReportMapData>();
                FbReportInfo objReport = new FbReportInfo();

                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format("api/api/mission/{0}/mission-users", missionId);
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {

                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    JObject reportDataJson = JObject.Parse(reportData.Result);
                    if (reportDataJson != null && reportDataJson.GetValue("data") != null)
                    {

                        foreach (var item in reportDataJson.GetValue("data").ToArray())
                        {
                            if (item["type"].ToString() == "MissionUser")
                            {
                                mappedUserList.Add(new FbUserReportMapData()
                                {
                                    mapId = item["id"].ToString(),
                                    userId = item["relationships"]["user"]["data"]["id"].ToString(),
                                    userRole = item["attributes"]["classification"].ToString()

                                });
                            }
                        }

                        foreach (var item in reportDataJson.GetValue("included").ToArray())
                        {
                            if (item["type"].ToString() == "Mission")
                            {
                                objReport.status = item["attributes"]["status"].ToString();

                                //objReport.statusFilling = item["attributes"]["statusFilling"].ToString();
                                //objReport.statusPreparation = item["attributes"]["statusPreparation"].ToString();
                                //objReport.statusReview = item["attributes"]["statusReview"].ToString();
                            }
                        }

                        objReport.mappedUsers = mappedUserList;
                    }

                }

                return objReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get FB Report details and status
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<FbReportMainDetail> GetFullBridgeReportDetails(int fbReportId, string fbToken)
        {
            try
            {
                FbReportMainDetail objReport = new FbReportMainDetail();

                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format(_fbSettings.ReportDeleteRequestUrl, fbReportId);
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    JObject reportDataJson = JObject.Parse(reportData.Result);
                    if (reportDataJson != null && reportDataJson.GetValue("data") != null)
                    {

                        objReport.reportTitle = reportDataJson["data"]["attributes"]["title"].ToString();
                        objReport.status = reportDataJson["data"]["attributes"]["status"].ToString();
                        objReport.statusPreparation = reportDataJson["data"]["attributes"]["statusPreparation"].ToString();
                        objReport.statusFilling = reportDataJson["data"]["attributes"]["statusFilling"].ToString();
                        objReport.statusReview = reportDataJson["data"]["attributes"]["statusReview"].ToString();

                        foreach (var item in reportDataJson.GetValue("included").ToArray())
                        {
                            if (item["type"].ToString() == "Mission")
                            {
                                objReport.missionTitle = item["attributes"]["reference"].ToString();
                                objReport.missionStatus = item["attributes"]["status"].ToString();
                            }
                        }

                    }

                    return objReport;

                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get Full bridge Information of particular report
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<string> GetFullBridgeReportInfo(int fbReportId, string fbToken)
        {
            try
            {
                var fbBase = _fbSettings.BaseUrl;

                var fbRequest = string.Format(_fbSettings.ReportInfo, fbReportId);

                // backend api from FB.
                fbRequest = fbRequest + "?token=" + fbToken;

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, null);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    return reportData.Result;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<FbReportInfo> GetReportInfo(int fbReportId, string fbToken)
        {
            try
            {
                List<FbUserReportMapData> mappedUserList = new List<FbUserReportMapData>();
                FbReportInfo objReport = new FbReportInfo();

                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format(_fbSettings.ReportUserRequestUrl, fbReportId);
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {

                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    JObject reportDataJson = JObject.Parse(reportData.Result);
                    if (reportDataJson != null && reportDataJson.GetValue("data") != null)
                    {

                        foreach (var item in reportDataJson.GetValue("data").ToArray())
                        {
                            if (item["type"].ToString() == "ReportUser")
                            {
                                mappedUserList.Add(new FbUserReportMapData()
                                {
                                    mapId = item["id"].ToString(),
                                    userId = item["relationships"]["user"]["data"]["id"].ToString(),
                                    userRole = item["attributes"]["classification"].ToString()

                                });
                            }
                        }

                        foreach (var item in reportDataJson.GetValue("included").ToArray())
                        {
                            if (item["type"].ToString() == "Report")
                            {
                                objReport.status = item["attributes"]["status"].ToString();
                                objReport.statusFilling = item["attributes"]["statusFilling"].ToString();
                                objReport.statusPreparation = item["attributes"]["statusPreparation"].ToString();
                                objReport.statusReview = item["attributes"]["statusReview"].ToString();
                            }
                        }

                        objReport.mappedUsers = mappedUserList;
                    }

                }

                return objReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Boolean> MapCombineProductsToFBMission(List<FBMapProductReport> combineProductList, int FbReportId, string fbToken, InspTransaction bookingData, int apiReportId)
        {
            try
            {
                List<FBMissionProducts> listOfProductswithPO = new List<FBMissionProducts>();

                if (combineProductList != null && combineProductList.Any())
                {

                    foreach (var productReport in combineProductList)
                    {

                        FBMissionProducts objMissionProduct = new FBMissionProducts()
                        {
                            product = productReport.product,
                            quantity = productReport.quantity,
                            purchaseOrderNumber = productReport.poNumber,
                            customerReferenceNo = productReport.customerReferenceNo,
                            ean = productReport.ean,
                            etd = productReport.etd,
                            destinationCountry = productReport.destinationCountry,
                            factoryReference = productReport.factoryReference,
                            isDisplay = productReport.isDisplay,
                            isEcopack = productReport.isEcopack,
                            color = productReport.color,
                            addInMission = false
                        };

                        listOfProductswithPO.Add(objMissionProduct);
                    }


                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.ProductFBReportRequestUrl, FbReportId);


                    // Add Full bridge Log information for combine products mapping list          

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        MissionId = bookingData?.FbMissionId,
                        BookingId = bookingData?.Id,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(listOfProductswithPO)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, listOfProductswithPO, fbBase, fbToken);

                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        foreach (var prodcutPoData in combineProductList)
                        {
                            if (bookingData != null)
                            {
                                bookingData.InspPurchaseOrderTransactions.FirstOrDefault(x => (x.Active.HasValue && x.Active.Value)
                                        && x.Id == prodcutPoData.poDetailId).ProductRef.FbReportId = apiReportId;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// map container products to fb mission
        /// </summary>
        /// <param name="combineProductList"></param>
        /// <param name="FbReportId"></param>
        /// <param name="missionId"></param>
        /// <param name="fbToken"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private async Task<Boolean> MapContainerProductsToFBMission(List<FBMapProductReport> combineProductList, int FbReportId, string fbToken, InspTransaction bookingData)
        {
            try
            {
                List<FBMissionProducts> listOfProductswithPO = new List<FBMissionProducts>();

                if (combineProductList != null && combineProductList.Any())
                {

                    foreach (var productReport in combineProductList)
                    {
                        listOfProductswithPO.Add(new FBMissionProducts()
                        {
                            product = productReport.product,
                            quantity = productReport.quantity,
                            purchaseOrderNumber = productReport.poNumber,
                            customerReferenceNo = productReport.customerReferenceNo,
                            addInMission = false,
                            containerId = productReport.containerId,
                            ean = productReport.ean,
                            destinationCountry = productReport.destinationCountry,
                            etd = productReport.etd,
                            factoryReference = productReport.factoryReference
                        });
                    }

                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.ProductFBReportRequestUrl, FbReportId);

                    // Add Full bridge Log information for combine products mapping list                 

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        MissionId = bookingData?.FbMissionId,
                        BookingId = bookingData?.Id,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(listOfProductswithPO)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, listOfProductswithPO, fbBase, fbToken);

                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete the mission by mission id
        /// </summary>
        /// <param name="MissionId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> DeleteMission(InspTransaction bookingData, string fbToken, List<int> reportIds, bool IsFbAdmin)
        {
            StringBuilder failureResult = new StringBuilder();
            List<int> currentReports = new List<int>();
            try
            {
                var fbBase = _fbSettings.BaseUrl;
                bool isReportProcessed = false;

                if (bookingData.FbMissionId != null)
                {
                    // if mission status completed then dont  delete this mission
                    var missionInfo = await GetMissionProductsInfo(bookingData.FbMissionId.Value, fbToken);

                    if (missionInfo != null && missionInfo.status == FBConstants.Status_Completed)
                    {

                        failureResult.Append(FBFailure.MissionCompleted.ToString());

                        throw new Exception();
                    }

                    // Any one report is validated then dont delete the mission 

                    var activePoTransactionList = bookingData.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value);

                    foreach (var item in activePoTransactionList)
                    {
                        if (item != null && item.FbReport != null && item.FbReport.FbReportMapId != null && item.FbReport.FbReportMapId > 0)
                        {
                            // get Report details and check the status then delete the report.

                            var reportInfo = await GetReportInfo(item.FbReport.FbReportMapId.Value, fbToken);

                            if (reportInfo != null && reportInfo.statusFilling == "validated")
                            {
                                isReportProcessed = true;
                            }
                        }
                    }

                    if (missionInfo != null && !isReportProcessed)
                    {
                        var fbRequest = string.Format(_fbSettings.MissionDeleteRequestUrl, bookingData.FbMissionId);

                        _logger.LogInformation("API-FB : FB mission delete  request start ");
                        _logger.LogInformation(fbRequest);

                        await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                        {
                            MissionId = bookingData?.FbMissionId,
                            BookingId = bookingData?.Id,
                            RequestUrl = fbRequest,
                            CreatedBy = _ApplicationContext.UserId,
                            ServiceId = (int)Service.InspectionId,
                            LogInformation = "API-FB : FB mission delete  request"
                        });

                        HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);
                        _logger.LogInformation("API-FB : FB mission delete  request end ");
                        _logger.LogInformation(httpResponse.StatusCode.ToString());
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            // set mission local in db
                            bookingData.FbMissionId = null;
                            //don't delete qc & cs when call from admin page
                            if (!IsFbAdmin)
                                await RemoveQCAndCSFromSchedule(bookingData.Id);
                        }
                    }
                    else
                    {
                        failureResult.Append(FBFailure.ReportProcessDone.ToString());
                        throw new Exception();
                    }
                }

                // Delete created FB Report id in API Booking Transaction. 
                if (!isReportProcessed)
                {
                    var activePoTransactions = bookingData.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value);
                    foreach (var item in activePoTransactions)
                    {
                        // Delete Report details based on the Report id 
                        if (item.FbReportId != null)
                        {
                            currentReports.Add(item.FbReportId.Value);
                            item.FbReportId = null;
                        }
                    }
                }

                RemoveMissionAttachments(bookingData);

                await _bookingRepo.EditInspectionBooking(bookingData);

                if (!isReportProcessed)
                {
                    // Delete all the reports finally from report details
                    foreach (var reportId in currentReports)
                    {
                        await _fbReportManager.DeleteFBReportDetails(reportId);
                    }
                    // if any runtime error then delete report details 
                    if (reportIds != null && reportIds.Any())
                    {
                        foreach (var reportId in reportIds)
                        {
                            await _fbReportManager.DeleteFBReportDetails(reportId);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.Source = failureResult.ToString();
                throw ex;
            }
        }


        /// <summary>
        /// Remove FB Id for the attachments
        /// </summary>
        /// <param name="bookingData"></param>
        private void RemoveMissionAttachments(InspTransaction bookingData)
        {
            var fileAttachments = bookingData.InspTranFileAttachments.Where(x => x.FbId > 0).ToList();
            if (fileAttachments.Any())
                fileAttachments.ForEach(x => x.FbId = null);
        }


        private async Task<Boolean> DeleteMissionByContainer(InspTransaction bookingData, string fbToken, List<int> reportIds, bool IsFbAdmin)
        {
            StringBuilder failureResult = new StringBuilder();
            List<int> currentReports = new List<int>();
            try
            {
                var fbBase = _fbSettings.BaseUrl;
                bool isReportProcessed = false;


                if (bookingData.FbMissionId != null)
                {
                    // if mission status completed then dont  delete this mission
                    var missionInfo = await GetMissionProductsInfo(bookingData.FbMissionId.Value, fbToken);

                    if (missionInfo != null && missionInfo.status == FBConstants.Status_Completed)
                    {

                        failureResult.Append(FBFailure.MissionCompleted.ToString());
                        throw new Exception();
                    }

                    // Any one report is validated then dont delete the mission 

                    var activePoTransactionList = bookingData.InspContainerTransactions.Where(x => x.Active.HasValue && x.Active.Value);

                    foreach (var item in activePoTransactionList)
                    {
                        if (item != null && item.FbReport != null && item.FbReport.FbReportMapId != 0)
                        {
                            // get Report details and check the status then delete the report.

                            var reportInfo = await GetReportInfo(item.FbReport.FbReportMapId.Value, fbToken);

                            if (reportInfo != null && reportInfo.statusFilling == "validated")
                            {
                                isReportProcessed = true;
                            }
                        }
                    }

                    if (!isReportProcessed)
                    {
                        var fbRequest = string.Format(_fbSettings.MissionDeleteRequestUrl, bookingData.FbMissionId);

                        _logger.LogInformation("API-FB : FB mission delete  request start ");
                        _logger.LogInformation(fbRequest);

                        await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                        {
                            MissionId = bookingData?.FbMissionId,
                            BookingId = bookingData?.Id,
                            RequestUrl = fbRequest,
                            CreatedBy = _ApplicationContext.UserId,
                            LogInformation = "API-FB : FB mission delete  request start"
                        });

                        HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);
                        _logger.LogInformation("API-FB : FB mission delete  request end ");
                        _logger.LogInformation(httpResponse.StatusCode.ToString());
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            // set mission local in db
                            bookingData.FbMissionId = null;
                            //don't delete qc & cs when call from admin page
                            if (!IsFbAdmin)
                                await RemoveQCAndCSFromSchedule(bookingData.Id);
                        }
                    }
                    else
                    {
                        failureResult.Append(FBFailure.ReportProcessDone.ToString());
                        throw new Exception();
                    }
                }

                // Delete created FB Report id in API Booking Transaction. 
                if (!isReportProcessed)
                {
                    var activePoTransactions = bookingData.InspContainerTransactions.Where(x => x.Active.HasValue && x.Active.Value);
                    foreach (var item in activePoTransactions)
                    {
                        // Delete Report details based on the Report id 
                        if (item.FbReportId != null)
                        {
                            currentReports.Add(item.FbReportId.Value);
                            item.FbReportId = null;
                        }
                    }
                }

                await _bookingRepo.EditInspectionBooking(bookingData);

                if (!isReportProcessed)
                {
                    // Delete all the reports finally from report details
                    foreach (var reportId in currentReports)
                    {
                        await _fbReportManager.DeleteFBReportDetails(reportId);
                    }
                    // if any runtime error then delete report details 
                    if (reportIds != null && reportIds.Any())
                    {
                        foreach (var reportId in reportIds)
                        {
                            await _fbReportManager.DeleteFBReportDetails(reportId);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.Source = failureResult.ToString();
                throw ex;
            }
        }


        private async Task<Boolean> DeleteFBReportAndMission(InspTransaction bookingData, string fbToken)
        {
            try
            {
                if (bookingData.InspTranServiceTypes.Any(x => x.Active && x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container))
                {
                    //report ids will be null since this method calling from fb admin screen
                    await DeleteMissionByContainer(bookingData, fbToken, null, true);
                }
                else
                {
                    //report ids will be null since this method calling from fb admin screen
                    await DeleteMission(bookingData, fbToken, null, true);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// get Report id from the result.
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private string getFbReportdIdFromResult(HttpResponseMessage httpResponse)
        {
            string fbRecordId = string.Empty;
            var responseData = httpResponse.Content.ReadAsStringAsync();
            JObject JsonData = JObject.Parse(responseData.Result);
            _logger.LogInformation("API-FB : Report Data :" + JsonData.ToString() + "");
            if (JsonData != null && JsonData.GetValue("data") != null)
            {
                fbRecordId = JsonData.GetValue("data").ToArray()[4].Last()["report"].Last().ToArray().Last()["id"].ToString();
            }
            return fbRecordId;
        }


        /// <summary>
        /// get Fb Result from response
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private FbReportMainDetail getFBResultData(HttpResponseMessage httpResponse)
        {
            try
            {
                FbReportMainDetail objReport = new FbReportMainDetail();

                var reportData = httpResponse.Content.ReadAsStringAsync();
                JObject reportDataJson = JObject.Parse(reportData.Result);
                if (reportDataJson != null && reportDataJson.GetValue("data") != null)
                {

                    // objReport.fbMissionProductId = reportDataJson.GetValue("data")["id"].ToString();

                    foreach (var item in reportDataJson.GetValue("included").ToArray())
                    {
                        if (item["type"].ToString() == "Mission")
                        {
                            objReport.fbMissionId = item["id"].ToString();
                            objReport.missionTitle = item["attributes"]["reference"].ToString();
                            objReport.missionStatus = item["attributes"]["status"].ToString();
                        }

                        if (item["type"].ToString() == "Report")
                        {
                            objReport.fbReportId = item["id"].ToString();
                            objReport.reportTitle = item["attributes"]["reference"].ToString();
                            objReport.status = item["attributes"]["status"].ToString();
                            objReport.statusPreparation = item["attributes"]["statusPreparation"].ToString();
                            objReport.statusFilling = item["attributes"]["statusFilling"].ToString();
                            objReport.statusReview = item["attributes"]["statusReview"].ToString();
                        }

                        if (item["type"].ToString() == "Product")
                        {
                            objReport.fbProductId = item["id"].ToString();
                        }

                    }
                }

                return objReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get Fb Mission Url from response
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private List<FbMissionUrlResponse> getFBMissionUrlData(HttpResponseMessage httpResponse)
        {
            try
            {
                List<FbMissionUrlResponse> lstMissionUrls = new List<FbMissionUrlResponse>();

                var missionUrlData = httpResponse.Content.ReadAsStringAsync();
                JObject missionUrlDataJson = JObject.Parse(missionUrlData.Result);
                if (missionUrlData != null && missionUrlDataJson.GetValue("data") != null)
                {
                    foreach (var item in missionUrlDataJson.GetValue("data").ToArray())
                    {
                        lstMissionUrls.Add(new FbMissionUrlResponse()
                        {
                            FbId = item["id"].ToString(),
                            Url = item["attributes"]["url"].ToString()
                        });
                    }
                }

                return lstMissionUrls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get the Result id from full bridge response
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private string getFbRecordIdFromResult(HttpResponseMessage httpResponse)
        {
            string fbRecordId = null;
            var userData = httpResponse.Content.ReadAsStringAsync();
            JObject userDataJson = JObject.Parse(userData.Result);
            if (userDataJson != null && userDataJson.GetValue("data") != null)
            {
                fbRecordId = userDataJson.GetValue("data")["id"].ToString();
            }
            return fbRecordId;
        }

        private string getFbUserIdIdFromResponse(HttpResponseMessage httpResponse)
        {
            string fbRecordId = null;
            var userData = httpResponse.Content.ReadAsStringAsync();
            JObject userDataJson = JObject.Parse(userData.Result);
            if (userDataJson != null && userDataJson.GetValue("data") != null)
            {
                fbRecordId = userDataJson.GetValue("data")[0]["id"].ToString();
            }
            return fbRecordId;
        }



        public async Task<Boolean> SaveCustomerMasterDataToFB(int customerId, string fbToken, int entityId)
        {
            try
            {
                var customer = await _customerRepo.GetFBCustomerDataById(customerId);

                if (customer != null)
                {
                    FBUserAccountData objAccount = new FBUserAccountData()
                    {
                        client = true,
                        title = customer.CustomerName,
                        address = customer.CustomerAddress,
                        country = customer.FbCountryId,
                        city = customer.CityName,
                        status = FBConstants.Active
                    };

                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.AccountRequestUrl;

                    if (customer.FbCusId != null)
                        fbRequest = string.Format(_fbSettings.AccountUpdateRequestUrl, customer.FbCusId);

                    _logger.LogInformation("API-FB : FB Customer data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        AccountId = customer.FbCusId,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    var method = customer.FbCusId == null ? Method.Post : Method.JSONPut;

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(method, fbRequest, objAccount, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB Customer data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);

                        // update customer table with FB id.
                        var customerEntity = await _customerRepo.GetCustomerDataByCustomerIdAndEntityId(customerId, entityId);
                        customerEntity.FbCusId = Convert.ToInt32(fbRecordId);
                        customer.FbCusId = customerEntity.FbCusId;
                        await _customerRepo.EditCustomer(customerEntity);
                        return true;
                    }
                    else if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save Supplier Master data to FB
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<Boolean> SaveSupplierMasterDataToFB(int supplierId, string fbToken, int entityId)
        {
            try
            {
                var supplier = await _supplierRepo.GetFBSupplierData(supplierId);

                if (supplier != null)
                {
                    FBUserAccountData objAccount = new FBUserAccountData()
                    {
                        vendor = true,
                        title = supplier.SupplierName,
                        address = supplier.Address,
                        cn_address = supplier.RegionalAddress,
                        country = supplier.FbCountryId,
                        countrySubdivision = supplier.FbProvinceId,
                        city = supplier.CityName,
                        status = FBConstants.Active
                    };

                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.AccountRequestUrl;

                    if (supplier.FbFactSupId != null)
                        fbRequest = string.Format(_fbSettings.AccountUpdateRequestUrl, supplier.FbFactSupId);

                    _logger.LogInformation("API-FB : FB supplier/Factory data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        AccountId = supplier.FbFactSupId,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    var method = supplier.FbFactSupId == null ? Method.Post : Method.JSONPut;

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(method, fbRequest, objAccount, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB supplier/Factory data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        // update customer table with FB id.
                        var supplierEntity = await _supplierRepo.GetSupplierDataBySupplierIdAndEntityId(supplierId, entityId);
                        supplierEntity.FbFactSupId = Convert.ToInt32(fbRecordId);
                        supplier.FbFactSupId = supplierEntity.FbFactSupId;
                        await _supplierRepo.EditSupplier(supplierEntity);
                        return true;
                    }
                    else if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save Factory details to FB from api 
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<Boolean> SaveFactoryMasterDataToFB(int factoryId, string fbToken, int entityId)
        {
            try
            {

                var factory = await _supplierRepo.GetFBSupplierData(factoryId);

                if (factory != null)
                {

                    FBUserAccountData objAccount = new FBUserAccountData()
                    {
                        factory = true,
                        title = factory.SupplierName,
                        address = factory.Address,
                        cn_address = factory.RegionalAddress,
                        country = factory.FbCountryId,
                        countrySubdivision = factory.FbProvinceId,
                        city = factory.CityName,
                        status = FBConstants.Active
                    };

                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.AccountRequestUrl;

                    if (factory.FbFactSupId != null)
                        fbRequest = string.Format(_fbSettings.AccountUpdateRequestUrl, factory.FbFactSupId);

                    _logger.LogInformation("API-FB : FB Factory data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        AccountId = factory.FbFactSupId,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    var method = factory.FbFactSupId == null ? Method.Post : Method.JSONPut;

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(method, fbRequest, objAccount, fbBase, fbToken);
                    _logger.LogInformation("API-FB : FB Factory data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        var factoryEntity = await _supplierRepo.GetSupplierDataBySupplierIdAndEntityId(factoryId, entityId);
                        factoryEntity.FbFactSupId = Convert.ToInt32(fbRecordId);
                        factory.FbFactSupId = factoryEntity.FbFactSupId;
                        await _supplierRepo.EditSupplier(factoryEntity);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save the customer products master to FB
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<Boolean> SaveProductMasterDataToFB(int productId, int? entityId, string fbToken)
        {
            try
            {
                var product = await _productRepo.GetFBCustomerProduct(productId, entityId);

                string fbRecordId = string.Empty;
                string fbRequest = string.Empty;
                var objProduct = getProductMapping(product);
                var fbBase = _fbSettings.BaseUrl;

                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                {
                    RequestUrl = "SaveProductMasterDataToFB start",
                    LogInformation = JsonConvert.SerializeObject(objProduct)
                });

                // update customer produts to FB.
                if (product != null && product.FBProducId != null)
                {

                    fbRequest = string.Format(_fbSettings.ProductUpdateUrl, product.FBProducId);

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProduct)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Patch, fbRequest, objProduct, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB product data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        await SaveProductMsChartDataToFB(product.Id, product.FBProducId, fbToken);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                // add customer produts to FB.
                else if (objProduct != null)
                {
                    fbRequest = _fbSettings.ProductRequestUrl;

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProduct)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objProduct, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB product data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        if (product != null)
                        {
                            fbRecordId = getFbRecordIdFromResult(httpResponse);
                            var productEntity = _productRepo.GetCustomerProductByEntityAndProductId(product.Id, entityId);
                            if (productEntity != null)
                            {
                                productEntity.FbCusProdId = Convert.ToInt32(fbRecordId);
                                await _productRepo.EditCustomerProducts(productEntity);
                                await SaveProductMsChartDataToFB(productEntity.Id, productEntity.FbCusProdId, fbToken);
                            }
                        }

                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// map the product for fb request
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private FBProductData getProductMapping(FBProductMasterData product)
        {
            FBProductData objProduct = null;

            if (product != null)
            {
                int? productcategoryId = 0;

                if (product.ProductSub2CategoryId != null)
                {
                    productcategoryId = product.ProductSub2CategoryId.Value;
                }

                else if (product.ProductSubCategoryId != null)
                {
                    productcategoryId = product.ProductSubCategoryId.Value;
                }
                else if (product.ProductCategoryId != null)
                {
                    productcategoryId = product.ProductCategoryId.Value;
                }

                objProduct = new FBProductData()
                {
                    title = product?.ProductDescription,
                    reference = product.ProductName,
                    client = product.FBCustomerId != null ? product.FBCustomerId.Value : 0,
                    productCategory = productcategoryId != 0 ? productcategoryId.Value : 0,
                    status = FBConstants.Active,
                    measurementChart = product.MsChartFileUrl
                };


            }

            return objProduct;

        }

        /// <summary>
        /// get fb reports by service dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<ReportIdData>> getReportIdsbyBookingServiceDates(DateTime startDate, DateTime endDate)
        {
            return await _fbManager.getReportIdsbyBookingServiceDates(startDate, endDate);
        }


        public async Task<(bool isSuccess, string error)> SaveReportDataToFB(int fbReportId, string reportUrl, string fbToken)
        {
            try
            {
                string fbRequest = string.Empty;
                var fbBase = _fbSettings.BaseUrl;
                var objectRequest = new UpdateFbReportData()
                {
                    external_url = reportUrl,
                };
                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                {
                    RequestUrl = "SaveReportDataToFB start",
                    LogInformation = JsonConvert.SerializeObject(objectRequest)
                });

                fbRequest = string.Format(_fbSettings.ReportUpdateUrl, fbReportId);

                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                {
                    RequestUrl = fbRequest,
                    LogInformation = JsonConvert.SerializeObject(objectRequest)
                });

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Patch, fbRequest, objectRequest, fbBase, fbToken);

                _logger.LogInformation("API-FB : FB Report data request end ");
                _logger.LogInformation(httpResponse.StatusCode.ToString());

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    return (isSuccess: true, error: null);
                }
                else
                {
                    var response = httpResponse.Content.ReadAsStringAsync().Result;
                    var responseObject = JObject.Parse(response);
                    var erros = responseObject.GetValue("errors").ToList();
                    return (isSuccess: false, error: string.Join(",", erros.Select(x => ((JObject)x).GetValue("title").ToString())));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateAuditReportRequest(AudTransaction auditData, string fbToken)
        {
            try
            {
                if (auditData.FbmissionId != null)
                {
                    ReportRequest fbReportRequest = new ReportRequest();
                    fbReportRequest.FbMissionId = auditData.FbmissionId;
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.ReportRequestUrl;
                    _logger.LogInformation("API-FB : FB mission data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(fbReportRequest));
                    // Add Full bridge Log information for mission request
                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        ServiceId = (int)Service.AuditId,
                        MissionId = fbReportRequest.FbMissionId,
                        BookingId = auditData.Id,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(fbReportRequest)
                    });

                    var reportRequest = new Dictionary<string, object>()
                    {
                        {"mission" ,auditData.FbmissionId}
                    };
                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, reportRequest, fbBase, fbToken);
                    _logger.LogInformation("API-FB : FB mission data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        if (int.TryParse(fbRecordId, out int fbReportId))
                        {
                            var response = await httpResponse.Content.ReadAsStringAsync();
                            var jobject = JObject.Parse(response);
                            auditData.FbreportTitle = jobject["data"]["attributes"]["title"].ToString();
                            auditData.FbreportId = Convert.ToInt32(fbRecordId);
                            auditData.FbreportStatus = (int)FBStatus.ReportDraft;
                            auditData.FbfillingStatus = (int)FBStatus.ReportFillingNotstarted;
                            auditData.FbreportStatus = (int)FBStatus.ReportReviewNotStarted;
                            await _auditRepository.UpdateAudit(auditData);
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateReportProductCategory(AudTransaction auditData, string fbToken)
        {
            var fbBase = _fbSettings.BaseUrl;
            try
            {

                if (auditData.FbreportId > 0 && auditData.CuProductCategoryNavigation != null)
                {
                    var objectRequest = new Dictionary<string, string>
                                {{"auditProductCategory", auditData.CuProductCategoryNavigation.FbName }};

                    string fbRequest = string.Format(_fbSettings.ReportUpdateUrl, auditData.FbreportId);
                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Patch, fbRequest, objectRequest, fbBase, fbToken);
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        return true;
                    else
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task UpdateSentReportDateAndTime(List<int> bookingIds, string fbToken)
        {
            var fbBase = _fbSettings.BaseUrl;

            var fbReportDetail = await _bookingRepo.GetListAsync<FbReportDetail>(x => x.Active.Value && bookingIds.Contains(x.InspectionId.GetValueOrDefault()));
            var fbReportIds = fbReportDetail.Select(x => x.FbReportMapId).Distinct().ToList();
            var apiReportIds = fbReportDetail.Select(x => x.Id).Distinct().ToList();
            var reportDetails = await _bookingRepo.GetReportVersionDetails(apiReportIds);

            foreach (var fbReportId in fbReportIds)
            {
                if (fbReportId > 0)
                {
                    var apiReport = fbReportDetail.FirstOrDefault(x => x.FbReportMapId == fbReportId);
                    if (apiReport != null)
                    {
                        var objectRequest = new Dictionary<string, string>
                                        {{"sentDateTime",DateTime.Now.ToString() }};

                        var reportData = reportDetails.FirstOrDefault(x => x.ReportId == apiReport.Id);

                        if (reportData != null)
                        {
                            if (reportData.ReportRevision > 0)
                            {
                                objectRequest.Add("revision", reportData.ReportRevision.ToString());
                            }
                            if (reportData.ReportVersion > 0)
                            {
                                objectRequest.Add("version", reportData.ReportVersion.ToString());
                            }
                        }
                        string fbRequest = string.Format(_fbSettings.ReportUpdateUrl, fbReportId);
                        await _helper.SendRequestToPartnerAPI(Method.Patch, fbRequest, objectRequest, fbBase, fbToken);
                    }
                }
            }
        }

        private async Task<FbReportInfo> GetMissionDetails(int missionId, string fbToken)
        {
            try
            {
                List<FbProdutsReportMapData> mappedProductList = new List<FbProdutsReportMapData>();
                FbReportInfo objReport = new FbReportInfo();

                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format(_fbSettings.MissionDeleteRequestUrl, missionId);
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {

                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    JObject reportDataJson = JObject.Parse(reportData.Result);
                    if (reportDataJson != null)
                    {
                        objReport.status = reportDataJson["data"]["attributes"]["status"].ToString();
                    }
                }

                return objReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Save Mission data to FB with booking details from API.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<Boolean> SaveAuditMissionDataToFB(AudTransaction audit, FbAuditData auditData, List<AuditCustomerContactData> customerContacts, List<AuditCustomerContactData> supplierContacts, string fbToken)
        {
            try
            {
                if (audit?.FbmissionId == null) // check if mission not created in FB
                {
                    //Create mission object
                    var objAccount = await CreateMissionObjectForAudit(auditData, audit.EntityId.GetValueOrDefault(), customerContacts, supplierContacts, audit.CuProductCategoryNavigation?.FbName);
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.MissionRequestUrl;
                    _logger.LogInformation("API-FB : FB mission data request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));
                    // Add Full bridge Log information for mission request
                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        MissionId = audit?.FbmissionId,
                        BookingId = audit?.Id,
                        RequestUrl = fbRequest,
                        ServiceId = (int)Service.AuditId,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });
                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objAccount, fbBase, fbToken);
                    _logger.LogInformation("API-FB : FB mission data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        // update booking table with FB id.
                        var response = await httpResponse.Content.ReadAsStringAsync();
                        var jobject = JObject.Parse(response);
                        audit.FbmissionId = Convert.ToInt32(fbRecordId);
                        audit.FbmissionTitle = jobject["data"]["attributes"]["reference"].ToString();
                        await _auditRepository.UpdateAudit(audit);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var objAccount = await CreateMissionObjectForAudit(auditData, audit.EntityId.GetValueOrDefault(), customerContacts, supplierContacts, audit.CuProductCategoryNavigation?.FbName);

                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = string.Format(_fbSettings.MissionDeleteRequestUrl, audit.FbmissionId);

                    _logger.LogInformation("API-FB : FB mission data update request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objAccount));

                    // Add Full bridge Log information for mission request update
                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        MissionId = audit?.FbmissionId,
                        BookingId = audit?.Id,
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objAccount)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Patch, fbRequest, objAccount, fbBase, fbToken);

                    _logger.LogInformation("API-FB : FB mission data update request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete the mission by mission id
        /// </summary>
        /// <param name="MissionId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<Boolean> DeleteAuditMission(AudTransaction audit, string fbToken)
        {
            StringBuilder failureResult = new StringBuilder();
            try
            {
                var fbBase = _fbSettings.BaseUrl;
                bool isReportProcessed = false;

                if (audit.FbmissionId != null)
                {
                    // if mission status completed then dont  delete this mission
                    var missionInfo = await GetMissionDetails(audit.FbmissionId.Value, fbToken);

                    //check the mission status
                    if (missionInfo != null && missionInfo.status == FBConstants.Status_Completed)
                    {
                        failureResult.Append(FBFailure.MissionCompleted.ToString());

                        throw new Exception();
                    }

                    //check report generated
                    if (audit.FbreportId.HasValue && audit.FbreportId > 0)
                    {
                        //get report details
                        var reportInfo = await GetReportInfo(audit.FbreportId.Value, fbToken);
                        //check the report is validated
                        if (reportInfo != null && reportInfo.statusFilling == "validated")
                        {
                            isReportProcessed = true;
                        }
                    }

                    //mission is available and not mission status is not completed and report is not validated then mission delete
                    if (missionInfo != null && !isReportProcessed)
                    {
                        var fbRequest = string.Format(_fbSettings.MissionDeleteRequestUrl, audit.FbmissionId);

                        _logger.LogInformation("API-FB : FB mission delete  request start ");
                        _logger.LogInformation(fbRequest);

                        await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                        {
                            MissionId = audit?.FbmissionId,
                            BookingId = audit?.Id,
                            RequestUrl = fbRequest,
                            CreatedBy = _ApplicationContext.UserId,
                            LogInformation = "API-FB : FB mission delete  request"
                        });

                        HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);
                        _logger.LogInformation("API-FB : FB mission delete  request end ");
                        _logger.LogInformation(httpResponse.StatusCode.ToString());
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            // set mission local in db
                            audit.FbmissionId = null;
                            RemoveAuditorAndCSFromAuditSchedule(audit);
                        }
                    }
                    else
                    {
                        failureResult.Append(FBFailure.ReportProcessDone.ToString());
                        throw new Exception();
                    }
                }


                await _auditRepository.UpdateAudit(audit);

                return true;
            }
            catch (Exception ex)
            {
                ex.Source = failureResult.ToString();
                throw ex;
            }
        }

        private void RemoveAuditorAndCSFromAuditSchedule(AudTransaction audTransaction)
        {
            try
            {
                if (audTransaction != null) // if audit is valid
                {
                    //get auditor list from Aud_Tran_Auditors table using booking id
                    var auditorList = audTransaction.AudTranAuditors;

                    //get CS list from AUD_Tran_CS table using booking id
                    var csList = audTransaction.AudTranCS;

                    if (auditorList != null && auditorList.Count() > 0)
                    {
                        foreach (var qcItem in auditorList)
                        {
                            qcItem.Active = false;
                            qcItem.DeletedBy = _ApplicationContext.UserId;
                            qcItem.DeletedTime = DateTime.Now;
                        }
                        _auditRepository.EditEntities(auditorList);
                    }
                    if (csList != null && csList.Count() > 0)
                    {
                        foreach (var csItem in csList)
                        {
                            csItem.Active = false;
                            csItem.DeletedBy = _ApplicationContext.UserId;
                            csItem.DeletedTime = DateTime.Now;
                        }
                        _auditRepository.EditEntities(csList);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveAuditMissionUrlsDataToFb(AudTransaction audit, List<AudTranFileAttachment> audTranFileAttachments, string fbToken)
        {
            try
            {
                if (audit.FbmissionId > 0)
                {
                    var missionUrls = await GetMissionUrlsDataToFb(audit.FbmissionId.GetValueOrDefault(), audit.Id, fbToken, (int)Service.AuditId);
                    if (missionUrls != null)
                    {
                        if (audTranFileAttachments != null && audTranFileAttachments.Any())
                        {
                            //for deleted attachement need to remove from the mission                            
                            var auditAttachmentFbIds = audTranFileAttachments.Where(x => x.FbMissionUrlId.HasValue && x.Active).Select(x => x.FbMissionUrlId);
                            //so we are getting active and inactive attachments and check fb mission urls ids with  mission urls
                            var deleteFbUrlIds = missionUrls.Where(x => x > 0 && !auditAttachmentFbIds.Contains(x));
                            if (deleteFbUrlIds != null && deleteFbUrlIds.Any())
                            {
                                foreach (var deleteFbUrlId in deleteFbUrlIds)
                                {
                                    if (deleteFbUrlId > 0)
                                    {
                                        if (!await DeleteAuditMissionUrlDataToFb(audit, deleteFbUrlId, fbToken))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            List<FBMissionUrlData> requestMissionUrl = new List<FBMissionUrlData>();

                            string fbRecordId = string.Empty;
                            var fbBase = _fbSettings.BaseUrl;
                            var fbRequest = string.Format(_fbSettings.MissionUrlRequestUrl, audit.FbmissionId);

                            var auditTranFileAttachments = audTranFileAttachments.Where(x => !x.FbMissionUrlId.HasValue && x.Active).ToList();
                            if (auditTranFileAttachments.Any())
                            {
                                foreach (var item in auditTranFileAttachments)
                                {
                                    requestMissionUrl.Add(new FBMissionUrlData() { url = item.FileUrl, title = item.FileName, classification = FBMissionUrlClassification.MissionAttachment });
                                }

                                _logger.LogInformation("API-FB : FB mission url data request start ");
                                _logger.LogInformation(JsonConvert.SerializeObject(requestMissionUrl));

                                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                                {
                                    ServiceId = (int)Service.AuditId,
                                    MissionId = audit?.FbmissionId,
                                    BookingId = audit?.Id,
                                    RequestUrl = fbRequest,
                                    LogInformation = JsonConvert.SerializeObject(requestMissionUrl)
                                });

                                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, requestMissionUrl, fbBase, fbToken);

                                _logger.LogInformation("API-FB : FB mission url data request end ");
                                _logger.LogInformation(httpResponse.StatusCode.ToString());
                                if (httpResponse.StatusCode == HttpStatusCode.Created)
                                {
                                    var result = getFBMissionUrlData(httpResponse);
                                    await UpdateAuditTranFileAttachementFbID(auditTranFileAttachments, result);
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (missionUrls.Any())
                            {
                                foreach (var missionUrl in missionUrls)
                                {
                                    if (missionUrl > 0)
                                    {
                                        await DeleteAuditMissionUrlDataToFb(audit, missionUrl, fbToken);
                                    }
                                }
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> DeleteAuditMissionUrlDataToFb(AudTransaction audit, int fbUrlId, string fbToken)
        {
            try
            {
                string fbRecordId = string.Empty;
                var fbBase = _fbSettings.BaseUrl;
                var fbRequest = string.Format(_fbSettings.MissionUrlDeleteUrl, audit.FbmissionId);
                fbRequest = string.Concat(fbRequest, "/", fbUrlId);

                _logger.LogInformation("API-FB : FB mission url data request start ");

                await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                {
                    ServiceId = (int)Service.InspectionId,
                    MissionId = audit.FbmissionId,
                    BookingId = audit?.Id,
                    RequestUrl = fbRequest,
                });

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Delete, fbRequest, null, fbBase, fbToken);

                _logger.LogInformation("API-FB : FB mission url data request end ");
                _logger.LogInformation(httpResponse.StatusCode.ToString());

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task UpdateAuditTranFileAttachementFbID(IEnumerable<AudTranFileAttachment> inspTranFileAttachments, List<FbMissionUrlResponse> fbMissionUrls)
        {
            if (fbMissionUrls != null)
            {
                foreach (var fbMissionUrl in fbMissionUrls)
                {
                    var inspTranFileAttachment = inspTranFileAttachments.FirstOrDefault(x => x.FileUrl == fbMissionUrl.Url);
                    if (inspTranFileAttachment != null && Int32.TryParse(fbMissionUrl.FbId, out int fbId))
                    {
                        inspTranFileAttachment.FbMissionUrlId = fbId;
                    }
                }

                _auditRepository.EditEntities(inspTranFileAttachments);
                await _auditRepository.Save();
            }

        }
        private async Task SaveProductMsChartDataToFB(int productId, int? fbCusProdId, string fbToken)
        {
            var fbBase = _fbSettings.BaseUrl;
            if (productId > 0 && fbCusProdId != null && fbCusProdId > 0)
            {
                string fbRequest = string.Format(fbBase + "/" + _fbSettings.MeasurementRequestUrl, fbCusProdId.GetValueOrDefault());
                var productMschartList = await _productRepo.GetFBProductMschart(productId);
                await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, productMschartList, fbBase, fbToken);
            }
        }
    }
}

