using DTO.FullBridge;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IReportFastTransactionRepository : IRepository
    {
        Task<List<RepFastTransaction>> GetNotStartedOrErrorReportFastTransactions();
        Task<List<FbReportIdDto>> GetFbReportIdsByBookingIds(IEnumerable<int> reportIds);
    }
}
