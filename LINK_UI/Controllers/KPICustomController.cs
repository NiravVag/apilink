using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.Kpi;
using Microsoft.AspNetCore.Mvc;
using Contracts.Managers;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using static DTO.Common.Static_Data_Common;
using Components.Web;
using Microsoft.Extensions.Configuration;
using LINK_UI.FileModels;
using Microsoft.AspNetCore.Hosting;
using DTO.Common;
using Microsoft.AspNetCore.SignalR;
using LINK_UI.App_start;
using System.IO;
using OfficeOpenXml;
using BI.Utilities;
using DTO.EventBookingLog;
using DTO.Invoice;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class KPICustomController : ControllerBase
    {

        private readonly IKpiCustomManager _manager = null;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IEventBookingLogManager _logger = null;
        private static IConfiguration _configuration = null;
        private readonly ITenantProvider _tenant = null;
        public KPICustomController(IKpiCustomManager manager, IConfiguration configuration, ISharedInspectionManager helper,
            IEventBookingLogManager logger, ITenantProvider tenant)
        {
            _manager = manager;
            _configuration = configuration;
            _helper = helper;
            _logger = logger;
            _tenant = tenant;
        }

        [HttpGet("GetSummary")]
        [Right("kpi-summary")]
        public async Task<KpiResponse> GetSummary()
        {
            var res = await _manager.GetKpisummary();
            return res;
        }

        [Right("kpi-summary")]
        [HttpPost("ExportInspectionSearchSummary")]
        public async Task<IActionResult> ExportTemplateSummary([FromBody] KpiRequest request)
        {
            var response = new ExportResult();
            Stream stream = null;
            string fileName = null;

            if (request == null)
                return BadRequest();

            var list = await _manager.GetTemplateList();

            switch (list.Where(x => x.Id == request.TemplateId).Select(x => x.Id).FirstOrDefault())
            {
                case (int)KPICustomTemplate.ECI_YTD:
                    response = await _manager.ExportEciTemplate(request);
                    fileName = "KPI/" + "EciTemplate";
                    break;

                case (int)KPICustomTemplate.WareHouse:
                    response = await _manager.ExportWarehouseTemplate(request);
                    fileName = "KPI/" + "WarehouseTemplate";
                    break;

                case (int)KPICustomTemplate.GIFI_KPI:
                    response = await _manager.ExportGifiTemplate(request);
                    fileName = "KPI/" + "GifiTemplate";
                    break;

                case (int)KPICustomTemplate.AdeoInspFollowUp:
                    var res = await _manager.ExportAdeoInspFollowUpTemplateByEfCore(request);
                    if (res == null || !res.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(res);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdeoInspFollowUpTemplate.xlsx");


                case (int)KPICustomTemplate.AdeoFranceInspSummary:
                    var adeoFranceInspSummaryRes = await _manager.ExportAdeoFranceInspSummaryTemplateByEfCore(request);
                    if (adeoFranceInspSummaryRes == null || !adeoFranceInspSummaryRes.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(adeoFranceInspSummaryRes);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdeoFranceInspSummaryTemplate.xlsx");


                case (int)KPICustomTemplate.AdeoMonthInspSumByFactory:
                    var adeoInspByFactRes = await _manager.ExportAdeoMonthInspSumbySubconFactoTemplateByEfCore(request);
                    if (adeoInspByFactRes == null || !adeoInspByFactRes.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(adeoInspByFactRes);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdeoMonthInspSumbySubconFactoTemplate.xlsx");

                case (int)KPICustomTemplate.AdeoMonthInspSumByQA:
                    response = await _manager.ExportAdeoInspSummaryByQATemplate(request);
                    fileName = "KPI/" + "AdeoMonthInspSumbyQATemplate";
                    break;

                case (int)KPICustomTemplate.AdeoEANCode:
                    var adeoEanResult = await _manager.ExportAdeoEanCodeTemplate(request);
                    if (adeoEanResult == null || !adeoEanResult.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(adeoEanResult);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdeoEanCodeTemplate.xlsx");

                case (int)KPICustomTemplate.AdeoVietnamInspSumOverall:
                    var adeoInspSumOverallResult = await _manager.ExportAdeoInspSumOverallTemplateByEfCore(request);
                    if (adeoInspSumOverallResult == null || !adeoInspSumOverallResult.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(adeoInspSumOverallResult);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdeoInspSumOverallTemplate.xlsx");

                case (int)KPICustomTemplate.AdeoPOFailed:
                    var adeoFailedResult = await _manager.ExportAdeoFailedPoTemplateByEfCore(request);
                    if (adeoFailedResult == null || !adeoFailedResult.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(adeoFailedResult);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdeoFailedPoTemplate.xlsx");

                case (int)KPICustomTemplate.StockQCPerformance:
                    response = await _manager.ExportQcPerformanceBreakdownTemplate(request);
                    fileName = "KPI/" + "StokomaniQcPerformanceBreakdownTemplate";
                    break;

                case (int)KPICustomTemplate.StockMonthlyStatement:
                    response = await _manager.ExportMonthlyOrderStatementTemplate(request);
                    fileName = "KPI/" + "StokomaniMonthlyOrderStatementTemplate";
                    break;

                case (int)KPICustomTemplate.AdeoSummaryRemarks:
                    response = await _manager.ExportAdeoSummaryRemarksTemplate(request);
                    fileName = "KPI/" + "AdeoSummaryRemarks";
                    break;

                case (int)KPICustomTemplate.ECIRemark:
                    response = await _manager.ExportECIRemarksTemplate(request);
                    fileName = "KPI/" + "EciRemarkTemplate";
                    break;

                case (int)KPICustomTemplate.InspDefectSummary:
                    var defectResponse = await _manager.ExportInspDefectSummaryTemplateByEfCore(request);
                    if (defectResponse == null || defectResponse.Data == null || !defectResponse.Data.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(defectResponse.Data);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspDefectSummaryTemplate.xlsx");

                case (int)KPICustomTemplate.InspResultSummary:
                    var reportResultResponse = await _manager.ExportInspResultSummaryTemplate(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(reportResultResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspResultSummaryTemplate.xlsx");

                case (int)KPICustomTemplate.InspRemarksSummary:
                    var remarksresponse = await _manager.ExportInspRemarksSummaryTemplate(request);
                    if (remarksresponse == null || remarksresponse.Data == null || !remarksresponse.Data.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(remarksresponse.Data);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspRemarkSummaryTemplate.xlsx");

                case (int)KPICustomTemplate.Liverpool:
                    response = await _manager.ExportLiverpoolTemplate(request);
                    fileName = "KPI/" + "LiverpoolTemplate";
                    break;

                case (int)KPICustomTemplate.InspExpenseSummary:
                    var expenseBookingResponse = await _manager.ExportInspExpenseSummaryTemplate(request);
                    if (expenseBookingResponse == null || !expenseBookingResponse.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(expenseBookingResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspExpenseSummaryTemplate.xlsx");

                case (int)KPICustomTemplate.CasinoSummary:
                    response = await _manager.ExportLiverpoolTemplate(request);
                    fileName = "KPI/" + "CasinoTemplate";
                    break;

                case (int)KPICustomTemplate.StockWeeklyStatement:
                    response = await _manager.ExportWeeklyBookingStatus(request);
                    fileName = "KPI/" + "StokomaniWeeklyBookingStatusTemplate";
                    break;
                case (int)KPICustomTemplate.CarrefourInvoiceStatement:
                    var carreFourInvoiceResponse = await _manager.ExportCarreFourInvoiceTemplate(request);
                    if (carreFourInvoiceResponse == null)
                        return NotFound();
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(carreFourInvoiceResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Carrefour-Invoice.xlsx");
                case (int)KPICustomTemplate.GeneralInvoiceStatement:
                    var generalInvoiceResponse = await _manager.GetGeneralInvoiceTemplate(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(generalInvoiceResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GeneralInvoiceTemplate.xlsx");
                case (int)KPICustomTemplate.CarrefourEcopack:
                    var carreFourEcoResponse = await _manager.ExportCarrefourEcopackTemplate(request);
                    if (carreFourEcoResponse == null || !carreFourEcoResponse.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(carreFourEcoResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Carrefour-EcoPack.xlsx");
                case (int)KPICustomTemplate.CarrefourDailyResult:

                    var reportCarrefourDailyResultResponse = await _manager.ExportInspResultSummaryTemplate(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(reportCarrefourDailyResultResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CarrefourDailyResultTemplate.xlsx");


                case (int)KPICustomTemplate.MDMKPI:
                    response = await _manager.ExportMdmDefectTemplate(request);
                    fileName = "KPI/" + "MdmDefectTemplate";
                    break;
                case (int)KPICustomTemplate.InspPickingSummary:
                    response = await _manager.ExportInspectionPickingData(request);
                    fileName = "KPI/" + "InspectionPickingTemplate";
                    break;

                case (int)KPICustomTemplate.InspCommentSummary:
                    var reportCommentResponse = await _manager.ExportInspCommentSummaryTemplate(request);
                    if (reportCommentResponse == null || !reportCommentResponse.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(reportCommentResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspCommentSummaryTemplate.xlsx");

                case (int)KPICustomTemplate.OrderStatusLog:
                    var orderStatusLogResponse = await _manager.ExportOrderStatusLog(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(orderStatusLogResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderStatusLog.xlsx");

                case (int)KPICustomTemplate.InspectionData:
                    var inspectionDataResponse = await _manager.ExportInspectionData(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(inspectionDataResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspectionDataTemplate.xlsx");

                case (int)KPICustomTemplate.Cultura:
                    var customerCulturaResponse = await _manager.ExportCustomerCultura(request);
                    if (customerCulturaResponse == null || !customerCulturaResponse.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(customerCulturaResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CulturaTemplate.xlsx");

                case (int)KPICustomTemplate.ECI:
                    var eciResponse = await _manager.ExportECITemplate(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(eciResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ECITemplate.xlsx");
                case (int)KPICustomTemplate.ScheduleAnalysis:
                    var scheduleAnalysisResponse = await _manager.ExportScheduleAnalysisTemplate(request);
                    if (scheduleAnalysisResponse == null || !scheduleAnalysisResponse.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(scheduleAnalysisResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ScheduleAnalysisTemplate.xlsx");
                case (int)KPICustomTemplate.InspectionSummaryQC:
                    var inspectionSummaryQCResponse = await _manager.ExportInspectionSummaryQCTemplate(request);
                    if (inspectionSummaryQCResponse == null || !inspectionSummaryQCResponse.Any())
                        return NotFound();
                    stream = _helper.GetAsStreamObject(inspectionSummaryQCResponse);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InspectionSummaryQC.xlsx");

                case (int)KPICustomTemplate.CustomerManday:
                    var customerMandayData = await _manager.ExportCustomerMandaySummaryTemplate(request);
                    if (customerMandayData == null)
                        return NotFound();
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(customerMandayData);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerMandau.xlsx");

                case (int)KPICustomTemplate.XeroInvoice:
                    var xeroxInvoice = await _manager.ExportXeroxInvoiceTemplate(request);
                    if (xeroxInvoice != null)
                    {
                        if (xeroxInvoice.Result == XeroInvoiceResponseResult.Success)
                        {
                            stream = _helper.GetAsStreamObjectAndLoadDataTable(xeroxInvoice.ResultData);
                            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Xero_Invoice.xlsx");
                        }
                        else if (xeroxInvoice.Result == XeroInvoiceResponseResult.NoInvoiceAccess)
                        {
                            return BadRequest("No Invoice Access to export this data");
                        }
                        else if (xeroxInvoice.Result == XeroInvoiceResponseResult.StaffIsNotValid)
                        {
                            return BadRequest("Staff Id is not Valid");
                        }
                        else if (xeroxInvoice.Result == XeroInvoiceResponseResult.CannotGetList)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                case (int)KPICustomTemplate.XeroExpense:
                    var xeroExpense = await _manager.ExportXeroExpenseTemplate(request);
                    if (xeroExpense.Result == XeroInvoiceResponseResult.Success)
                    {
                        stream = _helper.GetAsStreamObjectAndLoadDataTable(xeroExpense.ResultData);
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Xero_Expense.xlsx");
                    }
                    else if (xeroExpense.Result == XeroInvoiceResponseResult.CannotGetList)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                case (int)KPICustomTemplate.GapProductRefLevel:
                    var gapProductRefData = await _manager.ExportGapProductRefLevel(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(gapProductRefData);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GapProductRef.xlsx");
                case (int)KPICustomTemplate.ARFollowUpReport:
                    var arFollowUpReport = await _manager.ExportARFollowUpReport(request);
                    if (arFollowUpReport == null)
                        return NotFound();
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(arFollowUpReport);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ARFollowUpReport.xlsx");

                case (int)KPICustomTemplate.GapFlashProcessAudit:
                    var gapFlasProcessAudit = await _manager.ExportGapFlashProcessAudit(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(gapFlasProcessAudit);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GapFlashProcessAudit.xlsx");

                case (int)KPICustomTemplate.GapAudit:
                    var gapProcessAudit = await _manager.ExportGapProcessAudit(request);
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(gapProcessAudit);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GapAudit.xlsx");

                case (int)KPICustomTemplate.InspectionExpenseSummary:
                    var inspectionSummaryExpenses = await _manager.ExportExpenseInspectionSummaryTemplate(request);
                    if (inspectionSummaryExpenses == null)
                        return NotFound();
                    stream = _helper.GetAsStreamObjectAndLoadDataTable(inspectionSummaryExpenses);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Inspection Summary - Expense.xlsx");
            }

            if (response == null || response.Data == null || !response.Data.Any())
                return NotFound();

            return await this.FileAsync(fileName, response, Components.Core.entities.FileType.Excel);
        }

        [Right("kpi-summary")]
        [HttpPost("ppt")]
        public async Task<IActionResult> GetKpiPPT([FromBody] KpiPPTRequest request, [FromServices] IHubContext<UserHub> hubLogContext, [FromServices] IAPIUserContext appContext)
        {
            var model = new KpiPPTModel
            {
                CustomerId = request.IdCustomer,
                StartDate = request.BeginDate.ToDateTime(),
                EndDate = request.EndDate.ToDateTime(),
                ByDepartment = request.LoadDepartment,
                ByFactoryLevel = request.LoadFactory,
                ListBrand = request.BrandList,
                ListDepartment = request.DeptList,
                EntityId = _tenant.GetCompanyId(),
                SearchDateTypeId = request.SearchDateTypeId
            };
            var requestMessage = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            await _logger.SaveEventLogInformation(new EventLogRequest()
            {
                CreatedTime = DateTime.Now,
                Name = "Export CustomTemplate KPI ppt",
                LogLevel = "ExportInspectionSearchSummary PPT",
                Message = requestMessage
            });

            return this.FileFromJson("PPT/KPI_API", model, Components.Core.entities.FileType.PPT, async (percent, message) =>
            {
                if (UserHub.UserList != null)
                {
                    foreach (var element in UserHub.UserList.Where(x => x.Id == appContext.UserId))
                        await hubLogContext.Clients.Client(element.ConnectionId).SendAsync("progress", new EventProgress
                        {
                            Percent = percent,
                            Message = message
                        });
                }
            });
        }

        [HttpPost("KpiTeamplateSummary")]
        [Right("kpi-summary")]
        public async Task<List<KPITemplate>> KpiTeamplateSummary(KPITemplateRequest request)
        {
            return await _manager.GetKpiTeamplate(request);
        }
    }
}