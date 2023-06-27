using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class StartingPortRepository: Repository, IStartingPortRepository
    {
        public StartingPortRepository(API_DBContext context): base(context)
        {

        }
        public IQueryable<EcAutRefStartPort> GetAllStartingPort()
        {
            return _context.EcAutRefStartPorts.Where(x => x.Active.Value);
        }

        public async Task<EcAutRefStartPort> GetStartingPortDataById(int Id)
        {
            return await _context.EcAutRefStartPorts.Where(x => x.Active.Value && x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckifStartingPortDataExists(int Id, int cityId, string portName)
        {
            return await _context.EcAutRefStartPorts.Where(x => x.Active.Value && x.Id != Id && x.CityId == cityId && x.StartPortName == portName).AnyAsync();
        }
        public async Task<bool> CheckifStartingPortDataExistsByName(int Id, string portName)
        {
            return await _context.EcAutRefStartPorts.Where(x => x.Active.Value && x.Id != Id && x.StartPortName == portName).AnyAsync();
        }
    }
}
