using AutoMapper;
using BI.Cache;
using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.CustomerProducts;
using DTO.HumanResource;
using DTO.ProductManagement;
using DTO.Quotation;
using DTO.References;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BI
{
    public class ReferenceManager : ApiCommonData, IReferenceManager
    {
        #region Declaration 
        private IReferenceRepository _referenceRepository = null;
        private readonly IHumanResourceRepository _humanResourceRepository;
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IMapper _mapper;
        private ICacheManager _cache = null;
        private readonly ReferenceMap ReferenceMap = null;
        private readonly LocationMap LocationMap = null;
        private readonly QuotationMap QuotationMap = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        #endregion Declaration

        #region Constructor
        public ReferenceManager(IReferenceRepository referenceRepository, IAPIUserContext ApplicationContext,
            IHumanResourceRepository humanResourceRepository, IMapper mapper, ICacheManager cache, IInvoiceRepository invoiceRepository)
        {
            _referenceRepository = referenceRepository;
            _humanResourceRepository = humanResourceRepository;
            _mapper = mapper;
            _cache = cache;
            ReferenceMap = new ReferenceMap();
            LocationMap = new LocationMap();
            QuotationMap = new QuotationMap();
            _ApplicationContext = ApplicationContext;
            _invoiceRepository = invoiceRepository;
        }
        #endregion Constructor

        #region methods

        public async Task<IEnumerable<Season>> GetSeasons()
        {
            var data = await _referenceRepository.GetSeasons();
            if (data == null || data.Count == 0)
                return null;
            return data.Select(ReferenceMap.GetSeason);
        }

        public async Task<SeasonYearResponse> GetSeasonsYear()
        {
            var response = new SeasonYearResponse();
            var data = await _referenceRepository.GetSeasonsYear();
            if (data == null || data.Count == 0)
                return new SeasonYearResponse() { Result = SeasonYearResponseResult.error };
            response.SeasonYearList = data.Select(ReferenceMap.GetSeasonYear);
            response.Result = SeasonYearResponseResult.success;
            return response;
        }

        public async Task<IEnumerable<Unit>> GetUnits()
        {
            var data = await _referenceRepository.GetUnits();
            if (data == null || data.Count == 0)
                return null;
            return data.Select(ReferenceMap.GetUnit);
        }


        public async Task<ProductCategoryResponse> GetProductCategories()
        {
            ProductCategoryResponse response = new ProductCategoryResponse();
            var data = await _referenceRepository.GetProductCategories();
            if (data == null || data.Count == 0)
            {
                response.ProductCategoryList = null;
                response.Result = ProductManagementResult.CannotGetProductManagementList;
            }
            else
            {
                response.ProductCategoryList = data.Select(x => _mapper.Map<ProductCategory>(x)).OrderBy(x => x.Name);
                response.Result = ProductManagementResult.Success;
            }
            return response;
        }

        public async Task<ProductSubCategoryResponse> GetProductSubCategories(int productCategoryID)
        {
            ProductSubCategoryResponse response = new ProductSubCategoryResponse();
            var data = await _referenceRepository.GetProductSubCategories(productCategoryID);
            if (data == null || data.Count == 0)
            {
                response.ProductSubCategoryList = null;
                response.Result = ProductManagementResult.CannotGetProductManagementList;
            }
            else
            {
                response.ProductSubCategoryList = data.Select(x => _mapper.Map<ProductSubCategory>(x)).OrderBy(x => x.Name);
                response.Result = ProductManagementResult.Success;
            }
            return response;
        }

        public async Task<ProductCategorySub2Response> GetProductCategorySub2(int? productSubCategoryID)
        {
            ProductCategorySub2Response response = new ProductCategorySub2Response();
            var data = await _referenceRepository.GetProductCategorySub2(productSubCategoryID);
            if (data == null || data.Count == 0)
            {
                response.ProductCategorySub2List = null;
                response.Result = ProductCategorySub2Result.CannotGetProductCategorySub2;
            }
            else
            {
                response.ProductCategorySub2List = data.Select(x => _mapper.Map<ProductCategorySub2>(x)).OrderBy(x => x.Name);
                response.Result = ProductCategorySub2Result.Success;
            }
            return response;
        }

        public async Task<RefCurrency> GetCurrencyData(string currencyCode)
        {
            return await _referenceRepository.GetCurrencyDataByCode(currencyCode);
        }

        public async Task<CuCustomer> GetCustomerData(int customerId)
        {
            return await _referenceRepository.GetCustomerData(customerId);
        }

        public IEnumerable<Currency> GetCurrencies()
        {

            var currencies = _cache.CacheTryGetValueSet(CacheKeys.AllCurrencies,
            () => _referenceRepository.GetCurrencies().ToArray());

            if (currencies == null || !currencies.Any())
                return null;

            return currencies.Select(LocationMap.GetCurrency).ToArray();
        }

        public async Task<DataSourceResponse> GetCurrencyList()
        {

            var currencies = _cache.CacheTryGetValueSet(CacheKeys.AllCurrencies,
            () => _referenceRepository.GetCurrencies().ToArray());

            if (currencies != null)
            {
                var currencyList = currencies.Select(ReferenceMap.GetCurrencyMap);

                return new DataSourceResponse()
                {
                    DataSourceList = currencyList,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new DataSourceResponse()
                {
                    DataSourceList = null,
                    Result = DataSourceResult.CannotGetList
                };
            }
        }

        public async Task<CurrencyDataSourceResponse> GetCurrencyListWithCurrencyCode()
        {

            var currencies = _cache.CacheTryGetValueSet(CacheKeys.AllCurrencies,
            () => _referenceRepository.GetCurrencies().ToArray());

            if (currencies != null)
            {
                var currencyList = currencies.Select(ReferenceMap.GetCurrencyCodeMap);

                return new CurrencyDataSourceResponse()
                {
                    DataSourceList = currencyList,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new CurrencyDataSourceResponse()
                {
                    DataSourceList = null,
                    Result = DataSourceResult.CannotGetList
                };
            }
        }


        public async Task<IEnumerable<DTO.References.Service>> GetServices()
        {

            var data = await _referenceRepository.GetServices();

            if (data == null || !data.Any())
                return null;

            return data.Select(ReferenceMap.GetService);

        }

        /// <summary>
        /// get service list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetServiceDataList()
        {
            var response = new DataSourceResponse();

            var serviceList = await GetServices();

            if (!serviceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.Success };

            response.DataSourceList = serviceList.Select(x => ReferenceMap.GetServiceData(x));

            response.Result = DataSourceResult.Success;

            return response;
        }


        public async Task<QuotationDataSourceResponse> GetServiceTypeList(int customerId, int serviceId)
        {
            var data = await _referenceRepository.GetServiceTypeList(customerId, serviceId);

            if (data == null || !data.Any())
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.NotFound };


            return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.Success, DataSource = data.Select(QuotationMap.GetDataSource) };
        }
        //get service type list from refservicetype table
        public async Task<DataSourceResponse> GetServiceList()
        {
            var data = await _referenceRepository.GetServiceTypes();

            if (data != null)
            {
                var serviceList = data.Select(ReferenceMap.GetServiceTypeMap);

                return new DataSourceResponse()
                {
                    DataSourceList = serviceList,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new DataSourceResponse()
                {
                    DataSourceList = null,
                    Result = DataSourceResult.CannotGetList
                };
            }
        }
        //get service type list from refservicetype table
        public async Task<DataSourceResponse> GetServiceTypeListByCusService(int customerId, int serviceId)
        {
            var data = await _referenceRepository.GetServiceTypeList(customerId, serviceId);

            if (data != null)
            {
                var serviceList = data.Select(ReferenceMap.GetServiceTypeMap);

                return new DataSourceResponse()
                {
                    DataSourceList = serviceList,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new DataSourceResponse()
                {
                    DataSourceList = null,
                    Result = DataSourceResult.CannotGetList
                };
            }
        }

        //get list from QuBillMethods table
        public async Task<DataSourceResponse> GetBillingMethodList()
        {
            var data = await _referenceRepository.GetBillingMethodList();

            var featureList = await _referenceRepository.GetEntityFeatureList();

            if (!featureList.Any())
            {
                // take only manday and sampling
                data = data.OrderBy(x => x.Id).Take(2).ToList();
            }
            if (featureList.Any() && !featureList.Any(x => x.FeatureId == (int)Entities.Enums.EntityFeature.MandayComplex)
                && !featureList.Any(x => x.FeatureId == (int)Entities.Enums.EntityFeature.SamplingComplex))
            {
                data = data.OrderBy(x => x.Id).Take(2).ToList();
            }

            if (data != null)
            {
                return new DataSourceResponse()
                {
                    DataSourceList = data,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new DataSourceResponse()
                {
                    DataSourceList = null,
                    Result = DataSourceResult.CannotGetList
                };
            }
        }

        //get list form QuPaidBies table
        public async Task<DataSourceResponse> GetBillingToList()
        {
            var data = await _referenceRepository.GetBillingToList();

            if (data != null)
            {

                return new DataSourceResponse()
                {
                    DataSourceList = data,
                    Result = DataSourceResult.Success
                };
            }
            else
            {
                return new DataSourceResponse()
                {
                    DataSourceList = null,
                    Result = DataSourceResult.CannotGetList
                };
            }
        }


        /// <summary>
        /// Get custom Sample size list
        /// </summary>
        /// <returns></returns>
        public async Task<CustomSampleSizeResponse> GetCustomSampleSizeList()
        {
            CustomSampleSizeResponse response = new CustomSampleSizeResponse();
            try
            {
                response.DataSourceList = await _referenceRepository.GetCustomSampleSizeList();
                response.Result = CustomSampleSizeResult.Success;
            }
            catch (Exception)
            {
                response.Result = CustomSampleSizeResult.Failed;
            }
            return response;
        }

        #endregion methods

        /// <summary>
        /// Get the API Service List
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetAPIServices()
        {
            var response = new DataSourceResponse();

            var data = await _referenceRepository.GetAPIServices();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get billing entity list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBillingEntityList()
        {
            var response = new DataSourceResponse();

            var data = await _referenceRepository.GetBillingEntityList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get invoice request type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceRequestTypeList()
        {
            var response = new DataSourceResponse();

            var data = await _referenceRepository.GetInvoiceRequestTypeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get invoice bank list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceBankList(int? billingEntity)
        {
            var response = new DataSourceResponse();

            var bankList = await _referenceRepository.GetInvoiceBankList(billingEntity);
            var data = bankList.Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = "[" + x.CurrencyCode + "] " + x.BankName + (x.Remarks != null ? " - " + (x.Remarks.Length > 15 ? x.Remarks.Substring(0, 15) : x.Remarks) : String.Empty),
            });
            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        public async Task<IEnumerable<CommonBankDataSource>> GetBankList(int? billingEntity)
        {
            var bankData = await _referenceRepository.GetBankList(billingEntity);
            var bankList = bankData.Select(x => new CommonBankDataSource
            {
                Id = x.Id,
                Name = "[" + x.CurrencyCode + "] " + x.BankName + (x.Remarks != null ? " - " + (x.Remarks.Length > 15 ? x.Remarks.Substring(0, 15) : x.Remarks) : String.Empty),
                CurrencyId = x.CurrencyId
            }).ToList();

            if (bankList.Any())
            {
                var bankIdList = bankList.Select(x => x.Id).ToList();

                var taxList = await _invoiceRepository.GetBankTaxDetails(bankIdList);

                foreach (var item in bankList)
                {
                    item.TaxList = taxList.Where(x => x.BankId == item.Id).ToList();
                }
            }

            return bankList;
        }



        /// <summary>
        /// Get invoice Fees types list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceFeesTypeList()
        {
            var response = new DataSourceResponse();

            var data = await _referenceRepository.GetInvoiceFeesTypeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get Invoice Office list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceOfficeList()
        {
            var response = new DataSourceResponse();

            var data = await _referenceRepository.GetInvoiceOfficeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }
        /// <summary>
        /// Get invoice payment type list
        /// </summary>
        /// <returns></returns>
        public async Task<PaymentTypeResponse> GetInvoicePaymentTypeList()
        {
            var response = new PaymentTypeResponse();

            var data = await _referenceRepository.GetInvoicePaymentTypeList();

            if (data != null)
            {
                return new PaymentTypeResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new PaymentTypeResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the product category source list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetProductCategorySourceList(ProductCategoryDataSourceRequest request)
        {
            var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            //get the Iqueryable data
            var data = _referenceRepository.GetProductCategoryDataSourceList();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ServiceId > 0)
            {
                data = data.Where(x => x.RefProductCategoryApiServices.Any(y => y.ServiceId == request.ServiceId));
            }

            if (request.ProductCategoryIds != null && request.ProductCategoryIds.Any())
            {
                data = data.Where(x => request.ProductCategoryIds.Contains(x.Id));
            }
            //execute the data
            var productCategoryList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            if (productCategoryList != null && productCategoryList.Any())
            {
                response.DataSourceList = productCategoryList.OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        /// <summary>
        /// Get the product sub category source list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetProductSubCategorySourceList(ProductSubCategoryDataSourceRequest request)
        {
            var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            //get the Iqueyrable data
            var data = _referenceRepository.GetProductSubCategoryDataSourceList();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ProductSubCategoryIds != null && request.ProductSubCategoryIds.Any())
            {
                data = data.Where(x => request.ProductSubCategoryIds.Contains(x.Id));
            }
            if (request.ProductCategoryIds != null && request.ProductCategoryIds.Any())
                data = data.Where(x => request.ProductCategoryIds.Contains(x.ProductCategoryId));
            //execute the data
            var productCategoryList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            //assign it to the datasource list
            if (productCategoryList != null && productCategoryList.Any())
            {
                response.DataSourceList = productCategoryList.OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        /// <summary>
        /// Get the product sub category 2 source list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetProductSubCategory2SourceList(ProductSubCategory2DataSourceRequest request)
        {
            var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            //get the Iqueyrable data
            var data = _referenceRepository.GetProductSubCategory2DataSourceList();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ProductCategoryId > 0)
            {
                data = data.Where(x => x.ProductSubCategory.ProductCategoryId == request.ProductCategoryId);
            }

            if (request.ProductSubCategoryIds != null && request.ProductSubCategoryIds.Any())
                data = data.Where(x => request.ProductSubCategoryIds.Contains(x.ProductSubCategoryId));

            if (request.ProductSubCategory2Ids != null && request.ProductSubCategory2Ids.Any())
                data = data.Where(x => request.ProductSubCategory2Ids.Contains(x.Id));

            //execute the data
            var productSubCategory2List = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();

            //assign it to the datasource list
            if (productSubCategory2List != null && productSubCategory2List.Any())
            {
                response.DataSourceList = productSubCategory2List;
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        /// <summary>
        /// Get invoice extra type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInvoiceExtraTypeList()
        {
            var data = await _referenceRepository.GetInvoiceExtraTypeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetFBResultList()
        {
            var response = new DataSourceResponse();

            response.DataSourceList = await _referenceRepository.GetFbReportResults();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.Success };

            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// get delimiter list
        /// </summary>
        /// <returns></returns>
        public async Task<EmailSubjectDelimiterResponse> GetDelimiterList()
        {
            var data = await _referenceRepository.GetDelimiterList();

            if (data != null)
            {
                return new EmailSubjectDelimiterResponse() { delimiterList = data, Result = EmailSubjectDelimiterResult.Success };
            }

            return new EmailSubjectDelimiterResponse() { Result = EmailSubjectDelimiterResult.CannotGetList };
        }

        //get all the office locations
        public async Task<DataSourceResponse> GetOfficeLocations()
        {
            var response = new DataSourceResponse();
            response.DataSourceList = await _referenceRepository.GetOfficeLocations();

            if (response.DataSourceList == null || !response.DataSourceList.Any())
                response.Result = DataSourceResult.CannotGetList;

            response.Result = DataSourceResult.Success;
            return response;

        }

        /// <summary>
        /// Get the date formats
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetDateFormats()
        {
            var data = await _referenceRepository.GetDateFormats();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get Customer Service Types
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceTypeResponse> GetCustomerServiceTypes(ServiceTypeRequest request)
        {
            if (request != null)
            {

                var data = _referenceRepository.GetCustomerServiceTypeQuery();

                if (request.CustomerId > 0)
                    data = data.Where(x => x.CuServiceTypes.Any(y => y.CustomerId == request.CustomerId && y.Active));
                //commented as part of ALD-3877
                //if (request.BusinessLineId > 0)
                //    data = data.Where(x => x.BusinessLineId == request.BusinessLineId);
                if (request.ServiceId > 0)
                    data = data.Where(x => x.ServiceId == request.ServiceId);

                if (request.IsReInspectedService != null)
                {
                    if (request.IsReInspectedService.HasValue && request.IsReInspectedService.Value)
                        data = data.Where(x => x.IsReInspectedService.Value);
                    else if (request.IsReInspectedService.HasValue && !request.IsReInspectedService.Value)
                        data = data.Where(x => !x.IsReInspectedService.Value);
                }


                var serviceTypeList = await data.Select(x => new ServiceTypeData()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sort = x.Sort,
                    Is100Inspection = x.Is100Inspection.GetValueOrDefault(),
                    ShowServiceDateTo = x.ShowServiceDateTo.HasValue && x.ShowServiceDateTo.Value ? true : false
                }).Distinct().ToListAsync();
                serviceTypeList = serviceTypeList.OrderBy(x => x.Sort).ThenBy(x => x.Name).ToList();
                if (request.BookingId > 0)
                {
                    var bookingServiceTypes = await _referenceRepository.GetEditBookingServiceTypes(request.BookingId);

                    foreach (var serviceType in bookingServiceTypes)
                    {
                        if (!serviceTypeList.Any(x => x.Id == serviceType.Id))
                            serviceTypeList.Add(serviceType);
                    }
                }

                if (serviceTypeList != null && serviceTypeList.Any())
                {
                    return new ServiceTypeResponse() { ServiceTypeList = serviceTypeList, Result = ServiceTypeResult.Success };
                }

            }

            return new ServiceTypeResponse() { Result = ServiceTypeResult.NotFound };
        }

        /// <summary>
        /// Get the inspection location list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInspectionLocations()
        {
            var data = await _referenceRepository.GetInspectionLocation();
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }
        /// <summary>
        /// Get the inspection shipment types
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInspectionShipmentTypes()
        {
            var data = await _referenceRepository.GetInspectionShipmentTypes();
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }
        /// <summary>
        /// Get Business Lines
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBusinessLines()
        {
            var data = await _referenceRepository.GetBusinessLines();
            if (data != null && data.Any())
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        public async Task<DataSourceResponse> GetEntityList()
        {
            var data = await _referenceRepository.GetEntityList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        public async Task<DataSourceResponse> GetTripTypeList()
        {
            var data = await _referenceRepository.GetTripTypeDataSource();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the user entity list
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetUserEntityList(int userType, int id)
        {
            var data = new List<CommonDataSource>();
            switch (userType)
            {
                case (int)Entities.Enums.UserTypeEnum.Customer:
                    data = await _referenceRepository.GetCustomerEntityList(id);
                    break;
                case (int)Entities.Enums.UserTypeEnum.Supplier:
                    data = await _referenceRepository.GetSupplierEntityList(id);
                    break;
                case (int)Entities.Enums.UserTypeEnum.Factory:
                    data = await _referenceRepository.GetSupplierEntityList(id);
                    break;
                case (int)Entities.Enums.UserTypeEnum.InternalUser:
                    data = await _referenceRepository.GetInternalUserEntityList(id);
                    break;
                case (int)Entities.Enums.UserTypeEnum.OutSource:
                    data = await _referenceRepository.GetInternalUserEntityList(id);
                    break;
            }


            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get the product sub category 3 source list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetProductSubCategory3SourceList(ProductSubCategory3DataSourceRequest request)
        {
            var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            //get the Iqueyrable data
            var data = _referenceRepository.GetProductSubCategory3DataSourceList();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ProductCategoryId > 0)
                data = data.Where(x => x.ProductSubCategory2.ProductSubCategory.ProductCategoryId == request.ProductCategoryId);

            if (request.ProductSubCategoryId > 0)
                data = data.Where(x => x.ProductSubCategory2.ProductSubCategoryId == request.ProductSubCategoryId);

            if (request.ProductSubCategory2Ids != null && request.ProductSubCategory2Ids.Any())
                data = data.Where(x => request.ProductSubCategory2Ids.Contains(x.ProductSubCategory2Id));

            if (request.ProductSubCategory3Ids != null && request.ProductSubCategory3Ids.Any())
                data = data.Where(x => request.ProductSubCategory3Ids.Contains(x.Id));

            //execute the data
            var productSubCategory3List = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();

            //assign it to the datasource list
            if (productSubCategory3List != null && productSubCategory3List.Any())
            {
                response.DataSourceList = productSubCategory3List;
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        public async Task<List<CommonDataSource>> GetProdCategoriesByProdCategoryIds(IEnumerable<int> prodCategoryIds)
        {
            return await _referenceRepository.GetProdCategoriesByProdCategoryIds(prodCategoryIds);
        }

        public async Task<DataSourceResponse> GetServiceTypesByServiceIds(IEnumerable<int> serviceIds)
        {
            var data = await _referenceRepository.GetServiceTypesByServiceId(serviceIds);

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get billing frequency list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBillingFrequncyList()
        {
            var data = await _referenceRepository.GetBillingFrequencyList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get Billing Quantity type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBillingQuantityTypeList()
        {
            var data = await _referenceRepository.GetBillingQuantityTypeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        /// <summary>
        /// Get Billing Quantity type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetInterventionTypeList()
        {
            var data = await _referenceRepository.GetInterventionTypeList();

            if (data != null)
            {
                return new DataSourceResponse() { DataSourceList = data, Result = DataSourceResult.Success };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        public async Task<List<int>> GetEntityFeatureList()
        {
            var data = await _referenceRepository.GetEntityFeatureList();

            return data.Where(x => x.FeatureId != null).Select(x => x.FeatureId.GetValueOrDefault()).Distinct().ToList();
        }

        public async Task<DataSourceResponse> GetStaffSourceList(CommonDataSourceRequest request)
        {
            var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            var data = _referenceRepository.GetStaffDataSource();

            if (request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }

            if (request.IdList != null && request.IdList.Any())
            {
                data = data.Where(x => request.IdList.Contains(x.Id));
            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.PersonName != null && EF.Functions.Like(x.PersonName, $"%{request.SearchText.Trim()}%"));
            }

            //execute the customer list
            var customerList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.PersonName
            }).ToListAsync();

            //assign it to customer list
            if (customerList != null && customerList.Any())
            {
                response.DataSourceList = customerList.OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        public async Task<DataSourceResponse> GetExpertiseList()
        {
            var data = await _humanResourceRepository.GetExpertises();


            if (data != null)
            {
                return new DataSourceResponse() { Result = DataSourceResult.Success, DataSourceList = data.Select(x => new CommonDataSource() { Id = x.Id, Name = x.Name }) };
            }

            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }

        public async Task<bool> CheckUserHasInvoiceAccess()
        {
            return await _referenceRepository.CheckUserHasInvoiceAccess(_ApplicationContext.StaffId);
        }

        /// <summary>
        /// Get the inspection booking types
        /// </summary>
        /// <returns></returns>
        public async Task<InspectionBookingTypeResponse> GetInspectionBookingTypeList()
        {
            var response = new InspectionBookingTypeResponse() { Result = InspectionBookingTypeResult.NotFound };
            var inspectionBookingTypeList = await _referenceRepository.GetInspectionBookingTypes();

            if (inspectionBookingTypeList != null && inspectionBookingTypeList.Any())
            {
                response.InspectionBookingTypeList = inspectionBookingTypeList;
                response.Result = InspectionBookingTypeResult.Success;
            }

            return response;
        }

        /// <summary>
        /// Get the inspection payment options
        /// </summary>
        /// <returns></returns>
        public async Task<InspectionPaymentOptionsResponse> GetInspectionPaymentOptions(int customerId)
        {
            var response = new InspectionPaymentOptionsResponse() { Result = InspectionPaymentOptionsResult.NotFound };
            var inspectionPaymentOptions = await _referenceRepository.GetInspectionPaymentOptions(customerId);

            if (inspectionPaymentOptions != null && inspectionPaymentOptions.Any())
            {
                response.InspectionPaymentOptions = inspectionPaymentOptions;
                response.Result = InspectionPaymentOptionsResult.Success;
            }

            return response;
        }

        public async Task<object> GetEAQFProductsCategories(string ProductLineName, int ProductLineId)
        {
            var response = new EaqfGetSuccessResponse();
            try
            {
                var data = _referenceRepository.GetProductsCategories();

                if (!string.IsNullOrEmpty(ProductLineName))
                {
                    data = data.Where(x => x.BusinessLine.BusinessLine.ToLower() == ProductLineName.ToLower());
                }

                if (ProductLineId > 0)
                {
                    data = data.Where(x => x.BusinessLine.Id == ProductLineId);
                }

                var resultData = await data.Select(x => new EaqfProductCategories()
                {
                    Id = x.Id,
                    Name = x.Name,
                    BusinessLine = x.BusinessLine.BusinessLine
                }).AsNoTracking().ToListAsync();

                if (resultData == null || !resultData.Any())
                {
                    return new EaqfGetSuccessResponse() { statusCode = HttpStatusCode.BadRequest, message = BadRequest, data = resultData };
                }
                else
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = Success;
                    response.data = resultData;
                }
                return response;
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(System.Net.HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { InternalServerError });
            }

        }

        public async Task<object> GetEAQFServices(int CustomerId, string ServiceCategoryName, int ServiceCategoryId)
        {
            var response = new EaqfGetSuccessResponse() { };
            try
            {
                var data = _referenceRepository.GetCustomerServiceTypeQuery();

                if (CustomerId > 0)
                {
                    data = data.Where(x => x.CuServiceTypes.Any(y => y.CustomerId == CustomerId));
                }
                if (!string.IsNullOrWhiteSpace(ServiceCategoryName))
                {
                    data = data.Where(x => x.Service.Name.ToLower() == ServiceCategoryName.ToLower());
                }
                if (ServiceCategoryId > 0)
                {
                    data = data.Where(x => x.CuServiceTypes.Any(y => x.Service.Id == ServiceCategoryId));
                }

                var eaqfServices = await data.Select(x => new EaqfServices
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Abbreviation,
                    ServiceCategory = x.Service.Name
                }).ToListAsync();

                if (eaqfServices != null && eaqfServices.Any())
                {
                    response.statusCode = System.Net.HttpStatusCode.OK;
                    response.message = Success;
                    response.data = eaqfServices;
                    return response;
                }
                else
                {
                    return BuildCommonEaqfResponse(System.Net.HttpStatusCode.BadRequest, BadRequest, new List<string>() { BadRequest });
                }
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(System.Net.HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { InternalServerError });
            }

        }

        public async Task<object> GetEAQFProductType(string productCategoryName, string productSubCategoryName)
        {

            var response = new EaqfGetSuccessResponse() { };
            try
            {
                var productType = _referenceRepository.GetProductSubCategory2DataSourceList();

                if (productCategoryName != null && !string.IsNullOrWhiteSpace(productCategoryName))
                {
                    productType = productType.Where(x => x.ProductSubCategory.ProductCategory.Name.ToLower() == productCategoryName.ToLower());
                }
                if (productSubCategoryName != null && !string.IsNullOrWhiteSpace(productSubCategoryName))
                {
                    productType = productType.Where(x => x.ProductSubCategory.Name.ToLower() == productSubCategoryName.ToLower());
                }

                var eaqfProductTypes = await productType.Select(x => new EaqfProductType
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductCategoryName = x.ProductSubCategory.ProductCategory.Name
                }).ToListAsync();

                if (eaqfProductTypes != null && eaqfProductTypes.Any())
                {
                    response.statusCode = System.Net.HttpStatusCode.OK;
                    response.message = Success;
                    response.data = eaqfProductTypes;
                    return response;
                }
                else
                {
                    return new EaqfGetSuccessResponse() { statusCode = HttpStatusCode.BadRequest, message = BadRequest, data = eaqfProductTypes };
                }
            }
            catch (Exception ex)
            {
                return BuildCommonEaqfResponse(System.Net.HttpStatusCode.InternalServerError, InternalServerError, new List<string>() { InternalServerError });
            }

        }


        public EaqfErrorResponse BuildCommonEaqfResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new EaqfErrorResponse()
            {
                errors = errors,
                statusCode = statusCode,
                message = message
            };
        }

        /// <summary>
        /// get outsource staff list
        /// </summary>
        /// <returns></returns>
        public async Task<HRStaffResponse> GetOutSourceStaffList()
        {
            var response = new HRStaffResponse();

            var staffQuery = _referenceRepository.GetStaffDataSource();

            staffQuery = staffQuery.Where(x => x.EmployeeTypeId != (int)EmployeeTypeEnum.Permanent);

            response.StaffList = await staffQuery.Select(x => new HRStaffDetail()
            {
                Id = x.Id,
                StaffName = x.PersonName
            }).AsNoTracking().ToListAsync();
            response.Result = HRStaffDetailResult.Success;

            return response;
        }

        public async Task<bool> IsEntityFeatureExist(int featureId)
        {
            return await _referenceRepository.CheckEntityFeature(featureId);
        }

        /// <summary>
        /// Get the audit service types
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceTypeResponse> GetAuditServiceTypes()
        {
            var response = new ServiceTypeResponse();
            //get the service type query
            var serviceTypeQuery = _referenceRepository.GetCustomerServiceTypeQuery();
            //apply the audit service id
            serviceTypeQuery = serviceTypeQuery.Where(x => x.ServiceId == (int)Entities.Enums.Service.AuditId);
            //execute the service type list
            var serviceTypeList = await serviceTypeQuery.Select(x =>
                                                new ServiceTypeData()
                                                {
                                                    Id = x.Id,
                                                    Name = x.Name,
                                                }).AsNoTracking().ToListAsync();
            if (!serviceTypeList.Any())
                response.Result = ServiceTypeResult.NotFound;
            if (serviceTypeList.Any())
            {
                response.ServiceTypeList = serviceTypeList;
                response.Result = ServiceTypeResult.Success;
            }

            return response;
        }
    }
}
