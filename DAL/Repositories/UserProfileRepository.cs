using Contracts.Repositories;
using DTO.UserProfile;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserProfileRepository: Repository, IUserProfileRepository
    {
        public UserProfileRepository(API_DBContext context) : base(context)
        {
        }

        //Get the user profile data based on the User Id and user type
        public async Task<UserProfile> GetUserProfileData(int userId, int userType)
        {
            UserProfile res = new UserProfile();

            if (userType == (int)UserTypeEnum.Customer)
            {
                return await _context.ItUserMasters.Where(x => x.Id == userId && x.Active)
                    .Select(x => new UserProfile
                    {
                        UserId = x.Id,
                        UserName = x.LoginName,
                        DisplayName = x.FullName,
                        EmailId = x.CustomerContact.Email,
                        Phone = x.CustomerContact.Phone,
                        ProfileImageName = x.FileName,
                        ProfileImageUrl = x.FileUrl
                    }).FirstOrDefaultAsync();
            }

            else if (userType == (int)UserTypeEnum.Supplier)
            {
                return await _context.ItUserMasters.Where(x => x.Id == userId && x.Active)
                    .Select(x => new UserProfile
                    {
                        UserId = x.Id,
                        UserName = x.LoginName,
                        DisplayName = x.FullName,
                        EmailId = x.SupplierContact.Mail,
                        Phone = x.SupplierContact.Phone,
                        ProfileImageName = x.FileName,
                        ProfileImageUrl = x.FileUrl
                    }).FirstOrDefaultAsync();
            }

            else if (userType == (int)UserTypeEnum.Factory)
            {
                return await _context.ItUserMasters.Where(x => x.Id == userId && x.Active)
                    .Select(x => new UserProfile
                    {
                        UserId = x.Id,
                        UserName = x.LoginName,
                        DisplayName = x.FullName,
                        EmailId = x.FactoryContact.Mail,
                        Phone = x.FactoryContact.Phone,
                        ProfileImageName = x.FileName,
                        ProfileImageUrl = x.FileUrl
                    }).FirstOrDefaultAsync();
            }

            return res;
        }

        //get the itusermaster for update
        public async Task<ItUserMaster> GetUserProfileEntity(int userId)
        {
            return await _context.ItUserMasters
                .Include(x => x.CustomerContact)
                .Include(x => x.SupplierContact)
                .Include(x => x.FactoryContact)
                .Where(x => x.Id == userId && x.Active).FirstOrDefaultAsync();
        }

        //Get the contacts based on email
        public async Task<List<UserProfile>> GetContactsByEmail (string emailId, int userType)
        {
            List<UserProfile> res = new List<UserProfile>();

            if (userType == (int)UserTypeEnum.Customer)
            {
                return await _context.CuContacts.Where(x => x.Email == emailId && x.Active == true)
                    .Select(x => new UserProfile
                    {
                        UserId = x.Id,
                        EmailId = x.Email
                    }).ToListAsync();
            }

            else if (userType == (int)UserTypeEnum.Supplier)
            {
                return await _context.SuContacts.Where(x => x.Mail == emailId && x.Active == true && x.Supplier.TypeId == (int)Supplier_Type.Supplier_Agent)
                    .Select(x => new UserProfile
                    {
                        UserId = x.Id,
                        EmailId = x.Mail
                    }).ToListAsync();
            }

            else if (userType == (int)UserTypeEnum.Factory)
            {
                return await _context.SuContacts.Where(x => x.Mail == emailId && x.Active == true && x.Supplier.TypeId == (int)Supplier_Type.Factory)
                    .Select(x => new UserProfile
                    {
                       UserId = x.Id,
                        EmailId = x.Mail
                    }).ToListAsync();
            }

            return res;

        }
    }
}
