using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Schedule;
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
    public class QCBlockManager : IQCBlockManager
    {
        readonly IQCBlockRepository _qcBlockrepo = null;
        readonly IAPIUserContext _applicationContext = null;
        readonly IInspectionBookingRepository _bookingRepo = null;
        readonly IHumanResourceManager _humanResourceManager = null;
        private ITenantProvider _filterService = null;
        private readonly QCBlockMap QCBlockMap = null;

        public QCBlockManager(IQCBlockRepository qcBlockrepo, IAPIUserContext applicationContext,
            IInspectionBookingRepository bookingRepo,
            IHumanResourceManager humanResourceManager, ITenantProvider filterService)
        {
            _qcBlockrepo = qcBlockrepo;
            _applicationContext = applicationContext;
            _bookingRepo = bookingRepo;
            _humanResourceManager = humanResourceManager;
            QCBlockMap = new QCBlockMap();
            _filterService = filterService;
        }

        /// <summary>
        /// QC Block already exists
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> IsQCBlockExists(QCBlockRequest request)
        {
            bool IsRecordExists = false;

            var QCBlockList = _qcBlockrepo.GetQCBlockRelatedIdList();

            //apply get particular records
            QCBlockList = QCBlockList.Where(x => x.Qcid == request.QCId && x.Id != request.Id);

            if (QCBlockList.Any())
            {
                //apply where customer Id list available or not from request
                if (request.CustomerIds != null && request.CustomerIds.Any())
                {
                    QCBlockList = QCBlockList.Where(x => x.QcBlCustomers.Any(y => request.CustomerIds.Contains(y.CustomerId.GetValueOrDefault())));
                }

                //apply where supplier Id list available or not from request
                if (request.SupplierIds != null && request.SupplierIds.Any())
                {
                    QCBlockList = QCBlockList.Where(x => x.QcBlSupplierFactories.Any(y => y.TypeId == (int)Supplier_Type.Supplier_Agent && request.SupplierIds.Contains(y.SupplierFactoryId.GetValueOrDefault())));
                }

                //apply where factory Id list available or not from request
                if (request.FactoryIds != null && request.FactoryIds.Any())
                {
                    QCBlockList = QCBlockList.Where(x => x.QcBlSupplierFactories.Any(y => y.TypeId == (int)Supplier_Type.Factory && request.FactoryIds.Contains(y.SupplierFactoryId.GetValueOrDefault())));
                }

                //apply where  product category Id list available or not from request
                if (request.ProductCategoryIds.Any())
                {
                    QCBlockList = QCBlockList.Where(x => x.QcBlProductCatgeories.Any(y => request.ProductCategoryIds.Contains(y.ProductCategoryId.GetValueOrDefault())));
                }

                //apply where  product category sub Id list available or not from request
                if (request.ProductCategorySubIds != null && request.ProductCategorySubIds.Any())
                {
                    QCBlockList = QCBlockList.Where(x => x.QcBlProductSubCategories.Any(y => request.ProductCategorySubIds.Contains(y.ProductSubCategoryId.GetValueOrDefault())));
                }

                //apply where  product category sub 2 Id list available or not from request
                if (request.ProductCategorySub2Ids != null && request.ProductCategorySub2Ids.Any())
                {
                    QCBlockList = QCBlockList.Where(x => x.QcBlProductSubCategory2S.Any(y => request.ProductCategorySub2Ids.Contains(y.ProductSubCategory2Id.GetValueOrDefault())));
                }

                if (QCBlockList.Any())
                {
                    IsRecordExists = true;
                }
            }

            return IsRecordExists;
        }

        /// <summary>
        /// save the qc details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveQCBlockResponse> Save(QCBlockRequest request)
        {
            if (request == null)
            {
                return new SaveQCBlockResponse() { Result = QCBlockResponseResult.RequestNotCorrectFormat };
            }

            //same request if exists in DB
            if (await IsQCBlockExists(request))
            {
                return new SaveQCBlockResponse() { Result = QCBlockResponseResult.IsExists };
            }
            else
            {
                if (request.Id > 0)
                {
                    //update
                    return await UpdateQCBlockList(request);
                }
                else
                {
                    //save
                    return await AddQCBlockList(request);
                }
            }
        }

        /// <summary>
        /// add qc block details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<SaveQCBlockResponse> AddQCBlockList(QCBlockRequest request)
        {
            //map qc block data
            QcBlockList qcblockEntity = new QcBlockList()
            {
                Active = true,
                CreatedBy = _applicationContext.UserId,
                CreatedOn = DateTime.Now,
                Qcid = request.QCId,
                EntityId= _filterService.GetCompanyId()
            };

            _qcBlockrepo.AddEntity(qcblockEntity);

            //adding the vlaues to child tables
            AddQCChildEntity(request, qcblockEntity);

            await _qcBlockrepo.Save();

            return new SaveQCBlockResponse() { Id = qcblockEntity.Id, Result = QCBlockResponseResult.Success };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private void AddQCChildEntity(QCBlockRequest request, QcBlockList qcblockEntity)
        {
            //product category has data, adding to entity
            if (request.ProductCategoryIds != null && request.ProductCategoryIds.Any())
            {
                foreach (var ProductCategoryId in request.ProductCategoryIds)
                {
                    QcBlProductCatgeory qcBlProductCatgeory = new QcBlProductCatgeory()
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = _applicationContext.UserId,
                        ProductCategoryId = ProductCategoryId
                    };
                    qcblockEntity.QcBlProductCatgeories.Add(qcBlProductCatgeory);
                    _qcBlockrepo.AddEntity(qcBlProductCatgeory);
                }
            }

            //customer has data, adding to entity
            if (request.CustomerIds != null && request.CustomerIds.Any())
            {
                foreach (var customerId in request.CustomerIds)
                {

                    QcBlCustomer qcBlCustomer = new QcBlCustomer()
                    {
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        CustomerId = customerId
                    };
                    qcblockEntity.QcBlCustomers.Add(qcBlCustomer);
                    _qcBlockrepo.AddEntity(qcBlCustomer);
                }
            }

            //factory has data, adding to entity
            if (request.FactoryIds != null && request.FactoryIds.Any())
            {
                foreach (var factoryId in request.FactoryIds)
                {
                    QcBlSupplierFactory qcBlFactory = new QcBlSupplierFactory()
                    {
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        SupplierFactoryId = factoryId,
                        TypeId = (int)Supplier_Type.Factory
                    };

                    qcblockEntity.QcBlSupplierFactories.Add(qcBlFactory);
                    _qcBlockrepo.AddEntity(qcBlFactory);
                }
            }

            //supplier has data, adding to entity
            if (request.SupplierIds != null && request.SupplierIds.Any())
            {
                foreach (var SupplierId in request.SupplierIds)
                {
                    QcBlSupplierFactory qcBlSupplier = new QcBlSupplierFactory()
                    {
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        SupplierFactoryId = SupplierId,
                        TypeId = (int)Supplier_Type.Supplier_Agent
                    };

                    qcblockEntity.QcBlSupplierFactories.Add(qcBlSupplier);
                    _qcBlockrepo.AddEntity(qcBlSupplier);
                }
            }

            //product sub category has data, adding to entity
            if (request.ProductCategorySubIds != null && request.ProductCategorySubIds.Any())
            {
                foreach (var ProductCategorySubId in request.ProductCategorySubIds)
                {
                    QcBlProductSubCategory qcBlProductSubCategory = new QcBlProductSubCategory()
                    {
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        ProductSubCategoryId = ProductCategorySubId
                    };

                    qcblockEntity.QcBlProductSubCategories.Add(qcBlProductSubCategory);
                    _qcBlockrepo.AddEntity(qcBlProductSubCategory);
                }
            }

            //product sub category 2 has data, adding to entity
            if (request.ProductCategorySub2Ids != null && request.ProductCategorySub2Ids.Any())
            {
                foreach (var ProductCategorySub2Ids in request.ProductCategorySub2Ids)
                {
                    QcBlProductSubCategory2 qcBlProductSubCategory2 = new QcBlProductSubCategory2()
                    {
                        CreatedBy = _applicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        ProductSubCategory2Id = ProductCategorySub2Ids
                    };

                    qcblockEntity.QcBlProductSubCategory2S.Add(qcBlProductSubCategory2);
                    _qcBlockrepo.AddEntity(qcBlProductSubCategory2);
                }
            }
        }

        /// <summary>
        /// update the qc block details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<SaveQCBlockResponse> UpdateQCBlockList(QCBlockRequest request)
        {
            var qcBlockDetails = await _qcBlockrepo.GetQCDetails(request.Id);

            //getQCBlockDetails.Active = false;
            //getQCBlockDetails.DeletedBy = _applicationContext.UserId;
            //getQCBlockDetails.DeletedOn = DateTime.Now;

            _qcBlockrepo.RemoveEntities(qcBlockDetails.QcBlCustomers);
            _qcBlockrepo.RemoveEntities(qcBlockDetails.QcBlProductCatgeories);
            _qcBlockrepo.RemoveEntities(qcBlockDetails.QcBlProductSubCategories);
            _qcBlockrepo.RemoveEntities(qcBlockDetails.QcBlProductSubCategory2S);
            _qcBlockrepo.RemoveEntities(qcBlockDetails.QcBlSupplierFactories);

            AddQCChildEntity(request, qcBlockDetails);

            await _qcBlockrepo.Save();

            return new SaveQCBlockResponse() { Id = qcBlockDetails.Id, Result = QCBlockResponseResult.Success };
        }

        /// <summary>
        /// edit the qc block details
        /// </summary>
        /// <param name="qcBlockId"></param>
        /// <returns></returns>
        public async Task<EditQCBlockResponse> Edit(int qcBlockId)
        {
            var getQCBlockDetails = await _qcBlockrepo.GetQCDetails(qcBlockId);

            if (getQCBlockDetails != null)
            {
                return new EditQCBlockResponse() { QCBlockDetails = QCBlockMap.MapQCBlockEdit(getQCBlockDetails), Result = QCBlockResponseResult.Success };
            }
            else
            {
                return new EditQCBlockResponse() { Result = QCBlockResponseResult.NoDataFound };
            }
        }

        /// <summary>
        /// get qc block summary data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QCBlockSummaryResponse> Search(QCBlockSummaryRequest request)
        {
            if (request == null)
                return new QCBlockSummaryResponse() { Result = QCBlockResponseResult.RequestNotCorrectFormat };

            var response = new QCBlockSummaryResponse { Index = request.Index.Value, PageSize = request.PageSize.Value };

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var data = _qcBlockrepo.GetQCBlockSummaryDetails();

            if (data == null)
                return new QCBlockSummaryResponse() { Result = QCBlockResponseResult.NoDataFound };

            data = await FilterSummaryData(data, request);

            data = data.Skip(skip).Take(take);

            var qcblockid = data.Select(x => x.QCBlockId);
            //execute the data from db
            var result = await data.ToListAsync();

            var qcsummarydetails = new QcblockSummaryDetailsRepo();
            qcsummarydetails.CustomerNameList = await _qcBlockrepo.GetQcBlockCustomers(qcblockid);
            qcsummarydetails .SupplierList= await _qcBlockrepo.GetQcBlockSuppliers(qcblockid);
            qcsummarydetails.FactoryList = await _qcBlockrepo.GetQcBlockFactories(qcblockid);
            qcsummarydetails.ProductCategoryList = await _qcBlockrepo.GetQcBlockProductCategory(qcblockid);
            qcsummarydetails.ProductCateogrySubList = await _qcBlockrepo.GetQcBlockProductSubCategory(qcblockid);
            qcsummarydetails.ProductCategorySub2List = await _qcBlockrepo.GetQcBlockProductSub2Category(qcblockid);

            if (result == null || !result.Any())
                return new QCBlockSummaryResponse() { Result = QCBlockResponseResult.NoDataFound };

            var res = result.Select(x => QCBlockMap.MapQCSummaryData(x, qcsummarydetails)).ToList();

            return new QCBlockSummaryResponse
            {
                Result = QCBlockResponseResult.Success,
                TotalCount = await data.CountAsync(),
                Index = request.Index.Value,
                PageSize = request.PageSize.Value,
                PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0),
                Data = res
            };
        }

        /// <summary>
        /// filter the data by request
        /// </summary>
        /// <param name="data"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<IQueryable<QCBlockSummaryRepo>> FilterSummaryData(IQueryable<QCBlockSummaryRepo> data, QCBlockSummaryRequest request)
        {
            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 20;

            //get qc list under UNDER Office 
            if (request.OfficeIds != null && request.OfficeIds.Any())
            {
                //get qcids
                IEnumerable<int> QCIds = await _humanResourceManager.GetStaffIdsByProfileAndLocation(request.OfficeIds);

                data = data.Where(x => QCIds.Contains(x.QCId));
            }

            if (request.QCIds != null && request.QCIds.Any())
            {
                data = data.Where(x => request.QCIds.Contains(x.QCId));
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<QCBlockSummaryItem>> ExportQCBlockSummary(QCBlockSummaryRequest requestDto)
        {
            int skip = (requestDto.Index.Value - 1) * requestDto.PageSize.Value;

            int take = requestDto.PageSize.Value;

            var data = _qcBlockrepo.GetQCBlockSummaryDetails();

            if (data == null)
                return null;

            data = await FilterSummaryData(data, requestDto);

            data = data.Skip(skip).Take(take);

            var qcblockid = data.Select(x => x.QCBlockId);
            //execute the data
            var result = await data.ToListAsync();

            var qcsummarydetails = new QcblockSummaryDetailsRepo();
            qcsummarydetails.CustomerNameList = await _qcBlockrepo.GetQcBlockCustomers(qcblockid);
            qcsummarydetails.SupplierList = await _qcBlockrepo.GetQcBlockSuppliers(qcblockid);
            qcsummarydetails.FactoryList = await _qcBlockrepo.GetQcBlockFactories(qcblockid);
            qcsummarydetails.ProductCategoryList = await _qcBlockrepo.GetQcBlockProductCategory(qcblockid);
            qcsummarydetails.ProductCateogrySubList = await _qcBlockrepo.GetQcBlockProductSubCategory(qcblockid);
            qcsummarydetails.ProductCategorySub2List = await _qcBlockrepo.GetQcBlockProductSub2Category(qcblockid);

            return result.Select(x => QCBlockMap.MapQCSummaryData(x, qcsummarydetails)).ToList();
        }

        /// <summary>
        /// delete the qc block details and child tables
        /// </summary>
        /// <param name="qcBlockIds"></param>
        /// <returns></returns>
        public async Task<DeleteQCBlockResponse> DeleteQCBlock(IEnumerable<int> qcBlockIds)
        {
            var qcBlockDetails = await _qcBlockrepo.GetQCBlockData(qcBlockIds);

            foreach (var _qcBlock in qcBlockDetails)
            {
                _qcBlock.Active = false;
                _qcBlock.DeletedBy = _applicationContext.UserId;
                _qcBlock.DeletedOn = DateTime.Now;

                _qcBlockrepo.RemoveEntities(_qcBlock.QcBlCustomers);
                _qcBlockrepo.RemoveEntities(_qcBlock.QcBlProductCatgeories);
                _qcBlockrepo.RemoveEntities(_qcBlock.QcBlProductSubCategories);
                _qcBlockrepo.RemoveEntities(_qcBlock.QcBlProductSubCategory2S);
                _qcBlockrepo.RemoveEntities(_qcBlock.QcBlSupplierFactories);
            }

            await _qcBlockrepo.Save();

            return new DeleteQCBlockResponse() { Result = QCBlockResponseResult.Success };
        }

        /// <summary>
        /// get QC Block Id list
        /// </summary>
        /// <returns></returns>
        public async Task<List<int>> GetQCBlockIdList(int bookingId)
        {
            //get product category Details by booking id
            var productCategoryList = await _bookingRepo.GetProductCategoryDetails(new[] { bookingId });

            //get booking data by booking id
            var bookingData = await _bookingRepo.GetBookingTransaction(bookingId);

            //get block id list
           var QCBlockList =  _qcBlockrepo.GetQCBlockRelatedIdList();            

            var ListProductCategory = productCategoryList.ToList();

            //QC filter by customer list if booking customer has data
            if (QCBlockList.Where(x => x.QcBlCustomers.Any()).Any())
            {
                QCBlockList = QCBlockList.Where(x => (!x.QcBlCustomers.Any()) || 
                x.QcBlCustomers.Any(y => y.CustomerId == bookingData.CustomerId));
            }

            //QC filter by supplier list if booking supplier has data
            if (QCBlockList.Where(x => x.QcBlSupplierFactories.Any()).Any())
            {
                QCBlockList = QCBlockList.Where(x => (!x.QcBlSupplierFactories.Any()) ||
                x.QcBlSupplierFactories.Any(y => y.TypeId == (int)Supplier_Type.Supplier_Agent && 
                y.SupplierFactoryId.GetValueOrDefault() == bookingData.SupplierId));
            }

            //QC filter by factory list if booking facotry has data
            if (QCBlockList.Where(x => x.QcBlSupplierFactories.Any()).Any())
            {
                QCBlockList = QCBlockList.Where(x => (!x.QcBlSupplierFactories.Any()) ||
                x.QcBlSupplierFactories.Any(y => y.TypeId == (int)Supplier_Type.Factory && y.SupplierFactoryId.GetValueOrDefault() == bookingData.FactoryId));
            }

            //QC filter by productcategory, sub, susb 2 list if booking product category details
            if (ListProductCategory.Any())
            {
                if (QCBlockList.Where(x => x.QcBlProductCatgeories.Any()).Any())
                {
                    QCBlockList = QCBlockList.Where(x => (!x.QcBlProductCatgeories.Any()) ||
                    x.QcBlProductCatgeories.Any(z => ListProductCategory.Select(y => y.ProductCategoryId).Contains(z.ProductCategoryId.Value)));
                }

                if (QCBlockList.Where(x => x.QcBlProductSubCategories.Any()).Any())
                {
                    QCBlockList = QCBlockList.Where(x => (!x.QcBlProductSubCategories.Any()) ||
                    x.QcBlProductSubCategories.Any(z => ListProductCategory.Select(y => y.ProductCategorySubId).Contains(z.ProductSubCategoryId.Value)));
                }

                if (QCBlockList.Where(x => x.QcBlProductSubCategory2S.Any()).Any())
                {
                    QCBlockList = QCBlockList.Where(x => (!x.QcBlProductSubCategory2S.Any()) ||
                    x.QcBlProductSubCategory2S.Any(z => ListProductCategory.Select(y => y.ProductCategorySub2Id).Contains(z.ProductSubCategory2Id.Value)));
                }
            }

            var QCBLList = await QCBlockList.ToListAsync();

            var QCIds = QCBLList.Any() ? QCBLList.Select(x => x.Qcid).ToList() : new List<int>();

            return QCIds;
        }
    }
}
