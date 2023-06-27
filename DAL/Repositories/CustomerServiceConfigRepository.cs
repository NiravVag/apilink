using Contracts.Repositories;
using DTO.Customer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;
using DTO.CommonClass;

namespace DAL.Repositories
{
    public class CustomerServiceConfigRepository : Repository, ICustomerServiceConfigRepository
    {

        public CustomerServiceConfigRepository(API_DBContext context) : base(context)
        {
        }

        public Task<List<CuServiceType>> GetServiceTypeByCustomerID(int? CustomerId, int? ServiceId)
        {
            if (CustomerId != null && ServiceId != null)
            {
                return _context.CuServiceTypes
                 .Include(x => x.Customer)
                 .Include(x => x.Service)
                 .Include(x => x.ServiceType)
                 .Include(x => x.ProductCategory)
                 .Where(x => x.CustomerId == CustomerId && x.ServiceId == ServiceId && x.Active == true)
                 .ToListAsync();
            }
            else if (CustomerId != null)
            {
                return _context.CuServiceTypes
                 .Include(x => x.Customer)
                 .Include(x => x.Service)
                 .Include(x => x.ServiceType)
                 .Include(x => x.ProductCategory)
                 .Where(x => x.CustomerId == CustomerId && x.Active == true)
                 .ToListAsync();
            }
            else if (ServiceId != null)
            {
                return _context.CuServiceTypes
                 .Include(x => x.Customer)
                 .Include(x => x.Service)
                 .Include(x => x.ServiceType)
                 .Include(x => x.ProductCategory)
                 .Where(x => x.ServiceId == ServiceId && x.Active == true)
                 .ToListAsync();
            }

            return null;


        }

        public CuServiceType GetServiceTypeByServiceID(int? ServiceId)
        {
            if (ServiceId != null)
            {
                return _context.CuServiceTypes
                        .Where(x => x.Id == ServiceId && x.Active == true)
                        .SingleOrDefault();
            }
            return null;
        }

        public CuServiceType GetServiceTypeBySPST(int? ServiceID, int? ServiceTypeID, int CustomerID)
        {
            if (ServiceID != null && ServiceTypeID != null)
            {
                return _context.CuServiceTypes
                        .Where(x => x.ServiceId == ServiceID &&
                                    x.ServiceTypeId == ServiceTypeID
                                    && x.Active == true && x.CustomerId == CustomerID)
                        .SingleOrDefault();
            }
            else if (ServiceID != null && ServiceTypeID != null)
            {
                return _context.CuServiceTypes
                        .Where(x => x.ServiceId == ServiceID &&
                                    x.ServiceTypeId == ServiceTypeID
                                    && x.Active == true && x.CustomerId == CustomerID)
                        .SingleOrDefault();
            }
            return null;
        }

        public Task<int> AddCustomerServiceConfig(CuServiceType entity)
        {
            _context.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> EditCustomerServiceConfig(CuServiceType entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();

        }

        public async Task<bool> RemoveCustomerServiceConfig(int id)
        {
            var entity = await _context.CuServiceTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            entity.Active = false;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }

        public CuServiceType ServiceByCustomerAndServiceTypeID(int customerId, int serviceTypeId)
        {

            return _context.CuServiceTypes
                    .Where(x => x.ServiceTypeId == serviceTypeId 
                                && x.CustomerId == customerId && x.Active && x.ServiceId==(int)Service.InspectionId)
                    .FirstOrDefault();

        }

        public async Task<List<CommonDataSource>> GetCustomerServiceType(int customerId, int serviceId)
        {
            if(serviceId > 0)
            {
                return await _context.CuServiceTypes.Where(x => x.CustomerId == customerId && x.ServiceId == serviceId && x.Active)
                    .Select(x => new CommonDataSource
                    {
                        Id = x.Id,
                        Name = x.ServiceType.Name
                    }).AsNoTracking().ToListAsync();
            }


            else
            {
                return await _context.CuServiceTypes.Where(x => x.CustomerId == customerId && x.Active)
                    .Select(x => new CommonDataSource
                    {
                        Id = x.Id,
                        Name = x.ServiceType.Name
                    }).AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        /// Check service type mapped with the customer 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public async Task<bool> CheckServiceTypeMappedWithCustomer(int customerId,int serviceTypeId)
        {
            return await _context.CuServiceTypes.AnyAsync(x=>x.Active && x.CustomerId==customerId && x.ServiceTypeId==serviceTypeId);
        }
    }
}
