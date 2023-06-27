using DTO.UserGuide;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IUserGuideRepository
    {
        IQueryable<UgUserGuideDetail> GetUserGuideDetails();
        Task<string> GetUserGuideFile(int userGuideId);
        Task<List<int>> GetRoleUserGuideIds(List<int> roleIds, List<int> userGuideIds);
    }
}
