using DTO.AuditDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IAuditDashboardManager
    {
        Task<AuditMapGeoLocation> GetAuditCountryGeoCode(AuditDashboardMapFilterRequest request);
        Task<AuditDashboardResponse> GetAuditDashboardSummary(AuditDashboardMapFilterRequest request);
        Task<ResultAnalyticsAuditDashboardResponse> GetServiceTypeAuditDashboard(AuditDashboardMapFilterRequest request);
        Task<OverviewAuditDashboardResponse> OverviewDashboardSearch(AuditDashboardMapFilterRequest request);
        Task<AuditDashboardChartExport> ExportServiceTypeChart(AuditDashboardMapFilterRequest request);
        Task<ResultAnalyticsAuditDashboardResponse> GetAuditTypeAuditDashboard(AuditDashboardMapFilterRequest request);
        Task<AuditDashboardChartExport> ExportAuditTypeChart(AuditDashboardMapFilterRequest request);
    }
}
