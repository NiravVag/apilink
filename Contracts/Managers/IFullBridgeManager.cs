using DTO.FullBridge;
using DTO.Report;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IFullBridgeManager
    {
        /// <summary>
        /// Update the Fb Status by report id 
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FbStatusResponse> UpdateFBFillingAndReviewStatus(int reportId, FbStatusRequest request, bool fromBulkUpdate);
        /// <summary>
        /// Save the Report information by report id 
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FbStatusResponse> SaveFBReportDetails(int reportId, FbReportDataRequest request);
        Task<FbStatusResponse> SaveFBReportInfo(int reportId, int bookingId, FbReportDataRequest request);
        Task<IEnumerable<FbReportDetail>> GetFBReportDetails();
        Task<FbStatusResponse> DeleteFBReportDetails(int reportId);
        Task<FbStatusResponse> UpdateBookingStatusIfAllReportsValidated(int reportId, FbReportDataRequest request);
        Task<List<ReportIdData>> getReportIdsbyBooking(int bookingId);
        Task<List<ReportIdData>> getReportIdsbyBookingServiceDates(DateTime startDate, DateTime endDate);
        Task<List<ReportIdData>> getNonValidatedReportIdsbyBooking(int bookingId);
        Task<int> GetApiReportIdbyFbReport(int fbReportId);
        Task<List<FBReportDetails>> GetFbReportTitleListByReportIds(IEnumerable<int?> fbReportMapIdList);
    }
}
