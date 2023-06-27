using DTO.Claim;
using DTO.CommonClass;
using DTO.Customer;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IClaimManager
    {
        Task<ClaimSummaryResponse> GetClaimSummary();
        Task<ClaimSearchResponse> GetClaimDetails(ClaimSearchRequest request);
        Task<ClaimBookingResponse> GetBookingData(BookingClaimRequest request);
        Task<DataSourceResponse> GetBookingIdDataSource(CommonBookingIdSourceRequest request);
        Task<DataSourceResponse> GetReportListByBooking(int bookingId);
        Task<DataSourceResponse> GetClaimFromList();
        Task<DataSourceResponse> GetReceivedFromList();
        Task<DataSourceResponse> GetClaimSourceList();
        Task<DataSourceResponse> GetFbDefectList();
        Task<DataSourceResponse> GetClaimDepartmentList();
        Task<DataSourceResponse> GetClaimCustomerRequestList();
        Task<DataSourceResponse> GetClaimPriorityList();
        Task<DataSourceResponse> GetClaimRefundTypeList();
        Task<DataSourceResponse> GetClaimDefectDistributionList();
        Task<DataSourceResponse> GetClaimResultList();
        Task<DataSourceResponse> GetClaimFinalResultList();
        Task<DataSourceResponse> GetCurrencies();
        Task<DataSourceResponse> GetClaimFileTypeList();
        Task<SaveClaimResponse> SaveClaim(ClaimDetails request);
        Task<EditClaimResponse> GetEditClaim(int id);
        Task<BookingClaimsResponse> GetClaimsByBookingId(int bookingId);
        Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId);
        Task<ClaimEmailRequest> GetClaimMailDetail(int claimId, int bookingId, IEnumerable<int> reportIdList, bool? isEdit);
        Task<User> GetUserInfo(int userId);
        Task<InvoiceResponse> GetClaimInvoiceByBooking(int bookingId);
        Task<List<ClaimItemExportSummary>> ExportSummary(IEnumerable<ClaimExportItem> data);
        Task<ClaimCancelResponse> CancelClaim(int id);
        Task<ClaimSearchExportResponse> GetExportClaimDetails(ClaimSearchRequest request);

        Task<PendingClaimSummaryResponse> GetPendingClaims(PendingClaimSearchRequest request);
        Task<SaveCreditNoteResponse> SaveCreditNote(SaveCreditNote request);
        Task<GetPendingClaimResponse> GetPendingClaimData(IEnumerable<int> ids);
        Task<CreditNoteSummaryResponse> GetCreditNoteSummary(CreditNoteSearchRequest request);
        Task<DataSourceResponse> GetCreditTypeList();
        Task<bool> CheckCreditNumberExist(string creditNo);
        Task<EditCreditNoteResponse> EditCreditNote(int id);
        Task<List<ExportCreditNoteSummary>> ExportCreditNoteSummary(CreditNoteSearchRequest request);
        Task<DeleteCreditNoteResponse> DeleteCreditNote(int id);
        Task<SaveCreditNoteResponse> UpdateCreditNote(SaveCreditNote request);
        Task<DataSourceResponse> GetCreditNoteNos();
    }
}
