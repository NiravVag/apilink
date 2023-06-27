using DTO.CommonClass;
using DTO.Manday;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IMandayManager
    {
        Task<MandayDashboardResponse> GetMandayDashboardSearch(MandayDashboardRequest request);
        Task<DataSourceResponse> GetServices();
        Task<DataSourceResponse> GetOfficeLocations();
        Task<MandayYearChartResponse> GetMandayYearChart(MandayDashboardRequest request);
        Task<MandayCustomerChartResponse> GetMandayCustomerChart(MandayDashboardRequest request); 
        Task<MandayCountryChartResponse> GetMandayCountryChart(MandayDashboardRequest request); 
        Task<MandayEmployeeTypeChartResponse> GetManDayEmployeeTypeChart(MandayDashboardRequest request);
        Task<MandayYearExport> ExportMandayYearChart(MandayDashboardRequest request); 
        Task<MandayCountryChartExportResponse> ExportMandayCountryChart(MandayDashboardRequest request);
        Task<MandayEmployeeTypeChartExport> ExportManDayEmployeeTypeChart(MandayDashboardRequest request);
        Task<MandayDashboardRequestExport> SetExportFilter(MandayDashboardRequest request);
        Task<MandayCustomerChartExportResponse> ExportMandayCustomerChart(MandayDashboardRequest request);
    }
}
