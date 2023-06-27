using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ISharedFullBridgeManager
    {
        Task<DataSourceResponse> FbReportResultList();
    }
}
