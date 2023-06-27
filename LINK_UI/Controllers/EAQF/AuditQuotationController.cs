//using Contracts.Managers;
//using DTO.Customer;
//using DTO.Eaqf;
//using DTO.InvoicePreview;
//using FastReport.Export.Pdf;
//using FastReport.Web;
//using LINK_UI.Controllers.EXTERNAL;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using RabbitMQUtility;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;

//namespace LINK_UI.Controllers.EAQF
//{
//    [Route("api/EAQF/[controller]")]
//    [Authorize(Policy = "EAQFUserPolicy")]
//    [ApiController]
//    public class AuditQuotationController : ExternalBaseController
//    {
//        private readonly IQuotationManager _manager = null;
//        private readonly ITenantProvider _filterService = null;
//        private readonly IEmailLogQueueManager _emailLogQueueManager = null;
//        private readonly IRabbitMQGenericClient _rabbitMQClient = null;
//        private static IConfiguration _configuration = null;
//        private IManualInvoiceManager _manualInvoiceManager;
//        private readonly IInspectionCustomReportManager _inspectionCustomReportManager;

//        public AuditQuotationController(IQuotationManager manager, ITenantProvider filterService, IManualInvoiceManager manualInvoiceManager,
//            IEmailLogQueueManager emailLogQueueManager, IInspectionCustomReportManager inspectionCustomReportManager,
//            IRabbitMQGenericClient rabbitMQClient,
//            IConfiguration configuration)
//        {
//            _manager = manager;
//            _filterService = filterService;
//            _emailLogQueueManager = emailLogQueueManager;
//            _rabbitMQClient = rabbitMQClient;
//            _configuration = configuration;
//            _manualInvoiceManager = manualInvoiceManager;
//            _inspectionCustomReportManager = inspectionCustomReportManager;
//        }

//        [HttpPost]
//        public async Task<IActionResult> SaveEaqfInspectionQuotation(SaveQuotationEaqfRequest request)
//        {
//            var response = await _manager.SaveEAQFQuotationAndInvoice(request);

//            System.Reflection.PropertyInfo pi = response.GetType().GetProperty("statusCode");
//            if (pi != null)
//            {
//                var statusCode = (HttpStatusCode)(pi.GetValue(response, null));
//                if (statusCode != HttpStatusCode.OK)
//                {
//                    return BuildCommonEaqfResponse(response);
//                }

//                System.Reflection.PropertyInfo piObj = response.GetType().GetProperty("data");
//                if (piObj != null)
//                {
//                    var eaqfGetSuccessResponse = (SaveQuotationEaqfResponse)(piObj.GetValue(response, null));
//                    if (eaqfGetSuccessResponse != null && eaqfGetSuccessResponse.InvoiceId > 0)
//                    {
//                        var result = await GenerateInovicePdf(eaqfGetSuccessResponse.InvoiceId, request.UserId);
//                        return BuildCommonEaqfResponse(result);
//                    }
//                }
//            }
//            return BuildCommonEaqfResponse(response);
//        }

//        [HttpGet("invoiceinfo")]
//        public async Task<IActionResult> GetEaqfInspectionBookingInvoiceInformation(string bookingIds)
//        {
//            var response = await _manager.GetAuditEaqfInspectionInvoiceDetails(bookingIds);
//            return BuildCommonEaqfResponse(response);
//        }

//        private async Task<object> GenerateInovicePdf(int invoiceId, int userId)
//        {
//            WebReport InspReport = new WebReport();
//            SaveInvoicePdfResponse saveInvoicePdf = new SaveInvoicePdfResponse();
//            string reportPath = "";
//            string invoiceNo = "";
//            try
//            {
//                if (invoiceId > 0)
//                {
//                    var invoice = await _manualInvoiceManager.GetEaqfManualInvoice(invoiceId);

//                    if (invoice == null)
//                        return new SaveInvoicePdfResponse() { Result = SaveInvoicePdfResult.InvoiceNotFound };

//                    invoiceNo = invoice.Invoice.FirstOrDefault()?.InvoiceNo;

//                    var invoicePdfPath = Path.Combine("Views", "Report_Templates", "EAQF", "E-AQF-Invoice.frx");

//                    InspReport.Report.Load(invoicePdfPath);

//                    if (invoice.Invoice != null)
//                    {
//                        InspReport.Report.RegisterData(invoice.Invoice, "Invoice");
//                    }


//                    if (invoice.InvoiceItems != null)
//                    {
//                        InspReport.Report.RegisterData(invoice.InvoiceItems, "InvoiceItems");
//                    }

//                    InspReport.Report.Prepare();
//                    string fileExtension = "pdf";
//                    string fileName = invoice.Invoice.FirstOrDefault().InvoiceNo + "." + fileExtension;

//                    PDFExport reportPDF = new PDFExport();
//                    reportPDF.PrintOptimized = true;
//                    reportPDF.JpegCompression = true;
//                    reportPDF.JpegQuality = 60;
//                    using (var memory = new MemoryStream())
//                    {
//                        InspReport.Report.Export(reportPDF, memory);
//                        var result = _inspectionCustomReportManager.FetchCloudReportUrl(memory, fileName, fileExtension, Entities.Enums.FileContainerList.InvoiceSend);
//                        reportPath = result.filePath;
//                        if (!string.IsNullOrWhiteSpace(result.filePath))
//                        {
//                            saveInvoicePdf = await _manualInvoiceManager.SaveInvoicePdfUrl(invoiceId, result.filePath, result.uniqueId, userId);
//                            if (saveInvoicePdf.Result != SaveInvoicePdfResult.Success)
//                                return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, "Internal Server Error", new List<string>() { "Invoice pdf url not saved" });
//                        }
//                        else
//                        {
//                            return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, "Internal Server Error", new List<string>() { "Invoice pdf is not saved in the database" });
//                        }
//                    }
//                }
//                else
//                {
//                    return BuildCommonEaqfResponse(HttpStatusCode.InternalServerError, "Internal Server Error", new List<string>() { "Something went wrong, Invoice is not found" });
//                }

//                InspReport.Report.Dispose();
//                return new EaqfGetSuccessResponse()
//                {
//                    data = new SaveEAQFInvoicePDFResponse()
//                    {
//                        invoicePdfUrl = reportPath,
//                        invoiceNo = invoiceNo
//                    },
//                    statusCode = HttpStatusCode.OK,
//                    message = "Success"
//                };
//            }
//            catch (Exception ex)
//            {
//                saveInvoicePdf.Result = SaveInvoicePdfResult.Error;
//                InspReport.Report.Dispose();
//            }
//            return saveInvoicePdf;
//        }

//        private object BuildCommonEaqfResponse(HttpStatusCode statusCode, string message, List<string> errors)
//        {
//            return new EaqfErrorResponse()
//            {
//                statusCode = statusCode,
//                message = message,
//                errors = errors
//            };
//        }
//    }
//}
