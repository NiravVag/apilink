using DTO.FullBridge;
using DTO.RepoRequest.Audit;
using DTO.Schedule;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IFBReportManager
    {
        Task<Boolean> SaveMasterDataToFB(SaveScheduleRequest request, string fbToken, bool isRecreate);

        Task<bool> UpdateFBBookingDetails(int bookingId, string fbToken);

        Task<bool> UpdateFBProductDetails(int bookingId, string fbToken);

        Task<bool> CreateFBMission(int bookingId, string fbToken);

        Task<SaveMissionResponse> DeleteFBMission(int bookingId, int fbMissionId, string fbToken);

        Task<bool> CreateFBReport(int bookingId, string fbToken, int poDetailId);

        Task<DeleteReportResponse> DeleteFBReport(int bookingId, int fbReportId, string fbToken, int apiReportId);

        Task<UpdateReportResponse> FetchFBReport(int fbReportId, string fbToken, int apiReportId);

        Task<UpdateReportResponse> FetchBulkFBReport(int fbReportId, int apiReportId, string fbToken);

        Task<Boolean> SaveProductMasterDataToFB(int productId, int? entityId, string fbToken);

        Task<Boolean> SaveCustomerMasterDataToFB(int customerId, string fbToken, int entityId);

        Task<Boolean> SaveSupplierMasterDataToFB(int supplierId, string fbToken, int entityId);

        Task<Boolean> SaveFactoryMasterDataToFB(int factoryId, string fbToken, int entityId);

        Task<List<ReportIdData>> getReportIdsbyBookingServiceDates(DateTime startDate, DateTime endDate);

        Task<UpdateReportResponse> FetchFBReportByBooking(int bookingId, int option, string fbToken);
        Task<(bool isSuccess, string error)> SaveReportDataToFB(int fbReportId, string reportUrl, string fbToken);
        Task UpdateSentReportDateAndTime(List<int> bookingIds, string fbToken);
        Task<Boolean> SaveAuditMasterDataToFB(int auditId, int entityId, string fbToken);
        Task<Boolean> SaveAuditMissionDataToFB(AudTransaction audit, FbAuditData auditData, List<AuditCustomerContactData> customerContacts, List<AuditCustomerContactData> supplierContacts, string fbToken);
        Task<Boolean> CreateUserToFBMissionForAudit(AudTransaction auditData, string fbToken);
        Task<bool> CreateAuditReportRequest(AudTransaction auditData, string fbToken);
        Task<Boolean> DeleteAuditMission(AudTransaction audit, string fbToken);

        Task<bool> SaveAuditMissionUrlsDataToFb(AudTransaction audit, List<AudTranFileAttachment> audTranFileAttachments, string fbToken);

        Task<bool> SaveMissionUrlsDataToFb(InspTransaction bookingData, string fbToken);
        Task<bool> UpdateReportProductCategory(AudTransaction auditData, string fbToken);
    }
}
