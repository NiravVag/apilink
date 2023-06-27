using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using Components.Web;
using DTO.PurchaseOrder;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class PurchaseOrderController : ControllerBase
    {

        private readonly IPurchaseOrderManager _manager = null;
        private readonly IHostingEnvironment _env;
        private static IConfiguration _Configuration = null;
        private readonly IDocumentManager _documentManager = null;

        public PurchaseOrderController(IPurchaseOrderManager manager, IHostingEnvironment env, 
            IConfiguration configuration, IDocumentManager documentManager)
        {
            _manager = manager;
            _env = env;
            _Configuration = configuration;
            _documentManager = documentManager;
        }

        // POST: api/PurchaseOrder/search
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("search")]
        [Right("purchaseorder-summary")]
        public async Task<PurchaseOrderSearchResponse> GetAllPurchaseOrderItems(PurchaseOrderSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null || request.pageSize == 0)
                request.pageSize = 10;
            return await _manager.GetAllPurchaseOrderItems(request);
        }

        //Export Purchase Order Products
        [Authorize(Policy = "ApiUserPolicy")]
        [Right("purchaseorder-summary")]
        [HttpPost("export-purchaseorderproducts")]
        public async Task<IActionResult> ExportPurchaseOrderProducts(PurchaseOrderExportRequest request)
        {
            var response = await _manager.PurchaseOrderExportDetails(request);
            if (response == null || response.Result == PurchaseOrderSearchResult.NotFound)
                return NotFound();
            return await this.FileAsync("PurchaseOrderProducts", response.PurchaseOrderExportDataData, Components.Core.entities.FileType.Excel);
        }

        // GET: api/PurchaseOrder/5
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpGet("{id}")]
        [Right("purchaseorder-summary")]
        public async Task<EditPurchaseOrderResponse> GetPurchaseOrderItemsById(int id)
        {
            return await _manager.GetPurchaseOrderItemsById(id);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpGet("purchaseordersbycustomer/{id}")]
        public async Task<PurchaseOrderSearchResponse> GetPurchaseOrderItemsByCustomerId(int id)
        {
            return await _manager.GetPurchaseOrderItemsByCustomerId(id);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpGet("PurchaseOrderById/{id}")]
        public async Task<PurchaseOrderResponse> GetPurchaseOrderById(int id)
        {
            return await _manager.GetPurchaseOrderById(id);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpGet("PurchaseOrderAttachmentsById/{id}")]
        public async Task<PurchaseOrderAttachmentsResponse> GetPurchaseOrderAttachmentsById(int id)
        {
            return await _manager.PurchaseOrderAttachmentsById(id);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("SearchPurchaseOrderDetails")]
        [Right("edit-booking")]
        public async Task<PurchaseOrderDetailsResponse> GetPurchaseOrderDetailsById([FromBody] PurchaseOrderDetailsRequest request)
        {
            return await _manager.PurchaseOrderDetailsById(request);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("RemovePurchaseOrderDetail")]
        public async Task<PurchaseOrderResponse> RemovePurchaseOrderDetail(RemovePurchaseOrderDetailsRequest request)
        {
            return await _manager.RemovePurchaseOrderDetail(request);
        }
        // POST: api/PurchaseOrder
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost]
        [Right("purchaseorder-summary")]
        public async Task<SavePurchaseOrderResponse> SavePurchaseOrder([FromBody] PurchaseOrderDetailedInfo request)
        {
            return await _manager.SavePurchaseOrder(request);
        }

        // POST: api/PurchaseOrder
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("savepolistfromexcel")]
        [Right("purchaseorder-summary")]
        public async Task<SavePurchaseOrdersResponse> SavePurchaseOrderFromExcel([FromBody] List<PurchaseOrderUpload> request)
        {
            return await _manager.SavePurchaseOrderFromExcel(request);


        }

        // PUT: api/PurchaseOrder/5
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPut("{id}")]
        [Right("purchaseorder-summary")]
        public async Task<SavePurchaseOrderResponse> UpdatePurchaseOrder(int id, [FromBody] PurchaseOrderDetailedInfo request)
        {
            return await _manager.SavePurchaseOrder(request);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpDelete("{id}")]
        [Right("purchaseorder-summary")]
        public async Task<PurchaseOrderDeleteResponse> DeletePurchaseOrder(int id)
        {
            return await _manager.RemovePurchaseOrder(id);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("attached/{poID}")]
        [DisableRequestSizeLimit]
        [Right("purchaseorder-summary")]
        public bool UploadAttachedFiles(int poID)
        {
            if (Request.Form.Files != null && Request.Form.Files.Any())
            {
                var dict = new Dictionary<Guid, byte[]>();

                foreach (var file in Request.Form.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        Guid.TryParse(file.Name, out Guid guid);
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            dict.Add(guid, fileBytes);
                        }
                    }
                }

                _manager.UploadFiles(poID, dict).Wait();
                return true;
            }
            return false;
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("uploadpurchaseorder/{customerId}")]
        [DisableRequestSizeLimit]
        [Right("uplaod-purchaseorder")]
        public PurchaseOrderUploadResponse UploadPurchaseOrder(int customerId)
        {
            if (Request.Form.Files != null && Request.Form.Files.Any())
            {
                foreach (var file in Request.Form.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        return _manager.GetUploadedPurchaseOrders(customerId, file);
                    }
                }
            }
            return null;
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpGet("file/{id}")]
        [Right("uplaod-purchaseorder")]
        public async Task<IActionResult> GetFile(int id)
        {
            var file = await _manager.GetFile(id);

            if (file.Result == DTO.File.FileResult.NotFound)
                return NotFound();

            return File(file.Content, file.MimeType); // returns a FileStreamResult
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("getpolistbyname")]
        [Right("purchaseorder-summary")]
        public async Task<PoListResponse> GetPOListByName([FromBody] AutoPoNumber request)
        {
            return await _manager.GetPoListByNameAndCustomer(request.poname, request.customerId);
        }

        [Authorize(Policy = "ApiUserPolicy")]
        [HttpGet("CheckPurchaseOrderExistInBooking/{id}")]
        [Right("purchaseorder-summary")]
        public async Task<bool> CheckPurchaseOrderExistInBooking(int id)
        {
            return await _manager.CheckPurchaseOrderExistInBooking(id);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("getpoproductlist")]
        [Right("purchaseorder-summary")]
        public async Task<POProductListResponse> GetProductListByName(POProductDataSourceRequest request)
        {
            return await _manager.GetPOProductList(request);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("getpodatasource")]
        [Right("purchaseorder-summary")]
        public async Task<PoListResponse> GetPODataSource(PODataSourceRequest request)
        {
            return await _manager.GetPODataSource(request);
        }
        [Authorize(Policy = "ApiUserPolicy")]
        [HttpPost("getpoproductdata")]
        [Right("purchaseorder-summary")]
        public async Task<POProductDataResponse> GetPoProductData(POProductDataRequest request)
        {
            return await _manager.GetPOProductListData(request);
        }


        [HttpPost("upload-po-product-data")]
        [DisableRequestSizeLimit]
        [Right("uplaod-purchaseorder")]
        public async Task<POProductUploadResponse> UploadPoProductData()
        {
            POProductUploadResponse response = null;
            
            if (Request.Form.Files != null && Request.Form.Files.Any())
            {
                //read the first file
                var file = Request.Form.Files.FirstOrDefault();

                if (file != null)
                {
                    var requestFormValues = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                    //mange the po product data
                    response = await _manager.ProcessPoProductData(file, requestFormValues);
                }
            }

            return response;
        }

        [Right("purchaseorder-summary")]
        [HttpGet("fileTerms")]
        public IActionResult GetPurchaseOrderUploadTemplate()
        {
            var filepath = _env.WebRootPath;
           
            var purchaseOrderUpload = _Configuration["PurchaseOrderTemplate"].ToString();
            var file = _documentManager.GetFileData(filepath, purchaseOrderUpload);

            if (file == null || file.Result == DTO.File.FileResult.NotFound || file.Content == null)
                return NotFound();

            return File(file.Content, file.MimeType); // returns a FileStreamResult
        }

        [HttpGet("get-po-booking-details/{poId}")]
        [Right("purchaseorder-summary")]
        public async Task<PoBookingDetailsResponse> GetBookingDetails(int poId)
        {
            return await _manager.GetPOBookingDetails(poId);
        }
    }
}
