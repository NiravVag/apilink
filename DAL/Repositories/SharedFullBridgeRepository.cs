using Contracts.Repositories;
using DTO.Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class SharedFullBridgeRepository: Repository, ISharedFullBridgeRepository
    {
        public SharedFullBridgeRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// Get the FB Report Results Master Data
        /// </summary>
        /// <returns></returns>
        public async Task<List<FBReportResultData>> GetFbReportResults()
        {
            return await _context.FbReportResults.Where(x => x.Active.HasValue && x.Active.Value).
                        Select(x =>
                        new FBReportResultData
                        {
                            ResultId = x.Id,
                            ResultName = x.ResultName
                        }).AsNoTracking().ToListAsync();
        }
    }
}
