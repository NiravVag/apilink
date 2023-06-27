using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IUserConfigRepository : IRepository
    {
        /// <summary>
        /// Get dausercustomer  and DaUserRoleNotificationByOffices data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> DaUserCustomer data</returns>
        Task<IEnumerable<DaUserCustomer>> GetDaUserCustomerMasterData(int userId);

        //Task SaveOrModifyList(IEnumerable<DaUserCustomer> entityList, bool isEdit = true);

        Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration();

        IQueryable<EntMasterConfig> GetMasterConfigurationQuery();

        Task<List<EntMasterConfig>> GetMasterConfigurationByTypeIds(List<int> masterTypeIds);
    }
}
