using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerPriceCardManager : ApiCommonData, ICustomerPriceCardManager
    {
        private readonly ICustomerPriceCardRepository _customerPriceCardRepository = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly CustomerPriceCardMap _customerpricecardmap = null;
        private ITenantProvider _filterService = null;
        private readonly ICustomerRepository _customerRepo = null;

        public CustomerPriceCardManager(ICustomerPriceCardRepository customerPriceCardRepository,
            IAPIUserContext applicationContext, ITenantProvider filterService, ICustomerRepository customerRepo)
        {
            _customerPriceCardRepository = customerPriceCardRepository;
            _applicationContext = applicationContext;
            _customerpricecardmap = new CustomerPriceCardMap();
            _filterService = filterService;
            _customerRepo = customerRepo;
        }

        //edit the customer price card values by id(customer price card )
        public async Task<EditSaveCustomerPriceCardResponse> Edit(int id)
        {
            ExportMapRequest paramList = new ExportMapRequest();
            try
            {
                var response = new EditSaveCustomerPriceCardResponse();

                //get saved user data values by id
                var customerPriceCardDetail = await _customerPriceCardRepository.GetCustomerPriceCardDetail(id);

                var cuPrIdList = new[] { id }.ToList();

                paramList.CountryData = await _customerPriceCardRepository.GetPrCountries(cuPrIdList);

                paramList.DeptData = await _customerPriceCardRepository.GetPrDepartment(cuPrIdList);

                paramList.PriceCategory = await _customerPriceCardRepository.GetPrPriceCategory(cuPrIdList);

                paramList.ServiceType = await _customerPriceCardRepository.GetPrServiceTypes(cuPrIdList);

                paramList.SupData = await _customerPriceCardRepository.GetPrSuppliers(cuPrIdList);

                paramList.ProductCategory = await _customerPriceCardRepository.GetPrProductCategories(cuPrIdList);

                paramList.ProductSubCategory = await _customerPriceCardRepository.GetPrProductSubCategories(cuPrIdList);

                paramList.ProvinceData = await _customerPriceCardRepository.GetPrProvince(cuPrIdList);

                paramList.CityData = await _customerPriceCardRepository.GetPrCity(cuPrIdList);

                paramList.BuyerData = await _customerPriceCardRepository.GetPrBuyer(cuPrIdList);

                paramList.BrandData = await _customerPriceCardRepository.GetPrBrand(cuPrIdList);

                paramList.HolidayType = await _customerPriceCardRepository.GetPrHolidayType(cuPrIdList);

                paramList.InspectionLocation = await _customerPriceCardRepository.GetPrInspectionLocation(cuPrIdList);

                paramList.Contact = await _customerPriceCardRepository.GetPrContacts(cuPrIdList);


                if (customerPriceCardDetail != null)
                {
                    var priceCardInvoiceDetails = await _customerPriceCardRepository.GetCustomerPriceCardInvoiceRequest(id);
                    var priceSubcategoryList = await _customerPriceCardRepository.GetCustomerPriceCardSubCategory(id);
                    var priceRuleList = await _customerPriceCardRepository.GetCustomerPriceRuleList(id);

                    // update newly added subccategory 2
                    List<PriceSubCategory> newPriceSubCategory2List = priceSubcategoryList.ToList();
                    if (!customerPriceCardDetail.SubCategorySelectAll.GetValueOrDefault())
                    {
                        var productCategoryIds = paramList.ProductCategory.Select(x => (int?)x.Id).Distinct().ToList();
                        var productSubcategoryIds = paramList.ProductSubCategory.Select(x => (int?)x.Id).Distinct().ToList();
                        if (productCategoryIds.Any() || productSubcategoryIds.Any())
                        {
                            var updatedsubCategory2List = await _customerRepo.GetCustomerProductSub2CategoryList(productCategoryIds, productSubcategoryIds);
                            foreach (var subCategory2 in updatedsubCategory2List)
                            {
                                if (!priceSubcategoryList.Any(x => x.SubCategory2Id == subCategory2.Id))
                                {
                                    newPriceSubCategory2List.Add(new PriceSubCategory() { SubCategory2Id = subCategory2.Id, SubCategory2Name = subCategory2.Name });
                                }
                            }
                        }
                    }

                    response.getData = _customerpricecardmap.MapCustomerPriceCard(customerPriceCardDetail, paramList);

                    var invoiceRequestIds = priceCardInvoiceDetails.Select(x => x.Id).Distinct().ToList();
                    paramList.InvoiceContactList = await _customerPriceCardRepository.GetInvoiceContacts(invoiceRequestIds);

                    foreach (var item in priceCardInvoiceDetails)
                    {
                        item.InvoiceRequestContactList = paramList.InvoiceContactList.Where(x => x.InvoiceId == item.Id).Select(x => x.ContactId.GetValueOrDefault());
                    }

                    response.getData.InvoiceRequestList = priceCardInvoiceDetails;
                    response.getData.SubCategoryList = newPriceSubCategory2List;
                    response.getData.RuleList = priceRuleList;
                    response.Result = ResponseResult.Success;
                }
                else
                    response.Result = ResponseResult.NotFound;

                return response;
            }
            catch (Exception ex)
            {
                return new EditSaveCustomerPriceCardResponse() { Result = ResponseResult.Error };
                //throw ex;
            }
        }

        public async Task<SaveCustomerPriceCardResponse> Save(CustomerPriceCard request)
        {
            //empty check
            if (request == null)
                return new SaveCustomerPriceCardResponse() { Result = ResponseResult.RequestNotCorrectFormat };

            var response = new SaveCustomerPriceCardResponse();

            try
            {
                //same request if exists in DB
                if (await IsPriceCardExists(request))
                {
                    return new SaveCustomerPriceCardResponse() { Result = ResponseResult.Exists };
                }

                //update
                if (request.Id > 0)
                {
                    //get data
                    var customerPriceDetail = await _customerPriceCardRepository.GetCustomerPriceCardDetails(request.Id);

                    if (customerPriceDetail != null)
                    {
                        //map the values
                        response.Id = await EditCustomerPriceCard(request, customerPriceDetail);
                    }
                }
                else //save
                {
                    //add a new record
                    response.Id = await AddCustomerPriceCard(request);

                }

                response.Result = ResponseResult.Success;

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return response;
        }

        //is customer price card exists in DB
        private async Task<bool> IsPriceCardExists(CustomerPriceCard request)
        {

            var customerPriceDetailList = _customerPriceCardRepository.IsExists(request);

            var recordExists = false;

            if (customerPriceDetailList.Any())
            {
                if (request.Id > 0)
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.Id != request.Id);
                }

                if (request.CustomerId > 0)
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CustomerId == request.CustomerId);
                }

                if (request.BillingMethodId > 0)
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.BillingMethodId == request.BillingMethodId || x.BillingMethodId != request.BillingMethodId);
                }

                if (request.BillingToId > 0)
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.BillingToId == request.BillingToId);
                }
                if (request.ServiceId > 0)
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.ServiceId == request.ServiceId);
                }

                if (request.CurrencyId > 0)
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CurrencyId == request.CurrencyId);
                }

                if (request.PeriodFrom?.ToDateTime() != null && request.PeriodTo?.ToDateTime() != null)
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => request.PeriodFrom.ToDateTime() <= x.PeriodTo &&
                      request.PeriodTo.ToDateTime() >= x.PeriodFrom);
                }
                //check product category is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(y => y.CuPrProductCategories).Any() && request.ProductCategoryIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrProductCategories.Any(y => y.Active.HasValue && y.Active.Value && request.ProductCategoryIdList.Contains(y.ProductCategoryId)));
                }

                //check product sub category is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(y => y.CuPrProductSubCategories).Any() && request.ProductSubCategoryIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrProductSubCategories.Any(y => y.Active.HasValue && y.Active.Value && request.ProductSubCategoryIdList.Contains(y.ProductSubCategoryId)));
                }
                //check country is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(y => y.CuPrCountries).Any() && request.FactoryCountryIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrCountries.Any(y => y.Active.HasValue && y.Active.Value && request.FactoryCountryIdList.Contains(y.FactoryCountryId)));
                }
                //check province is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(y => y.CuPrProvinces).Any() && request.FactoryProvinceIdList != null && request.FactoryProvinceIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrProvinces.Any(y => y.Active.HasValue && y.Active.Value && request.FactoryProvinceIdList.Contains(y.FactoryProvinceId)));
                }
                //check city is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(y => y.CuPrCities).Any() && request.FactoryCityIdList != null && request.FactoryCityIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrCities.Any(y => y.Active.Value && request.FactoryProvinceIdList.Contains(y.FactoryCityId)));
                }
                //check supplier is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(z => z.CuPrSuppliers).Any() && request.SupplierIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrSuppliers.Any(y => y.Active.HasValue && y.Active.Value && request.SupplierIdList.Contains(y.SupplierId)));
                }
                //check servicetype is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(z => z.CuPrServiceTypes).Any() && request.ServiceTypeIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrServiceTypes.Any(y => y.Active.HasValue && y.Active.Value && request.ServiceTypeIdList.Contains(y.ServiceTypeId)));
                }
                //check brand is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(z => z.CuPrBrands).Any() && request.BrandIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrBrands.Any(y => y.Active.HasValue && y.Active.Value && request.BrandIdList.Contains(y.BrandId.Value)));
                }
                //check dept is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(z => z.CuPrDepartments).Any() && request.DepartmentIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrDepartments.Any(y => y.Active.HasValue && y.Active.Value && request.DepartmentIdList.Contains(y.DepartmentId.Value)));
                }
                //check buyer is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(z => z.CuPrBuyers).Any() && request.BuyerIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrBuyers.Any(y => y.Active.HasValue && y.Active.Value && request.BuyerIdList.Contains(y.BuyerId.Value)));
                }
                //check pricecategory is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(z => z.CuPrPriceCategories).Any() && request.PriceCategoryIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrPriceCategories.Any(y => y.Active.HasValue && y.Active.Value && request.PriceCategoryIdList.Contains(y.PriceCategoryId.Value)));
                }
                //check holidaytype is available or not
                if (customerPriceDetailList.Any() && customerPriceDetailList.SelectMany(z => z.CuPrHolidayTypes).Any() && request.HolidayTypeIdList.Any())
                {
                    customerPriceDetailList = customerPriceDetailList.Where(x => x.CuPrHolidayTypes.Any(y => y.Active.HasValue && y.Active.Value && request.HolidayTypeIdList.Contains(y.HolidayInfoId.Value)));
                }
                var isDataExists = await customerPriceDetailList.AnyAsync();

                if (isDataExists)
                {
                    recordExists = true;
                }
            }

            return recordExists;
        }

        // update the customer price card details
        private async Task<int> EditCustomerPriceCard(CustomerPriceCard request, CuPrDetail customerPriceDetail)
        {
            try
            {
                customerPriceDetail.CustomerId = request.CustomerId;
                customerPriceDetail.BillingMethodId = request.BillingMethodId;
                customerPriceDetail.BillingToId = request.BillingToId;
                customerPriceDetail.ServiceId = request.ServiceId;
                customerPriceDetail.UnitPrice = Math.Round(request.UnitPrice, 2);
                customerPriceDetail.Remarks = request?.Remarks;
                customerPriceDetail.TaxIncluded = request.TaxIncluded;
                customerPriceDetail.TravelIncluded = request.TravelIncluded;
                customerPriceDetail.FreeTravelKm = request.FreeTravelKM;
                customerPriceDetail.CurrencyId = request.CurrencyId;
                customerPriceDetail.PeriodFrom = request.PeriodFrom?.ToDateTime();
                customerPriceDetail.PeriodTo = request.PeriodTo?.ToDateTime();
                customerPriceDetail.UpdatedBy = _applicationContext.UserId;
                customerPriceDetail.UpdatedOn = DateTime.Now;
                customerPriceDetail.TravelMatrixTypeId = request.TariffTypeId;
                customerPriceDetail.HolidayPrice = request.HolidayPrice;
                customerPriceDetail.ProductPrice = request.ProductPrice;
                customerPriceDetail.PriceToEachProduct = request.PriceToEachProduct;

                customerPriceDetail.InvoiceRequestSelectAll = request.InvoiceRequestSelectAll;
                customerPriceDetail.IsInvoiceConfigured = request.IsInvoiceConfigured;

                customerPriceDetail.InvoiceRequestType = request.InvoiceRequestType;
                customerPriceDetail.InvoiceRequestAddress = request.InvoiceRequestAddress;
                customerPriceDetail.InvoiceRequestBilledName = request.InvoiceRequestBilledName;

                customerPriceDetail.BillingEntity = request.BillingEntity;
                customerPriceDetail.BankAccount = request.BankAccount;
                customerPriceDetail.PaymentDuration = request.PaymentDuration;
                customerPriceDetail.PaymentTerms = request.PaymentTerms;

                // sampling details
                customerPriceDetail.MaxProductCount = request.MaxProductCount;
                customerPriceDetail.SampleSizeBySet = request.SampleSizeBySet;
                customerPriceDetail.MinBillingDay = request.MinBillingDay;
                customerPriceDetail.MaxSampleSize = request.MaxSampleSize;
                customerPriceDetail.AdditionalSampleSize = request.AdditionalSampleSize;
                customerPriceDetail.AdditionalSamplePrice = request.AdditionalSamplePrice;
                customerPriceDetail.Quantity8 = request.Quantity8;
                customerPriceDetail.Quantity13 = request.Quantity13;
                customerPriceDetail.Quantity20 = request.Quantity20;
                customerPriceDetail.Quantity32 = request.Quantity32;
                customerPriceDetail.Quantity50 = request.Quantity50;
                customerPriceDetail.Quantity80 = request.Quantity80;
                customerPriceDetail.Quantity125 = request.Quantity125;
                customerPriceDetail.Quantity200 = request.Quantity200;
                customerPriceDetail.Quantity315 = request.Quantity315;
                customerPriceDetail.Quantity500 = request.Quantity500;
                customerPriceDetail.Quantity800 = request.Quantity800;
                customerPriceDetail.Quantity1250 = request.Quantity1250;

                customerPriceDetail.InvoiceNoDigit = request.InvoiceNoDigit;
                customerPriceDetail.InvoiceNoPrefix = request.InvoiceNoPrefix;

                customerPriceDetail.InvoiceOffice = request.InvoiceOffice;

                customerPriceDetail.InvoiceInspFeeFrom = request.InvoiceInspFeeFrom;
                customerPriceDetail.InvoiceHotelFeeFrom = request.InvoiceHotelFeeFrom;
                customerPriceDetail.InvoiceDiscountFeeFrom = request.InvoiceDiscountFeeFrom;
                customerPriceDetail.InvoiceOtherFeeFrom = request.InvoiceOtherFeeFrom;
                customerPriceDetail.InvoiceTmfeeFrom = request.InvoiceTravelExpense;

                // new updates 

                customerPriceDetail.MaxFeeStyle = request.MaxFeeStyle;
                customerPriceDetail.InvoiceSubject = request.InvoiceSubject;
                customerPriceDetail.BilledQuantityType = request.BillQuantityType;
                customerPriceDetail.InterventionType = request.InterventionType;
                customerPriceDetail.BillingFreequency = request.BillFrequency;

                // manday related items
                customerPriceDetail.MandayBuffer = request.MandayBuffer;
                customerPriceDetail.ManDayProductivity = request.MandayProductivity;
                customerPriceDetail.MandayReportCount = request.MandayReports;

                customerPriceDetail.IsSpecial = request.IsSpecial;
                customerPriceDetail.SubCategorySelectAll = request.SubCategorySelectAll;

                customerPriceDetail.MandayBuffer = request.MandayBuffer;
                customerPriceDetail.ManDayProductivity = request.MandayProductivity;
                customerPriceDetail.MandayReportCount = request.MandayReports;



                UpdateProductCategory(request.ProductCategoryIdList, customerPriceDetail);

                UpdateProductSubCategory(request.ProductSubCategoryIdList, customerPriceDetail);

                UpdateSupplier(request.SupplierIdList, customerPriceDetail);

                UpdateServiceType(request.ServiceTypeIdList, customerPriceDetail);

                UpdateCountry(request.FactoryCountryIdList, customerPriceDetail);

                UpdateCustomerBuyer(request.BuyerIdList, customerPriceDetail);

                UpdateCustomerBrand(request.BrandIdList, customerPriceDetail);

                UpdateCustomerDepartment(request.DepartmentIdList, customerPriceDetail);

                UpdateCustomerPriceCategory(request.PriceCategoryIdList, customerPriceDetail);

                UpdateCustomerHolidayType(request.HolidayTypeIdList, customerPriceDetail);

                UpdateInspectionLocations(request.InspectionLocationList, customerPriceDetail);

                UpdatePriceSubcategory(request, customerPriceDetail);

                if (request.IsSpecial.HasValue && request.IsSpecial.Value)
                    UpdatePriceRuleList(request, customerPriceDetail);

                if (request.IsInvoiceConfigured.HasValue && request.IsInvoiceConfigured.Value)
                    UpdateCustomerPriceInvoiceRequestContacts(request, customerPriceDetail);

                if (request.IsInvoiceConfigured.HasValue && request.IsInvoiceConfigured.Value)
                    UpdateCustomerPriceInvoiceRequest(request, customerPriceDetail);

                if (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Count() > 0)
                {
                    //if country selected count 1, province will insert== 1
                    if (request.FactoryCountryIdList.Count() == CountryCount)
                    {
                        UpdateProvince(request.FactoryProvinceIdList, customerPriceDetail);
                    }
                    else
                    {
                        //remove the province saved data
                        RemoveProvince(customerPriceDetail);
                    }

                }

                if (request.FactoryProvinceIdList != null && request.FactoryProvinceIdList.Count() > 0)
                {
                    //if country selected count 1, province will insert== 1
                    if (request.FactoryProvinceIdList.Count() == CountryCount)
                    {
                        UpdateCity(request.FactoryCityIdList, customerPriceDetail);
                    }
                    else
                    {
                        //remove the province saved data
                        RemoveCity(customerPriceDetail);
                    }
                }


                _customerPriceCardRepository.EditEntity(customerPriceDetail);

                await _customerPriceCardRepository.Save();

                return customerPriceDetail.Id;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //product category
        private void UpdateProductCategory(IEnumerable<int> requestProductCategoryIdList, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var addProductCategoryIds = requestProductCategoryIdList?.Except(customerPriceDetail.CuPrProductCategories.Where(x => x.Active.Value).
                                                            Select(x => x.ProductCategoryId).ToList());
            //unselected data from request to remove from DB
            var removeProductCategoryIdList = customerPriceDetail?.CuPrProductCategories?.Where(x => x.Active.Value
                                                                && !requestProductCategoryIdList.Contains(x.ProductCategoryId));

            if (addProductCategoryIds != null)
            {
                //add
                foreach (var productCategoryId in addProductCategoryIds)
                {
                    var productCategoryEntity = new CuPrProductCategory()
                    {
                        ProductCategoryId = productCategoryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrProductCategories.Add(productCategoryEntity);
                    _customerPriceCardRepository.AddEntity(productCategoryEntity);
                }
            }

            if (removeProductCategoryIdList != null)
            {
                //remove
                foreach (var productCategoryData in removeProductCategoryIdList)
                {
                    productCategoryData.Active = false;
                    productCategoryData.DeletedOn = DateTime.Now;
                    productCategoryData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(productCategoryData);
                }
            }

            //update - nothing
        }


        //product sub category
        private void UpdateProductSubCategory(IEnumerable<int> requestProductCategoryIdList, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var addProductSubCategoryIds = requestProductCategoryIdList?.Except(customerPriceDetail.CuPrProductSubCategories.Where(x => x.Active.Value).
                                                            Select(x => x.ProductSubCategoryId).ToList());
            //unselected data from request to remove from DB
            var removeProductSubCategoryIdList = customerPriceDetail?.CuPrProductSubCategories?.Where(x => x.Active.Value
                                                                && !requestProductCategoryIdList.Contains(x.ProductSubCategoryId));

            if (addProductSubCategoryIds != null)
            {
                //add
                foreach (var productCategoryId in addProductSubCategoryIds)
                {
                    var productSubCategoryEntity = new CuPrProductSubCategory()
                    {
                        ProductSubCategoryId = productCategoryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrProductSubCategories.Add(productSubCategoryEntity);
                    _customerPriceCardRepository.AddEntity(productSubCategoryEntity);
                }
            }

            if (removeProductSubCategoryIdList != null)
            {
                //remove
                foreach (var productSubCategoryData in removeProductSubCategoryIdList)
                {
                    productSubCategoryData.Active = false;
                    productSubCategoryData.DeletedOn = DateTime.Now;
                    productSubCategoryData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(productSubCategoryData);
                }
            }

            //update - nothing
        }

        //service type
        private void UpdateServiceType(IEnumerable<int> requestServiceTypeIdList, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var addServiceTypeIds = requestServiceTypeIdList?.Except(customerPriceDetail.CuPrServiceTypes.Where(x => x.Active.Value).
                                                            Select(x => x.ServiceTypeId).ToList());

            //unselected data from request to remove from DB
            var removeServiceTypeIdList = customerPriceDetail.CuPrServiceTypes.Where(x => x.Active.Value
                                                                && !requestServiceTypeIdList.Contains(x.ServiceTypeId));
            if (addServiceTypeIds != null)
            {
                //add
                foreach (var serviceTypeId in addServiceTypeIds)
                {
                    var serviceTypeEntity = new CuPrServiceType()
                    {
                        ServiceTypeId = serviceTypeId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrServiceTypes.Add(serviceTypeEntity);
                    _customerPriceCardRepository.AddEntity(serviceTypeEntity);
                }
            }

            if (removeServiceTypeIdList != null)
            {
                //remove
                foreach (var serviceTypeData in removeServiceTypeIdList)
                {
                    serviceTypeData.Active = false;
                    serviceTypeData.DeletedOn = DateTime.Now;
                    serviceTypeData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(serviceTypeData);
                }

            }
            //update - nothing
        }

        //supplier
        private void UpdateSupplier(IEnumerable<int> requestSupplierIdList, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var addSupplierIds = requestSupplierIdList?.Except(customerPriceDetail.CuPrSuppliers.Where(x => x.Active.Value).
                                                            Select(x => x.SupplierId).ToList());
            //unselected data from request to remove from DB
            var removeSupplierIdList = customerPriceDetail.CuPrSuppliers.Where(x => x.Active.Value
                                                                && !requestSupplierIdList.Contains(x.SupplierId));
            if (addSupplierIds != null)
            {
                //add
                foreach (var SupplierId in addSupplierIds)
                {
                    var SupplierEntity = new CuPrSupplier()
                    {
                        SupplierId = SupplierId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrSuppliers.Add(SupplierEntity);
                    _customerPriceCardRepository.AddEntity(SupplierEntity);
                }
            }
            if (removeSupplierIdList != null)
            {
                //remove
                foreach (var SupplierData in removeSupplierIdList)
                {
                    SupplierData.Active = false;
                    SupplierData.DeletedOn = DateTime.Now;
                    SupplierData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(SupplierData);
                }
            }
            //update - nothing
        }

        //country - add new data from reqest and remove existing data not selected in request
        private void UpdateCountry(IEnumerable<int> requestcountryIdList, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var addCountryIds = requestcountryIdList?.Except(customerPriceDetail.CuPrCountries.Where(x => x.Active.Value).
                                                            Select(x => x.FactoryCountryId).ToList());
            //unselected data from request to remove from DB
            var removeCountryIdList = customerPriceDetail.CuPrCountries.Where(x => x.Active.Value
                                                                && !requestcountryIdList.Contains(x.FactoryCountryId));
            if (addCountryIds != null)
            {
                //add
                foreach (var countryId in addCountryIds)
                {
                    var CountryEntity = new CuPrCountry()
                    {
                        FactoryCountryId = countryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrCountries.Add(CountryEntity);
                    _customerPriceCardRepository.AddEntity(CountryEntity);
                }
            }

            if (removeCountryIdList != null)
            {
                //remove
                foreach (var countryData in removeCountryIdList)
                {
                    countryData.Active = false;
                    countryData.DeletedOn = DateTime.Now;
                    countryData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(countryData);
                }
            }
            //update - nothing
        }

        //province
        private void UpdateProvince(IEnumerable<int> requestProvinceIdList, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var addProvinceIds = requestProvinceIdList?.Except(customerPriceDetail?.CuPrProvinces?.Where(x => x.Active.Value).
                                                            Select(x => x.FactoryProvinceId).ToList());
            //unselected data from request to remove from DB
            var removeProvinceIdList = customerPriceDetail?.CuPrProvinces?.Where(x => x.Active.Value
                                                                && !requestProvinceIdList.Contains(x.FactoryProvinceId));
            if (addProvinceIds != null)
            {
                //add
                foreach (var provinceId in addProvinceIds)
                {
                    var ProvinceEntity = new CuPrProvince()
                    {
                        FactoryProvinceId = provinceId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrProvinces.Add(ProvinceEntity);
                    _customerPriceCardRepository.AddEntity(ProvinceEntity);
                }
            }


            if (removeProvinceIdList != null)
            {
                //remove
                foreach (var provinceData in removeProvinceIdList)
                {
                    provinceData.Active = false;
                    provinceData.DeletedOn = DateTime.Now;
                    provinceData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(provinceData);
                }
            }

            //update - nothing
        }

        //remove province from DB
        private void RemoveProvince(CuPrDetail customerPriceDetail)
        {
            var removeProvinceIdList = customerPriceDetail?.CuPrProvinces?.Where(x => x.Active.Value);

            if (removeProvinceIdList != null)
            {
                //remove the record
                foreach (var provinceData in removeProvinceIdList)
                {
                    provinceData.Active = false;
                    provinceData.DeletedOn = DateTime.Now;
                    provinceData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(provinceData);
                }
            }
        }

        //customer brand
        private void UpdateCustomerBrand(IEnumerable<int> requestCustomerBrands, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var newCustomerBrands = requestCustomerBrands?.Except(customerPriceDetail.CuPrBrands.Where(x => x.Active.Value).
                                                            Select(x => x.BrandId.GetValueOrDefault()).ToList());
            //unselected data from request to remove from DB
            var removeCustomerBrands = customerPriceDetail?.CuPrBrands?.Where(x => x.Active.Value && x.BrandId != null
                                                                && !requestCustomerBrands.Contains(x.BrandId.GetValueOrDefault()));

            if (newCustomerBrands != null)
            {
                //add
                foreach (var brandId in newCustomerBrands)
                {
                    var brandEntity = new CuPrBrand()
                    {
                        BrandId = brandId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrBrands.Add(brandEntity);
                    _customerPriceCardRepository.AddEntity(brandEntity);
                }
            }

            if (removeCustomerBrands != null)
            {
                //remove
                foreach (var customerBrand in removeCustomerBrands)
                {
                    customerBrand.Active = false;
                    customerBrand.DeletedOn = DateTime.Now;
                    customerBrand.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(customerBrand);
                }
            }

            //update - nothing
        }

        //customer buyer
        private void UpdateCustomerBuyer(IEnumerable<int> requestCustomerBuyers, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var newCustomerBuyers = requestCustomerBuyers?.Except(customerPriceDetail.CuPrBuyers.Where(x => x.Active.Value && x.Id > 0).
                                                            Select(x => x.BuyerId.GetValueOrDefault()).ToList());
            //unselected data from request to remove from DB
            var removeCustomerBuyers = customerPriceDetail?.CuPrBuyers?.Where(x => x.Active.Value && x.BuyerId != null
                                                                && !requestCustomerBuyers.Contains(x.BuyerId.GetValueOrDefault()));

            if (newCustomerBuyers != null)
            {
                //add
                foreach (var buyerId in newCustomerBuyers)
                {
                    var buyerEntity = new CuPrBuyer()
                    {
                        BuyerId = buyerId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrBuyers.Add(buyerEntity);
                    _customerPriceCardRepository.AddEntity(buyerEntity);
                }
            }

            if (removeCustomerBuyers != null)
            {
                //remove
                foreach (var customerBuyer in removeCustomerBuyers)
                {
                    customerBuyer.Active = false;
                    customerBuyer.DeletedOn = DateTime.Now;
                    customerBuyer.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(customerBuyer);
                }
            }

            //update - nothing
        }

        //customer department
        private void UpdateCustomerDepartment(IEnumerable<int> requestCustomerDepartments, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var newCustomerDepartments = requestCustomerDepartments?.Except(customerPriceDetail.CuPrDepartments.Where(x => x.Active.Value).
                                                            Select(x => x.DepartmentId.GetValueOrDefault()).ToList());
            //unselected data from request to remove from DB
            var removeCustomerDepartments = customerPriceDetail?.CuPrDepartments?.Where(x => x.Active.Value && x.DepartmentId != null
                                                                && !requestCustomerDepartments.Contains(x.DepartmentId.GetValueOrDefault()));

            if (newCustomerDepartments != null)
            {
                //add
                foreach (var departmentId in newCustomerDepartments)
                {
                    var departmentEntity = new CuPrDepartment()
                    {
                        DepartmentId = departmentId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrDepartments.Add(departmentEntity);
                    _customerPriceCardRepository.AddEntity(departmentEntity);
                }
            }

            if (removeCustomerDepartments != null)
            {
                //remove
                foreach (var customerDepartment in removeCustomerDepartments)
                {
                    customerDepartment.Active = false;
                    customerDepartment.DeletedOn = DateTime.Now;
                    customerDepartment.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(customerDepartment);
                }
            }

            //update - nothing
        }

        //customer price category
        private void UpdateCustomerPriceCategory(IEnumerable<int> requestCustomerPriceCategories, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var newCustomerPriceCategories = requestCustomerPriceCategories?.Except(customerPriceDetail.CuPrPriceCategories.Where(x => x.Active.Value).
                                                            Select(x => x.PriceCategoryId.GetValueOrDefault()).ToList());
            //unselected data from request to remove from DB
            var removeCustomerPriceCategories = customerPriceDetail?.CuPrPriceCategories?.Where(x => x.Active.Value && x.PriceCategoryId != null
                                                                && !requestCustomerPriceCategories.Contains(x.PriceCategoryId.GetValueOrDefault()));

            if (newCustomerPriceCategories != null)
            {
                //add
                foreach (var priceCategoryId in newCustomerPriceCategories)
                {
                    var priceCategoryEntity = new CuPrPriceCategory()
                    {
                        PriceCategoryId = priceCategoryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrPriceCategories.Add(priceCategoryEntity);
                    _customerPriceCardRepository.AddEntity(priceCategoryEntity);
                }
            }

            if (removeCustomerPriceCategories != null)
            {
                //remove
                foreach (var priceCategory in removeCustomerPriceCategories)
                {
                    priceCategory.Active = false;
                    priceCategory.DeletedOn = DateTime.Now;
                    priceCategory.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(priceCategory);
                }
            }

            //update - nothing
        }

        //customer holiday type
        private void UpdateCustomerHolidayType(IEnumerable<int> requestCustomerHolidayTypes, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var newCustomerHolidayTypes = requestCustomerHolidayTypes?.Except(customerPriceDetail.CuPrHolidayTypes.Where(x => x.Active.Value).
                                                            Select(x => x.HolidayInfoId.GetValueOrDefault()).ToList());
            //unselected data from request to remove from DB
            var removeCustomerHolidayTypes = customerPriceDetail?.CuPrHolidayTypes?.Where(x => x.Active.Value && x.HolidayInfoId != null
                                                                && !requestCustomerHolidayTypes.Contains(x.HolidayInfoId.GetValueOrDefault()));

            if (newCustomerHolidayTypes != null)
            {
                //add
                foreach (var holidayTypeId in newCustomerHolidayTypes)
                {
                    var holidayTypeEntity = new CuPrHolidayType()
                    {
                        HolidayInfoId = holidayTypeId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrHolidayTypes.Add(holidayTypeEntity);
                    _customerPriceCardRepository.AddEntity(holidayTypeEntity);
                }
            }

            if (removeCustomerHolidayTypes != null)
            {
                //remove
                foreach (var holidayType in removeCustomerHolidayTypes)
                {
                    holidayType.Active = false;
                    holidayType.DeletedOn = DateTime.Now;
                    holidayType.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(holidayType);
                }
            }

            //update - nothing
        }


        private void UpdateInspectionLocations(IEnumerable<int> requestInspectionLocations, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var newInspectionLocations = requestInspectionLocations?.Except(customerPriceDetail.CuPrInspectionLocations.Where(x => x.Active.Value).
                                                            Select(x => x.InspectionLocationId.GetValueOrDefault()).ToList());
            //unselected data from request to remove from DB
            var removeInspectionLocations = customerPriceDetail?.CuPrInspectionLocations?.Where(x => x.Active.Value && x.InspectionLocationId != null
                                                                && !requestInspectionLocations.Contains(x.InspectionLocationId.GetValueOrDefault()));

            if (newInspectionLocations != null)
            {
                //add
                foreach (var locationId in newInspectionLocations)
                {
                    var inspectionLocation = new CuPrInspectionLocation()
                    {
                        InspectionLocationId = locationId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrInspectionLocations.Add(inspectionLocation);
                    _customerPriceCardRepository.AddEntity(inspectionLocation);
                }
            }

            if (removeInspectionLocations != null)
            {
                //remove
                foreach (var inspectionLocation in removeInspectionLocations)
                {
                    inspectionLocation.Active = false;
                    inspectionLocation.DeletedOn = DateTime.Now;
                    inspectionLocation.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(inspectionLocation);
                }
            }

        }



        /// <summary>
        /// Update Customer Price Invoice Request and Contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerPriceDetail"></param>
        private void UpdateCustomerPriceInvoiceRequest(CustomerPriceCard request, CuPrDetail customerPriceDetail)
        {

            if (request.InvoiceRequestList.Any())
            {

                // Remove Logic if data is not exist 
                var removeItems = new List<InvTranInvoiceRequest>();

                var itemsFromTheRequest = customerPriceDetail.InvTranInvoiceRequests.
                                                     Where(x => x.Active.HasValue && x.Active.Value &&
                                                     !request.InvoiceRequestList.Where(y => y.Id > 0)
                                                     .Select(z => z.Id).Contains(x.Id));

                foreach (var invRequest in itemsFromTheRequest)
                {
                    // delete contacts
                    foreach (var invRequestContact in invRequest.InvTranInvoiceRequestContacts.
                                             Where(x => x.Active == true && invRequest.Id == x.InvoiceRequestId && x.IsCommon == false))
                    {
                        invRequestContact.DeletedOn = DateTime.Now;
                        invRequestContact.DeletedBy = _applicationContext.UserId;
                        invRequestContact.Active = false;
                        _customerPriceCardRepository.EditEntity(invRequest);
                    }

                    invRequest.DeletedOn = DateTime.Now;
                    invRequest.DeletedBy = _applicationContext.UserId;
                    invRequest.Active = false;
                    removeItems.Add(invRequest);
                }
                if (removeItems.Any())
                    _customerPriceCardRepository.EditEntities(removeItems);

                // Update Logic for invoice request start
                var editInvoiceRequestList = new List<InvTranInvoiceRequest>();

                foreach (var invRequest in request.InvoiceRequestList.Where(x => x.Id > 0))
                {
                    var invEntity = customerPriceDetail.InvTranInvoiceRequests.FirstOrDefault(x => x.Active.Value && x.Id == invRequest.Id);

                    if (invEntity != null)
                    {
                        invEntity.BilledName = invRequest.BilledName;
                        invEntity.BilledAddress = invRequest.BilledAddress;
                        invEntity.UpdatedBy = _applicationContext.UserId;
                        invEntity.UpdatedOn = DateTime.Now;

                        // new records
                        var newCustomerPriceContacts = invRequest.InvoiceRequestContactList.Except(invEntity.
                                                          InvTranInvoiceRequestContacts.Where(x => x.Active.Value && x.IsCommon == false).
                                                                        Select(x => x.ContactId.GetValueOrDefault()).ToList());
                        // remove records
                        var removeCustomerPriceContacts = invEntity?.InvTranInvoiceRequestContacts.Where(x => x.Active.Value && x.IsCommon == false && x.ContactId != null
                                                                            && !invRequest.InvoiceRequestContactList.Contains(x.ContactId.GetValueOrDefault()));


                        foreach (var invRequestContactId in newCustomerPriceContacts)
                        {

                            var contactId = invRequest.InvoiceRequestContactList.FirstOrDefault(x => x == invRequestContactId);

                            InvTranInvoiceRequestContact newContact = new InvTranInvoiceRequestContact()
                            {
                                ContactId = contactId,
                                IsCommon = false,
                                Active = true,
                                CreatedOn = DateTime.Now,
                                CreatedBy = _applicationContext.UserId
                            };
                            invEntity.InvTranInvoiceRequestContacts.Add(newContact);
                            _customerPriceCardRepository.AddEntity(newContact);
                        }

                        foreach (var contact in removeCustomerPriceContacts)
                        {
                            contact.Active = false;
                            contact.UpdatedOn = DateTime.Now;
                            _customerPriceCardRepository.EditEntity(contact);
                        }

                        editInvoiceRequestList.Add(invEntity);
                    }
                }

                _customerPriceCardRepository.EditEntities(editInvoiceRequestList);

                AddCustomerPriceInvoiceRequest(request, customerPriceDetail);
            }
            else
            {
                // Remove Logic if data is not exist 
                var removeItems = new List<InvTranInvoiceRequest>();

                var itemsFromTheRequest = customerPriceDetail.InvTranInvoiceRequests.
                                                     Where(x => x.Active.HasValue && x.Active.Value);

                foreach (var invRequest in itemsFromTheRequest)
                {
                    // delete contacts
                    foreach (var invRequestContact in invRequest.InvTranInvoiceRequestContacts.
                                             Where(x => x.Active == true && invRequest.Id == x.InvoiceRequestId && x.IsCommon == false))
                    {
                        invRequestContact.DeletedOn = DateTime.Now;
                        invRequestContact.DeletedBy = _applicationContext.UserId;
                        invRequestContact.Active = false;
                        _customerPriceCardRepository.EditEntity(invRequest);
                    }

                    invRequest.DeletedOn = DateTime.Now;
                    invRequest.DeletedBy = _applicationContext.UserId;
                    invRequest.Active = false;
                    removeItems.Add(invRequest);
                }
                if (removeItems.Any())
                    _customerPriceCardRepository.EditEntities(removeItems);
            }
        }



        private void UpdatePriceSubcategory(CustomerPriceCard request, CuPrDetail customerPriceDetail)
        {

            if (request.SubCategoryList.Any() && !request.SubCategorySelectAll.GetValueOrDefault())
            {
                // Remove Logic if data is not exist 
                var removeItems = new List<CuPrTranSubcategory>();

                var itemsFromTheRequest = customerPriceDetail.CuPrTranSubcategories.
                                                     Where(x => x.Active.HasValue && x.Active.Value &&
                                                     !request.SubCategoryList.Where(y => y.Id > 0)
                                                     .Select(z => z.Id).Contains(x.Id));

                foreach (var subCatReq in itemsFromTheRequest)
                {
                    subCatReq.DeletedOn = DateTime.Now;
                    subCatReq.DeletedBy = _applicationContext.UserId;
                    subCatReq.Active = false;
                    removeItems.Add(subCatReq);
                }

                if (removeItems.Any())
                    _customerPriceCardRepository.EditEntities(removeItems);

                // Update Logic for invoice request start
                var editSubCategoryList = new List<CuPrTranSubcategory>();

                foreach (var subCatReq in request.SubCategoryList.Where(x => x.Id > 0))
                {
                    var subcategory = customerPriceDetail.CuPrTranSubcategories.FirstOrDefault(x => x.Active.Value && x.Id == subCatReq.Id);

                    if (subcategory != null)
                    {
                        // assign the items  
                        subcategory.MandayBuffer = subCatReq.MandayBuffer;
                        subcategory.MandayProductivity = subCatReq.MandayProductivity;
                        subcategory.MandayReports = subCatReq.MandayReports;
                        subcategory.UnitPrice = subCatReq.UnitPrice;

                        subcategory.AqlQty125 = subCatReq.AQL_QTY_125;
                        subcategory.AqlQty1250 = subCatReq.AQL_QTY_1250;
                        subcategory.AqlQty13 = subCatReq.AQL_QTY_13;
                        subcategory.AqlQty20 = subCatReq.AQL_QTY_20;
                        subcategory.AqlQty200 = subCatReq.AQL_QTY_200;
                        subcategory.AqlQty315 = subCatReq.AQL_QTY_315;
                        subcategory.AqlQty32 = subCatReq.AQL_QTY_32;
                        subcategory.AqlQty50 = subCatReq.AQL_QTY_50;
                        subcategory.AqlQty500 = subCatReq.AQL_QTY_500;
                        subcategory.AqlQty8 = subCatReq.AQL_QTY_8;
                        subcategory.AqlQty80 = subCatReq.AQL_QTY_80;
                        subcategory.AqlQty800 = subCatReq.AQL_QTY_800;

                        editSubCategoryList.Add(subcategory);
                    }
                }

                _customerPriceCardRepository.EditEntities(editSubCategoryList);

                // new items 
                AddCustomerPriceSubcategoryList(request, customerPriceDetail);
            }
            else
            {
                // Remove Logic if data is not exist 
                var removeItems = new List<CuPrTranSubcategory>();

                var itemsFromTheRequest = customerPriceDetail.CuPrTranSubcategories.
                                                     Where(x => x.Active.HasValue && x.Active.Value);

                foreach (var subcategory in itemsFromTheRequest)
                {
                    subcategory.DeletedOn = DateTime.Now;
                    subcategory.DeletedBy = _applicationContext.UserId;
                    subcategory.Active = false;
                    removeItems.Add(subcategory);
                }
                if (removeItems.Any())
                    _customerPriceCardRepository.EditEntities(removeItems);
            }
        }

        private void UpdatePriceRuleList(CustomerPriceCard request, CuPrDetail customerPriceDetail)
        {

            if (request.RuleList.Any())
            {
                // Remove Logic if data is not exist 
                var removeItems = new List<CuPrTranSpecialRule>();

                var itemsFromTheRequest = customerPriceDetail.CuPrTranSpecialRules.
                                                     Where(x => x.Active.HasValue && x.Active.Value &&
                                                     !request.RuleList.Where(y => y.Id > 0)
                                                     .Select(z => z.Id).Contains(x.Id));

                foreach (var specialRule in itemsFromTheRequest)
                {
                    specialRule.DeletedOn = DateTime.Now;
                    specialRule.DeletedBy = _applicationContext.UserId;
                    specialRule.Active = false;
                    removeItems.Add(specialRule);
                }

                if (removeItems.Any())
                    _customerPriceCardRepository.EditEntities(removeItems);

                // Update Logic for invoice request start
                var editSpecialRuleList = new List<CuPrTranSpecialRule>();

                foreach (var priceSpecialRule in request.RuleList.Where(x => x.Id > 0))
                {
                    var rule = customerPriceDetail.CuPrTranSpecialRules.FirstOrDefault(x => x.Active.Value && x.Id == priceSpecialRule.Id);

                    if (rule != null)
                    {
                        // assign the items  
                        rule.InterventionFee = priceSpecialRule.Interventionfee;
                        rule.PerInterventionRange1 = priceSpecialRule.PerInterventionRange1;
                        rule.PerInterventionRange2 = priceSpecialRule.PerInterventionRange2;
                        rule.MaxStylePerDay = priceSpecialRule.Max_Style_Per_Day;
                        rule.MaxStylePerMonth = priceSpecialRule.Max_Style_per_Month;
                        rule.MaxStylePerWeek = priceSpecialRule.Max_Style_Per_Week;
                        rule.MandayProductivity = priceSpecialRule.MandayProductivity;
                        rule.MandayReports = priceSpecialRule.MandayReports;
                        rule.PiecerateMinBilling = priceSpecialRule.Piecerate_MinBilling;
                        rule.PieceRateBillingQStart = priceSpecialRule.PieceRate_Billing_Q_Start;
                        rule.PiecerateBillingQEnd = priceSpecialRule.Piecerate_Billing_Q_End;
                        rule.UnitPrice = priceSpecialRule.UnitPrice;
                        rule.AdditionalFee = priceSpecialRule.AdditionalFee;

                        editSpecialRuleList.Add(rule);
                    }
                }

                _customerPriceCardRepository.EditEntities(editSpecialRuleList);

                // new items 
                AddCustomerPriceRuleList(request, customerPriceDetail);
            }
            else
            {
                // Remove Logic if data is not exist 
                var removeItems = new List<CuPrTranSpecialRule>();

                var itemsFromTheRequest = customerPriceDetail.CuPrTranSpecialRules.
                                                     Where(x => x.Active.HasValue && x.Active.Value);

                foreach (var ruleData in itemsFromTheRequest)
                {
                    ruleData.DeletedOn = DateTime.Now;
                    ruleData.DeletedBy = _applicationContext.UserId;
                    ruleData.Active = false;
                    removeItems.Add(ruleData);
                }

                if (removeItems.Any())
                    _customerPriceCardRepository.EditEntities(removeItems);
            }
        }


        /// <summary>
        /// Update invoice request contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerPriceDetail"></param>
        private void UpdateCustomerPriceInvoiceRequestContacts(CustomerPriceCard request, CuPrDetail customerPriceDetail)
        {

            if (request.InvoiceRequestContact.Any())
            {
                // new records
                var newCustomerPriceContacts = request.InvoiceRequestContact.Except(customerPriceDetail.InvTranInvoiceRequestContacts.
                                                                Where(x => x.Active.Value && x.IsCommon.Value).
                                                                Select(x => x.ContactId.GetValueOrDefault()).ToList());
                // remove records
                var removeCustomerPriceContacts = customerPriceDetail?.InvTranInvoiceRequestContacts.Where(x => x.Active.Value && x.IsCommon.Value && x.ContactId != null
                                                                    && !request.InvoiceRequestContact.Contains(x.ContactId.GetValueOrDefault()));


                foreach (var invRequestContactId in newCustomerPriceContacts)
                {

                    var contact = request.InvoiceRequestContact.FirstOrDefault(x => x == invRequestContactId);

                    InvTranInvoiceRequestContact newContact = new InvTranInvoiceRequestContact()
                    {
                        ContactId = contact,
                        IsCommon = true,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.InvTranInvoiceRequestContacts.Add(newContact);
                    _customerPriceCardRepository.AddEntity(newContact);
                }

                foreach (var contact in removeCustomerPriceContacts)
                {
                    contact.Active = false;
                    contact.UpdatedOn = DateTime.Now;
                    _customerPriceCardRepository.EditEntity(contact);
                }
            }
        }



        //save the customer price card details
        private async Task<int> AddCustomerPriceCard(CustomerPriceCard request)
        {
            try
            {
                var entity = new CuPrDetail()
                {
                    CustomerId = request.CustomerId,
                    BillingMethodId = request.BillingMethodId,
                    BillingToId = request.BillingToId,
                    ServiceId = request.ServiceId,
                    PriceComplexType = request.PriceComplexType,
                    UnitPrice = Math.Round(request.UnitPrice, 2),
                    Remarks = request.Remarks,
                    TaxIncluded = request.TaxIncluded,
                    TravelIncluded = request.TravelIncluded,
                    FreeTravelKm = request.FreeTravelKM,
                    CurrencyId = request.CurrencyId,
                    PeriodFrom = request.PeriodFrom?.ToDateTime(),
                    PeriodTo = request.PeriodTo?.ToDateTime(),
                    CreatedBy = _applicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    Active = true,
                    TravelMatrixTypeId = request.TariffTypeId,
                    HolidayPrice = request.HolidayPrice,
                    PriceToEachProduct = request.PriceToEachProduct,
                    ProductPrice = request.ProductPrice,

                    MaxProductCount = request.MaxProductCount,
                    SampleSizeBySet = request.SampleSizeBySet,
                    MinBillingDay = request.MinBillingDay,
                    MaxSampleSize = request.MaxSampleSize,
                    AdditionalSampleSize = request.AdditionalSampleSize,
                    AdditionalSamplePrice = request.AdditionalSamplePrice,
                    Quantity8 = request.Quantity8,
                    Quantity13 = request.Quantity13,
                    Quantity20 = request.Quantity20,
                    Quantity32 = request.Quantity32,
                    Quantity50 = request.Quantity50,
                    Quantity80 = request.Quantity80,
                    Quantity125 = request.Quantity125,
                    Quantity200 = request.Quantity200,
                    Quantity315 = request.Quantity315,
                    Quantity500 = request.Quantity500,
                    Quantity800 = request.Quantity800,
                    Quantity1250 = request.Quantity1250,

                    InvoiceRequestSelectAll = request.InvoiceRequestSelectAll,
                    IsInvoiceConfigured = request.IsInvoiceConfigured,

                    InvoiceRequestType = request.InvoiceRequestType,
                    InvoiceRequestAddress = request.InvoiceRequestAddress,
                    InvoiceRequestBilledName = request.InvoiceRequestBilledName,

                    BillingEntity = request.BillingEntity,
                    BankAccount = request.BankAccount,
                    PaymentDuration = request.PaymentDuration,
                    PaymentTerms = request.PaymentTerms,

                    InvoiceNoDigit = request.InvoiceNoDigit,
                    InvoiceNoPrefix = request.InvoiceNoPrefix,

                    InvoiceOffice = request.InvoiceOffice,

                    InvoiceInspFeeFrom = request.InvoiceInspFeeFrom,
                    InvoiceHotelFeeFrom = request.InvoiceHotelFeeFrom,
                    InvoiceDiscountFeeFrom = request.InvoiceDiscountFeeFrom,
                    InvoiceOtherFeeFrom = request.InvoiceOtherFeeFrom,
                    InvoiceTmfeeFrom = request.InvoiceTravelExpense,

                    IsSpecial = request.IsSpecial,
                    SubCategorySelectAll = request.SubCategorySelectAll,

                    MaxFeeStyle = request.MaxFeeStyle,
                    InvoiceSubject = request.InvoiceSubject,
                    BilledQuantityType = request.BillQuantityType,
                    InterventionType = request.InterventionType,
                    BillingFreequency = request.BillFrequency,

                    MandayBuffer = request.MandayBuffer,
                    ManDayProductivity = request.MandayProductivity,
                    MandayReportCount = request.MandayReports,

                    EntityId = _filterService.GetCompanyId()
                };

                AddProductCategoryList(request, entity);

                AddProductSubCategoryList(request, entity);

                AddSupplierList(request, entity);

                AddServiceTypesList(request, entity);

                AddFactoryCountryList(request, entity);

                AddFactoryProvinceList(request, entity);

                AddFactoryCityList(request, entity);

                AddCustomerBrandList(request, entity);

                AddCustomerBuyerList(request, entity);

                AddCustomerDepartmentList(request, entity);

                AddPriceCategoryList(request, entity);

                AddHolidayTypeList(request, entity);

                AddInspectionLocationList(request, entity);

                if (request.IsSpecial.HasValue && request.IsSpecial.Value)
                {
                    AddCustomerPriceRuleList(request, entity);
                }

                if (!request.SubCategorySelectAll.GetValueOrDefault())
                {
                    AddCustomerPriceSubcategoryList(request, entity);
                }

                if (request.IsInvoiceConfigured.HasValue && request.IsInvoiceConfigured.Value && request.InvoiceRequestType != (int)InvoiceRequestType.NotApplicable)
                    AddCustomerPriceInvoiceRequest(request, entity);

                if (request.IsInvoiceConfigured.HasValue && request.IsInvoiceConfigured.Value)
                    AddCustomerPriceInvoiceRequestContact(request, entity);

                _customerPriceCardRepository.AddEntity(entity);
                await _customerPriceCardRepository.Save();

                return entity.Id;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // add productcategory list
        private void AddProductCategoryList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.ProductCategoryIdList != null)
            {
                foreach (var productCategoryId in request.ProductCategoryIdList)
                {
                    var productCategoryEntity = new CuPrProductCategory()
                    {
                        ProductCategoryId = productCategoryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    entity.CuPrProductCategories.Add(productCategoryEntity);
                    _customerPriceCardRepository.AddEntity(productCategoryEntity);
                }
            }
        }

        // add product sub category list
        private void AddProductSubCategoryList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.ProductSubCategoryIdList != null)
            {
                foreach (var productSubCategoryId in request.ProductSubCategoryIdList)
                {
                    var productSubCategoryEntity = new CuPrProductSubCategory()
                    {
                        ProductSubCategoryId = productSubCategoryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    entity.CuPrProductSubCategories.Add(productSubCategoryEntity);
                    _customerPriceCardRepository.AddEntity(productSubCategoryEntity);
                }
            }
        }

        //add supplier list
        private void AddSupplierList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.SupplierIdList != null)
            {
                var _entityId = _filterService.GetCompanyId();
                foreach (var supplierId in request.SupplierIdList)
                {
                    var SupplierEntity = new CuPrSupplier()
                    {
                        SupplierId = supplierId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrSuppliers.Add(SupplierEntity);
                    _customerPriceCardRepository.AddEntity(SupplierEntity);
                }
            }
        }

        //add servicetype list
        private void AddServiceTypesList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.ServiceTypeIdList != null)
            {
                var _entityId = _filterService.GetCompanyId();
                foreach (var ServiceTypeId in request.ServiceTypeIdList)
                {
                    var ServiceTypeEntity = new CuPrServiceType()
                    {
                        ServiceTypeId = ServiceTypeId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrServiceTypes.Add(ServiceTypeEntity);
                    _customerPriceCardRepository.AddEntity(ServiceTypeEntity);
                }
            }


        }

        //factory country list
        private void AddFactoryCountryList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.FactoryCountryIdList != null)
            {
                //country
                var _entityId = _filterService.GetCompanyId();

                foreach (var countryId in request.FactoryCountryIdList)
                {
                    var CountryEntity = new CuPrCountry()
                    {
                        FactoryCountryId = countryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrCountries.Add(CountryEntity);
                    _customerPriceCardRepository.AddEntity(CountryEntity);
                }
            }

        }

        //factory province list
        private void AddFactoryProvinceList(CustomerPriceCard request, CuPrDetail entity)
        {
            //if country list == 1 then we have to insert province data or else we should not insert
            if (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Count() == CountryCount
                 && request.FactoryProvinceIdList != null)
            {
                //province
                var _entityId = _filterService.GetCompanyId();
                foreach (var provinceId in request.FactoryProvinceIdList)
                {
                    var ProvinceEntity = new CuPrProvince()
                    {
                        FactoryProvinceId = provinceId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrProvinces.Add(ProvinceEntity);
                    _customerPriceCardRepository.AddEntity(ProvinceEntity);
                }
            }
        }

        //factory city list
        private void AddFactoryCityList(CustomerPriceCard request, CuPrDetail entity)
        {
            //if country list == 1 then we have to insert province data or else we should not insert
            if (request.FactoryProvinceIdList != null && request.FactoryProvinceIdList.Count() == CountryCount
                  && request.FactoryCityIdList != null)
            {
                //city
                var _entityId = _filterService.GetCompanyId();
                foreach (var cityId in request.FactoryCityIdList)
                {
                    var cityEntity = new CuPrCity()
                    {
                        FactoryCityId = cityId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrCities.Add(cityEntity);
                    _customerPriceCardRepository.AddEntity(cityEntity);
                }
            }
        }

        //add customer brand list
        private void AddCustomerBrandList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.BrandIdList != null)
            {
                //brand
                var _entityId = _filterService.GetCompanyId();
                foreach (var brandId in request.BrandIdList)
                {
                    var BrandEntity = new CuPrBrand()
                    {
                        BrandId = brandId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrBrands.Add(BrandEntity);
                    _customerPriceCardRepository.AddEntity(BrandEntity);
                }
            }
        }

        //add customer buyer list
        private void AddCustomerBuyerList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.BuyerIdList != null)
            {
                //buyer
                var _entityId = _filterService.GetCompanyId();
                foreach (var buyerId in request.BuyerIdList)
                {
                    var BuyerEntity = new CuPrBuyer()
                    {
                        BuyerId = buyerId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrBuyers.Add(BuyerEntity);
                    _customerPriceCardRepository.AddEntity(BuyerEntity);
                }
            }
        }

        //add customer department list
        private void AddCustomerDepartmentList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.DepartmentIdList != null)
            {
                //department
                var _entityId = _filterService.GetCompanyId();
                foreach (var departmentId in request.DepartmentIdList)
                {
                    var DepartmentEntity = new CuPrDepartment()
                    {
                        DepartmentId = departmentId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrDepartments.Add(DepartmentEntity);
                    _customerPriceCardRepository.AddEntity(DepartmentEntity);
                }
            }
        }

        //add price category list
        private void AddPriceCategoryList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.PriceCategoryIdList != null)
            {
                //pricecategory
                var _entityId = _filterService.GetCompanyId();
                foreach (var priceCategoryId in request.PriceCategoryIdList)
                {
                    var PriceCategoryEntity = new CuPrPriceCategory()
                    {
                        PriceCategoryId = priceCategoryId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrPriceCategories.Add(PriceCategoryEntity);
                    _customerPriceCardRepository.AddEntity(PriceCategoryEntity);
                }
            }
        }

        //add holiday type list
        private void AddHolidayTypeList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.HolidayTypeIdList != null)
            {
                foreach (var holidayTypeId in request.HolidayTypeIdList)
                {
                    var HolidayTypeEntity = new CuPrHolidayType()
                    {
                        HolidayInfoId = holidayTypeId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrHolidayTypes.Add(HolidayTypeEntity);
                    _customerPriceCardRepository.AddEntity(HolidayTypeEntity);
                }
            }
        }

        //add inspection location list
        private void AddInspectionLocationList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.InspectionLocationList != null)
            {
                foreach (var locationId in request.InspectionLocationList)
                {
                    var inspectionLocation = new CuPrInspectionLocation()
                    {
                        InspectionLocationId = locationId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };
                    entity.CuPrInspectionLocations.Add(inspectionLocation);
                    _customerPriceCardRepository.AddEntity(inspectionLocation);
                }
            }
        }


        /// <summary>
        /// Add customer price card invoice transaction request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddCustomerPriceInvoiceRequest(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.InvoiceRequestList != null && request.InvoiceRequestList.Any())
            {
                foreach (var item in request.InvoiceRequestList.Where(x => x.Id == 0 && x.IsCommon))
                {
                    // map invoice request entity
                    var invoiceRequestEntity = _customerpricecardmap.MapInvoiceRequest(item);
                    invoiceRequestEntity.CreatedBy = _applicationContext.UserId;

                    // check any item is exist then proceed
                    if (item.InvoiceRequestContactList != null && item.InvoiceRequestContactList.Any())
                    {
                        foreach (var contact in item.InvoiceRequestContactList)
                        {
                            // map invoice request entity contacts
                            PriceInvoiceRequestContact contactRequest = new PriceInvoiceRequestContact()
                            {
                                ContactId = contact,
                                InvoiceRequestId = null,
                                IsCommon = false,
                                CuPriceCardId = invoiceRequestEntity.CuPriceCardId
                            };
                            var invoiceRequestContact = _customerpricecardmap.MapInvoiceRequestContact(contactRequest);
                            invoiceRequestContact.CreatedBy = _applicationContext.UserId;
                            invoiceRequestEntity.InvTranInvoiceRequestContacts.Add(invoiceRequestContact);
                            _customerPriceCardRepository.AddEntity(invoiceRequestContact);
                        }
                    }

                    // map invoice request entity contacts
                    entity.InvTranInvoiceRequests.Add(invoiceRequestEntity);
                    _customerPriceCardRepository.AddEntity(invoiceRequestEntity);
                }
            }
        }

        /// <summary>
        /// Add price rule List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddCustomerPriceRuleList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.RuleList != null && request.RuleList.Any())
            {
                foreach (var item in request.RuleList.Where(x => x.Id == 0))
                {
                    // map special rule request

                    var ruleEntity = new CuPrTranSpecialRule()
                    {
                        InterventionFee = item.Interventionfee,
                        PerInterventionRange1 = item.PerInterventionRange1,
                        PerInterventionRange2 = item.PerInterventionRange2,
                        MaxStylePerDay = item.Max_Style_Per_Day,
                        MaxStylePerMonth = item.Max_Style_per_Month,
                        MaxStylePerWeek = item.Max_Style_Per_Week,
                        MandayProductivity = item.MandayProductivity,
                        MandayReports = item.MandayReports,
                        PiecerateMinBilling = item.Piecerate_MinBilling,
                        PieceRateBillingQStart = item.PieceRate_Billing_Q_Start,
                        PiecerateBillingQEnd = item.Piecerate_Billing_Q_End,
                        AdditionalFee = item.AdditionalFee,
                        UnitPrice = item.UnitPrice,
                        Active = true,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.CuPrTranSpecialRules.Add(ruleEntity);

                    _customerPriceCardRepository.AddEntity(ruleEntity);
                }
            }
        }

        /// <summary>
        /// map price sub category list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddCustomerPriceSubcategoryList(CustomerPriceCard request, CuPrDetail entity)
        {
            if (request.SubCategoryList != null && request.SubCategoryList.Any())
            {

                foreach (var item in request.SubCategoryList.Where(x => x.Id == 0 && x.IsCommon))
                {
                    // map price sub category list

                    var priceSubcategory = new CuPrTranSubcategory()
                    {
                        SubCategory2Id = item.SubCategory2Id,
                        MandayBuffer = item.MandayBuffer,
                        MandayProductivity = item.MandayProductivity,
                        MandayReports = item.MandayReports,
                        UnitPrice = item.UnitPrice,


                        AqlQty125 = item.AQL_QTY_125,
                        AqlQty1250 = item.AQL_QTY_1250,
                        AqlQty13 = item.AQL_QTY_13,
                        AqlQty20 = item.AQL_QTY_20,
                        AqlQty200 = item.AQL_QTY_200,
                        AqlQty315 = item.AQL_QTY_315,
                        AqlQty32 = item.AQL_QTY_32,
                        AqlQty50 = item.AQL_QTY_50,
                        AqlQty500 = item.AQL_QTY_500,
                        AqlQty8 = item.AQL_QTY_8,
                        AqlQty80 = item.AQL_QTY_80,
                        AqlQty800 = item.AQL_QTY_800,

                        Active = true,
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    entity.CuPrTranSubcategories.Add(priceSubcategory);

                    _customerPriceCardRepository.AddEntity(priceSubcategory);
                }
            }
        }

        /// <summary>
        /// Add invoice Request contact
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddCustomerPriceInvoiceRequestContact(CustomerPriceCard request, CuPrDetail entity)
        {

            foreach (var contact in request.InvoiceRequestContact)
            {
                PriceInvoiceRequestContact contactRequest = new PriceInvoiceRequestContact()
                {
                    ContactId = contact,
                    InvoiceRequestId = null,
                    CuPriceCardId = entity.Id,
                    IsCommon = true
                };
                var invoiceRequestContact = _customerpricecardmap.MapInvoiceRequestContact(contactRequest);
                invoiceRequestContact.CreatedBy = _applicationContext.UserId;
                entity.InvTranInvoiceRequestContacts.Add(invoiceRequestContact);
                _customerPriceCardRepository.AddEntity(invoiceRequestContact);
            }
        }

        //summary data search based on request filters
        public async Task<CustomerPriceCardSummaryResponse> GetData(CustomerPriceCardSummary request)
        {
            ExportMapRequest paramList = new ExportMapRequest();

            if (request == null)
                return new CustomerPriceCardSummaryResponse() { Result = ResponseResult.RequestNotCorrectFormat };

            var response = new CustomerPriceCardSummaryResponse { Index = request.Index.GetValueOrDefault(), PageSize = request.pageSize.GetValueOrDefault() };
            try
            {
                if (request.Index == null || request.Index.Value <= 0)
                    request.Index = 1;

                if (request.pageSize == null || request.pageSize.Value == 0)
                    request.pageSize = 20;

                int skip = (request.Index.Value - 1) * request.pageSize.Value;

                int take = request.pageSize.Value;

                var data = _customerPriceCardRepository.GetAllData();

                if (request.PeriodFrom?.ToDateTime() != null && request.PeriodTo?.ToDateTime() != null)
                {
                    data = data.Where(x => !((x.PeriodFrom > request.PeriodTo.ToDateTime()) ||
                                                     (x.PeriodTo < request.PeriodFrom.ToDateTime())));
                }

                if (request != null && request.CustomerId > 0)
                {
                    data = data.Where(x => request.CustomerId == x.CustomerId);
                }

                if (request != null && request.BillingMethodId != null && request.BillingMethodId > 0)
                {
                    data = data.Where(x => request.BillingMethodId == x.BillingMethodId);
                }
                if (request != null && request.BillingToId != null && request.BillingToId > 0)
                {
                    data = data.Where(x => request.BillingToId == x.BillingToId);
                }
                if (request != null && request.ServiceId != null && request.ServiceId > 0)
                {
                    data = data.Where(x => request.ServiceId == x.ServiceId);
                }
                if (request != null && request.TaxIncluded != null)
                {
                    data = data.Where(x => request.TaxIncluded == x.TaxIncluded);
                }
                if (request != null && request.TravelIncluded != null)
                {
                    data = data.Where(x => request.TravelIncluded == x.TravelIncluded);
                }

                //var TotalData = await data.ToListAsync();

                if (request != null && request.ProductCategoryIdList != null && request.ProductCategoryIdList.Count() > 0)
                {
                    foreach (var item in request.ProductCategoryIdList)
                    {
                        data = data.Where(x => x.CuPrProductCategories.Any(y => y.Active.Value && y.ProductCategoryId == item));
                    }
                }

                if (request != null && request.ServiceTypeIdList != null && request.ServiceTypeIdList.Count() > 0)
                {
                    foreach (var item in request.ServiceTypeIdList)
                    {
                        data = data.Where(x => x.CuPrServiceTypes.Any(y => y.Active.Value && y.ServiceTypeId == item));
                    }
                }

                if (request != null && request.CountryIdList != null && request.CountryIdList.Count() > 0)
                {
                    foreach (var item in request.CountryIdList)
                    {
                        data = data.Where(x => x.CuPrCountries.Any(y => y.Active.Value && y.FactoryCountryId == item));
                    }
                }
                // department filter
                if (request.DepartmentIdList != null && request.DepartmentIdList.Count() > 0)
                {
                    foreach (var item in request.DepartmentIdList)
                    {
                        data = data.Where(x => x.CuPrDepartments.Any(y => y.Active.Value && y.Department.Id == item));
                    }
                }
                // price category filter
                if (request.PriceCategoryIdList != null && request.PriceCategoryIdList.Count() > 0)
                {
                    foreach (var item in request.PriceCategoryIdList)
                    {
                        data = data.Where(x => x.CuPrPriceCategories.Any(y => y.Active.Value && y.PriceCategory.Id == item));
                    }
                }

                // take total count after filter
                response.TotalCount = data.Count();

                if (response.TotalCount == 0)
                {
                    response.Result = ResponseResult.NotFound;
                    return response;
                }

                var result = data.Select(x => new CustomerPriceCardSummaryItem
                {
                    BillMethodName = x.BillingMethod.Label,
                    BillToName = x.BillingTo.Label,
                    CurrencyName = x.Currency.CurrencyName,
                    CustomerName = x.Customer.CustomerName,
                    Id = x.Id,
                    ServiceName = x.Service.Name,
                    UnitPrice = x.UnitPrice,
                    PeriodToDate = x.PeriodTo,
                    PeriodFromDate = x.PeriodFrom,
                    TravelIncludedBool = x.TravelIncluded,
                    TaxIncludedBool = x.TaxIncluded,
                    Remarks = x.Remarks,
                    FreeTraveKM = x.FreeTravelKm,
                    CreatedByName = x.CreatedByNavigation.FullName,
                    CreatedOn = x.CreatedOn.ToString(StandardDateFormat),
                    UpdatedByName = x.UpdatedByNavigation.FullName,
                    UpdatedOn = x.UpdatedOn.Value.ToString(StandardDateFormat)
                }).Skip(skip).Take(take).ToList();

                var cuPrIdList = result.Select(x => x.Id).Distinct().ToList();

                paramList.CountryData = await _customerPriceCardRepository.GetPrCountries(cuPrIdList);

                paramList.DeptData = await _customerPriceCardRepository.GetPrDepartment(cuPrIdList);

                paramList.PriceCategory = await _customerPriceCardRepository.GetPrPriceCategory(cuPrIdList);

                paramList.ServiceType = await _customerPriceCardRepository.GetPrServiceTypes(cuPrIdList);

                paramList.SupData = await _customerPriceCardRepository.GetPrSuppliers(cuPrIdList);

                paramList.ProductCategory = await _customerPriceCardRepository.GetPrProductCategories(cuPrIdList);

                paramList.ProvinceData = await _customerPriceCardRepository.GetPrProvince(cuPrIdList);

                if (result == null || !result.Any())
                    return new CustomerPriceCardSummaryResponse() { Result = ResponseResult.NotFound };

                var _resultdata = _customerpricecardmap.GetDataMap(result, paramList);

                //_resultdata = _resultdata.OrderByDescending(y => y.PeriodFrom).ThenBy(z => z.CustomerName);

                return new CustomerPriceCardSummaryResponse()
                {
                    Result = ResponseResult.Success,
                    TotalCount = response.TotalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0),
                    GetData = _resultdata
                };
            }
            catch (Exception ex)
            {
                return new CustomerPriceCardSummaryResponse() { Result = ResponseResult.Error };
                //throw ex;
            }
        }

        //delete logically
        public async Task<ResponseResult> Delete(int id)
        {
            try
            {
                //get saved user data values by id
                var customerPriceDetail = await _customerPriceCardRepository.GetCustomerPriceCardDetails(id);

                if (customerPriceDetail != null)
                {
                    customerPriceDetail.Active = false;
                    customerPriceDetail.DeletedBy = _applicationContext.UserId;
                    customerPriceDetail.DeletedOn = DateTime.Now;

                    if (customerPriceDetail.CuPrCountries != null)
                    {
                        foreach (var countryData in customerPriceDetail.CuPrCountries)
                        {
                            countryData.Active = false;
                            countryData.DeletedOn = DateTime.Now;
                            countryData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(countryData);
                        }
                    }

                    if (customerPriceDetail.CuPrProductCategories != null)
                    {
                        foreach (var provinceData in customerPriceDetail.CuPrProductCategories)
                        {
                            provinceData.Active = false;
                            provinceData.DeletedOn = DateTime.Now;
                            provinceData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(provinceData);
                        }
                    }

                    if (customerPriceDetail.CuPrProvinces != null)
                    {
                        foreach (var provinceData in customerPriceDetail.CuPrProvinces)
                        {
                            provinceData.Active = false;
                            provinceData.DeletedOn = DateTime.Now;
                            provinceData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(provinceData);
                        }
                    }

                    if (customerPriceDetail.CuPrServiceTypes != null)
                    {
                        foreach (var serviceTypeData in customerPriceDetail.CuPrServiceTypes)
                        {
                            serviceTypeData.Active = false;
                            serviceTypeData.DeletedOn = DateTime.Now;
                            serviceTypeData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(serviceTypeData);
                        }
                    }

                    if (customerPriceDetail.CuPrSuppliers != null)
                    {
                        foreach (var SupplierData in customerPriceDetail.CuPrSuppliers)
                        {
                            SupplierData.Active = false;
                            SupplierData.DeletedOn = DateTime.Now;
                            SupplierData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(SupplierData);
                        }
                    }

                    if (customerPriceDetail.CuPrBrands != null)
                    {
                        foreach (var BrandData in customerPriceDetail.CuPrBrands)
                        {
                            BrandData.Active = false;
                            BrandData.DeletedOn = DateTime.Now;
                            BrandData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(BrandData);
                        }
                    }

                    if (customerPriceDetail.CuPrBuyers != null)
                    {
                        foreach (var BuyerData in customerPriceDetail.CuPrBuyers)
                        {
                            BuyerData.Active = false;
                            BuyerData.DeletedOn = DateTime.Now;
                            BuyerData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(BuyerData);
                        }
                    }

                    if (customerPriceDetail.CuPrDepartments != null)
                    {
                        foreach (var DepartmentData in customerPriceDetail.CuPrDepartments)
                        {
                            DepartmentData.Active = false;
                            DepartmentData.DeletedOn = DateTime.Now;
                            DepartmentData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(DepartmentData);
                        }
                    }

                    if (customerPriceDetail.CuPrPriceCategories != null)
                    {
                        foreach (var priceCategoryData in customerPriceDetail.CuPrPriceCategories)
                        {
                            priceCategoryData.Active = false;
                            priceCategoryData.DeletedOn = DateTime.Now;
                            priceCategoryData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(priceCategoryData);
                        }
                    }

                    if (customerPriceDetail.CuPrHolidayTypes != null)
                    {
                        foreach (var HolidayTypeData in customerPriceDetail.CuPrHolidayTypes)
                        {
                            HolidayTypeData.Active = false;
                            HolidayTypeData.DeletedOn = DateTime.Now;
                            HolidayTypeData.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(HolidayTypeData);
                        }
                    }



                    if (customerPriceDetail.InvTranInvoiceRequestContacts != null)
                    {
                        foreach (var contact in customerPriceDetail.InvTranInvoiceRequestContacts.Where(x => x.Active == true && x.IsCommon == true))
                        {
                            contact.Active = false;
                            contact.DeletedOn = DateTime.Now;
                            contact.DeletedBy = _applicationContext.UserId;

                            _customerPriceCardRepository.EditEntity(contact);
                        }
                    }

                    if (customerPriceDetail.InvTranInvoiceRequests != null)
                    {
                        foreach (var invRequest in customerPriceDetail.InvTranInvoiceRequests.Where(x => x.Active.Value))
                        {

                            foreach (var invContact in invRequest.InvTranInvoiceRequestContacts.Where(x => x.Active == true && x.IsCommon == false))
                            {
                                invContact.Active = false;
                                invContact.DeletedOn = DateTime.Now;
                                invContact.DeletedBy = _applicationContext.UserId;
                                _customerPriceCardRepository.EditEntity(invContact);
                            }

                            invRequest.Active = false;
                            invRequest.DeletedOn = DateTime.Now;
                            invRequest.DeletedBy = _applicationContext.UserId;
                            _customerPriceCardRepository.EditEntity(invRequest);
                        }
                    }

                    _customerPriceCardRepository.EditEntity(customerPriceDetail);

                    await _customerPriceCardRepository.Save();
                }


                return ResponseResult.Success;

            }
            catch (Exception ex)
            {
                return ResponseResult.Error;
                //throw ex;
            }
        }

        //get unit price value based on customer and service id
        public async Task<IEnumerable<CustomerPriceCardDetails>> GetCustomerUnitPriceByCustomerIdServiceId(int customerId, int serviceId)
        {
            try
            {
                return null; //await _customerPriceCardRepository.GetCustomerUnitPriceByCustomerIdServiceId(customerId, serviceId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //export summary details
        public async Task<List<ExportSummary>> ExportSummary(CustomerPriceCardSummary request)
        {
            IEnumerable<ExportSummary> result = new List<ExportSummary>();
            ExportMapRequest mapRequest = new ExportMapRequest();

            var data = await GetData(request);

            var idList = data.GetData.Select(x => x.Id).Distinct().ToList();

            var priceCardDetails = await _customerPriceCardRepository.GetCustomerPriceCardDetailForExport(idList);

            mapRequest.SupData = await _customerPriceCardRepository.GetPrSuppliers(idList);

            mapRequest.ProductCategory = await _customerPriceCardRepository.GetPrProductCategories(idList);

            mapRequest.ServiceType = await _customerPriceCardRepository.GetPrServiceTypes(idList);

            mapRequest.CountryData = await _customerPriceCardRepository.GetPrCountries(idList);

            mapRequest.ProvinceData = await _customerPriceCardRepository.GetPrProvince(idList);

            mapRequest.DeptData = await _customerPriceCardRepository.GetPrDepartment(idList);

            mapRequest.BuyerData = await _customerPriceCardRepository.GetPrBuyer(idList);

            mapRequest.BrandData = await _customerPriceCardRepository.GetPrBrand(idList);

            mapRequest.PriceCategory = await _customerPriceCardRepository.GetPrPriceCategory(idList);

            mapRequest.HolidayType = await _customerPriceCardRepository.GetPrHolidayType(idList);

            mapRequest.Contact = await _customerPriceCardRepository.GetPrContacts(idList);

            var res = priceCardDetails.Select(x => _customerpricecardmap.MapExportSummary(x, mapRequest));

            return res.ToList();
        }


        /// <summary>
        /// Get the customer price holiday list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetCustomerPriceHolidayList()
        {
            var data = await _customerPriceCardRepository.GetCustomerPriceHolidayList();
            if (data == null || data.Count == 0)
                return new DataSourceResponse { DataSourceList = null, Result = DataSourceResult.CannotGetList };
            return new DataSourceResponse { DataSourceList = data, Result = DataSourceResult.Success };
        }

        //City
        private void UpdateCity(IEnumerable<int> requestCityIdList, CuPrDetail customerPriceDetail)
        {
            //get to add a new data from request
            var addCityIds = requestCityIdList?.Except(customerPriceDetail?.CuPrCities?.Where(x => x.Active.Value).
                                                            Select(x => x.FactoryCityId).ToList());
            //unselected data from request to remove from DB
            var removeCityIdList = customerPriceDetail?.CuPrCities?.Where(x => x.Active.Value
                                                                && !requestCityIdList.Contains(x.FactoryCityId));
            if (addCityIds != null)
            {
                //add
                foreach (var CityId in addCityIds)
                {
                    var CityEntity = new CuPrCity()
                    {
                        FactoryCityId = CityId,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId
                    };

                    customerPriceDetail.CuPrCities.Add(CityEntity);
                    _customerPriceCardRepository.AddEntity(CityEntity);
                }
            }


            if (removeCityIdList != null)
            {
                //remove
                foreach (var CityData in removeCityIdList)
                {
                    CityData.Active = false;
                    CityData.DeletedOn = DateTime.Now;
                    CityData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(CityData);
                }
            }

            //update - nothing
        }

        //remove City from DB
        private void RemoveCity(CuPrDetail customerPriceDetail)
        {
            var removeCityIdList = customerPriceDetail?.CuPrCities?.Where(x => x.Active.Value);

            if (removeCityIdList != null)
            {
                //remove the record
                foreach (var CityData in removeCityIdList)
                {
                    CityData.Active = false;
                    CityData.DeletedOn = DateTime.Now;
                    CityData.DeletedBy = _applicationContext.UserId;

                    _customerPriceCardRepository.EditEntity(CityData);
                }
            }
        }

    }
}