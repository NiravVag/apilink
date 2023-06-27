using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerContactRepository : Repository, ICustomerContactRepository
    {
        public CustomerContactRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<bool> RemoveCustomerContact(int id)
        {
            var entity = await _context.CuContacts.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            entity.Active = false;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }

        public Task<List<CuContact>> GetCustomerContacts(int? CustomerId)
        {
            return _context.CuContacts
                 .Include(z => z.CuContactBrands)
                 .ThenInclude(y => y.Brand)
                 .Include(z => z.CuContactDepartments)
                 .ThenInclude(y => y.Department)
                 .Include(z => z.CuContactServices)
                 .ThenInclude(y => y.Service)
                 .Include(x => x.Customer)
                 .Include(x => x.CuCustomerContactTypes)
                 .Include(x => x.CuContactBrands)
                 .Include(x => x.CuContactDepartments)
                 .Include(x => x.CuCustomerContactTypes).ThenInclude(x => x.ContactType)
                 .Where(x => x.CustomerId == CustomerId) //&& x.Active.Value)
                 .OrderBy(x => x.ContactName).ToListAsync();
        }

        public CuCustomer GetCustomerContactsByID(int? customerID)
        {
            return _context.CuCustomers
                            .Where(x => x.Id == customerID && x.Active.HasValue && x.Active.Value)
                            .Include(z => z.CuContacts)
                            .ThenInclude(z => z.CuContactBrands)
                            .ThenInclude(y => y.Brand)
                            .Include(z => z.CuContacts)
                            .ThenInclude(z => z.CuContactDepartments)
                            .ThenInclude(y => y.Department)
                            .Include(z => z.CuContacts)
                            .ThenInclude(z => z.CuContactServices)
                            .ThenInclude(y => y.Service)
                            .SingleOrDefault();
        }

        public CuContact GetCustomerContactByContactID(int? contactID)
        {
            return _context.CuContacts.
                Where(x => x.Id == contactID)
                .Include(z => z.CuCustomerContactTypes)
                .Include(x => x.CuContactBrands)
                .Include(x => x.CuContactDepartments)
                .Include(x => x.CuContactServices)
                .Include(x => x.CuContactEntityMaps)
                .Include(x => x.CuContactEntityServiceMaps)
                .ThenInclude(x => x.Service)
                .Include(x => x.CuContactEntityServiceMaps)
                .ThenInclude(x => x.Entity)
                .Include(x => x.CuContactSisterCompanies)
                .SingleOrDefault();
        }

        public IEnumerable<CuAddress> GetCustomerAddressByCustomerID(int? customerID)
        {
            return _context.CuAddresses.Where(x => x.Active.HasValue && x.Active.Value)
                    .Where(x => x.CustomerId == customerID);
        }

        public IEnumerable<CuContactType> GetContactTypes()
        {
            return _context.CuContactTypes.
                    Include(x => x.CuCustomerContactTypes).
                    ToList();
        }

        public int AddCustomerContact(CuContact entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity.Id;

        }

        public int EditCustomerContact(CuContact entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity.Id;

        }

        public Task<List<CuContact>> GetCustomerContactsList(List<int> cuscontactids)
        {
            return _context.CuContacts.Where(x => x.Active != null && x.Active.Value && cuscontactids.Contains(x.Id)).ToListAsync();
        }

        public int SaveContactBrand(CuContactBrand entity)
        {
            _context.CuContactBrands.Add(entity);
            return _context.SaveChanges();
        }

        public int SaveContactDepartment(CuContactDepartment entity)
        {
            _context.CuContactDepartments.Add(entity);
            return _context.SaveChanges();
        }

        public int SaveContactService(CuContactService entity)
        {
            _context.CuContactServices.Add(entity);
            return _context.SaveChanges();
        }

        public async Task<CuContact> GetCustomerContacts(int customerId, int contactID)
        {
            return await _context.CuContacts.
                Where(x => x.Id == contactID && x.CustomerId == customerId)
                .SingleOrDefaultAsync();
        }
        /// <summary>
        /// get the customer contact by zoho customerid and contactid
        /// </summary>
        /// <param name="zohoCustomerID"></param>
        /// <param name="zohoContactID"></param>
        /// <returns></returns>
        public async Task<CuContact> GetCustomerContactByZohoData(long zohoCustomerID, long zohoContactID)
        {
            return await _context.CuContacts.
                Where(x => x.ZohoContactId == zohoContactID && x.ZohoCustomerId == zohoCustomerID
                                                        && x.Active.HasValue && x.Active.Value).
                FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the customercontact by zoho customerid and email
        /// </summary>
        /// <param name="zohoCustomerId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<CuContact> GetCustomerContactByEmailAndZohoID(long zohoCustomerId, string email)
        {
            return await _context.CuContacts.
                Where(x => x.ZohoCustomerId == zohoCustomerId && x.Email.ToLower().Trim() == email.ToLower().Trim()
                                        && x.Active.HasValue && x.Active.Value).
                FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the customer contacts by zoho customerid
        /// </summary>
        /// <param name="zohoCustomerId"></param>
        /// <returns></returns>
        public async Task<List<CuContact>> GetCustomerContactByZohoId(long zohoCustomerId)
        {
            return await _context.CuContacts.
                Where(x => x.ZohoCustomerId == zohoCustomerId && x.Active.HasValue && x.Active.Value).
                ToListAsync();
        }

        /// <summary>
        /// get the customer contacts by zoho contactid
        /// </summary>
        /// <param name="zohoContactId"></param>
        /// <returns></returns>
        public async Task<CuContact> GetCustomerContactByZohoContactId(long zohoContactId)
        {
            return await _context.CuContacts.
                Where(x => x.ZohoContactId == zohoContactId && x.Active.HasValue && x.Active.Value).
                FirstOrDefaultAsync();
        }

        public async Task<ZohoCustomerContact> GetCustomerContactByEmail(string email, int cusid)
        {
            return await _context.CuContacts.
                Where(x => x.CustomerId == cusid && x.Customer.Active.HasValue && x.Customer.Active.Value
                && x.Email.ToLower().Trim() == email.ToLower().Trim() && x.Active.HasValue && x.Active.Value).
                Select(x => new ZohoCustomerContact
                {
                    Id = x.Id,
                    Name = x.ContactName,
                    Email = x.Email
                }).
                FirstOrDefaultAsync();
        }

        public async Task<ZohoCustomerContact> GetOtherContactByEmail(string email, int id)
        {
            return await _context.CuContacts.
                Where(x => x.Id != id && x.Email.ToLower().Trim() == email.ToLower().Trim()
                && x.Active.HasValue && x.Active.Value).
                                Select(x => new ZohoCustomerContact
                                {
                                    Id = x.Id,
                                    Name = x.ContactName,
                                    Email = x.Email
                                }).
                FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the Customer Contact IQueryable data
        /// </summary>
        /// <returns></returns>
        public IQueryable<CuContact> GetCustomerContactDataSourceList()
        {
            return _context.CuContacts.
                    Where(x => x.Active.HasValue && x.Active.Value).OrderBy(x => x.ContactName);

        }

        //get the customer contact by booking Id
        public async Task<List<CommonDataSource>> GetCustomerContactByBooking(int bookingId)
        {
            return await _context.InspTranCuContacts.Where(x => x.InspectionId == bookingId)
                .Select(x => new CommonDataSource
                {
                    Id = x.ContactId,
                    Name = x.Contact.ContactName
                }).AsNoTracking().ToListAsync();
        }

        //get the customer contact by booking Id
        public async Task<List<CommonDataSource>> GetAuditCustomerContactByBooking(int bookingId)
        {
            return await _context.AudTranCuContacts.Where(x => x.AuditId == bookingId)
                .Select(x => new CommonDataSource
                {
                    Id = x.ContactId,
                    Name = x.Contact.ContactName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get contact name list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetCustomerContactList(int customerId, int contactId)
        {
            if (contactId > 0)
            {
                return await _context.CuContacts.
                        Where(x => x.CustomerId == customerId && x.Id != contactId && x.Active.HasValue && x.Active.Value).Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.ContactName
                        }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.CuContacts.
                        Where(x => x.CustomerId == customerId && x.Active.HasValue && x.Active.Value).Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.ContactName
                        }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        /// Get the customer contact email ids
        /// </summary>
        /// <param name="emailIds"></param>
        /// <returns></returns>
        public async Task<List<string>> GetContactEmailIds(List<string> emailIds)
        {
            return await _context.CuContacts.
                Where(x => emailIds.Contains(x.Email.ToLower().Trim()) && x.Active.HasValue && x.Active.Value).
                Select(x => x.Email).ToListAsync();
        }

        public async Task<List<CustomerContact>> GetCustomerContactByCustomerId(int customerId)
        {
            return await _context.CuContacts.Where(x => x.Active == true && x.CustomerId == customerId)
                .Select(y => new CustomerContact()
                {
                    ContactName = y.ContactName,
                    LastName = y.LastName,
                    Id = y.Id,
                    Email = y.Email,
                    JobTitle = y.JobTitle,
                    Mobile = y.Mobile,
                }).OrderBy(x => x.ContactName).AsNoTracking().ToListAsync();
        }

        public async Task<List<CustomerContact>> GetCustomerContactByContactId(int contactId)
        {
            return await _context.CuContacts.Where(x => x.Active == true && x.Id == contactId)
                .Select(y => new CustomerContact()
                {
                    ContactName = y.ContactName,
                    LastName = y.LastName,
                    Id = y.Id,
                    Email = y.Email,
                    JobTitle = y.JobTitle,
                    Mobile = y.Mobile,
                }).OrderBy(x => x.ContactName).AsNoTracking().ToListAsync();
        }

        public async Task<CuContact> GetCustomerContactByCustomerIdAndContactId(int customerId, int contactId)
        {
            return await _context.CuContacts.
                         Where(x => x.Active == true &&
                         x.Customer.IsEaqf.Value &&
                         x.CustomerId == customerId &&
                         x.Id == contactId).
                         FirstOrDefaultAsync();
        }

        public async Task<List<ParentDataSource>> GetCustomerContactTypesByContactIds(IEnumerable<int> contactIds)
        {
            return await _context.CuCustomerContactTypes.Where(x => contactIds.Contains(x.ContactId))
                .Select(x => new ParentDataSource()
                {
                    Name = x.ContactType.ContactType,
                    ParentId = x.ContactId,
                    Id = x.ContactTypeId,
                })
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<CuContactData>> GetCustomerContactByCustomerIds(IEnumerable<int> customerIds)
        {
            return await _context.CuContacts.Where(x => x.Active == true && customerIds.Contains(x.CustomerId))
                .Select(x => new CuContactData()
                {
                    CustomerId = x.CustomerId,
                    FirstName = x.ContactName,
                    LastName = x.LastName,
                    Mobile = x.Phone,
                    UserId = x.ItUserMasters.FirstOrDefault().Id
                })
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<CuAddressesData>> GetCustomerAddressByCustomerId(IEnumerable<int> customerIds)
        {
            return await _context.CuAddresses.Where(x => x.Active == true && customerIds.Contains(x.CustomerId.GetValueOrDefault()))
                .Select(x => new CuAddressesData()
                {
                    CustomerId = x.CustomerId,
                    Alpha2Code = x.Country.Alpha2Code
                })
                .AsNoTracking().ToListAsync();
        }


        public async Task<List<GetEaqfCustomerAddressData>> GetCustomerAddressListByCustomerId(IEnumerable<int> customerIds)
        {
            return await _context.CuAddresses.Where(x => x.Active == true && customerIds.Contains(x.CustomerId.GetValueOrDefault()))
                .Select(x => new GetEaqfCustomerAddressData()
                {
                    Id = x.Id,
                    Country = x.Country.Alpha2Code,
                    City = x.City.CityName,
                    ZipCode = x.ZipCode,
                    Address = x.Address,
                    AddressTypeId = x.AddressType
                })
                .AsNoTracking().ToListAsync();
        }
    }
}
