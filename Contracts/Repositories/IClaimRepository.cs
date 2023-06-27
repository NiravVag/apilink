using DTO.Claim;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Schedule;
using DTO.InvoicePreview;

namespace Contracts.Repositories
{
    public interface IClaimRepository : IRepository
    {
        Task<BookingClaimRepoData> GetBookingData(int bookingId);
        //Task<InspTransaction> GetBookingResponse(int inspectionId);
        Task<ClaimBookingData> GetBookingResponse(int inspectionId);
        Task<List<FbReportQuantityData>> GetFbQuantityData(int bookingId);
        IQueryable<InspTransaction> GetBookingIdDataSource();
        Task<List<CommonDataSource>> GetClaimFromList();
        Task<List<CommonDataSource>> GetReceivedFromList();
        Task<List<CommonDataSource>> GetClaimSourceList();
        Task<List<CommonDataSource>> GetFbDefectList();
        Task<List<CommonDataSource>> GetClaimDepartmentList();
        Task<List<CommonDataSource>> GetClaimCustomerRequestList();
        Task<List<CommonDataSource>> GetClaimPriorityList();
        Task<List<CommonDataSource>> GetClaimRefundTypeList();
        Task<List<CommonDataSource>> GetClaimDefectDistributionList();
        Task<List<CommonDataSource>> GetClaimResultList();
        Task<List<CommonDataSource>> GetCurrencies();
        Task<List<CommonDataSource>> GetClaimFileTypeList();
        int AddClaim(ClmTransaction entity);
        Task<int> EditClaim(ClmTransaction entity);
        //void RemoveEntities<T>(IEnumerable<T> entities) where T : class, new();
        Task<ClaimData> GetClaimDetailsByClaimId(int claimId);
        Task<ClmTransaction> GetClaimDetails(int claimId);
        Task<List<BookingClaims>> GetClaimByBookingId(int inspectionNo);
        Task<ItUserMaster> GetUserName(int userId);
        Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId);
        Task<List<CommonDataSource>> GetClaimFinalResultList();
        Task<List<string>> GetClaimFinalResultListByIds(List<int> claimFinalDecisionids);
        Task<InvoiceDetail> GetClaimInvoiceDetails(int inspectionNo);
        Task<IEnumerable<ClmRefStatus>> GetClaimStatusList();
        Task<IQueryable<ClmTransaction>> GetAllClaimsQuery();
        Task<List<InspectionQcDetail>> GetQcDetails(IEnumerable<int> claimIds);
        Task<List<InvoiceDetail>> GetClaimInvoiceDetailByBookingIds(List<int> bookingIds);
        Task<List<FbReportQuantityData>> GetFbQuantityDataByBookingIds(List<int> bookingIds);

        IQueryable<PendingClaimRepoItem> GetPendingClaims();

        IQueryable<CreditNoteSummaryRepoItem> GetCreditNotes();

        Task<List<CommonDataSource>> GetCreditTypeList();
        Task<bool> IsCreditNoExist(string creditNo);
        Task<List<CreditNoteClaimRepoItem>> GetCreditClaimDetailsByCreditIds(IEnumerable<int?> ids);
        Task<List<InvCreTranContact>> GetCreditContacts(IEnumerable<int?> creditIds);
        Task<List<InvCreTranClaimDetail>> GetCreditTranClaimDetails(IEnumerable<int> creditIds);
        Task<List<CreditNoteDetailsRepoItem>> GetCreditNoteTransactionDetails(IEnumerable<int> creditIds);
        IQueryable<ExportCreditNoteSummaryRepoItem> GetExportCreditNotes();
        Task<IEnumerable<ClmTranReport>> GetClaimReportList(List<int> claimIds);
        Task<List<InvoiceDetailsRepo>> GetCreditNoteDetails(string creditNo);

        Task<List<InvoiceCreditDetails>> GetInvoiceCreditDetailsByBookingIds(IQueryable<int> bookingIds);
    }
}
