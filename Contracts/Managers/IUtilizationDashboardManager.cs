using DTO.Manday;
using DTO.UtilizationDashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IUtilizationDashboardManager
    {
        Task<UtilizationResponse> GetCapacityUtilizationReport(UtilizationDashboardRequest request);
        Task<UtilizationExportResponse> GetCapacityUtilizationReportExport(UtilizationDashboardRequest request);
        Task<MandayYearChartResponse> GetMandayYearChart(UtilizationDashboardRequest request);
        Task<MandayYearExport> ExportMandayYearChart(UtilizationDashboardRequest request);
    }
}
