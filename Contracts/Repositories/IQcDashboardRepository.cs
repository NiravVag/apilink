using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Common;
using DTO.CommonClass;
using DTO.QcDashboard;
using DTO.Quotation;
using DTO.Schedule;
using DTO.UserAccount;
using Entities;

namespace Contracts.Repositories
{
    public interface IQcDashboardRepository : IRepository
    {
        /// <summary>
        /// Returns all the qc inspections to the Qc dashboard page
        /// </summary>        
        /// <returns></returns>
        IQueryable<ScheduleBookingInfo> GetQcInspections();
        Task<IEnumerable<QcDashboardCountData>> GetScheduelQcDetails(int qcId, DateTime fromDate, DateTime toDate);
        /// <summary>
        /// Returns all the qc report count to the Qc dashboard page
        /// </summary>        
        /// <returns></returns>
        IQueryable<QcReports> GetQcReport(List<int> bookingIds);
        Task<IEnumerable<QcReports>> GetQcReportsDetails(int qcId, List<int> bookingIds);
        /// <summary>
        /// Returns all the rejection report count
        /// </summary>        
        /// <returns></returns>
        Task<int> GetAllRejectionReport(DateTime fromDate, DateTime toDate);
        Task<int> GetQcRejectionReport(int qcId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Returns QC  rejection report count
        /// </summary>        
        /// <returns></returns>
        // Task<int> GetQcRejectionReport(int qcId, DateObject fromDate, DateObject toDate);


    }
}
