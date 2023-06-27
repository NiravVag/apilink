using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.Inspection;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;
using BookingDetail = DTO.Dashboard.BookingDetail;

namespace BI
{
    public class CSDashboardManager : ApiCommonData, ICSDashboardManager
    {
        private readonly ICSDashboardRepository _csDashboardRepo = null;
        private readonly IInspectionBookingManager _inspBookingManager = null;
        private readonly IManagementDashboardManager _managementDashboardManager = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly ITenantProvider _tenant = null;
        public CSDashboardManager(ICSDashboardRepository csDashboardRepo, IInspectionBookingManager inspBookingManager,
            IManagementDashboardManager managementDashboardManager, IAPIUserContext applicationContext, ITenantProvider tenant)
        {
            _csDashboardRepo = csDashboardRepo;
            _inspBookingManager = inspBookingManager;
            _managementDashboardManager = managementDashboardManager;
            _applicationContext = applicationContext;
            _tenant = tenant;
        }

        /// <summary>
        /// get the bookings details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<BookingDetail>> CommonInspSearch(CSDashboardModelRequest request)
        {
            //get all bookings by filters
            var bookingdata = await _csDashboardRepo.GetBookingDetail(request, _applicationContext);

            return bookingdata;
        }

        /// <summary>
        /// get the new count of booking related details and customer supplier factory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetNewBookingRelatedCountResponse> GetCountNewBookingRelatedDetails(CSDashboardModelRequest request)
        {
            //convert the request object to DB request
            CSDashboardFilterModel objCSDashboardFilterModel = new CSDashboardFilterModel()
            {
                FromDate = request.ServiceDateFrom.ToDateTime(),
                ToDate = request.ServiceDateTo.ToDateTime(),
                EntityId = _tenant.GetCompanyId()
            };

            var newCountData = await _csDashboardRepo.GetCountNewDetails(objCSDashboardFilterModel);

            return new GetNewBookingRelatedCountResponse() { Result = CSDashboardResult.Success, CSDashboardCountItem = newCountData.FirstOrDefault() };
        }

        /// <summary>
        /// get service type list by filter with reequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CSDashboardserviceTypeResponse> GetServiceTypeList(CSDashboardModelRequest request)
        {           
            CSDashboardserviceTypeResponse response = new CSDashboardserviceTypeResponse();
            var bookingList = await CommonInspSearch(request);

            if (!bookingList.Any())
                return new CSDashboardserviceTypeResponse() { Result = CSDashboardResult.NoFound };


            //convert the request object to DB request
            CSDashboardDBRequest objCSDashboardDBRequest = new CSDashboardDBRequest()
            {
                EntityId = _tenant.GetCompanyId(),
                BookingIdList = bookingList != null && bookingList.Any() ? bookingList.ConvertAll(x => new CommonId { Id = x.InspectionId }) : null,
            };

            var res = await _csDashboardRepo.GetServiceTypeList(objCSDashboardDBRequest);

            if (res == null || !res.Any())
            {
                return new CSDashboardserviceTypeResponse { Result = CSDashboardResult.NoFound };
            }

            response.Data = res;


            response.Result = CSDashboardResult.Success;

            return response;
        }

        /// <summary>
        /// get man day count by office list by filters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CSDashboardMandayByOfficeResponse> GetMandayByOfficeList(CSDashboardModelRequest request)
        {
            CSDashboardMandayByOfficeResponse response = new CSDashboardMandayByOfficeResponse();

            //apply top filters
            var bookingList = await CommonInspSearch(request);

            if (!bookingList.Any())
                return new CSDashboardMandayByOfficeResponse() { Result = CSDashboardResult.NoFound };

            //convert the request object to DB request
            CSDashboardDBRequest objCSDashboardDBRequest = new CSDashboardDBRequest()
            {
                EntityId = _tenant.GetCompanyId(),
                BookingIdList = bookingList != null && bookingList.Any() ? bookingList.ConvertAll(x => new CommonId { Id = x.InspectionId }) : null,
            };

            var res = await _csDashboardRepo.GetMandayByOfficeList(objCSDashboardDBRequest);

            if (res == null || !res.Any())
            {
                return new CSDashboardMandayByOfficeResponse { Result = CSDashboardResult.NoFound };
            }

            response.Data = res;


            response.Result = CSDashboardResult.Success;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DayFBReportCountResponse> GetReportCountByDayList(CSDashboardModelRequest request)
        {
            DayFBReportCountResponse response = new DayFBReportCountResponse();

            //apply top filters
            var bookingList = await CommonInspSearch(request);

            if (!bookingList.Any())
                return new DayFBReportCountResponse() { Result = CSDashboardResult.NoFound };

            //convert the request object to DB request
            CSDashboardDBRequest objCSDashboardDBRequest = new CSDashboardDBRequest()
            {
                EntityId = _tenant.GetCompanyId(),
                BookingIdList = bookingList != null && bookingList.Any() ? bookingList.ConvertAll(x => new CommonId { Id = x.InspectionId }) : null,
            };

            var res = await _csDashboardRepo.GetDayFBReportCountList(objCSDashboardDBRequest);

            if (res == null || !res.Any())
            {
                return new DayFBReportCountResponse { Result = CSDashboardResult.NoFound };
            }

            var _dateList = Enumerable.Range(0, 1 + request.ServiceDateTo.ToDateTime().Subtract(request.ServiceDateFrom.ToDateTime()).Days)
                              .Select(offset => request.ServiceDateFrom.ToDateTime().AddDays(offset)).ToList();

            response.Data = new List<DayFBReportCount>();

            for (int i = 0; i < _dateList.Count; i++)
            {
                
                response.Data.Add(new DayFBReportCount
                {
                    Date = _dateList[i].ToString(StandardDateFormat),
                    Count = res.Where(x => x.ServiceToDate == _dateList[i]).Select(x => x.FbReportCount).FirstOrDefault(),
                });
            }

            response.Result = CSDashboardResult.Success;

            return response;
        }

        /// <summary>
        /// assign booking status count with task count
        /// </summary>
        /// <param name="statusList"></param>
        /// <returns></returns>
        private List<StatusTaskCountItem> BookingStatusTaskCount(List<StatusTaskCountItemRepo> statusList)
        {
            return statusList.Where(X => X.ActionType == (int)CSDashboardStatusCount.BookingStatusList).Select(x => new StatusTaskCountItem
            {
                StatusCount = x.Count,
                StatusName = x.Name,
                TaskCount = statusList.Where(z => z.ActionType == (int)CSDashboardStatusCount.BookingTaskList
                                            && ((z.Id == (int)TaskType.VerifyInspection && x.Id == (int)BookingStatus.Verified) ||
                                            (z.Id == (int)TaskType.ConfirmInspection && x.Id == (int)BookingStatus.Confirmed))).
                                          Select(z => z.Count).FirstOrDefault(),
                TaskName = x.Name
            }).ToList();
        }

        /// <summary>
        /// assign quotation status count with task count
        /// </summary>
        /// <param name="statusList"></param>
        /// <returns></returns>
        private List<StatusTaskCountItem> QuotationStatusTaskCount(List<StatusTaskCountItemRepo> statusList)
        {
            var _quotationStatusCount = new List<StatusTaskCountItem>();
            
            var quotationCreateTaskCount = statusList.Count(x =>
            x.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            x.Id == (int)TaskType.QuotationModify);


            var quotationCreateStatus = statusList.Where(x => x.ActionType == (int)CSDashboardStatusCount.QuotationStatusList && x.Id
                                             == (int)QuotationStatus.QuotationCreated)
            .Select(x => new StatusTaskCountItem
            {
                StatusCount = x.Count,
                StatusName = x.Name,
                TaskCount = quotationCreateTaskCount,
                Id = x.Id,
                TaskName = CSDashboard_Quotation_Modify
            }).FirstOrDefault();

            //push quotation create task and status data
            _quotationStatusCount.Add(quotationCreateStatus);

            var quotationVerifyStatus = statusList.Where(x => x.ActionType == (int)CSDashboardStatusCount.QuotationStatusList && x.Id
                                           == (int)QuotationStatus.QuotationVerified)
          .Select(x => new StatusTaskCountItem
          {
              StatusCount = x.Count,
              StatusName = x.Name,
              TaskCount = 0,
              Id = x.Id,
              TaskName = CSDashboard_Quotation_Modify
          }).FirstOrDefault();

            //push quotation verify task and status data
            _quotationStatusCount.Add(quotationVerifyStatus);

            //quotation status count
            _quotationStatusCount.AddRange(statusList.Where(x => x.ActionType == (int)CSDashboardStatusCount.QuotationStatusList && x.Id
                                             != (int)QuotationStatus.QuotationCreated && x.Id != (int)QuotationStatus.QuotationVerified)
            .Select(x => new StatusTaskCountItem
            {
                StatusCount = x.Count,
                StatusName = x.Name,
                TaskCount = statusList.Where(z => (z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            z.Id == (int)TaskType.QuotationPending && x.Id == (int)QuotationStatus.Pending) ||

                            (z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            z.Id == (int)TaskType.QuotationToApprove &&
                            x.Id == (int)QuotationStatus.ManagerApproved) ||

                            (z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            (z.Id == (int)TaskType.QuotationSent || z.Id == (int)TaskType.QuotationCustomerReject) &&
                            x.Id == (int)QuotationStatus.SentToClient) ||

                            (z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList && 
                            z.Id == (int)TaskType.QuotationCustomerConfirmed &&
                            x.Id == (int)QuotationStatus.CustomerValidated)
                            
                            // || (z.Id == (int)TaskType.QuotationModify && x.Name == QuotationStatus.AERejected.ToString() 
                            //|| x.Name == QuotationStatus.ManagerRejected.ToString()) 
                            )
                            .Select(z => z.Count).FirstOrDefault(),
                Id = x.Id,

                TaskName = statusList.Any(z => z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            z.Id == (int)TaskType.QuotationPending && x.Id == (int)QuotationStatus.Pending) ?
                            CSDashboard_Created :
                            statusList.Any(z =>
                            z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            z.Id == (int)TaskType.QuotationToApprove &&
                            x.Id == (int)QuotationStatus.ManagerApproved) ? CSDashboard_Quotation_Approved :

                            statusList.Any(z => z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            (z.Id == (int)TaskType.QuotationSent || z.Id == (int)TaskType.QuotationCustomerReject) &&

                            x.Id == (int)QuotationStatus.SentToClient) ? CSDashboard_Quotation_Sent :

                            statusList.Any(z =>
                            z.ActionType == (int)CSDashboardStatusCount.QuotationTaskList &&
                            z.Id == (int)TaskType.QuotationCustomerConfirmed &&
                            x.Id == (int)QuotationStatus.CustomerValidated)
                            ? CSDashboard_Customer_Confirmed : ""

            }).ToList());

            return _quotationStatusCount;
        }

        /// <summary>
        /// assign allocation status count with task count
        /// </summary>
        /// <param name="statusList"></param>
        /// <returns></returns>
        private List<StatusTaskCountItem> AllocationStatusTaskCount(List<StatusTaskCountItemRepo> statusList)
        {
            return statusList.Where(x => x.ActionType == (int)CSDashboardStatusCount.AllocationStatusList).Select(x => new StatusTaskCountItem
            {
                StatusCount = x.Count,
                StatusName = x.Name == BookingStatus.Confirmed.ToString() ? CSDashboard_Allocation_Pending : x.Name,
                TaskCount = statusList.Where(z => z.ActionType == (int)CSDashboardStatusCount.AllocationTaskList
                                            && z.Id == (int)TaskType.ScheduleInspection && x.Id == (int)BookingStatus.Confirmed).Select(z => z.Count)
                                            .FirstOrDefault(),
                TaskName = x.Name == BookingStatus.Confirmed.ToString() ? CSDashboard_Allocate : x.Name,
            }).ToList();
        }

        /// <summary>
        /// assign report status count with task count
        /// </summary>
        /// <param name="statusList"></param>
        /// <returns></returns>
        private List<StatusTaskCountItem> ReportStatusTaskCount(List<StatusTaskCountItemRepo> statusList)
        {
            return statusList.Where(z => z.ActionType == (int)CSDashboardStatusCount.ReportStatusList).Select(x => new StatusTaskCountItem
            {
                StatusCount = x.Count,
                StatusName = x.Name,
                TaskCount = statusList.Where(z => z.ActionType == (int)CSDashboardStatusCount.ReportTaskList && x.Id == (int)FBStatus.ReportReviewNotStarted)
                .Select(z => z.Count).FirstOrDefault(),
                TaskName = x.Id == (int)FBStatus.ReportReviewNotStarted ? CSDashboard_CS_Validate : x.Name
            }).ToList();
        }

        /// <summary>
        /// booking,quotation,report, allocation status count by logged user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<StatusListCountResponse> GetStatusByLoggedUserList(CSDashboardModelRequest request)
        {
            StatusListCountResponse response = new StatusListCountResponse();

            request.StatusIdList = BookingStatusList;

            //apply top filters
            var bookingList = await CommonInspSearch(request);

            if (!bookingList.Any())
                return new StatusListCountResponse() { Result = CSDashboardResult.NoFound };

            //convert the request object to DB request
            CSDashboardStatusDBRequest objCSDashboardDBRequest = new CSDashboardStatusDBRequest()
            {
                EntityId = _tenant.GetCompanyId(),
                BookingIdList = bookingList != null && bookingList.Any() ? bookingList.ConvertAll(x => new CommonId { Id = x.InspectionId }) : null,
                UserId = _applicationContext.UserId,
                RoleIdList = _applicationContext.RoleList != null && _applicationContext.RoleList.Any() ? 
                                            _applicationContext.RoleList.ToList().ConvertAll(x => new CommonId { Id = x }) : null,
            };

            //get status(booking, quotation, allocation, report) count as list
            var res = await _csDashboardRepo.GetStatusTaskCountList(objCSDashboardDBRequest);

            if (res == null && !res.Any())
            {
                return new StatusListCountResponse { Result = CSDashboardResult.NoFound };
            }

            response.BookingStatusCount = BookingStatusTaskCount(res);

            response.QuotationStatusCount = QuotationStatusTaskCount(res);

            response.AllocationStatusCount = AllocationStatusTaskCount(res);

            response.ReportStatusCount = ReportStatusTaskCount(res);

            response.Result = CSDashboardResult.Success;

            return response;
        }
    }
}
