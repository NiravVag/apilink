using DTO.StartingPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IStartingPortManager
    {
        Task<StartingPortSaveResponse> SaveStartingPort(StartingPortRequest request);
        Task<StartingPortSaveResponse> UpdateStartingPort(StartingPortRequest request);
        Task<StartingPortSaveResponse> DeleteStartingPort(int id);
        Task<StartingPortResponse> GetStartingPortSummary(StartingPortSearchRequest request);
        Task<StartingPortEditResponse> GetStartingPortDetails(int id);
    }
}
