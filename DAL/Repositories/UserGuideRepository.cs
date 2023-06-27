using Contracts.Repositories;
using DTO.UserGuide;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserGuideRepository : Repository, IUserGuideRepository
    {
        public UserGuideRepository(API_DBContext context) : base(context)
        {

        }
        /// <summary>
        /// Get the User guide details (IQueryable)
        /// </summary>
        /// <returns></returns>
        public IQueryable<UgUserGuideDetail> GetUserGuideDetails()
        {
            return _context.UgUserGuideDetails.Where(x => x.Active.HasValue && x.Active.Value);
        }

        /// <summary>
        /// Get the user guides mapped with the given role ids and userguide ids
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userGuideIds"></param>
        /// <returns></returns>
        public async Task<List<int>> GetRoleUserGuideIds(List<int> roleIds, List<int> userGuideIds)
        {
            return await _context.UgRoles.Where(x => x.Active.HasValue && x.Active.Value && x.RoleId != null && x.UserGuideId != null
                            && roleIds.Contains(x.RoleId.Value) && userGuideIds.Contains(x.UserGuideId.Value)).Select(x => x.UserGuideId.Value).ToListAsync();
        }

        /// <summary>
        /// Get the user guide file url
        /// </summary>
        /// <param name="userGuideId"></param>
        /// <returns></returns>
        public async Task<string> GetUserGuideFile(int userGuideId)
        {
            return await _context.UgUserGuideDetails.Where(x => x.Active.HasValue && x.Active.Value && x.Id == userGuideId).Select(x => x.FileUrl).FirstOrDefaultAsync();
        }
    }
}
