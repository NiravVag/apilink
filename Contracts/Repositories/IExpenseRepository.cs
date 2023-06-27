using DTO.Expense;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IExpenseRepository : IRepository
    {
        /// <summary>
        /// GetExpenseTypeList
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EcExpensesType>> GetExpenseTypeList();


        IQueryable<EcExpensesType> GetExpenseTypes();
        /// <summary>
        /// Get expense Claim
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(EcExpencesClaim, IEnumerable<ExpenseClaimReceipt>)> GetExpenseClaim(int id);


        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EcReceiptFile> GetFile(int id);

        /// <summary>
        /// Get receipt files
        /// </summary>
        /// <param name="GuidList"></param>
        /// <returns></returns>
        Task<IEnumerable<EcReceiptFile>> GetReceptFiles(IEnumerable<Guid> GuidList);

        /// <summary>
        /// Get status list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EcExpClaimStatus>> GetStatusList();

        /// <summary>
        /// Set status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        Task<bool> SetStatus(int id, int statusId);

        /// <summary>
        /// Reject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<bool> Reject(int id, string comment);

        /// <summary>
        /// GetExpenseClaimTypeList
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EcExpenseClaimtype>> GetExpenseClaimTypeList();

        /// <summary>
        /// GetQCAssignedBookings
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ExpenseBookingDetail>> GetQCAssignedBookings(int userId, DateTime filterdate);

        /// <summary>
        /// GetAuditorAssignedBookings
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ExpenseBookingDetail>> GetAuditorAssignedBookings(int userId, DateTime filterdate);
        /// <summary>
        /// GetExpenseInspectionBookings
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetExpenseInspectionBookings(int staffId, int? expenseId, List<int> BookingIds);
        /// <summary>
        /// GetExpenseAuditBookings
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetExpenseAuditBookings(int staffId, int? expenseId, List<int> BookingIds);
        /// <summary>
        /// GetExpenseClaimById
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        Task<EcExpencesClaim> GetExpenseClaimById(int? expenseId);
        /// <summary>
        /// GetHRProfileByStaffId
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<IEnumerable<HrStaffProfile>> GetHRProfileByStaffId(int staffId);

        Task<List<ExpenseBookingDetail>> GetAuditbookingByExpenseId(int expenseId);

        Task<List<ExpenseBookingDetail>> GetInspbookingByExpenseId(int expenseId);

        Task<List<ExpenseBookingDetail>> GetAuditbookingsByExpenseId(int expenseId);

        Task<List<ExpenseBookingDetail>> GetInspbookingsByExpenseId(int expenseId);

        /// <summary>
        /// get expense claim details
        /// </summary>
        /// <param name="claimDetailIdList"></param>
        /// <returns></returns>
        Task<List<ExpenseClaimVoucherItem>> ExpenseClaimDetails(List<int> claimDetailIdList);

        //commented by ganesh-expense claim

        /// <summary>
        /// get booking data by expense Id
        /// </summary>
        /// <param name="claimDetailIdList"></param>
        /// <returns></returns>
        Task<List<ExpenseBookingData>> BookingDataByExpenseId(List<int> claimDetailIdList);

        /// <summary>
        /// Get audit data by claim id
        /// </summary>
        /// <param name="claim id"></param>
        /// <returns></returns>
        Task<List<ExpenseAuditData>> AuditDataByExpenseId(List<int> claimDetailIdList);
        Task<List<EcExpencesClaim>> GetExpenseClaimByQcAndBookingList(List<int> qcList, List<int> bookingList);
        Task<bool> CheckPendingFoodExpenseExist(int QcId, List<int> BookingList);
        Task<bool> CheckPendingTravelExpenseExist(int QcId, List<int> BookingList);
        Task<List<ExpenseBookingDetail>> GetEditExpenseQCAssignedBookings(int userId, DateTime filterdate);
        Task<List<ExpenseBookingDetail>> GetOutSourceQCBookingDetails(int outSourceCompanyId, DateTime minDate);
        Task<List<ExpenseBookingDetail>> GetEditOutSourceQCBookingDetails(int qcId, int outSourceCompanyId, DateTime minDate);
        Task<List<ExpenseClaim>> GetExpenseClaimListForEmail(List<int> expenseClaimIds);
        Task<int> GetQCByExpenseId(int expenseId);
        Task<bool> CheckExpenseClaimNoExist(string expenseClaimNo);

        Task<decimal> GetExpenseFoodAllowance(ExpenseFoodClaimRequest request);
        Task<List<ExpenseQCPort>> GetExpenseTravelPortList(List<int?> qcTravelExpenseIdList);

        Task<List<int?>> GetPendingFoodExpenseBookingIds(int QcId, List<int?> BookingIdList);
        Task<List<int?>> GetPendingTravelExpenseBookingIds(int QcId, List<int?> BookingIdList);
        IQueryable<EcExpencesClaim> GetIqueryableExpense();
        Task<List<ExpenseDetailsRepo>> GetExpenseDetailsListByExpenseId(List<int> expenseIdList);
        Task<List<int?>> GetExpenseBookingIdsForQC(int expenseClaimId, int staffId, List<int> bookingList);

        Task<List<int?>> GetEditExpenseBookingIdsForQC(int expenseClaimId, int staffId, List<int> bookingList, bool isAutoQCExpense);
    }
}
