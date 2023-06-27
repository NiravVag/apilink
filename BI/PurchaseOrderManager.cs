using AutoMapper;
using BI.Maps;
using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.CustomerProducts;
using DTO.File;
using DTO.Inspection;
using DTO.PurchaseOrder;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BI
{
    public class PurchaseOrderManager : ApiCommonData, IPurchaseOrderManager
    {
        private readonly IPurchaseOrderRepository _repo = null;
        private readonly ICustomerProductManager _productmanager = null;
        private readonly ISupplierRepository _supplierRepo = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly IInspectionBookingRepository _bookingRepo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ITenantProvider _filterService = null;
        private ICustomerProductRepository _productRepository = null;
        private readonly IReferenceRepository _referenceRepository = null;
        private readonly ICustomerRepository _customerRepo = null;

        private readonly IFileManager _fileManager = null;
        private readonly IMapper _mapper;
        private readonly PurchaseOrderMapData PurchaseOrderMapData = null;

        public PurchaseOrderManager(ICustomerProductRepository productRepository, IPurchaseOrderRepository repo, IFileManager fileManager, IMapper mapper,
            ICustomerProductManager productManager, ICustomerRepository customerRepository,
            ISupplierRepository supplierRepository, ILocationRepository locationRepository, IInspectionBookingRepository bookingRepo,
            IAPIUserContext applicationContext, ITenantProvider filterService, IReferenceRepository referenceRepository

            )
        {
            _productRepository = productRepository;
            _repo = repo;
            _fileManager = fileManager;
            _productmanager = productManager;
            _supplierRepo = supplierRepository;
            _locationRepo = locationRepository;
            _bookingRepo = bookingRepo;
            _mapper = mapper;
            _ApplicationContext = applicationContext;
            PurchaseOrderMapData = new PurchaseOrderMapData();
            _filterService = filterService;
            _referenceRepository = referenceRepository;
            _customerRepo = customerRepository;
        }
        public async Task<PurchaseOrderDeleteResponse> RemovePurchaseOrder(int id)
        {
            var purchaseOrder = _repo.GetPurchaseOrderItemsById(id);

            if (purchaseOrder == null)
                return new PurchaseOrderDeleteResponse { Id = id, Result = PurchaseOrderDeleteResult.NotFound };

            await _repo.RemovePurchaseOrder(id, _ApplicationContext.UserId);

            return new PurchaseOrderDeleteResponse { Id = id, Result = PurchaseOrderDeleteResult.Success };

        }

        public async Task<PurchaseOrderSearchResponse> GetAllPurchaseOrderItems(PurchaseOrderSearchRequest request)
        {
            var response = new PurchaseOrderSearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            var filterResultData = PurchaseOrderFilterDetails(request.PurchaseOrderData);

            var purchaseOrderQuerable = _repo.GetAllPurchaseOrderData();

            var filterPoIds = filterResultData.Select(x => x.PoId);

            var purchaseOrderList = purchaseOrderQuerable.Where(x => filterPoIds.Contains(x.Id));

            response.TotalCount = purchaseOrderList.Count();

            if (response.TotalCount == 0)
            {
                response.Result = PurchaseOrderSearchResult.NotFound;
                return response;
            }
            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);

            var poDataList = await purchaseOrderList.Skip(skip).Take(request.pageSize.Value).ToListAsync();

            var poIds = poDataList.Select(x => x.PoId);

            var poDetails = await _repo.GetOrderDetails(poIds);

            var _bookingNoPOIdList = await _repo.GetPurchaseOrderExistInBooking(poIds);

            response.Data = poDataList.Select(x => PurchaseOrderMapData.GetPurchaseOrder(x, poDetails.ToList(), _bookingNoPOIdList)).ToArray();

            response.Result = PurchaseOrderSearchResult.Success;

            return response;
        }

        public IQueryable<CuPurchaseOrderDetail> PurchaseOrderFilterDetails(PurchaseOrderData request)
        {
            var purchaseOrderDetails = _repo.GetPurchaseOrderDetails();

            if (request.CustomerId != 0)
            {
                purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Po.CustomerId == request.CustomerId);
            }

            if (!string.IsNullOrEmpty(request.Pono))
            {
                purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Po.Pono.ToLower() == request.Pono.Trim().ToLower());
            }

            if (request.FromEtd != null && request.ToEtd != null)
            {
                purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Etd >= request.FromEtd.ToDateTime().Date &&
                    x.Etd <= request.ToEtd.ToDateTime().Date);
            }
            else
            {
                if (request.FromEtd != null)
                {
                    purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Etd >= request.FromEtd.ToDateTime().Date);
                }
                if (request.ToEtd != null)
                {
                    purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Etd <= request.ToEtd.ToDateTime().Date);
                }
            }

            if (request.SupplierId != null && request.SupplierId != 0)
            {
                purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Po.CuPoSuppliers.Any(y => y.SupplierId == request.SupplierId));
            }

            if (request.FactoryId != null && request.FactoryId != 0)
            {
                purchaseOrderDetails = purchaseOrderDetails.Where(x => x.Po.CuPoFactories.Any(y => y.FactoryId == request.FactoryId));
                //                purchaseOrderDetails = purchaseOrderDetails.Where(x => x.FactoryId == request.FactoryId);
            }

            if (request.DestinationCountry != null && request.DestinationCountry != 0)
            {
                purchaseOrderDetails = purchaseOrderDetails.Where(x => x.DestinationCountryId == request.DestinationCountry);
            }

            return purchaseOrderDetails;
        }

        //Export Purchase Order Products
        public async Task<PurchaseOrderExportDataResponse> PurchaseOrderExportDetails(PurchaseOrderExportRequest request)
        {
            var response = new PurchaseOrderExportDataResponse();

            var resultData = PurchaseOrderFilterDetails(request.PurchaseOrderExportData);

            var data = await resultData.Select(x => new PurchaseOrderDetailsRepo()
            {
                Id = x.Id,
                Pono = x.Po.Pono,
                CustomerId = x.Po.CustomerId,
                CustomerName = x.Po.Customer.CustomerName,
                CustomerRefNo = x.Po.CustomerReferencePo,
                DestinationCountry = x.DestinationCountry.CountryName,
                DestinationCountryId = x.DestinationCountryId,
                ETD = x.Etd,
                PoId = x.PoId,
                ProductId = x.Product.ProductId,
                ProductDescription = x.Product.ProductDescription,
                Qty = x.Quantity
            }).OrderBy(x => x.Pono).AsNoTracking().ToListAsync();

            //take the po id list
            var poIdList = data.Select(x => x.PoId).Distinct().ToList();

            //get the po mapped suppliers
            var poMappedSuppliers = await _repo.GetPurchaseOrderSupplierDetails(poIdList);

            //get the po mapped factories
            var poMappedFactories = await _repo.GetPurchaseOrderFactoryDetails(poIdList);

            response.PurchaseOrderExportDataData = data.Select(x => PurchaseOrderMapData.GetPurchaseOrderExportData(x, poMappedSuppliers, poMappedFactories));

            if (response.PurchaseOrderExportDataData == null || response.PurchaseOrderExportDataData.Count() == 0)
                response.Result = PurchaseOrderSearchResult.NotFound;
            else
                response.Result = PurchaseOrderSearchResult.Success;

            return response;
        }


        public async Task<SavePurchaseOrdersResponse> SavePurchaseOrderFromExcel(List<PurchaseOrderUpload> poList)
        {
            try
            {
                DateTime edtDate;
                var response = new SavePurchaseOrdersResponse();
                List<SavePurchaseOrdersResult> savePurchaseOrdersList = new List<SavePurchaseOrdersResult>();
                List<BookingPurchaseOrdersResult> bookingPurchaseOrdersList = new List<BookingPurchaseOrdersResult>();

                foreach (var poItem in poList)
                {
                    SavePurchaseOrdersResult savePurchaseOrdersResult = new SavePurchaseOrdersResult();

                    // check po already exist for this customer
                    var entity = _repo.GetPurchaseOrderItemsByCustomerAndPO(poItem.CustomerId, poItem.Pono);

                    if (entity != null)
                    {
                        if (entity.CuPurchaseOrderDetails != null)
                        {
                            var CuPurchaseOrderDetails = entity.CuPurchaseOrderDetails.FirstOrDefault(x => x.ProductId == poItem.ProductId);

                            var lstPurchaseOrderDetailToEdit = new List<CuPurchaseOrderDetail>();

                            if (CuPurchaseOrderDetails != null)
                            {
                                // check whether po is already mapped for booking.
                                if (!await _repo.CheckPoProductIsMappedToBooking(poItem.PoId.GetValueOrDefault(), poItem.ProductId))
                                {
                                    CuPurchaseOrderDetails.ProductId = poItem.ProductId;
                                    CuPurchaseOrderDetails.Quantity = poItem.Quantity != "" ? Int32.Parse(poItem.Quantity) : 0;
                                    CuPurchaseOrderDetails.FactoryReference = poItem.FtyRef;
                                    CuPurchaseOrderDetails.DestinationCountryId = poItem.CountryId;
                                    if (DateTime.TryParseExact(poItem.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out edtDate))
                                    {
                                        CuPurchaseOrderDetails.Etd = edtDate;
                                    }

                                    CuPurchaseOrderDetails.BookingStatus = (int)BookingProductStatus.NotUtlized; // Not utilized for booking


                                    lstPurchaseOrderDetailToEdit.Add(CuPurchaseOrderDetails);

                                    if (lstPurchaseOrderDetailToEdit.Count > 0)
                                        _repo.EditEntities(lstPurchaseOrderDetailToEdit);

                                    savePurchaseOrdersResult.Id = poItem.Id;
                                    savePurchaseOrdersResult.purchaseOrderStatus = PurchaseOrderStatus.Uploaded;
                                    savePurchaseOrdersList.Add(savePurchaseOrdersResult);
                                }
                                else
                                {
                                    if (!bookingPurchaseOrdersList.Any(x => x.ProductId == CuPurchaseOrderDetails.Product?.ProductId && x.PoId == CuPurchaseOrderDetails.Po?.Pono))
                                        bookingPurchaseOrdersList.Add(new BookingPurchaseOrdersResult() { PoId = CuPurchaseOrderDetails.Po.Pono, ProductId = CuPurchaseOrderDetails.Product.ProductId });
                                }
                            }
                            else
                            {

                                var poDetail = this.mapUploadDataAsPurchaseOrderDetails(poItem);
                                entity.CuPurchaseOrderDetails.Add(poDetail);
                                var result = await _repo.EditPurchaseOrder(entity);
                                if (result > 0)
                                {
                                    savePurchaseOrdersResult.Id = poItem.Id;
                                    savePurchaseOrdersResult.purchaseOrderStatus = PurchaseOrderStatus.Uploaded;
                                    savePurchaseOrdersList.Add(savePurchaseOrdersResult);
                                }
                            }
                        }

                    }

                    // if po not exist in the table simply add po and po details if po number as valid.
                    else if (!string.IsNullOrEmpty(poItem.Pono))
                    {

                        var result = await AddNewPurchaseOrder(poItem);
                        if (result > 0)
                        {
                            savePurchaseOrdersResult.Id = poItem.Id;
                            savePurchaseOrdersResult.purchaseOrderStatus = PurchaseOrderStatus.Uploaded;
                            savePurchaseOrdersList.Add(savePurchaseOrdersResult);
                        }
                    }

                }
                await _repo.Save();
                response.bookingPurchaseOrdersResult = bookingPurchaseOrdersList;
                response.savePurchaseOrdersResult = savePurchaseOrdersList;
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<int> AddNewPurchaseOrder(PurchaseOrderUpload poItem)
        {

            int resultId = 0;
            // CuPurchaseOrder entity = _mapper.Map<CuPurchaseOrder>(poItem);
            CuPurchaseOrder entity = this.mapUploadDataAsPurchaseOrder(poItem);

            //add supplier to the purchase order
            AddPoSuppliers(new[] { poItem.SupplierId }.ToList(), entity);

            if (entity.CuPurchaseOrderAttachments.Count > 0)
            {
                foreach (var item in entity.CuPurchaseOrderAttachments)
                {
                    item.UserId = _ApplicationContext.UserId;
                }
            }
            if (entity != null)
            {

                var poDetail = this.mapUploadDataAsPurchaseOrderDetails(poItem);
                poDetail.BookingStatus = (int)BookingProductStatus.NotUtlized; // Not utilized for booking
                entity.CuPurchaseOrderDetails.Add(poDetail);
                _repo.AddEntity(poDetail);
                return await _repo.AddPurchaseOrder(entity);
            }
            return resultId;
        }


        private CuPurchaseOrderDetail mapUploadDataAsPurchaseOrderDetails(PurchaseOrderUpload poUploadData)
        {
            CuPurchaseOrderDetail objPurchaseOrder = new CuPurchaseOrderDetail();
            DateTime edtDate;
            objPurchaseOrder.DestinationCountryId = poUploadData.CountryId;
            if (DateTime.TryParseExact(poUploadData.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out edtDate))
            {
                objPurchaseOrder.Etd = edtDate;
            }
            objPurchaseOrder.PoId = poUploadData.PoId.GetValueOrDefault();
            objPurchaseOrder.BookingStatus = (int)BookingProductStatus.NotUtlized; // Not utilized for booking
            objPurchaseOrder.Active = true;
            objPurchaseOrder.CreatedBy = _ApplicationContext.UserId;
            objPurchaseOrder.CreatedTime = DateTime.Now;
            objPurchaseOrder.ProductId = poUploadData.ProductId;
            objPurchaseOrder.Quantity = poUploadData.Quantity != "" ? Int32.Parse(poUploadData.Quantity) : 0;
            objPurchaseOrder.FactoryReference = poUploadData.FtyRef;
            objPurchaseOrder.EntityId = _filterService.GetCompanyId();
            return objPurchaseOrder;
        }

        private CuPurchaseOrder mapUploadDataAsPurchaseOrder(PurchaseOrderUpload poUploadData)
        {
            CuPurchaseOrder objPurchaseOrder = new CuPurchaseOrder();
            objPurchaseOrder.Pono = poUploadData.Pono?.Trim();
            objPurchaseOrder.CustomerId = poUploadData.CustomerId;
            objPurchaseOrder.EntityId = _filterService.GetCompanyId();
            objPurchaseOrder.Active = true;
            objPurchaseOrder.CreatedBy = _ApplicationContext.UserId;
            objPurchaseOrder.CreatedOn = DateTime.Now;
            return objPurchaseOrder;
        }


        public async Task<SavePurchaseOrderResponse> SavePurchaseOrder(PurchaseOrderDetailedInfo request)
        {
            try
            {

                var response = new SavePurchaseOrderResponse();

                if (request.Id == 0)
                {
                    var poId = this.getPoIdByCustomerAndPoNo(request.CustomerId, request.Pono);
                    // check po already exist for this customer
                    if (poId > 0)
                    {
                        return new SavePurchaseOrderResponse { Result = SavePurchaseOrderResult.PurchaseOrderExists, Id = poId };
                    }

                    CuPurchaseOrder entity = _mapper.Map<CuPurchaseOrder>(request);
                    entity.EntityId = _filterService.GetCompanyId();
                    entity.CreatedBy = _ApplicationContext.UserId;
                    entity.CreatedOn = DateTime.Now;

                    if (request.SupplierIds.Any(x => x <= 0))
                        return new SavePurchaseOrderResponse { Result = SavePurchaseOrderResult.SupplierIdCannotBeNullOrZero };

                    //add the supplier data to purchase order
                    AddPoSuppliers(request.SupplierIds, entity);
                    //add the factory data to purchase order
                    AddPoFactories(request.FactoryIds, entity);

                    if (entity.CuPurchaseOrderAttachments.Count > 0)
                    {
                        foreach (var item in entity.CuPurchaseOrderAttachments)
                        {
                            item.UserId = _ApplicationContext.UserId;
                        }
                    }

                    // Add purchase order details list
                    if (request.PurchaseOrderDetails != null)
                    {
                        foreach (var item in request.PurchaseOrderDetails)
                        {
                            var orderdetail = entity.CuPurchaseOrderDetails.
                                Where(x => x.ProductId == item.ProductId);

                            if (orderdetail != null && orderdetail.Any())
                            {
                                return new SavePurchaseOrderResponse { Result = SavePurchaseOrderResult.ProductDuplicate };
                            }

                            item.BookingStatus = (int)BookingProductStatus.NotUtlized; // Not utlized for booking 
                            var poDetail = _mapper.Map<CuPurchaseOrderDetail>(item);
                            poDetail.EntityId = _filterService.GetCompanyId();
                            entity.CuPurchaseOrderDetails.Add(poDetail);
                            _repo.AddEntity(poDetail);
                        }
                    }

                    await _repo.AddPurchaseOrder(entity);
                    response.Id = entity.Id;

                    if (response.Id == 0)
                    {
                        return new SavePurchaseOrderResponse { Result = SavePurchaseOrderResult.PurchaseOrderIsNotSaved };
                    }

                    response.Result = SavePurchaseOrderResult.Success;

                    return response;
                }
                else
                {
                    var entity = _repo.GetPurchaseOrderItemsById(request.Id);

                    if (entity == null)
                        return new SavePurchaseOrderResponse { Result = SavePurchaseOrderResult.PurchaseOrderIsNotFound };

                    request.PurchaseOrderAttachments = request?.PurchaseOrderAttachments ?? new HashSet<FileAttachment>();

                    var newfiles = request.PurchaseOrderAttachments.Where(x => x.Id == 0);

                    //removed files
                    var fiIds = request.PurchaseOrderAttachments.Where(x => x.Id > 0).Select(x => x.Id);
                    var filesToremove = entity.CuPurchaseOrderAttachments.Where(x => !fiIds.Contains(x.Id));
                    var lstremove = new List<CuPurchaseOrderAttachment>();
                    foreach (var fileItem in filesToremove.ToList())
                    {
                        entity.CuPurchaseOrderAttachments.Remove(fileItem);
                        lstremove.Add(fileItem);
                    }
                    if (lstremove.Count > 0)
                        _repo.RemoveEntities(lstremove);

                    //Updated
                    var filesToUpdate = request.PurchaseOrderAttachments.Where(x => x.Id > 0);
                    foreach (var fileItem in filesToUpdate)
                    {
                        var fileEntity = entity.CuPurchaseOrderAttachments.FirstOrDefault(x => x.Id == fileItem.Id);

                        if (fileEntity != null)
                        {
                            fileEntity.FileName = fileItem.FileName?.Trim();
                            fileEntity.UploadDate = DateTime.Now;
                            fileEntity.UserId = _ApplicationContext.UserId;
                            fileEntity.Active = true;
                        }
                    }

                    AddFiles(newfiles, entity);

                    //remove the suppliers from the po if it is not available in the request data
                    RemovePOSuppliers(request, entity);
                    //remove the factories from the po if it is not available in the request data
                    RemovePOFactories(request, entity);

                    if (request.SupplierIds.Any(x => x <= 0))
                        return new SavePurchaseOrderResponse { Result = SavePurchaseOrderResult.SupplierIdCannotBeNullOrZero };

                    //add the suppliers to purchase orders
                    AddPoSuppliers(request.SupplierIds, entity);
                    //add the factories to purchase orders
                    AddPoFactories(request.FactoryIds, entity);

                    this.updatePurchaseOrder(entity, request);

                    if (request.PurchaseOrderDetails != null)
                    {

                        var purchaseOrderDetailResponse = this.updatePurchaseOrderDetails(entity, request);

                        if (purchaseOrderDetailResponse.Result == PurchaseOrderDetailResult.ProductDuplicate)
                            return new SavePurchaseOrderResponse() { Result = SavePurchaseOrderResult.ProductDuplicate };
                    }

                    await _repo.EditPurchaseOrder(entity);
                    response.Id = entity.Id;

                    response.Result = SavePurchaseOrderResult.Success;
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add the supplier data to purchase orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddPoSuppliers(List<int> supplierIds, CuPurchaseOrder entity)
        {
            if (supplierIds != null && supplierIds.Any())
            {
                foreach (var supplierId in supplierIds)
                {
                    if (!entity.CuPoSuppliers.Any(x => x.SupplierId == supplierId && x.Active.HasValue && x.Active.Value))
                    {
                        CuPoSupplier poSupplier = new CuPoSupplier();
                        poSupplier.SupplierId = supplierId;
                        poSupplier.CreatedBy = _ApplicationContext.UserId;
                        poSupplier.CreatedOn = DateTime.Now;
                        poSupplier.Active = true;
                        _repo.AddEntity(poSupplier);
                        entity.CuPoSuppliers.Add(poSupplier);
                    }
                }

            }
        }

        /// <summary>
        /// Add the factories to purchase order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddPoFactories(List<int> factoryIds, CuPurchaseOrder entity)
        {
            if (factoryIds != null && factoryIds.Any())
            {
                foreach (var factoryId in factoryIds)
                {
                    if (!entity.CuPoFactories.Any(x => x.FactoryId == factoryId && x.Active.HasValue && x.Active.Value))
                    {
                        CuPoFactory poFactory = new CuPoFactory();
                        poFactory.FactoryId = factoryId;
                        poFactory.Active = true;
                        poFactory.CreatedBy = _ApplicationContext.UserId;
                        poFactory.CreatedOn = DateTime.Now;
                        _repo.AddEntity(poFactory);
                        entity.CuPoFactories.Add(poFactory);
                    }
                }
            }
        }

        /// <summary>
        /// Remove the supplier from po if it is not available in the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void RemovePOSuppliers(PurchaseOrderDetailedInfo request, CuPurchaseOrder entity)
        {
            //get the supplier ids
            var supplierIds = request.SupplierIds.Where(x => x > 0).Select(x => x).Distinct().ToList();
            //take the supplier ids from entity which is not available in the request
            var supplierListToRemove = entity.CuPoSuppliers.Where(x => x.Active.HasValue
                && x.Active.Value && !supplierIds.Contains(x.SupplierId.GetValueOrDefault())).ToList();
            var lstSupplierRemove = new List<CuPoSupplier>();
            //loop through the supplier remove list and make inactive
            foreach (var supplierEntity in supplierListToRemove)
            {
                supplierEntity.Active = false;
                supplierEntity.DeletedBy = _ApplicationContext.UserId;
                supplierEntity.DeletedOn = DateTime.Now;
                //entity.CuPoSuppliers.Remove(supplierEntity);
                lstSupplierRemove.Add(supplierEntity);
            }
            if (lstSupplierRemove.Count > 0)
                _repo.EditEntities(lstSupplierRemove);
        }

        /// <summary>
        /// Remove the factories which is not available in the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void RemovePOFactories(PurchaseOrderDetailedInfo request, CuPurchaseOrder entity)
        {
            if (request.FactoryIds == null)
            {
                request.FactoryIds = new List<int>();
            }
            //take the factory id from the request data
            var factoryIds = request.FactoryIds?.Where(x => x > 0).Select(x => x).Distinct().ToList();
            //take the factory ids from entity which is not available in the request
            var factoryListToRemove = entity.CuPoFactories.Where(x => x.Active.HasValue
                && x.Active.Value && !factoryIds.Contains(x.FactoryId.GetValueOrDefault())).ToList();
            var lstFactoryRemove = new List<CuPoFactory>();
            //loop through the factory list and make inactive
            foreach (var factoryEntity in factoryListToRemove)
            {
                factoryEntity.Active = false;
                factoryEntity.DeletedBy = _ApplicationContext.UserId;
                factoryEntity.DeletedOn = DateTime.Now;
                //entity.CuPoFactories.Remove(factoryEntity);
                lstFactoryRemove.Add(factoryEntity);
            }
            if (lstFactoryRemove.Count > 0)
                _repo.EditEntities(lstFactoryRemove);
        }

        private CuPurchaseOrderDetail MapPurchasrOrderDetailsData(CustomerPurchaseOrderDetails req)
        {

            return new CuPurchaseOrderDetail()
            {
                Etd = (!string.IsNullOrEmpty(req.Etd) ? DateTime.ParseExact(req.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None) : null),
                Quantity = req.Qty,
                CreatedTime = DateTime.Now,
                Active = true,
                BookingStatus = 3,
                CreatedBy = _ApplicationContext.UserId // In this clamis we don't get userIds

            };

        }

        private void UpdateMapPurchasrOrderDetailsData(CuPurchaseOrderDetail entity, CustomerPurchaseOrderDetails req)
        {
            entity.Etd = (!string.IsNullOrEmpty(req.Etd) ? DateTime.ParseExact(req.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None) : null);
            entity.Quantity = req.Qty;
            entity.Active = true;
        }

        private async void MapSupplierAndFactoryCode(CuPurchaseOrderDetail entity, CustomerPurchaseOrderDetails req, CuPurchaseOrder purchaseOrderEntity)
        {
            if (!string.IsNullOrEmpty(req.SupplierCode))
            {
                var supplierId = await _supplierRepo.GetSupplierIdByCode(req.SupplierCode);
                if (supplierId != 0)
                {
                    AddPoSuppliers(new[] { supplierId }.ToList(), purchaseOrderEntity);
                }
            }
            if (!string.IsNullOrEmpty(req.FactoryCode))
            {
                var factoryId = await _supplierRepo.GetSupplierIdByCode(req.FactoryCode);
                if (factoryId != 0)
                {
                    AddPoFactories(new[] { factoryId }.ToList(), purchaseOrderEntity);
                }
            }
        }
        //Update Purchase order if it is exist in Db

        private void updatePurchaseOrder(CuPurchaseOrder entity, CustomerPurchaseOrderDetails request)
        {
            entity.CustomerReferencePo = request.CustomerReferencePo?.Trim();
            entity.UpdatedBy = _ApplicationContext.UserId;
            entity.UpdatedOn = DateTime.Now;
        }
        //Update Purchase order details if it is exist in Db
        private async Task AddPurchaseOrderDetails(CuPurchaseOrder entity, CustomerPurchaseOrderDetails request, int ProductId)
        {
            var purchaseOrderDetails = MapPurchasrOrderDetailsData(request);
            purchaseOrderDetails.EntityId = _filterService.GetCompanyId();
            purchaseOrderDetails.ProductId = ProductId;

            MapSupplierAndFactoryCode(purchaseOrderDetails, request, entity);
            var countryEntity = await _referenceRepository.GetCountryDetailsByAlphaCode(request.DestinationCountry);
            if (countryEntity != null)
            {
                purchaseOrderDetails.DestinationCountryId = countryEntity.Id;
            }
            entity.CuPurchaseOrderDetails.Add(purchaseOrderDetails);

        }

        //Update Purchase order details if it is exist in Db
        private async Task ModifyPurchaseOrderDetails(CuPurchaseOrder entity, CustomerPurchaseOrderDetails request, int ProductId)
        {
            var purchaseOrderDetails = entity.CuPurchaseOrderDetails.FirstOrDefault(x => x.PoId == entity.Id && x.ProductId == ProductId);
            UpdateMapPurchasrOrderDetailsData(purchaseOrderDetails, request);
            purchaseOrderDetails.EntityId = _filterService.GetCompanyId();
            purchaseOrderDetails.ProductId = ProductId;

            MapSupplierAndFactoryCode(purchaseOrderDetails, request, entity);
            var countryEntity = await _referenceRepository.GetCountryDetailsByAlphaCode(request.DestinationCountry);
            if (countryEntity != null)
            {
                purchaseOrderDetails.DestinationCountryId = countryEntity.Id;
            }
            entity.CuPurchaseOrderDetails.Add(purchaseOrderDetails);
            _repo.EditEntity(purchaseOrderDetails);

        }

        public async Task<CommonCustomerPurchaseOrderResponse> UpdateCustomerPurchaseDetails(CustomerPurchaseOrderDetails request)
        {
            var response = new CommonCustomerPurchaseOrderResponse();
            try
            {

                //get Cutomer Id from Claim
                var customerId = _ApplicationContext.CustomerId;


                //Check CustomerId exist in System

                var customerData = await _customerRepo.GetCustomerItemByIdForCFL(customerId);
                if (customerData == null)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = HttpStatusCode.BadRequest
                    };

                // Check if PONO exist for customer
                var cuPO = _repo.GetPurchaseOrderItemsByCustomerAndPO(customerId, request.Pono);



                if (cuPO == null)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Po record not found" },
                        message = "Not Found",
                        statusCode = HttpStatusCode.NotFound
                    };

                // Check product is exist for Customer
                var productEntity = _productRepository.GetProductsByCustomerAndProducts(customerId, request.ProductRef);
                // get SupplierId  
                var SupplierId = await _productRepository.GetSupplierId(request.SupplierCode);

                if (SupplierId == null)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Supplier is not exist in our system" },
                        message = "Not Found",
                        statusCode = HttpStatusCode.NotFound
                    };
                if (productEntity == null)
                {
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Product not found for PO" },
                        message = "Not Found",
                        statusCode = HttpStatusCode.NotFound
                    };
                }
                else
                {
                    var purchaseOrderDetails = _repo.GetPurchaseOrderDetailsExist(cuPO.Id, productEntity.Id, SupplierId.SupplierId);
                    if (purchaseOrderDetails == null)
                    {
                        return new CommonCustomerPurchaseOrderResponse
                        {
                            errors = new List<string>() { "Product not found for supplier" },
                            message = "Not Found",
                            statusCode = HttpStatusCode.NotFound
                        };
                    }
                }

                var inspdetails = await _bookingRepo.GetPODataBypoId(cuPO.Pono);
                if (inspdetails)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Purchase order details can’t update because the booking was in progress" },
                        message = "Bad Request",
                        statusCode = HttpStatusCode.BadRequest
                    };

                if (!string.IsNullOrEmpty(request.Etd))
                {

                    if (!DateTime.TryParseExact(request.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime etddate))
                    {
                        return new CommonCustomerPurchaseOrderResponse()
                        {
                            errors = new List<string>() { string.Format(CustomerReportDetailsErrorMessages.InvalidDateFormat, "ETD") },
                            statusCode = HttpStatusCode.BadRequest,
                            message = "Bad Request"
                        };
                    }
                }

                //Update PO
                this.updatePurchaseOrder(cuPO, request);

                // Update PurchaseOrder details
                await this.ModifyPurchaseOrderDetails(cuPO, request, productEntity.Id);

                await _repo.EditPurchaseOrder(cuPO);

                response.message = "Success";
                response.statusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return new CommonCustomerPurchaseOrderResponse()
                {
                    errors = new List<string>() { CustomerReportDetailsErrorMessages.InternalServerError },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
            return response;
        }
        /// <summary>
        /// Save Purchase Order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CommonCustomerPurchaseOrderResponse> SaveCustomerPurchaseDetails(CustomerPurchaseOrderDetails request)
        {
            var response = new CommonCustomerPurchaseOrderResponse();
            try
            {


                //get Cutomer Id from Claim
                var customerId = _ApplicationContext.CustomerId;


                //Check CustomerId exist in System

                var customerData = await _customerRepo.GetCustomerItemByIdForCFL(customerId);
                if (customerData == null)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };

                if (!string.IsNullOrEmpty(request.Etd))
                {
                    if (!DateTime.TryParseExact(request.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime etddate))
                    {
                        return new CommonCustomerPurchaseOrderResponse()
                        {
                            errors = new List<string>() { string.Format(CustomerReportDetailsErrorMessages.InvalidDateFormat, "ETD") },
                            statusCode = HttpStatusCode.BadRequest,
                            message = "Bad Request"
                        };
                    }
                }

                // Check if PONO exist for customer
                var cuPO = _repo.GetPurchaseOrderItemsByCustomerAndPO(customerId, request.Pono);

                // Check product is exist for Customer
                var productEntity = _productRepository.GetProductsByCustomerAndProducts(customerId, request.ProductRef);

                // get SupplierId  
                var GetSupplierId = await _productRepository.GetSupplierId(request.SupplierCode);


                // check the product exists for Po & supplier
                if (cuPO != null && productEntity != null)
                {
                    var purchaseOrderDetail = _repo.GetPurchaseOrderDetailsExist(cuPO.Id, productEntity.Id, GetSupplierId.SupplierId);

                    if (purchaseOrderDetail != null)
                        return new CommonCustomerPurchaseOrderResponse
                        {
                            errors = new List<string>() { "Record already exists" },
                            message = "Bad Request",
                            statusCode = System.Net.HttpStatusCode.BadRequest
                        };
                }

                // Get Api Link Product Sub Category Id from Customer's sub Product categroy
                var customerProduct = await _productRepository.GetSubCategoryIdFromCustomerCategory(request.ProductSubCateory);

                // Get Api Link Product Sub Two Category Id from Customer's Product Type
                var productType = await _productRepository.GetSubCategoryTwoIdFromCustomerCategory(request.ProductType);

                if (productEntity == null)
                {
                    productEntity = MapCustomerProduct(request, customerId);

                    if (customerProduct != null)
                    {
                        productEntity.ProductCategory = customerProduct.ProductCategoryId;
                        productEntity.ProductSubCategory = customerProduct.ProductSubCategoryId;
                    }
                    if (productType != null)
                    {
                        productEntity.ProductCategorySub2 = productType.LinkProductType;
                    }
                    // Add new Customer Product
                    await _productRepository.AddCustomerProducts(productEntity);
                }

                if (cuPO != null)
                {

                    // Add PurchaseOrder details
                    await this.AddPurchaseOrderDetails(cuPO, request, productEntity.Id);

                    await _repo.EditPurchaseOrder(cuPO);
                    response.message = "Success";
                    response.statusCode = HttpStatusCode.Created;

                }
                else
                {

                    //Map Purchase order data
                    var poEntity = MapCuPurchaseOrder(request, customerId);

                    // Map purchase order details data
                    var purchaseOrderDetails = MapPurchasrOrderDetailsData(request);

                    MapSupplierAndFactoryCode(purchaseOrderDetails, request, poEntity);

                    //Get Country Data from AlphaTwoCode
                    var countryEntity = await _referenceRepository.GetCountryDetailsByAlphaCode(request.DestinationCountry);
                    purchaseOrderDetails.EntityId = _filterService.GetCompanyId();
                    purchaseOrderDetails.ProductId = productEntity.Id;
                    if (countryEntity != null)
                    {
                        purchaseOrderDetails.DestinationCountryId = countryEntity.Id;
                    }
                    poEntity.CuPurchaseOrderDetails.Add(purchaseOrderDetails);
                    _repo.AddEntity(purchaseOrderDetails);


                    await _repo.AddPurchaseOrder(poEntity);
                    response.message = "Success";
                    response.statusCode = HttpStatusCode.Created;
                }

            }
            catch (Exception ex)
            {
                return new CommonCustomerPurchaseOrderResponse()
                {
                    errors = new List<string>() { CustomerReportDetailsErrorMessages.InternalServerError },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
            return response;
        }
        /// <summary>
        /// Delete Customer Purchase Order details
        /// </summary>
        /// <param name="pono"></param>
        /// <returns></returns>
        public async Task<CommonCustomerPurchaseOrderResponse> Deletecustomerpurchaseorder(string pono)
        {
            try
            {
                //get Cutomer Id from Claim
                var customerId = _ApplicationContext.CustomerId;

                var customerData = await _customerRepo.GetCustomerItemByIdForCFL(customerId);
                if (customerData == null)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = HttpStatusCode.BadRequest
                    };

                // Get Purchase order details by PONO
                var purchaseOrder = _repo.GetPurchaseOrderItemsByPono(pono, customerId);

                if (purchaseOrder == null)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        message = "Not Found",
                        errors = new List<string>() { "Po Record Not Found" },
                        statusCode = System.Net.HttpStatusCode.NotFound
                    };

                // Check If Po is exist in booking 
                var inspdetails = await _bookingRepo.GetPODataBypoId(purchaseOrder.Pono);

                if (inspdetails)
                    return new CommonCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Purchase order details can’t delete because the booking was in progress." },
                        message = "Bad Request",
                        statusCode = HttpStatusCode.BadRequest
                    };

                await _repo.RemovePurchaseOrder(purchaseOrder.Id, _ApplicationContext.UserId);
                return new CommonCustomerPurchaseOrderResponse
                {
                    message = "Success",
                    statusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new CommonCustomerPurchaseOrderResponse()
                {
                    errors = new List<string>() { CustomerReportDetailsErrorMessages.InternalServerError },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
        }
        /// <summary>
        /// Get Purchase Order Details by PONO
        /// </summary>
        /// <param name="pono">Po number</param>
        /// <returns></returns>
        public async Task<GetCustomerPurchaseOrderResponse> GetPurchaseOrderDetails(string pono)
        {
            var response = new GetCustomerPurchaseOrderResponse();
            try
            {
                //get Cutomer Id from Claim
                var customerId = _ApplicationContext.CustomerId;

                var customerData = await _customerRepo.GetCustomerItemByIdForCFL(customerId);
                if (customerData == null)
                    return new GetCustomerPurchaseOrderResponse
                    {
                        errors = new List<string>() { "Customer id is not exist in our system" },
                        message = "Bad Request",
                        statusCode = HttpStatusCode.BadRequest
                    };

                if (pono != null)
                {
                    // Get Purchase Order data by PONO
                    var data = await _repo.GetPurchaseOrderDetailsByPo(pono, customerId);

                    if (data != null)
                    {
                        response.data = data;
                        response.statusCode = HttpStatusCode.OK;
                        response.message = "Success";
                    }
                    else
                    {
                        response.errors = new List<string>() { "No record found" };
                        response.message = "Not Found";
                        response.statusCode = HttpStatusCode.NotFound;
                    }

                    //  check PO exist in any booking
                    if (data != null)
                    {
                        var inspdetails = await _bookingRepo.GetPODataBypoId(data.pono);

                        if (inspdetails)
                        {
                            response.data.status = await _bookingRepo.GetBoookingStatus(data.pono);
                        }
                        else
                        {
                            response.data.status = "draft";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new GetCustomerPurchaseOrderResponse()
                {
                    errors = new List<string>() { CustomerReportDetailsErrorMessages.InternalServerError },
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal ServerError"
                };
            }
            return response;
        }


        public CuPurchaseOrder MapCuPurchaseOrder(CustomerPurchaseOrderDetails request, int customerId)
        {
            CuPurchaseOrder entity = new CuPurchaseOrder();

            entity.Pono = request.Pono;
            entity.CustomerReferencePo = request.CustomerReferencePo;
            entity.CustomerId = customerId;
            entity.CreatedOn = DateTime.Now;
            entity.CreatedBy = _ApplicationContext.UserId;
            entity.Active = true;
            entity.EntityId = _filterService.GetCompanyId();
            return entity;
        }

        private void AddFiles(IEnumerable<FileAttachment> files, CuPurchaseOrder purchaseOrder)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var ecFile = new CuPurchaseOrderAttachment
                    {
                        FileName = file.FileName?.Trim(),
                        GuidId = file.uniqueld,
                        UploadDate = DateTime.Now,
                        UserId = _ApplicationContext.UserId
                    };
                    purchaseOrder.CuPurchaseOrderAttachments.Add(ecFile);
                }
            }

        }

        private void updatePurchaseOrder(CuPurchaseOrder entity, PurchaseOrderDetailedInfo request)
        {
            entity.CustomerId = request.CustomerId;
            entity.DepartmentId = request.DepartmentId;
            entity.Pono = request.Pono?.Trim();
            entity.OfficeId = request.OfficeId;
            entity.BrandId = request.BrandId;
            entity.InternalRemarks = request.InternalRemarks?.Trim();
            entity.CustomerRemarks = request.CustomerRemarks?.Trim();
            entity.CustomerReferencePo = request.CustomerReferencePo?.Trim();
        }

        private int getPoIdByCustomerAndPoNo(int customerId, string pono)
        {
            int poId = 0;

            var poData = _repo.GetPurchaseOrderItemsByCustomerAndPO(customerId, pono);

            if (poData != null)
            {

                poId = poData.Id;
            }

            return poId;

        }



        private PurchaseOrderDetailResponse updatePurchaseOrderDetails(CuPurchaseOrder entity, PurchaseOrderDetailedInfo request)
        {

            if (request.PurchaseOrderDetails != null)
            {
                foreach (var item in request.PurchaseOrderDetails.Where(x => x.Id <= 0))
                {
                    var orderdetail = entity.CuPurchaseOrderDetails.
                        Where(x => x.ProductId == item.ProductId && x.Active.HasValue && x.Active.Value);

                    if (orderdetail != null && orderdetail.Any())
                    {
                        return new PurchaseOrderDetailResponse { Result = PurchaseOrderDetailResult.ProductDuplicate };
                    }

                    item.BookingStatus = (int)BookingProductStatus.NotUtlized; // Not utlized for booking 
                    CuPurchaseOrderDetail purchaseOrderDetail = _mapper.Map<CuPurchaseOrderDetail>(item);
                    purchaseOrderDetail.EntityId = _filterService.GetCompanyId();
                    entity.CuPurchaseOrderDetails.Add(purchaseOrderDetail);
                }


                var lstPurchaseOrderDetailToEdit = new List<CuPurchaseOrderDetail>();
                foreach (var item in request.PurchaseOrderDetails.Where(x => x.Id > 0))
                {
                    var purchaseOrderDetail = entity.CuPurchaseOrderDetails.FirstOrDefault(x => x.Id == item.Id);

                    if (purchaseOrderDetail != null)
                    {
                        purchaseOrderDetail.ProductId = item.ProductId;
                        purchaseOrderDetail.Quantity = item.Quantity;
                        //purchaseOrderDetail.SupplierId = item.SupplierId;
                        purchaseOrderDetail.FactoryReference = item.FactoryReference;
                        //purchaseOrderDetail.FactoryId = item.FactoryId;
                        purchaseOrderDetail.DestinationCountryId = item.DestinationCountryId;
                        purchaseOrderDetail.Etd = item.Etd?.ToNullableDateTime();

                        lstPurchaseOrderDetailToEdit.Add(purchaseOrderDetail);
                    }
                }

                if (lstPurchaseOrderDetailToEdit.Count > 0)
                    _repo.EditEntities(lstPurchaseOrderDetailToEdit);
            }

            return new PurchaseOrderDetailResponse() { Result = PurchaseOrderDetailResult.Success };

        }


        public async Task<EditPurchaseOrderResponse> GetPurchaseOrderItemsById(int? id)
        {
            var response = new EditPurchaseOrderResponse();

            if (id != null)
            {
                var purchaseOrder = _repo.GetPurchaseOrderItemsById(id);

                if (purchaseOrder == null)
                {
                    response.PurchaseOrderData = null;

                }
                else
                {
                    response.PurchaseOrderData = _mapper.Map<PurchaseOrderDetailedInfo>(purchaseOrder);

                    if (response.PurchaseOrderData.PurchaseOrderAttachments != null)
                    {
                        foreach (var item in response.PurchaseOrderData.PurchaseOrderAttachments)
                        {
                            item.MimeType = _fileManager.GetMimeType(Path.GetExtension(item.FileName));
                        }
                    }

                    foreach (var item in purchaseOrder.CuPurchaseOrderDetails.Where(x => x.Active.HasValue && x.Active.Value))
                    {

                        var productList = purchaseOrder.InspPurchaseOrderTransactions.Where(x => x.ProductRef.ProductId == item.ProductId
                                                         && x.Active.HasValue && x.Active.Value && x.Inspection.StatusId != (int)BookingStatus.Cancel);
                        if (productList != null && productList.Any())
                        {
                            response.PurchaseOrderData.PurchaseOrderDetails.FirstOrDefault(x => x.Id == item.Id).IsBooked = true;
                        }
                    }
                }


                if (response.PurchaseOrderData == null)
                    return new EditPurchaseOrderResponse { Result = EditPurchaseOrderResult.CannotGetPurchaseOrder };
            }
            response.Result = EditPurchaseOrderResult.Success;
            return response;
        }


        /// <summary>
        /// Get the po Data By ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<PurchaseOrderResponse> GetPurchaseOrderById(int? Id)
        {
            var response = new PurchaseOrderResponse();
            if (Id != null)
            {
                var data = await _repo.GetPurchaseOrderById(Id);

                if (data != null)
                {
                    response.PurchaseOrderData = data;

                    //get the po mapped suppliers
                    var poMappedSuppliers = await _repo.GetPurchaseOrderSupplierDetails(new[] { Id.GetValueOrDefault() }.ToList());
                    //get the po mapped factories
                    var poMappedFactories = await _repo.GetPurchaseOrderFactoryDetails(new[] { Id.GetValueOrDefault() }.ToList());

                    //map the supplier data
                    if (poMappedSuppliers.Any())
                        response.PurchaseOrderData.SupplierData = poMappedSuppliers.
                                                                    Select(x => new CommonDataSource()
                                                                    {
                                                                        Id = x.SupplierId.GetValueOrDefault(),
                                                                        Name = x.SupplierName
                                                                    }).ToList();
                    //map the factory data 
                    if (poMappedFactories.Any())
                        response.PurchaseOrderData.FactoryData = poMappedFactories.
                                                                    Select(x => new CommonDataSource()
                                                                    {
                                                                        Id = x.FactoryId.GetValueOrDefault(),
                                                                        Name = x.FactoryName
                                                                    }).ToList();

                    response.Result = EditPurchaseOrderResult.Success;
                }
                else
                {
                    response.Result = EditPurchaseOrderResult.CannotGetPurchaseOrder;
                }
            }
            response.Result = EditPurchaseOrderResult.Success;
            return response;
        }


        /// <summary>
        /// Get the po product details with paging
        /// </summary>
        /// <returns></returns>
        public async Task<PurchaseOrderDetailsResponse> PurchaseOrderDetailsById(PurchaseOrderDetailsRequest request)
        {
            try
            {
                var data = _repo.PurchaseOrderDetailsById(request.Id);



                if (request.Index == null || request.Index.Value <= 0)
                    request.Index = 1;

                if (request.pageSize == null || request.pageSize.Value == 0)
                    request.pageSize = 10;

                int skip = (request.Index.Value - 1) * request.pageSize.Value;

                int take = request.pageSize.Value;

                var _totalCount = await data.CountAsync();

                var _result = await data.Skip(skip).Take(take).ToListAsync();

                var poProducts = _result.Select(x => x.ProductId).ToList();

                var products = await _repo.GetPoProductIds(request.Id, poProducts);

                var resultData = _result.Select(x => PurchaseOrderMapData.GetPurchaseOrderDetailsList(x, products));

                return new PurchaseOrderDetailsResponse()
                {
                    Result = EditPurchaseOrderResult.Success,
                    TotalCount = _totalCount,
                    Index = request.Index.Value,
                    PageSize = request.pageSize.Value,
                    PageCount = (_totalCount / request.pageSize.Value) + (_totalCount % request.pageSize.Value > 0 ? 1 : 0),
                    Data = resultData
                };
            }
            catch (Exception ex)
            {
                return new PurchaseOrderDetailsResponse() { Result = EditPurchaseOrderResult.CannotGetPurchaseOrder };
            }
        }
        /// <summary>
        /// Get the po Attachments
        /// </summary>
        /// <returns></returns>
        public async Task<PurchaseOrderAttachmentsResponse> PurchaseOrderAttachmentsById(int? Id)
        {
            try
            {
                var response = new PurchaseOrderAttachmentsResponse();
                if (Id != null)
                {
                    var data = await _repo.PurchaseOrderAttachmentsById(Id).ToListAsync();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.MimeType = _fileManager.GetMimeType(Path.GetExtension(item.FileName));
                        }
                        response.Result = EditPurchaseOrderResult.Success;
                        response.Data = data;
                    }
                    else
                    {
                        response.Result = EditPurchaseOrderResult.CannotGetPurchaseOrder;
                        return response;
                    }
                }
                response.Result = EditPurchaseOrderResult.Success;
                return response;

            }
            catch (Exception ex)
            {
                return new PurchaseOrderAttachmentsResponse() { Result = EditPurchaseOrderResult.CannotGetPurchaseOrder };
            }
        }
        /// Remove PO pruduct 
        public async Task<PurchaseOrderResponse> RemovePurchaseOrderDetail(RemovePurchaseOrderDetailsRequest request)
        {
            var response = new PurchaseOrderResponse();
            if (request.Id >= 0)
            {
                var entity = _repo.GetPurchaseOrderDetailsById(request.Id).FirstOrDefault();
                if (entity == null)
                {
                    response.Result = EditPurchaseOrderResult.CannotGetPurchaseOrder;
                    return response;
                }
                if (request.AccessType == (int)PurchaseOrderAccessType.PurchaseOrder)
                {

                    entity.Active = false;
                    entity.DeletedBy = _ApplicationContext.UserId;
                    entity.DeletedTime = DateTime.Now;
                    _repo.EditEntity(entity);
                    await _repo.Save();
                }
            }
            else
            {
                response.Result = EditPurchaseOrderResult.CannotGetPurchaseOrder;
                return response;
            }

            response.Result = EditPurchaseOrderResult.Success;
            return response;
        }

        public async Task<PurchaseOrderSearchResponse> GetPurchaseOrderItemsByCustomerId(int? id)
        {
            var response = new PurchaseOrderSearchResponse { Index = 0, PageSize = 100 };

            if (id != null)
            {
                var purchaseOrder = _repo.GetPurchaseOrderItemsByCustomerId(id);

                if (purchaseOrder == null)
                {
                    response.Data = null;
                    response.Result = PurchaseOrderSearchResult.NotFound;
                }
                else
                {
                    response.Data = purchaseOrder.Select(x => PurchaseOrderMapData.GetPurchaseOrderList(x)).ToArray();
                }
            }
            response.Result = PurchaseOrderSearchResult.Success;
            return response;
        }

        public async Task UploadFiles(int poid, Dictionary<Guid, byte[]> fileList)
        {
            var guidList = fileList.Select(x => x.Key);
            var data = await _repo.GetReceptFiles(poid, guidList);

            foreach (var item in data)
                item.File = fileList[item.GuidId];

            await _repo.Save();

        }

        public async Task<FileResponse> GetFile(int id)
        {
            var file = await _repo.GetFile(id);

            if (file == null)
                return new FileResponse { Result = FileResult.NotFound };

            return new FileResponse
            {
                Content = file.File,
                MimeType = _fileManager.GetMimeType(Path.GetExtension(file.FileName)),
                Result = FileResult.Success
            };
        }

        public PurchaseOrderUploadResponse GetUploadedPurchaseOrders(int customerId, IFormFile file)
        {
            PurchaseOrderUploadResponse response = new PurchaseOrderUploadResponse();
            PurchaseOrderUploadMap purchaseOrderUploadMap = new PurchaseOrderUploadMap();
            var purchaseOrdersList = purchaseOrderUploadMap.GetPurchaseOrderList(file);

            if (purchaseOrdersList == null)
            {
                response.Result = PurchaseOrderUploadResult.NotAbleToProcess;
            }
            else if (purchaseOrdersList.Count == 0)
            {
                response.Result = PurchaseOrderUploadResult.NotAbleToProcess;
            }
            var purchaseOrderItems = _repo.GetPurchaseOrderItems();
            var customerProducts = _productmanager.GetCustomerProductsByCustomer(customerId);

            var supplierDetails = _supplierRepo.GetAllSuppliers();
            var countryList = _locationRepo.GetCountryList().ToList();

            if (purchaseOrdersList != null)
            {
                foreach (var item in purchaseOrdersList)
                {

                    var product = customerProducts.FirstOrDefault(x => x.ProductId?.ToLower().Trim() == item?.Product?.ToLower().Trim());
                    item.purchaseOrderStatus = PurchaseOrderStatus.NotUploaded;

                    try
                    {
                        DateTime edtDate;
                        // if po exist then check date is valid .
                        if (!string.IsNullOrEmpty(item.Pono) && !string.IsNullOrEmpty(item.Etd))
                        {
                            if (!DateTime.TryParseExact(item.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out edtDate))
                            {
                                item.Etd = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Result = PurchaseOrderUploadResult.GivenDateFormatIsWrong;
                        return response;
                    }


                    item.IsSelected = false;
                    if (product != null)
                    {
                        item.ProductId = product.Id;
                    }
                    else
                    {
                        item.IsProductNew = true;
                    }
                    // if ponumber exist then check poid is exist and assign
                    if (!string.IsNullOrEmpty(item.Pono))
                    {
                        var purchaseOrder = purchaseOrderItems.FirstOrDefault(x => x.Pono.ToLower().Trim() == item.Pono.ToLower().Trim());
                        if (purchaseOrder != null)
                        {
                            item.PoId = purchaseOrder.Id;
                        }
                    }

                    if (!string.IsNullOrEmpty(item.Supplier))
                    {
                        var supplier = supplierDetails.FirstOrDefault(x => x.SupplierName.ToLower().Trim() == item.Supplier.ToLower().Trim());
                        if (supplier != null)
                        {
                            item.SupplierId = supplier.Id;
                        }
                    }

                    if (!string.IsNullOrEmpty(item.DestinationCountry))
                    {
                        var country = countryList.FirstOrDefault(x => x.CountryName.ToLower().Trim() == item.DestinationCountry.ToLower().Trim());
                        if (country != null)
                        {
                            item.CountryId = country.Id;
                        }
                    }
                }
            }
            response.PurchaseOrderUploadList = purchaseOrdersList.OrderBy(x => x.Pono).ThenBy(x => x.Product);
            response.Result = PurchaseOrderUploadResult.Success;
            return response;
        }

        /// <summary>
        /// Get the po product upload data
        /// </summary>
        /// <param name="requestFormValues"></param>
        /// <returns></returns>
        private PoProductUploadRequest GetPoProductUploadRequestData(Dictionary<string, string> requestFormValues)
        {
            var request = new PoProductUploadRequest();
            request.CustomerId = Convert.ToInt32(requestFormValues["customerId"]);
            request.SupplierId = Convert.ToInt32(requestFormValues["supplierId"]);
            request.BusinessLineId = Convert.ToInt32(requestFormValues["businessLineId"]);
            request.poProductRequest = JsonConvert.DeserializeObject<List<BookingPoProductDataRequest>>(requestFormValues["poProductRequest"]);
            return request;
        }

        /// <summary>
        /// Process the uploaded po product data
        /// </summary>
        /// <param name="file"></param>
        /// <param name="requestFormValues"></param>
        /// <returns></returns>
        public async Task<POProductUploadResponse> ProcessPoProductData(IFormFile file, Dictionary<string, string> requestFormValues)
        {

            #region VariableDeclaration
            var response = new POProductUploadResponse();
            PurchaseOrderUploadMap purchaseOrderUploadMap = new PurchaseOrderUploadMap();
            #endregion

            //read the file and get the purchase order rows
            var purchaseOrderExcelList = purchaseOrderUploadMap.GetPurchaseOrderDataFromFile(file);

            //get the purchase order data with empty rows
            purchaseOrderExcelList.Where(x => string.IsNullOrEmpty(x.PoNumber) && string.IsNullOrEmpty(x.ProductReference)
                                          && string.IsNullOrEmpty(x.ProductDescription) && string.IsNullOrEmpty(x.Etd)
                                              && string.IsNullOrEmpty(x.Quantity)).ToList().
                                              ForEach(x => x.ErrorData = ProductUploadErrorData.EmptyRows);

            var purchaseOrderList = purchaseOrderExcelList.Where(x => x.ErrorData != ProductUploadErrorData.EmptyRows).ToList();

            if (purchaseOrderList == null || !purchaseOrderList.Any())
            {
                response.Result = POProductUploadResult.EmptyRows;
                return response;
            }

            if (purchaseOrderList != null && purchaseOrderList.Any())
            {
                var countryDataSource = _locationRepo.GetCountryDataSource();

                var countryList = await countryDataSource.Select(x => new CountryMasterData() { Id = x.Id, Name = x.CountryName, Code = x.Alpha2Code }).ToListAsync();

                //generate the po product upload data from the form value dictionary data
                var poProductUploadRequest = GetPoProductUploadRequestData(requestFormValues);

                //validate the purchase order list data
                var poProductErrorList = await ValidatePoProductData(purchaseOrderList, countryList, poProductUploadRequest.poProductRequest, poProductUploadRequest.CustomerId, poProductUploadRequest.SupplierId, poProductUploadRequest.BusinessLineId);

                //if any error send the validation error response
                if (poProductErrorList.Any())
                {
                    response.PoProductUploadErrorList = poProductErrorList;
                    response.Result = POProductUploadResult.ValidationError;
                    return response;
                }

                if (!poProductErrorList.Any())
                {
                    //take the product name list
                    var productNameList = purchaseOrderList.Select(x => x.ProductReference?.Trim().ToLower()).Distinct().ToList();

                    //get the existing customer product list for the given product ids
                    var customerProductExistingList = await _productRepository.GetCustomerProductsByName(poProductUploadRequest.CustomerId, productNameList);

                    //get the existing product name list
                    var existingProductNameList = customerProductExistingList.Select(x => x.ProductName.Trim().ToLower()).Distinct().ToList();

                    //get the customer products to create
                    var customerProductsToCreate = purchaseOrderList.Where(x => !existingProductNameList.Contains(x.ProductReference?.Trim().ToLower())).Distinct().ToList();

                    //save the customer product data if it is not available
                    if (customerProductsToCreate.Any())
                    {
                        //save the customer product data
                        await SaveCustomerProductData(poProductUploadRequest.CustomerId, customerProductsToCreate);
                        //add newly creatd product to existing product list
                        customerProductExistingList.AddRange(await GetNewlyCreatedProductData(poProductUploadRequest.CustomerId, customerProductsToCreate));
                    }


                    //save the purchase order(po,product data)
                    var poProductUploadSuccessData = await SavePoProductUploadData(poProductUploadRequest.CustomerId, poProductUploadRequest.SupplierId, purchaseOrderList, countryList, customerProductExistingList);

                    if (poProductUploadSuccessData.Any())
                    {
                        response.PurchaseOrderSuccessList = poProductUploadSuccessData;
                        response.Result = POProductUploadResult.Success;
                    }
                }

            }
            return response;
        }

        /// <summary>
        /// Save the customer product data
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="purchaseOrderList"></param>
        /// <returns></returns>
        private async Task SaveCustomerProductData(int customerId, List<POProductUploadData> purchaseOrderList)
        {
            //map the customer product request
            var customerProducts = MapCustomerProductsRequest(customerId, purchaseOrderList);

            //save the customer product list
            await _productmanager.SaveCustomerProductList(customerProducts);

        }

        /// <summary>
        /// Get the newly created product data
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="purchaseOrderList"></param>
        /// <returns></returns>
        private async Task<List<CustomerProductDetail>> GetNewlyCreatedProductData(int customerId, List<POProductUploadData> purchaseOrderList)
        {
            //get the product name list
            var productNameList = purchaseOrderList.Select(x => x.ProductReference?.Trim().ToLower()).ToList();

            //get the customer product by name
            return await _productRepository.GetCustomerProductsByName(customerId, productNameList);
        }

        /// <summary>
        /// Save the purchase order data(po product data)
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="supplierId"></param>
        /// <param name="purchaseOrderList"></param>
        /// <param name="countryList"></param>
        /// <param name="productList"></param>
        /// <returns></returns>
        private async Task<List<PoProductUploadSuccessData>> SavePoProductUploadData(int customerId, int supplierId, List<POProductUploadData> poProductDataList, List<CountryMasterData> countryList, List<CustomerProductDetail> productList)
        {

            #region VariableDeclaration
            List<int> existingPoIds = new List<int>();
            List<CuPurchaseOrder> purchaseOrderEntities = new List<CuPurchaseOrder>();
            List<POProductUploadData> poProductListToUpdate = new List<POProductUploadData>();
            List<PoProductUploadSuccessData> poProductUploadSuccessDataList = new List<PoProductUploadSuccessData>();
            #endregion

            //get the po name list
            var poNameList = poProductDataList.Select(x => x.PoNumber?.Trim().ToLower()).
                Distinct().ToList();

            //get the existing ponumber list by customerid,supplierid,poname list
            var existingPoNumberList = await _repo.GetPurchaseOrderByPONoCustomerSupplier
                (customerId, supplierId, poNameList);

            //group the po product list by ponumber and product reference
            var groupedPoProductList = poProductDataList.GroupBy(x => new { x.PoNumber, x.ProductReference })
                           .Select(g => g.Key).ToList();

            foreach (var groupedPoProductData in groupedPoProductList)
            {
                //take the po product list by po,product reference
                var poProductList = poProductDataList.Where(x =>
                        x.PoNumber.Trim().ToLower() == groupedPoProductData.PoNumber.Trim().ToLower()
                        && x.ProductReference.Trim().ToLower() == groupedPoProductData.ProductReference.Trim().ToLower());

                if (poProductList != null && poProductList.Any())
                {
                    //take the first po product data
                    var poProductData = poProductList.FirstOrDefault();

                    if (poProductData != null)
                    {
                        var poQuantity = poProductList.Sum(x => Convert.ToInt32(x.Quantity));

                        //check po and product combination exists
                        var existingPoData = existingPoNumberList.FirstOrDefault(x =>
                                    x.PoName?.Trim().ToLower() == poProductData.PoNumber?.Trim().ToLower());

                        //if not available in the database then create it
                        if (existingPoData == null)
                        {

                            CuPurchaseOrder purchaseOrderEntity = null;

                            //if po number is not avaiable in the list then add it else take from the existing
                            if (!purchaseOrderEntities.Any(x => x.Pono.ToLower() == poProductData.PoNumber.ToLower()))
                            {
                                purchaseOrderEntity = MapAddPurchaseOrderEntity(purchaseOrderEntity, poProductData, customerId);
                            }
                            else
                                purchaseOrderEntity = purchaseOrderEntities.Where(x => x.Pono.Trim().ToLower() == poProductData.PoNumber?.Trim().ToLower()).FirstOrDefault();

                            AddPoSuppliers(new[] { supplierId }.ToList(), purchaseOrderEntity);

                            //map the purchase order detail
                            MapPurchaseOrderDetail(purchaseOrderEntity, poProductData, countryList, productList, supplierId, poQuantity);

                            purchaseOrderEntities.Add(purchaseOrderEntity);
                        }
                        //else push the poid to the list so that it can be updated completely
                        else
                        {
                            existingPoIds.Add(existingPoData.PoId);
                            poProductListToUpdate.AddRange(poProductList);
                        }
                    }
                }
            }

            _repo.SaveList(purchaseOrderEntities, false);

            if (purchaseOrderEntities.Any())
                poProductUploadSuccessDataList = MapPoProductUploadSuccessData(poProductDataList, purchaseOrderEntities, productList, countryList);

            if (existingPoIds.Any())
            {
                var poUploadSuccessData = await UpdateAndGetPODetails(customerId, supplierId, existingPoIds, poProductListToUpdate, countryList, productList);
                poProductUploadSuccessDataList.AddRange(poUploadSuccessData);
            }

            return poProductUploadSuccessDataList;

        }

        /// <summary>
        /// Update the get the po details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="supplierId"></param>
        /// <param name="existingPoIds"></param>
        /// <param name="poProductListToUpdate"></param>
        /// <param name="countryList"></param>
        /// <returns></returns>
        private async Task<List<PoProductUploadSuccessData>> UpdateAndGetPODetails(int customerId, int supplierId,
                List<int> existingPoIds, List<POProductUploadData> poProductListToUpdate, List<CountryMasterData> countryList, List<CustomerProductDetail> productList)
        {
            List<CuPurchaseOrder> purchaseOrderEntities = new List<CuPurchaseOrder>();
            List<PoProductUploadSuccessData> poProductUploadSuccessDataList = new List<PoProductUploadSuccessData>();

            if (existingPoIds.Any())
            {
                var purchaseOrders = await _repo.GetPurchaseOrderDetailsByPoIds(existingPoIds);

                //group the po product list by ponumber and product reference
                var groupedPoProductList = poProductListToUpdate.GroupBy(x => new { x.PoNumber, x.ProductReference })
                          .Select(g => g.Key).ToList();

                foreach (var groupedPoProductData in groupedPoProductList)
                {
                    //take the po product list data by po,product reference
                    var poProductList = poProductListToUpdate.Where(x =>
                        x.PoNumber.Trim().ToLower() == groupedPoProductData.PoNumber.Trim().ToLower()
                        && x.ProductReference.Trim().ToLower() == groupedPoProductData.ProductReference.Trim().ToLower());

                    if (poProductList != null && poProductList.Any())
                    {
                        //take the first po product data
                        var poProductData = poProductList.FirstOrDefault();

                        if (poProductData != null)
                        {
                            var poQuantity = poProductList.Sum(x => Convert.ToInt32(x.Quantity));

                            //get the purchase order entity
                            var purchaseOrderEntity = purchaseOrders.FirstOrDefault(x => x.Active && x.Pono.Trim().ToLower() == poProductData.PoNumber?.Trim().ToLower());

                            if (purchaseOrderEntity != null)
                            {
                                //update the purchase order entity
                                MapUpdatePurchaseOrderEntity(purchaseOrderEntity, poProductData, customerId);

                                AddPoSuppliers(new[] { supplierId }.ToList(), purchaseOrderEntity);

                                //get the purchase order detail
                                var purchaseOrderDetail = purchaseOrderEntity.CuPurchaseOrderDetails.FirstOrDefault(y => y.Active.HasValue && y.Active.Value
                                                        && y.Product != null && y.Product.ProductId.Trim().ToLower() == poProductData.ProductReference?.Trim().ToLower());

                                if (purchaseOrderDetail != null)
                                {
                                    MapUpdatePurchaseOrderDetailEntity(purchaseOrderDetail, poProductData, supplierId, countryList);
                                }
                                else
                                    //map the purchase order detail
                                    MapPurchaseOrderDetail(purchaseOrderEntity, poProductData, countryList, productList, supplierId, poQuantity);

                                purchaseOrderEntities.Add(purchaseOrderEntity);
                            }
                        }
                    }
                }

                _repo.SaveList(purchaseOrderEntities, true);


                if (purchaseOrderEntities.Any())
                {
                    poProductUploadSuccessDataList = MapPoProductUploadSuccessData(poProductListToUpdate, purchaseOrderEntities, productList, countryList);
                }

            }

            return poProductUploadSuccessDataList;
        }

        /// <summary>
        /// Map the update purchase order entity
        /// </summary>
        /// <param name="purchaseOrderEntity"></param>
        /// <param name="poProductData"></param>
        /// <param name="customerId"></param>
        private void MapUpdatePurchaseOrderEntity(CuPurchaseOrder purchaseOrderEntity, POProductUploadData poProductData, int customerId)
        {
            purchaseOrderEntity.Pono = poProductData.PoNumber;
            purchaseOrderEntity.CustomerId = customerId;
            purchaseOrderEntity.UpdatedBy = _ApplicationContext.UserId;
            purchaseOrderEntity.UpdatedOn = DateTime.Now;
        }

        /// <summary>
        /// Map the purchase order detail entity
        /// </summary>
        /// <param name="purchaseOrderDetail"></param>
        /// <param name="poProductData"></param>
        /// <param name="supplierId"></param>
        /// <param name="countryList"></param>
        private void MapUpdatePurchaseOrderDetailEntity(CuPurchaseOrderDetail purchaseOrderDetail, POProductUploadData poProductData,
                int supplierId, List<CountryMasterData> countryList)
        {
            //purchaseOrderDetail.SupplierId = supplierId;

            DateTime edtDate;
            if (DateTime.TryParseExact(poProductData.Etd, StandardDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out edtDate))
            {
                purchaseOrderDetail.Etd = edtDate;
            }

            //purchaseOrderDetail.Etd = Convert.ToDateTime(purchaseOrder.Etd);
            purchaseOrderDetail.Quantity = Convert.ToInt32(poProductData.Quantity);

            var country = countryList.FirstOrDefault(x => x.Name.ToLower().Trim() == poProductData.DestinationCountry?.ToLower().Trim()
                    || (x.Code != null && !string.IsNullOrEmpty(x.Code.Trim()) && x.Code.ToLower().Trim() == poProductData.DestinationCountry?.ToLower().Trim()));

            if (country != null)
                purchaseOrderDetail.DestinationCountryId = country.Id;

            //purchaseOrderDetail.SupplierId = supplierId;
        }

        /// <summary>
        /// Map the po product upload success data
        /// </summary>
        /// <param name="poProductDataList"></param>
        /// <param name="purchaseOrderEntities"></param>
        /// <param name="productList"></param>
        /// <param name="countryList"></param>
        /// <returns></returns>
        private List<PoProductUploadSuccessData> MapPoProductUploadSuccessData(List<POProductUploadData> poProductDataList, List<CuPurchaseOrder> purchaseOrderEntities,
            List<CustomerProductDetail> productList, List<CountryMasterData> countryList)
        {
            List<PoProductUploadSuccessData> poProductUploadSuccessList = new List<PoProductUploadSuccessData>();
            foreach (var poProductData in poProductDataList)
            {
                PoProductUploadSuccessData data = new PoProductUploadSuccessData();

                var purchaseOrderEntity = purchaseOrderEntities.Where(x => x.Pono.Trim().ToLower() == poProductData.PoNumber?.Trim().ToLower()).FirstOrDefault();

                if (purchaseOrderEntity != null)
                {
                    var poDataList = new List<CommonDataSource>() { new CommonDataSource() { Id = purchaseOrderEntity.Id, Name = poProductData.PoNumber } };
                    data.PoData = poDataList;
                }


                var product = productList.FirstOrDefault(x => x.ProductName.Trim().ToLower() == poProductData.ProductReference?.Trim().ToLower());

                //push the product data to the list
                if (product != null)
                {
                    data.ProductData = new List<CustomerProductDetail>();
                    data.ProductData.Add(product);
                }


                var country = countryList.FirstOrDefault(x => x.Name.ToLower().Trim() == poProductData.DestinationCountry?.ToLower().Trim()
                                                        || (x.Code != null && !string.IsNullOrEmpty(x.Code.Trim()) && x.Code.ToLower().Trim() == poProductData.DestinationCountry?.ToLower().Trim()));

                data.DestinationCountryId = null;
                if (country != null)
                    data.DestinationCountryId = country.Id;

                DateTime etdDate;

                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/MM/yyyy hh:mm:ss", "dd/MM/yyyy hh:mm:ss tt" };

                if (DateTime.TryParseExact(poProductData.Etd, formats, CultureInfo.InvariantCulture,
                                                                                            DateTimeStyles.None, out etdDate))
                {
                    data.Etd = new DateObject() { Year = etdDate.Year, Month = etdDate.Month, Day = etdDate.Day };
                }

                data.ProductDescription = poProductData.ProductDescription;
                data.Quantity = Convert.ToInt32(poProductData.Quantity);
                data.ColorCode = poProductData.ColorCode;
                data.ColorName = poProductData.ColorName;

                poProductUploadSuccessList.Add(data);
            }

            return poProductUploadSuccessList;
        }

        /// <summary>
        /// Map the purchase order entity
        /// </summary>
        /// <param name="purchaseOrderEntity"></param>
        /// <param name="purchaseOrder"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private CuPurchaseOrder MapAddPurchaseOrderEntity(CuPurchaseOrder purchaseOrderEntity, POProductUploadData purchaseOrder, int customerId)
        {
            purchaseOrderEntity = new CuPurchaseOrder();
            purchaseOrderEntity.Pono = purchaseOrder.PoNumber.Trim();
            purchaseOrderEntity.CustomerId = customerId;
            purchaseOrderEntity.Active = true;
            purchaseOrderEntity.CreatedBy = _ApplicationContext.UserId;
            purchaseOrderEntity.CreatedOn = DateTime.Now;
            purchaseOrderEntity.EntityId = _filterService.GetCompanyId();
            return purchaseOrderEntity;
        }

        /// <summary>
        /// Map the purchase order detail
        /// </summary>
        /// <param name="purchaseOrderEntity"></param>
        /// <param name="poProductUploadData"></param>
        /// <param name="countryList"></param>
        /// <param name="productList"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        private CuPurchaseOrderDetail MapPurchaseOrderDetail(CuPurchaseOrder purchaseOrderEntity, POProductUploadData poProductUploadData,
                                    List<CountryMasterData> countryList, List<CustomerProductDetail> productList, int supplierId, int poQuantity)
        {
            CuPurchaseOrderDetail purchaseOrderDetail = new CuPurchaseOrderDetail();

            //purchaseOrderDetail.SupplierId = supplierId;

            DateTime edtDate;

            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/MM/yyyy hh:mm:ss", "dd/MM/yyyy hh:mm:ss tt" };

            if (DateTime.TryParseExact(poProductUploadData.Etd, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out edtDate))
            {
                purchaseOrderDetail.Etd = edtDate;
            }

            purchaseOrderDetail.Quantity = poQuantity;

            var country = countryList.FirstOrDefault(x => x.Name.ToLower().Trim() == poProductUploadData.DestinationCountry?.ToLower().Trim()
                || (x.Code != null && !string.IsNullOrEmpty(x.Code.Trim()) && x.Code.ToLower().Trim() == poProductUploadData.DestinationCountry?.ToLower().Trim()));

            if (country != null)
                purchaseOrderDetail.DestinationCountryId = country.Id;

            var product = productList.FirstOrDefault(x => x.ProductName.ToLower() == poProductUploadData.ProductReference?.Trim().ToLower());

            if (product != null)
                purchaseOrderDetail.ProductId = product.Id;

            //purchaseOrderDetail.SupplierId = supplierId;
            purchaseOrderDetail.CreatedBy = _ApplicationContext.UserId;
            purchaseOrderDetail.CreatedTime = DateTime.Now;
            purchaseOrderDetail.Active = true;
            purchaseOrderDetail.EntityId = _filterService.GetCompanyId();

            purchaseOrderEntity.CuPurchaseOrderDetails.Add(purchaseOrderDetail);



            _repo.AddEntity(purchaseOrderDetail);

            return purchaseOrderDetail;
        }

        /// <summary>
        /// Map the customer products request
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="poProductUploadList"></param>
        /// <returns></returns>
        private List<CustomerProduct> MapCustomerProductsRequest(int customerId, List<POProductUploadData> poProductUploadList)
        {
            List<CustomerProduct> customerProducts = new List<CustomerProduct>();

            foreach (var poProductUploadData in poProductUploadList)
            {
                if (!customerProducts.Any(x => x.ProductId?.ToLower().Trim() == poProductUploadData.ProductReference?.ToLower().Trim()))
                {
                    var customerProduct = new CustomerProduct();
                    customerProduct.CustomerId = customerId;
                    customerProduct.ProductId = poProductUploadData.ProductReference;
                    customerProduct.ProductDescription = poProductUploadData.ProductDescription;
                    customerProduct.isNewProduct = true;

                    customerProduct.Barcode = poProductUploadData.Barcode?.Trim();
                    customerProduct.FactoryReference = poProductUploadData.FactoryReference?.Trim();

                    customerProduct.ApiServiceIds = new List<int>();
                    customerProduct.ApiServiceIds.Add((int)APIServiceEnum.Inspection);
                    customerProducts.Add(customerProduct);
                }
            }

            return customerProducts;
        }

        /// <summary>
        /// Validate the po product data
        /// </summary>
        /// <param name="purchaseOrderList"></param>
        /// <param name="countryList"></param>
        /// <param name="bookingPoProductList"></param>
        /// <param name="customerId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        private async Task<List<POProductUploadData>> ValidatePoProductData(List<POProductUploadData> purchaseOrderList,
                List<CountryMasterData> countryList, List<BookingPoProductDataRequest> bookingPoProductList,
                int customerId, int supplierId, int businessLineId)
        {
            List<POProductUploadData> poProductUploadErrorData = new List<POProductUploadData>();

            purchaseOrderList.ForEach(x => x.ErrorData = ProductUploadErrorData.NonErrorData);

            //check po number is empty or null
            var poNumberEmptyOrNullList = purchaseOrderList.Where(x => string.IsNullOrEmpty(x.PoNumber?.Trim())).ToList();

            if (poNumberEmptyOrNullList.Any())
            {
                poNumberEmptyOrNullList.ForEach(x => x.ErrorData = ProductUploadErrorData.PoNoMandatory);
                poProductUploadErrorData.AddRange(poNumberEmptyOrNullList);
            }

            //check product reference is empty or null
            var productReferenceEmptyOrNullList = purchaseOrderList.Where(x => string.IsNullOrEmpty(x.ProductReference?.Trim())).ToList();

            if (productReferenceEmptyOrNullList.Any())
            {
                productReferenceEmptyOrNullList.ForEach(x => x.ErrorData = ProductUploadErrorData.ProductRefMandatory);
                poProductUploadErrorData.AddRange(productReferenceEmptyOrNullList);
            }

            //check product reference is empty or null
            var productDescriptionEmptyOrNullList = purchaseOrderList.Where(x => string.IsNullOrEmpty(x.ProductDescription?.Trim())).ToList();

            if (productDescriptionEmptyOrNullList.Any())
            {
                productDescriptionEmptyOrNullList.ForEach(x => x.ErrorData = ProductUploadErrorData.ProductRefDescMandatory);
                poProductUploadErrorData.AddRange(productDescriptionEmptyOrNullList);
            }

            //check the quantity is mandatory
            var quantityMandatoryList = purchaseOrderList.Where(x => string.IsNullOrEmpty(x.Quantity?.Trim())).ToList();

            if (quantityMandatoryList.Any())
            {
                quantityMandatoryList.ForEach(x => x.ErrorData = ProductUploadErrorData.QuantityMandatory);
                poProductUploadErrorData.AddRange(quantityMandatoryList);
            }

            if (businessLineId == (int)BusinessLine.SoftLine)
            {
                //check the color code is mandatory
                var colorCodeMandatoryList = purchaseOrderList.Where(x => string.IsNullOrEmpty(x.ColorCode?.Trim())).ToList();

                if (colorCodeMandatoryList.Any())
                {
                    colorCodeMandatoryList.ForEach(x => x.ErrorData = ProductUploadErrorData.ColorCodeMandatory);
                    poProductUploadErrorData.AddRange(colorCodeMandatoryList);
                }

                //check the color name is mandatory
                var colorNameMandatoryList = purchaseOrderList.Where(x => string.IsNullOrEmpty(x.ColorName?.Trim())).ToList();

                if (colorNameMandatoryList.Any())
                {
                    colorNameMandatoryList.ForEach(x => x.ErrorData = ProductUploadErrorData.ColorNameMandatory);
                    poProductUploadErrorData.AddRange(colorNameMandatoryList);
                }
            }


            //take the list with quantity available data
            var quantityAvailableList = purchaseOrderList.Where(x => !string.IsNullOrEmpty(x.Quantity?.Trim())).ToList();

            if (quantityAvailableList.Any())
            {
                //check the invalid quantity data
                int quantity;
                var invalidQuantityList = quantityAvailableList.Where(x => !int.TryParse(x.Quantity, out quantity)).ToList();
                if (invalidQuantityList.Any())
                {
                    invalidQuantityList.ForEach(x => x.ErrorData = ProductUploadErrorData.InvalidQuantityData);
                    poProductUploadErrorData.AddRange(invalidQuantityList);
                }

                var validQuantityList = quantityAvailableList.Where(x => int.TryParse(x.Quantity, out quantity)).ToList();
                if (validQuantityList.Any())
                {
                    var QuantityListWithZero = validQuantityList.Where(x => Convert.ToInt32(x.Quantity) <= 0).ToList();
                    if (QuantityListWithZero.Any())
                    {
                        QuantityListWithZero.ForEach(x => x.ErrorData = ProductUploadErrorData.QuantityShouldBeZero);
                        poProductUploadErrorData.AddRange(QuantityListWithZero);
                    }
                }

            }

            //check the barcode exceeded limit
            var barcodeCharExceededLimitList = purchaseOrderList.Where(x => !string.IsNullOrEmpty(x.Barcode?.Trim())
                        && x.Barcode?.Trim().Count() > BarCodeCharactersExceeded).ToList();

            if (barcodeCharExceededLimitList.Any())
            {

                barcodeCharExceededLimitList.ForEach(x => x.ErrorData = ProductUploadErrorData.BarcodecharacterLimitExceeded);
                poProductUploadErrorData.AddRange(barcodeCharExceededLimitList);
            }

            var etdAvailableList = purchaseOrderList.Where(x => !string.IsNullOrEmpty(x.Etd?.Trim())).ToList();

            if (etdAvailableList.Any())
            {
                //check etd date is invalid
                DateTime edtDate;

                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/MM/yyyy hh:mm:ss", "dd/MM/yyyy hh:mm:ss tt" };

                var invalidDateFormatList = etdAvailableList.Where(x => !DateTime.TryParseExact(x.Etd, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out edtDate)).ToList();

                if (invalidDateFormatList.Any())
                {
                    invalidDateFormatList.ForEach(x => x.ErrorData = ProductUploadErrorData.EtdNotValidDateFormat);
                    poProductUploadErrorData.AddRange(invalidDateFormatList);
                }
            }

            var poIds = bookingPoProductList.Select(x => x.PoId).ToList();
            var productIds = bookingPoProductList.Select(x => x.ProductId).ToList();

            var bookingExistingProductList = await _repo.GetPurchaseOrdersByPoProduct(customerId, poIds, productIds);

            if (bookingExistingProductList.Any())
            {
                var existingBookingPoProductList = purchaseOrderList.Where(x => bookingExistingProductList.Any(y => y.PoName.Trim().ToLower() == x.PoNumber?.Trim().ToLower()
                              && y.ProductName.Trim().ToLower() == x.ProductReference?.Trim().ToLower())).ToList();
                if (existingBookingPoProductList.Any())
                {
                    existingBookingPoProductList.ForEach(x => x.ErrorData = ProductUploadErrorData.PoProductAlreadyExists);
                    poProductUploadErrorData.AddRange(existingBookingPoProductList);
                }
            }

            var nonErrorList = purchaseOrderList.Where(x => x.ErrorData == ProductUploadErrorData.NonErrorData);

            if (nonErrorList.Any())
            {

                if (businessLineId == (int)BusinessLine.HardLine)
                {
                    var duplicatePoProductList = nonErrorList.GroupBy(x => new { x.PoNumber, x.ProductReference })
                           .Where(g => g.Count() > 1)
                           .Select(g => g.Key).ToList();

                    if (duplicatePoProductList.Any())
                    {
                        var duplicateProductList = nonErrorList.Where(x => duplicatePoProductList.Any(y => y.PoNumber?.Trim().ToLower() == x.PoNumber?.Trim().ToLower()
                                            && y.ProductReference?.Trim().ToLower() == x.ProductReference?.Trim().ToLower())).ToList();
                        duplicateProductList.ForEach(x => x.ErrorData = ProductUploadErrorData.PoProductDuplicate);
                        poProductUploadErrorData.AddRange(duplicateProductList);
                    }
                }
                else if (businessLineId == (int)BusinessLine.SoftLine)
                {
                    var duplicatePoProductList = nonErrorList.GroupBy(x => new { x.PoNumber, x.ProductReference, x.ColorCode, x.ColorName })
                           .Where(g => g.Count() > 1)
                           .Select(g => g.Key).ToList();

                    if (duplicatePoProductList.Any())
                    {
                        var duplicateProductList = nonErrorList.Where(x => duplicatePoProductList.Any(y => y.PoNumber?.Trim().ToLower() == x.PoNumber?.Trim().ToLower()
                                            && y.ProductReference?.Trim().ToLower() == x.ProductReference?.Trim().ToLower()
                                            && y.ColorCode?.Trim().ToLower() == x.ColorCode?.Trim().ToLower()
                                            && y.ColorName?.Trim().ToLower() == x.ColorName?.Trim().ToLower())).ToList();

                        duplicateProductList.ForEach(x => x.ErrorData = ProductUploadErrorData.PoProductDuplicate);
                        poProductUploadErrorData.AddRange(duplicateProductList);
                    }
                }


            }

            return poProductUploadErrorData;
        }

        /// <summary>
        /// Get the po search list with the auto complete option
        /// </summary>
        /// <param name="poName"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<PoListResponse> GetPoListByNameAndCustomer(string poName, int customerId)
        {
            var response = new PoListResponse();
            if (poName != null && poName.Length >= (int)AutoCompleteTextLimit.BookingPoLimit)
            {
                var data = await _repo.GetPoDetailByNameAndCustomer(poName, customerId);
                if (data != null && data.Any())
                {
                    response.Result = PoListResult.Success;
                    response.PoDataSource = data;
                }
                else
                {
                    response.Result = PoListResult.NotFound;
                }
            }
            return response;
        }

        /// <summary>
        /// Get the product list belongs to po
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<POProductListResponse> GetPOProductList(POProductDataSourceRequest request)
        {

            try
            {
                var response = new POProductListResponse();

                //Get the purchase order details Iqueryable
                var data = _repo.GetPurchaseOrderDetails();

                //apply the po number search text filter
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => EF.Functions.Like(x.Product.ProductId, $"%{request.SearchText.Trim()}%"));
                }

                //apply selected po id filter
                if (request.PoId > 0)
                {
                    data = data.Where(x => x.PoId == request.PoId);
                }

                if (request.SupplierId > 0)
                {
                    data = data.Where(x => x.Po.CuPoSuppliers.Any(y => y.Active.Value && y.SupplierId == request.SupplierId));
                }

                //execute the product list
                var productList = await data.Skip(request.Skip).Take(request.Take).
                                        Select(x => new POProductList()
                                        {
                                            Id = x.Product.Id,
                                            Name = x.Product.ProductId,
                                            Description = x.Product.ProductDescription,
                                            PoQuantity = x.Quantity,
                                            ProductCategoryId = x.Product.ProductCategory,
                                            ProductCategoryName = x.Product.ProductCategoryNavigation.Name,
                                            ProductSubCategoryId = x.Product.ProductSubCategory,
                                            ProductSubCategoryName = x.Product.ProductSubCategoryNavigation.Name,
                                            ProductSubCategory2Id = x.Product.ProductCategorySub2,
                                            ProductSubCategory2Name = x.Product.ProductCategorySub2Navigation.Name,
                                            ProductSubCategory3Id = x.Product.ProductCategorySub3,
                                            ProductSubCategory3Name = x.Product.ProductCategorySub3Navigation.Name,
                                            BarCode = x.Product.Barcode,
                                            FactoryReference = x.Product.FactoryReference,
                                            IsNewProduct = x.Product.IsNewProduct,
                                            Remarks = x.Product.Remarks,
                                            ProductImageCount = x.Product.CuProductFileAttachments.Where(x => x.Active).Select(x => x.Id).Count()

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

        /// <summary>
        /// Get the po list data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PoListResponse> GetPODataSource(PODataSourceRequest request)
        {
            try
            {
                var response = new PoListResponse();
                //Get the purchase order Iqueryable
                var data = _repo.GetPurchaseOrder();

                //apply the po number search text
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => EF.Functions.Like(x.Pono.ToLower().Trim(), $"%{request.SearchText.ToLower().Trim()}%"));
                }

                //filter the selected buyer ids
                if (request.CustomerId > 0)
                {
                    data = data.Where(x => x.CustomerId == request.CustomerId);
                }
                //apply the supplier id filter
                if (request.SupplierId > 0)
                {
                    data = data.Where(x => x.CuPoSuppliers.Any(y => y.Active.Value && y.SupplierId == request.SupplierId));
                }
                //apply the po id filter
                if (request.PoId > 0)
                {
                    data = data.Where(x => x.CuPurchaseOrderDetails.Any(y => y.PoId == request.PoId));
                }
                //execute the po list 
                var poList = await data.Skip(request.Skip).Take(request.Take).
                                        Select(x => new PoDataSource()
                                        {
                                            Id = x.Id,
                                            Name = x.Pono
                                        }).AsNoTracking().ToListAsync();

                if (poList == null || !poList.Any())
                    response.Result = PoListResult.NotFound;

                else
                {
                    response.PoDataSource = poList;
                    response.Result = PoListResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the po product list data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<POProductDataResponse> GetPOProductListData(POProductDataRequest request)
        {

            List<InspectionProductSubCategory> productSubCategoryList = new List<InspectionProductSubCategory>();

            List<InspectionProductSubCategory2> productSubCategory2List = new List<InspectionProductSubCategory2>();

            List<InspectionProductSubCategory3> productSubCategory3List = new List<InspectionProductSubCategory3>();

            try
            {
                var response = new POProductDataResponse();

                //Get the purchase order details Iqueryable
                var data = _repo.GetPurchaseOrderDetails();

                //apply selected po id filter
                if (request.PoId > 0)
                {
                    data = data.Where(x => x.PoId == request.PoId);
                }

                if (request.SupplierId > 0)
                {
                    data = data.Where(x => x.Po.CuPoSuppliers.Any(y => y.Active.Value && y.SupplierId == request.SupplierId));
                }

                //execute the product list
                var poProductRelatedDataList = await data.
                                        Select(x => new POProductRelatedData()
                                        {
                                            Id = x.Product.Id,
                                            Name = x.Product.ProductId,
                                            Description = x.Product.ProductDescription,
                                            PoQuantity = x.Quantity,
                                            ProductCategoryId = x.Product.ProductCategory,
                                            ProductCategoryName = x.Product.ProductCategoryNavigation.Name,
                                            ProductSubCategoryId = x.Product.ProductSubCategory,
                                            ProductSubCategoryName = x.Product.ProductSubCategoryNavigation.Name,
                                            ProductSubCategory2Id = x.Product.ProductCategorySub2,
                                            ProductSubCategory2Name = x.Product.ProductCategorySub2Navigation.Name,
                                            ProductSubCategory3Id = x.Product.ProductCategorySub3,
                                            ProductSubCategory3Name = x.Product.ProductCategorySub3Navigation.Name,
                                            BarCode = x.Product.Barcode,
                                            FactoryReference = x.Product.FactoryReference,
                                            IsNewProduct = x.Product.IsNewProduct,
                                            Etd = x.Etd,
                                            DestinationCountryId = x.DestinationCountryId,
                                            DestinationCountryName = x.DestinationCountry.CountryName,
                                            PoId = x.PoId,
                                            PoName = x.Po.Pono,
                                            Remarks = x.Product.Remarks
                                        }).AsNoTracking().ToListAsync();



                if (poProductRelatedDataList == null || !poProductRelatedDataList.Any())
                    response.Result = POProductDataResult.NotFound;
                else
                {
                    //take the product category ids if product subcategory is null for the product
                    var productCategoryIds = poProductRelatedDataList.Where(x => x.ProductSubCategoryId == null
                                            && x.ProductCategoryId != null).Select(x => x.ProductCategoryId).ToList();
                    //take the product sub category list
                    if (productCategoryIds != null && productCategoryIds.Any())
                        productSubCategoryList = await _bookingRepo.GetProductSubCategoryList(productCategoryIds);

                    //take the product sub category ids if product subcategory2 is null for the product
                    var productSubCategoryIds = poProductRelatedDataList.Where(x => x.ProductSubCategory2Id == null
                                                && x.ProductSubCategoryId != null).Select(x => x.ProductSubCategoryId).ToList();
                    //take the product sub category2 list
                    if (productSubCategoryIds != null && productSubCategoryIds.Any())
                        productSubCategory2List = await _bookingRepo.GetProductSubCategory2List(productSubCategoryIds);
                    //take the product sub category ids if product subcategory2 is null for the product

                    var productSubCategory2Ids = poProductRelatedDataList.Where(x => x.ProductSubCategory3Id == null
                                                && x.ProductSubCategory2Id != null).Select(x => x.ProductSubCategory2Id.GetValueOrDefault()).ToList();
                    //take the product sub category2 list
                    if (productSubCategory2Ids != null && productSubCategory2Ids.Any())
                        productSubCategory3List = await _bookingRepo.GetProductSubCategory3List(productSubCategory2Ids);

                    var productList = PurchaseOrderMapData.MapPoProductDataList(poProductRelatedDataList, productSubCategoryList,
                                                productSubCategory2List, productSubCategory3List);

                    response.ProductList = productList.OrderBy(x => x.Name).ToList();
                    response.Result = POProductDataResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                return null;
            }
        }
        private CuProduct MapCustomerProduct(CustomerPurchaseOrderDetails request, int customerId)
        {
            var product = new CuProduct()
            {
                CreatedBy = _ApplicationContext.UserId,
                EntityId = _filterService.GetCompanyId(),
                CreatedTime = DateTime.Now,
                ProductId = request.ProductRef.Trim(),
                ProductDescription = request.ProductRefDesc.RemoveExtraSpace(),
                Active = true,
                Barcode = request.BarCode,
                CustomerId = customerId,
                FactoryReference = request.FactoryRef
            };
            product.CuProductApiServices = new HashSet<CuProductApiService>()
            {
                new CuProductApiService{ServiceId= (int)Service.InspectionId,Active=true,
                    CreatedBy=_ApplicationContext.UserId,CreatedOn=DateTime.Now}
            };
            return product;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poId"></param>
        /// <returns></returns>
        public async Task<PoBookingDetailsResponse> GetPOBookingDetails(int poId)
        {
            var response = new PoBookingDetailsResponse();

            var poBookingList = await _repo.GetPOBookingDetailsList(poId);

            if (poBookingList.Any())
            {
                foreach (var poBooking in poBookingList)
                {
                    poBooking.ServiceDateTo = poBooking.ServiceToDate.ToString(StandardDateFormat);
                    poBooking.ServiceDateFrom = poBooking.ServiceFromDate.ToString(StandardDateFormat);
                    poBooking.StatusColor = BookingSummaryInspectionStatusColor.GetValueOrDefault(poBooking.StatusId, "");
                }
                response.PoBookingDetails = poBookingList;
                response.Result = PoListResult.Success;
            }
            else
            {
                response.Result = PoListResult.NotFound;
            }
            return response;
        }

        /// <summary>
        /// Check po is already mapped to any booking
        /// </summary>
        /// <param name="poid"></param>
        /// <returns></returns>
        public async Task<bool> CheckPurchaseOrderExistInBooking(int poid)
        {
            return await _repo.CheckPurchaseOrderExistInBooking(poid);
        }
    }
}