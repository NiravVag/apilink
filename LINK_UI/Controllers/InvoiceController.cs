using Contracts.Managers;
using DTO.CommonClass;
using DTO.Invoice;
using Entities.Enums;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BI.TenantProvider;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceManager _invoiceService = null;
        private static IConfiguration _configuration = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly ITenantProvider _tenant = null;
        public InvoiceController(IInvoiceManager invoiceService, IConfiguration configuration, ISharedInspectionManager helper,ITenantProvider tenant)
        {
            _invoiceService = invoiceService;
            _configuration = configuration;
            _helper = helper;
            _tenant = tenant;
        }

        [HttpPost("generate")]
        [Right("invoice")]
        public async Task<InvoiceGenerateResponse> GenerateInvoice(InvoiceGenerateRequest requestDto)
        {

            if (!ModelState.IsValid)
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.RequestIsNotValid };
            }

            if (requestDto.InvoiceType == (int)INVInvoiceType.Monthly && ((requestDto.RealInspectionFromDate.ToDateTime().Date > DateTime.Now.Date) || (requestDto.RealInspectionToDate.ToDateTime().Date > DateTime.Now.Date)))
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.FutureDateNotAllowed };
            }

            if (requestDto.RealInspectionToDate.ToDateTime().Date < requestDto.RealInspectionFromDate.ToDateTime().Date)
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.FromDateAfterToDate };
            }

            if (!requestDto.IsTravelExpense && !requestDto.IsInspection)
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.TravelOrInspectionRequired };
            }
            // supplier means
            if (requestDto.InvoiceTo == (int)InvoiceTo.Supplier)
            {
                var supplierInfor = requestDto.SupplierInfo;
                if (supplierInfor == null)
                {
                    return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoSupplierSelected };
                }
                if (supplierInfor?.SupplierId == 0)
                {
                    return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.SupplierIsRequired };
                }
            }

            if (requestDto.IsNewBookingInvoice)
            {
                if (requestDto.BookingNoList == null || !requestDto.BookingNoList.Any())
                {
                    return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.NoInspectionSelected };
                }
            }

            // Added bank validation
            if (requestDto.BillingEntity > 0 && (requestDto.BankAccount == null || requestDto.BankAccount == 0))
            {
                return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.BankIsRequired };
            }

            if (requestDto.Service == (int)Service.InspectionId)
            {
                return await _invoiceService.InspectionInvoiceGenerate(requestDto);
            }

            else if (requestDto.Service == (int)Service.AuditId)
            {
                return await _invoiceService.AuditInvoiceGenerate(requestDto);
            }
             
            return new InvoiceGenerateResponse() { Result = InvoiceGenerateResult.RequestIsNotValid };
        }

        [HttpGet("getinvoicebasedetails/{invoiceNo}/{serviceId}")]
        public async Task<InvoiceBaseDetailResponse> GetInvoiceBaseDetails(string invoiceNo, int serviceId)
        {
            if (string.IsNullOrEmpty(invoiceNo))
                return new InvoiceBaseDetailResponse { Result = InvoiceBaseDetailResult.InvoiceNoEmpty };
            return await _invoiceService.GetInvoiceBaseDetails(invoiceNo, serviceId);
        }

        [HttpGet("getbilledaddress/{billToId}/{searchId}")]
        public async Task<InvoiceBilledAddressResponse> GetBilledAddressDetails(int billToId, int searchId)
        {
            if (billToId == 0)
                return new InvoiceBilledAddressResponse { Result = InvoiceBilledAddressResult.BillToIdCannotBeEmpty };
            if (searchId == 0)
                return new InvoiceBilledAddressResponse { Result = InvoiceBilledAddressResult.SearchIdCannotBeEmpty };
            return await _invoiceService.GetInvoiceBilledAddress(billToId, searchId);
        }

        [HttpGet("getinvoicecontacts/{billToId}/{searchId}")]
        public async Task<InvoiceContactsResponse> GetInvoiceContacts(int billToId, int searchId)
        {
            if (billToId == 0)
                return new InvoiceContactsResponse { Result = InvoiceBilledContactsResult.BillToIdCannotBeEmpty };
            if (searchId == 0)
                return new InvoiceContactsResponse { Result = InvoiceBilledContactsResult.SearchIdCannotBeEmpty };
            return await _invoiceService.GetInvoiceContacts(billToId, searchId);
        }

        [HttpGet("getinvoicepaymentstatus")]
        public async Task<DataSourceResponse> GetInvoicePaymentStatus()
        {
            return await _invoiceService.GetInvoicePaymentStatus();
        }

        [HttpGet("getinvoiceoffice")]
        public async Task<DataSourceResponse> GetInvoiceOffice()
        {
            return await _invoiceService.GetInvoiceOffice();
        }

        [HttpGet("getinvoicetransactiondetails/{invoiceNo}/{serviceId}")]
        public async Task<InvoiceTransactionDetailsResponse> GetInvoiceTransactionDetails(string invoiceNo, int serviceId)
        {
            if (string.IsNullOrEmpty(invoiceNo))
                return new InvoiceTransactionDetailsResponse { Result = InvoiceTransactionDetailsResult.InvoiceNoCannotBeEmptyOrZero };
            return await _invoiceService.GetInvoiceTransactionDetails(invoiceNo, serviceId);
        }

        [HttpGet("getinvoicebookingmoreinfo/{bookingId}")]
        public async Task<InvoiceBookingMoreInfoResponse> GetInvoiceBookingMoreInfo(int bookingId)
        {
            return await _invoiceService.GetInvoiceBookingMoreInfo(bookingId);
        }

        [HttpPost("updateinvoicedetails")]
        public async Task<UpdateInvoiceDetailsResponse> Update(UpdateInvoiceDetailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new UpdateInvoiceDetailsResponse() { Result = UpdateInvoiceDetailResult.Failure };
            }
            return await _invoiceService.UpdateInvoiceTransaction(request);
        }

        [HttpDelete("{id}")]
        public async Task<DeleteInvoiceDetailResponse> DeleteInvoiceDetail(int id)
        {
            return await _invoiceService.DeleteInvoiceDetail(id);
        }

        [HttpPost("getInvoiceSummary")]
        public async Task<InvoiceSummaryResponse> GetInvoiceSummary(InvoiceSummaryRequest requestDto)
        {
            return await _invoiceService.GetInvoiceSearchSummary(requestDto);
        }


        [HttpPost("CheckInvoicePdfCreated")]
        public async Task<InvoicePdfCreatedResponse> CheckInvoicePdfCreated(InvoicePdfCreatedRequest request)
        {
            return await _invoiceService.CheckInvoicePdfCreated(request);
        }

        [HttpPost("getInvoiceReportTemplates")]
        public async Task<InvoiceReportTemplateResponse> GetInvoiceReportTemplates(InvoiceReportTemplateRequest templateRequest)
        {
            return await _invoiceService.GetInvoiceReportTemplates(templateRequest);
        }

        [HttpGet("GetInvoiceReportTemplateUrl")]
        public InvoiceReportUrlResponse GetInvoiceReportUrl()
        {
            var response = new InvoiceReportUrlResponse();
            IEnumerable<IConfigurationSection> config = null;
            var Entityid = _tenant.GetCompanyId();
            if(Entityid==(int) Company.api)
            {
                 config = _configuration.GetSection("InvoiceViewSettings:API").GetChildren();
            }
            else if(Entityid == (int)Company.sgt)
            {
                config = _configuration.GetSection("InvoiceViewSettings:SGT").GetChildren();
            }
            else if (Entityid == (int)Company.aqf)
            {
                config = _configuration.GetSection("InvoiceViewSettings:AQF").GetChildren();
            }
            response.Url = config?.Where(x => x.Key == "InvoiceTemplateUrl").Select(x => x.Value).FirstOrDefault();
            response.EntityId = config?.Where(x => x.Key == "EntityId").Select(x => x.Value).FirstOrDefault();

            response.Result = string.IsNullOrEmpty(response.Url) ? InvoiceSummaryResult.Success : InvoiceSummaryResult.Success;

            return response;
        }

        [HttpGet("CancelInvoice/{invoiceNo}")]
        public async Task<InvoiceCancelResponse> CancelInvoice(string invoiceNo)
        {
            return await _invoiceService.CancelInvoice(invoiceNo);
        }

        [HttpGet("getInvoiceBookingSummary/{invoiceNo}/{serviceId}")]
        public async Task<InvoiceBookingSummaryResponse> GetInvoiceSummary(string invoiceNo, int serviceId)
        {
            return await _invoiceService.GetInvoiceBookingSearchSummary(invoiceNo, serviceId);
        }

        [HttpPost("ExportInvoiceSearchSummary")]
        public async Task<IActionResult> ExportInvoiceSearchSummary(InvoiceSummaryRequest request)
        {
            int pageindex = 0;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.pageSize = PageSize;
            var response = await _invoiceService.ExportInvoiceSearchSummary(request);
            if (response == null)
                return NotFound();
            var stream = _helper.GetAsStreamObject(response);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "invoice_summary.xlsx");
        }

        [HttpGet("checkinvoicenumberexist/{invoiceNo}")]
        public async Task<InvoiceMoExistsResult> CheckInvoiceNumberExist(string invoiceNo)
        {
            return await _invoiceService.CheckInvoiceNumberExist(invoiceNo);
        }

        [HttpGet("getinvoicebookingproducts/{bookingId}")]
        public async Task<InvoiceBookingProductsResponse> GetInvoiceBookingProducts(int bookingId)
        {
            return await _invoiceService.GetInvoiceBookingProducts(bookingId);
        }

        [HttpGet("getInvoiceStatusList")]
        public async Task<DataSourceResponse> GetInvoiceStatusList()
        {
            return await _invoiceService.GetInvoiceStatusList();
        }

        [HttpPost("getNewInvoiceBookingList")]
        [Right("invoice")]
        public async Task<InvoiceNewBookingResponse> GetNewInvoiceBookingList(NewBookingInvoiceSearch request)
        {
            return await _invoiceService.GetNewInvoiceBookingData(request);
        }

        [HttpPost("getKpiTemplateList")]
        public async Task<InvoiceKpiTemplateResponse> GetKpiTemplateList(InvoiceKpiTemplateRequest request)
        {
            return await _invoiceService.GetInvoiceKpiTemplate(request);
        }
    }
}
