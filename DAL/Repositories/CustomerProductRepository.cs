using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.FullBridge;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerProductRepository : Repository, ICustomerProductRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public CustomerProductRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        //public IEnumerable<CuProduct> GetCustomerProducts(int? customerID,int? productCategoryID,int? productSubCategoryID,int? productTypeID,string productID)
        //{
        //    return _context.CuProducts.Where(x => x.Active && x.CustomerId==customerID && x.ProductCategory==productCategoryID
        //                                     && x.ProductSubCategory==productSubCategoryID && x.InternalProductType==productTypeID
        //                                     && x.ProductId==productID);
        //}

        public IQueryable<CuProduct> GetCustomerProducts()
        {
            return _context.CuProducts.Where(x => x.Active)
                .Include(x => x.Customer)
                .Include(x => x.ProductCategoryNavigation)
                .Include(x => x.ProductSubCategoryNavigation)
                .Include(x => x.ProductCategorySub2Navigation)
                .Include(x => x.ProductCategorySub3Navigation);
        }
        public IQueryable<CustomerProductRepoExportData> GetCustomerProductsExportData()
        {
            return _context.CuProducts.Where(x => x.Active)
                            .Select(x => new CustomerProductRepoExportData()
                            {
                                CustomerId = x.CustomerId,
                                ProductCategoryId = x.ProductCategory,
                                ProductSubCategoryId = x.ProductSubCategory,
                                ProductCategorySub2Id = x.ProductCategorySub2,
                                ProductId = x.ProductId,
                                CustomerName = x.Customer.CustomerName,
                                ProductDescription = x.ProductDescription,
                                Barcode = x.Barcode,
                                FactoryReference = x.FactoryReference,
                                Remarks = x.Remarks,
                                ProductCategory = x.ProductCategoryNavigation.Name,
                                ProductSubCategory = x.ProductSubCategoryNavigation.Name,
                                ProductCategorySub2 = x.ProductCategorySub2Navigation.Name,
                                IsNewProduct = x.IsNewProduct,
                                IsStyle = x.IsStyle,
                                ProductCategorySub3Id = x.ProductCategorySub3,
                                ProductCategorySub3 = x.ProductCategorySub3Navigation.Name,
                                SampleSize8h = x.SampleSize8h,
                                TimePreparation = x.TimePreparation
                            });
        }

        public IEnumerable<CuProduct> GetCustomerProductsByCustomers(int customerId)
        {
            return _context.CuProducts.Where(x => x.CustomerId == customerId && x.Active);

        }
        public IQueryable<CuProduct> GetCustomerProductDataSource()
        {
            return _context.CuProducts.Where(x => x.Active).OrderBy(x => x.ProductId);
        }

        public async Task<bool> CheckProductInspExists(int productid)
        {
            return await _context.CuProducts
                .Include(x => x.InspProductTransactionProducts)
                .Where(x => x.Id == productid && x.InspProductTransactionProducts.Any(y => y.Active.HasValue && y.Active.Value && y.Inspection.StatusId != (int)BookingStatus.Cancel)).AnyAsync();
        }

        public CuProduct GetCustomerProductByID(int id)
        {
            return _context.CuProducts.
                Where(x => x.Id == id && x.Active).
                Include(x => x.CuProductFileAttachments)
                .ThenInclude(x => x.CuProductMscharts)
                .Include(x => x.CuPurchaseOrderDetails)
                .ThenInclude(x => x.Po)
                .ThenInclude(x => x.InspPurchaseOrderTransactions)
                .ThenInclude(x => x.Inspection)
                .Include(x => x.CuPurchaseOrderDetails)
                .ThenInclude(x => x.Po)
                .ThenInclude(x => x.InspPurchaseOrderTransactions)
                .ThenInclude(x => x.ProductRef)
                .Include(x => x.CuProductApiServices)
                .FirstOrDefault();
        }

        public CuProduct GetCustomerProductByEntityAndProductId(int id, int? entityId)
        {
            return _context.CuProducts.IgnoreQueryFilters().
                 Where(x => x.Id == id && x.EntityId == entityId && x.Active)
                .FirstOrDefault();
        }

        public CuProduct GetProductsByCustomerAndProducts(int customerId, string productId)
        {
            return _context.CuProducts.
                Where(x => x.ProductId == productId && x.CustomerId == customerId && x.Active).
                Include(x => x.CuProductFileAttachments).
                SingleOrDefault();
        }

        public async Task<SuSupplierCustomer> GetSupplierId(string SupplierCode)
        {
            return await _context.SuSupplierCustomers.Where(x => x.Code == SupplierCode).Select(x => x).FirstOrDefaultAsync();
        }



        public async Task<bool> CheckProductIsExistForThisCustomer(int customerId, string productId, int id)
        {
            if (id > 0)
            {
                return await _context.CuProducts.
                    AnyAsync(x => x.ProductId == productId && x.CustomerId == customerId && x.Active && x.Id != id);
            }
            else
            {
                return await _context.CuProducts.
                    AnyAsync(x => x.ProductId == productId && x.CustomerId == customerId && x.Active);
            }
        }

        /// <summary>
        /// Get product id by productname and customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<int> GetProductIdByNameAndCustomer(int customerId, string productId)
        {
            return await _context.CuProducts.
                Where(x => x.ProductId == productId && x.CustomerId == customerId && x.Active)
                .Select(x=>x.Id).FirstOrDefaultAsync();
        }

        public CuProduct GetCustomerProductByProductID(string productID)
        {
            return _context.CuProducts.
                Where(x => x.ProductId == productID && x.Active).
                Include(x => x.CuProductFileAttachments).
                FirstOrDefault();
        }

        public CuProduct GetCustomerProductById(int id)
        {
            return _context.CuProducts.
                Where(x => x.Id == id && x.Active).
                Include(x => x.CuProductFileAttachments).
                FirstOrDefault();
        }

        public async Task<int> AddCustomerProducts(CuProduct entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public Task<int> EditCustomerProducts(CuProduct entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> RemoveCustomerProducts(int id)
        {
            var entity = _context.CuProducts.Where(x => x.Id == id).SingleOrDefault();
            entity.Active = false;
            entity.DeletedBy = _ApplicationContext.UserId;
            entity.DeletedTime = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> RemoveProductFileAttachement(int id)
        {
            var entities = _context.CuProductFileAttachments.Where(x => x.ProductId == id);
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CuProductFileAttachment>> GetReceptFiles(int productId, IEnumerable<string> UniqueIdList)
        {
            return await _context.CuProductFileAttachments.Where(x => x.ProductId == productId && UniqueIdList.Contains(x.UniqueId)).ToListAsync();
        }

        public async Task<CuProductFileAttachment> GetFile(int id)
        {
            return await _context.CuProductFileAttachments.FirstOrDefaultAsync(x => x.Id == id);
        }
        public IEnumerable<CuProduct> GetProductsByCustomerAndCategory(int customerId, int productCategoryId)
        {
            return _context.CuProducts
                .Where(x => x.CustomerId == customerId &&
                (x.ProductCategory == productCategoryId || x.ProductCategory == null) && x.Active)
                .Include(x => x.CuPurchaseOrderDetails);

        }
        /// <summary>
        /// Get the customer products with the categories by product id
        /// </summary>                                  
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FBProductMasterData> GetFBCustomerProduct(int id, int? entityId)
        {
            int msFileType = (int)ProductRefFileType.MSChartExcel;
            return await _context.CuProducts.IgnoreQueryFilters().Where(x => x.Active && x.Id == id && x.EntityId == entityId)
                    .Select(x => new FBProductMasterData()
                    {
                        Id = x.Id,
                        ProductName = x.ProductId,
                        ProductDescription = x.ProductDescription,
                        ProductCategoryId = x.ProductCategoryNavigation.FbProductCategoryId,
                        ProductSubCategoryId = x.ProductSubCategoryNavigation.FbProductSubCategoryId,
                        ProductSub2CategoryId = x.ProductCategorySub2Navigation.FbProductSubCategory2Id,
                        FBCustomerId = x.Customer.FbCusId,
                        FBProducId = x.FbCusProdId,
                        MsChartFileUrl = x.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId == msFileType).Select(x => x.FileUrl).FirstOrDefault()
                    }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get file type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetFileTypeList()
        {
            return await _context.CuProductFileTypes.Where(x => x.Active.Value)
                      .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }
        /// <summary>
        /// Get the product subcategory 2 list
        /// </summary>
        /// <param name="productSubCategoryId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetProductSubCategory2List(int? productSubCategoryId)
        {
            return await _context.RefProductCategorySub2S.Where(x => x.Active && x.Id == productSubCategoryId).
                Select(x => new CommonDataSource() { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        /// <summary>
        /// Get the products by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<CustomerProductDetail>> GetProductsByProductIds(List<int> ProductIds)
        {
            return await _context.CuProducts.Where(x => ProductIds.Contains(x.Id) && x.Active).
                Select(x => new CustomerProductDetail()
                {
                    Id = x.Id,
                    ProductName = x.ProductId,
                    ProductDescription = x.ProductDescription,
                    ProductCategoryId = x.ProductCategory,
                    ProductCategoryName = x.ProductCategoryNavigation.Name,
                    ProductSubCategoryId = x.ProductSubCategory,
                    ProductSubCategoryName = x.ProductSubCategoryNavigation.Name,
                    ProductSubCategory2Id = x.ProductCategorySub2,
                    ProductSubCategory2Name = x.ProductCategorySub2Navigation.Name,
                    ProductSubCategory3Id = x.ProductCategorySub3,
                    ProductSubCategory3Name = x.ProductCategorySub3Navigation.Name,
                    FactoryReference = x.FactoryReference,
                    Barcode = x.Barcode,
                    Remarks = x.Remarks,
                    ProductImageCount = x.CuProductFileAttachments.Where(x => x.Active && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).Select(x => x.Id).Count()
                }).ToListAsync();

        }

        public async Task<List<string>> GetProductNameByProductIds(IEnumerable<int> productIds)
        {
            return await _context.CuProducts.Where(x => x.Active && productIds.Contains(x.Id)).
                Select(x => x.ProductId).ToListAsync();
        }

        /// <summary>
        /// Get thr product file urls by product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetProductImageUrls(int productId)
        {
            return await _context.CuProductFileAttachments.Where(x => x.Active && x.ProductId == productId && x.FileTypeId.HasValue && x.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).
                Select(x => x.FileUrl).ToListAsync();
        }

        public async Task<CustomerProductCategory> GetSubCategoryIdFromCustomerCategory(string subCategory)
        {
            return await _context.CuProductCategories
                .Where(x => x.Name.ToLower().Trim() == subCategory.ToLower().Trim() && x.Active.HasValue && x.Active.Value)
                .Select(x => new CustomerProductCategory()
                {
                    CustomerId = x.CustomerId,
                    Code = x.Code,
                    Name = x.Name,
                    ProductSubCategoryId = x.LinkProductSubCategory,
                    ProductCategoryId = x.LinkProductSubCategoryNavigation.ProductCategoryId
                }).FirstOrDefaultAsync();
        }

        public async Task<CuProductType> GetSubCategoryTwoIdFromCustomerCategory(string productType)
        {
            return await _context.CuProductTypes.Where(x => x.Name.ToLower().Trim() == productType.ToLower().Trim() && x.Active.HasValue && x.Active.Value).FirstOrDefaultAsync();
        }




        #region

        public async Task<CuProduct> GetProductRefByCustomer(int customerId, string productRef)
        {
            return await _context.CuProducts.FirstOrDefaultAsync(x => x.ProductId == productRef && x.CustomerId == customerId && x.Active);
        }

        public async Task<CUProductListResponse> GetProductFileList(int customerId, string productRef)
        {
            var respo = (from cp in _context.CuProducts
                         join cpCategory in _context.CuProductCategories on cp.ProductCategory equals cpCategory.LinkProductSubCategory into customerProductCategory
                         from cpCategory in customerProductCategory.DefaultIfEmpty()
                         join cpType in _context.CuProductTypes on cp.ProductCategorySub2 equals cpType.LinkProductType into customerProductType
                         from cpType in customerProductType.DefaultIfEmpty()
                         join refPc in _context.RefProductCategories on cpCategory.LinkProductSubCategory equals refPc.Id into refProductCategory
                         from refPc in refProductCategory.DefaultIfEmpty()
                         join refSub in _context.RefProductCategorySubs on cpCategory.LinkProductSubCategory equals refSub.Id into refProductSubCategory
                         from refSub in refProductSubCategory.DefaultIfEmpty()
                         where cp.CustomerId == customerId && cp.ProductId == productRef
                         select new CUProductListResponse()
                         {
                             ProductReference = cp.ProductId,
                             ProductRefDescription = cp.ProductDescription,
                             Barcode = cp.Barcode,
                             FactoryReference = cp.FactoryReference,
                             Remarks = cp.Remarks,
                             Category = cpCategory.Name,
                             SubCategory = refSub.Name,
                             ProductType = cpType.Name,
                             files = cp.CuProductFileAttachments.Where(x => x.ProductId == cp.Id && x.Active)
                             .Select(x => new FileTypes
                             {
                                 Type = (int)x.FileTypeId,
                                 TypeName = x.FileName,
                                 UniqueId = x.UniqueId,
                                 Link = x.FileUrl
                             })
                         }).AsNoTracking().AsSingleQuery().FirstOrDefault();

            return respo;
        }
        public async Task<bool> CheckPOExist(int productId)
        {
            return await _context.CuPurchaseOrderDetails.AnyAsync(x => x.ProductId == productId && x.Active == true);
        }

        public IEnumerable<FileTypes> GetFilesByProductId(int productId)
        {
            return _context.CuProductFileAttachments
                .Where(x => x.ProductId == productId && x.Active)
                .Select(x => new FileTypes()
                {
                    Type = Convert.ToInt32(x.FileTypeId),
                    TypeName = Convert.ToString(_context.CuProductFileTypes.Where(y => y.Id == x.FileTypeId).Select(y => y.Name).FirstOrDefault()),
                    UniqueId = x.UniqueId,
                    Link = x.FileUrl
                }).AsSingleQuery();
        }

        #endregion

        /// <summary>
        /// Get the customer products by name
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productNameList"></param>
        /// <returns></returns>
        public async Task<List<CustomerProductDetail>> GetCustomerProductsByName(int customerId, List<string> productNameList)
        {
            return await _context.CuProducts.Where(x => x.Active && x.CustomerId == customerId && productNameList.Contains(x.ProductId.Trim().ToLower())).
                            Select(x => new CustomerProductDetail()
                            {
                                Id = x.Id,
                                ProductName = x.ProductId,
                                ProductDescription = x.ProductDescription,
                                ProductCategoryId = x.ProductCategory,
                                ProductCategoryName = x.ProductCategoryNavigation.Name,
                                ProductSubCategoryId = x.ProductSubCategory,
                                ProductSubCategoryName = x.ProductSubCategoryNavigation.Name,
                                ProductSubCategory2Id = x.ProductCategorySub2,
                                ProductSubCategory2Name = x.ProductCategorySub2Navigation.Name,
                                ProductSubCategory3Id = x.ProductCategorySub3,
                                ProductSubCategory3Name = x.ProductCategorySub3Navigation.Name,
                                FactoryReference = x.FactoryReference,
                                Barcode = x.Barcode,
                                Remarks = x.Remarks,
                            }).AsNoTracking().ToListAsync();
        }

        public async Task<bool> CheckProductExistsForCustomerId(int productid, int customerId)
        {
            return await _context.CuProducts.Where(x => x.Active).AnyAsync();
        }

        public IQueryable<CuProductMschartOcrMap> GetMSChartFileFormatByCustomer(int customerId)
        {
            return _context.CuProductMschartOcrMaps.Where(x => x.Active && x.CustomerId == customerId);
        }
        public async Task<List<FBProductMschartData>> GetFBProductMschart(int productId)
        {
            return await _context.CuProductMscharts.Where(x => x.ProductId == productId && x.ProductFile.Active)
                    .Select(x => new FBProductMschartData()
                    {
                        code = x.Mpcode,
                        description = x.Description,
                        requiredValue = Convert.ToString(x.Required),
                        size = x.Code,
                        tolerancePlus = Convert.ToString(x.Tolerance1Up),
                        toleranceMinus = Convert.ToString(x.Tolerance1Down)
                    }).AsNoTracking().ToListAsync();
        }
    }
}
