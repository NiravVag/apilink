using BI.Maps;
using BI.Maps.APP;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.EmailSendingDetails;
using DTO.Inspection;
using DTO.InspectionCertificate;
using DTO.InspectionCustomerDecision;
using DTO.Kpi;
using DTO.MobileApp;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class InspectionCustomerDecisionManager : ApiCommonData, IInspectionCustomerDecisionManager
    {
        private readonly IInspectionCustomerDecisionRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ISharedFullBridgeManager _sharedFbManager = null;
        private readonly ISharedInspectionManager _sharedManager = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly ICustomerCheckPointRepository _customerCheckPointRepository = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IHelper _helper = null;
        private readonly IKpiCustomRepository _kpiRepo = null;
        private readonly CustomerDecisionMap _cusdecmap = null;
        private readonly IDashboardRepository _dashboardRepo = null;

        public InspectionCustomerDecisionManager(IInspectionCustomerDecisionRepository repo, IAPIUserContext ApplicationContext,
            ISharedFullBridgeManager sharedFbManager, ISharedInspectionManager sharedManager, IInspectionBookingRepository inspRepo,
            ICustomerCheckPointRepository customerCheckPointRepository, IOfficeLocationManager office, IHelper helper,
            IKpiCustomRepository kpiRepo, IDashboardRepository dashboardRepo)
        {
            _repo = repo;
            _ApplicationContext = ApplicationContext;
            _sharedFbManager = sharedFbManager;
            _sharedManager = sharedManager;
            _inspRepo = inspRepo;
            _customerCheckPointRepository = customerCheckPointRepository;
            _office = office;
            _helper = helper;
            _kpiRepo = kpiRepo;
            _cusdecmap = new CustomerDecisionMap();
            _dashboardRepo = dashboardRepo;
        }
        /// <summary>
        /// Get customer decision list  by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerDecisionListResponse> GetCustomerDecisionList(int customerId)
        {
            var result = _repo.GetCustomerDecisionListByEfCore();
            IQueryable<CustomerDecisionRepo> res = null;
            if (customerId > 0)
            {
                res = result.Where(x => x.CustomerId == customerId);
                if (res == null || !res.Any())
                {
                    result = result.Where(x => x.IsDefault.Value);
                }

                else
                {
                    result = res;
                }
            }

            else
            {
                result = result.Where(x => x.IsDefault.Value);
            }

            var customerDecisionList = await result.Select(x => new CustomerDecisionModel
            {
                Id = x.CusDecId,
                Name = string.IsNullOrEmpty(x.CustomName) ? x.Name : x.CustomName,
                CusDecId = x.Id
            }).OrderBy(x => x.CusDecId).AsNoTracking().ToListAsync();

            if (customerDecisionList == null || !customerDecisionList.Any())
            {
                return new CustomerDecisionListResponse() { Result = CustomerDecisionListResponseResult.notfound };
            }

            return new CustomerDecisionListResponse()
            {
                CustomerDecisionList = customerDecisionList,
                Result = CustomerDecisionListResponseResult.success
            };

        }

        /// <summary>
        /// get customer decision Data.
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<CustomerDecisionResponse> GetCustomerDecisionData(int reportId)
        {

            var repsonse = new CustomerDecisionResponse();
            var dataSource = await _repo.GetCustomerDecisionData(reportId);

            if (dataSource != null)
            {
                repsonse.CustomerDecision = new CustomerDecisionResponseData()
                {
                    CustomerResultId = dataSource.CustomerResultId,
                    Comments = dataSource.Comments
                };
                repsonse.Result = CustomerDecisionResponseResult.success;
                return repsonse;
            }

            repsonse.Result = CustomerDecisionResponseResult.success;
            repsonse.CustomerDecision = null;
            return repsonse;
        }

        /// <summary>
        /// Add customer decision data 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public async Task<CustomerDecisionSaveResponse> AddCustomerDecision(CustomerDecisionSaveRequest modelData)
        {
            if (modelData != null)
            {

                // check user has edit customer decision role or not
                if (_ApplicationContext.RoleList.Contains((int)RoleEnum.EditInspectionCustomerDecision))
                {
                    // check data is exist or not 
                    var customerDecision = await _repo.GetCustomerDecisionData(modelData.ReportId);

                    if (customerDecision != null)
                    {
                        customerDecision.Active = false;
                        await _repo.UpdateCustomerDecision(customerDecision);

                        // new entry 
                        await AddInspectionCustomerDecision(modelData);

                        return new CustomerDecisionSaveResponse() { Result = CustomerDecisionSaveResponseResult.success };
                    }
                    else
                    {
                        await AddInspectionCustomerDecision(modelData);
                        return new CustomerDecisionSaveResponse() { Result = CustomerDecisionSaveResponseResult.success };
                    }
                }
                else
                {
                    return new CustomerDecisionSaveResponse() { Result = CustomerDecisionSaveResponseResult.noroleconfiguration };
                }
            }
            return new CustomerDecisionSaveResponse() { Result = CustomerDecisionSaveResponseResult.fail };
        }

        /// <summary>
        /// Get Customer Decision Reports Data
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<ReportCustomerDecisionResponse> GetCustomerDecisionReportsData(int reportId)
        {
            // get report customer decision data 
            var reportCustomerDecision = await _repo.GetReportCustomerDecision(reportId);

            if (reportCustomerDecision != null)
            {
                return new ReportCustomerDecisionResponse()
                {
                    CustomerResultId = reportCustomerDecision.CustomerResultId,
                    Comments = reportCustomerDecision.Comments,
                    Status = reportCustomerDecision.CustomerDecisionCustomStatus != null ? reportCustomerDecision.CustomerDecisionCustomStatus : reportCustomerDecision.CustomerDecisionStatus,
                    Result = ReportCustomerDecisionResponseResult.success
                };
            }

            return new ReportCustomerDecisionResponse()
            {
                Result = ReportCustomerDecisionResponseResult.fail
            };
        }

        /// <summary>
        /// Add new item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<int> AddInspectionCustomerDecision(CustomerDecisionSaveRequest request)
        {
            // new entry
            return await _repo.AddCustomerDecision(new InspRepCusDecision()
            {
                Comments = request.Comments,
                ReportId = request.ReportId,
                CustomerResultId = request.CustomerResultId,
                CreatedBy = request.IsAutoCustomerDecision.GetValueOrDefault() ? null : _ApplicationContext.UserId,
                Active = true,
                CreatedOn = DateTime.Now,
                IsAutoCustomerDecision = request.IsAutoCustomerDecision.GetValueOrDefault()
            });
        }
        /// <summary>
        /// get report id and customer decision details by fb report ids
        /// </summary>
        /// <param name="fbReportIds"></param>
        /// <returns></returns>
        public async Task<List<FBReportCustomerDecision>> GetCustomerDescistionWithReportId(List<int> fbReportIds)
        {
            return await _repo.GetCustomerDescistionWithReportId(fbReportIds);
        }

        //save customer decision from mobile
        public async Task<CustomerDecisionSaveMobileResponse> SaveMobileCustomerDecision(CustomerDecisionMobileSaveRequest request)
        {
            var response = new CustomerDecisionSaveMobileResponse();
            try
            {
                CustomerDecisionSaveRequest inspRequest = RequestMobileMap.MapCustomerDecisionRequest(request);
                var res = await AddCustomerDecision(inspRequest);

                if (res.Result == CustomerDecisionSaveResponseResult.success)
                {
                    response.data = Result.success;
                    response.meta = new MobileResult { success = true, message = "Customer Decision Save successful" };
                }
                else if (res.Result == CustomerDecisionSaveResponseResult.fail)
                {
                    response.data = Result.fail;
                    response.meta = new MobileResult { success = false, message = "Customer Decision Save failed." };
                }
                else if (res.Result == CustomerDecisionSaveResponseResult.noroleconfiguration)
                {
                    response.data = Result.noroleconfiguration;
                    response.meta = new MobileResult { success = false, message = "No Role configured to edit customer decision" };
                }
                else if (res.Result == CustomerDecisionSaveResponseResult.noemailconfiguration)
                {
                    response.data = Result.noemailconfiguration;
                    response.meta = new MobileResult { success = true, message = "No Email configured" };
                }
            }

            catch (Exception ex)
            {
                response.meta = new MobileResult { success = false, message = "Customer Decision Save failed." };
            }

            return response;

        }

        private async Task<IQueryable<InspTransaction>> CommonSummaryFilter(CustomerDecisionSummaryRequest request)
        {
            //get the booking summary request
            request = await GetCustomerDecisionSummaryRequest(request);

            //get the Iqueryable inspection data
            var data = _sharedManager.GetAllInspectionQuery();

            //map the request to specific type
            var sharedRequest = _sharedManager.GetCustomerDecisionQueryRequestMap(request);

            //apply fb report status filter
            if (request.CusDecisionGiven != null)
            {
                if (request.CusDecisionGiven == (int)CustomerDecisionGiven.Given)
                {
                    if (request.FbReportResultList != null && request.FbReportResultList.Any())
                    {
                        data = data.Where(x => x.FbReportDetails.Any(y => y.Active.Value && request.FbReportResultList.Contains(y.InspRepCusDecisions.Where(z => z.Active.Value).Select(z => z.CustomerResultId).FirstOrDefault())));
                    }
                    else
                    {
                        data = data.Where(x => x.FbReportDetails.Any(y => y.Active.HasValue && y.Active == true && y.InspRepCusDecisions.Any(z => z.Active.Value)));
                    }
                }

                else if (request.CusDecisionGiven == (int)CustomerDecisionGiven.NotGiven)
                {
                    data = data.Where(x => x.FbReportDetails.Any(y => y.Active.HasValue && y.Active == true && !y.InspRepCusDecisions.Any(z => z.Active.Value)));
                }
            }
            //get the data after applying request filters
            var res = _sharedManager.GetInspectionQuerywithRequestFilters(sharedRequest, data);

            if (request.BookingIds != null && request.BookingIds.Any())
            {
                res = res.Where(x => request.BookingIds.Contains(x.Id));
            }
            return res;
        }

        /// <summary>
        /// get customer decision summary data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CustomerDecisionReponse> CustomerDecisionSummary(CustomerDecisionSummaryRequest request)
        {
            if (request == null)
                return new CustomerDecisionReponse() { Result = CustomerDecisionResponseResult.notfound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var response = new BookingSummarySearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            var res = await CommonSummaryFilter(request);

            //apply skip and take
            var result = await GetCustomerDecisionBookingData(res, skip, take);


            if (result == null || !result.Any())
            {
                return new CustomerDecisionReponse { Result = CustomerDecisionResponseResult.notfound };
            }

            var bookingIds = result.Select(x => x.BookingId).ToList();

            //Get the service Type for the bookings
            var serviceTypeList = await _inspRepo.GetServiceType(bookingIds);

            var customerDecisionReportIdList = await _repo.GetCustomerDescistionWithBookingId(bookingIds);

            //get the product and container data
            var reportData = await _repo.GetCustomerDecisionContainerList(bookingIds);
            reportData.AddRange(await _repo.GetCustomerDecisionProductList(bookingIds));

            var customerResultIds = reportData.Select(x => x.CustomerDecisionResultId).Distinct().ToList();

            //take the customer result data from the resultids
            var customerResultAnalysis = await _dashboardRepo.GetCustomerResultAnalysis(customerResultIds);

            MapCustomerResultAnalysis(customerResultAnalysis, reportData);

            //group the data by reportID with product list
            var items = reportData.Where(x => x.ReportId.HasValue && x.ReportId > 0).GroupBy(p => p.ReportId, (key, _data) => new CustomerDecisionProductList
            {
                ReportId = key,
                BookingId = _data.Select(x => x.BookingId).FirstOrDefault(),
                ProductId = string.Join(" ,", _data.Select(x => x.ProductId).Distinct()),
                ResportResultId = _data.Select(x => x.ResportResultId).FirstOrDefault(),
                CustomerDecisionResultId = _data.Select(x => x.CustomerDecisionResultId).FirstOrDefault(),
                ReportResultName = _data.Select(x => x.ReportResultName).FirstOrDefault(),
                CustomerDecisionName = _data.Select(x => x.CustomerDecisionName).FirstOrDefault(),
                ProductIdList = _data.Select(x => x.ProductId).Distinct().ToList(),
                ResportResultColor = ReportResultColor.GetValueOrDefault(_data.Select(x => x.ResportResultId.GetValueOrDefault()).FirstOrDefault()),
                CustomerDecisionResultColor = CustomerResultDashboardColor.GetValueOrDefault(_data.Select(x => x.CustomerDecisionName).FirstOrDefault() ?? "")
            }).ToList();

            response.TotalCount = await res.CountAsync();

            //execute the status list in the booking summary page
            var inspectionStatusList = await _sharedManager.GetInspectionStatusList(res);

            inspectionStatusList.ForEach(x => x.StatusColor = InspectionStatusColor.GetValueOrDefault(x.Id, ""));

            var resultSet = result.Select(x => _cusdecmap.CustomerDecisionDataMap(x, customerDecisionReportIdList, serviceTypeList, items));

            return new CustomerDecisionReponse
            {
                TotalCount = response.TotalCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                InspectionStatuslst = inspectionStatusList,
                Data = resultSet.ToList(),
                Result = CustomerDecisionResponseResult.success
            };
        }

        /// <summary>
        /// execute the final booking list with pagination
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private async Task<List<CustomerDecisionSummaryResult>> GetCustomerDecisionBookingData(IQueryable<InspTransaction> bookingData, int skip, int take)
        {
            return await bookingData.Select(x => new CustomerDecisionSummaryResult
            {
                BookingId = x.Id,
                CustomerId = x.CustomerId,
                SupplierId = x.SupplierId,
                FactoryId = x.FactoryId,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName,
                //HasCustomerDecisionRole = _ApplicationContext.RoleList.Any(y => y == (int)RoleEnum.EditInspectionCustomerDecision || y == (int)RoleEnum.ViewInspectionCustomerDecision),
                TotalReportCount = x.FbReportDetails.Count(y => y.Active == true),
                StatusId = x.StatusId,
                CustomerBookingNo = x.CustomerBookingNo,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo
            }).OrderByDescending(x => x.BookingId).Skip(skip).Take(take).AsNoTracking().ToListAsync();

        }


        /// <summary>
        /// get booking products
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<EditCustomerDecisionResponse> GetCustomerDecisionBookingAndProducts(int bookingId)
        {
            //get booking data
            var bookingData = await _inspRepo.GetBookingData(bookingId);

            var bookingIdList = new[] { bookingId }.ToList();

            //get the booking service types
            var bookingServiceTypes = await _inspRepo.GetBookingServiceTypes(bookingId);

            //get the booking brands
            var bookingCustomerBrands = await _inspRepo.GetBookingBrands(bookingId);

            //get the booking departments
            var bookingCustomerDepartments = await _inspRepo.GetBookingDepartments(bookingId);

            if (bookingServiceTypes != null && bookingServiceTypes.Any())
            {
                bookingData.ServiceTypeIds = bookingServiceTypes;
            }

            if (bookingCustomerBrands != null && bookingCustomerBrands.Any())
            {
                bookingData.BrandIds = bookingCustomerBrands;
            }

            if (bookingCustomerDepartments != null && bookingCustomerDepartments.Any())
            {
                bookingData.DepartmentIds = bookingCustomerDepartments;
            }

            //checck if there is Customer Decision check point
            bookingData.CustomerDecisionCheckpointExists = await CheckPointExists(bookingData.CustomerId, (int)Service.InspectionId, (int)CheckPointTypeEnum.CustomerDecisionRequired, bookingData);

            //check if there is hide multi select customer decision check point
            bookingData.HideMultiSelectCustomerDecisionCheckPointExists = await CheckPointExists(bookingData.CustomerId, (int)Service.InspectionId, (int)CheckPointTypeEnum.HideMultiSelectCustomerDecision, bookingData);

            bookingData.IsContainer = ContainerServiceList.Contains(bookingData.InspectionTypeId);

            //fetch the product or container list
            var reportData = bookingData.InspectionTypeId == (int)InspectionServiceTypeEnum.Container ? await _repo.GetCustomerDecisionContainerList(bookingIdList) :
                await _repo.GetCustomerDecisionProductList(bookingIdList);

            var customerResultIds = reportData.Select(x => x.CustomerDecisionResultId).Distinct().ToList();

            //take the customer result data from the resultids
            var customerResultAnalysis = await _dashboardRepo.GetCustomerResultAnalysis(customerResultIds);

            MapCustomerResultAnalysis(customerResultAnalysis, reportData);

            var items = reportData.Where(x => x.ReportId.HasValue && x.ReportId > 0).GroupBy(p => p.ReportId, (key, _data) => new CustomerDecisionProductList
            {
                ReportId = key,
                ContainerId = _data.Select(x => x.ContainerId).FirstOrDefault(),
                BookingId = _data.Select(x => x.BookingId).FirstOrDefault(),
                ProductId = string.Join(" ,", _data.Select(x => x.ProductId).Distinct()),
                ResportResultId = _data.Select(x => x.ResportResultId).FirstOrDefault(),
                CustomerDecisionResultId = _data.Select(x => x.CustomerDecisionResultId).FirstOrDefault(),
                ReportResultName = _data.Select(x => x.ReportResultName).FirstOrDefault(),
                CustomerDecisionName = _data.Select(x => x.CustomerDecisionName).FirstOrDefault(),
                ProductIdList = bookingData.IsContainer ? _data.Select(x => "Container - " + x.ProductId).Distinct().ToList() : _data.Select(x => x.ProductId).Distinct().ToList(),
                CustomerDecisionComment = _data.Select(x => x.CustomerDecisionComment).FirstOrDefault(),
                CustomerDecisionResultCusDecId = _data.Select(x => x.CustomerDecisionResultCusDecId).FirstOrDefault(),
                ReportTitle = _data.Select(x => x.ReportTitle).FirstOrDefault(),
                ProductPhoto = _data.Select(x => x.ProductPhoto).FirstOrDefault()
            }).OrderBy(x => x.ProductId).ThenBy(y => y.ContainerId).ToList();

            return new EditCustomerDecisionResponse
            {
                BookingData = bookingData,
                ProductList = items,
                Result = CustomerDecisionResponseResult.success
            };
        }

        /// <summary>
        /// check if check point exists for the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <param name="checkpointType"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private async Task<bool> CheckPointExists(int customerId, int serviceId, int checkpointType, BookingData bookingData)
        {
            // get customer check point count
            var customerCheckPoint = await _inspRepo.GetCusCPByCusServiceId(customerId, serviceId, checkpointType);
            bool checkPointExists = false;

            if (customerCheckPoint > 0)
            {
                checkPointExists = true;
                //if no brand or dept or service type is selected, then checkpoint is applied to all the brands, depts and service types
                var brandCheckpoint = true;
                var deptCheckpoint = true;
                var serviceCheckpoint = true;

                var brandData = await _customerCheckPointRepository.GetCustomerCheckPointBrand(new int[] { customerCheckPoint }.ToList());

                if (brandData.Any())
                {
                    brandCheckpoint = bookingData.BrandIds.Any(x => brandData.Any(y => y.Id == x));
                }

                var deptData = await _customerCheckPointRepository.GetCustomerCheckPointDept(new int[] { customerCheckPoint }.ToList());
                if (deptData.Any())
                {
                    deptCheckpoint = bookingData.DepartmentIds.Any(x => deptData.Any(y => y.Id == x));
                }

                var serviceTypeData = await _customerCheckPointRepository.GetCustomerCheckPointServiceType(new int[] { customerCheckPoint }.ToList());
                if (serviceTypeData.Any())
                {
                    serviceCheckpoint = bookingData.ServiceTypeIds.Any(x => serviceTypeData.Any(y => y.ServiceTypeId == x));
                }

                if (!brandCheckpoint || !deptCheckpoint || !serviceCheckpoint)
                {
                    checkPointExists = false;
                }

            }
            return checkPointExists;
        }

        /// <summary>
        /// Add customer decision data 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public async Task<CustomerDecisionSaveResponse> SaveCustomerDecisionList(CustomerDecisionListSaveRequest modelData)
        {
            if (modelData != null)
            {
                // check data is exist or not 
                var customerDecision = await _repo.GetCustomerDecisionListData(modelData.ReportIdList);

                if (customerDecision != null && customerDecision.Any())
                {
                    customerDecision.ForEach(x => x.Active = false);
                    _repo.EditEntities(customerDecision);

                    // new entry 
                    SaveInspectionCustomerDecisionList(modelData);

                    return new CustomerDecisionSaveResponse() { Result = CustomerDecisionSaveResponseResult.success };
                }
                else
                {
                    SaveInspectionCustomerDecisionList(modelData);
                    return new CustomerDecisionSaveResponse() { Result = CustomerDecisionSaveResponseResult.success };
                }
            }
            return new CustomerDecisionSaveResponse() { Result = CustomerDecisionSaveResponseResult.fail };
        }

        /// <summary>
        /// Add new item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private void SaveInspectionCustomerDecisionList(CustomerDecisionListSaveRequest request)
        {
            // new entry
            List<InspRepCusDecision> listToSave = new List<InspRepCusDecision>();
            foreach (var reportId in request.ReportIdList)
            {
                listToSave.Add(new InspRepCusDecision
                {
                    Comments = request.Comments,
                    ReportId = reportId,
                    CustomerResultId = request.CustomerResultId,
                    CreatedBy = request.IsAutoCustomerDecision.GetValueOrDefault() ? null : _ApplicationContext.UserId,
                    Active = true,
                    CreatedOn = DateTime.Now,
                    IsAutoCustomerDecision = request.IsAutoCustomerDecision.GetValueOrDefault()
                });
            }

            _repo.SaveList(listToSave, false);
        }

        /// <summary>
        /// get problematic remarks by report Id
        /// </summary>
        /// <param name="summaryName"></param>
        /// <param name="fbReportId"></param>
        /// <returns></returns>
        public async Task<CusDecisionProblematicRemarksResponse> GetProblematicRemarksByReport(int id, int fbReportId)
        {
            var res = await _repo.GetProblematicRemarksCd(id, fbReportId);

            if (res == null || !res.Any())
            {
                return new CusDecisionProblematicRemarksResponse { Result = CustomerDecisionResponseResult.notfound };
            }

            return new CusDecisionProblematicRemarksResponse
            {
                Data = res,
                Result = CustomerDecisionResponseResult.success
            };
        }

        /// <summary>
        /// Map the initial booking summary request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<CustomerDecisionSummaryRequest> GetCustomerDecisionSummaryRequest(CustomerDecisionSummaryRequest request)
        {
            //filter data based on user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.CustomerId = request?.CustomerId != null && request?.CustomerId != 0 ? request?.CustomerId : _ApplicationContext.CustomerId;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.FactoryIdlst = request.FactoryIdlst != null && request.FactoryIdlst.Any() ? request.FactoryIdlst : new List<int>().Append(_ApplicationContext.FactoryId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.SupplierId = request.SupplierId != null && request.SupplierId != 0 ? request.SupplierId.Value : _ApplicationContext.SupplierId;
                        break;
                    }
            }

            request.CustomerList = new List<int>();

            //if logged in user type is internal user
            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
            {
                var _cusofficelist = await _office.GetOnlyOfficeIdsByUser(_ApplicationContext.StaffId);

                if (_cusofficelist.Any())
                {
                    if (request.Officeidlst != null && request.Officeidlst.Any())
                    {
                        request.Officeidlst = _cusofficelist.Where(x => request.Officeidlst.Contains(x)).Select(x => (int?)x).ToList();
                    }
                    else
                    {
                        request.Officeidlst = _cusofficelist.Select(x => (int?)x).ToList();
                    }
                }

                if (request.CustomerId > 0)
                    request.CustomerList.Add(request.CustomerId.Value);
            }
            else
            {
                if (request.CustomerId > 0)
                    request.CustomerList.Add(request.CustomerId.Value);
            }

            return request;

        }

        public async Task<int> GetBookingIdByReportId(int reportId)
        {
            return await _repo.GetBookingIdByReportId(reportId);
        }

        public async Task<DataTable> ExportCustomerDecisionSummary(CustomerDecisionSummaryRequest request)
        {
            var res = await CommonSummaryFilter(request);

            //Get the booking information
            var bookingDetails = await res.Select(x => new InspectionBookingExportData()
            {
                BookingNo = x.Id,
                CustomerBookingNo = x.CustomerBookingNo,
                CustomerId = x.CustomerId,
                Customer = x.Customer.CustomerName,
                Supplier = x.Supplier.SupplierName,
                Factory = x.Factory.SupplierName,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo,
                Office = x.Office.LocationName,
                Collection = x.Collection.Name
            }).AsNoTracking().ToListAsync();

            var bookingIds = res.Select(x => x.Id);

            //Get the service Type for the bookings
            var serviceTypeList = await _inspRepo.GetServiceTypeList(bookingIds);

            //get the product and container data
            var reportData = await _repo.GetCustomerDecisionContainerListByEfCore(bookingIds);
            reportData.AddRange(await _repo.GetCustomerDecisionProductListByEfCore(bookingIds));

            var poDetails = await _inspRepo.GetBookingPoListByBookingQuery(bookingIds);

            //get dept id and booking id by booking
            var bookingDeptAccessList = await _inspRepo.GetDeptBookingIdsByBookingQuery(bookingIds);

            //get brand id and booking id  by booking
            var bookingBrandAccessList = await _inspRepo.GetBrandBookingIdsByBookingQuery(bookingIds);

            //get buyer details and booking id  by booking
            var bookingBuyerAccessList = await _inspRepo.GetBuyerBookingIdsByBookingQuery(bookingIds);

            //get the fb_report_insp_summary data
            var inspSummaryData = await _kpiRepo.GetFBInspSummaryResultbyReport(bookingIds);

            var cusDecisionData = await _repo.GetCustomerDecisionByEfCore(bookingIds);

            //select only the summary names as it is a dynamic field
            var resultName = inspSummaryData?.
                         Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).Distinct().ToList();

            var data = reportData.Select(x => CustomerDecisionMap.ExportComplaintSummaryDataMap(x, bookingDetails, serviceTypeList, bookingDeptAccessList, bookingBrandAccessList, bookingBuyerAccessList, poDetails, cusDecisionData));

            data = data.OrderByDescending(x => x.BookingId);

            //add the dynamic columns from col 17
            int columnIndex = 17;

            //add the dynamic column names to the data table
            var dtBookingTable = _helper.ConvertToDataTableWithCaptionAndDynamiColumns(data.ToList(), columnIndex, resultName);

            var reportResultDataList = inspSummaryData?
                                .Select(y => new FbReportInspectionResult
                                {
                                    ReportId = y.FBReportId,
                                    Name = y.Name,
                                    Result = y.Result
                                }).ToList();

            var removedColumn = "ReportId";

            //map the values to the dynamic columns
            CustomerDecisionMap.MapDynamicColumns(dtBookingTable, resultName, reportResultDataList, removedColumn);

            return dtBookingTable;
        }

        public async Task<IEnumerable<InspRepCusDecisionTemplate>> GetCusDecisionTemplate()
        {
            return await _repo.GetCusDecisionTemplate();
        }

        private void MapCustomerResultAnalysis(List<CustomerResultMasterRepo> customerResultAnalysis, List<CustomerDecisionProductList> reportDetailsList)
        {
            foreach (var reportDetail in reportDetailsList)
            {
                var customerResult = customerResultAnalysis.FirstOrDefault(x => x.Id == reportDetail.CustomerDecisionResultId);
                if (customerResult != null)
                {
                    if (!string.IsNullOrEmpty(customerResult.CustomDecisionName))
                        reportDetail.CustomerDecisionName = customerResult.CustomDecisionName;
                    else
                        reportDetail.CustomerDecisionName = customerResult.CustomerDecisionName;
                }
            }
        }
    }
}
