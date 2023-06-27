using DTO.AuditReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
   public interface IAuditCusReportRepository:IRepository
    {
        /// <summary>
        /// Search Audit customer Report
        /// </summary>
        /// <param name=""></param>
        /// <returns>iqueryable list</returns>
        IQueryable<AuditRepoCusReportBookingDetails> SearchAuditCusReport();

        /// <summary>
        /// Get ServiceType based on BookingId
        /// </summary>
        /// <param name=""></param>
        /// <returns>i</returns>
        Task<List<AuditServiceType>> GetAuditserviceType(List<int>lstbookId);

        /// <summary>
        /// Is audit report exists or not
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<List<AuditcusReport>> IsAuditReportExists(List<int> lstbookId);
        Task<List<AuditFactoryCountry>> GetFactorycountryByAuditIds(IEnumerable<int> auditIds);
    }
}
