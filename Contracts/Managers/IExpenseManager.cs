using DTO.Expense;
using DTO.File;
using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IExpenseManager
    {
        /// <summary>
        /// Get expense claim 
        /// </summary>
        /// <returns></returns>
        Task<ExpenseClaimResponse> GetExpenseClaim(string name, int? id);


        /// <summary>
        /// Get cities
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        Task<ClaimCitiesResponse> GetClaimCities(string term);


        /// <summary>
        /// Get File
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FileResponse> GetFile(int id);

        /// <summary>
        /// Save expense claim
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveExpenseClaimResponse> SaveExpenseClaim(ExpenseClaim request);

        Task<SaveExpenseClaimResponse> AddOutSourceQCExpenseClaim(ExpenseClaim request);

        Task<SaveExpenseClaimResponse> UpdateExpenseClaim(ExpenseClaim request);

        /// <summary>
        /// Upload files
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        Task  UploadFiles(Dictionary<Guid, byte[]> fileList);

        /// <summary>
        /// Get expense summary
        /// </summary>
        /// <returns></returns>
        Task<ExpenseClaimSummaryResponse> GetExpenseSummary();

        /// <summary>
        /// Get expense claim list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ExpenseClaimListResponse> GetExpenseClaimList(ExpenseClaimListRequest request);

        /// <summary>
        /// Set expense status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        Task<SetExpenseStatusResponse> SetExpenseStatus(int id, int statusId);

        /// <summary>
        /// Reject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<SetExpenseStatusResponse> Reject(int id, string comment);

        /// <summary>
        /// GetExpenseClaimTypeList
        /// </summary>
        /// <returns></returns>
        Task<ExpenseClaimTypeResponse> GetExpenseClaimTypeList();

        Task<ExpenseBookingDetailResponse> GetOutSourceQCBookingDetails(int? expenseId);

        /// <summary>
        /// GetExpenseBookingDetails
        /// </summary>
        /// <param name="claimTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        Task<ExpenseBookingDetailResponse> GetExpenseBookingDetails(int claimTypeId, int? expenseId, bool isEdit);

        /// <summary>
        /// Fetch the user based data for voucher export
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ExpenseClaimVoucherData> ExportVocherSummary(ExpenseClaimListRequest request);

        /// <summary>
        /// Fetch the user based data for expense summary kpi export
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ExportExpenseClaimSummaryKpiResponse> ExportExpenseKpiSummary(ExpenseClaimListRequest request);

        Task<bool> CheckPendingExpenseExist(PendingExpenseRequest request);

        Task<ExpenseFoodClaimResponse> GetExpenseFoodAmount(ExpenseFoodClaimRequest request);

        Task<ExportExpenseSummaryKpiResponse> ExportExpenseDetailKpiSummary(ExpenseClaimListRequest request);

        Task<PendingBookingExpenseResponse> GetFoodOrTravelPendingExpenseBookingIdList(List<PendingBookingExpenseRequest> request);
    }
}
