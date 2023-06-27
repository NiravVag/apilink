using Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserConfigRepository : Repository, IUserConfigRepository
    {
        public UserConfigRepository(API_DBContext context) : base(context)
        {

        }

        //Get dausercustomer  and DaUserRoleNotificationByOffices data
        public async Task<IEnumerable<DaUserCustomer>> GetDaUserCustomerMasterData(int userId)
        {
            return await _context.DaUserCustomers
                .Include(x => x.DaUserRoleNotificationByOffices)
                .Include(x => x.DaUserByProductCategories)
                .Include(x => x.DaUserByRoles)
                .Include(x => x.DaUserByServices)
                .Include(x => x.DaUserByDepartments)
                .Include(x => x.DaUserByBrands)
                //.Include(x => x.DaUserByBuyers)
                .Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration()
        {
            return await _context.EntMasterConfigs.Where(x => x.Active.Value).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the master congig query
        /// </summary>
        /// <returns></returns>
        public IQueryable<EntMasterConfig> GetMasterConfigurationQuery()
        {
            return _context.EntMasterConfigs.Where(x => x.Active.Value);
        }

        /// <summary>
        /// Get the master config query
        /// </summary>
        /// <returns></returns>
        public async Task<List<EntMasterConfig>> GetMasterConfigurationByTypeIds(List<int> masterTypeIds)
        {
            return await _context.EntMasterConfigs.Where(x => x.Active.Value && masterTypeIds.Contains(x.Type.GetValueOrDefault()))
               .AsNoTracking().ToListAsync();
        }

    }
}
