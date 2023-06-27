using Contracts.Repositories;
using DTO.FullBridge;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ReportFastTransactionRepository : Repository, IReportFastTransactionRepository
    {
        public ReportFastTransactionRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<List<RepFastTransaction>> GetNotStartedOrErrorReportFastTransactions()
        {
            return await _context.RepFastTransactions.Where(x => x.Status == (int)ReportFastStatus.NotStarted || (x.Status == (int)ReportFastStatus.Error && x.TryCount < 3))
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<FbReportIdDto>> GetFbReportIdsByBookingIds(IEnumerable<int> reportIds)
        {
            return await _context.FbReportDetails.Where(x => reportIds.Contains(x.Id) && x.Active == true)
                .Select(x => new FbReportIdDto
                {
                    ReportId = x.Id,
                    FbReportId = x.FbReportMapId,
                })
                .AsNoTracking().ToListAsync();
        }
    }
}
