using Contracts.Repositories;
using DTO.Common;
using DTO.PurchaseOrder;
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
    public class PurchaseOrderRepository : Repository, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(API_DBContext context) : base(context)
        {
        }

        public IEnumerable<CuPurchaseOrder> GetAllPurchaseOrderItems(PurchaseOrderSearchRequest request)
        {
            return _context.CuPurchaseOrders
                     .Include(c => c.Customer)
                     .Include(x => x.CuPurchaseOrderDetails)
                     .ThenInclude(x => x.Po)
                     .ThenInclude(x => x.InspPurchaseOrderTransactions)
                     .ThenInclude(x => x.Inspection)
                     .Include(x => x.CuPurchaseOrderDetails)
                     .ThenInclude(x => x.Po)
                     .ThenInclude(x => x.InspPurchaseOrderTransactions)
                     .ThenInclude(x => x.ProductRef)
                     .Include(x => x.CuPurchaseOrderDetails)
                     .ThenInclude(x => x.DestinationCountry)
                         .Where(x => x.Active);
        }


        /// <summary>
        /// Get all the purchase order items
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public IQueryable<PurchaseOrderRepoData> GetAllPurchaseOrderData()
        {
            return _context.CuPurchaseOrders
                         .Where(x => x.Active)
                         .Select(x => new PurchaseOrderRepoData()
                         {
                             Id = x.Id,
                             PoId = x.Id,
                             Pono = x.Pono,
                             CustomerId = x.CustomerId,
                             CustomerName = x.Customer.CustomerName
                         });
        }

        //Export Purchase Order
        public IQueryable<PurchaseOrderDetailsRepo> GetAllPurchaseOrderExportData()
        {
            return _context.CuPurchaseOrderDetails
                         .Where(x => x.Active.Value && x.Po.Active)
                         .Select(x => new PurchaseOrderDetailsRepo()
                         {
                             Id = x.Id,
                             Pono = x.Po.Pono,
                             CustomerId = x.Po.CustomerId,
                             CustomerName = x.Po.Customer.CustomerName,
                             CustomerRefNo = x.Po.CustomerReferencePo,
                             DestinationCountry = x.DestinationCountry.CountryName,
                             DestinationCountryId = x.DestinationCountryId,
                             ETD = x.Etd,
                             //FactoryId = x.FactoryId,
                             //FactoryName = x.Factory.SupplierName,
                             //SupplierId = x.SupplierId,
                             //SupplierName = x.Supplier.SupplierName,
                             PoId = x.PoId,
                             ProductId = x.Product.ProductId,
                             ProductDescription = x.Product.ProductDescription,
                             Qty = x.Quantity
                         });
        }

        public async Task<IEnumerable<PurchaseOrderDetailsRepoData>> GetOrderDetails(IEnumerable<int> poIds)
        {

            return await _context.CuPurchaseOrderDetails.Where(x => poIds.Contains(x.PoId) && x.Active.Value)
                .Select(x => new PurchaseOrderDetailsRepoData()
                {
                    Id = x.Id,
                    DestinationCountry = x.DestinationCountry.CountryName,
                    DestinationCountryId = x.DestinationCountryId,
                    ETD = x.Etd,
                    //FactoryId = x.FactoryId,
                    //SupplierId = x.SupplierId,
                    PoId = x.PoId,
                    ProductId = x.ProductId
                }).ToListAsync();
        }


        public Task<int> AddPurchaseOrder(CuPurchaseOrder entity)
        {
            _context.CuPurchaseOrders.Add(entity);
            return _context.SaveChangesAsync();
        }

        public CuPurchaseOrder GetPurchaseOrderItemsByPono(string pono, int customerId)
        {
            return _context.CuPurchaseOrders.Where(x => x.Pono == pono && x.CustomerId == customerId && x.Active).FirstOrDefault();
        }
        public CuPurchaseOrder GetPurchaseOrderItemsById(int? id)
        {
            return _context.CuPurchaseOrders.Where(x => x.Id == id)

                .Include(x => x.CuPurchaseOrderDetails)
                  .ThenInclude(x => x.Po)
                     .ThenInclude(x => x.InspPurchaseOrderTransactions)
                      .ThenInclude(x => x.ProductRef)
                      .ThenInclude(x => x.Inspection)
                  .Include(x => x.CuPurchaseOrderDetails)
                  .ThenInclude(x => x.Product)
                  //  .ThenInclude(x => x.CuProductFileAttachments)
                  .Include(x => x.CuPurchaseOrderDetails).
                  ThenInclude(x => x.Product)
                  .ThenInclude(x => x.ProductSubCategoryNavigation)
                  .ThenInclude(x => x.RefProductCategorySub2S)
                  .Include(x => x.CuPurchaseOrderDetails).
                  ThenInclude(x => x.Product)
                  .ThenInclude(x => x.ProductCategorySub2Navigation)

                  .Include(x => x.CuPurchaseOrderDetails).
                  ThenInclude(x => x.Product)
                  .ThenInclude(x => x.ProductCategoryNavigation)
                  .Include(x => x.CuPurchaseOrderDetails).
                  ThenInclude(x => x.Product)
                  .ThenInclude(x => x.Customer)
                  .ThenInclude(x => x.CuServiceTypes)
                  .Include(x => x.CuPoSuppliers)
                  .Include(x => x.CuPoFactories)

                  .FirstOrDefault();
        }

        public async Task<CUPurchaseOrderListResponse> GetPurchaseOrderDetailsByPo(string pono, int customerId)
        {

            return (from po in _context.CuPurchaseOrders
                    join poDetails in _context.CuPurchaseOrderDetails on po.Id equals poDetails.PoId
                    join product in _context.CuProducts on poDetails.ProductId equals product.Id
                    join country in _context.RefCountries on poDetails.DestinationCountryId equals country.Id into c
                    from country in c.DefaultIfEmpty()
                    where po.Pono == pono && po.CustomerId == customerId && po.Active
                    select new CUPurchaseOrderListResponse()
                    {
                        pono = po.Pono,
                        customerReferencePo = po.CustomerReferencePo,
                        purchaseOrderDetailLists = po.CuPurchaseOrderDetails.Where(x => x.Active.HasValue && x.Active.Value).Select(x => new PurchaseOrderDetailList()
                        {
                            productRef = x.Product.ProductId,
                            productRefDesc = x.Product.ProductDescription,
                            barCode = x.Product.Barcode,
                            factoryRef = x.Product.FactoryReference,
                            qty = x.Quantity,
                            etd = x.Etd.HasValue ? x.Etd.Value.ToString(StandardDateFormat) : null,
                            destinationCountry = x.DestinationCountry.Alpha2Code,
                            productType = (from product in _context.CuProducts
                                           join productCategorySub2 in _context.RefProductCategorySub2S on product.ProductCategorySub2 equals productCategorySub2.Id into productSub2
                                           from productCategorySub2 in productSub2.DefaultIfEmpty()
                                           join prodcutType in _context.CuProductTypes on productCategorySub2.Id equals prodcutType.LinkProductType into productTypes
                                           from prodcutType in productTypes.DefaultIfEmpty()
                                           where product.Id == x.Product.Id
                                           select prodcutType.Name).AsNoTracking().FirstOrDefault(),
                            productSubCategory = (from product in _context.CuProducts
                                                  join productSubCategory in _context.RefProductCategorySubs on product.ProductSubCategory equals productSubCategory.Id into productSub
                                                  from productSubCategory in productSub.DefaultIfEmpty()
                                                  join cuProductCategory in _context.CuProductCategories on productSubCategory.Id equals cuProductCategory.LinkProductSubCategory into cuProductCategories
                                                  from cuProductCategory in cuProductCategories.DefaultIfEmpty()
                                                  where product.Id == x.Product.Id
                                                  select cuProductCategory.Name).AsNoTracking().FirstOrDefault(),
                            supplierCode = (_context.SuSupplierCustomers.Where(y => y.Supplier.CuPoSuppliers.Any(z => z.Active.Value && y.SupplierId == z.SupplierId)).Select(z => z.Code).AsNoTracking().FirstOrDefault()),
                            factoryCode = (_context.SuSupplierCustomers.Where(y => y.Supplier.CuPoFactories.Any(z => z.Active.Value && y.SupplierId == z.FactoryId)).Select(z => z.Code).AsNoTracking().FirstOrDefault())
                        })
                    }).AsNoTracking().AsSingleQuery().FirstOrDefault();
        }



        /// Get the po Data By ID
        public async Task<PurchaseOrder> GetPurchaseOrderById(int? Id)
        {
            return await _context.CuPurchaseOrders.Select(x => new PurchaseOrder()
            {
                Id = x.Id,
                Pono = x.Pono,
                OfficeId = x.OfficeId,
                CustomerId = x.CustomerId.Value,
                DepartmentId = x.DepartmentId,
                BrandId = x.BrandId,
                InternalRemarks = x.InternalRemarks,
                CustomerRemarks = x.CustomerRemarks,
                Active = x.Active,
                CreatedBy = x.CreatedBy,
                CreatedTime = x.CreatedOn,
                CustomerreferencePO = x.CustomerReferencePo,
            }).Where(x => x.Id == Id && x.Active.Value).FirstOrDefaultAsync();
        }
        /// Get the po product details
        public IQueryable<PurchaseOrderRepo> PurchaseOrderDetailsById(int id)
        {
            return _context.CuPurchaseOrderDetails.Where(x => x.PoId == id && x.Active.Value)
                .Select(x => new PurchaseOrderRepo
                {
                    Id = x.Id,
                    PoId = x.PoId,
                    BookingStatus = x.BookingStatus,
                    DestinationCountryId = x.DestinationCountryId,
                    Etd = x.Etd,
                    //FactoryId = x.FactoryId,
                    FactoryReference = x.FactoryReference,
                    ProductDesc = x.Product.ProductDescription,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    //SupplierId = x.SupplierId,
                    ProductName = x.Product.ProductId,
                    //SupplierName = x.Supplier.SupplierName,
                    DestinationCountryName = x.DestinationCountry.CountryName
                });

        }
        /// Get the po Attachments
        public IQueryable<FileAttachment> PurchaseOrderAttachmentsById(int? id)
        {
            return _context.CuPurchaseOrderAttachments.Where(x => x.PoId == id)
                .Select(x => new FileAttachment
                {
                    Id = x.Id,
                    Active = x.Active,
                    FileName = x.FileName,
                    uniqueld = x.GuidId,
                    MimeType = ""
                });

        }
        public IQueryable<CuPurchaseOrderDetail> PurchaseOrderdetail(int id)
        {
            return _context.CuPurchaseOrderDetails.Where(x => x.Id == id);


        }
        public async Task<IEnumerable<int>> GetPoProductIds(int Id, IEnumerable<int> productIds)
        {
            return await _context.InspPurchaseOrderTransactions
                .Include(x => x.ProductRef)
                .Where(x => x.PoId == Id && productIds.Contains(x.ProductRef.ProductId)
                && x.Active.HasValue && x.Active.Value && x.Inspection.StatusId != (int)BookingStatus.Cancel)
                .Select(x => x.ProductRef.ProductId)
                .ToListAsync();
        }

        public IEnumerable<CuPurchaseOrder> GetPurchaseOrderItemsByCustomerId(int? id)
        {
            return _context.CuPurchaseOrders.Where(x => x.CustomerId == id && x.Active);
        }

        public CuPurchaseOrder GetPurchaseOrderItemsByCustomerAndPO(int? id, string pono)
        {

            if (!string.IsNullOrEmpty(pono))
            {
                return _context.CuPurchaseOrders
                    .Include(x => x.CuPurchaseOrderDetails)
                    .ThenInclude(x => x.Po)
                     .Include(x => x.CuPurchaseOrderDetails)
                    .ThenInclude(x => x.Product)
                    .Where(x => x.CustomerId == id && x.Pono.Trim() == pono.Trim() && x.Active).FirstOrDefault();

            }

            return null;
        }

       
        public CuPurchaseOrderDetail GetPurchaseOrderDetailsExist(int poId, int productId, int supplierId)
        {
            return _context.CuPurchaseOrderDetails.Where(x => x.PoId == poId && x.ProductId == productId).FirstOrDefault();
        }

        public CuPurchaseOrder GetPurchaseOrderItemsByCustomerAndPO(int? id, int poid)
        {
            return _context.CuPurchaseOrders.Where(x => x.CustomerId == id && x.Id == poid && x.Active).FirstOrDefault();

        }

        public Task<int> EditPurchaseOrder(CuPurchaseOrder entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> EditPurchaseOrderDetail(CuPurchaseOrderDetail entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<bool> RemovePurchaseOrder(int id, int deletedby)
        {
            var entity = await _context.CuPurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            entity.Active = false;
            entity.DeletedBy = deletedby != 0 ? deletedby : null;
            entity.DeletedOn = DateTime.Now;

            _context.Entry(entity).State = EntityState.Modified;
            int numReturn = await _context.SaveChangesAsync();
            return numReturn > 0;
        }

        public async Task<IEnumerable<CuPurchaseOrderAttachment>> GetReceptFiles(int poId, IEnumerable<Guid> GuidList)
        {
            return await _context.CuPurchaseOrderAttachments.Where(x => x.PoId == poId && GuidList.Contains(x.GuidId)).ToListAsync();
        }

        public async Task<CuPurchaseOrderAttachment> GetFile(int id)
        {
            return await _context.CuPurchaseOrderAttachments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<CuPurchaseOrder> GetPurchaseOrderItems()
        {
            return _context.CuPurchaseOrders.Where(x => x.Active).ToList();
        }
        public IEnumerable<CuPurchaseOrder> GetPurchaseOrderDetailsByCustomerId(int? id)
        {
            return _context.CuPurchaseOrders.
                    Where(x => x.CustomerId == id && x.Active)
                    .Include(x => x.CuPurchaseOrderDetails)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.CuPoSuppliers)
                    .Include(x => x.CuPoFactories);


        }

        public IEnumerable<CuPurchaseOrderDetail> GetPurchaseOrderDetailsById(int? id)
        {
            return _context.CuPurchaseOrderDetails.
                    Where(x => x.Id == id && x.Active.HasValue && x.Active.Value)
                    .Include(x => x.Product);

        }
        /// <summary>
        /// Get the po detail list with autocomplete option
        /// </summary>
        /// <param name="poName"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PoDataSource>> GetPoDetailByNameAndCustomer(string poName, int customerId)
        {
            return await _context.CuPurchaseOrders
                                  .Where(x => x.Active && x.CustomerId == customerId &&
                                   x.Pono != null && EF.Functions.Like(x.Pono, $"%{poName.Trim()}%"))
                                  .Select(x => new PoDataSource { Id = x.Id, Name = x.Pono })
                                  .ToListAsync();

        }

        public IEnumerable<CuPurchaseOrderDetail> GetPurchaseOrderDetailsById(int poid, int productid)
        {
            return _context.CuPurchaseOrderDetails.
                    Where(x => x.PoId == poid && x.ProductId == productid && x.Active.HasValue && x.Active.Value)
                    .Include(x => x.Product);
        }

        public async Task<bool> CheckPoProductIsMappedToBooking(int poid, int productid)
        {
            return await _context.InspPurchaseOrderTransactions.AnyAsync(y => y.Active.Value && y.PoId == poid && y.ProductRef.ProductId == productid);
        }

        /// <summary>
        /// Get the purchase order Iqueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<CuPurchaseOrder> GetPurchaseOrder()
        {
            return _context.CuPurchaseOrders.Where(x => x.Active);
        }

        /// <summary>
        /// Get the purchase order detail as Iqueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<CuPurchaseOrderDetail> GetPurchaseOrderDetails()
        {
            return _context.CuPurchaseOrderDetails.Where(x => x.Active.HasValue && x.Active.Value);
        }

        public IQueryable<CuPurchaseOrderDetail> GetPurchaseOrderDetailsAndPOSupplierData()
        {
            return _context.CuPurchaseOrderDetails
                .Include(x => x.Po)
                .ThenInclude(x => x.CuPoSuppliers)
                .Include(x => x.Po)
                .ThenInclude(x => x.CuPoFactories)
                .Where(x => x.Active.HasValue && x.Active.Value);
        }

        /// <summary>
        /// Get the purchase order by po ids
        /// </summary>
        /// <param name="poList"></param>
        /// <returns></returns>
        public async Task<List<CuPurchaseOrder>> GetPurchaseOrderByPoIds(List<int> poList)
        {
            return await _context.CuPurchaseOrders.Include(x => x.CuPurchaseOrderDetails)
                    .Where(x => x.Active && poList.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        ///  Get the purchase order by pono,customer,supplier
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="supplierId"></param>
        /// <param name="poNameList"></param>
        /// <returns></returns>
        public async Task<List<ExistingPoProductList>> GetPurchaseOrderByPONoCustomerSupplier(int customerId, int supplierId, List<string> poNameList)
        {
            return await _context.CuPurchaseOrderDetails.Where(x => x.Active.Value
                            && x.Po.CustomerId == customerId && x.Active.Value && x.Po.Active && poNameList.Contains(x.Po.Pono.Trim().ToLower())).
                            Select(x => new ExistingPoProductList()
                            {
                                PoId = x.PoId,
                                PoName = x.Po.Pono,
                                ProductId = x.ProductId,
                                ProductName = x.Product.ProductId
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the purchase order details by poids
        /// </summary>
        /// <param name="poList"></param>
        /// <returns></returns>
        public async Task<List<CuPurchaseOrder>> GetPurchaseOrderDetailsByPoIds(List<int> poList)
        {
            return await _context.CuPurchaseOrders.Include(x => x.CuPurchaseOrderDetails).ThenInclude(x => x.Product)
                    .Include(x => x.CuPoSuppliers)
                    .Where(x => x.Active && poList.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        /// get poid and booking number in purchase order
        /// </summary>
        /// <param name="poid"></param>
        /// <returns></returns>
        public async Task<List<POBookingRepo>> GetPurchaseOrderExistInBooking(IEnumerable<int> poid)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => poid.Contains(x.PoId) && x.Active.Value)
                .Select(x => new POBookingRepo()
                {
                    PoId = x.PoId,
                    BookingNumber = x.InspectionId,
                    StatusId = x.Inspection.StatusId
                }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get booking details by poid in purchase order table
        /// </summary>
        /// <param name="poid"></param>
        /// <returns></returns>
        public async Task<List<PoBookingDetailRepo>> GetPOBookingDetailsList(int poid)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.PoId == poid && x.Active.Value)
                .Select(x => new PoBookingDetailRepo()
                {
                    BookingNumber = x.InspectionId,
                    StatusName = x.Inspection.Status.Status,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    StatusId = x.Inspection.StatusId,
                    ServiceFromDate = x.Inspection.ServiceDateFrom,
                    ServiceToDate = x.Inspection.ServiceDateTo
                }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the purchase order by po,product and supplierids
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="supplierId"></param>
        /// <param name="PoIds"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public async Task<List<ExistingPoProductList>> GetPurchaseOrdersByPoProduct(int customerId, List<int> PoIds, List<int> productIds)
        {
            return await _context.CuPurchaseOrderDetails.Where(x => x.Po.CustomerId == customerId && PoIds.Contains(x.Po.Id) && productIds.Contains(x.ProductId)).
                        Select(x => new ExistingPoProductList()
                        {
                            PoId = x.PoId,
                            PoName = x.Po.Pono,
                            ProductId = x.ProductId,
                            ProductName = x.Product.ProductId
                        }).AsNoTracking().ToListAsync();
        }
        public async Task<bool> CheckPurchaseOrderExistInBooking(int poid)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.PoId == poid && x.Active.Value).AnyAsync();
        }

        /// <summary>
        /// Get po mapped supplier details
        /// </summary>
        /// <param name="poList"></param>
        /// <returns></returns>
        public async Task<List<POMappedSupplier>> GetPurchaseOrderSupplierDetails(List<int> poIdList)
        {
            return await _context.CuPoSuppliers.Where(x => x.Active.Value && x.SupplierId != null && poIdList.Contains(x.PoId.GetValueOrDefault())).
                Select(x => new POMappedSupplier()
                {
                    PoId=x.PoId,
                    SupplierId=x.SupplierId,
                    SupplierName=x.Supplier.SupplierName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get po mapped factory details
        /// </summary>
        /// <param name="poList"></param>
        /// <returns></returns>
        public async Task<List<POMappedFactory>> GetPurchaseOrderFactoryDetails(List<int> poIdList)
        {
            return await _context.CuPoFactories.Where(x => x.Active.Value && x.FactoryId!=null && poIdList.Contains(x.PoId.GetValueOrDefault())).
                Select(x => new POMappedFactory()
                {
                    PoId = x.PoId,
                    FactoryId=x.FactoryId,
                    FactoryName = x.Factory.SupplierName
                }).AsNoTracking().ToListAsync();
        }

        public CuPurchaseOrder GetPurchaseOrderDetailsByCustomerAndPO(int? id, string pono)
        {

            if (!string.IsNullOrEmpty(pono))
            {
                return _context.CuPurchaseOrders
                    .Include(x => x.CuPurchaseOrderDetails)
                    .ThenInclude(x => x.Po)
                     .ThenInclude(x => x.CuPoSuppliers)
                     .Include(x => x.CuPurchaseOrderDetails)
                    .ThenInclude(x => x.Po)
                     .ThenInclude(x => x.CuPoFactories)
                    .Where(x => x.CustomerId == id && x.Pono.Trim() == pono.Trim() && x.Active).FirstOrDefault();

            }

            return null;
        }

        /// <summary>
        /// Get the Purchase order supplier factory by po id list
        /// </summary>
        /// <param name="poList"></param>
        /// <returns></returns>
        public async Task<List<CuPurchaseOrder>> GetPurchaseOrderSupplierFactoryByPoIds(List<int> poList)
        {
            return await _context.CuPurchaseOrders.Include(x => x.CuPoSuppliers).Include(x => x.CuPoFactories)
                    .Where(x => x.Active && poList.Contains(x.Id)).ToListAsync();
        }

    }
}
