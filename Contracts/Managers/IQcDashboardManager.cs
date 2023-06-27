using DTO.Dashboard;
using DTO.DataAccess;
using DTO.Manday;
using DTO.MobileApp;
using DTO.QcDashboard;
using DTO.Schedule;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{

    public interface IQcDashboardManager
    {

        /// <summary>
        /// Get the schdule details
        /// </summary>
        /// <returns></returns>
        Task<QcDashboardCalendarResponse> GetQcScheduleDetails();
        /// <summary>
        /// Get the QC Productivity Details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<QcDashboardReportsResponse> GetQcProductivityDetails(QcDashboardSearchRequest request);
        /// <summary>
        /// Get the Qc Rejection Details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<QcRejectionReportsResponse> GetQcRejectionDetails(QcDashboardSearchRequest request);
        /// <summary>
        /// Get the Qc Dashboard Count Details
        /// </summary>
        ///<param name="request"></param>
        /// <returns></returns>
        Task<QcDashboardCountResponse> GetQcDashboardCountDetails(QcDashboardSearchRequest request);

    }
}
