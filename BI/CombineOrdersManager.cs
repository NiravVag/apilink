using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.CombineOrders;
using DTO.Common;
using DTO.DataAccess;
using DTO.EventBookingLog;
using DTO.SamplingQuantity;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    /// <summary>
    /// This Manager class will handle all the business logic of combine orders and sampling quantity
    /// </summary>
    public class CombineOrdersManager : ApiCommonData, ICombineOrdersManager
    {
        private readonly ICombineOrdersRepository _repo = null;
        private readonly IInspectionBookingRepository _repoInspection = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IReferenceRepository _referencerepo = null;
        private readonly ICancelBookingRepository _cancelBookingRepository = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly IEventBookingLogManager _eventBookingLog = null;
        private readonly ISupplierManager _supplierManager = null;

        /// <summary>
        /// This constructor will initalize the interface instance 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="repoInspection"></param>
        public CombineOrdersManager(ICombineOrdersRepository repo, IInspectionBookingRepository repoInspection, IAPIUserContext ApplicationContext,
            IReferenceRepository _ref, ICancelBookingRepository cancelBookingRepository, IUserRightsManager userManager, IEventBookingLogManager eventBookingLog, ISupplierManager supplierManager)
        {
            _repo = repo;
            _repoInspection = repoInspection;
            _ApplicationContext = ApplicationContext;
            _referencerepo = _ref;
            _cancelBookingRepository = cancelBookingRepository;
            _userManager = userManager;
            _eventBookingLog = eventBookingLog;
            _supplierManager = supplierManager;
        }

        /// <summary>
        /// Get the booking items with Combine orders by booking Id
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public async Task<CombineOrderSummaryResponse> GetCombineOrderDetails(int inspectionId)
        {
            var combineOrdersData = await _repo.GetCombineOrderDetails(inspectionId);

            var response = new CombineOrderSummaryResponse();

            if (combineOrdersData == null)
            {
                return new CombineOrderSummaryResponse() { Result = CombineOrdersSummaryResponseResult.failed };
            }

            response.CombineOrdersList = await GetCombineOrdersList(combineOrdersData);

            // set booking role for accessing check to combine orders
            if (response.CombineOrdersList != null)
            {
                response.bookingStatus = combineOrdersData.StatusId;
                this.settotalNumberOfReports(response);
                this.setBookingRole(response);
            }

            response.Result = CombineOrdersSummaryResponseResult.success;

            return response;
        }


        public async Task<PoDetailsResponse> GetPoDetails(int inspectionId, int productRefId)
        {
            var poDetails = await _repo.GetPODetailsbyProductRefId(inspectionId, productRefId);

            var response = new PoDetailsResponse();

            if (poDetails == null)
            {
                return new PoDetailsResponse() { Result = PoDetailsResponseResult.failed };
            }

            response.PoList = MapPoList(poDetails);

            response.Result = PoDetailsResponseResult.success;

            return response;
        }

        /// <summary>
        /// To check the role for booking combine orders
        /// </summary>
        /// <param name="combineOrdersData"></param>
        private void setBookingRole(CombineOrderSummaryResponse combineOrdersData)
        {
            if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)//logic for location configuration
            {
                combineOrdersData.IsBookingRequestRole = false;
                combineOrdersData.IsBookingConfirmRole = false;
                combineOrdersData.IsBookingVerifyRole = false;

                foreach (var role in _ApplicationContext.RoleList)
                {

                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionRequest))
                    {
                        combineOrdersData.IsBookingRequestRole = true;
                    }

                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionConfirmed))
                    {
                        combineOrdersData.IsBookingConfirmRole = true;
                    }

                    if (_ApplicationContext.RoleList.Contains((int)RoleEnum.InspectionVerified))
                    {
                        combineOrdersData.IsBookingVerifyRole = true;
                    }
                }
            }
        }

        /// <summary>
        /// set total number of reports based on the combine orders
        /// if the item is combined then report count 1 and any product not combined then each product will be a new report count.        
        /// </summary>
        /// <param name="combineOrdersData"></param>
        private void settotalNumberOfReports(CombineOrderSummaryResponse combineOrdersData)
        {
            int totalNumberOfReports = 0;

            totalNumberOfReports += combineOrdersData.CombineOrdersList != null ? combineOrdersData.CombineOrdersList.Where(x => x.CombineProductId == null).Count() +
                                                       combineOrdersData.CombineOrdersList.Where(x => x.CombineProductId != null).
                                                       Select(x => x.CombineProductId).Distinct().Count() : 0;


            combineOrdersData.totalNumberofReports = totalNumberOfReports;
        }



        private async Task<List<CombineOrders>> GetCombineOrdersList(InspTransaction entity)
        {
            List<CombineOrders> objList = new List<CombineOrders>();

            if (entity == null)
                return null;

            var activeCombineOrders = entity.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value);


            foreach (var productData in activeCombineOrders)
            {

                objList.Add(new CombineOrders()
                {
                    InspectionId = productData.InspectionId,
                    TotalBookingQuantity = productData.TotalBookingQuantity,
                    ProductId = productData.ProductId,
                    ColorCode = "FFFFFF", // default
                    ProductName = productData.Product?.ProductId,
                    ProductDescription = productData.Product?.ProductDescription,
                    CombineProductId = productData.CombineProductId,
                    CombinedAqlQuantity = (productData.IsDisplayMaster.HasValue && productData.IsDisplayMaster.Value) ? 0 : productData.CombineAqlQuantity,
                    Id = productData.Id,
                    SamplingQuantity = (productData.IsDisplayMaster.HasValue && productData.IsDisplayMaster.Value) ? 0 : productData.AqlQuantity,
                    AqlLevel = productData?.Aql,
                    IsDisplayMaster = productData.IsDisplayMaster,
                    ParentProductId = productData.ParentProductId,
                    ParentProductName = productData.ParentProduct?.ProductId,
                    PoName = string.Join(",", entity.InspPurchaseOrderTransactions.Where(x => x.ProductRefId == productData.Id && x.Active.HasValue
                                                                                            && x.Active.Value).Select(x => x.Po?.Pono).ToList().Distinct()),
                    FactoryReference = productData.Product?.FactoryReference
                });
            }

            return objList.OrderBy(x => x.CombineProductId).
                                        ThenByDescending(x => x.CombinedAqlQuantity).ThenBy(x => x.ProductName).ToList();
        }


        private List<PoDetails> MapPoList(IEnumerable<InspPurchaseOrderTransaction> poList)
        {
            List<PoDetails> objList = new List<PoDetails>();

            foreach (var poData in poList)
            {
                objList.Add(new PoDetails()
                {

                    Id = poData.Id,
                    InspectionId = poData.InspectionId,
                    PoId = poData.PoId,
                    PoName = poData.Po.Pono,
                    BookingQuantity = poData.BookingQuantity
                });
            }

            return objList;
        }

        /// <summary>
        /// To claculate the sampling quantity based on the productId and booking id 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<int> GetSamplingQuantityByBookingProduct(int bookingId, int productId)
        {
            var bookingPoTransactions = await _repoInspection.GetBookingPOTransactionDetails(bookingId);

            int? aqlId = 0;
            int critical = 0;
            int major = 0;
            int minor = 0;
            int orderQuantity = 0;
            int resultSampling = 0;

            if (bookingPoTransactions != null)
            {
                var insppo = bookingPoTransactions.Where(x => x.ProductId == productId).FirstOrDefault();
                aqlId = insppo?.Aql;
                critical = insppo?.Critical ?? 0;
                major = insppo?.Major ?? 0;
                minor = insppo?.Minor ?? 0;
                orderQuantity = bookingPoTransactions.Where(x => x.ProductId == productId).FirstOrDefault().TotalBookingQuantity;
            }

            if (aqlId != null && aqlId != 0)
            {
                var request = new SamplingQuantityRequest()
                {
                    AqlId = aqlId,
                    OrderQuantity = orderQuantity,
                    CriticalId = critical,
                    MajorId = major,
                    MinorId = minor,
                    BookingId = bookingId
                };

                resultSampling = await GetSamplingQuantityByBookingAndAql(request);
            }

            return resultSampling;
        }
        /// <summary>
        /// Get combine aql quantity for specific booking products.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<int?> GetCombineAqlQuantityByBookingProduct(int bookingId, int productId)
        {
            //var bookingEntity = await _repoInspection.GetInspectionByID(bookingId);

            int? combineAqlQuantity = 0;

            var combineOrdersData = await _repo.GetCombineOrderDetails(bookingId);

            var response = new CombineOrderSummaryResponse();

            if (combineOrdersData == null)
            {
                return combineAqlQuantity;
            }

            var combineOrdersList = await GetCombineOrdersList(combineOrdersData);

            // get unique combine orders data  for checking 
            var uniqueCombineOrderIds = combineOrdersList.Where(x => x.CombineProductId != null).Select(x => x.CombineProductId).Distinct();

            foreach (var item in combineOrdersList)
            {
                // check any product which is not combined and combined
                if (item.ProductId == productId)
                {
                    // check the product is not combined then simply pass SamplingQuantity as aql quantity
                    if (item.CombineProductId == null && !uniqueCombineOrderIds.Any(x => x == item.ProductId))
                    {
                        combineAqlQuantity = item.SamplingQuantity;
                        break;
                    }
                    else // if the product is combined then pass the combine aql quantity.
                    {
                        combineAqlQuantity = item.CombinedAqlQuantity;
                        break;
                    }
                }
            }

            return combineAqlQuantity;
        }

        /// <summary>

        /// <summary>
        /// Get the sampling quantity by booking id and aql(critical,major,minor)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> GetSamplingQuantityByBookingAndAql(SamplingQuantityRequest request)
        {
            int samplingQuantity = 0;
            if (request != null)
            {
                var bookingCustomerServiceTypeData = await _repoInspection.GetBookingCustomerServiceTypes(request.BookingId);

                if (bookingCustomerServiceTypeData != null)
                {
                    request.ServiceTypeId = bookingCustomerServiceTypeData.ServiceTypeId;
                    request.CustomerId = bookingCustomerServiceTypeData.CustomerId;
                    samplingQuantity = await GetSamplingQuantity(request);
                }
            }

            return samplingQuantity;
        }


        /// <summary>
        /// Get the sampling quantity by aql(crticial,major,minor)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> GetSamplingQuantity(SamplingQuantityRequest request)
        {
            int samplingQuantity = 0;

            if (request != null)
            {

                var samplingSizeIgnoreAcceptanceLevel = await _repoInspection.GetServiceTypeIgnoreAcceptanceLevel(request.CustomerId, request.ServiceTypeId);

                var sampleSizeData = await _repo.GetSampleSizeCode(request.OrderQuantity);

                string strSampleSizeCode = string.Empty;

                if (sampleSizeData != null && sampleSizeData.Count() > 0)
                {
                    var sampleSizeDataCode = sampleSizeData.FirstOrDefault();

                    if (sampleSizeDataCode != null)
                    {
                        switch (request.AqlId)
                        {
                            case 1:
                                strSampleSizeCode = sampleSizeDataCode.LevelISampleSizeCode.ToString();
                                break;
                            case 2:
                                strSampleSizeCode = sampleSizeDataCode.LevelIiSampleSizeCode.ToString();
                                break;
                            case 3:
                                strSampleSizeCode = sampleSizeDataCode.LevelIiiSampleSizeCode.ToString();
                                break;
                            case 4:
                                strSampleSizeCode = sampleSizeDataCode.LevelS1SampleSizeCode.ToString();
                                break;
                            case 5:
                                strSampleSizeCode = sampleSizeDataCode.LevelS2SampleSizeCode.ToString();
                                break;
                            case 6:
                                strSampleSizeCode = sampleSizeDataCode.LevelS3SampleSizeCode.ToString();
                                break;
                            case 7:
                                strSampleSizeCode = sampleSizeDataCode.LevelS4SampleSizeCode.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!samplingSizeIgnoreAcceptanceLevel)
                {
                    //get all sample size acceptance code
                    var lstSamplingCodeAcceptanceQlty = await _referencerepo.GetSampleSizeCodeForAcceQuality();

                    //get single pick values  
                    var aqlpicktype = await _referencerepo.GetServicePickFirst();
                    if (lstSamplingCodeAcceptanceQlty != null && lstSamplingCodeAcceptanceQlty.Any())
                    {
                        //get all the pick values of critical / major / minor id
                        var pickvalues = aqlpicktype.Where(x => new List<int>() { request.CriticalId, request.MajorId, request.MinorId }.Contains(x.Id)).ToList().Select(x => x.Value);

                        if (pickvalues != null && pickvalues.Any())
                        {
                            var lstaqltype = new List<string>();
                            //sample size code calculate based on Acceptance quality levels.
                            foreach (var pickvalue in pickvalues)
                            {
                                var aqllevel = lstSamplingCodeAcceptanceQlty.Where(x => x.SampleSizeCode == strSampleSizeCode
                                                && x.PickValue.HasValue && pickvalue.HasValue && pickvalue.Value == x.PickValue.Value).FirstOrDefault();
                                if (aqllevel != null)
                                {
                                    lstaqltype.Add(aqllevel.AccSampleSizeCode);
                                }
                            }
                            //if none or one acceptance level value then add the default AQL level
                            if (!lstaqltype.Any() || (lstaqltype.Any() && lstaqltype.Count == 1))
                            {
                                lstaqltype.Add(strSampleSizeCode);
                            }
                            // take the highest sample size code
                            strSampleSizeCode = lstaqltype.OrderByDescending(x => x).First();
                        }
                    }
                }

                var samplingSizeQuantity = await _repo.GetSamplingQuantityBySamplingSizeByCode(strSampleSizeCode);

                if (samplingSizeQuantity != null)
                {
                    samplingQuantity = samplingSizeQuantity.Select(x => x.SampleSize).FirstOrDefault();
                }
                //if sample size greater than booking qty then take booking qty.
                if (samplingQuantity > request.OrderQuantity)
                {
                    samplingQuantity = request.OrderQuantity;
                }

            }

            return samplingQuantity;

        }

        /// <summary>
        /// Save or update Combine Orders List
        /// </summary>
        /// <param name="combineOrders"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public async Task<SaveCombineOrdersResponse> SaveCombineOrders(List<SaveCombineOrdersRequest> combineOrders, int inspectionId)
        {
            try
            {
                //if it is internal user and not falls under booking request,verify,confirm then throw the message
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser
                    && !_ApplicationContext.RoleList.Any(x => InternalUserCombineRoleAccessList.Contains(x)))
                    return new SaveCombineOrdersResponse() { Result = SaveCombineOrdersResult.InternalUserRoleNotMatched };

                var response = new SaveCombineOrdersResponse();

                var entity = await _repoInspection.GetBookingCombineOrders(inspectionId);

                //if it is internal user and booking status not falls under the given status then throw the message
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser && !InternalUserCombineStatusAccessList.Any(x => x == entity.StatusId))
                    return new SaveCombineOrdersResponse() { Result = SaveCombineOrdersResult.InternalUserStatusNotMatched };

                //if it is external user(customer,supplier,factory) and not falls under the specific status then throw the message
                else if ((_ApplicationContext.UserType == UserTypeEnum.Customer
                    || _ApplicationContext.UserType == UserTypeEnum.Supplier
                    || _ApplicationContext.UserType == UserTypeEnum.Factory)
                    && !ExternalUserCombineStatusAccessList.Any(x => x == entity.StatusId))
                    return new SaveCombineOrdersResponse() { Result = SaveCombineOrdersResult.ExternalUserStatusNotMatched };

                if (entity == null)
                    return new SaveCombineOrdersResponse { Result = SaveCombineOrdersResult.CombineOrdersIsNotFound };

                if (combineOrders != null)
                {
                    // Add Booking Log information - in the Event booking log table.
                    await _eventBookingLog.SaveLogInformation(new EventBookingLogInfo()
                    {
                        Id = 0,
                        AuditId = 0,
                        BookingId = inspectionId,
                        StatusId = entity.StatusId,
                        LogInformation = JsonConvert.SerializeObject(combineOrders)
                    });

                    //if combine aql quantity for the combine group is less than or equal to zero
                    var combineProductIds = combineOrders.Where(x => x.CombineProductId != null && x.AqlId > 0).Select(x => x.CombineProductId).Distinct();

                    foreach (var combineProductId in combineProductIds)
                    {
                        var combineData = combineOrders.Where(x => x.CombineProductId == combineProductId && x.CombinedAqlQuantity > 0);
                        if (combineData != null && combineData.Count() == 0)
                        {
                            return new SaveCombineOrdersResponse { Result = SaveCombineOrdersResult.CombineAqlQuantityGreaterThanZero };
                        }
                    }

                    this.UpdateCombineOrders(combineOrders, entity);
                    // need changes 
                    var quotationDetails = await _cancelBookingRepository.BookingQuotationExists(inspectionId);
                    if (quotationDetails != null)
                    {
                        response.isEmailRequired = true;
                        await GetEmailDetails(entity, response);
                    }
                }

                await _repoInspection.EditInspectionBooking(entity);
                response.Id = entity.Id;
                response.Result = SaveCombineOrdersResult.Success;
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To update booking Combine Orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void UpdateCombineOrders(List<SaveCombineOrdersRequest> combineOrders, InspTransaction entity)
        {
            if (combineOrders != null)
            {
                // Update if data already exist in the db
                var lstCombineOrdersToEdit = new List<InspProductTransaction>();

                foreach (var item in combineOrders.Where(x => x.Id > 0))
                {
                    var combineOrder = entity.InspProductTransactions.FirstOrDefault(x => x.Id == item.Id);
                    if (combineOrder != null)
                    {
                        combineOrder.CombineProductId = item.CombineProductId;
                        // check aql value is valid and it is involved in combine then update
                        if (item.CombineProductId != null && item.AqlId != null && item.AqlId > 0)
                            combineOrder.Aql = item.AqlId;
                        combineOrder.AqlQuantity = item.SamplingQuantity;
                        combineOrder.CombineAqlQuantity = item.CombinedAqlQuantity;
                        combineOrder.Active = true;
                        combineOrder.UpdatedBy = _ApplicationContext.UserId;
                        combineOrder.UpdatedOn = DateTime.Now;
                    }

                    lstCombineOrdersToEdit.Add(combineOrder);
                }

                if (lstCombineOrdersToEdit.Count > 0)
                    _repo.EditEntities(lstCombineOrdersToEdit);
            }
        }

        /// <summary>
        /// Add new combine orders list
        /// </summary>
        /// <param name="combineOrders"></param>
        /// <param name="entity"></param>
        /// 

        /// <summary>
        /// Calculate the sampling quantity based on the list of combine order products and its order quantity
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="combineOrders"></param>
        /// <returns></returns>
        public async Task<CombineSamplingProductListResponse> GetCombinedAQLQty(int bookingId, int? aqlId, List<CombineOrderSamplingData> combineOrders)
        {
            int orderQuantity = 0;
            int resultSampling = 0;
            int critical = 0;
            int major = 0;
            int minor = 0;
            List<CombineOrderSamplingData> objListSamplingData = new List<CombineOrderSamplingData>();
            var response = new CombineSamplingProductListResponse();
            response.SamplingDataList = new List<CombineOrderSamplingData>();
            //added function to fetch only the product transaction details
            var bookingPoTransactions = await _repoInspection.GetBookingProductTransactionDetails(bookingId);

            if (bookingPoTransactions == null)
            {
                return new CombineSamplingProductListResponse() { Result = CombineSamplingProductListResult.failed };
            }

            if (combineOrders == null || combineOrders.Count == 0)
            {
                return new CombineSamplingProductListResponse() { Result = CombineSamplingProductListResult.failed };
            }

            // if there is a aql change from combine screen - update Aql Quantity
            if (aqlId != null && aqlId != 0)
            {
                foreach (var item in combineOrders)
                {
                    var insppo = bookingPoTransactions.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

                    critical = insppo?.Critical ?? 0;
                    major = insppo?.Major ?? 0;
                    minor = insppo?.Minor ?? 0;
                    orderQuantity = item.OrderQuantity;
                    // if the request aql is not same as db aql then calculate sampling and dont update aql quantity if it is display master product
                    if (aqlId != null && aqlId != 0 && insppo.Aql != aqlId && insppo.IsDisplayMaster.HasValue && !insppo.IsDisplayMaster.Value)
                    {
                        if (aqlId != (int)AqlType.AQLCustom)
                        {
                            var request = new SamplingQuantityRequest()
                            {
                                AqlId = aqlId,
                                OrderQuantity = orderQuantity,
                                CriticalId = critical,
                                MajorId = major,
                                MinorId = minor,
                                BookingId = bookingId
                            };

                            resultSampling = await GetSamplingQuantityByBookingAndAql(request);

                            item.AqlQuantity = resultSampling;
                        }
                    }
                }
            }

            var uniqueCombineOrderIds = combineOrders.Select(x => x.CombineProductId).Distinct();

            foreach (var id in uniqueCombineOrderIds)
            {
                var combineGroupList = combineOrders.Where(x => x.CombineProductId == id).ToList();
                combineGroupList.Select(x => x.SamplingQuantity = 0);

                var aqlProductId = combineGroupList.Where(x => x.AqlQuantity > 0).Select(x => x.ProductId).FirstOrDefault();

                var insppo = bookingPoTransactions.Where(x => x.ProductId == aqlProductId).FirstOrDefault();

                var combineProductIds = combineGroupList.Select(x => x.ProductId);

                var masterProductId = bookingPoTransactions.Where(x => combineProductIds.Contains(x.ProductId) && x.IsDisplayMaster.HasValue
                                                                                    && x.IsDisplayMaster.Value).FirstOrDefault()?.ProductId;

                // check fb report is exist then notify to user 
                var inspReports = bookingPoTransactions.Where(x => combineGroupList.Any(y => y.ProductId == x.ProductId));

                if (inspReports.Any(x => x.FbReportId != null))
                {
                    return new CombineSamplingProductListResponse() { Result = CombineSamplingProductListResult.reportExist };
                }

                aqlId = (aqlId != null && aqlId != 0) ? aqlId : insppo?.Aql;
                critical = insppo?.Critical ?? 0;
                major = insppo?.Major ?? 0;
                minor = insppo?.Minor ?? 0;

                //if display master prouduct then exclude display master product aql qty
                if (masterProductId != null)
                    orderQuantity = combineGroupList.Where(x => x.ProductId != masterProductId).Sum(x => x.OrderQuantity);
                else
                    orderQuantity = combineGroupList.Sum(x => x.OrderQuantity);

                if (aqlId != null && aqlId != 0)
                {
                    //check if we have the display master product with single child
                    bool displayProductSingleChild = false;
                    if (masterProductId != null)
                    {
                        if (bookingPoTransactions.Where(x => combineProductIds.Contains(x.ProductId) && x.IsDisplayMaster.HasValue
                                                                                    && !x.IsDisplayMaster.Value).Count() == 1)
                            displayProductSingleChild = true;
                    }


                    if (aqlId != (int)AqlType.AQLCustom && !displayProductSingleChild)
                    {
                        var request = new SamplingQuantityRequest()
                        {
                            AqlId = aqlId,
                            OrderQuantity = orderQuantity,
                            CriticalId = critical,
                            MajorId = major,
                            MinorId = minor,
                            BookingId = bookingId
                        };

                        resultSampling = await GetSamplingQuantityByBookingAndAql(request);

                        var combineGroup = combineGroupList.Where(x => x.ProductId == aqlProductId && x.AqlQuantity > 0).FirstOrDefault();
                        if (combineGroup != null)
                            combineGroup.SamplingQuantity = resultSampling;
                    }
                    else
                    {
                        resultSampling = combineGroupList.Sum(x => x.AqlQuantity);
                        var combineGroup = combineGroupList.Where(x => x.ProductId == aqlProductId).FirstOrDefault();
                        if (combineGroup != null)
                            combineGroup.SamplingQuantity = resultSampling;
                    }
                }
                response.SamplingDataList.AddRange(combineGroupList);
            }

            response.Result = CombineSamplingProductListResult.success;
            return response;
        }

        public async Task GetEmailDetails(InspTransaction entity, SaveCombineOrdersResponse response)
        {
            //Get product category details
            var productCategoryList = await _repoInspection.GetProductCategoryDetails(new[] { entity.Id });
            //Get Department details
            var departmentData = await _repoInspection.GetBookingDepartmentList(new[] { entity.Id });
            //Get Brand details
            var brandData = await _repoInspection.GetBookingBrandList(new[] { entity.Id });

            //factory country 
            int? factoryCountryId = null;
            if (entity.FactoryId.HasValue)
            {
                var factoryCountryData = await _supplierManager.GetSupplierHeadOfficeAddress(entity.FactoryId.Value);
                if (factoryCountryData.Result == SupplierListResult.Success)
                    factoryCountryId = factoryCountryData.countryId;
            }


            var userAccessFilter = new UserAccess
            {
                OfficeId = entity.OfficeId.GetValueOrDefault(),
                ServiceId = (int)Service.InspectionId,
                CustomerId = entity.CustomerId,
                RoleId = (int)RoleEnum.QuotationRequest,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };
            var quotationUsers = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            response.ToRecipients = quotationUsers;

            userAccessFilter = new UserAccess
            {
                OfficeId = entity.OfficeId.GetValueOrDefault(),
                ServiceId = (int)Service.InspectionId,
                CustomerId = entity.CustomerId,
                RoleId = (int)RoleEnum.InspectionVerified,
                ProductCategoryIds = productCategoryList != null && productCategoryList.Any() ?
                                         productCategoryList?.Where(x => x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct() : Enumerable.Empty<int?>(),
                DepartmentIds = departmentData.Any() ? departmentData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                BrandIds = brandData.Any() ? brandData?.Select(x => (int?)x.Id).Distinct() : Enumerable.Empty<int?>(),
                FactoryCountryId = factoryCountryId
            };

            var verifiedUsers = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

            response.CcRecipients = verifiedUsers;
            //return response;
        }
    }
}
