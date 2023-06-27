using DTO.Inspection;
using DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IReportManager
    {
        Task<IEnumerable<ReportProductItem>> GetProductsByBooking(int bookingId);
        Task<IEnumerable<ReportProductItem>> GetContainersByBooking(int bookingId);
        Task<CustomerReportSummaryResponseResult> BookingStatusUpdate(BookingStatusRequest request);
        Task<CustomerReportSummaryResponse> GetAllInspectionReports(InspectionSummarySearchRequest request);
        Task<ExportReportData> ExportReportDataSummary(InspectionSummarySearchRequest request);
        Task<UploadCustomReportResponse> UpdateCustomReport(UploadCustomReportRequest request);


        Task<InspectionOccupancySummaryResponse> GetInspectionOccupancySummary(InspectionOccupancySearchRequest request);

        Task<ExportInspectionOccupancySummaryResponse> ExportInspectionOccupanySummary(InspectionOccupancySearchRequest request);

        Task<CustomerReportDetailsResponse> GetCustomerReportDetails(CustomerReportDetailsRequest request);

    }
}