using AutoMapper;
using BI.Maps;
using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using CsvHelper;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.File;
using DTO.Master;
using DTO.PurchaseOrder;
using DTO.References;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static DTO.Common.ApiCommonData;

namespace BI
{
    public class CustomerProductManager : ICustomerProductManager
    {
        #region Declaration 
        private ICustomerProductRepository _productRepository = null;
        private IProductManagementManager _repoProduct = null;
        private IReferenceRepository _refRepository = null;
        private IFileManager _fileManager = null;
        private readonly IMapper _mapper;
        private ICacheManager _cache = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private static IConfiguration _configuration = null;
        private readonly IHelper _helper = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IInspectionBookingRepository _inspectionBookingRepository = null;
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerMap _customermap = null;
        private readonly OcrSettings _ocrSettings = null;
        #endregion Declaration

        #region Constructor
        public CustomerProductManager(ICustomerProductRepository productRepository, IProductManagementManager productManagementRepository, IReferenceRepository referenceRepository, IFileManager fileManager, ICacheManager cache,
                IMapper mapper, IAPIUserContext applicationContextService, IConfiguration configuration, IHelper helper, ITenantProvider filterService,
                IInspectionBookingRepository inspectionBookingRepository, ICustomerRepository customerRepository, IOptions<OcrSettings> ocrSettings)
        {
            _productRepository = productRepository;
            _repoProduct = productManagementRepository;
            _refRepository = referenceRepository;
            _fileManager = fileManager;
            _cache = cache;
            _mapper = mapper;
            _ApplicationContext = applicationContextService;
            _configuration = configuration;
            _helper = helper;
            _customermap = new CustomerMap();
            _filterService = filterService;
            _inspectionBookingRepository = inspectionBookingRepository;
            _customerRepository = customerRepository;
            _ocrSettings = ocrSettings.Value;
        }
        #endregion Constructor

        public async Task<CustomerProductSearchResponse> SearchCustomerProducts(CustomerProductSearchRequest request)
        {
            request.index = request.index.Value == 0 ? 1 : request.index.Value;
            var response = new CustomerProductSearchResponse { Index = request.index.Value, PageSize = request.pageSize.Value };

            var data = _productRepository.GetCustomerProducts();

            // fetch only style
            if (request.isStyle)
            {
                data = data.Where(x => x.IsStyle == request.isStyle);
            }
            else
            {
                // other than style it means products
                data = data.Where(x => x.IsStyle != true);
            }

            if (request.customerValue != null)
            {
                data = data.Where(x => x.CustomerId == request.customerValue);
            }
            if (request.productCategoryValue != null)
            {
                data = data.Where(x => x.ProductCategory == request.productCategoryValue);
            }
            if (request.productSubCategoryValue != null)
            {
                data = data.Where(x => x.ProductSubCategory == request.productSubCategoryValue);
            }
            if (request.productCategorySub2s != null && request.productCategorySub2s.Any())
            {
                var productTypeIds = request.productCategorySub2s.Select(x => x.Id);
                data = data.Where(x => x.ProductCategorySub2.HasValue && productTypeIds.Contains(x.ProductCategorySub2.Value));
            }
            if (request.productCategorySub3s != null && request.productCategorySub3s.Any())
            {
                data = data.Where(x => x.ProductCategorySub3.HasValue && request.productCategorySub3s.Contains(x.ProductCategorySub3.Value));
            }
            if (!string.IsNullOrEmpty(request.productValue))
            {
                data = data.Where(x => x.ProductId != null && x.ProductId.Trim().ToLower() == request.productValue.Trim().ToLower());
            }
            if (!string.IsNullOrEmpty(request.productDescription))
            {
                data = data.Where(x => x.ProductDescription != null && EF.Functions.Like(x.ProductDescription.Trim(), $"%{request.productDescription.Trim()}%"));
            }
            if (!string.IsNullOrEmpty(request.factoryReference))
            {
                data = data.Where(x => x.FactoryReference != null && EF.Functions.Like(x.FactoryReference.Trim(), $"%{request.factoryReference.Trim()}%"));
            }
            //add isnew product filter 
            if (request.isNewProduct.HasValue && request.isNewProduct.Value)
            {
                data = data.Where(x => x.IsNewProduct.HasValue && x.IsNewProduct.Value);
            }
            if (request.categorytypeid != null && request.categoryMapped != null)
            {
                data = GetInspectionQuerywithRequestFilters(request, data);
            }
            response.TotalCount = await data.CountAsync();

            if (response.TotalCount == 0)
            {
                response.Result = CustomerProductSearchResult.NotFound;
                return response;
            }

            int skip = (request.index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            response.Data = await data.Skip(skip).Take(request.pageSize.Value).Select(x => _mapper.Map<CustomerProductSearchData>(x)).ToListAsync();
            response.Result = CustomerProductSearchResult.Success;

            return response;


        }
        //Export Customer Products
        public async Task<CustomerProductExportDataResponse> CustomerProductsExportDetails(CustomerProductSearchRequest request)
        {
            var response = new CustomerProductExportDataResponse();

            var data = _productRepository.GetCustomerProductsExportData();
            if (request.customerValue != null)
            {
                data = data.Where(x => x.CustomerId == request.customerValue);
            }
            if (request.isStyle)
            {
                data = data.Where(x => x.IsStyle == request.isStyle);
            }
            if (request.productCategoryValue != null)
            {
                data = data.Where(x => x.ProductCategoryId == request.productCategoryValue);
            }
            if (request.productSubCategoryValue != null)
            {
                data = data.Where(x => x.ProductSubCategoryId == request.productSubCategoryValue);
            }
            if (request.productCategorySub2s != null && request.productCategorySub2s.Any())
            {
                var productTypeIds = request.productCategorySub2s.Select(x => x.Id);
                data = data.Where(x => x.ProductCategorySub2Id.HasValue && productTypeIds.Contains(x.ProductCategorySub2Id.Value));
            }
            if (request.productCategorySub3s != null && request.productCategorySub3s.Any())
            {
                data = data.Where(x => x.ProductCategorySub3Id.HasValue && request.productCategorySub3s.Contains(x.ProductCategorySub3Id.Value));
            }
            if (!string.IsNullOrEmpty(request.productValue))
            {
                data = data.Where(x => x.ProductId != null && x.ProductId.Trim().ToLower() == request.productValue.Trim().ToLower());
            }
            if (!string.IsNullOrEmpty(request.productDescription))
            {
                data = data.Where(x => x.ProductDescription != null && EF.Functions.Like(x.ProductDescription.Trim(), $"%{request.productDescription.Trim()}%"));
            }
            if (!string.IsNullOrEmpty(request.factoryReference))
            {
                data = data.Where(x => x.FactoryReference != null && EF.Functions.Like(x.FactoryReference.Trim(), $"%{request.factoryReference.Trim()}%"));
            }
            //add isnew product filter 
            if (request.isNewProduct.HasValue && request.isNewProduct.Value)
            {
                data = data.Where(x => x.IsNewProduct.HasValue && x.IsNewProduct.Value);
            }
            var customerProductDataList = await data.ToListAsync();

            response.CustomerProductExportData = customerProductDataList.Select(x => _customermap.GetCustomerProductExportData(x));

            if (response.CustomerProductExportData == null || !response.CustomerProductExportData.Any())
                response.Result = CustomerProductSearchResult.NotFound;
            else
                response.Result = CustomerProductSearchResult.Success;

            return response;


        }
        /// <summary>
        /// Save customer product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveCustomerProductResponse> SaveCustomerProduct(CustomerProduct request)
        {
            try
            {
                var response = new SaveCustomerProductResponse();

                if (request.Id == 0)
                {
                    // map customer product
                    var entity = MapCustomerProduct(request);

                    // map customer product file attachment
                    await MapCustomerProductFile(entity);

                    //add the customer product api services
                    AddCustomerProductAPIServices(entity, request);

                    // check already product is exist for the customer
                    var isProductExist = await _productRepository.CheckProductIsExistForThisCustomer(entity.CustomerId, entity.ProductId, request.Id);
                    if (!isProductExist)
                    {
                        await _productRepository.AddCustomerProducts(entity);
                        response.Id = entity.Id;
                        response.Result = SaveCustomerProductResult.Success;
                    }
                    else if (request.screenCallType == (int)ProductScreenCallType.Booking && isProductExist)
                    {
                        response.Id = await _productRepository.GetProductIdByNameAndCustomer(entity.CustomerId, entity.ProductId);

                        response.ProductList = await _productRepository.GetProductsByProductIds(new[] { response.Id }.ToList());
                        response.Result = SaveCustomerProductResult.Success;
                    }
                    else
                    {
                        return new SaveCustomerProductResponse { Result = SaveCustomerProductResult.CustomerProductExists };
                    }


                    return response;
                }
                else
                {
                    var entity = _productRepository.GetCustomerProductByID(request.Id);

                    if (entity == null)
                        return new SaveCustomerProductResponse { Result = SaveCustomerProductResult.CustomerProductIsNotFound };

                    var isProductExist = await _productRepository.CheckProductIsExistForThisCustomer
                                                                             (request.CustomerId, request.ProductId, entity.Id);
                    if (!isProductExist)
                    {
                        //attched files
                        request.CuProductFileAttachments = request?.CuProductFileAttachments ?? new HashSet<ProductAttachment>();

                        var newfiles = request.CuProductFileAttachments.Where(x => x.Id == 0);

                        //removed files
                        var fiIds = request.CuProductFileAttachments.Where(x => x.Id > 0).Select(x => x.Id);
                        var filesToremove = entity.CuProductFileAttachments.Where(x => !fiIds.Contains(x.Id));
                        var lstremove = new List<CuProductFileAttachment>();
                        var lstMsChartremove = new List<CuProductMschart>();
                        foreach (var fileItem in filesToremove.ToList())
                        {
                            fileItem.Active = false;
                            fileItem.DeletedOn = DateTime.Now;
                            fileItem.DeletedBy = _ApplicationContext.UserId;

                            //commented to remove mschart data

                            // clear ms chart if data avialable                      
                            foreach (var chart in fileItem.CuProductMscharts)
                            {
                                lstMsChartremove.Add(chart);
                            }

                            lstremove.Add(fileItem);
                        }
                        if (lstremove.Count > 0)
                            _productRepository.EditEntities(lstremove);


                        //commented to remove mschart data

                        if (lstMsChartremove.Count > 0)
                            _productRepository.RemoveEntities(lstMsChartremove);

                        //Updated
                        var filesToUpdate = request.CuProductFileAttachments.Where(x => x.Id > 0);
                        foreach (var fileItem in filesToUpdate)
                        {
                            var fileEntity = entity.CuProductFileAttachments.FirstOrDefault(x => x.Id == fileItem.Id);

                            if (fileEntity != null)
                            {
                                var newMsCharts = fileItem.ProductMsCharts.Where(x => x.Id == 0).ToList();
                                var updateMsCharts = fileItem.ProductMsCharts.Where(x => x.Id > 0).ToList();

                                //remove mschart data
                                var removeMsCharts = fileEntity.CuProductMscharts.Where(x => !updateMsCharts.Select(y => y.Id).Contains(x.Id)).ToList();

                                if (removeMsCharts.Count > 0)
                                    _productRepository.RemoveEntities(removeMsCharts);

                                //update mscharts data
                                foreach (var item in updateMsCharts)
                                {
                                    var productMschart = fileEntity.CuProductMscharts.FirstOrDefault(x => x.Id == item.Id);
                                    if (productMschart != null)
                                    {
                                        productMschart.Code = item.Code;
                                        productMschart.Code = item.Code;
                                        productMschart.Mpcode = item.Mpcode;
                                        productMschart.Required = item.Required;
                                        productMschart.Description = item.Description;
                                        productMschart.Tolerance1Up = item.Tolerance1Up;
                                        productMschart.Tolerance1Down = item.Tolerance1Down;
                                        productMschart.Tolerance2Up = item.Tolerance2Up;
                                        productMschart.Tolerance2Down = item.Tolerance2Down;
                                        productMschart.Sort = item.Sort;
                                        productMschart.UpdatedBy = _ApplicationContext.UserId;
                                        productMschart.UpdatedOn = DateTime.Now;
                                    }
                                    _productRepository.EditEntity(productMschart);
                                }

                                //add mscharts data
                                foreach (var item in newMsCharts)
                                {
                                    var chartData = new CuProductMschart()
                                    {
                                        Code = item.Code,
                                        Mpcode = item.Mpcode,
                                        Required = item.Required,
                                        Description = item.Description,
                                        Tolerance1Up = item.Tolerance1Up,
                                        Tolerance1Down = item.Tolerance1Down,
                                        Tolerance2Up = item.Tolerance2Up,
                                        Tolerance2Down = item.Tolerance2Down,
                                        Sort = item.Sort,
                                        CreatedBy = _ApplicationContext.UserId,
                                        CreatedOn = DateTime.Now
                                    };
                                    _productRepository.AddEntity(chartData);

                                    fileEntity.CuProductMscharts.Add(chartData);
                                    entity.CuProductMscharts.Add(chartData);
                                }
                            }
                        }

                        await AddFiles(newfiles, entity);


                        if ((entity.ProductCategory.HasValue && request.ProductCategory.HasValue && request.ProductCategory.Value != entity.ProductCategory.Value)
                            || (entity.ProductSubCategory.HasValue && request.ProductSubCategory.HasValue && request.ProductSubCategory.Value != entity.ProductSubCategory.Value)
                            || (entity.ProductCategorySub2.HasValue && request.ProductCategorySub2.HasValue && request.ProductCategorySub2.Value != entity.ProductCategorySub2.Value))
                        {
                            //update the inspection product categories
                            await UpdateInspectionProductCategories(entity, request);
                        }


                        _customermap.UpdateCustomerProductEntity(entity, request);

                        entity.UpdatedBy = _ApplicationContext.UserId;
                        entity.UpdatedOn = DateTime.Now;

                        //update the customer product api services
                        UpdateCustomerProductAPIServices(entity, request);

                        await _productRepository.EditCustomerProducts(entity);
                        response.Id = entity.Id;
                        if (response.Id > 0)
                        {
                            response.Result = SaveCustomerProductResult.Success;
                        }
                    }
                    else
                    {
                        return new SaveCustomerProductResponse { Result = SaveCustomerProductResult.CustomerProductExists };
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// when change product category, product sub category and product sub category 2 then update inspection data
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task UpdateInspectionProductCategories(CuProduct entity, CustomerProduct request)
        {
            //get the inspection of product id
            var inspections = await _inspectionBookingRepository.GetInspectionByProductId(entity.Id);
            //check any inspection available
            if (inspections.Any())
            {
                //check the product category will be changed
                if (entity.ProductCategory.HasValue && request.ProductCategory.HasValue && request.ProductCategory.Value != entity.ProductCategory.Value)
                {
                    // change the product category in inspections
                    inspections.ForEach(x => x.ProductCategoryId = request.ProductCategory.Value);
                }
                //check the product sub category will be changed
                if (entity.ProductSubCategory.HasValue && request.ProductSubCategory.HasValue && request.ProductSubCategory.Value != entity.ProductSubCategory.Value)
                {
                    // change the product sub category in inspections
                    inspections.ForEach(x => x.ProductSubCategoryId = request.ProductSubCategory.Value);
                }
                //check the product sub category 2 will be changed
                if (entity.ProductCategorySub2.HasValue && request.ProductCategorySub2.HasValue && request.ProductCategorySub2.Value != entity.ProductCategorySub2.Value)
                {
                    // change the product sub category 2 in inspections
                    inspections.ForEach(x => x.ProductSubCategory2Id = request.ProductCategorySub2.Value);
                }
                _inspectionBookingRepository.EditEntities(inspections);

            }

        }

        /// <summary>
        /// Map Customer Product Entity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private CuProduct MapCustomerProduct(CustomerProduct request)
        {
            CuProduct entity = _mapper.Map<CuProduct>(request);
            entity.CreatedBy = _ApplicationContext.UserId;
            entity.EntityId = _filterService.GetCompanyId();
            entity.CreatedTime = DateTime.Now;
            // remove empty character
            entity.ProductId = entity.ProductId.Trim();
            entity.ProductDescription = entity.ProductDescription.RemoveExtraSpace();
            entity.Active = true;

            if (request.screenCallType == (int)ProductScreenCallType.Product)
                entity.ItRemarks = "Created from product";
            else if (request.screenCallType == (int)ProductScreenCallType.Booking)
                entity.ItRemarks = "Created from booking";
            else if (request.screenCallType == (int)ProductScreenCallType.PurchaseOrder)
                entity.ItRemarks = "Created from purchase order page";

            return entity;
        }

        private async Task MapCustomerProductFile(CuProduct entity)
        {
            if (entity.CuProductFileAttachments.Count > 0)
            {
                foreach (var item in entity.CuProductFileAttachments)
                {
                    item.UserId = _ApplicationContext.UserId;
                    item.BookingId = null;
                    item.EntityId = _filterService.GetCompanyId();

                    foreach (var productMschart in item.CuProductMscharts)
                    {
                        productMschart.Product = entity;
                        productMschart.CreatedBy = _ApplicationContext.UserId;
                        productMschart.CreatedOn = DateTime.Now;
                    }
                    await AddMsChartFileData(item, entity);
                }
            }
        }

        /// <summary>
        /// Add MS chart file data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task AddMsChartFileData(CuProductFileAttachment item, CuProduct entity)
        {
            if (item.FileTypeId != null && item.FileTypeId == (int)ProductRefFileType.MSChartExcel)
            {
                // Add mschart data 
                List<MSChartUpload> uploadMsChartList = new List<MSChartUpload>();

                await _helper.SendRequestToPartnerAPI(Method.Get, item.FileUrl, null, "");


                //commented to add ms chart data

                //if (httpResponse.StatusCode == HttpStatusCode.OK)
                //{
                //    using (var stream = await httpResponse.Content.ReadAsStreamAsync())
                //    {
                //        using (MemoryStream mstream = new MemoryStream())
                //        {
                //            stream.CopyTo(mstream);
                //            // If you use EPPlus in a noncommercial context
                //            // according to the Polyform Noncommercial license:
                //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                //            using (ExcelPackage excelPackage = new ExcelPackage(mstream))
                //            {
                //                //loop all worksheets
                //                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                //                {

                //                    //var idx = worksheet
                //                    //            .Cells["1:1"]
                //                    //            .FirstOrDefault(c => c.Value.ToString() == "Code")
                //                    //            .Start
                //                    //            .Column;
                //                    //if (idx > 0)
                //                    //{

                //                    //}

                //                    string strPrevCode = string.Empty;

                //                    //loop all rows except first
                //                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                //                    {
                //                        if (!string.IsNullOrEmpty(worksheet.Cells[i, 1].Value?.ToString()))
                //                        {
                //                            strPrevCode = worksheet.Cells[i, 1].Value?.ToString();
                //                        }

                //                        uploadMsChartList.Add(new MSChartUpload()
                //                        {
                //                            Code = string.IsNullOrEmpty(worksheet.Cells[i, 1].Value?.ToString()) ? strPrevCode : worksheet.Cells[i, 1].Value?.ToString(),
                //                            Description = worksheet.Cells[i, 2].Value?.ToString(),
                //                            MpCode = worksheet.Cells[i, 3].Value?.ToString(),
                //                            Required = worksheet.Cells[i, 4].Value?.ToString(),
                //                            Tol1Up = worksheet.Cells[i, 5].Value?.ToString(),
                //                            Tol1Down = worksheet.Cells[i, 6].Value?.ToString(),
                //                            Tol2Up = worksheet.Cells[i, 7].Value?.ToString(),
                //                            Tol2Down = worksheet.Cells[i, 8].Value?.ToString(),
                //                        });


                //                    }
                //                }
                //            }
                //        }
                //    }
                //}


                // upload ms charts
                //if (uploadMsChartList.Any())
                //{
                //    int sort = 1;
                //    foreach (var chartInfo in uploadMsChartList)
                //    {
                //        var chartData = new CuProductMschart()
                //        {
                //            Code = chartInfo.Code,
                //            Description = chartInfo.Description,
                //            Mpcode = chartInfo.MpCode,
                //            Required = Double.Parse(chartInfo.Required),
                //            Tolerance1Up = Double.Parse(chartInfo.Tol1Up),
                //            Tolerance1Down = Double.Parse(chartInfo.Tol1Down),
                //            Tolerance2Up = Double.Parse(chartInfo.Tol2Up),
                //            Tolerance2Down = Double.Parse(chartInfo.Tol2Down),
                //            CreatedBy = _ApplicationContext.UserId,
                //            CreatedOn = DateTime.Now,
                //            Sort = sort
                //        };
                //        _productRepository.AddEntity(chartData);
                //        item.CuProductMscharts.Add(chartData);
                //        entity.CuProductMscharts.Add(chartData);
                //        sort++;
                //    }
                //}
            }
        }

        /// <summary>
        /// Add the api services for the customer product
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void AddCustomerProductAPIServices(CuProduct entity, CustomerProduct request)
        {
            if (request.ApiServiceIds != null)
            {
                entity.CuProductApiServices = new List<CuProductApiService>();
                foreach (var item in request.ApiServiceIds)
                {
                    var cuProductApiService = new CuProductApiService()
                    {
                        ServiceId = item,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    _productRepository.AddEntity(cuProductApiService);
                    entity.CuProductApiServices.Add(cuProductApiService);
                }
            }
        }

        /// <summary>
        /// Update the customer product api services
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void UpdateCustomerProductAPIServices(CuProduct entity, CustomerProduct request)
        {
            if (entity.CuProductApiServices == null)
                entity.CuProductApiServices = new List<CuProductApiService>();
            // find the item not exist in current list and update active
            foreach (var item in entity.CuProductApiServices.Where(x => !request.ApiServiceIds.Contains(x.ServiceId) && x.Active))
            {
                item.Active = false;
                item.DeletedBy = _ApplicationContext.UserId;
                item.DeletedOn = DateTime.Now;
            }
            if (request.ApiServiceIds != null)
            {
                // find item not exist in entity and add new item
                foreach (var item in request.ApiServiceIds.Where(x => !entity.CuProductApiServices.Where(z => z.Active).Select(y => y.ServiceId).Contains(x)))
                {
                    entity.CuProductApiServices.Add(new CuProductApiService
                    {
                        ServiceId = item,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }
            }
        }

        public async Task<SaveCustomerProductResponse> SaveCustomerProductList(List<CustomerProduct> requestList)
        {
            try
            {

                var response = new SaveCustomerProductResponse();
                //int successCount = 0;
                List<CuProduct> productEntityList = new List<CuProduct>();

                if (requestList != null && requestList.Any())
                {
                    var customerId = requestList[0].CustomerId;
                    var productNameList = requestList.Select(x => x.ProductId).ToList();

                    //var customerProductExistingList = await _productRepository.GetCustomerProductsByName(customerId, productNameList);

                    foreach (var request in requestList)
                    {
                        if (request.Id == 0)
                        {
                            var entity = MapCustomerProduct(request);

                            if (entity == null)
                                return new SaveCustomerProductResponse { Result = SaveCustomerProductResult.CustomerProductIsNotFound };


                            //add the customer product api services
                            AddCustomerProductAPIServices(entity, request);

                            productEntityList.Add(entity);

                        }
                    }

                    _productRepository.SaveList(productEntityList, false);

                    response.Result = SaveCustomerProductResult.Success;


                    return response;
                }

                response = new SaveCustomerProductResponse { Result = SaveCustomerProductResult.CustomerProductIsNotSaved };

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task AddFiles(IEnumerable<ProductAttachment> files, CuProduct product)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var ecFile = new CuProductFileAttachment
                    {
                        FileName = file.FileName?.Trim(),
                        UniqueId = file.uniqueld,
                        UploadDate = DateTime.Now,
                        UserId = _ApplicationContext.UserId,
                        Active = true,
                        BookingId = null,
                        FileUrl = file.FileUrl,
                        FileTypeId = file.FileTypeId,
                        EntityId = _filterService.GetCompanyId()
                    };

                    var msCharts = file.ProductMsCharts.ToList();
                    foreach (var item in msCharts)
                    {
                        var chartData = new CuProductMschart()
                        {
                            Code = item.Code,
                            Mpcode = item.Mpcode,
                            Required = item.Required,
                            Description = item.Description,
                            Tolerance1Up = item.Tolerance1Up,
                            Tolerance1Down = item.Tolerance1Down,
                            Tolerance2Up = item.Tolerance2Up,
                            Tolerance2Down = item.Tolerance2Down,
                            Sort = item.Sort,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        };
                        _productRepository.AddEntity(chartData);

                        ecFile.CuProductMscharts.Add(chartData);
                        product.CuProductMscharts.Add(chartData);
                    }
                    product.CuProductFileAttachments.Add(ecFile);
                    await AddMsChartFileData(ecFile, product);
                }
            }

        }

        public EditCustomerProductResponse GetEditCustomerProduct(int id)
        {
            var response = new EditCustomerProductResponse();

            var CustomerProduct = _productRepository.GetCustomerProductByID(id);
            if (CustomerProduct != null)
            {
                var CustomerProductDetails = _mapper.Map<CustomerProduct>(CustomerProduct);

                // Fetch only active file attachments.
                CustomerProductDetails.CuProductFileAttachments = CustomerProductDetails.CuProductFileAttachments.Where(x => x.Active);

                CustomerProductDetails.ApiServiceIds = CustomerProduct.CuProductApiServices.Where(x => x.Active).Select(x => x.ServiceId).ToList();

                if (CustomerProduct.InspProductTransactionProducts != null && CustomerProduct.InspProductTransactionProducts.Any())
                {
                    CustomerProductDetails.isProductBooked = CustomerProduct.InspProductTransactionProducts.Select(x => x.InspectionId).Distinct().Count() >= 2 ? true : false;
                }

                foreach (var item in CustomerProductDetails.CuProductFileAttachments.Where(x => x.Active))
                {
                    item.MimeType = _fileManager.GetMimeType(Path.GetExtension(item.FileName));
                }
                response.CustomerProductDetails = CustomerProductDetails;
                if (response.CustomerProductDetails == null)
                    return new EditCustomerProductResponse { Result = EditCustomerProductResult.CannotGetCustomerProduct };

            }
            response.Result = EditCustomerProductResult.Success;
            return response;
        }

        public async Task<CustomerProductDeleteResponse> RemoveCustomerProduct(int id)
        {
            var IsproductmapToInsp = await _productRepository.CheckProductInspExists(id);
            if (IsproductmapToInsp)
            {
                return new CustomerProductDeleteResponse { Id = id, Result = CustomerProductDeleteResult.MappedtoInsp };
            }

            var customerProduct = _productRepository.GetCustomerProductByID(id);

            if (customerProduct == null)
                return new CustomerProductDeleteResponse { Id = id, Result = CustomerProductDeleteResult.NotFound };

            await _productRepository.RemoveCustomerProducts(id);

            return new CustomerProductDeleteResponse { Id = id, Result = CustomerProductDeleteResult.Success };

        }

        public IEnumerable<CustomerProduct> GetCustomerProductsByCustomer(int customerId)
        {
            var data = _productRepository.GetCustomerProductsByCustomers(customerId);

            var customerProducts = data.Select(x => _mapper.Map<CustomerProduct>(x)).ToArray();

            return customerProducts;
        }

        public IEnumerable<CustomerProduct> GetProductsByCustomerAndCategory(int customerId, int productCategoryId)
        {
            var data = _productRepository.GetProductsByCustomerAndCategory(customerId, productCategoryId);

            var customerProducts = data.Select(x => _mapper.Map<CustomerProduct>(x)).ToArray();

            return customerProducts;
        }

        /// <summary>
        /// get file type list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetFileTypeList()
        {
            var response = new DataSourceResponse();
            response.DataSourceList = await _productRepository.GetFileTypeList();

            if (!response.DataSourceList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.Success };

            response.Result = DataSourceResult.Success;

            return response;
        }

        public async Task<CustomerProductDataSourceResponse> GetCustomerProductDataSourceList(CustomerProductDataSourceRequest request)
        {
            try
            {
                var response = new CustomerProductDataSourceResponse() { Result = DataSourceResult.CannotGetList };

                var data = _productRepository.GetCustomerProductDataSource();

                //apply search text
                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    data = data.Where(x => x.ProductId != null && EF.Functions.Like(x.ProductId, $"%{request.SearchText.Trim()}%"));
                }

                //apply Product id
                if (request.ProductIds != null && request.ProductIds.Any())
                    data = data.Where(x => request.ProductIds.Contains(x.Id));

                //apply customer filter
                if (request.CustomerIds != null && request.CustomerIds.Any())
                    data = data.Where(y => request.CustomerIds.Contains(y.CustomerId));


                if (request.SupplierIdList != null && request.SupplierIdList.Any())
                {
                    data = data.Where(x => x.CuPurchaseOrderDetails.Any(y => y.Active.Value && y.Po.CuPoSuppliers.Any(z => z.Active.HasValue && z.Active.Value && request.SupplierIdList.Contains(z.SupplierId))));
                }

                if (request.FactoryIdList != null && request.FactoryIdList.Any())
                {
                    data = data.Where(x => x.CuPurchaseOrderDetails.Any(y => y.Active.Value && y.Po.CuPoFactories.Any(z => z.Active.HasValue && z.Active.Value && request.FactoryIdList.Contains(z.FactoryId) || z.FactoryId == null)));
                }


                //execute the data
                var CustomerProductList = await data.Skip(request.Skip).Take(request.Take).
                                    Select(x => new CommonCustomerProductDataSource()
                                    {
                                        Id = x.Id,
                                        ProductId = x.ProductId,
                                        ProductDescription = x.ProductDescription
                                    }).AsNoTracking().ToListAsync();

                if (CustomerProductList != null && CustomerProductList.Any())
                {
                    response.DataSourceList = CustomerProductList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the product details by customers
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerProductDetailResponse> GetProductDetailsByCustomer(PoProductRequest poProductRequest)
        {
            if (poProductRequest.ProductIds != null && poProductRequest.ProductIds.Any())
            {
                var productList = await _productRepository.GetProductsByProductIds(poProductRequest.ProductIds);
                if (productList != null && productList.Any())
                {
                    return new CustomerProductDetailResponse() { ProductList = productList, Result = CustomerProductDetailResult.Success };
                }
            }
            return new CustomerProductDetailResponse() { Result = CustomerProductDetailResult.CannotGetProducts };
        }

        public async Task<List<string>> GetProductNameByProductIds(IEnumerable<int> productIds)
        {
            return await _productRepository.GetProductNameByProductIds(productIds);
        }
        public IQueryable<CuProduct> GetInspectionQuerywithRequestFilters(CustomerProductSearchRequest request, IQueryable<CuProduct> customerProductQuery)
        {
            //filter by creation date or service date or firstservicedate
            if (Enum.TryParse(request.categorytypeid.ToString(), out CustomerProductSearch _searchtype))
            {
                switch (_searchtype)
                {
                    case CustomerProductSearch.Category:
                        {
                            if (request.categoryMapped == (int)CustomerProductMapped.Mapped)
                            {
                                customerProductQuery = customerProductQuery.Where(x => x.ProductCategory.HasValue && x.ProductCategory > 0 && x.Active);
                            }
                            else
                            {
                                customerProductQuery = customerProductQuery.Where(x => !x.ProductCategory.HasValue && x.Active);
                            }
                            break;
                        }
                    case CustomerProductSearch.SubCategory:
                        {
                            if (request.categoryMapped == (int)CustomerProductMapped.Mapped)
                            {
                                customerProductQuery = customerProductQuery.Where(x => x.ProductSubCategory.HasValue && x.ProductSubCategory > 0 && x.Active);
                            }
                            else
                            {
                                customerProductQuery = customerProductQuery.Where(x => !x.ProductSubCategory.HasValue && x.Active);
                            }
                            break;
                        }
                    case CustomerProductSearch.SubCategory2:
                        {
                            if (request.categoryMapped == (int)CustomerProductMapped.Mapped)
                            {
                                customerProductQuery = customerProductQuery.Where(x => x.ProductCategorySub2.HasValue && x.ProductCategorySub2 > 0 && x.Active);
                            }
                            else
                            {
                                customerProductQuery = customerProductQuery.Where(x => !x.ProductCategorySub2.HasValue && x.Active);
                            }
                            break;
                        }
                    case CustomerProductSearch.SubCategory3:
                        {
                            if (request.categoryMapped == (int)CustomerProductMapped.Mapped)
                            {
                                customerProductQuery = customerProductQuery.Where(x => x.ProductCategorySub3.HasValue && x.ProductCategorySub3 > 0 && x.Active);
                            }
                            else
                            {
                                customerProductQuery = customerProductQuery.Where(x => !x.ProductCategorySub3.HasValue && x.Active);
                            }
                            break;
                        }
                }
            }
            return customerProductQuery;
        }

        /// <summary>
        /// Get the product file urls
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<CustomerProductFileResponse> GetProductImageUrls(int productId)
        {
            var response = new CustomerProductFileResponse() { Result = CustomerProductFileResult.FileNotFound };

            //get the product file urls
            var productFileUrls = await _productRepository.GetProductImageUrls(productId);

            if (productFileUrls.Any())
            {
                response.ProductFileUrls = productFileUrls;
                response.Result = CustomerProductFileResult.Success;
            }

            return response;
        }

        /// <summary>
        /// Get the customer product extended data source list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<POProductListResponse> GetCustomerProductDetailsDataSourceList(CustomerProductDetailsDataSourceRequest request)
        {
            try
            {
                var response = new POProductListResponse();

                //Get the purchase order details Iqueryable
                var data = _productRepository.GetCustomerProductDataSource();

                //apply the po number search text filter
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => EF.Functions.Like(x.ProductId, $"%{request.SearchText.Trim()}%"));
                }

                //apply customer filter
                if (request.CustomerIds != null && request.CustomerIds.Any())
                    data = data.Where(y => request.CustomerIds.Contains(y.CustomerId));

                //execute the product list
                var productList = await data.Skip(request.Skip).Take(request.Take).
                                        Select(x => new POProductList()
                                        {
                                            Id = x.Id,
                                            Name = x.ProductId,
                                            Description = x.ProductDescription,
                                            ProductCategoryId = x.ProductCategory,
                                            ProductCategoryName = x.ProductCategoryNavigation.Name,
                                            ProductSubCategoryId = x.ProductSubCategory,
                                            ProductSubCategoryName = x.ProductSubCategoryNavigation.Name,
                                            ProductSubCategory2Id = x.ProductCategorySub2,
                                            ProductSubCategory2Name = x.ProductCategorySub2Navigation.Name,
                                            ProductSubCategory3Id = x.ProductCategorySub3,
                                            ProductSubCategory3Name = x.ProductCategorySub3Navigation.Name,
                                            BarCode = x.Barcode,
                                            FactoryReference = x.FactoryReference,
                                            IsNewProduct = x.IsNewProduct,
                                            Remarks = x.Remarks,
                                            ProductImageCount = x.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).Select(x => x.Id).Count()

                                        }).AsNoTracking().ToListAsync();

                if (productList == null || !productList.Any())
                    response.Result = POProductResult.NotFound;

                else
                {
                    response.ProductList = productList;
                    response.Result = POProductResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                return null;
            }
        }


        #region 

        public async Task<CustomerProductRefResponse> SaveCustomerProductRef(CustomerProductRefRequest request)
        {
            var response = new CustomerProductRefResponse();

            try
            {

                var customerId = _ApplicationContext.CustomerId;

                //var customerId = 59;

                //Check CustomerId exist in System
                var customer = await _customerRepository.GetCustomerItemByIdForCFL(customerId);
                if (customer == null)
                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };

                var existCustomerProduct = await _productRepository.GetProductRefByCustomer(customerId, request.ProductReference);

                if (existCustomerProduct != null)
                {
                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "Record already exists" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                var cuProduct = MapExternalCustomerProductrefdata(request, customerId);
                //map the product category and unit data
                await MapExternalProductCategoryAndUnit(cuProduct, request);
                _productRepository.AddEntity(cuProduct);
                await _productRepository.Save();
                response.message = "Success";
                response.statusCode = HttpStatusCode.Created;

            }
            catch (Exception ex)
            {
                return new CustomerProductRefResponse()
                {
                    errors = new List<string>() { CustomerReportDetailsErrorMessages.InternalServerError },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
            return response;
        }

        public async Task<CustomerProductRefResponse> UpdateCustomerProductRef(CustomerProductRefRequest request)
        {
            var response = new CustomerProductRefResponse();
            try
            {

                var customerId = _ApplicationContext.CustomerId;

                var customer = await _customerRepository.GetCustomerItemByIdForCFL(customerId);

                if (customer == null)

                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };

                var customerProduct = await _productRepository.GetProductRefByCustomer(customerId, request.ProductReference);

                if (customerProduct == null)
                {
                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "No Record Found" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                //map the request to entity
                MapExternalCustomerProductRef(customerProduct, request);

                //map the product category and unit data
                await MapExternalProductCategoryAndUnit(customerProduct, request);

                _productRepository.EditEntity(customerProduct);
                await _productRepository.Save();
                response.message = "Success";
                response.statusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                return new CustomerProductRefResponse()
                {
                    errors = new List<string>() { CustomerReportDetailsErrorMessages.InternalServerError },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
            return response;
        }

        /// <summary>
        /// map the request data to entity class, when update the customer product,
        /// ignore the product ref
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void MapExternalCustomerProductRef(CuProduct entity, CustomerProductRefRequest request)
        {
            entity.ProductDescription = request.ProductRefDescription?.Trim();
            entity.Barcode = request.Barcode;
            entity.FactoryReference = request.FactoryReference;
            entity.Unit = Convert.ToInt32(request.Unit);
            entity.Remarks = request.Remarks;
            entity.UpdatedBy = _ApplicationContext.UserId;
            entity.UpdatedOn = DateTime.Now;
        }


        private CuProduct MapExternalCustomerProductrefdata(CustomerProductRefRequest request, int customerId)
        {
            return new CuProduct
            {
                ProductId = request.ProductReference,
                ProductDescription = request.ProductRefDescription,
                CustomerId = customerId,
                Barcode = request.Barcode,
                FactoryReference = request.FactoryReference,
                Remarks = request.Remarks,
                CreatedTime = DateTime.Now,
                CreatedBy = customerId,
                Active = true,
                EntityId = _filterService.GetCompanyId()
            };
        }

        /// <summary>
        /// map the product category, sub category, sub category 2 and unit data for create and update 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task MapExternalProductCategoryAndUnit(CuProduct entity, CustomerProductRefRequest request)
        {
            if (request.Category != null)
            {
                // Get Api Link Product Sub Category Id from Customer's sub Product categroy
                var customerProduct = await _productRepository.GetSubCategoryIdFromCustomerCategory(request.SubCategory);
                entity.ProductCategory = customerProduct?.ProductCategoryId;
                entity.ProductSubCategory = customerProduct?.ProductSubCategoryId;
            }
            if (request.ProductType != null)
            {
                // Get Api Link Product Sub Two Category Id from Customer's Product Type
                var productType = await _productRepository.GetSubCategoryTwoIdFromCustomerCategory(request.ProductType);
                entity.ProductCategorySub2 = productType?.LinkProductType;
            }

            if (request.Unit != null)
            {
                // Get Unit by unit name
                var unit = await _inspectionBookingRepository.GetUnitByName(request.Unit);
                entity.Unit = unit?.Id;
            }
        }
        #endregion

        #region Delete
        public async Task<CustomerProductRefResponse> DeleteCustomerProductRef(string productReference)
        {
            try
            {
                // Check  ProductReferance Exist for Customer 
                var customerId = _ApplicationContext.CustomerId;

                var customer = await _customerRepository.GetCustomerItemByIdForCFL(customerId);
                if (customer == null)
                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };

                var customerProduct = await _productRepository.GetProductRefByCustomer(customerId, productReference);

                if (customerProduct == null)
                {
                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "no record found" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                // Check Po used any Product 
                var isPoExist = await _productRepository.CheckPOExist(customerProduct.Id);
                if (isPoExist)
                {
                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "product ref can’t delete because it is map to PO " },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                // Remove ProductReferanceDetails
                customerProduct.Active = false;
                customerProduct.DeletedBy = _ApplicationContext.UserId;
                customerProduct.DeletedTime = DateTime.Now;

                _productRepository.EditEntity(customerProduct);
                await _productRepository.Save();
                return new CustomerProductRefResponse()
                {
                    errors = new List<string>() { "Success" },
                    message = "Success",
                    statusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new CustomerProductRefResponse()
                {
                    errors = new List<string>() { "InternalServerError" },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
        }


        public async Task<SaveCustomerProductResponses> GetProductReferance(string productReference)
        {
            var response = new SaveCustomerProductResponses();

            try
            {
                //get Cutomer Id from Claim
                var customerId = _ApplicationContext.CustomerId;

                var customer = await _customerRepository.GetCustomerItemByIdForCFL(customerId);
                if (customer == null)
                    return new SaveCustomerProductResponses
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };


                // Check  ProductReferance Exist for Customer 
                var customerProduct = await _productRepository.GetProductFileList(customerId, productReference);


                if (customerProduct != null)
                {
                    return new SaveCustomerProductResponses
                    {
                        data = customerProduct,
                        errors = new List<string>() { "Success" },
                        message = "Success",
                        statusCode = System.Net.HttpStatusCode.OK,
                    };

                }
                else
                {
                    return new SaveCustomerProductResponses
                    {
                        errors = new List<string>() { "no record found" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };

                }


            }
            catch (Exception ex)
            {
                return new SaveCustomerProductResponses()
                {
                    errors = new List<string>() { "internal Server Problem" },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
        }

        public async Task<CustomerProductRefResponse> DeleteProductFileAttachments(string uniqueId)
        {
            try
            {
                var fileData = _inspectionBookingRepository.GetProductFile(uniqueId);

                if (fileData == null)
                {
                    return new CustomerProductRefResponse
                    {
                        errors = new List<string>() { "no record found" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                if (fileData.FileTypeId == (int)ProductRefFileType.MSChartExcel)
                {
                    var isAnyProductReportInProgress = await _inspectionBookingRepository.IsAnyProductReportInProgress(fileData.ProductId);
                    if (isAnyProductReportInProgress)
                    {
                        return new CustomerProductRefResponse
                        {
                            errors = new List<string>() { "MS chart can’t delete because Report in progress" },
                            message = "Bad Request",
                            statusCode = System.Net.HttpStatusCode.BadRequest
                        };
                    }
                }

                fileData.Active = false;
                fileData.DeletedBy = _ApplicationContext.UserId;
                fileData.DeletedOn = DateTime.Now;
                _productRepository.EditEntity(fileData);
                await _productRepository.Save();


                return new CustomerProductRefResponse()
                {
                    statusCode = System.Net.HttpStatusCode.OK,
                    message = "Success"
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UploadCustomerProductResponse> UploadProductAttachment(CustomerProductFileUpload request)
        {
            var response = new UploadCustomerProductResponse();

            try
            {
                long fileSizeInMB = request.File.Length / 1024;
                FileInfo fi = new FileInfo(request.File.FileName);
                if (request.FileType == (int)ProductRefFileType.MSChartExcel)
                {
                    if (fi.Extension != ".xlsx")
                        return new UploadCustomerProductResponse()
                        {
                            errors = new List<string>() { "Only xsls format accepted" },
                            statusCode = HttpStatusCode.BadRequest,
                            message = "BadRequest"
                        };

                    if (fileSizeInMB > (10 * 1024))
                        return new UploadCustomerProductResponse()
                        {
                            errors = new List<string>() { "File shouldn’t greater than 10 MB" },
                            statusCode = HttpStatusCode.BadRequest,
                            message = "BadRequest"
                        };

                }
                else if (request.FileType == (int)ProductRefFileType.ProductRefPictures)
                {
                    //(fi.Extension == ".png" || extn == ".jpeg" || extn == ".jpg") && fileSizeInMB <= 5
                    if (fi.Extension != ".png" && fi.Extension != ".jpeg" && fi.Extension != ".jpg")
                        return new UploadCustomerProductResponse()
                        {
                            errors = new List<string>() { "Only image format accepted" },
                            statusCode = HttpStatusCode.BadRequest,
                            message = "BadRequest"
                        };

                    if (fileSizeInMB > (5 * 1024))
                        return new UploadCustomerProductResponse()
                        {
                            errors = new List<string>() { "File shouldn’t greater than 5 MB" },
                            statusCode = HttpStatusCode.BadRequest,
                            message = "BadRequest"
                        };
                }
                else
                {

                    return new UploadCustomerProductResponse()
                    {
                        errors = new List<string>() { "Invalid file type" },
                        statusCode = HttpStatusCode.BadRequest,
                        message = "BadRequest"
                    };
                }

                //get Cutomer Id 
                var customerId = _ApplicationContext.CustomerId;

                // Check  ProductReferance Exist for Customer 
                var customerProduct = await _productRepository.GetProductRefByCustomer(customerId, request.ProductReferance);

                if (customerProduct == null)
                {
                    return new UploadCustomerProductResponse
                    {
                        errors = new List<string>() { "no Product Found" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                }

                using var ms = new MemoryStream();
                request.File.CopyTo(ms);
                var fileBytes = ms.ToArray();
                ms.Dispose();
                // upload file to cloud
                var multipartContent = new MultipartFormDataContent();
                var byteContent = new ByteArrayContent(fileBytes);
                byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
                multipartContent.Add(byteContent, "files", Guid.NewGuid().ToString());

                using (var httpClient = new HttpClient())
                {
                    int containerId = (int)FileContainerList.DevContainer;
                    if (!Convert.ToBoolean(_configuration["IsDevelopment_Enviornment"]))
                    {
                        containerId = (int)FileContainerList.Products;
                    }
                    int entityId = _filterService.GetCompanyId();
                    var cloudFileUrl = _configuration["FileServer"] + "savefile/" + containerId.ToString() + "/" + entityId.ToString();

                    HttpResponseMessage dataResponse = httpClient.PostAsync(cloudFileUrl, multipartContent).Result;

                    if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string result = dataResponse.Content.ReadAsStringAsync().Result;

                        if (!string.IsNullOrEmpty(result))
                        {
                            var fileResultData = JsonConvert.DeserializeObject<FileUploadResponse>(result);

                            if (fileResultData != null && fileResultData.FileUploadDataList != null
                                && fileResultData.FileUploadDataList.FirstOrDefault() != null
                                && fileResultData.FileUploadDataList.FirstOrDefault().Result == FileUploadResponseResult.Sucess)
                            {
                                var fileResult = fileResultData.FileUploadDataList.FirstOrDefault();
                                var files = new List<ProductAttachment>()
                                {
                                    new ProductAttachment()
                                    {
                                         FileName = request.File.FileName?.Trim(),
                                        uniqueld = fileResult.FileName,
                                        FileUrl = fileResult.FileCloudUri,
                                        FileTypeId = request.FileType
                                    }
                                };
                                await AddFiles(files, customerProduct);
                                await _productRepository.Save();
                                response.message = "Success";
                                response.statusCode = HttpStatusCode.Created;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                return new UploadCustomerProductResponse()
                {
                    errors = new List<string>() { "internal Server Problem" },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
            return response;
        }

        public async Task<MSChartFileFormatResponse> GetMSChartFileFormatByCustomer(int customerId)
        {
            if (customerId <= 0)
                return new MSChartFileFormatResponse { Result = DataSourceResult.RequestNotCorrectFormat };

            var msChartFileFormatData = _productRepository.GetMSChartFileFormatByCustomer(customerId);

            var msChartFileFormatList = await msChartFileFormatData.Select(x => new MSChartFileFormat()
                {
                    Id = x.Id,
                    OcrCustomerName = x.OcrCustomerName,
                    OcrFileFormat = x.OcrFileFormat
                }).AsNoTracking().ToListAsync();

            if (msChartFileFormatList == null || !msChartFileFormatList.Any())
                return new MSChartFileFormatResponse { Result = DataSourceResult.CannotGetList };

            return new MSChartFileFormatResponse()
            {
                DataSourceList = msChartFileFormatList,
                Result = DataSourceResult.Success
            };
        }

        public async Task<dynamic> GetOcrTableData(OcrTableRequest request)
        {
            var requestUrl = _ocrSettings.BaseUrl + _ocrSettings.OCRTableRequestUrl;

            if (string.IsNullOrEmpty(requestUrl))
                return null;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(_ocrSettings.Key, _ocrSettings.Value);

            string jsonString = JsonConvert.SerializeObject(request, Formatting.Indented);
            var serializeobject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            var stringContent = new FormUrlEncodedContent(serializeobject);

            try
            {
                HttpResponseMessage response = httpClient.PostAsync(requestUrl, stringContent).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //get the response data
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(responseBody);
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

        #endregion
    }
}
 