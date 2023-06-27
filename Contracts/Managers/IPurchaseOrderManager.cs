using DTO.Customer;
using DTO.File;
using DTO.PurchaseOrder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IPurchaseOrderManager
    {

        Task<SavePurchaseOrderResponse> SavePurchaseOrder(PurchaseOrderDetailedInfo request);
        Task<CommonCustomerPurchaseOrderResponse> SaveCustomerPurchaseDetails(CustomerPurchaseOrderDetails request);
        /// <summary>
        /// Update PO details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CommonCustomerPurchaseOrderResponse> UpdateCustomerPurchaseDetails(CustomerPurchaseOrderDetails request);
        Task<CommonCustomerPurchaseOrderResponse> Deletecustomerpurchaseorder(string pono);
        Task<GetCustomerPurchaseOrderResponse> GetPurchaseOrderDetails(string pono);
        Task<SavePurchaseOrdersResponse> SavePurchaseOrderFromExcel(List<PurchaseOrderUpload> request);

        Task<EditPurchaseOrderResponse> GetPurchaseOrderItemsById(int? id);

        Task<PurchaseOrderSearchResponse> GetPurchaseOrderItemsByCustomerId(int? id);

        Task<PurchaseOrderSearchResponse> GetAllPurchaseOrderItems(PurchaseOrderSearchRequest request);
        Task<PurchaseOrderExportDataResponse> PurchaseOrderExportDetails(PurchaseOrderExportRequest request);

        Task<PurchaseOrderDeleteResponse> RemovePurchaseOrder(int id);

        Task UploadFiles(int poid, Dictionary<Guid, byte[]> fileList);

        Task<FileResponse> GetFile(int id);

        PurchaseOrderUploadResponse GetUploadedPurchaseOrders(int customerId, IFormFile file);

        Task<POProductUploadResponse> ProcessPoProductData(IFormFile file, Dictionary<string, string> requestFormValues);

        Task<PoListResponse> GetPoListByNameAndCustomer(string poName, int customerId);

        Task<PurchaseOrderResponse> GetPurchaseOrderById(int? Id);
        Task<PurchaseOrderDetailsResponse> PurchaseOrderDetailsById(PurchaseOrderDetailsRequest request);
        Task<PurchaseOrderAttachmentsResponse> PurchaseOrderAttachmentsById(int? Id);
        Task<PurchaseOrderResponse> RemovePurchaseOrderDetail(RemovePurchaseOrderDetailsRequest request);

        Task<PoListResponse> GetPODataSource(PODataSourceRequest request);

        Task<POProductListResponse> GetPOProductList(POProductDataSourceRequest request);

        Task<POProductDataResponse> GetPOProductListData(POProductDataRequest request);
        Task<PoBookingDetailsResponse> GetPOBookingDetails(int poId);

        Task<bool> CheckPurchaseOrderExistInBooking(int poid);
    }
}
