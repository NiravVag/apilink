using DTO.FullBridge;
using DTO.Inspection;
using DTO.Schedule;
using DTO.ScheduleJob;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IScheduleJobManager
    {
        Task<bool> SaveCulturaPackingInfo();
        Task<List<ScheduleTravelTariffEmail>> GetInActivatTravelTariffList();
        Task<bool> SaveScheduleAutoQcExpenseInfo(List<int> travelExpenseIds, List<int> foodExpenseIds);
        Task<List<QcExpenseEmailData>> GetQcExpenseList();
        Task<List<QcPendingExpenseData>> GetPendingQcExpenseList(QcPendingExpenseRequest request);
        Task<IQueryable<ClmTransaction>> GetAllClaims();

        Task<List<ScheduleClaimReminderEmailItem>> GetClaimMailDetail(IQueryable<ClmTransaction> claimQuery, int statusId);
        Task<List<string>> GetUserEmailsByIds(List<int> userids);

        Task<PushReportInfoToFastReportResponse> PushReportInfoToFastReport(string fbToken);
        Task<List<JobConfiguration>> getJobConfigurationList();
        Task UploadInspectionAttachementsAsZipToCloud(FileAttachmentToZipRequest request);
        Task<List<InspectionDetail>> ScheduleSkipMSchart();
        ScheduleSkipMSchartEmailResponse ScheduleSkipMSchartForCustomer(List<InspectionDetail> inspectionList);
        Task<List<ScheduleJobCsEmail>> ScheduleBookingCS();
        Task<List<JobConfiguration>> GetJobConfigurations(List<int> typeIds);

        Task ProcessInspectionFastReport(FastReportRequest request, string fbToken, string token);
        Task<List<BookingCsItem>> GetBookingCSItemList(List<int> bookingIds);
        Task<SchedulePlanningForCSResponse> SchedulePlanningForCS(string offices, string configureId);
        Task<List<SchedulePlanningFileDateForCS>> SchedulePlanningForCSFileData(SchedulePlanningForCS response);
        Task<InitialBookingExtractResponse> BBGInitialBookingExtractEmail();
    }
}
