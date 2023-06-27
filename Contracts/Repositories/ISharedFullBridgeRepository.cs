using DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ISharedFullBridgeRepository
    {
        Task<List<FBReportResultData>> GetFbReportResults();
    }
}
