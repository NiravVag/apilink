using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Inspection;
using DTO.InspectionCertificate;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class InspectionCertificateManager : ApiCommonData, IInspectionCertificateManager
    {
        private readonly IInspectionCertificateRepository _repo = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICustomerCheckPointManager _ccpManager = null;
        private readonly IInspectionCustomerDecisionManager _cdManager = null;
        private readonly IInspectionBookingManager _inspBookingManager = null;
        private readonly IInspectionBookingRepository _inspBookingRepo = null;
        private readonly ISupplierRepository _supplierRepository;
        private IUserAccountManager _userAccountManager = null;
        private ICustomerCheckPointManager _customercheckmanager = null;
        private InspectionCertificateMap _inspcertificatemap = null;
        private ITenantProvider _filterService = null;
        private readonly IUserConfigRepository _userConfigRepo = null;
        public InspectionCertificateManager(IInspectionCertificateRepository repo, ICustomerManager customerManager, ISupplierRepository supplierRepository,
            IAPIUserContext applicationContext, ICustomerCheckPointManager ccpManager, IInspectionCustomerDecisionManager cdManager,
            IInspectionBookingManager inspBookingManager, IUserAccountManager userAccountManager, ICustomerCheckPointManager customercheckmanager,
            ITenantProvider filterService, IUserConfigRepository userConfigRepo, IInspectionBookingRepository inspBookingRepo)
        {
            _repo = repo;
            _ApplicationContext = applicationContext;
            _customerManager = customerManager;
            _ccpManager = ccpManager;
            _cdManager = cdManager;
            _inspBookingManager = inspBookingManager;
            _userAccountManager = userAccountManager;
            _customercheckmanager = customercheckmanager;
            _inspcertificatemap = new InspectionCertificateMap();
            _filterService = filterService;
            _userConfigRepo = userConfigRepo;
            _inspBookingRepo = inspBookingRepo;
            _supplierRepository = supplierRepository;
        }
        public async Task<InspectionCertificateResponse> SaveIC(InspectionCertificateRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (request.Id == 0) //add new IC
                    {
                        var entity = ICRequestEntityMap(request);
                        ICRequestEntityProdcutMap(request, entity);

                        int id = await _repo.SaveIC(entity);
                        if (id > 0)
                        {
                            //generate IC / GL number
                            entity.Icno = entity.IctitleId == (int)InspectionCertificateTitle.InspectionCertificate ? "IC-" + id
                                                                : entity.IctitleId == (int)InspectionCertificateTitle.GreenLight ? "GL-" + id : id.ToString();
                            _repo.Save(entity, true);
                            return new InspectionCertificateResponse() { id = id, Result = InspectionCertificateResult.Success };
                        }
                        else
                        {
                            return new InspectionCertificateResponse() { Result = InspectionCertificateResult.Failure };
                        }
                    }
                    else if (request.Id > 0) //Edit IC
                    {

                        var entity = await _repo.GetICDetailsProductDetails(request.Id);
                        if (entity != null)
                        {
                            entity.CustomerId = request?.CustomerId;
                            entity.SupplierId = request?.SupplierId;
                            entity.SupplierAddress = request?.SupplierAddress;
                            entity.ApprovalDate = request?.ApprovalDate?.ToDateTime();
                            entity.BeneficiaryName = request?.BeneficiaryName;
                            entity.IctitleId = request.ICTitleId;
                            entity.Comment = request?.Comment?.Trim();
                            entity.UpdatedBy = _ApplicationContext.UserId;
                            entity.UpdatedOn = DateTime.Now;
                            entity.BuyerName = request?.BuyerName.Trim();

                            if (entity.InspIcTranProducts != null && entity.InspIcTranProducts.Count() > 0)
                                _repo.RemoveEntities(entity.InspIcTranProducts);

                            ICRequestEntityProdcutMap(request, entity);

                            _repo.EditEntity(entity);

                            await _repo.Save();
                        }

                        return new InspectionCertificateResponse() { id = request.Id, Result = InspectionCertificateResult.Success };
                    }
                    else
                        return new InspectionCertificateResponse() { Result = InspectionCertificateResult.Failure };
                }
                else
                    return new InspectionCertificateResponse() { Result = InspectionCertificateResult.Failure };
            }
            catch (Exception ex)
            {
                throw ex;
                //return new InspectionCertificateResponse() { Result = InspectionCertificateResult.Failure };
            }
        }
        private InspIcTransaction ICRequestEntityMap(InspectionCertificateRequest request)
        {
            InspIcTransaction entity = new InspIcTransaction()
            {
                Id = request.Id,
                CustomerId = request?.CustomerId,
                SupplierId = request?.SupplierId,
                SupplierAddress = request?.SupplierAddress?.Trim(),
                ApprovalDate = request?.ApprovalDate?.ToDateTime(),
                BeneficiaryName = request?.BeneficiaryName?.Trim(),
                IctitleId = request.ICTitleId,
                Icstatus = (int)InspectionCertificateStatus.Created, //later status may change
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                Comment = request?.Comment?.Trim(),
                BuyerName = request?.BuyerName.Trim(),
                EntityId = _filterService.GetCompanyId()
            };
            return entity;
        }
        private void ICRequestEntityProdcutMap(InspectionCertificateRequest request, InspIcTransaction entity)
        {
            if (request.ICBookingList != null)
            {
                foreach (var bookingICItem in request.ICBookingList)
                {
                    if (bookingICItem.BookingProductId > 0)
                    {
                        var inspIcTranProduct = new InspIcTranProduct()
                        {
                            Active = true,
                            CreatedBy = _ApplicationContext.UserId,
                            BookingProductId = bookingICItem.BookingProductId,
                            ShipmentQty = bookingICItem.ShipmentQty,
                            PoColorId = bookingICItem.PoColorId
                        };
                        entity.InspIcTranProducts.Add(inspIcTranProduct);
                        _repo.AddEntity(inspIcTranProduct);
                    }
                }
            }
        }

        public async Task<ICBookingResponse> GetInspectionICData(ICBookingSearchRequest bookingRequest)
        {
            try
            {
                if (bookingRequest.Index == null || bookingRequest.Index.Value <= 0)
                    bookingRequest.Index = 1;

                if (bookingRequest.pageSize == null || bookingRequest.pageSize.Value == 0)
                    bookingRequest.pageSize = 20;

                int skip = (bookingRequest.Index.Value - 1) * bookingRequest.pageSize.Value;

                int take = bookingRequest.pageSize.Value;

                //filter data based on user type
                switch (_ApplicationContext.UserType)
                {
                    case UserTypeEnum.Customer:
                        {
                            bookingRequest.CustomerId = bookingRequest?.CustomerId == null || bookingRequest?.CustomerId == 0 ? _ApplicationContext.CustomerId : bookingRequest?.CustomerId;
                            break;
                        }
                    case UserTypeEnum.Supplier:
                        {
                            bookingRequest.SupplierId = bookingRequest?.SupplierId == null || bookingRequest?.SupplierId == 0 ? _ApplicationContext.SupplierId : bookingRequest.SupplierId;
                            break;
                        }
                }

                var cuslist = new List<int>();
                if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                {
                    if (bookingRequest?.CustomerId != null)
                        cuslist.Add(bookingRequest.CustomerId.Value);
                }
                else
                {
                    if (Convert.ToBoolean(bookingRequest?.CustomerId.HasValue))
                        cuslist.Add(bookingRequest.CustomerId.Value);
                }
                //get all booking customer decision =true + specific date range or booking id
                var cusdecpassbookno = await _repo.GetInspCusDecision(bookingRequest);

                if (cusdecpassbookno == null || !cusdecpassbookno.Any())
                    return new ICBookingResponse() { Result = ICBookingSearchResult.NoDataFound };

                //get all booking related data for filter
                var bookingDataRepo = _repo.GetInspectionICData(bookingRequest);

                if (bookingDataRepo == null)
                    return new ICBookingResponse() { Result = ICBookingSearchResult.Failure };

                //fetch & filter the ic required check point 
                var culist = cusdecpassbookno.Select(x => x.CustomerId).Distinct().ToList();
                var cucheckpoint = await _customercheckmanager.GetCheckPointList(culist, (int)Service.InspectionId, new List<int>() { (int)CheckPointTypeEnum.ICRequired });

                if (cucheckpoint == null || !cucheckpoint.Any())
                    return new ICBookingResponse() { Result = ICBookingSearchResult.NoDataFound };

                var checkPointIds = cucheckpoint.Select(x => x.Id).Distinct().ToList();

                var checkPointBrandData = await _customercheckmanager.GetCheckPointBrandList(checkPointIds);

                var checkpointBrandIds = checkPointBrandData.Select(x => x.Id).ToList();

                //check if the booking brand has checkpoint configured
                if (checkPointBrandData != null && checkPointBrandData.Any())
                {
                    //get the booking brands
                    bookingDataRepo = bookingDataRepo.Where(x => x.InspTranCuBrands.Any(y => checkpointBrandIds.Contains(y.BrandId)));
                }

                //check if the booking dept has checkpoint configured
                var checkPointDeptData = await _customercheckmanager.GetCheckPointDeptList(checkPointIds);

                var checkpointDeptIds = checkPointDeptData.Select(x => x.Id).ToList();

                if (checkPointDeptData != null && checkPointDeptData.Any())
                {
                    //get the booking depts
                    bookingDataRepo = bookingDataRepo.Where(x => x.InspTranCuDepartments.Any(y => checkpointDeptIds.Contains(y.DepartmentId)));
                }

                //check if the booking service Type has checkpoint configured
                var checkPointServiceTypeData = await _customercheckmanager.GetCheckPointServiceTypeList(checkPointIds);

                var checkpointServiceIds = checkPointServiceTypeData.Select(x => x.Id).ToList();

                if (checkPointServiceTypeData != null && checkPointServiceTypeData.Any())
                {
                    //get the booking service type
                    bookingDataRepo = bookingDataRepo.Where(x => x.InspTranServiceTypes.Any(y => checkpointServiceIds.Contains(y.ServiceTypeId)));
                }

                //apply filter
                if (bookingRequest != null && cuslist != null && cuslist.Any())
                {
                    bookingDataRepo = bookingDataRepo.Where(x => cuslist.Contains(x.CustomerId));
                }

                var cusBookingIds = cusdecpassbookno.Select(y => y.BookingId).ToList();

                bookingDataRepo = bookingDataRepo.Where(x => cusBookingIds.Contains(x.Id));

                if (bookingRequest.SupplierId > 0)
                {
                    bookingDataRepo = bookingDataRepo.Where(x => x.SupplierId == bookingRequest.SupplierId);
                }

                var bookingIdQuery = bookingDataRepo.Select(x => x.Id);

                //get issue or partial bookingids list
                var issueICBookingIdList = await _repo.GetIssuedICBookingIdList(bookingIdQuery);

                //get partial booking id list
                var partialICBookingIdList = await _repo.GetPartialIssueICBookingIdList(bookingIdQuery);

                //select booking ids
                var issueBookingIds = issueICBookingIdList.Select(x => x.BookingId).ToList();

                //select booking ids
                var partialBookingIds = partialICBookingIdList.Select(x => x.BookingId).ToList();

                //execute booking id list
                var bookingIdList = await bookingIdQuery.ToListAsync();

                //get overall bookingids except issued one
                var nonIssueICBookingIdList = bookingIdList.Except(issueBookingIds).ToList();

                //concat the non issue and partial booking id list
                var finalBookingIdList = nonIssueICBookingIdList.Concat(partialBookingIds).Distinct().ToList();

                //filter the booking ids
                bookingDataRepo = bookingDataRepo.Where(x => finalBookingIdList.Contains(x.Id));

                var bookingdatafilter = await bookingDataRepo.Skip(skip).Take(take)
                    .Select(x => new ICBookingSearchRepoResponse
                    {
                        Customer = x.Customer,
                        SupplierName = x.Supplier.SupplierName,
                        BookingNumber = x.Id,
                        FactoryName = x.Factory.SupplierName,
                        ServiceFromDate = x.ServiceDateFrom,
                        ServiceToDate = x.ServiceDateTo,
                        CustomerId = x.CustomerId,
                        SupplierId = x.SupplierId,
                        BookingStatus = x.Status.Status,
                        BusinessLine = x.BusinessLine
                    })
                    .ToListAsync();

                if (bookingdatafilter == null || !bookingdatafilter.Any())
                    return new ICBookingResponse() { Result = ICBookingSearchResult.NoDataFound };

                //check the total ic count
                var dataCount = await bookingDataRepo.Select(x => x.Id).Distinct().CountAsync();

                if (dataCount == 0)
                    return new ICBookingResponse() { Result = ICBookingSearchResult.NoDataFound };

                return await BookingSearchMap(bookingdatafilter, dataCount, bookingRequest);


            }
            catch (Exception)
            {
                return new ICBookingResponse() { Result = ICBookingSearchResult.Failure };
            }
        }

        private async Task<ICBookingResponse> BookingSearchMap(List<ICBookingSearchRepoResponse> bookingList, int dataCount, ICBookingSearchRequest request)
        {
            var customerIds = bookingList.Select(x => x.CustomerId).Distinct().ToList();
            var checkpointIds = new[] { (int)CheckPointTypeEnum.CustomerDecisionRequired };

            //get all  booking
            var lstbookingId = bookingList.Select(x => x.BookingNumber).Distinct().ToList();

            //get service type details for booking id list
            var serviceTypeList = await _inspBookingManager.GetServiceTypeList(lstbookingId);

            var businessLine = bookingList.FirstOrDefault().BusinessLine;

            List<ICBookingSearchProductResponse> productList = null;
            if (businessLine == (int)BusinessLine.HardLine)
            {
                //get booking product list
                productList = await _repo.GetBookingProductList(lstbookingId);
            }
            else
            {
                productList = await _repo.GetBookingProductListSoftline(lstbookingId);
            }

            if (productList == null || !productList.Any())
                return new ICBookingResponse() { Result = ICBookingSearchResult.Failure };

            //get all ic list
            var lstinspotranIds = productList.Where(x => x.InspPOTransactionId.HasValue).Select(x => x.InspPOTransactionId.Value).Distinct().ToList();

            var icProductList = await _repo.GetICProducts(lstinspotranIds);

            //get all report list
            var lstreportids = productList.Where(x => x.FBReportId.HasValue).Select(x => x.FBReportId.Value).Distinct().ToList();

            var fbReportList = await _repo.GetProductFBList(lstreportids);

            //get all customer list which have CustomerDecisionRequired checkpoints
            var customerCheckPointList = await _ccpManager.GetCheckPointList(customerIds, (int)Service.InspectionId, checkpointIds);

            // get all customer descision list based on fb report ids
            var customerDecisionList = await _cdManager.GetCustomerDescistionWithReportId(lstreportids);

            //get result after map
            var DataResult = _inspcertificatemap.BookingSearchIC(bookingList, productList, fbReportList, icProductList, customerDecisionList, customerCheckPointList,
                                                serviceTypeList, _ApplicationContext?.RoleList);

            if (!DataResult.Any())
                return new ICBookingResponse() { Result = ICBookingSearchResult.NoDataFound };

            return new ICBookingResponse()
            {
                BookingList = DataResult,
                Result = ICBookingSearchResult.Success,
                TotalCount = dataCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (dataCount / request.pageSize.Value) + (dataCount % request.pageSize.Value > 0 ? 1 : 0),
            };
        }
        public async Task<EditInspectionCertificateResponse> EditICDetails(int id)
        {
            try
            {
                //get IC details By Id
                var icMainDetails = await _repo.GetICDetails(id);

                if (icMainDetails == null)
                    return new EditInspectionCertificateResponse() { Result = InspectionCertificateResult.NoDataFound };

                icMainDetails.ICBookingList = new List<InspectionCertificateBookingRequest>();

                //get IC product details
                var productList = await _repo.GetICProductDetails(id);

                //get IC reamining qty 
                var lstICRemaingQty = await GetProductRemainingQty(productList.Select(x => x.BookingProductId).Distinct().ToList());

                icMainDetails.ICBookingList = _inspcertificatemap.RemainingQtyMap(productList, lstICRemaingQty);

                return new EditInspectionCertificateResponse()
                {
                    editInspectionCertificate = icMainDetails,
                    Result = InspectionCertificateResult.Success
                };
            }
            catch (Exception ex)
            {
                //return new EditInspectionCertificateResponse() { Result = InspectionCertificateResult.Failure };
                throw ex;
            }
        }
        public async Task<InspectionCertificateResponse> CancelICDetails(int id)
        {
            try
            {
                if (id > 0)
                {
                    //get IC details
                    var icEntity = await _repo.GetICDetailsProductDetails(id);

                    icEntity.Icstatus = (int)InspectionCertificateStatus.Cancel;
                    icEntity.UpdatedBy = _ApplicationContext.UserId;
                    icEntity.UpdatedOn = DateTime.Now;

                    productCancel(icEntity.InspIcTranProducts.ToList(), icEntity);

                    _repo.EditEntity(icEntity);

                    await _repo.Save();

                    return new InspectionCertificateResponse() { id = id, Result = InspectionCertificateResult.Success };
                }
                else
                    return new InspectionCertificateResponse() { Result = InspectionCertificateResult.RequestNotCorrectFormat };
            }
            catch (Exception ex)
            {
                //return new InspectionCertificateResponse() { Result = InspectionCertificateResult.Failure};
                throw ex;
            }
        }
        private void productCancel(List<InspIcTranProduct> icProductList, InspIcTransaction entity)
        {
            foreach (var productItem in icProductList)
            {
                productItem.Active = false;
                productItem.UpdatedBy = _ApplicationContext.UserId;
                productItem.UpdatedOn = DateTime.Now;
                _repo.EditEntity(productItem);
            }
        }
        public async Task<InspectionCertificatePDF> GetICPreviewDetails(int id, bool isDraft)
        {
            try
            {
                if (id > 0)
                {
                    var icDetails = await _repo.GetICPDFDetails(id);
                    icDetails.ProductDetails = await _repo.GetICPDFProductDetails(id);
                    var bookingId = icDetails?.ProductDetails?.FirstOrDefault()?.BookingNumber;
                    if (bookingId.HasValue)
                    {
                        var booking = await _inspBookingRepo.GetAllInspectionsQuery().AsNoTracking().FirstOrDefaultAsync(x => x.Id == bookingId.Value);
                        if (booking != null)
                        {
                            var factoryAddress = await _supplierRepository.GetSupplierHeadOfficeAddress(booking.FactoryId.GetValueOrDefault());
                            if (factoryAddress != null)
                            {
                                icDetails.FactoryCountryId = factoryAddress.countryId;
                            }
                        }
                    }

                    icDetails.IsDraft = isDraft;
                    icDetails.EntityMasterConfigs = await _userConfigRepo.GetMasterConfiguration();
                    return icDetails;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICSummarySearchResponse> GetICSummaryDetails(ICSummarySearchRequest request)
        {

            if (request == null)
                return new ICSummarySearchResponse() { Result = ICSummarySearchResponseResult.NotFound };


            //filter data based on user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.CustomerId = request?.CustomerId == null ? _ApplicationContext.CustomerId : 0;
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.SupplierId = request.SupplierId == null ? _ApplicationContext.SupplierId : 0;
                        break;
                    }
            }

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var cuslist = new List<int>();
            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
            {
                if (request?.CustomerId == null)
                {
                    var customerresponse = await _customerManager.GetCustomersByUserId(_ApplicationContext.StaffId);
                    if (customerresponse != null && customerresponse.Count() != 0)
                        cuslist.AddRange(customerresponse.Select(x => x.Id));
                }
                else if (request?.CustomerId != null)
                    cuslist.Add(request.CustomerId.Value);
            }
            else
            {
                if (request?.CustomerId != null)
                    cuslist.Add(request.CustomerId.Value);
            }

            //get data
            var icinspectiondetails = _repo.GetICInspectionTransactions();

            if (request != null && cuslist != null && cuslist.Count() > 0)
            {
                icinspectiondetails = icinspectiondetails.Where(x => cuslist.Contains(x.customerId));
            }

            if (request != null && request.SupplierId != 0 && request.SupplierId != null)
            {
                icinspectiondetails = icinspectiondetails.Where(x => x.SupplierId == request.SupplierId);
            }

            if (request != null && request.StatusIdlst != null && request.StatusIdlst.Count() > 0)
            {
                icinspectiondetails = icinspectiondetails.Where(x => request.StatusIdlst.ToList().Contains(x.StatusId));
            }

            if (Enum.TryParse(request.SearchTypeId.ToString(), out ICSummarySearchType _seachtypeenum))
            {
                switch (_seachtypeenum)
                {
                    case ICSummarySearchType.BookingNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int bookid))
                            {
                                icinspectiondetails = icinspectiondetails.Where(x => x.BookingNo == bookid);
                            }
                            break;
                        }

                    //case ICSummarySearchType.PoNo:
                    //    {
                    //        if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
                    //        {
                    //            icinspectiondetails = icinspectiondetails.Where(x => EF.Functions.Like(x.PoNo, $"%{request.SearchTypeText.Trim()}%"));
                    //        }
                    //        break;
                    //    }
                    case ICSummarySearchType.IcNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()))
                            {
                                icinspectiondetails = icinspectiondetails.Where(x => EF.Functions.Like(x.ICNo, $"%{request.SearchTypeText.Trim()}%"));
                            }
                            break;
                        }

                }
                if (Enum.TryParse(request.DateTypeid.ToString(), out ICDataSearchType _datesearchtype))
                {
                    switch (_datesearchtype)
                    {
                        case ICDataSearchType.ApplyDate:
                            {
                                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                                {
                                    // icinspectiondetails = icinspectiondetails.Where(x => x.ICCreatedDate >= (request.FromDate.ToDateTime()) && x.ICCreatedDate <= (request.ToDate.ToDateTime()));
                                    icinspectiondetails = icinspectiondetails.Where(x => EF.Functions.DateDiffDay(request.FromDate.ToDateTime(), x.ICCreatedDate) >= 0 &&
                                                    EF.Functions.DateDiffDay(x.ICCreatedDate, request.ToDate.ToDateTime()) >= 0);
                                }
                                break;
                            }
                        case ICDataSearchType.ServiceDate:
                            {
                                if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
                                {
                                    icinspectiondetails = icinspectiondetails.Where(x => !(x.ServiceDateFrom > (request.ToDate.ToDateTime()) || x.ServiceDateTo < (request.FromDate.ToDateTime())));
                                }
                                break;
                            }
                    }
                }
            }
            var filterDistinctICList = await icinspectiondetails.Select(x => x.ICId).Distinct().ToListAsync();

            var icList = filterDistinctICList.Skip(skip).Take(take).ToList();

            var groupByICDetails = await icinspectiondetails.Where(x => icList.Contains(x.ICId)).ToListAsync();

            //total ic count
            var datacount = filterDistinctICList.Count();

            if (datacount == 0)
                return new ICSummarySearchResponse() { Result = ICSummarySearchResponseResult.NotFound };

            var IcStatusitems = new List<IcStatus>();
            IcStatusitems = await icinspectiondetails.Select(x => new { x.StatusId, x.StatusName, x.BookingNo }).
                GroupBy(y => new { y.StatusName, y.StatusId }, y => y.BookingNo, (key, data) => new IcStatus()
                {
                    Id = key.StatusId,
                    StatusName = key.StatusName,
                    TotalCount = data.Distinct().Count(),
                    StatusColor = ICStatusColor.GetValueOrDefault(key.StatusId, "")
                }).ToListAsync();

            //get all filter data
            var filterIcDate = await icinspectiondetails.Where(x => icList.Contains(x.ICId)).ToListAsync();

            var Mapdata = _inspcertificatemap.ICSummarySearchMap(filterIcDate);

            try
            {
                return new ICSummarySearchResponse()
                {
                    Result = ICSummarySearchResponseResult.Success,
                    TotalCount = datacount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (datacount / request.pageSize.Value) + (datacount % request.pageSize.Value > 0 ? 1 : 0),
                    IcStatusList = IcStatusitems,
                    Data = Mapdata,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ICSummaryProducts> GetICSummaryProducts(int icID)
        {
            //get ic details
            var inspCertificateDetails = _repo.GetInspectionCertificateDetails(icID);
            //get all insp po ids
            var inspPoTransactionIds = inspCertificateDetails.SelectMany(x => x.InspIcTranProducts).Where(x => x.BookingProductId.HasValue).Select(x => x.BookingProductId.Value).Distinct().ToList();
            //get all the products of IC
            var icSummaryProducts = _repo.GetICSummaryProducts(inspPoTransactionIds, icID).ToList();

            return icSummaryProducts;

        }

        public ICStatusResponse GetICStatus()
        {
            var response = new ICStatusResponse();
            var data = _repo.GetICStatus().Result;
            if (data == null)
                response = new ICStatusResponse { Result = ICStatusResponseResult.NotFound };
            if (data != null)
            {
                var icStatusList = data.Select(x => _inspcertificatemap.GetICStatusList(x)).ToList();
                response = new ICStatusResponse { ICStatusList = icStatusList, Result = ICStatusResponseResult.Success };
            }

            return response;
        }
        public async Task<List<QuantityPoId>> GetProductRemainingQty(List<int> inspPoTransactionId)
        {
            try
            {
                var productICQtyList = await _repo.GetICProductQty(inspPoTransactionId);
                var fbPresentedQtyList = await _repo.GetFBPresentedQty(inspPoTransactionId);

                if (fbPresentedQtyList != null && fbPresentedQtyList.Count() > 0)
                {
                    return _inspcertificatemap.RemainingQtyCalculationMap(productICQtyList, fbPresentedQtyList);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ICTitleResponse> GetICTitleList()
        {
            try
            {
                var icTitleResponse = new ICTitleResponse();
                icTitleResponse.DropDownList = new List<DropDown>();

                icTitleResponse.DropDownList = await _repo.GetICTitleList();

                if (icTitleResponse.DropDownList == null || icTitleResponse.DropDownList.Count == 0)
                    icTitleResponse.Result = DropdownResult.CannotGetList;
                else
                    icTitleResponse.Result = DropdownResult.Success;

                return icTitleResponse;
            }
            catch (Exception ex)
            {
                return new ICTitleResponse() { Result = DropdownResult.Failed };
                //throw ex;
            }
        }
        // get booking ic product list
        public async Task<ICBookingProductResponse> BookingICProduct(ICBookingProductRequest request)
        {
            if (request == null)
                return new ICBookingProductResponse() { Result = ICBookingSearchResult.BookingNosRequired };
            var bookingId = request.BookingIdList.FirstOrDefault();
            //get all products for booking
            var booking = await _inspBookingRepo.GetAllInspectionsQuery().AsNoTracking().FirstOrDefaultAsync(x => x.Id == bookingId);

            List<ICBookingSearchProductResponse> productList = null;
            if (booking.BusinessLine == (int)BusinessLine.HardLine)
            {
                productList = await _repo.GetBookingProductList(request.BookingIdList.ToList());
            }
            else if (booking.BusinessLine == (int)BusinessLine.SoftLine)
            {
                productList = await _repo.GetBookingProductListSoftline(request.BookingIdList.ToList());
            }
            if (productList == null || !productList.Any())
                return new ICBookingProductResponse() { Result = ICBookingSearchResult.Failure };
            var selectedProductList = productList.Where(x => x.InspPOTransactionId.HasValue &&
                                                request.ProductIdList.ToList().Contains(x.InspPOTransactionId.Value)).ToList();

            var icProductList = await _repo.GetICProducts(request.ProductIdList.ToList());

            //get all report list
            var lstreportids = productList.Where(x => x.FBReportId.HasValue).Select(x => x.FBReportId.Value).Distinct().ToList();

            var fbReportList = await _repo.GetProductFBList(lstreportids);

            //get result after map            
            var DataResult = _inspcertificatemap.BookingProductMapIC(request.BookingIdList.ToList(), booking.BusinessLine.GetValueOrDefault(), selectedProductList, fbReportList, icProductList);

            return new ICBookingProductResponse()
            {
                ProductBookingList = DataResult,
                Result = ICBookingSearchResult.Success
            };
        }
    }
}
