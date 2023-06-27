using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using DTO.Common;
using DTO.References;
using DTO.UserAccount;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserAccountRepository : Repository, IUserAccountRepository
    {
        public UserAccountRepository(API_DBContext context) : base(context)
        {

        }

        public bool GetUserByName(string name, int? id)
        {
            var res = _context.ItUserMasters
                .Where(x => x.LoginName == name && x.Active);
            if (id.HasValue)
            {
                res = res.Where(x => x.Id != id);
            }
            return res.Any();
        }

        public IQueryable<ItUserMaster> GetUserByType(int? type)
        {
            return _context.ItUserMasters
                .Include(x => x.ItUserRoles)
                .Include(x => x.Staff)
                .Include(x => x.CustomerContact)
                .Include(x => x.SupplierContact)
                .Where(x => x.UserTypeId == type)
                .OrderByDescending(x => x.Active);
        }

        public IEnumerable<ItUserType> GetUserTypes()
        {
            return _context.ItUserTypes;
        }

        public async Task<bool> RemoveUserAccount(ItUserMaster entity, int updatedby)
        {

            if (entity == null)
                return false;
            entity.Active = false;
            entity.DeletedBy = updatedby;
            entity.DeletedOn = DateTime.Now;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }

        public Task<int> SaveEditUserAccount(ItUserMaster entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveNewUserAccount(ItUserMaster entity)
        {
            _context.ItUserMasters.Add(entity);
            return _context.SaveChangesAsync();
        }

        //get the User ID from ITUserMaster for the deleted Customer or Supplier contact
        public async Task<ItUserMaster> GetUserByContactId(int contactId)
        {
            return await _context.ItUserMasters.Where(x => x.CustomerContactId == contactId && x.Active).FirstOrDefaultAsync();
        }

        //More than one contact deleted, update the ITUserMaster User assigned to the contact - only in supplier and factory
        public async Task<IEnumerable<ItUserMaster>> GetUserListByContactId(List<int> contactIds)
        {
            var supplier = await _context.ItUserMasters.Where(x => contactIds.Contains(x.SupplierContactId.GetValueOrDefault())).ToListAsync();

            var factory = await _context.ItUserMasters.Where(x => contactIds.Contains(x.FactoryContactId.GetValueOrDefault())).ToListAsync();

            return supplier.Concat(factory);
        }

        //ge username by id
        public async Task<string> GetUserName(int id)
        {
            return await _context.ItUserMasters.Where(x => x.Id == id).Select(x => x.FullName).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get itusermaster data 
        /// </summary>
        /// <returns></returns>
        public IQueryable<ItUserMaster> GetUserDetails()
        {
            return _context.ItUserMasters.Where(x => x.Active);
        }

        /// <summary>
        /// Login name get max word of same login name
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public async Task<string> GetLoginName(string prefix)
        {
            return await (from itusermaster in _context.ItUserMasters
                          where itusermaster.Active && itusermaster.LoginName.StartsWith(prefix)
                          orderby itusermaster.LoginName descending
                          select itusermaster.LoginName).FirstOrDefaultAsync();
        }

        /// <summary>
        /// login name exists or not
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public async Task<bool> CheckLoginNameExist(string LoginName)
        {
            return await _context.ItUserMasters.Where(x => x.LoginName == LoginName).AnyAsync();
        }

        /// <summary>
        /// Get the customer contact id by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetCustomerContactIdByUser(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Active && x.Id == userId).
                        Select(x => x.CustomerContactId.Value).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the supplier contact id by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetSupplierContactIdByUser(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Active && x.Id == userId).
                        Select(x => x.SupplierContactId.Value).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the factory contact id by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetFactoryContactIdByUser(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Active && x.Id == userId).
                        Select(x => x.FactoryContactId.Value).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get user applicant details for the customer contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public async Task<UserApplicantDetails> GetCustomerContactUserApplicationDetails(int contactId)
        {
            return await _context.CuContacts.Where(x => x.Active.Value && x.Id == contactId).
                        Select(x => new UserApplicantDetails()
                        {
                            ApplicantName = x.ContactName,
                            ApplicantEmail = x.Email,
                            ApplicantPhoneNo = x.Phone
                        }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get user applicant details for the supplier contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public async Task<UserApplicantDetails> GetSupplierContactUserApplicationDetails(int contactId)
        {
            return await _context.SuContacts.Where(x => x.Active.Value && x.Id == contactId).
                        Select(x => new UserApplicantDetails()
                        {
                            ApplicantName = x.ContactName,
                            ApplicantEmail = x.Mail,
                            ApplicantPhoneNo = x.Phone
                        }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ItUserMaster>> GetUserByCustomerContactIds(IEnumerable<int> customerContactIds)
        {
            return await _context.ItUserMasters.Where(x => x.Active == true && x.CustomerContactId.HasValue && customerContactIds.Contains(x.CustomerContactId.Value)).AsNoTracking().ToListAsync();
        }
        
    }
}
