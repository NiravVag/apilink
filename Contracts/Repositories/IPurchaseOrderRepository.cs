using DTO.Common;
using DTO.CommonClass;
using DTO.PurchaseOrder;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IPurchaseOrderRepository : IRepository
    {
        Task<int> AddPurchaseOrder(CuPurchaseOrder entity);

        CuPurchaseOrderDetail GetPurchaseOrderDetailsExist(int poId, int productId, int supplierId);
        CuPurchaseOrder GetPurchaseOrderItemsById(int? id);
        CuPurchaseOrder GetPurchaseOrderItemsByPono(string pono,int customerId);

        IEnumerable<CuPurchaseOrder> GetPurchaseOrderItemsByCustomerId(int? id);

        CuPurchaseOrder GetPurchaseOrderItemsByCustomerAndPO(int? id, string pono);

        CuPurchaseOrder GetPurchaseOrderItemsByCustomerAndPO(int? id, int poid);

        IEnumerable<CuPurchaseOrder> GetAllPurchaseOrderItems(PurchaseOrderSearchRequest request);

        Task<int> EditPurchaseOrder(CuPurchaseOrder entity);

        Task<int> EditPurchaseOrderDetail(CuPurchaseOrderDetail entity);

        Task<bool> RemovePurchaseOrder(int id,int deletedby);

        Task<IEnumerable<CuPurchaseOrderAttachment>> GetReceptFiles(int poId, IEnumerable<Guid> GuidList);

        Task<CuPurchaseOrderAttachment> GetFile(int id);

        IEnumerable<CuPurchaseOrder> GetPurchaseOrderItems();
        IEnumerable<CuPurchaseOrder> GetPurchaseOrderDetailsByCustomerId(int? id);

        IEnumerable<CuPurchaseOrderDetail> GetPurchaseOrderDetailsById(int? id);
        /// <summary>
        /// Get the po detail list with the auto complete option
        /// </summary>
        /// <param name="poName"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<IEnumerable<PoDataSource>> GetPoDetailByNameAndCustomer(string poName, int customerId);

        IEnumerable<CuPurchaseOrderDetail> GetPurchaseOrderDetailsById(int poid, int productid);

        IQueryable<PurchaseOrderRepoData> GetAllPurchaseOrderData();
        IQueryable<PurchaseOrderDetailsRepo> GetAllPurchaseOrderExportData();

        Task<IEnumerable<PurchaseOrderDetailsRepoData>> GetOrderDetails(IEnumerable<int> poIds);

        Task<PurchaseOrder> GetPurchaseOrderById(int? Id);
        IQueryable<PurchaseOrderRepo> PurchaseOrderDetailsById(int id);
        IQueryable<FileAttachment> PurchaseOrderAttachmentsById(int? id);
        Task<IEnumerable<int>> GetPoProductIds(int Id, IEnumerable<int> productIds);
        IQueryable<CuPurchaseOrderDetail> PurchaseOrderdetail(int id);

        Task<bool> CheckPoProductIsMappedToBooking(int poid, int productid);

        IQueryable<CuPurchaseOrder> GetPurchaseOrder();

        IQueryable<CuPurchaseOrderDetail> GetPurchaseOrderDetails();

        IQueryable<CuPurchaseOrderDetail> GetPurchaseOrderDetailsAndPOSupplierData();

        Task<List<CuPurchaseOrder>> GetPurchaseOrderByPoIds(List<int> poList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pono">Po number</param>
        /// <returns></returns>
        Task<CUPurchaseOrderListResponse> GetPurchaseOrderDetailsByPo(string pono, int customerId);

        Task<List<ExistingPoProductList>> GetPurchaseOrderByPONoCustomerSupplier(int customerId, int supplierId, List<string> poNameList);

        Task<List<CuPurchaseOrder>> GetPurchaseOrderDetailsByPoIds(List<int> poList);

        Task<List<ExistingPoProductList>> GetPurchaseOrdersByPoProduct(int customerId, List<int> PoIds, List<int> productIds);

        //Task<List<CommonDataSource>> GetPurchaseOrderByPONoCustomerSupplier(int customerId, int supplierId, List<string> poNameList);

        Task<List<POBookingRepo>> GetPurchaseOrderExistInBooking(IEnumerable<int> poid);
        Task<List<PoBookingDetailRepo>> GetPOBookingDetailsList(int poid);

        Task<bool> CheckPurchaseOrderExistInBooking(int poid);

        //Task<List<CommonDataSource>> GetPoMappedSuppliers(int? poId);
        //Task<List<CommonDataSource>> GetPoMappedFactories(int? poId);

        Task<List<POMappedSupplier>> GetPurchaseOrderSupplierDetails(List<int> poIdList);

        Task<List<POMappedFactory>> GetPurchaseOrderFactoryDetails(List<int> poIdList);

        CuPurchaseOrder GetPurchaseOrderDetailsByCustomerAndPO(int? id, string pono);

        Task<List<CuPurchaseOrder>> GetPurchaseOrderSupplierFactoryByPoIds(List<int> poList);

    }
}
