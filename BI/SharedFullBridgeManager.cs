using Contracts.Managers;
using Contracts.Repositories;
using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class SharedFullBridgeManager: ISharedFullBridgeManager
    {

        private readonly ISharedFullBridgeRepository _repo = null;

        public SharedFullBridgeManager(ISharedFullBridgeRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// get fb result list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> FbReportResultList()
        {
            var response = await _repo.GetFbReportResults();

            if (response == null || !response.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = response.ConvertAll(x => new CommonDataSource { Id = x.ResultId, Name = x.ResultName }),
                Result = DataSourceResult.Success
            };
        }
    }
}
