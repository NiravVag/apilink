using DTO.Invoice;
using DTO.Kpi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IKpiCustomManager
    {
        Task<KpiResponse> GetKpisummary();
        Task<ExportResult> ExportEciTemplate(KpiRequest request);
        Task<ExportResult> ExportWarehouseTemplate(KpiRequest request);


        /// <summary>
        /// get inspection, quotation, report details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>export the data</returns>
        Task<ExportResult> ExportGifiTemplate(KpiRequest request);
        Task<List<AdeoEanTemplate>> ExportAdeoEanCodeTemplate(KpiRequest request);
        Task<ExportResult> ExportAdeoInspSummaryByQATemplate(KpiRequest request);
        Task<ExportResult> ExportQcPerformanceBreakdownTemplate(KpiRequest request);
        Task<ExportResult> ExportMonthlyOrderStatementTemplate(KpiRequest request);
        Task<ExportResult> ExportAdeoSummaryRemarksTemplate(KpiRequest request);
        /// <summary>
        /// get inspection, quotation, report details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>export the data</returns>
        Task<ExportResult> ExportECIRemarksTemplate(KpiRequest request);
        Task<ExportResult> ExportInspDefectSummaryTemplate(KpiRequest request);
        Task<RemarksExportResult> ExportInspRemarksSummaryTemplate(KpiRequest request);
        Task<ExportResult> ExportLiverpoolTemplate(KpiRequest request);
        Task<List<ExpenseTemplateItem>> ExportInspExpenseSummaryTemplate(KpiRequest request);
        Task<ExportResult> ExportWeeklyBookingStatus(KpiRequest request);
        /// <summary>
        /// get inspection, invoice, report details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>export the data</returns>
        Task<DataTable> ExportCarreFourInvoiceTemplate(KpiRequest request);

        Task<DataTable> GetGeneralInvoiceTemplate(KpiRequest request);
        Task<List<CarreFourECOPackTemplate>> ExportCarrefourEcopackTemplate(KpiRequest request);
        Task<ExportResult> ExportMdmDefectTemplate(KpiRequest request);
        Task<List<InspectionPickingReport>> GetInspectionPickingData(KpiRequest request);
        Task<ExportResult> ExportInspectionPickingData(KpiRequest request);
        Task<DataTable> ExportInspResultSummaryTemplate(KpiRequest request);
        
        IQueryable<int> GetBookingIdAsQueryable(KpiRequest request);
        Task<DefectExportResult> ExportInspDefectSummaryTemplateByEfCore(KpiRequest request);
        Task<List<AdeoFollowUpTemplate>> ExportAdeoInspFollowUpTemplateByEfCore(KpiRequest request);
        Task<List<AdeoFranceInspSummaryTemplate>> ExportAdeoFranceInspSummaryTemplateByEfCore(KpiRequest request);
        Task<List<AdeoMonthInspSumbySubconFactoTemplate>> ExportAdeoMonthInspSumbySubconFactoTemplateByEfCore(KpiRequest request);
        Task<List<AdeoInspSumOverallTemplate>> ExportAdeoInspSumOverallTemplateByEfCore(KpiRequest request);
        Task<List<AdeoFailedPoTemplate>> ExportAdeoFailedPoTemplateByEfCore(KpiRequest request);
        Task<List<KpiReportCommentsTemplate>> ExportInspCommentSummaryTemplate(KpiRequest request);
        Task<DataTable> ExportOrderStatusLog(KpiRequest request);
        Task<DataTable> ExportInspectionData(KpiRequest request);
        Task<List<KPITemplate>> GetTemplateList();
        Task<List<KPITemplate>> GetKpiTeamplate(KPITemplateRequest request);
        Task<List<CustomerCulturaTemplate>> ExportCustomerCultura(KpiRequest request);
        Task<DataTable> ExportECITemplate(KpiRequest request);
        Task<List<ScheduleAnalysisTemplate>> ExportScheduleAnalysisTemplate(KpiRequest request);
        Task<List<InspectionSummaryQCTemplate>> ExportInspectionSummaryQCTemplate(KpiRequest request);
        Task<DataTable> ExportCustomerMandaySummaryTemplate(KpiRequest request);
        Task<XeroInvoiceResponse> ExportXeroxInvoiceTemplate(KpiRequest request);
        Task<XeroInvoiceResponse> ExportXeroExpenseTemplate(KpiRequest request);
        Task<DataTable> ExportGapProductRefLevel(KpiRequest request);
        Task<DataTable> ExportARFollowUpReport(KpiRequest request);
        Task<DataTable> ExportGapFlashProcessAudit(KpiRequest request);

        Task<DataTable> ExportGapProcessAudit(KpiRequest request);

        Task<DataTable> ExportExpenseInspectionSummaryTemplate(KpiRequest request);
    }
}
