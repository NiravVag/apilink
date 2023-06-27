using BI.Cache;
using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.EventBookingLog;
using DTO.FullBridge;
using DTO.ProductManagement;
using DTO.References;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BI
{
    public class ProductManagementManager : IProductManagementManager
    {
        #region Declaration 
        private IProductManagementRepository _productManagementRepository = null;
        private IInspectionBookingRepository _inspRepository = null;
        private ICacheManager _cache = null;
        private ILogger _logger = null;
        private readonly FBSettings _fbSettings = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IEventBookingLogManager _fbLog = null;
        private readonly IHelper _helper = null;
        private readonly ProductManagementMap ProductManagementMap = null;
        private readonly ITenantProvider _filterService = null;
        #endregion Declaration

        #region Constructor
        public ProductManagementManager(IProductManagementRepository productManagementRepository, ICacheManager cache, ILogger<ProductManagementManager> logger,
            IAPIUserContext applicationContext, IOptions<FBSettings> fbSettings, IEventBookingLogManager fbLog, IHelper helper, ITenantProvider filterService,
            IInspectionBookingRepository inspRepository)
        {
            _productManagementRepository = productManagementRepository;
            _cache = cache;
            _logger = logger;
            _fbSettings = fbSettings.Value;
            _fbLog = fbLog;
            _ApplicationContext = applicationContext;
            _helper = helper;
            ProductManagementMap = new ProductManagementMap();
            _filterService = filterService;
            _inspRepository = inspRepository;
        }
        #endregion Constructor

        #region Product Category

        public DataSourceResponse GetProductCategories()
        {
            // products
            var products = _productManagementRepository.GetProductCategories().ToArray();

            if (products == null || !products.Any())
                return null;

            var data = products.Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            });

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        public ProductCategoryResponse GetProductCategorySummary()
        {
            var response = new ProductCategoryResponse();
            try
            {
                response.ProductCategoryList = _productManagementRepository.GetProductCategories().Select(ProductManagementMap.GetProductCategory)
                    .OrderBy(x => x.Name).ToArray();
                
                if (response.ProductCategoryList == null)
                    return new ProductCategoryResponse { Result = ProductManagementResult.CannotGetProductManagementList };
                response.Result = ProductManagementResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product category summary");
            }
            return response;
        }

        public EditProductCategoryResponse GetProductCategoryById(int? id)
        {
            var response = new EditProductCategoryResponse();
            try
            {
                if (id != null)
                {
                    var list = _productManagementRepository.GetProductCategories();
                    response.ProductCategory = list.Where(x => x.Id == id)
                        .Select(ProductManagementMap.GetProductCategory).FirstOrDefault();
                    if (response.ProductCategory == null)
                        return new EditProductCategoryResponse() { Result = EditProductManagementResult.CanNotGetProductManagementList };
                }
                response.Result = EditProductManagementResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product category by id");
            }
            return response;
        }
        public async Task<SaveProductCategoryResponse> SaveProductCategory(ProductCategory request, string fbToken)
        {
            var response = new SaveProductCategoryResponse();

            try
            {
                request.Name = request.Name?.Trim();
                if (request.Id == 0)
                {
                    response = await AddProductCategory(request, fbToken);
                }
                else
                {
                    response = await EditProductCategory(request, fbToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save product.");
            }
            return response;
        }

        private async Task<SaveProductCategoryResponse> AddProductCategory(ProductCategory request, string fbToken)
        {
            try
            {
                RefProductCategory entity = ProductManagementMap.MapProductCategoryEntity(request, _filterService.GetCompanyId());

                if (entity == null)
                    return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.CannotMapRequestToEntites };

                var dupl = _productManagementRepository.GetProductCategories().Any(x => x.Name.Trim().ToLower() == request.Name.ToLower() && x.Id != request.Id);

                if (dupl)
                {
                    return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.DuplicateName };
                }
                // push to fb 
                if (!await SaveProductCategoryDataToFB(entity, fbToken))
                {
                    return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.CannotSaveInFB };
                }

                int id = await _productManagementRepository.SaveNewProductCategory(entity);

                if (id != 0)
                    return new SaveProductCategoryResponse() { ProductId = id, Result = SaveProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "add product category");
            }
            return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.CannotSaveProductManagement };
        }

        /// <summary>
        /// save product category to fb.
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveProductCategoryDataToFB(RefProductCategory productCategory, string fbToken)
        {
            try
            {
                if (productCategory.FbProductCategoryId == null)
                {
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.ProductCategoryUrl;

                    var objProductCategory = new FBProductCategory() { title = productCategory.Name, status = "active" };

                    _logger.LogInformation("API-FB : FB Product category data save request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objProductCategory));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProductCategory)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objProductCategory, fbBase, fbToken);

                    _logger.LogInformation("API-FB :  FB Product category data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        productCategory.FbProductCategoryId = Convert.ToInt32(fbRecordId);
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
        /// save product sub category to fb
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveProductSubCategoryDataToFB(RefProductCategorySub productSubCategory, string fbToken, int? fbProductCategoryId)
        {
            try
            {
                if (productSubCategory?.FbProductSubCategoryId == null && fbProductCategoryId != null)
                {
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.ProductCategoryUrl;

                    var objProductCategory = new FBProductSubCategory() { title = productSubCategory?.Name, status = "active", parent = fbProductCategoryId.GetValueOrDefault() };

                    _logger.LogInformation("API-FB : FB Product sub category data save request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objProductCategory));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProductCategory)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objProductCategory, fbBase, fbToken);

                    _logger.LogInformation("API-FB :  FB Product sub category data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        if (productSubCategory != null)
                            productSubCategory.FbProductSubCategoryId = Convert.ToInt32(fbRecordId);
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
        /// update product category to fb 
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> UpdateProductSubCategoryDataToFB(RefProductCategorySub productSubCategory, string fbToken)
        {
            try
            {
                if (productSubCategory.FbProductSubCategoryId != null && productSubCategory?.ProductCategory?.FbProductCategoryId != null)
                {
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.ProductCategoryUpdateUrl + "/" + productSubCategory.FbProductSubCategoryId;

                    var objProductCategory = new FBProductSubCategory() { title = productSubCategory.Name, status = "active", parent = productSubCategory.ProductCategory.FbProductCategoryId.GetValueOrDefault() };

                    _logger.LogInformation("API-FB : FB Product sub category data update request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objProductCategory));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProductCategory)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.JSONPut, fbRequest, objProductCategory, fbBase, fbToken);

                    _logger.LogInformation("API-FB :  FB Product sub category data update request end ");
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
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// save product sub category2 to fb
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> SaveProductSubCategory2DataToFB(RefProductCategorySub2 productSubCategory2, string fbToken, int? fbProductSubCategoryId)
        {
            try
            {
                if (productSubCategory2?.FbProductSubCategory2Id == null && fbProductSubCategoryId != null)
                {
                    string fbRecordId = string.Empty;
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.ProductCategoryUrl;

                    var objProductCategory = new FBProductSubCategory() { title = productSubCategory2?.Name, status = "active", parent = fbProductSubCategoryId.GetValueOrDefault() };

                    _logger.LogInformation("API-FB : FB Product sub category2 data save request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objProductCategory));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProductCategory)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, fbRequest, objProductCategory, fbBase, fbToken);

                    _logger.LogInformation("API-FB :  FB Product sub category2 data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                    {
                        fbRecordId = getFbRecordIdFromResult(httpResponse);
                        if (productSubCategory2 != null)
                            productSubCategory2.FbProductSubCategory2Id = Convert.ToInt32(fbRecordId);
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
        /// update product category2 to fb 
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> UpdateProductSubCategory2DataToFB(RefProductCategorySub2 productSubCategory2, string fbToken)
        {
            try
            {
                if (productSubCategory2.FbProductSubCategory2Id != null && productSubCategory2?.ProductSubCategory?.FbProductSubCategoryId != null)
                {
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.ProductCategoryUpdateUrl + "/" + productSubCategory2.FbProductSubCategory2Id;

                    var objProductCategory = new FBProductSubCategory() { title = productSubCategory2.Name, status = "active", parent = productSubCategory2.ProductSubCategory.FbProductSubCategoryId.GetValueOrDefault() };

                    _logger.LogInformation("API-FB : FB Product sub category2 data update request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objProductCategory));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProductCategory)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.JSONPut, fbRequest, objProductCategory, fbBase, fbToken);

                    _logger.LogInformation("API-FB :  FB Product sub category2 data update request end ");
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
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// update product category to fb 
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task<Boolean> UpdateProductCategoryDataToFB(RefProductCategory productCategory, string fbToken)
        {
            try
            {
                if (productCategory.FbProductCategoryId != null)
                {
                    var fbBase = _fbSettings.BaseUrl;
                    var fbRequest = _fbSettings.ProductCategoryUpdateUrl + "/" + productCategory.FbProductCategoryId;

                    var objProductCategory = new FBProductCategory() { title = productCategory.Name, status = "active" };

                    _logger.LogInformation("API-FB : FB Product category data update request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(objProductCategory));

                    await _fbLog.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = fbRequest,
                        LogInformation = JsonConvert.SerializeObject(objProductCategory)
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.JSONPut, fbRequest, objProductCategory, fbBase, fbToken);

                    _logger.LogInformation("API-FB :  FB Product category data update request end ");
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
                return true;
            }
            catch (Exception ex)
            {       
                throw ex;
            }
        }


        private async Task<SaveProductCategoryResponse> EditProductCategory(ProductCategory request, string fbToken)
        {
            var entity = _productManagementRepository.GetProductCategorybyId(request.Id);
            try
            {
                if (entity == null)
                    return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.CurrentProductManagementNotFound };

                var isDuplicate = _productManagementRepository.GetProductCategories().Any(x => x.Name.Trim().ToLower() == request.Name.ToLower() && x.Id != request.Id);

                if (isDuplicate)
                {
                    return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.DuplicateName };
                }

                #region mapping
                entity.Name = request.Name;
                entity.Active = true;
                entity.BusinessLineId = request.BusinessLineId;
                entity.EntityId = _filterService.GetCompanyId();
                #endregion

                // update to fb 
                if (entity.FbProductCategoryId != null && !await UpdateProductCategoryDataToFB(entity, fbToken))
                {
                    return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.CannotUpdateInFB };
                }

                else if (entity.FbProductCategoryId == null && !await SaveProductCategoryDataToFB(entity, fbToken))
                {
                    return new SaveProductCategoryResponse() { Result = SaveProductManagementResult.CannotSaveInFB };
                }

                int re = await _productManagementRepository.SaveEditProductCategory(entity);
                if (re > 0)
                    return new SaveProductCategoryResponse { ProductId = entity.Id, Result = SaveProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "edit product category");
            }
            return new SaveProductCategoryResponse { ProductId = entity.Id, Result = SaveProductManagementResult.CannotSaveProductManagement };
        }

        public async Task<DeleteProductCategoryResponse> RemoveProductCategory(int id)
        {
            var item = _productManagementRepository.GetProductCategorybyId(id);
            try
            {
                if (item == null)
                    return new DeleteProductCategoryResponse { Id = id, Result = DeleteProductManagementResult.NotFound };
                if ((item.CuProducts.Where(x => x.Active).Count() == 0 || item.CuProducts == null) && (item.CuServiceTypes.Where(x => x.Active).Count() == 0 || item.CuServiceTypes == null)
                    && (item.HrStaffProductCategories.Count() == 0 || item.HrStaffProductCategories == null) && (item.RefProductCategorySubs.Where(x => x.Active).Count() == 0 || item.RefProductCategorySubs == null))
                {
                    await _productManagementRepository.RemoveProductCategory(id);
                }
                else
                {
                    return new DeleteProductCategoryResponse() { Id = id, Result = DeleteProductManagementResult.CannotDelete };
                }
                return new DeleteProductCategoryResponse() { Id = id, Result = DeleteProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "remove product category");
            }
            return new DeleteProductCategoryResponse() { Id = id, Result = DeleteProductManagementResult.CannotDelete };
        }

        /// <summary>
        /// get the Result id from full bridge response
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private string getFbRecordIdFromResult(HttpResponseMessage httpResponse)
        {
            string fbRecordId = null;
            var data = httpResponse.Content.ReadAsStringAsync();
            JObject userDataJson = JObject.Parse(data.Result);
            if (userDataJson != null && userDataJson.GetValue("data") != null)
            {
                fbRecordId = userDataJson.GetValue("data")["id"].ToString();
            }
            return fbRecordId;
        }
        #endregion

        #region Product Sub Category
        public DataSourceResponse GetProductSubCategories()
        {
            // products
            var products = _productManagementRepository.GetProductSubCategories().ToArray();

            if (products == null || !products.Any())
                return null;

            var data = products.Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name);

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        public ProductSubCategoryResponse GetProductSubCategorySummary()
        {
            var response = new ProductSubCategoryResponse();
            try
            {
                response.ProductSubCategoryList = _productManagementRepository.GetProductSubCategories().Select(ProductManagementMap.GetProductSubCategory).OrderBy(x => x.ProductCategory.Name).ThenBy(x => x.Name).ToArray();

                response.ProductCategoryList = _productManagementRepository.GetProductCategories().Select(ProductManagementMap.GetProductCategory).OrderBy(x => x.Name).ToArray();

                if (response.ProductSubCategoryList == null || response.ProductCategoryList == null)
                    return new ProductSubCategoryResponse { Result = ProductManagementResult.CannotGetProductManagementList };
                response.Result = ProductManagementResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product sub category summary");
            }
            return response;
        }

        public async Task<SaveProductSubCategoryResponse> SaveProductSubCategory(ProductSubCategory request, string fbToken)
        {
            var response = new SaveProductSubCategoryResponse();

            try
            {
                request.Name = request.Name?.Trim();
                if (request.Id == 0)
                {
                    response = await AddProductSubCategory(request, fbToken);
                }
                else
                {
                    response = await EditProductSubCategory(request, fbToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save product sub category.");
            }
            return response;
        }

        private async Task<SaveProductSubCategoryResponse> AddProductSubCategory(ProductSubCategory request, string fbToken)
        {
            try
            {
                RefProductCategorySub entity = ProductManagementMap.MapProductSubCategoryEntity(request, _filterService.GetCompanyId());

                if (entity == null)
                    return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.CannotMapRequestToEntites };

                int dupl = _productManagementRepository.GetProductSubCategories().Where(x => x.Name.Trim().ToLower() == request.Name.ToLower() && x.Id != request.Id && x.ProductCategoryId == request.ProductCategoryId).Count();

                if (dupl > 0)
                {
                    return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.DuplicateName };
                }


                int? fbProductCategoryId = await _productManagementRepository.GetFBProductCategorybyId(entity.ProductCategoryId);

                // push to fb 
                if (!await SaveProductSubCategoryDataToFB(entity, fbToken, fbProductCategoryId))
                {
                    return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.CannotSaveInFB };
                }

                int id = await _productManagementRepository.SaveNewProductSubCategory(entity);

                if (id != 0)
                    return new SaveProductSubCategoryResponse() { ProductSubCategoryId = id, Result = SaveProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "add product sub category");
            }
            return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.CannotSaveProductManagement };
        }

        private async Task<SaveProductSubCategoryResponse> EditProductSubCategory(ProductSubCategory request, string fbToken)
        {
            var entity = _productManagementRepository.GetProductSubCategorybyId(request.Id);
            try
            {
                if (entity == null)
                    return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.CurrentProductManagementNotFound };

                int dupl = _productManagementRepository.GetProductSubCategories().Where(x => x.Name.Trim().ToLower() == request.Name.ToLower() && x.Id != request.Id && x.ProductCategoryId == request.ProductCategoryId).Count();

                if (dupl > 0)
                {
                    return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.DuplicateName };
                }
                #region mapping
                entity.Name = request.Name;
                entity.Active = true;
                entity.ProductCategoryId = request.ProductCategoryId;
                entity.EntityId = _filterService.GetCompanyId();
                #endregion

                // update to fb 
                if (entity.FbProductSubCategoryId != null && !await UpdateProductSubCategoryDataToFB(entity, fbToken))
                {
                    return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.CannotUpdateInFB };
                }

                else if (entity.FbProductSubCategoryId == null && !await SaveProductSubCategoryDataToFB(entity, fbToken, entity.ProductCategory?.FbProductCategoryId))
                {
                    return new SaveProductSubCategoryResponse() { Result = SaveProductManagementResult.CannotSaveInFB };
                }

                int re = await _productManagementRepository.SaveEditProductSubCategory(entity);
                if (re > 0)
                    return new SaveProductSubCategoryResponse { ProductSubCategoryId = entity.Id, Result = SaveProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "edit product sub category");
            }
            return new SaveProductSubCategoryResponse { ProductSubCategoryId = entity.Id, Result = SaveProductManagementResult.CannotSaveProductManagement };
        }

        public EditProductSubCategoryResponse GetProductSubCategoryById(int? id)
        {
            var response = new EditProductSubCategoryResponse();
            try
            {
                if (id != null)
                {
                    var list = _productManagementRepository.GetProductSubCategories();
                    response.ProductSubCategory = list.Where(x => x.Id == id)
                        .Select(ProductManagementMap.GetProductSubCategory).FirstOrDefault();
                    if (response.ProductSubCategory == null)
                        return new EditProductSubCategoryResponse() { Result = EditProductManagementResult.CanNotGetProductManagementList };
                }
                response.Result = EditProductManagementResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product sub category by id");
            }
            return response;
        }

        public async Task<DeleteProductSubCategoryResponse> RemoveProductSubCategory(int id)
        {
            var item = _productManagementRepository.GetProductSubCategorybyId(id);
            try
            {
                if (item == null)
                    return new DeleteProductSubCategoryResponse { Id = id, Result = DeleteProductManagementResult.NotFound };
                if ((item.CuProducts.Where(x => x.Active).Count() == 0 || item.CuProducts == null)
                    && (item.RefProductCategorySub2S.Where(x => x.Active).Count() == 0 || item.RefProductCategorySub2S == null) && (item.ProductCategory.Active))
                {
                    await _productManagementRepository.RemoveProductSubCategory(id);
                }
                else
                {
                    return new DeleteProductSubCategoryResponse() { Id = id, Result = DeleteProductManagementResult.CannotDelete };
                }
                return new DeleteProductSubCategoryResponse() { Id = id, Result = DeleteProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "remove product sub category");
            }
            return new DeleteProductSubCategoryResponse() { Id = id, Result = DeleteProductManagementResult.CannotDelete };
        }

        #endregion

        #region Product Category Sub2
        public IEnumerable<ProductCategorySub2> GetProductCategorySub2s()
        {
            // products
            var products = _cache.CacheTryGetValueSet(CacheKeys.AllProductCategorySub2s,
                        () => _productManagementRepository.GetProductCategorySub2s().ToArray());

            if (products == null || !products.Any())
                return null;

            return products.Select(ProductManagementMap.GetProductCategorySub2).OrderBy(x => x.Name);
        }

        public ProductCategorySub2Response GetProductCategorySub2Summary()
        {
            var response = new ProductCategorySub2Response();
            try
            {
                response.ProductCategoryList = _productManagementRepository.GetProductCategories().Select(ProductManagementMap.GetProductCategory).OrderBy(x => x.Name).ToArray();

                if (response.ProductCategoryList == null)
                    return new ProductCategorySub2Response { Result = ProductCategorySub2Result.CannotGetProductCategorySub2 };
                response.Result = ProductCategorySub2Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product category sub2 summary");
            }
            return response;
        }

        public async Task<SaveProductCategorySub2Response> SaveProductCategorySub2(ProductCategorySub2 request, string strFbToken)
        {
            var response = new SaveProductCategorySub2Response();

            try
            {
                request.Name = request.Name?.Trim();
                if (request.Id == 0)
                {
                    response = await AddProductCategorySub2(request, strFbToken);
                }
                else
                {
                    response = await EditProductCategorySub2(request, strFbToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save product sub2 category.");
            }
            return response;
        }

        private async Task<SaveProductCategorySub2Response> EditProductCategorySub2(ProductCategorySub2 request, string fbToken)
        {
            var entity = _productManagementRepository.GetProductCategorySub2ById(request.Id);
            try
            {
                if (entity == null)
                    return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.CurrentProductManagementNotFound };

                int dupl = _productManagementRepository.GetProductCategorySub2s().Where(x => x.Name.Trim().ToLower() == request.Name.ToLower() && x.Id != request.Id && x.ProductSubCategoryId == request.ProductSubCategoryId).Count();

                if (dupl > 0)
                {
                    return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.DuplicateName };
                }
                #region mapping
                entity.Name = request.Name;
                entity.Active = true;
                entity.ProductSubCategoryId = request.ProductSubCategoryId;
                entity.EntityId = _filterService.GetCompanyId();
                #endregion

                // update to fb 
                if (entity.FbProductSubCategory2Id != null && !await UpdateProductSubCategory2DataToFB(entity, fbToken))
                {
                    return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.CannotUpdateInFB };
                }

                else if (entity.FbProductSubCategory2Id == null && !await SaveProductSubCategory2DataToFB(entity, fbToken, entity?.ProductSubCategory?.FbProductSubCategoryId))
                {
                    return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.CannotSaveInFB };
                }
                int re = await _productManagementRepository.SaveEditProductCategorySub2(entity);
                if (re > 0)
                    return new SaveProductCategorySub2Response { ProductId = entity.Id, Result = SaveProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "edit product sub2 category");
            }
            return new SaveProductCategorySub2Response { ProductId = entity.Id, Result = SaveProductManagementResult.CannotSaveProductManagement };
        }

        private async Task<SaveProductCategorySub2Response> AddProductCategorySub2(ProductCategorySub2 request, string fbToken)
        {
            try
            {
                RefProductCategorySub2 entity = ProductManagementMap.MapProductCategorySub2Entity(request, _filterService.GetCompanyId());

                if (entity == null)
                    return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.CannotMapRequestToEntites };

                int dupl = _productManagementRepository.GetProductCategorySub2s().Where(x => x.Name.Trim().ToLower() == request.Name.ToLower() && x.Id != request.Id && x.ProductSubCategoryId == request.ProductSubCategoryId).Count();

                if (dupl > 0)
                {
                    return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.DuplicateName };
                }

                int? fbProductSubCategoryId = await _productManagementRepository.GetFBProductSubCategorybyId(entity.ProductSubCategoryId);

                // push to fb 
                if (!await SaveProductSubCategory2DataToFB(entity, fbToken, fbProductSubCategoryId))
                {
                    return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.CannotSaveInFB };
                }

                int id = await _productManagementRepository.SaveNewProductCategorySub2(entity);

                if (id != 0)
                    return new SaveProductCategorySub2Response() { ProductId = id, Result = SaveProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "add product sub2 category");
            }
            return new SaveProductCategorySub2Response() { Result = SaveProductManagementResult.CannotSaveProductManagement };
        }

        public EditProductCategorySub2Response GetProductCategorySub2ById(int? id)
        {
            var response = new EditProductCategorySub2Response();
            try
            {
                if (id != null)
                {
                    var list = _productManagementRepository.GetProductCategorySub2s();
                    response.ProductCategorySub2 = list.Where(x => x.Id == id)
                        .Select(ProductManagementMap.GetProductCategorySub2).FirstOrDefault();
                    if (response.ProductCategorySub2 == null)
                        return new EditProductCategorySub2Response() { Result = EditProductManagementResult.CanNotGetProductManagementList };
                }
                response.ProductCategoryList = _productManagementRepository.GetProductCategories().
                    Select(ProductManagementMap.GetProductCategory).OrderBy(x => x.Name).ToArray();
                response.Result = EditProductManagementResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product sub2 category by id");
            }
            return response;
        }

        public async Task<DeleteProductCategorySub2Response> RemoveProductCategorySub2(int id)
        {
            var item = _productManagementRepository.GetProductCategorySub2ById(id);
            try
            {
                if (item == null)
                    return new DeleteProductCategorySub2Response { Id = id, Result = DeleteProductManagementResult.NotFound };
                if ((item.CuProducts.Where(x => x.Active).Count() == 0 || item.CuProducts == null) && (item.ProductSubCategory.Active))
                {
                    await _productManagementRepository.RemoveProductCategorySub2(id);
                }
                else
                {
                    return new DeleteProductCategorySub2Response() { Id = id, Result = DeleteProductManagementResult.CannotDelete };
                }
                return new DeleteProductCategorySub2Response() { Id = id, Result = DeleteProductManagementResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "remove product sub2 category");
            }
            return new DeleteProductCategorySub2Response() { Id = id, Result = DeleteProductManagementResult.CannotDelete };
        }

        public ProductSubCategoryResponse GetProductSubCategoryByCategory(int? id)
        {
            var response = new ProductSubCategoryResponse();
            try
            {
                response.ProductSubCategoryList = _productManagementRepository.GetProductSubCategories().Where(x => x.ProductCategoryId == id && x.Active).Select(ProductManagementMap.GetProductSubCategory)
                    .OrderBy(x => x.Name).ToArray();
                response.ProductCategoryList = _productManagementRepository.GetProductCategories().Select(ProductManagementMap.GetProductCategory).OrderBy(x => x.Name).ToArray();
                if (response.ProductSubCategoryList == null)
                    return new ProductSubCategoryResponse { Result = ProductManagementResult.CannotGetProductManagementList };
                response.Result = ProductManagementResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product sub category by category");
            }
            return response;
        }

        public ProductCategorySub2Response GetProductCategorySub2BySubCategory(int? id)
        {
            var response = new ProductCategorySub2Response();
            try
            {
                response.ProductCategorySub2List = _productManagementRepository.GetProductCategorySub2s().Where(x => x.ProductSubCategoryId == id && x.Active)
                    .Select(ProductManagementMap.GetProductCategorySub2).OrderBy(x => x.Name).ToArray();

                if (response.ProductCategorySub2List == null)
                    return new ProductCategorySub2Response { Result = ProductCategorySub2Result.CannotGetProductCategorySub2 };
                response.Result = ProductCategorySub2Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get product sub2 category by sub category");
            }
            return response;
        }

        public ProductCategorySub2SearchResponse GetProductCategorySub2SearchSummary(ProductCategorySub2SearchRequest request)
        {
            var response = new ProductCategorySub2SearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };
            try
            {
                var data = _productManagementRepository.GetProductCategorySub2s();
                if (request.ProductCategorySub2Name != null && request.ProductCategorySub2Name?.Trim() != "")
                {
                    data = data.Where(x => x.Name.ToLower().Contains(request.ProductCategorySub2Name.ToLower()));
                }
                else
                {
                    if (request.ProductCategorySub2Values != null && request.ProductCategorySub2Values.Any())
                        data = data.Where(x => request.ProductCategorySub2Values.Any(y => x.Id == y.Id));
                    else if (request.ProductSubCategoryId != null)
                        data = data.Where(x => x.ProductSubCategoryId == request.ProductSubCategoryId);
                    else if (request.ProductCategoryId != null)
                        data = data.Where(x => x.ProductSubCategory.ProductCategoryId == request.ProductCategoryId);
                }

                response.TotalCount = data.Count();
                if (response.TotalCount == 0)
                {
                    response.Result = ProductManagementResult.NotFound;
                    return response;
                }
                int skip = (request.Index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.Data = data.Skip(skip).Take(request.pageSize.Value).Select(ProductManagementMap.GetProductCategorySub2).
                    OrderBy(x => x.ProductCategory.Name).ThenBy(x => x.ProductSubCategory.Name).ThenBy(x => x.Name).ToArray();
                response.Result = ProductManagementResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get  product sub2 category search summary");
            }

            return response;
        }
        #endregion

        #region Product Category Sub3

        public async Task<SaveProductCategorySub3Response> SaveProdSubCategory3(SaveProductCategorySub3 request)
        {
            try
            {
                if (request == null)
                {
                    return new SaveProductCategorySub3Response { Result = SaveProductManagementResult.CannotMapRequestToEntites };
                }

                if (await _productManagementRepository.CheckIfSubCat3AlreadyExists(request.ProductSubCategory2Id, request.Name))
                {
                    return new SaveProductCategorySub3Response { Result = SaveProductManagementResult.DuplicateName };
                }

                var entityId = _filterService.GetCompanyId();

                RefProductCategorySub3 entity = new RefProductCategorySub3()
                {
                    Name = request.Name,
                    Active = true,
                    ProductSubCategory2Id = request.ProductSubCategory2Id,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    EntityId = entityId
                };

                _productManagementRepository.Save(entity, false);

                if (request.WorkLoadMatrixChecked)
                {
                    AddWorkLoadMatrixEntity(request, entityId, entity.Id);
                }

                return new SaveProductCategorySub3Response
                {
                    Result = SaveProductManagementResult.Success
                };
            }
            catch (Exception ex)
            {
                return new SaveProductCategorySub3Response { Result = SaveProductManagementResult.CannotSaveProductManagement };
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveProductCategorySub3Response> UpdateProdSubCategory3(SaveProductCategorySub3 request)
        {
            try
            {
                if (request == null)
                {
                    return new SaveProductCategorySub3Response { Result = SaveProductManagementResult.CannotMapRequestToEntites };
                }

                if (await _productManagementRepository.CheckIfSubCat3AlreadyExists(request.ProductSubCategory2Id, request.Name))
                {
                    return new SaveProductCategorySub3Response { Result = SaveProductManagementResult.DuplicateName };
                }

                var entity = await _productManagementRepository.GetProdSubCat3(request.Id);

                if (entity != null)
                {
                    entity.Name = request.Name;
                    entity.ProductSubCategory2Id = request.ProductSubCategory2Id;
                    entity.UpdatedBy = _ApplicationContext.UserId;
                    entity.UpdatedOn = DateTime.Now;
                };

                _productManagementRepository.Save(entity, true);

                return new SaveProductCategorySub3Response
                {
                    Result = SaveProductManagementResult.Success
                };
            }
            catch (Exception ex)
            {
                return new SaveProductCategorySub3Response { Result = SaveProductManagementResult.CannotSaveProductManagement };
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DeleteProdCategorySub3Response> DeleteProdSubCategory3(int id)
        {
            try
            {
                if (!(id > 0))
                {
                    return new DeleteProdCategorySub3Response { Result = DeleteProductManagementResult.CannotDelete };
                }

                if (await _productManagementRepository.CheckIdProdCatSub3MappedToCustomerProduct(id))
                {
                    return new DeleteProdCategorySub3Response { Result = DeleteProductManagementResult.ProductMapped };
                }

                var entity = await _productManagementRepository.GetProdSubCat3(id);

                if (entity == null)
                {
                    return new DeleteProdCategorySub3Response { Result = DeleteProductManagementResult.NotFound };
                };

                entity.Active = false;
                entity.DeletedBy = _ApplicationContext.UserId;
                entity.DeletedOn = DateTime.Now;

                _productManagementRepository.Save(entity, true);

                return new DeleteProdCategorySub3Response
                {
                    Result = DeleteProductManagementResult.Success
                };
            }
            catch (Exception ex)
            {
                return new DeleteProdCategorySub3Response { Result = DeleteProductManagementResult.CannotDelete };
            }
        }
        /// <summary>
        /// Get ProdSubCategory3 by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditProdCategorySub3Response> GetProdSubCategory3ById(int id)
        {
            if (!(id > 0))
            {
                return new EditProdCategorySub3Response { Result = EditProductManagementResult.CanNotGetProductManagement };
            }

            var entity = await _productManagementRepository.GetProdSubCat3ById(id);

            if (entity == null)
            {
                return new EditProdCategorySub3Response { Result = EditProductManagementResult.CanNotGetProductManagement };
            };

            return new EditProdCategorySub3Response
            {
                Data = entity,
                Result = EditProductManagementResult.Success
            };
        }
        public async Task<ProdCatSub3SummaryResponse> GetProdSubCategory3Summary(ProdCatSub3SummaryRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            if (request == null)
            {
                return new ProdCatSub3SummaryResponse { Result = ProductManagementResult.CannotGetProductManagementList };
            }

            var data = _productManagementRepository.GetProdSubCat3ByEfCore();

            if (data == null)
            {
                return new ProdCatSub3SummaryResponse { Result = ProductManagementResult.NotFound };
            }

            if (request.ProductCategoryId > 0)
            {
                data = data.Where(x => x.ProdCategoryId == request.ProductCategoryId);
            }
            if (request.ProductSubCategoryId > 0)
            {
                data = data.Where(x => x.ProdSubCategoryId == request.ProductSubCategoryId);
            }
            if (request.ProductCategorySub2Values != null && request.ProductCategorySub2Values.Any())
            {
                data = data.Where(x => request.ProductCategorySub2Values.Contains(x.ProdCategorySub2Id));
            }
            if (!string.IsNullOrEmpty(request.ProductCategorySub3Name))
            {
                data = data.Where(x => x.ProdCategorySub3 == request.ProductCategorySub3Name);
            }

            var totalCount = await data.CountAsync();

            var result = await data.Skip(skip).Take(take).OrderBy(x => x.ProdCategoryId).ToListAsync();

            return new ProdCatSub3SummaryResponse
            {
                Data = result,
                HasITRole = _ApplicationContext.RoleList.ToList().Contains((int)RoleEnum.IT_Team),
                Result = ProductManagementResult.Success,
                PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0),
                PageSize = request.PageSize.GetValueOrDefault(),
                TotalCount = totalCount,
                Index = request.Index.GetValueOrDefault()
            };
        }

        private void AddWorkLoadMatrixEntity(SaveProductCategorySub3 request, int entityId, int prodCatSub3Id)
        {
            QuWorkLoadMatrix workLoadEntity = new QuWorkLoadMatrix()
            {
                ProductSubCategory3Id = prodCatSub3Id,
                PreparationTime = request.PreparationTime,
                SampleSize8h = request.EightHourSampleSize,
                Active = true,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                EntityId = entityId
            };
            _productManagementRepository.Save(workLoadEntity, false);
        }

        public async Task<List<ExportProdCatSub3Data>> ExportProdSubCategory3Summary(ProdCatSub3SummaryRequest request)
        {
            var data = _productManagementRepository.GetProdSubCat3ByEfCore();

            if (data == null)
            {
                return null;
            }

            if (request.ProductCategoryId > 0)
            {
                data = data.Where(x => x.ProdCategoryId == request.ProductCategoryId);
            }
            if (request.ProductSubCategoryId > 0)
            {
                data = data.Where(x => x.ProdSubCategoryId == request.ProductSubCategoryId);
            }
            if (request.ProductCategorySub2Values != null && request.ProductCategorySub2Values.Any())
            {
                data = data.Where(x => request.ProductCategorySub2Values.Contains(x.ProdCategorySub2Id));
            }
            if (!string.IsNullOrEmpty(request.ProductCategorySub3Name))
            {
                data = data.Where(x => x.ProdCategorySub3 == request.ProductCategorySub3Name);
            }

            var totalCount = await data.CountAsync();

            var result = await data.OrderBy(x => x.ProdCategoryId).AsNoTracking().ToListAsync();

            return result.ConvertAll(x => new ExportProdCatSub3Data
            {
                ProdCategoryName = x.ProdCategoryName,
                ProdSubCategoryName = x.ProdSubCategoryName,
                ProdCategorySub2Name = x.ProdCategorySub2Name,
                ProdCategorySub3 = x.ProdCategorySub3,
                PreparationTime = x.PreparationTime,
                EightHourSampleSize = x.EightHourSampleSize
            }).ToList();
        }

        /// <summary>
        /// Get the product sub category3 list
        /// </summary>
        /// <param name="productSubCategory2Id"></param>
        /// <returns></returns>
        public async Task<ProductSubCategory3Response> GetProductCategorySub3List(int productSubCategory2Id)
        {
            var productSubCategory2Ids = new[] { productSubCategory2Id }.ToList();

            var productSubCategory3List = await _inspRepository.GetProductSubCategory3List(productSubCategory2Ids);

            if (productSubCategory3List != null && productSubCategory3List.Any())
            {
                return new ProductSubCategory3Response()
                {
                    ProductSubCategory3List = productSubCategory3List.Select(x => new CommonDataSource()
                    {
                        Id = x.ProductSubCategory3Id.GetValueOrDefault(),
                        Name = x.ProductSubCategory3Name
                    }).ToList(),
                    Result = ProductSubCategory3Result.Success
                };
            }

            return new ProductSubCategory3Response() { Result = ProductSubCategory3Result.CannotGetList };
        }

        #endregion


    }
}
