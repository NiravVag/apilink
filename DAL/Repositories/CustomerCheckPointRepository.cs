using Contracts.Repositories;
using DTO.Customer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerCheckPointRepository : Repository, ICustomerCheckPointRepository
    {
        public CustomerCheckPointRepository(API_DBContext context) : base(context)
        {
        }
        public Task<List<CuCheckPointType>> GetCheckPointType()
        {
            return _context.CuCheckPointTypes.Where(x => x.Active).ToListAsync();
        }
        public async Task<IEnumerable<CustomerCheckPoint>> GetCustomerCheckPoint()
        {
            return await _context.CuCheckPoints.Where(x => x.Active).Select(x => new CustomerCheckPoint()
            {
                Id = x.Id,
                CustomerName = x.Customer.CustomerName,
                CheckPointName = x.CheckpointType.Name,
                ServiceName = x.Service.Name,
                Remarks = x.Remarks,
                CustomerId = x.CustomerId,
                ServiceId = x.ServiceId,
                CheckPointId = x.CheckpointTypeId
            }).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<CustomerCheckPoint>> GetCusCPByCusServiceId(int? cusId, int? serviceId)
        {
            return await _context.CuCheckPoints.Where(x => x.CustomerId == cusId && x.ServiceId == serviceId && x.Active).Select(x => new CustomerCheckPoint()
            {
                Id = x.Id,
                CustomerName = x.Customer.CustomerName,
                CheckPointName = x.CheckpointType.Name,
                ServiceName = x.Service.Name,
                Remarks = x.Remarks,
                CustomerId = x.CustomerId,
                ServiceId = x.ServiceId,
                CheckPointId = x.CheckpointTypeId
            }).AsNoTracking().ToListAsync();

        }
        public async Task<IEnumerable<CustomerCheckPoint>> GetCusCPByCustomerId(int? cusId)
        {
            return await _context.CuCheckPoints.Where(x => x.CustomerId == cusId && x.Active).Select(x => new CustomerCheckPoint()
            {
                Id = x.Id,
                CustomerName = x.Customer.CustomerName,
                CheckPointName = x.CheckpointType.Name,
                ServiceName = x.Service.Name,
                Remarks = x.Remarks,
                CustomerId = x.CustomerId,
                ServiceId = x.ServiceId,
                CheckPointId = x.CheckpointTypeId
            }).AsNoTracking().ToListAsync();

        }
        public Task<int> SaveCustomerCP(CuCheckPoint entity)
        {
            _context.CuCheckPoints.Add(entity);
            return _context.SaveChangesAsync();
        }
        public Task<CuCheckPoint> GetCustomerCPbyId(int id)
        {
            return _context.CuCheckPoints
                .Include(x => x.CuCheckPointsBrands)
                .Include(x => x.CuCheckPointsDepartments)
                .Include(x => x.CuCheckPointsServiceTypes)
                .Include(x=> x.CuCheckPointsCountries)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public Task<int> UpdateCustomerCP(CuCheckPoint entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
        public Task<bool> IsRecordExists(CuCheckPoint entity)
        {
            return _context.CuCheckPoints.AnyAsync(x => x.CustomerId == entity.CustomerId &&
                x.CheckpointTypeId == entity.CheckpointTypeId &&
                    x.ServiceId == entity.ServiceId && x.Id != entity.Id && x.Active);
        }
        /// <summary>
        /// get checkpoint list based on customer list , service id, checkpoint list
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <param name="serviceId"></param>
        /// <param name="checkPointIdList"></param>
        /// <returns> checkpoint list stasify the condition</returns>
        public Task<List<CuCheckPoint>> GetCheckPointList(IEnumerable<int> customerIdList, int serviceId, IEnumerable<int> checkPointIdList)
        {
            return _context.CuCheckPoints.Where(x => customerIdList.Contains(x.CustomerId) &&
                        checkPointIdList.Contains(x.CheckpointTypeId) &&
                        x.ServiceId == serviceId && x.Active).ToListAsync();
        }

        public async Task<List<CuCheckPoint>> GetCustomerCheckPointByCustomer(List<int> customerIdList, int serviceId)
        {
            return await _context.CuCheckPoints.Where(x => x.Active && customerIdList.Contains(x.CustomerId) && x.ServiceId == serviceId)
                .Include(x => x.CuCheckPointsBrands)
                .Include(x => x.CuCheckPointsCountries)
                .Include(x => x.CuCheckPointsDepartments).ToListAsync();
        }

        //get checkpoint brands by checkpoint ids
        public async Task<List<CommonCheckPointDataSource>> GetCustomerCheckPointBrand(List<int> checkpointIdList)
        {
            return await _context.CuCheckPointsBrands.Where(x => x.Active && checkpointIdList.Contains(x.CheckpointId)).
                Select(x => new CommonCheckPointDataSource
                {
                    Id = x.BrandId,
                    CheckPointId = x.CheckpointId,
                    Name = x.Brand.Name
                }).ToListAsync();
        }

        //get checkpoint departments by checkpoint ids
        public async Task<List<CommonCheckPointDataSource>> GetCustomerCheckPointDept(List<int> checkpointIdList)
        {
            return await _context.CuCheckPointsDepartments.Where(x => x.Active && checkpointIdList.Contains(x.CheckpointId)).
                Select(x => new CommonCheckPointDataSource
                {
                    Id = x.DeptId,
                    CheckPointId = x.CheckpointId,
                    Name = x.Dept.Name
                }).ToListAsync();
        }

        //get checkpoint service type by checkpoint ids
        public async Task<List<CommonCheckPointServiceTypeDataSource>> GetCustomerCheckPointServiceType(List<int> checkpointIdList)
        {
            return await _context.CuCheckPointsServiceTypes.Where(x => x.Active && checkpointIdList.Contains(x.CheckpointId)).
                Select(x => new CommonCheckPointServiceTypeDataSource
                {
                    Id = x.ServiceTypeId,
                    CheckPointId = x.CheckpointId,
                    Name = x.ServiceType.ServiceType.Name,
                    ServiceTypeId = x.ServiceType.ServiceTypeId
                }).ToListAsync();
        }

        public async Task<bool> IsCustomerCheckpointConfigured(int customerId, int checkPointTypeId)
        {
            return await _context.CuCheckPoints.AsNoTracking().AnyAsync(x => x.CustomerId == customerId && x.Active && x.CheckpointTypeId == checkPointTypeId);
        }

        public async Task<List<CommonCheckPointDataSource>> GetCustomerCheckPointCountry(List<int> checkpointIdList)
        {
            return await _context.CuCheckPointsCountries.Where(x => x.Active && checkpointIdList.Contains(x.CheckpointId)).
                Select(x => new CommonCheckPointDataSource
                {
                    Id = x.CountryId,
                    CheckPointId = x.CheckpointId,
                    Name = x.Country.CountryName
                }).ToListAsync();
        }

        public async Task<CustomerCheckPoint> GetCustomerCheckpoint(int customerId,int serviceId,int checkpointTypeId)
        {
            return await _context.CuCheckPoints.Where(x => x.Active && x.CustomerId == customerId && x.ServiceId == serviceId &&
                       x.CheckpointTypeId == checkpointTypeId).Select(x => new CustomerCheckPoint()
                       {
                           Id = x.Id,
                           CustomerName = x.Customer.CustomerName,
                           CheckPointName = x.CheckpointType.Name,
                           ServiceName = x.Service.Name,
                           Remarks = x.Remarks,
                           CustomerId = x.CustomerId,
                           ServiceId = x.ServiceId,
                           CheckPointId = x.CheckpointTypeId
                       }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the customer check point list by customerid,serviceid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetCustomerCheckPointList(int customerId, int serviceId)
        {
            return await _context.CuCheckPoints.Where(x => x.CustomerId == customerId 
                    && x.ServiceId == serviceId && x.Active).Select(x => x.CheckpointTypeId).ToListAsync();

        }
    }
}
