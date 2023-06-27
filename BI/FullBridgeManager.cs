using Components.Core.contracts;
using Components.Core.entities.Emails;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Eaqf;
using DTO.EventBookingLog;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.Report;
using Entities;
using Entities.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class FullBridgeManager : ApiCommonData, IFullBridgeManager
    {
        private readonly IFullBridgeRepository _fbRepo = null;
        private readonly IEventBookingLogManager _fbLog = null;
        private readonly IEmailManager _emailManager = null;
        private readonly INotificationManager _notificationManger = null;
        private readonly ITenantProvider _tenantProvider = null;
        private static IConfiguration _Configuration = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICustomerCheckPointRepository _customerCheckPointRepository = null;

        private readonly IReportFastTransactionRepository _reportFastTransactionRepository = null;
        private readonly IInspectionBookingRepository _inspectionBookingRepository;
        private readonly IInspectionCustomReportManager _inspectionCustomReportManager = null;
        private readonly IHumanResourceManager _humanresourcemanager = null;
        private readonly IEaqfEventUpdateManager _eaqfEventUpdate = null;
        int adminUserFromAppSettings = 0;

        public FullBridgeManager(IFullBridgeRepository fbRepo, IInspectionBookingRepository inspectionBookingRepository, IEventBookingLogManager fbLog, ICustomerCheckPointRepository customerCheckPointRepository,
            IReportFastTransactionRepository reportFastTransactionRepository,
            IEmailManager emailManager, INotificationManager notificationManger, IConfiguration configuration, ITenantProvider tenantProvider,
            IAPIUserContext ApplicationContext, IInspectionCustomReportManager inspectionCustomReportManager,
            IHumanResourceManager humanresourcemanager, IEaqfEventUpdateManager eaqfEventUpdate)
        {
            _fbRepo = fbRepo;
            _fbLog = fbLog;
            _emailManager = emailManager;
            _notificationManger = notificationManger;
            _Configuration = configuration;
            _tenantProvider = tenantProvider;
            _ApplicationContext = ApplicationContext;
            _customerCheckPointRepository = customerCheckPointRepository;
            _reportFastTransactionRepository = reportFastTransactionRepository;
            _inspectionBookingRepository = inspectionBookingRepository;
            _inspectionCustomReportManager = inspectionCustomReportManager;
            _humanresourcemanager = humanresourcemanager;
            int.TryParse(_Configuration["ExternalAccessorUserId"], out adminUserFromAppSettings);
            _eaqfEventUpdate = eaqfEventUpdate;
        }

        /// <summary>
        /// Update the Fullbridge status based on the report 
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FbStatusResponse> UpdateFBFillingAndReviewStatus(int reportId, FbStatusRequest request, bool fromBulkUpdate)
        {
            var response = new FbStatusResponse();
            // Add Full bridge Log information 
            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
            {
                ReportId = reportId,
                LogInformation = JsonConvert.SerializeObject(request)
            });
            var refService = (int)Service.InspectionId;
            if (request.ServiceId.HasValue)
            {
                refService = await _fbRepo.GetFbServiceTypeId(request.ServiceId.Value);
            }
            if (refService == (int)Service.InspectionId)
            {
                int apiReportId = await GetApiReportIdbyFbReport(reportId);
                var reportData = await _fbRepo.GetInspectionFbReportsWithStatus(apiReportId);
                if (reportData != null)
                {
                    await updateReportFillingAndReviewStatus(request, reportData);

                    // Send email and notification to report checker if the report filling is validated.
                    if (!fromBulkUpdate && reportData?.FbFillingStatus == (int)FBStatus.ReportFillingValidated &&
                        reportData?.FbReviewStatus == (int)FBStatus.ReportReviewNotStarted)
                    {
                        var bookingData = await _fbRepo.GetInspectionReports(apiReportId);
                        var bookingContainerData = bookingData == null ? await _fbRepo.GetInspectionReportsFromContainer(apiReportId) : null;

                        if (bookingData != null || bookingContainerData != null)
                            await SendNotificationAndEmailToReportChecker(bookingData, reportData, bookingContainerData);
                    }
                    response.InspectionId = reportData.InspectionId;
                }
                response.ReportId = apiReportId;
            }
            else if (refService == (int)Service.AuditId)
            {
                var auditData = await _fbRepo.GetAuditFbReports(reportId);
                if (auditData != null)
                {
                    await UpdateAuditReportFillingAndReviewStatus(request, auditData);
                    // Send email and notification to report checker if the report filling is validated.
                    if (!fromBulkUpdate && auditData?.FbfillingStatus == (int)FBStatus.ReportFillingValidated &&
                        auditData?.FbreviewStatus == (int)FBStatus.ReportReviewNotStarted)
                    {
                        await AuditSendNotificationAndEmailToReportChecker(auditData);
                    }
                }
                response.ReportId = reportId;
            }
            response.ServiceId = refService;
            response.Result = FbStatusResponseResult.success;
            return response;
        }

        /// <summary>
        /// Update Fb Report status and API inspection status
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FbStatusResponse> UpdateBookingStatusIfAllReportsValidated(int reportId, FbReportDataRequest request)
        {
            // Add Full bridge Log information 
            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
            {
                ReportId = reportId,
                LogInformation = JsonConvert.SerializeObject(request)
            });

            var bookingData = await _fbRepo.GetInspectionReports(reportId);
            var bookingContainerData = bookingData == null ? await _fbRepo.GetInspectionReportsFromContainer(reportId) : null;
            if (bookingData != null || bookingContainerData != null)
            {
                await updateBookingStatusbyFbReportStatus(bookingData, request, bookingContainerData);
            }

            return new FbStatusResponse() { Result = FbStatusResponseResult.success };
        }

        /// <summary>
        /// update the status
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        /// <returns></returns>

        private async Task updateBookingStatusbyFbReportStatus(InspProductTransaction bookingData, FbReportDataRequest request, InspContainerTransaction bookingContainerData)
        {

            var inspection = bookingData != null ? bookingData.Inspection : bookingContainerData != null ?
                             bookingContainerData.Inspection : null;

            if (inspection != null)
            {
                // check all the reports validated or invalidated then make it as inspected
                if (!await _fbRepo.checkAllTheReportsNotValidatedOrInvalidated(inspection.Id))
                {
                    if (inspection.StatusId != (int)BookingStatus.ReportSent)
                    {
                        inspection.StatusId = (int)BookingStatus.Inspected;
                        inspection.UpdatedOn = DateTime.Now;
                        AddInspectionStatusLog(inspection);
                        // update booking status to EAQF
                        if (inspection.IsEaqf.GetValueOrDefault())
                        {
                            EAQFEventUpdate cancelRequest = new EAQFEventUpdate();
                            cancelRequest.BookingId = inspection.Id;
                            cancelRequest.StatusId = inspection.StatusId;
                            await _eaqfEventUpdate.UpdateRescheduleStatusToEAQF(cancelRequest, EAQFBookingEventRequestType.AddStatus);
                        }
                    }

                    //update the QC and CS filling and review columns
                    await ScheduleCsUpdate(inspection.Id);
                    await ScheduleQcUpdate(inspection.Id);
                    await _fbRepo.Save();
                }
                else
                {
                    if (InspectedStatusList.Contains(inspection.StatusId) && await _fbRepo.IsAnyReportsAvailableByReportStatuses(inspection.Id, new List<int?>() { (int)FBStatus.ReportInValidated }))
                    {
                        inspection.StatusId = (int)BookingStatus.AllocateQC;
                        inspection.UpdatedOn = DateTime.Now;
                        AddInspectionStatusLog(inspection);
                        await _fbRepo.Save();
                    }
                }
            }
            await _fbRepo.UpdateInspectionStatus(inspection);
        }

        /// <summary>
        /// update filling and review status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        /// <returns></returns>
        private async Task updateReportFillingAndReviewStatus(FbStatusRequest request, FbReportDetail reportData)
        {

            SetFillingAndReviewStatus(reportData, request);

            await _fbRepo.UpdateInspectionFbReport(reportData);
        }

        /// <summary>
        /// Adding status log for inspected 
        /// </summary>
        /// <param name="entity"></param>
        private void AddInspectionStatusLog(InspTransaction entity)
        {
            entity.InspTranStatusLogs.Add(new InspTranStatusLog()
            {
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                StatusId = entity.StatusId,
                ServiceDateFrom = entity.ServiceDateFrom,
                ServiceDateTo = entity.ServiceDateTo,
                StatusChangeDate = DateTime.Now,
                EntityId = _tenantProvider.GetCompanyId()
            });
        }
        /// <summary>
        /// Save Report Information by Report Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FbStatusResponse> SaveFBReportDetails(int reportId, FbReportDataRequest request)
        {
            var response = new FbStatusResponse();

            // Add Full bridge Log information            
            await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
            {
                ReportId = reportId,
                LogInformation = JsonConvert.SerializeObject(request)
            });

            bool isNewReportFormatCheckpoint = false;
            int? entityId = null;
            var refService = await _fbRepo.GetFbServiceTypeId(request.ServiceId.GetValueOrDefault());

            if (refService == (int)Service.InspectionId)
            {
                int apiReportId = await GetApiReportIdbyFbReport(reportId);
                var bookingFbReportData = await _fbRepo.GetInspectionFbReports(apiReportId);
                // if data exist make child table entry in-active and data push again
                if (bookingFbReportData != null)
                {

                    await updateReportFillingAndReviewStatus(request, bookingFbReportData);
                    await UpdateReportDetails(bookingFbReportData, request);
                    await UpdateReportChildDetails(request, bookingFbReportData);
                    await AutoUpdateReportCustomerDecision(bookingFbReportData);
                    await UpdateBookingStatusIfAllReportsValidated(apiReportId, request);
                    //cu checkpoint new report format 
                    isNewReportFormatCheckpoint = await _customerCheckPointRepository.IsCustomerCheckpointConfigured(bookingFbReportData.Inspection.CustomerId, (int)CuCheckPointTypeService.NewReportFormat);
                    if (isNewReportFormatCheckpoint)
                    {
                        entityId = bookingFbReportData?.Inspection?.EntityId;
                    }
                    response.InspectionId = bookingFbReportData.InspectionId;
                }
                response.ReportId = apiReportId;
            }
            else if (refService == (int)Service.AuditId)
            {
                var auditData = await _fbRepo.GetAuditFbReports(reportId);
                if (auditData != null)
                {
                    await UpdateAuditReportFillingAndReviewStatus(request, auditData);
                    await UpdateAuditReportDetails(auditData, request);
                }
                response.ReportId = reportId;
            }
            else
            {
                response.ServiceId = refService;
                response.Result = FbStatusResponseResult.WrongServiceType;
                response.IsNewReportFormatCheckPoint = false;
                return response;
            }
            response.ServiceId = refService;
            response.EntityId = entityId;
            response.IsNewReportFormatCheckPoint = isNewReportFormatCheckpoint;
            response.Result = FbStatusResponseResult.success;
            return response;
        }


        /// <summary>
        /// Delete Fb Report details if the mission got delete
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<FbStatusResponse> DeleteFBReportDetails(int reportId)
        {

            var bookingFbReportData = await _fbRepo.GetInspectionFbReports(reportId);

            // Make parent and child data as in active.
            if (bookingFbReportData != null)
            {
                await DeleteFbReportDetails(bookingFbReportData);
            }

            return new FbStatusResponse() { Result = FbStatusResponseResult.success };
        }

        /// <summary>
        /// Get Full bridge Report List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FbReportDetail>> GetFBReportDetails()
        {
            return await _fbRepo.GetFbReportDetails();
        }

        /// <summary>
        /// Save FB Report Information in API 
        /// </summary>
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FbStatusResponse> SaveFBReportInfo(int reportId, int bookingId, FbReportDataRequest request)
        {
            var apiReportId = await AddReportDetails(reportId, bookingId, request);

            return new FbStatusResponse() { ReportId = apiReportId, Result = FbStatusResponseResult.success };
        }

        /// <summary>
        /// Add fb report details 
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        private async Task<int> AddReportDetails(int reportId, int bookingId, FbReportDataRequest request)
        {
            try
            {
                var fbReportEntity = new FbReportDetail()
                {
                    Active = true,
                    FbReportMapId = reportId,
                    InspectionId = bookingId,
                    OverAllResult = request.OverAllResult,
                    CreatedOn = DateTime.Now,
                    FbReportStatus = (int)FBStatus.ReportDraft,
                    FbFillingStatus = (int)FBStatus.ReportFillingNotstarted,
                    FbReviewStatus = (int)FBStatus.ReportReviewNotStarted,
                    MissionTitle = request.MissionTitle,
                    ReportTitle = request.ReportTitle,
                    MainProductPhoto = request.MainProductPhoto,
                    FinalReportPath = request.FinalReportPath
                };

                DateTime serviceFromDate;
                DateTime serviceToDate;

                // if the dates PROPERLY CONVERTED THEN SEND TO DB
                if (DateTime.TryParseExact(request.InspectionFromDate, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out serviceFromDate))
                {
                    fbReportEntity.ServiceFromDate = serviceFromDate;
                }

                if (DateTime.TryParseExact(request.InspectionToDate, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out serviceToDate))
                {
                    fbReportEntity.ServiceToDate = serviceToDate;
                }


                await _fbRepo.AddInspectionFbReport(fbReportEntity);

                return fbReportEntity.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add fb report details 
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        private async Task UpdateReportChildDetails(FbReportDataRequest request, FbReportDetail reportData)
        {
            List<ReportProductsAndPo> productPoList = null;
            try
            {
                if (reportData.Inspection.BusinessLine == (int)BusinessLine.SoftLine)
                    productPoList = await _fbRepo.GetInspectionProductColorReportData(reportData.Id);
                else
                    productPoList = await _fbRepo.GetInspectionProductContainerReportData(reportData.Id);

                if (request.InspectionSummary != null && request.InspectionSummary.Any())
                {
                    foreach (var inspectionSummary in request.InspectionSummary)
                    {
                        var reportSummary = new FbReportInspSummary()
                        {
                            Active = true,
                            CreatedOn = DateTime.Now,
                            FbReportInspsumTypeId = (int)FbReportInspectionSummaryType.Main,
                            Name = inspectionSummary.Name,
                            Result = inspectionSummary.Result,
                            Sort = inspectionSummary.Sort,
                            ScoreValue = inspectionSummary.ScoreValue,
                            ScorePercentage = inspectionSummary.ScorePercentage
                        };

                        // set report result id
                        if (inspectionSummary.Result != null && inspectionSummary.Result.ToLower() == FBReportResult.Pass.ToString().ToLower())
                        {
                            reportSummary.ResultId = (int)FBReportResult.Pass;
                        }

                        if (inspectionSummary.Result != null && inspectionSummary.Result.ToLower() == FBReportResult.Fail.ToString().ToLower())
                        {
                            reportSummary.ResultId = (int)FBReportResult.Fail;
                        }

                        if (inspectionSummary.Result != null && inspectionSummary.Result.ToLower() == FBReportResult.Pending.ToString().ToLower())
                        {
                            reportSummary.ResultId = (int)FBReportResult.Pending;
                        }

                        if (inspectionSummary.Result != null && inspectionSummary.Result.ToLower() == FBReportResult.Not_Applicable.ToString().ToLower())
                        {
                            reportSummary.ResultId = (int)FBReportResult.Not_Applicable;
                        }

                        if (inspectionSummary.Result != null && inspectionSummary.Result.ToLower() == FBReportResult.Missing.ToString().ToLower())
                        {
                            reportSummary.ResultId = (int)FBReportResult.Missing;
                        }


                        // Add sub check point to specific inspection summary
                        if (inspectionSummary.SubCheckPoints != null && inspectionSummary.SubCheckPoints.Any())
                        {
                            foreach (var subSummary in inspectionSummary.SubCheckPoints)
                            {
                                if (subSummary != null)
                                {
                                    var reportSubSummary = new FbReportInspSubSummary()
                                    {
                                        Active = true,
                                        CreatedOn = DateTime.Now,
                                        Name = subSummary.Name,
                                        Result = subSummary.Result,
                                        Remarks = subSummary.Remarks,
                                        Sort = subSummary.Sort
                                    };

                                    // set report result id
                                    if (subSummary.Result != null && subSummary.Result.ToLower() == FBReportResult.Pass.ToString().ToLower())
                                    {
                                        reportSubSummary.ResultId = (int)FBReportResult.Pass;
                                    }

                                    if (subSummary.Result != null && subSummary.Result.ToLower() == FBReportResult.Fail.ToString().ToLower())
                                    {
                                        reportSubSummary.ResultId = (int)FBReportResult.Fail;
                                    }

                                    if (subSummary.Result != null && subSummary.Result.ToLower() == FBReportResult.Pending.ToString().ToLower())
                                    {
                                        reportSubSummary.ResultId = (int)FBReportResult.Pending;
                                    }

                                    if (subSummary.Result != null && subSummary.Result.ToLower() == FBReportResult.Not_Applicable.ToString().ToLower())
                                    {
                                        reportSubSummary.ResultId = (int)FBReportResult.Not_Applicable;
                                    }

                                    if (subSummary.Result != null && subSummary.Result.ToLower() == FBReportResult.Missing.ToString().ToLower())
                                    {
                                        reportSubSummary.ResultId = (int)FBReportResult.Missing;
                                    }
                                    reportSummary.FbReportInspSubSummaries.Add(reportSubSummary);
                                    _fbRepo.AddEntity(reportSubSummary);
                                }
                            }
                        }

                        if (inspectionSummary.Photos != null && inspectionSummary.Photos.Any())
                        {
                            foreach (var data in inspectionSummary.Photos)
                            {
                                if (!string.IsNullOrEmpty(data))
                                {
                                    var reportSummaryPhoto = new FbReportInspSummaryPhoto()
                                    {
                                        Active = true,
                                        CreatedOn = DateTime.Now,
                                        Photo = data
                                    };
                                    reportSummary.FbReportInspSummaryPhotos.Add(reportSummaryPhoto);
                                    _fbRepo.AddEntity(reportSummaryPhoto);
                                }
                            }
                        }

                        // Adding photos to summary
                        if (inspectionSummary.Pictures != null && inspectionSummary.Pictures.Any())
                        {
                            foreach (var data in inspectionSummary.Pictures)
                            {
                                if (data != null)
                                {
                                    // set API product id from fb Product Id
                                    int? productId = null;

                                    if (data.ProductId != null && data.ProductId > 0)
                                    {
                                        productId = getProductIdFromPoList(productPoList, data.ProductId);
                                    }

                                    var reportSummaryPhoto = new FbReportInspSummaryPhoto()
                                    {
                                        Active = true,
                                        CreatedOn = DateTime.Now,
                                        Photo = data.Path,
                                        ProductId = productId > 0 ? productId : null,
                                        Description = data.Description
                                    };
                                    reportSummary.FbReportInspSummaryPhotos.Add(reportSummaryPhoto);
                                    _fbRepo.AddEntity(reportSummaryPhoto);
                                }
                            }
                        }

                        // Adding remarks to summary
                        if (inspectionSummary.Remarks != null && inspectionSummary.Remarks.Any())
                        {
                            foreach (var remark in inspectionSummary.Remarks)
                            {
                                if (remark != null)
                                {

                                    // set API product id from fb Product Id
                                    int? productId = null;

                                    if (remark.ProductId != null && remark.ProductId > 0)
                                    {
                                        productId = getProductIdFromPoList(productPoList, remark.ProductId);
                                    }

                                    var reportProblematicRemarks = new FbReportProblematicRemark()
                                    {
                                        Remarks = remark.Remarks,
                                        ProductId = productId > 0 ? productId : null,
                                        Result = remark.Result,
                                        Active = true,
                                        CreatedOn = DateTime.Now,
                                        SubCategory = remark.SubCategory,
                                        SubCategory2 = remark.SubCategory2,
                                        CustomerRemarkCode = remark.CustomerRemarkCode
                                    };

                                    reportSummary.FbReportProblematicRemarks.Add(reportProblematicRemarks);
                                    _fbRepo.AddEntity(reportProblematicRemarks);
                                }
                            }
                        }

                        reportData.FbReportInspSummaries.Add(reportSummary);
                        _fbRepo.AddEntity(reportSummary);

                    }
                }

                await AddFbReportQuantityDetails(request, reportData, productPoList);

                await AddFbReportDefects(request, reportData, productPoList);

                AddFbReportQcList(request, reportData);

                AddFbReportReviewerList(request, reportData);

                AddFbReportAdditionalPhotos(request, reportData, productPoList);

                AddFbReportProductInfo(request, reportData, productPoList);

                AddFbReportProductDimention(request, reportData, productPoList);

                AddFbReportProductWeight(request, reportData, productPoList);

                AddFbReportProductPackingDimention(request, reportData, productPoList);

                AddFbReportProductPackingWeight(request, reportData, productPoList);

                AddFbReportProductPackingInfo(request, reportData, productPoList);

                AddFbReportProductBatteryInfo(request, reportData, productPoList);

                AddFbReportSamplePickings(request, reportData, productPoList);

                AddFbReportSampleTypes(request, reportData, productPoList);

                AddFbReportOtherInformations(request, reportData);

                AddFbReportBarCodeInfoList(request, reportData);

                AddFbReportFabricControlMadeWith(request, reportData);

                AddFbReportRDNumbers(request, reportData, productPoList);

                //packing for packaging labelling product
                AddFbReportPackingPackagingLabellingProducts(request, reportData);

                //packing for products
                AddFbReportPackingProducts(request, reportData);

                AddFbReportQualityPlans(request, reportData);

                if (request.Product != null && request.Product.Defects != null && request.Product.Defects.Any())
                    AddFbReportDefectsFromProductAndPacking(reportData, request.Product.Defects, FbReportPackageType.Workmanship);

                if (request.PackingPackagingLabelling != null && request.PackingPackagingLabelling.Defects != null && request.PackingPackagingLabelling.Defects.Any() && productPoList != null && productPoList.Any())
                {
                    var productPo = productPoList.FirstOrDefault();
                    AddFbReportDefectsFromProductAndPacking(reportData, request.PackingPackagingLabelling.Defects, FbReportPackageType.Packing, productPo.poTrnsactionId, productPo.colorTransactionId);
                }

                if (request.QualityPlans != null && request.QualityPlans.Any() && productPoList != null && productPoList.Any())
                {
                    var productPo = productPoList.FirstOrDefault();
                    var defectList = request.QualityPlans.Where(x => x.MeasurementDefectsPOM != null).SelectMany(x => x.MeasurementDefectsPOM).Where(x => x.Quantity.HasValue).ToList();
                    AddFbReportQualityPlanDefects(reportData, defectList, productPo.poTrnsactionId, productPo.colorTransactionId);
                }

                var activeDefects = reportData.FbReportInspDefects.Where(x => x.Active == true);
                if (activeDefects.Any())
                {
                    reportData.FoundMajor = activeDefects.Select(x => x.Major).Sum();
                    reportData.FoundMinor = activeDefects.Select(x => x.Minor).Sum();
                    reportData.FoundCritical = activeDefects.Select(x => x.Critical).Sum();
                }

                await _fbRepo.UpdateInspectionFbReport(reportData);

                await AddOrUpdateFactoryGPSInfo(request, reportData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// get product Id from 
        /// </summary>
        /// <param name="productPoList"></param>
        /// <param name="fbProductId"></param>
        /// <returns></returns>
        private int getProductIdFromPoList(List<ReportProductsAndPo> productPoList, int? fbProductId)
        {

            if (productPoList.Any())
            {
                return productPoList.Where(x => x.fbProductId == fbProductId).Select(x => x.productId).FirstOrDefault();
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Add fb reprot quantity details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        /// <returns></returns>
        private async Task AddFbReportQuantityDetails(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.QuantityDetails != null && request.QuantityDetails.Any())
            {
                foreach (var quantityRequest in request.QuantityDetails)
                {

                    int? poTransactionId = null;
                    int? poColorTransactionId = null;
                    string strPoNumber = (!string.IsNullOrEmpty(quantityRequest.Pono)) ? quantityRequest.Pono.Trim() : string.Empty;
                    string strColorName = (!string.IsNullOrEmpty(quantityRequest.Color)) ? quantityRequest.Color.Trim() : string.Empty;

                    if (quantityRequest.ContainerId > 0)
                    {
                        poTransactionId = productPoList.Where(x => x.poNumber.Trim() == strPoNumber && x.fbProductId == quantityRequest.ProductId
                                     && x.containerRefId == quantityRequest.ContainerId.GetValueOrDefault()).Select(x => x.poTrnsactionId).FirstOrDefault();
                    }
                    else
                    {
                        poTransactionId = productPoList.Where(x => x.poNumber.Trim() == strPoNumber && x.fbProductId == quantityRequest.ProductId)
                                        .Select(x => x.poTrnsactionId).FirstOrDefault();
                    }

                    if (reportData.Inspection.BusinessLine == (int)BusinessLine.SoftLine)
                        poColorTransactionId = productPoList.Where(x => x.colorName.Trim() == strColorName && x.poNumber.Trim() == strPoNumber
                                                && x.fbProductId == quantityRequest.ProductId).Select(x => x.colorTransactionId).FirstOrDefault();

                    if (poTransactionId != null && poTransactionId > 0)
                    {
                        var reportQuantity = new FbReportQuantityDetail()
                        {
                            Active = true,
                            CreatedOn = DateTime.Now,
                            InspectedQuantity = quantityRequest.InspectedQuantity,
                            OrderQuantity = quantityRequest.OrderQuantity,
                            PresentedQuantity = quantityRequest.PresentedQuantity,
                            InspPoTransactionId = poTransactionId.GetValueOrDefault(),
                            InspColorTransactionId = poColorTransactionId,
                            ProductionStatus = Double.TryParse(quantityRequest.ProductionStatus, out double prodStatus) ? prodStatus : 0,
                            PackingStatus = Double.TryParse(quantityRequest.PackingStatus, out double packingStatus) ? packingStatus : 0,
                            TotalUnits = quantityRequest.TotalUnits,
                            TotalCartons = Double.TryParse(quantityRequest.TotalCartons, out double totalCarton) ? totalCarton : 0,
                            FinishedPackedUnits = Double.TryParse(quantityRequest.FinishedPackedUnits, out double finPackUnits) ? finPackUnits : 0,
                            FinishedUnpackedUnits = Double.TryParse(quantityRequest.FinishedUnpackedUnits, out double finUnPackUnits) ? finUnPackUnits : 0,
                            NotFinishedUnits = Double.TryParse(quantityRequest.NotFinishedUnits, out double notFinUnits) ? notFinUnits : 0,
                            SelectCtnNo = quantityRequest.SelectCtnNO,
                            SelectCtnQty = Double.TryParse(quantityRequest.SelectCtnQty, out double selectCtnQty) ? selectCtnQty : 0,
                            FabricPoints100Sqy = quantityRequest.Points100Sqy,
                            FabricAcceptanceCriteria = quantityRequest.AcceptanceCriteria,
                            ProducedQuantity = quantityRequest.ProducedQuantity,
                            FabricOverLessProducedQty = quantityRequest.OverLessProducedQty,
                            FabricRejectedQuantity = quantityRequest.RejectedQuantity,
                            FabricRejectedRolls = quantityRequest.RejectedRolls,
                            FabricDemeritPts = quantityRequest.DemeritPts,
                            FabricTolerance = quantityRequest.Tolerance,
                            FabricRating = quantityRequest.Rating
                        };

                        reportData.FbReportQuantityDetails.Add(reportQuantity);

                        // if valid container and size then update
                        if (quantityRequest.ContainerId > 0 && !string.IsNullOrEmpty(quantityRequest.ContainerSize))
                        {
                            var containerSizeId = await _fbRepo.GetContainersizeId(quantityRequest.ContainerSize);
                            reportData.InspContainerTransactions.Where(x => x.Id == quantityRequest.ContainerId).FirstOrDefault().ContainerSize = containerSizeId;
                        }

                        _fbRepo.AddEntity(reportQuantity);
                    }
                }
            }
        }

        /// <summary>
        /// Add fb report defects
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private async Task AddFbReportDefects(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.Defects != null && request.Defects.Any())
            {
                foreach (var defectInfo in request.Defects)
                {
                    if (defectInfo.DefectDetails != null && defectInfo.DefectDetails.Any())
                    {
                        foreach (var defect in defectInfo.DefectDetails)
                        {
                            if (defect != null)
                            {
                                int? poTransactionId = null;
                                int? poColorTransactionId = null;

                                string strPoNumber = (!string.IsNullOrEmpty(defectInfo.Pono)) ? defectInfo.Pono.Trim() : string.Empty;
                                string strColorName = (!string.IsNullOrEmpty(defectInfo.Color)) ? defectInfo.Color.Trim() : string.Empty;

                                if (defectInfo.ContainerId > 0)
                                {
                                    poTransactionId = productPoList.Where(x => x.poNumber.Trim() == strPoNumber && x.fbProductId == defectInfo.ProductId
                                    && x.containerRefId == defectInfo.ContainerId.GetValueOrDefault()).Select(x => x.poTrnsactionId).FirstOrDefault();
                                }
                                else
                                {
                                    poTransactionId = productPoList.Where(x => x.poNumber.Trim() == strPoNumber && x.fbProductId == defectInfo.ProductId)
                                        .Select(x => x.poTrnsactionId).FirstOrDefault();
                                }

                                if (reportData.Inspection.BusinessLine == (int)BusinessLine.SoftLine)
                                    poColorTransactionId = productPoList.Where(x => x.colorName.Trim() == strColorName && x.poNumber.Trim() == strPoNumber
                                           && x.fbProductId == defectInfo.ProductId).Select(x => x.colorTransactionId).FirstOrDefault();

                                if (poTransactionId != null && poTransactionId > 0)
                                {
                                    var reportDefect = new FbReportInspDefect()
                                    {
                                        Active = true,
                                        CreatedOn = DateTime.Now,
                                        Description = defect.Description,
                                        Critical = defect.DefectFound?.Critical,
                                        Major = defect.DefectFound?.Major,
                                        Minor = defect.DefectFound?.Minor,
                                        QtyRejected = defect.DefectFound?.Qty_Rejected,
                                        QtyReplaced = defect.DefectFound?.Qty_Replaced,
                                        QtyReworked = defect.DefectFound?.Qty_Reworked,
                                        Position = defect.Position,
                                        Code = defect.Code,
                                        Zone = defect.Zone,
                                        Size = defect.Size,
                                        Reparability = defect.Reparability,
                                        GarmentGrade = defect.Garment_grade,
                                        InspPoTransactionId = poTransactionId.GetValueOrDefault(),
                                        InspColorTransactionId = poColorTransactionId,
                                        CategoryId = defect.CategoryId,
                                        CategoryName = defect.CategoryName,
                                        DefectId = defect.Id,
                                        DefectInfo = defect.DefectInfo
                                    };

                                    // Add defect picture in defect details
                                    if (defect.Pictures != null && defect.Pictures.Any())
                                    {
                                        foreach (var picture in defect.Pictures)
                                        {
                                            var defectPhoto = new FbReportDefectPhoto()
                                            {
                                                Description = picture.Description,
                                                Path = picture.Path,
                                                Active = true,
                                                CreatedOn = DateTime.Now,
                                            };

                                            reportDefect.FbReportDefectPhotos.Add(defectPhoto);
                                            _fbRepo.AddEntity(defectPhoto);
                                        }
                                    }
                                    reportData.FbReportInspDefects.Add(reportDefect);
                                    _fbRepo.AddEntity(reportDefect);


                                    var fabricReportDefect = new FbReportFabricDefect()
                                    {
                                        AcceptanceCriteria = defectInfo.AcceptanceCriteria,
                                        Active = true,
                                        Code = defect.Code,
                                        CreatedOn = DateTime.Now,
                                        Description = defect.Description,
                                        DyeLot = defectInfo.DyeLot,
                                        InspColorTransactionId = poColorTransactionId,
                                        InspPoTransactionId = poColorTransactionId,
                                        LengthActual = defectInfo.LengthActual,
                                        LengthOriginal = defectInfo.LengthOriginal,
                                        LengthUnit = defectInfo.LengthUnit,
                                        Location = defect.Location,
                                        Point = defect.Point,
                                        Points100Sqy = defectInfo.Points100Sqy,
                                        Result = defectInfo.Result,
                                        RollNumber = defectInfo.RollNumber,
                                        WidthOriginal = defectInfo.WidthOriginal,
                                        WidthActual = defectInfo.WidthActual,
                                        WeightOriginal = defectInfo.WeightOriginal,
                                        WeightActual = defectInfo.WeightActual
                                    };

                                    reportData.FbReportFabricDefects.Add(fabricReportDefect);
                                    _fbRepo.AddEntity(fabricReportDefect);
                                }
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add Fb Report Qc List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportQcList(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (request.QcList != null && request.QcList.Any())
            {
                foreach (var qcId in request.QcList)
                {
                    if (qcId != null && qcId != 0)
                    {
                        var reportQcInfo = new FbReportQcdetail()
                        {
                            Active = true,
                            CreatedOn = DateTime.Now,
                            QcId = qcId
                        };
                        reportData.FbReportQcdetails.Add(reportQcInfo);
                        _fbRepo.AddEntity(reportQcInfo);
                    }

                }
            }
        }

        /// <summary>
        /// Add Fb Report Reviewer List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportReviewerList(FbReportDataRequest request, FbReportDetail reportData)
        {
            // Add reviewer list from FB
            if (request.ReviewerList != null && request.ReviewerList.Any())
            {
                foreach (var reviewerId in request.ReviewerList)
                {
                    if (reviewerId != null && reviewerId != 0)
                    {
                        var reportReviewerInfo = new FbReportReviewer()
                        {
                            Active = true,
                            CreatedOn = DateTime.Now,
                            ReviewerId = reviewerId
                        };
                        reportData.FbReportReviewers.Add(reportReviewerInfo);
                        _fbRepo.AddEntity(reportReviewerInfo);
                    }

                }
            }
        }

        /// <summary>
        /// Add report additional photos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportAdditionalPhotos(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.AdditionalPictures != null && request.AdditionalPictures.Any())
            {
                foreach (var data in request.AdditionalPictures)
                {
                    if (data != null)
                    {
                        // set API product id from fb Product Id
                        int? productId = null;

                        if (data.ProductId != null && data.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, data.ProductId);
                        }

                        var reportAdditionalPhoto = new FbReportAdditionalPhoto()
                        {
                            Active = true,
                            CreatedOn = DateTime.Now,
                            PhotoPath = data.Path,
                            ProductId = productId > 0 ? productId : null,
                            Description = data.Description

                        };
                        reportData.FbReportAdditionalPhotos.Add(reportAdditionalPhoto);
                        _fbRepo.AddEntity(reportAdditionalPhoto);
                    }
                }
            }
        }

        /// <summary>
        /// Add fb report product info 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportProductInfo(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.ProductInfo != null && request.ProductInfo.Any())
            {
                foreach (var product in request.ProductInfo)
                {
                    if (product != null)
                    {

                        // set API product id from fb Product Id
                        int? productId = null;

                        if (product.ProductId != null && product.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, product.ProductId);
                        }

                        var reportComments = new FbReportComment()
                        {
                            Active = true,
                            CreatedOn = DateTime.Now,
                            Comments = product.Remarks,
                            ProductId = productId > 0 ? productId : null,
                            Category = product.Category,
                            SubCategory = product.SubCategory,
                            SubCategory2 = product.Subcategory2,
                            CustomerReferenceCode = product.CustomerRemarkCode
                        };

                        reportData.FbReportComments.Add(reportComments);
                        _fbRepo.AddEntity(reportComments);
                    }
                }
            }
        }

        /// <summary>
        /// Add Fb Report Product Dimention
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportProductDimention(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.ProductDimensions != null && request.ProductDimensions.Any())
            {
                foreach (var dimention in request.ProductDimensions)
                {
                    if (dimention != null)
                    {

                        // set API product id from fb Product Id
                        int? productId = null;

                        if (dimention.ProductId != null && dimention.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, dimention.ProductId);
                        }

                        var productDimention = new FbReportProductDimention()
                        {
                            ProductId = productId > 0 ? productId : null,
                            SpecClientValuesH = dimention.SpecClientValuesH,
                            SpecClientValuesL = dimention.SpecClientValuesL,
                            SpecClientValuesW = dimention.SpecClientValuesW,
                            DiscrepancyToSpec = dimention.DiscrepancyToSpec,
                            DimensionPackValuesH = dimention.DimensionPackValuesH,
                            DimensionPackValuesL = dimention.DimensionPackValuesL,
                            DimensionPackValuesW = dimention.DimensionPackValuesW,
                            DiscrepancyToPack = dimention.DiscrepancyToPack,
                            MeasuredValuesH = dimention.MeasuredValuesH,
                            MeasuredValuesL = dimention.MeasuredValuesL,
                            MeasuredValuesW = dimention.MeasuredValuesW,
                            NoPcs = dimention.NoPcs,
                            Tolerance = dimention.Tolerance,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            Description = dimention.Description,
                            Unit = dimention.Unit
                        };

                        reportData.FbReportProductDimentions.Add(productDimention);
                        _fbRepo.AddEntity(productDimention);
                    }
                }
            }
        }
        /// <summary>
        /// Add Fb Report Product weight
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportProductWeight(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.ProductWeight != null && request.ProductWeight.Any())
            {
                foreach (var weight in request.ProductWeight)
                {
                    if (weight != null)
                    {
                        // set API product id from fb Product Id
                        int? productId = null;

                        if (weight.ProductId != null && weight.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, weight.ProductId);
                        }

                        var productWeight = new FbReportProductWeight()
                        {
                            ProductId = productId > 0 ? productId : null,
                            SpecClientValues = weight.SpecClientValues,
                            MeasuredValues = weight.MeasuredValues,
                            WeightPackValues = weight.WeightPackValues,
                            DiscrepancyToSpec = weight.DiscrepancyToSpec,
                            DiscrepancyToPack = weight.DiscrepancyToPack,
                            NoPcs = weight.NoPcs,
                            Tolerance = weight.Tolerance,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            Description = weight.Description,
                            Unit = weight.Unit
                        };

                        reportData.FbReportProductWeights.Add(productWeight);
                        _fbRepo.AddEntity(productWeight);
                    }
                }
            }
        }


        /// <summary>
        /// Add fb report product packing dimention
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportProductPackingDimention(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.PackingDimensions != null && request.PackingDimensions.Any())
            {
                foreach (var packingDimention in request.PackingDimensions)
                {
                    if (packingDimention != null)
                    {
                        int? productId = null;

                        if (packingDimention.ProductId != null && packingDimention.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, packingDimention.ProductId);
                        }

                        var productPackingDimention = new FbReportPackingDimention()
                        {
                            ProductId = productId > 0 ? productId : null,
                            SpecClientValuesH = packingDimention.SpecClientValuesH,
                            SpecClientValuesL = packingDimention.SpecClientValuesL,
                            SpecClientValuesW = packingDimention.SpecClientValuesW,
                            DiscrepancyToSpec = packingDimention.DiscrepancyToSpec,
                            DiscrepancyToPacking = packingDimention.DiscrepancyToPacking,
                            MeasuredValuesH = packingDimention.MeasuredValuesH,
                            MeasuredValuesL = packingDimention.MeasuredValuesL,
                            MeasuredValuesW = packingDimention.MeasuredValuesW,
                            NoPcs = packingDimention.NoPcs,
                            Tolerance = packingDimention.Tolerance,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            PackingType = packingDimention.PackingPackaging,
                            PrintedPackValuesH = packingDimention.DimensionPackValuesH,
                            PrintedPackValuesL = packingDimention.DimensionPackValuesL,
                            PrintedPackValuesW = packingDimention.DimensionPackValuesW,
                            Unit = packingDimention.Unit
                        };

                        reportData.FbReportPackingDimentions.Add(productPackingDimention);
                        _fbRepo.AddEntity(productPackingDimention);
                    }
                }
            }
        }



        /// <summary>
        /// Add Fb Report Product Packing weight 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportProductPackingWeight(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.PackingWeight != null && request.PackingWeight.Any())
            {
                foreach (var packingWeight in request.PackingWeight)
                {
                    if (packingWeight != null)
                    {

                        // set API product id from fb Product Id
                        int? productId = null;

                        if (packingWeight.ProductId != null && packingWeight.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, packingWeight.ProductId);
                        }

                        var productPackingWeight = new FbReportPackingWeight()
                        {
                            ProductId = productId > 0 ? productId : null,
                            SpecClientValues = packingWeight.SpecClientValues,
                            MeasuredValues = packingWeight.MeasuredValues,
                            WeightPackValues = packingWeight.WeightPackInfoValues,
                            DiscrepancyToSpec = packingWeight.DiscrepancyToSpec,
                            NoPcs = packingWeight.NoPcs,
                            Tolerance = packingWeight.Tolerance,
                            DiscrepancyToPacking = packingWeight.DiscrepancyToPacking,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            PackingType = packingWeight.PackingPackaging,
                            Unit = packingWeight.Unit
                        };
                        reportData.FbReportPackingWeights.Add(productPackingWeight);
                        _fbRepo.AddEntity(productPackingWeight);
                    }
                }
            }
        }

        /// <summary>
        /// Add Fb Report product packing info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportProductPackingInfo(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.ProductPackingInfo != null && request.ProductPackingInfo.Any())
            {
                foreach (var packingInfo in request.ProductPackingInfo)
                {
                    if (packingInfo != null)
                    {

                        // set API product id from fb Product Id
                        int? productId = null;

                        if (packingInfo.ProductId != null && packingInfo.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, packingInfo.ProductId);
                        }
                        var productPackingInfo = new FbReportPackingInfo()
                        {
                            ProductId = productId > 0 ? productId : null,
                            Active = true,
                            Location = packingInfo.Location,
                            MaterialType = packingInfo.MaterialType,
                            NetWeightPerQty = packingInfo.NetWeightPerQty,
                            PackagingDesc = packingInfo.PackagingDesc,
                            PieceNo = packingInfo.PieceNo,
                            Quantity = packingInfo.Quantity,
                            CreatedOn = DateTime.Now
                        };
                        reportData.FbReportPackingInfos.Add(productPackingInfo);
                        _fbRepo.AddEntity(productPackingInfo);
                    }
                }
            }
        }

        /// <summary>
        /// Add Fb Report paroduct battery info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportProductBatteryInfo(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.ProductBatteriesInfo != null && request.ProductBatteriesInfo.Any())
            {
                foreach (var batteryInfo in request.ProductBatteriesInfo)
                {
                    if (batteryInfo != null)
                    {

                        // set API product id from fb Product Id
                        int? productId = null;

                        if (batteryInfo.ProductId != null && batteryInfo.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, batteryInfo.ProductId);
                        }

                        var productBatteryInfo = new FbReportPackingBatteryInfo()
                        {
                            ProductId = productId > 0 ? productId : null,
                            Active = true,
                            Location = batteryInfo.Location,
                            NetWeightPerQty = batteryInfo.NetWeightPerQty,
                            BatteryModel = batteryInfo.BatteryModel,
                            BatteryType = batteryInfo.BatteryType,
                            Quantity = batteryInfo.Quantity,
                            CreatedOn = DateTime.Now
                        };
                        reportData.FbReportPackingBatteryInfos.Add(productBatteryInfo);
                        _fbRepo.AddEntity(productBatteryInfo);
                    }
                }
            }
        }



        /// <summary>
        /// Add fb Report Sample pickings 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportSamplePickings(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.SamplePickings != null && request.SamplePickings.Any())
            {
                foreach (var pickings in request.SamplePickings)
                {
                    if (pickings != null)
                    {
                        // set API product id from fb Product Id
                        int? productId = null;

                        if (pickings.ProductId != null && pickings.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, pickings.ProductId);
                        }

                        var samplePicking = new FbReportSamplePicking()
                        {
                            ProductId = productId > 0 ? productId : null,
                            Active = true,
                            Quantity = pickings.Quantity,
                            SampleType = pickings.SampleType,
                            Comments = pickings.Comments,
                            Destination = pickings.Destination,
                            CreatedOn = DateTime.Now
                        };
                        reportData.FbReportSamplePickings.Add(samplePicking);
                        _fbRepo.AddEntity(samplePicking);
                    }
                }
            }
        }

        /// <summary>
        /// Add fb report Sample types
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportSampleTypes(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.SampleTypes != null && request.SampleTypes.Any())
            {
                foreach (var sample in request.SampleTypes)
                {
                    if (sample != null)
                    {
                        // set API product id from fb Product Id
                        int? productId = null;

                        if (sample.ProductId != null && sample.ProductId > 0)
                        {
                            productId = getProductIdFromPoList(productPoList, sample.ProductId);
                        }

                        var sampleTypes = new FbReportSampleType()
                        {
                            ProductId = productId > 0 ? productId : null,
                            Active = true,
                            Quantity = sample.Quantity,
                            SampleType = sample.SampleType,
                            Comments = sample.Comments,
                            Description = sample.Description,
                            CreatedOn = DateTime.Now
                        };
                        reportData.FbReportSampleTypes.Add(sampleTypes);
                        _fbRepo.AddEntity(sampleTypes);
                    }
                }
            }
        }

        /// <summary>
        /// UpdateFactoryGPSInfo - head office only
        /// </summary>
        /// <param name="request"></param>
        private async Task AddOrUpdateFactoryGPSInfo(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (!string.IsNullOrEmpty(request.FactoryCoordinates))
            {
                var coordinates = request.FactoryCoordinates.Split(",");
                var longitude = coordinates[0];
                var latitude = coordinates[1];

                if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
                {
                    var bookingReportsFactory = _fbRepo.GetInspectionReportsFactoryAddress(reportData.Id).Result;

                    var bookingContainerReportsFactory = bookingReportsFactory == null ? _fbRepo.GetContainerReportsFactoryAddress(reportData.Id).Result : null;

                    var factorybyHeadOffice = bookingReportsFactory != null ?
                                              bookingReportsFactory.Inspection.Factory.SuAddresses.FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice) :
                                              bookingContainerReportsFactory != null ? bookingContainerReportsFactory.
                                              Inspection.Factory.SuAddresses.FirstOrDefault(x => x.AddressTypeId == (int)Supplier_Address_Type.HeadOffice) : null;

                    if (factorybyHeadOffice != null)
                    {
                        factorybyHeadOffice.Latitude = Convert.ToDecimal(latitude);
                        factorybyHeadOffice.Longitude = Convert.ToDecimal(longitude);
                        await _fbRepo.UpdateInspectionFactoryAddress(factorybyHeadOffice);
                    }
                }
            }
        }

        /// <summary>
        /// Add Fb Report other Informations
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportOtherInformations(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (request.OtherInformations != null && request.OtherInformations.Any())
            {
                foreach (var otherInfo in request.OtherInformations)
                {
                    if (otherInfo != null)
                    {

                        // set API product id from fb Product Id
                        int? productId = null;

                        if (otherInfo.ProductId != null && otherInfo.ProductId > 0)
                        {
                            productId = _fbRepo.GetProductIdByFbProductId(otherInfo.ProductId).Result;
                        }
                        var reportOtherInfo = new FbReportOtherInformation()
                        {
                            ProductId = productId > 0 ? productId : null,
                            Remarks = otherInfo.Remarks,
                            SubCategory = otherInfo.SubCategory,
                            SubCategory2 = otherInfo.SubCategory2,
                            Result = otherInfo.Result,
                            Active = true,
                            CreatedOn = DateTime.Now

                        };
                        reportData.FbReportOtherInformations.Add(reportOtherInfo);
                        _fbRepo.AddEntity(reportOtherInfo);
                    }
                }
            }
        }

        private void AddFbReportBarCodeInfoList(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (request.ProductBarcodesInfo != null && request.ProductBarcodesInfo.Any())
            {
                foreach (var barcode in request.ProductBarcodesInfo)
                {
                    if (barcode != null)
                    {

                        // set API product id from fb Product Id
                        int? productId = null;

                        if (barcode.ProductId != null && barcode.ProductId > 0)
                        {
                            productId = _fbRepo.GetProductIdByFbProductId(barcode.ProductId).Result;
                        }
                        var barcodeInfo = new FbReportProductBarcodesInfo()
                        {
                            ProductId = productId > 0 ? productId : null,
                            Description = barcode.Description,
                            BarCode = barcode.BarCode
                        };
                        reportData.FbReportProductBarcodesInfos.Add(barcodeInfo);
                        _fbRepo.AddEntity(barcodeInfo);
                    }
                }
            }
        }

        private void AddFbReportFabricControlMadeWith(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (request.ControlMadeWith != null && request.ControlMadeWith.Any())
            {
                foreach (var controlMadeWith in request.ControlMadeWith)
                {
                    if (controlMadeWith != null)
                    {
                        var controlsMadeWith = new FbReportFabricControlmadeWith()
                        {
                            Name = controlMadeWith,
                            Active = true,
                            CreatedOn = DateTime.Now
                        };
                        reportData.FbReportFabricControlmadeWiths.Add(controlsMadeWith);
                        _fbRepo.AddEntity(controlsMadeWith);
                    }
                }
            }
        }

        private void AddFbReportPackingPackagingLabellingProducts(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (request.PackingPackagingLabelling != null)
            {
                var fbReportPackingPackagingLabellingProduct = new FbReportPackingPackagingLabellingProduct()
                {
                    CreatedOn = DateTime.Now,
                    PackingType = (int)FbReportPackageType.Packing,
                    Critical = request.PackingPackagingLabelling?.TotalDefects?.Critical,
                    Minor = request.PackingPackagingLabelling?.TotalDefects.Minor,
                    Major = request.PackingPackagingLabelling?.TotalDefects.Major,
                    SampleSizeCtns = request.PackingPackagingLabelling?.SampleSizeCtns,
                    CartonQty = request.PackingPackagingLabelling?.CartonQty
                };

                AddFbReportPackingPackagingLabellingProductDefects(request.PackingPackagingLabelling.Defects, fbReportPackingPackagingLabellingProduct);
                reportData.FbReportPackingPackagingLabellingProducts.Add(fbReportPackingPackagingLabellingProduct);
                _fbRepo.AddEntity(fbReportPackingPackagingLabellingProduct);
            }
        }

        private void AddFbReportPackingProducts(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (request.Product != null)
            {
                var fbReportPackingPackagingLabellingProduct = new FbReportPackingPackagingLabellingProduct()
                {
                    CreatedOn = DateTime.Now,
                    PackingType = (int)FbReportPackageType.Workmanship,
                    Critical = request.Product?.TotalDefects?.Critical,
                    Minor = request.Product?.TotalDefects?.Minor,
                    Major = request.Product?.TotalDefects?.Major,
                    TotalDefectiveUnits = request.Product?.TotalDefectiveUnits,
                    SampleSizeCtns = request.Product?.SampleSize,
                    CartonQty = request.Product?.CartonQty
                };

                AddFbReportPackingPackagingLabellingProductDefects(request.Product?.Defects, fbReportPackingPackagingLabellingProduct);
                reportData.FbReportPackingPackagingLabellingProducts.Add(fbReportPackingPackagingLabellingProduct);
                _fbRepo.AddEntity(fbReportPackingPackagingLabellingProduct);
            }
        }


        private void AddFbReportPackingPackagingLabellingProductDefects(List<Defect> defects, FbReportPackingPackagingLabellingProduct fbReportPackingPackagingLabellingProduct)
        {
            if (defects != null && defects.Any())
            {
                foreach (var item in defects)
                {
                    var fbReportPackingPackagingLabellingProductDefect = new FbReportPackingPackagingLabellingProductDefect()
                    {
                        Code = item.Code,
                        CreatedOn = DateTime.Now,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Rdnumber = item.RDNumber,
                        Severity = item.Severity,
                        PackingType = fbReportPackingPackagingLabellingProduct.PackingType
                    };
                    fbReportPackingPackagingLabellingProduct.FbReportPackingPackagingLabellingProductDefects.Add(fbReportPackingPackagingLabellingProductDefect);
                    _fbRepo.AddEntity(fbReportPackingPackagingLabellingProductDefect);
                }
            }
        }

        private void AddFbReportDefectsFromProductAndPacking(FbReportDetail fbReportDetail, List<Defect> defects, FbReportPackageType packageType, int? packaingPoId = null, int? packingPoColorId = null)
        {
            if (defects == null || !defects.Any())
                return;

            foreach (var defect in defects)
            {
                if (packageType == FbReportPackageType.Workmanship)
                {
                    FbReportRdnumber fbReportRdNumber = null;
                    if (!string.IsNullOrWhiteSpace(defect.RDNumber))
                        fbReportRdNumber = fbReportDetail.FbReportRdnumbers.FirstOrDefault(x => x.Rdnumber == defect.RDNumber);
                    else
                        fbReportRdNumber = fbReportDetail.FbReportRdnumbers.FirstOrDefault();

                    packaingPoId = fbReportRdNumber?.PoId;
                    packingPoColorId = fbReportRdNumber?.PoColorId;
                }

                if (packaingPoId.HasValue)
                {
                    var reportInspDefect = new FbReportInspDefect()
                    {
                        Code = defect.Code,
                        Description = defect.Description,
                        CategoryName = defect.Classification,
                        CreatedOn = DateTime.Now,
                        DefectCheckPoint = (int)packageType,
                        InspPoTransactionId = packaingPoId.GetValueOrDefault(),
                        InspColorTransactionId = packingPoColorId,
                        Active = true,
                    };
                    if (!Enum.TryParse<DefectType>(defect.Severity, out DefectType defectType))
                    {
                        continue;
                    }
                    switch (defectType)
                    {
                        case DefectType.Critical:
                            reportInspDefect.Critical = defect.Quantity;
                            break;
                        case DefectType.Major:
                            reportInspDefect.Major = defect.Quantity;
                            break;
                        case DefectType.Minor:
                            reportInspDefect.Minor = defect.Quantity;
                            break;
                        default:
                            continue;
                    }

                    // Add defect picture in defect details
                    if (defect.Pictures != null && defect.Pictures.Any())
                    {
                        foreach (var picture in defect.Pictures)
                        {
                            var defectPhoto = new FbReportDefectPhoto()
                            {
                                Description = picture.Description,
                                Path = picture.Path,
                                Active = true,
                                CreatedOn = DateTime.Now
                            };

                            reportInspDefect.FbReportDefectPhotos.Add(defectPhoto);
                            _fbRepo.AddEntity(defectPhoto);
                        }
                    }

                    fbReportDetail.FbReportInspDefects.Add(reportInspDefect);
                    _fbRepo.AddEntity(reportInspDefect);
                }
            }
        }


        private void AddFbReportQualityPlanDefects(FbReportDetail fbReportDetail, List<MeasurementDefectsPOM> measurementDefectsPOMs, int packaingPoId, int? packingPoColorId)
        {
            foreach (var measurementDefects in measurementDefectsPOMs)
            {
                var reportInspDefect = new FbReportInspDefect()
                {
                    Code = measurementDefects.codePOM,
                    Description = measurementDefects.POM,
                    CreatedOn = DateTime.Now,
                    DefectCheckPoint = (int)FbReportPackageType.Measurement,
                    InspPoTransactionId = packaingPoId,
                    InspColorTransactionId = packingPoColorId,
                    Active = true,
                    Position = measurementDefects.SpecZone,
                    Major = measurementDefects.Quantity
                };

                fbReportDetail.FbReportInspDefects.Add(reportInspDefect);
                _fbRepo.AddEntity(reportInspDefect);
            }
        }

        /// <summary>
        /// Add fb report rd numbers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportRDNumbers(FbReportDataRequest request, FbReportDetail reportData, List<ReportProductsAndPo> productPoList)
        {
            if (request.RDNumbers != null && request.RDNumbers.Any())
            {
                foreach (var rdNumber in request.RDNumbers)
                {
                    if (rdNumber != null)
                    {
                        int? poTransactionId = null;
                        int? poColorTransactionId = null;
                        int? inspProductId = null;
                        string strPoNumber = (!string.IsNullOrEmpty(rdNumber.Pono)) ? rdNumber.Pono.Trim() : string.Empty;
                        string strColorName = (!string.IsNullOrEmpty(rdNumber.Color)) ? rdNumber.Color.Trim() : string.Empty;

                        if (productPoList != null)
                        {
                            poTransactionId = productPoList.Where(x => x.poNumber.Trim() == strPoNumber && x.fbProductId == rdNumber.ProductId)
                                            .Select(x => x.poTrnsactionId).FirstOrDefault();

                            if (reportData.Inspection.BusinessLine == (int)BusinessLine.SoftLine)
                                poColorTransactionId = productPoList.Where(x => x.colorName.Trim() == strColorName && x.poNumber.Trim() == strPoNumber
                                                        && x.fbProductId == rdNumber.ProductId).Select(x => x.colorTransactionId).FirstOrDefault();
                            inspProductId = productPoList.FirstOrDefault(x => x.fbProductId == rdNumber.ProductId)?.productTransactionId;
                        }



                        var fbReportRdnumber = new FbReportRdnumber()
                        {
                            Rdnumber = rdNumber.RDNumber,
                            ProductId = inspProductId,
                            PoColorId = poColorTransactionId,
                            PoId = poTransactionId,
                            CreatedOn = DateTime.Now,
                        };
                        reportData.FbReportRdnumbers.Add(fbReportRdnumber);
                        _fbRepo.AddEntity(fbReportRdnumber);
                    }
                }
            }
        }

        /// <summary>
        /// Add fb report quality plans
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private void AddFbReportQualityPlans(FbReportDataRequest request, FbReportDetail reportData)
        {
            if (request.QualityPlans != null && request.QualityPlans.Any())
            {
                foreach (var item in request.QualityPlans)
                {
                    var fbReportQualityPlan = new FbReportQualityPlan()
                    {
                        CreatedOn = DateTime.Now,
                        Result = item.Result,
                        Title = item.Title,
                        TotalDefectiveUnits = item.TotalDefectiveUnits,
                        TotalQtyMeasurmentDefects = item.TotalQtyMeasurmentDefects,
                        TotalPiecesMeasurmentDefects = item.TotalPiecesMeasurmentDefects,
                        SampleInspected = item.SampleInspected,
                        ActualMeasuredSampleSize = item.ActualMeasuredSampleSize
                    };

                    if (item.MeasurementDefectsPOM != null && item.MeasurementDefectsPOM.Any())
                    {
                        foreach (var measurementDefectPOM in item.MeasurementDefectsPOM)
                        {
                            var fbReportQualityPlanMeasurementDefectsPom = new FbReportQualityPlanMeasurementDefectsPom()
                            {
                                CodePom = measurementDefectPOM.codePOM,
                                CriticalPom = measurementDefectPOM.CriticalPOM,
                                Pom = measurementDefectPOM.POM,
                                Quantity = measurementDefectPOM.Quantity,
                                SpecZone = measurementDefectPOM.SpecZone
                            };

                            fbReportQualityPlan.FbReportQualityPlanMeasurementDefectsPoms.Add(fbReportQualityPlanMeasurementDefectsPom);
                            _fbRepo.AddEntity(fbReportQualityPlanMeasurementDefectsPom);
                        }
                    }

                    if (item.MeasurementDefectsSize != null && item.MeasurementDefectsSize.Any())
                    {
                        foreach (var measurementDefectSize in item.MeasurementDefectsSize)
                        {
                            var fbReportQualityPlanMeasurementDefectsSize = new FbReportQualityPlanMeasurementDefectsSize()
                            {
                                Quantity = measurementDefectSize.Quantity,
                                Size = measurementDefectSize.Size
                            };

                            fbReportQualityPlan.FbReportQualityPlanMeasurementDefectsSizes.Add(fbReportQualityPlanMeasurementDefectsSize);
                            _fbRepo.AddEntity(fbReportQualityPlanMeasurementDefectsSize);
                        }
                    }


                    reportData.FbReportQualityPlans.Add(fbReportQualityPlan);
                    _fbRepo.AddEntity(fbReportQualityPlan);
                }
            }
        }


        /// <summary>
        /// Update fb report details
        /// </summary>
        /// <param name="requestEntity"></param>
        private async Task UpdateReportDetails(FbReportDetail requestEntity, FbReportDataRequest requestModelData)
        {
            try
            {
                foreach (var inspectionSummary in requestEntity?.FbReportInspSummaries.Where(x => x.Active.HasValue && x.Active.Value))
                {

                    foreach (var inspectionSummaryPhoto in inspectionSummary?.FbReportInspSummaryPhotos.Where(x => x.Active.HasValue && x.Active.Value))
                    {
                        inspectionSummaryPhoto.DeletedOn = DateTime.Now;
                        inspectionSummaryPhoto.Active = false;
                    }

                    foreach (var inspectionSubSummary in inspectionSummary?.FbReportInspSubSummaries.Where(x => x.Active.HasValue && x.Active.Value))
                    {
                        inspectionSubSummary.DeletedOn = DateTime.Now;
                        inspectionSubSummary.Active = false;
                    }

                    foreach (var problematicRemarks in inspectionSummary?.FbReportProblematicRemarks.Where(x => x.Active.HasValue && x.Active.Value))
                    {
                        problematicRemarks.DeletedOn = DateTime.Now;
                        problematicRemarks.Active = false;
                    }

                    inspectionSummary.Active = false;
                    inspectionSummary.DeletedOn = DateTime.Now;
                }

                foreach (var inspRepCusDecision in requestEntity?.InspRepCusDecisions.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    inspRepCusDecision.Active = false;
                }

                foreach (var inspectionQuantity in requestEntity?.FbReportQuantityDetails.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    inspectionQuantity.Active = false;
                    inspectionQuantity.DeletedOn = DateTime.Now;
                }

                if (requestEntity.FbReportRdnumbers != null && requestEntity.FbReportRdnumbers.Any())
                {
                    var fbReportRdnumbers = requestEntity.FbReportRdnumbers.ToList();
                    foreach (var item in fbReportRdnumbers)
                        requestEntity.FbReportRdnumbers.Remove(item);

                    if (fbReportRdnumbers.Count > 0)
                        _fbRepo.RemoveEntities(fbReportRdnumbers);
                }

                if (requestEntity.FbReportPackingPackagingLabellingProducts != null && requestEntity.FbReportPackingPackagingLabellingProducts.Any())
                {
                    var fbReportPackingPackagingLabellingProducts = requestEntity.FbReportPackingPackagingLabellingProducts.ToList();
                    foreach (var item in fbReportPackingPackagingLabellingProducts)
                    {
                        var fbReportPackingPackagingLabellingProductDefects = item.FbReportPackingPackagingLabellingProductDefects.ToList();
                        if (fbReportPackingPackagingLabellingProductDefects.Any())
                        {
                            foreach (var fbReportPackingPackagingLabellingProductDefect in fbReportPackingPackagingLabellingProductDefects)
                            {
                                item.FbReportPackingPackagingLabellingProductDefects.Remove(fbReportPackingPackagingLabellingProductDefect);
                            }
                            _fbRepo.RemoveEntities(fbReportPackingPackagingLabellingProductDefects);
                        }

                        requestEntity.FbReportPackingPackagingLabellingProducts.Remove(item);
                    }

                    if (fbReportPackingPackagingLabellingProducts.Count > 0)
                        _fbRepo.RemoveEntities(fbReportPackingPackagingLabellingProducts);
                }

                if (requestEntity.FbReportQualityPlans != null && requestEntity.FbReportQualityPlans.Any())
                {
                    var fbReportQualityPlans = requestEntity.FbReportQualityPlans.ToList();
                    if (fbReportQualityPlans.Any())
                    {
                        foreach (var item in fbReportQualityPlans)
                        {
                            var fbReportQualityPlanMeasurementDefectsPoms = item.FbReportQualityPlanMeasurementDefectsPoms.ToList();
                            if (fbReportQualityPlanMeasurementDefectsPoms.Any())
                            {
                                foreach (var qualityPlanMeasurementDefectsPoms in fbReportQualityPlanMeasurementDefectsPoms)
                                {
                                    item.FbReportQualityPlanMeasurementDefectsPoms.Remove(qualityPlanMeasurementDefectsPoms);
                                }
                                _fbRepo.RemoveEntities(fbReportQualityPlanMeasurementDefectsPoms);
                            }

                            var fbReportQualityPlanMeasurementDefectsSizes = item.FbReportQualityPlanMeasurementDefectsSizes.ToList();
                            if (fbReportQualityPlanMeasurementDefectsSizes.Any())
                            {
                                foreach (var qualityPlanMeasurementDefectsSize in fbReportQualityPlanMeasurementDefectsSizes)
                                {
                                    item.FbReportQualityPlanMeasurementDefectsSizes.Remove(qualityPlanMeasurementDefectsSize);
                                }
                                _fbRepo.RemoveEntities(fbReportQualityPlanMeasurementDefectsSizes);
                            }

                            requestEntity.FbReportQualityPlans.Remove(item);
                        }
                        _fbRepo.RemoveEntities(fbReportQualityPlans);
                    }
                }


                foreach (var reportDefects in requestEntity?.FbReportInspDefects.Where(x => x.Active.HasValue && x.Active.Value))
                {

                    foreach (var item in reportDefects?.FbReportDefectPhotos.Where(x => x.Active.HasValue && x.Active.Value))
                    {
                        item.Active = false;
                        item.DeletedOn = DateTime.Now;
                    }

                    reportDefects.Active = false;
                    reportDefects.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportQcdetails.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportReviewers.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportComments.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportProductDimentions.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }
                foreach (var rowData in requestEntity?.FbReportProductWeights.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportPackingDimentions.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportPackingWeights.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportPackingInfos.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportPackingBatteryInfos.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }


                foreach (var rowData in requestEntity?.FbReportSamplePickings.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportSampleTypes.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var rowData in requestEntity?.FbReportOtherInformations.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    rowData.Active = false;
                    rowData.DeletedOn = DateTime.Now;
                }

                foreach (var controls in requestEntity?.FbReportFabricControlmadeWiths.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    controls.Active = false;
                    controls.DeletedOn = DateTime.Now;
                }

                foreach (var fabricDefect in requestEntity?.FbReportFabricDefects.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    fabricDefect.Active = false;
                    fabricDefect.DeletedOn = DateTime.Now;
                }

                List<FbReportAdditionalPhoto> objRemovedList = new List<FbReportAdditionalPhoto>();

                var additionalPhotos = requestEntity?.FbReportAdditionalPhotos.
                                             Where(x => x.Active.HasValue && x.Active.Value).ToList();

                if (additionalPhotos != null)
                {
                    // check and remove additional photos
                    foreach (var photo in additionalPhotos)
                    {
                        objRemovedList.Add(photo);
                    }
                }

                requestEntity.MissionTitle = requestModelData.MissionTitle;
                requestEntity.ReportTitle = requestModelData.ReportTitle;
                requestEntity.OverAllResult = requestModelData.OverAllResult;
                requestEntity.ReportSummaryLink = requestModelData.ReportSummaryLink;
                requestEntity.ReportPicturePath = requestModelData.PicturesReportPath;

                // set report result id
                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Pass.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Pass;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Fail.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Fail;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Pending.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Pending;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Not_Applicable.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Not_Applicable;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Missing.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Missing;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Conformed.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Conformed;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.NotConformed.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.NotConformed;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Delay.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Delay;
                }

                if (requestModelData.OverAllResult != null && requestModelData.OverAllResult.ToLower() == FBReportResult.Note.ToString().ToLower())
                {
                    requestEntity.ResultId = (int)FBReportResult.Note;
                }

                DateTime serviceFromDate;
                DateTime serviceToDate;

                // if the dates PROPERLY CONVERTED THEN SEND TO DB
                if (DateTime.TryParseExact(requestModelData.InspectionFromDate, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out serviceFromDate))
                {
                    requestEntity.ServiceFromDate = serviceFromDate;
                }

                if (DateTime.TryParseExact(requestModelData.InspectionToDate, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out serviceToDate))
                {
                    requestEntity.ServiceToDate = serviceToDate;
                }

                requestEntity.FinalReportPath = requestModelData.FinalReportPath;
                requestEntity.MainProductPhoto = requestModelData.MainProductPhoto;

                requestEntity.ProductionStatus = Double.TryParse(requestModelData.ProductionStatus, out double prodStatus) ? prodStatus : 0;
                requestEntity.PackingStatus = Double.TryParse(requestModelData.PackingStatus, out double packingStatus) ? packingStatus : 0;

                requestEntity.ReportVersion = requestModelData.ReportVersion;
                requestEntity.ReportRevision = requestModelData.ReportRevision;

                if (requestModelData.DefectMax != null)
                {
                    requestEntity.CriticalMax = requestModelData.DefectMax?.Critical;
                    requestEntity.MajorMax = requestModelData.DefectMax?.Major;
                    requestEntity.MinorMax = requestModelData.DefectMax?.Minor;
                }

                requestEntity.MainObservations = requestModelData.MainObservations;

                if (requestModelData.Aql != null)
                {
                    requestEntity.SampleSize = requestModelData?.Aql?.Sample_Size;
                    // get the aql value for reference id
                    if (!string.IsNullOrWhiteSpace(requestModelData?.Aql?.AQL_Level))
                    {
                        requestEntity.AqlLevel = await _fbRepo.GetAqlLevelbyFbAql(requestModelData.Aql?.AQL_Level);
                    }
                    if (!string.IsNullOrWhiteSpace(requestModelData?.Aql?.Aql_Critical))
                    {
                        requestEntity.AqlCritical = Double.Parse(requestModelData.Aql?.Aql_Critical);
                    }
                    if (!string.IsNullOrWhiteSpace(requestModelData?.Aql?.Aql_Major))
                    {
                        requestEntity.AqlMajor = Double.Parse(requestModelData.Aql?.Aql_Major);
                    }
                    if (!string.IsNullOrWhiteSpace(requestModelData?.Aql?.Aql_Minor))
                    {
                        requestEntity.AqlMinor = Double.Parse(requestModelData.Aql?.Aql_Minor);
                    }
                    requestEntity.FoundCritical = requestModelData?.Aql?.Found_Critical;
                    requestEntity.FoundMajor = requestModelData?.Aql?.Found_Major;
                    requestEntity.FoundMinor = requestModelData?.Aql?.Found_Minor;
                }

                // update quantity details from fb request

                if (requestModelData.QuantityDetails != null && requestModelData.QuantityDetails.Any())
                {
                    requestEntity.InspectedQty = requestModelData?.QuantityDetails.Sum(x => x.InspectedQuantity);
                    requestEntity.OrderQty = requestModelData?.QuantityDetails.Sum(x => x.OrderQuantity);
                    requestEntity.PresentedQty = requestModelData?.QuantityDetails.Sum(x => x.PresentedQuantity);
                    requestEntity.FabricNoOfRollsPresented = requestModelData.NumberRollsPresented;
                    requestEntity.FabricNoOfLotsPresented = requestModelData.NumberLotsPresented;
                    requestEntity.FabricProducedQtyRoll = requestModelData.ProducedQtyRoll;
                    requestEntity.FabricPresentedQtyRoll = requestModelData.PresentedQtyRoll;
                    requestEntity.FabricInspectedQtyRoll = requestModelData.InspectedQtyRoll;
                    requestEntity.FabricAcceptedQtyRoll = requestModelData.AcceptedQtyRoll;
                    requestEntity.FabricRejectedQtyRoll = requestModelData.RejectedQtyRoll;
                    requestEntity.FabricMachineSpeed = requestModelData.SpeedFabricMachine;
                    requestEntity.FabricType = requestModelData.TypeOfFabric;
                    requestEntity.FabricTypeCheck = requestModelData.TypeOfCheck;
                    requestEntity.FabricFactoryType = requestModelData.TypeOfFactory;
                }
                //first priority of the inspection started date
                if (requestModelData.InspectionStartedDate.HasValue)
                {
                    requestEntity.InspectionStartedDate = requestModelData.InspectionStartedDate;
                }
                else //second priority of the Audit started date
                {
                    if (DateTime.TryParseExact(requestModelData.AuditStartedDate, StandardISO8601DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime inspectionStartedDate))
                        requestEntity.InspectionStartedDate = inspectionStartedDate;
                }

                //first priority of the inspection submitted date
                if (requestModelData.InspectionSubmittedDate.HasValue)
                {
                    requestEntity.InspectionSubmittedDate = requestModelData.InspectionSubmittedDate;
                }
                else //second priority of the audit submitted date
                {
                    if (DateTime.TryParseExact(requestModelData.AuditSubmittedDate, StandardISO8601DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime inspectionSubmittedDate))
                        requestEntity.InspectionSubmittedDate = inspectionSubmittedDate;
                }

                if (!string.IsNullOrWhiteSpace(requestModelData.InspectionStartedTime))
                    requestEntity.InspectionStartTime = requestModelData.InspectionStartedTime;
                else
                    requestEntity.InspectionStartTime = requestModelData.AuditStartedTime;

                if (!string.IsNullOrWhiteSpace(requestModelData.InspectionSubmittedTime))
                    requestEntity.InspectionEndTime = requestModelData.InspectionSubmittedTime;
                else
                    requestEntity.InspectionEndTime = requestModelData.AuditSubmittedTime;

                requestEntity.QtyInspected = requestModelData.QtyInspected;
                requestEntity.ProductCategory = requestModelData.MainCategory;
                requestEntity.KeyStyleHighRisk = requestModelData.KeyStyleHighRisk;
                requestEntity.MasterCartonPackedQuantityCtns = requestModelData.MasterCartonPackedQuantityCtns;
                requestEntity.Region = requestModelData.Region;
                requestEntity.InspectionDurationMins = requestModelData.InspectionDurationMins;
                requestEntity.NumberPommeasured = requestModelData.NumberPOMMeasured;
                if (requestModelData.DACorrelation != null)
                {
                    requestEntity.DacorrelationEnabled = true;
                    requestEntity.DacorrelationEmail = requestModelData.DACorrelation.DAEmail;
                    requestEntity.DacorrelationInspectionSampling = requestModelData.DACorrelation.InspectionSampling;
                    requestEntity.DacorrelationRate = requestModelData.DACorrelation.CorrelationRate;
                    requestEntity.DacorrelationResult = requestModelData.DACorrelation.Result;
                }
                else
                {
                    requestEntity.DacorrelationEnabled = false;
                    requestEntity.DacorrelationEmail = null;
                    requestEntity.DacorrelationInspectionSampling = null;
                    requestEntity.DacorrelationRate = null;
                    requestEntity.DacorrelationResult = null;
                }

                if (requestModelData.FactoryTour != null)
                {
                    requestEntity.FactoryTourEnabled = true;
                    requestEntity.FactoryTourBottleneckProductionStage = requestModelData.FactoryTour.BottleneckProductionStage;
                    requestEntity.FactoryTourNotConductedReason = requestModelData.FactoryTour.NotConductedReason;
                    requestEntity.FactoryTourIrregularitiesIdentified = requestModelData.FactoryTour.IrregularitiesIdentified;
                }
                else
                {
                    requestEntity.FactoryTourEnabled = false;
                    requestEntity.FactoryTourBottleneckProductionStage = null;
                    requestEntity.FactoryTourNotConductedReason = null;
                    requestEntity.FactoryTourIrregularitiesIdentified = null;
                }

                requestEntity.FillingValidatedFirstTime = requestModelData.FillingValidatedFirstTime;
                requestEntity.ReviewValidatedFirstTime = requestModelData.ReviewValidatedFirstTime;
                requestEntity.DacorrelationDone = requestModelData.DACorrelationDone;
                requestEntity.FactoryTourDone = requestModelData.FactoryTourDone;
                requestEntity.FactoryTourResult = requestModelData?.FactoryTour?.Result;
                requestEntity.ExternalReportNumber = requestModelData.ExternalReportNumber;

                requestEntity.ReportType = requestModelData.ReportType;
                requestEntity.Origin = requestModelData.Origin;
                requestEntity.ShipMode = requestModelData.ShipMode;
                requestEntity.Othercategory = requestModelData.OtherCategory;
                requestEntity.Market = requestModelData.Market;
                requestEntity.TotalScore = requestModelData.Score;
                requestEntity.Grade = requestModelData.Grade;
                requestEntity.LastAuditScore = requestModelData.LastAuditScore;

                _fbRepo.RemoveEntities(objRemovedList);
                _fbRepo.RemoveEntities(requestEntity.FbReportProductBarcodesInfos);
                _fbRepo.EditEntity(requestEntity);
                await _fbRepo.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task AutoUpdateReportCustomerDecision(FbReportDetail reportData)
        {
            var reportCustomerDecisionComments = await _fbRepo.GetListAsync<CuReportCustomerDecisionComment>(x => x.Active && x.CustomerId == reportData.Inspection.CustomerId);
            var reportInspSummaries = reportData.FbReportInspSummaries.Where(x => x.Active.Value).ToList();
            if (reportCustomerDecisionComments.Any() && reportInspSummaries.Any())
            {
                int customerResultId = 0;
                string comments = null;

                if (reportInspSummaries.Any(x => x.ResultId == (int)FBReportResult.Fail))
                {
                    var reportInspSummary = reportInspSummaries.FirstOrDefault(x => x.ResultId == (int)FBReportResult.Fail);
                    customerResultId = (int)FBReportResult.Fail;
                    comments = reportCustomerDecisionComments.FirstOrDefault(x => x.ReportResult.ToLower() == reportInspSummary.Name.ToLower())?.Comments;
                }
                else if (reportInspSummaries.Any(x => x.ResultId == (int)FBReportResult.Pending))
                {
                    customerResultId = (int)FBReportResult.Pending;
                    comments = reportCustomerDecisionComments.FirstOrDefault(x => x.ReportResult.ToLower() == FBReportResult.Pending.ToString().ToLower())?.Comments;
                }

                if (customerResultId > 0)
                {
                    var inspRepCusDecision = new InspRepCusDecision
                    {
                        Comments = comments,
                        ReportId = reportData.Id,
                        CustomerResultId = customerResultId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        IsAutoCustomerDecision = true
                    };

                    reportData.InspRepCusDecisions.Add(inspRepCusDecision);
                    _fbRepo.AddEntity(inspRepCusDecision);
                }
            }
        }

        /// <summary>
        /// delete fb report details
        /// </summary>
        /// <param name="requestEntity"></param>
        private async Task DeleteReportDetailsFromDB(FbReportDetail requestEntity)
        {
            try
            {
                await _fbRepo.RemoveInspectionFbReport(requestEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete fb report details
        /// </summary>
        /// <param name="requestEntity"></param>
        private async Task DeleteFbReportDetails(FbReportDetail requestEntity)
        {
            try
            {
                foreach (var inspectionSummary in requestEntity.FbReportInspSummaries)
                {

                    foreach (var inspectionSummaryPhoto in inspectionSummary.FbReportInspSummaryPhotos)
                    {
                        inspectionSummaryPhoto.DeletedOn = DateTime.Now;
                        inspectionSummaryPhoto.Active = false;
                    }

                    inspectionSummary.Active = false;
                    inspectionSummary.DeletedOn = DateTime.Now;
                }

                foreach (var inspectionQuantity in requestEntity.FbReportQuantityDetails)
                {
                    inspectionQuantity.Active = false;
                    inspectionQuantity.DeletedOn = DateTime.Now;
                }

                foreach (var reportDefects in requestEntity.FbReportInspDefects)
                {
                    reportDefects.Active = false;
                    reportDefects.DeletedOn = DateTime.Now;
                }

                foreach (var qcInfo in requestEntity.FbReportQcdetails)
                {
                    qcInfo.Active = false;
                    qcInfo.DeletedOn = DateTime.Now;
                }
                requestEntity.FbReportMapId = null;
                requestEntity.Active = false;
                requestEntity.DeletedBy = _ApplicationContext.UserId;
                requestEntity.DeletedOn = DateTime.Now;

                _fbRepo.EditEntity(requestEntity);
                await _fbRepo.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Send Notification and Email to booking Report checker
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task SendNotificationAndEmailToReportChecker(InspProductTransaction entity, FbReportDetail reportData, InspContainerTransaction containerData)
        {
            List<string> recepientList = new List<string>();
            List<int> recepientStaffList = new List<int>();
            StringBuilder strBody = new StringBuilder();
            int? missionId = null;
            int inspectionId = 0;
            List<HrStaff> recepients = null;

            //get the inspection id
            if (entity != null)
                inspectionId = entity.InspectionId;
            else if (containerData != null)
                inspectionId = containerData.InspectionId;

            //get the recipient list
            if (entity != null && entity.Inspection != null && entity.Inspection.SchScheduleCS != null)
                recepients = entity.Inspection.SchScheduleCS.Where(x => x.Active).Select(x => x.Cs).ToList();
            else if (containerData != null && containerData.Inspection != null && containerData.Inspection.SchScheduleCS != null)
                recepients = containerData.Inspection.SchScheduleCS.Where(x => x.Active).Select(x => x.Cs).ToList();

            //get the mission id
            if (entity != null && entity.Inspection != null)
                missionId = entity.Inspection.FbMissionId;
            else if (containerData != null && containerData.Inspection != null)
                missionId = containerData.Inspection.FbMissionId;


            if (recepients != null)
            {
                if (recepients.Count > 1)
                {
                    strBody.Append("<div>Dear All,</div> <br/><br/>");
                }
                else
                {
                    strBody.Append("<div>Dear " + recepients?.FirstOrDefault()?.PersonName + ",</div> <br/><br/>");
                }
                foreach (var item in recepients)
                {
                    if (!string.IsNullOrEmpty(item.CompanyEmail))

                        recepientList.Add(item.CompanyEmail);

                    if (item.ItUserMasters.Any() && item.ItUserMasters.FirstOrDefault() != null)
                    {
                        recepientStaffList.Add(item.ItUserMasters.FirstOrDefault().Id);
                    }
                }
            }

            //get the emailuser verification url
            var emailUserVerficiationUrl = _Configuration["EmailUserVerificationEmail"].ToString();

            var missionUrl = string.Format("{0}?missionId={1}&callFrom={2}", emailUserVerficiationUrl, missionId, (int)UserEmailVerification.ReportFilledToReportChecker);

            var reportUrl = string.Format("{0}?reportId={1}&callFrom={2}", emailUserVerficiationUrl, reportData?.FbReportMapId, (int)UserEmailVerification.ReportFilledToReportChecker);

            strBody.Append("<div>The Inspection Report <a href=" + reportUrl + ">" + reportData?.ReportTitle + "</a> filled by QC for booking " +
                "                               <a href=" + missionUrl + ">#" + inspectionId + "</a>." +
                "please do the necessary action</div><br/><br/>");

            strBody.Append("<div>Thanks</div>");
            //strBody.Append("<div>API-Team</div><br/>");
            strBody.Append("<span style='fon-style:normal'>Note: - This receipt is auto generated from Link QMS. Please do not reply to this message. </span>");

            var emailRequest = new EmailRequest
            {
                Id = Guid.NewGuid(),
                Recepients = recepientList,
                Body = strBody.ToString(),
                CCList = null,
                Subject = $"Inspection Report filled by QC for booking #" + inspectionId + " ",
                ReturnToUpdate = (id, state, error) => { }
            };

            await _notificationManger.AddNotification(NotificationType.BookingReportValidated, inspectionId, recepientStaffList);
            _emailManager.SendEmail(emailRequest, _tenantProvider.GetCompanyId());
        }
        /// <summary>
        /// Set Booking Report status 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void SetFillingAndReviewStatus(FbReportDetail entity, FbStatusRequest request)
        {
            int? fillingStatus = null;
            int? reviewStatus = null;
            int? reportStatus = null;

            if (!string.IsNullOrEmpty(request.FillingStatus))
            {
                if (request.FillingStatus.ToLower() == "not_started")
                {
                    fillingStatus = (int)FBStatus.ReportFillingNotstarted;
                }
                if (request.FillingStatus.ToLower() == "in_progress")
                {
                    fillingStatus = (int)FBStatus.ReportFillingInprogress;
                }
                if (request.FillingStatus.ToLower() == "validated")
                {
                    fillingStatus = (int)FBStatus.ReportFillingValidated;
                }
            }

            if (!string.IsNullOrEmpty(request.ReviewStatus))
            {
                if (request.ReviewStatus.ToLower() == "not_started")
                {
                    reviewStatus = (int)FBStatus.ReportReviewNotStarted;
                }
                if (request.ReviewStatus.ToLower() == "in_progress")
                {
                    reviewStatus = (int)FBStatus.ReportReviewInprogress;
                }
                if (request.ReviewStatus.ToLower() == "validated")
                {
                    reviewStatus = (int)FBStatus.ReportReviewValidated;
                }
            }

            if (!string.IsNullOrEmpty(request.ReportStatus))
            {
                entity.FbReportStatus = GetReportStatusId(request);
            }

            entity.FbFillingStatus = fillingStatus;
            entity.FbReviewStatus = reviewStatus;
        }

        /// <summary>
        /// get Report status ids
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private int? GetReportStatusId(FbStatusRequest request)
        {

            int? reportStatus = null;

            if (!string.IsNullOrEmpty(request.ReportStatus))
            {
                if (request.ReportStatus.ToLower() == "draft")
                {
                    reportStatus = (int)FBStatus.ReportDraft;
                }
                if (request.ReportStatus.ToLower() == "archive")
                {
                    reportStatus = (int)FBStatus.ReportArchive;
                }
                if (request.ReportStatus.ToLower() == "validated")
                {
                    reportStatus = (int)FBStatus.ReportValidated;
                }
                if (request.ReportStatus.ToLower() == "invalidated")
                {
                    reportStatus = (int)FBStatus.ReportInValidated;
                }
            }

            return reportStatus;
        }

        /// <summary>
        /// get report Ids booking 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<ReportIdData>> getReportIdsbyBooking(int bookingId)
        {
            return await _fbRepo.getReportIdsbyBooking(bookingId);
        }

        /// <summary>
        /// get report ids by booking service dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<ReportIdData>> getReportIdsbyBookingServiceDates(DateTime startDate, DateTime endDate)
        {
            return await _fbRepo.getReportIdsbyBookingServiceDates(startDate, endDate);
        }

        public async Task<List<ReportIdData>> getNonValidatedReportIdsbyBooking(int bookingId)
        {
            return await _fbRepo.getNonValidatedReportIdsbyBooking(bookingId);
        }

        /// <summary>
        /// get api report id by fb reports
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <returns></returns>
        public async Task<int> GetApiReportIdbyFbReport(int fbReportId)
        {
            return await _fbRepo.GetApiReportIdbyFbReport(fbReportId);
        }

        private async Task SaveReportFastTransaction(ReportFastTransactionRequest input, int customerId, int? inspectionId)
        {
            var customerCheckPoints = await _customerCheckPointRepository.GetCusCPByCustomerId(customerId);
            if (inspectionId.HasValue && customerCheckPoints != null && customerCheckPoints.Any(x => x.CheckPointId == (int)CheckPointTypeEnum.NewReportFormat))
            {
                var reportFastTransaction = new RepFastTransaction()
                {
                    BookingId = input.BookingId,
                    ItNotification = false,
                    CreatedOn = DateTime.Now,
                    ReportId = input.ReportId,
                    Status = (int)ReportFastStatus.NotStarted,
                    TryCount = 0
                };

                var repFastTranLog = new RepFastTranLog()
                {
                    CreatedOn = DateTime.Now,
                    ReportId = input.ReportId,
                    Status = (int)ReportFastStatus.NotStarted,
                };
                reportFastTransaction.RepFastTranLogs.Add(repFastTranLog);
                _reportFastTransactionRepository.AddEntity(repFastTranLog);

                _reportFastTransactionRepository.AddEntity(reportFastTransaction);
                await _reportFastTransactionRepository.Save();
            }


        }

        /// <summary>
        /// get fb report title list by report id list
        /// </summary>
        /// <param name="fbReportMapIdList"></param>
        /// <returns></returns>
        public async Task<List<FBReportDetails>> GetFbReportTitleListByReportIds(IEnumerable<int?> fbReportMapIdList)
        {
            return await _fbRepo.GetFbReportTitleListByReportIds(fbReportMapIdList);
        }

        /// <summary>
        /// update IsReportReviewCs to schedule cs table
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        /// <returns></returns>
        private async Task ScheduleCsUpdate(int bookingId)
        {
            var fbUserIdList = await _humanresourcemanager.GetFBCSIds(bookingId);

            //get qcid fb_userid and staff id here as list from it user master
            var staffIdList = await _fbRepo.GetFBReportStaffList(fbUserIdList);

            //schedule have to take - qcid and inspection id
            var scheduleCSRepo = await _fbRepo.GetCSDetailList(bookingId, staffIdList);

            foreach (var scheduleItem in scheduleCSRepo)
            {
                scheduleItem.IsReportReviewCs = true;
                scheduleItem.ModifiedBy = _ApplicationContext.UserId != 0 ? _ApplicationContext.UserId : adminUserFromAppSettings;
                scheduleItem.ModifiedOn = DateTime.Now;
            }

            if (scheduleCSRepo.Any())
            {
                _fbRepo.EditEntities(scheduleCSRepo);
            }
        }

        /// <summary>
        /// update IsReportFilledQc to schedule qc table
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reportData"></param>
        private async Task ScheduleQcUpdate(int bookingId)
        {
            var fbUserIdList = await _humanresourcemanager.GetFBQCIdList(bookingId);

            //get qcid fb_userid and staff id here as list from it user master
            var staffIdList = await _fbRepo.GetFBReportStaffList(fbUserIdList);

            //schedule have to take - qcid and inspection id
            var scheduleQCRepo = await _fbRepo.GetQCDetailList(bookingId, staffIdList);

            foreach (var scheduleItem in scheduleQCRepo)
            {
                scheduleItem.IsReportFilledQc = true;
                scheduleItem.ModifiedBy = _ApplicationContext.UserId != 0 ? _ApplicationContext.UserId : adminUserFromAppSettings;
                scheduleItem.ModifiedOn = DateTime.Now;
            }

            //update the scheduleQC data
            if (scheduleQCRepo.Any())
            {
                _fbRepo.EditEntities(scheduleQCRepo);
            }
        }

        private async Task UpdateAuditReportDetails(AudTransaction requestAuditEntity, FbReportDataRequest requestModelData)
        {
            try
            {
                foreach (var requestEntity in requestAuditEntity.AudFbReportCheckpoints.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    requestEntity.Active = false;
                    requestEntity.DeletedOn = DateTime.Now;
                }
                requestAuditEntity.FinalReportPath = requestModelData.FinalReportPath;
                requestAuditEntity.PictureReportPath = requestModelData.PicturesReportPath;

                requestAuditEntity.FbmissionTitle = requestModelData.MissionTitle;
                requestAuditEntity.FbreportTitle = requestModelData.ReportTitle;

                // if audit status is audited and fb report status invalidated then we are change audit status to scheduled
                if (requestAuditEntity.StatusId == (int)AuditStatus.Audited && requestAuditEntity.FbreportStatus == (int)FBStatus.ReportInValidated)
                    requestAuditEntity.StatusId = (int)AuditStatus.Scheduled;
                //if the audit status is scheduled and fb report status is validated then we are change the audit status to audited.
                else if (requestAuditEntity.FbreportStatus == (int)FBStatus.ReportValidated && requestAuditEntity.StatusId == (int)AuditStatus.Scheduled)
                    requestAuditEntity.StatusId = (int)AuditStatus.Audited;

                requestAuditEntity.ExternalReportNo = requestModelData.ExternalReportNumber;
                // if the dates PROPERLY CONVERTED THEN SEND TO DB
                if (DateTime.TryParseExact(requestModelData.AuditStartedDate, StandardISO8601DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime auditStartDate))
                {
                    TimeSpan.TryParseExact(requestModelData.AuditStartedTime, StandardTimeFormart, CultureInfo.InvariantCulture, out TimeSpan auditStartTime);
                    requestAuditEntity.AuditStartTime = auditStartDate.Date.Add(auditStartTime).ToString(StandardDateTimeFormat1);
                }

                if (DateTime.TryParseExact(requestModelData.AuditSubmittedDate, StandardISO8601DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime auditEndDate))
                {
                    TimeSpan.TryParseExact(requestModelData.AuditSubmittedTime, StandardTimeFormart, CultureInfo.InvariantCulture, out TimeSpan auditEndTime);
                    requestAuditEntity.AuditEndTime = auditEndDate.Date.Add(auditEndTime).ToString(StandardDateTimeFormat1);
                }

                requestAuditEntity.MainCategory = requestModelData.MainCategory;
                requestAuditEntity.OtherCategory = requestModelData.OtherCategory;
                requestAuditEntity.Market = requestModelData.Market;
                requestAuditEntity.FillingValidatedFirstTime = requestModelData.FillingValidatedFirstTime;
                requestAuditEntity.ReviewValidatedFirstTime = requestModelData.ReviewValidatedFirstTime;
                requestAuditEntity.LastAuditScore = requestModelData.LastAuditScore;

                if (requestModelData.Audit != null)
                {
                    requestAuditEntity.ScoreValue = requestModelData.Audit.ScoreValue;
                    requestAuditEntity.Scorepercentage = requestModelData.Audit.Scorepercentage;
                    requestAuditEntity.Grade = requestModelData.Audit.Grade;
                }
                if (requestModelData.Evaluation != null && requestModelData.Evaluation.Any())
                {
                    foreach (var evaluationRequest in requestModelData.Evaluation)
                    {
                        var evaluationdata = new AudFbReportCheckpoint
                        {
                            Active = true,
                            ChekPointName = evaluationRequest.Title,
                            ScoreValue = evaluationRequest.ScoreValue,
                            ScorePercentage = evaluationRequest.ScorePercentage,
                            MaxPoint = evaluationRequest.MaxPoints,
                            Major = evaluationRequest.Major,
                            Minor = evaluationRequest.Minor,
                            ZeroTolerance = evaluationRequest.ZeroTolerance,
                            Remarks = evaluationRequest.Remarks,
                            Critical = evaluationRequest.Critical,
                            Grade = evaluationRequest.Grade,
                            CreatedOn = DateTime.Now
                        };
                        requestAuditEntity.AudFbReportCheckpoints.Add(evaluationdata);
                        _fbRepo.AddEntity(evaluationdata);
                    }
                }

                if (requestAuditEntity.StatusId == (int)AuditStatus.Audited)
                {
                    if (requestModelData.QcList != null)
                        await ScheduleAuditorUpdate(requestAuditEntity.Id, requestModelData.QcList.Select(x => x.GetValueOrDefault()).ToList());
                    if (requestModelData.ReviewerList != null)
                        await AuditCSUpdate(requestAuditEntity.Id, requestModelData.ReviewerList.Select(x => x.GetValueOrDefault()).ToList());
                }

                _fbRepo.EditEntity(requestAuditEntity);
                await _fbRepo.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task UpdateAuditReportFillingAndReviewStatus(FbStatusRequest request, AudTransaction auditData)
        {
            int? fillingStatus = null;
            int? reviewStatus = null;
            int? reportStatus = null;

            if (!string.IsNullOrEmpty(request.FillingStatus))
            {
                if (request.FillingStatus.ToLower() == "not_started")
                {
                    fillingStatus = (int)FBStatus.ReportFillingNotstarted;
                }
                if (request.FillingStatus.ToLower() == "in_progress")
                {
                    fillingStatus = (int)FBStatus.ReportFillingInprogress;
                }
                if (request.FillingStatus.ToLower() == "validated")
                {
                    fillingStatus = (int)FBStatus.ReportFillingValidated;
                }
            }

            if (!string.IsNullOrEmpty(request.ReviewStatus))
            {
                if (request.ReviewStatus.ToLower() == "not_started")
                {
                    reviewStatus = (int)FBStatus.ReportReviewNotStarted;
                }
                if (request.ReviewStatus.ToLower() == "in_progress")
                {
                    reviewStatus = (int)FBStatus.ReportReviewInprogress;
                }
                if (request.ReviewStatus.ToLower() == "validated")
                {
                    reviewStatus = (int)FBStatus.ReportReviewValidated;
                }
            }

            if (!string.IsNullOrEmpty(request.ReportStatus))
            {
                reportStatus = GetReportStatusId(request);
            }

            auditData.FbreportStatus = reportStatus;
            auditData.FbfillingStatus = fillingStatus;
            auditData.FillingStatus = fillingStatus;
            auditData.FbreviewStatus = reviewStatus;
            _fbRepo.EditEntity(auditData);
            await _fbRepo.Save();
        }
        private async Task AuditSendNotificationAndEmailToReportChecker(AudTransaction entity)
        {
            List<string> recepientList = new List<string>();
            List<int> recepientStaffList = new List<int>();
            StringBuilder strBody = new StringBuilder();

            //get the auditId
            int auditId = entity.Id;

            //get the mission id
            int? missionId = entity.FbmissionId;

            var recepients = await _fbRepo.AudTranCSForAudit(auditId);

            if (recepients != null && recepients.Any())
            {
                if (recepients.Count > 1)
                {
                    strBody.Append("<div>Dear All,</div> <br/><br/>");
                }
                else
                {
                    strBody.Append("<div>Dear " + recepients?.FirstOrDefault()?.PersonName + ",</div> <br/><br/>");
                }
                foreach (var item in recepients)
                {
                    if (!string.IsNullOrEmpty(item.CompanyEmail))

                        recepientList.Add(item.CompanyEmail);

                    if (item.ItUserMasters.Any() && item.ItUserMasters.FirstOrDefault() != null)
                    {
                        recepientStaffList.Add(item.ItUserMasters.FirstOrDefault().Id);
                    }
                }

            }

            //get the emailuser verification url
            var emailUserVerficiationUrl = _Configuration["EmailUserVerificationEmail"].ToString();

            var missionUrl = string.Format("{0}?missionId={1}&callFrom={2}", emailUserVerficiationUrl, missionId, (int)UserEmailVerification.ReportFilledToReportChecker);

            var reportUrl = string.Format("{0}?reportId={1}&callFrom={2}", emailUserVerficiationUrl, entity.FbreportId, (int)UserEmailVerification.ReportFilledToReportChecker);

            strBody.Append("<div>The Audit Report <a href=" + reportUrl + ">" + entity.FbreportTitle + "</a> filled by QC for booking " +
                "                               <a href=" + missionUrl + ">#" + auditId + "</a>." +
                "please do the necessary action</div><br/><br/>");

            strBody.Append("<div>Thanks</div>");
            strBody.Append("<span style='fon-style:normal'>Note: - This receipt is auto generated from Link QMS. Please do not reply to this message. </span>");

            var emailRequest = new EmailRequest
            {
                Id = Guid.NewGuid(),
                Recepients = recepientList,
                Body = strBody.ToString(),
                CCList = null,
                Subject = $"Audit Report filled by QC for booking #" + auditId + " ",
                ReturnToUpdate = (id, state, error) => { }
            };

            await _notificationManger.AddNotification(NotificationType.BookingReportValidated, auditId, recepientStaffList);
            _emailManager.SendEmail(emailRequest, _tenantProvider.GetCompanyId());
        }

        private async Task ScheduleAuditorUpdate(int auditId, List<int> fbAuditorIdList)
        {

            //get qcid fb_userid and staff id here as list from it user master
            var staffIdList = await _fbRepo.GetFBReportStaffList(fbAuditorIdList);

            //schedule have to take - qcid and inspection id
            var auditors = await _fbRepo.GetAuditAuditorList(auditId, staffIdList);

            foreach (var auditor in auditors)
            {
                auditor.IsAudited = true;
            }

            //update the scheduleQC data
            if (auditors.Any())
            {
                _fbRepo.EditEntities(auditors);
            }
        }

        private async Task AuditCSUpdate(int auditId, List<int> fbAuditCSIdList)
        {

            //get audit cs fb_userid and staff id here as list from it user master
            var staffIdList = await _fbRepo.GetFBReportStaffList(fbAuditCSIdList);

            //schedule have to take - qcid and inspection id
            var auditCSList = await _fbRepo.GetAuditCSList(auditId, staffIdList);

            foreach (var auditor in auditCSList)
            {
                auditor.IsReport = true;
            }

            //update the scheduleQC data
            if (auditCSList.Any())
            {
                _fbRepo.EditEntities(auditCSList);
            }
        }
    }
}
