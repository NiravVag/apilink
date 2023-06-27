using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IStartingPortRepository: IRepository
    {
        IQueryable<EcAutRefStartPort> GetAllStartingPort();
        Task<EcAutRefStartPort> GetStartingPortDataById(int Id);
        Task<bool> CheckifStartingPortDataExists(int Id, int cityId, string portName);
        Task<bool> CheckifStartingPortDataExistsByName(int Id, string portName);
    }
}
