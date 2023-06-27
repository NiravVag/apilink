using Contracts.Repositories;
using DTO.Lab;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LabRepository : Repository, ILabRepository
    {
        public LabRepository(API_DBContext context) : base(context)
        {
        }

        public IEnumerable<InspLabDetail> GetAllLabDetails()
        {
            return _context.InspLabDetails.Where(x => x.Active.Value)
                .Include(x => x.InspLabAddresses)
                .ThenInclude(x => x.Country)
                .Include(x => x.InspLabAddresses)
                .ThenInclude(x => x.City)
                .Include(x => x.InspLabAddresses)
                .ThenInclude(x => x.Province)
                .Include(x => x.InspLabContacts)
				.ThenInclude(x => x.InspLabCustomerContacts)
				.Include(x => x.InspLabCustomers)
                .ThenInclude(x => x.Customer)
                .Include(x => x.Type)
                .Include(x => x.InspLabCustomerContacts);
		}

        public IEnumerable<InspLabDetail> GetAllLab()
        {
            return _context.InspLabDetails
                .Include(x => x.InspLabCustomers)
                .Where(x => x.Active.Value);
        }

        public IEnumerable<InspLabAddressType> GetAllLabAddressType()
		{
			return _context.InspLabAddressTypes;
		}

        public IEnumerable<InspLabType> GetAllLabType()
        {
            return _context.InspLabTypes;
        }

        public Task<int> AddLabDetails(InspLabDetail entity)
        {
            _context.InspLabDetails.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> EditLabDetails(InspLabDetail entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return _context.SaveChangesAsync();
        }

        public Task<List<InspLabType>> GetLabTypes()
        {
            return _context.InspLabTypes.ToListAsync();
        }

        public Task<InspLabDetail> GetLabDetailsById(int? labId)
        {
            return _context.InspLabDetails
                .Include(x => x.InspLabAddresses)
                .Include(x => x.InspLabContacts)
                .ThenInclude(x => x.InspLabCustomerContacts)
                .ThenInclude(x => x.Customer)
                .Include(x => x.InspLabCustomers)
                .ThenInclude(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == labId);
        }
        /// <summary>
        /// Get Lab Addresses by Lab Id
        /// </summary>
        /// <param name="labId"></param>
        /// <returns></returns>
        public IEnumerable<InspLabAddress> GetLabAddressByLabId(int? labId)
        {
            return _context.InspLabAddresses.Where(x => x.LabId == labId);
        }
        /// <summary>
        /// Get Lab contacts by Lab and Customer
        /// </summary>
        /// <param name="labId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<InspLabContact> GetLabContactByLabIdAndCustomerId(int? labId, int? customerId)
        {
            return _context.InspLabContacts
                   .Include(x => x.InspLabCustomerContacts)
                   .Where(x=>x.InspLabCustomerContacts.Any(y=>y.CustomerId==customerId) && x.LabId==labId);
        }

        /// <summary>
        /// Get the lab address details by lab id list
        /// </summary>
        /// <param name="labIds"></param>
        /// <returns></returns>
        public IQueryable<InspLabAddress> GetLabAddressByLabIdList(List<int?> labIds)
        {
            return _context.InspLabAddresses.Where(x => labIds.Contains(x.LabId));
        }

        /// <summary>
        /// Get Lab Contact by lab id lis and customerid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IQueryable<InspLabContact> GetLabContactByLabIdListAndCustomerId(LabContactRequest request)
        {
            return _context.InspLabContacts
                   .Where(x => x.Active.Value && 
                               x.InspLabCustomerContacts.Any(y => y.CustomerId == request.CustomerId) 
                               && request.LabIdList.Contains(x.LabId));
        }

        /// <summary>
        /// Get the lab detail by lab id
        /// </summary>
        /// <param name="labId"></param>
        /// <returns></returns>
        public Task<InspLabDetail> GetLabDetailById(int? labId)
        {
            return _context.InspLabDetails
                .FirstOrDefaultAsync(x => x.Id == labId);
        }
    }
}
