using DTO.AuditDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IAuditDashboardRepository : IRepository
    {
        Task<List<AuditCountryGeoCode>> GetAuditCountryGeoCode(IEnumerable<int> auditIdlst);
        Task<List<AuditChartData>> GetAuditServiceTypeByQuery(IQueryable<int> auditIds);
    }
}
