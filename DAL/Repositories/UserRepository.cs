using Contracts.Repositories;
using DTO.CommonClass;
using DTO.DataAccess;
using DTO.Inspection;
using DTO.Quotation;
using DTO.User;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<ItUserMaster> GetUserDetails(int idUser)
        {
            return await _context.ItUserMasters.Include(x => x.ItUserRoles).IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == idUser);
        }

        public async Task<int?> GetUserSignIn(string loginName, string password)
        {
            var encpassword = EncryptPassword(password);
            return await _context.ItUserMasters.Where(x => x.LoginName == loginName.Trim() && x.Password == encpassword.Trim()).Select(x => x.Id).FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<ItRight>> GetRights()
        {
            return await _context.ItRights
                            .Include(x => x.InverseParent)
                            .Include(x => x.Parent)
                            .Include(x => x.ItRoleRights)
                            .Where(x => x.Active.HasValue && x.Active.Value)
                            .ToArrayAsync();

        }

        public IEnumerable<ItRight> GetRightList()
        {
            return _context.ItRights
                            .Include(x => x.InverseParent)
                            .ThenInclude(x => x.ItRoleRights)
                            .Include(x => x.InverseParent)
                            .ThenInclude(x => x.InverseParent)
                            .ThenInclude(x => x.ItRoleRights)
                            .Include(x => x.Parent)
                            .Include(x => x.ItRoleRights)
                            .Where(x => x.Active.HasValue && x.Active.Value);
        }

        public async Task<IEnumerable<ItRole>> GetRoles()
        {
            return await _context.ItRoles
                .Include(x => x.ItRoleRights)
                .Include(x => x.ItUserRoles)
                .Where(x => x.Active.HasValue && x.Active.Value)
                .ToArrayAsync();
        }
        public async Task<List<Role>> GetUserRole(int UserId, int entityId)
        {
            return await _context.ItUserRoles.Where(x => x.UserId == UserId && x.EntityId == entityId)
                .Select(x => new Role
                {
                    RoleName = x.Role.RoleName,
                    Active = x.Role.Active,
                    Id = x.RoleId
                }).IgnoreQueryFilters().AsNoTracking().ToListAsync();
        }
        public async Task<List<Right>> GetUserRights(List<Role> RoleIds, int entityId)
        {
            var lstroleids = RoleIds.Select(z => z.Id).Distinct().ToList();
            return await _context.ItRightMaps.Where(x => x.Active && x.Right.Active.HasValue && x.Right.Active.Value &&
            x.Right.ItRoleRights.Any(y => lstroleids.Contains(y.RoleId)) && x.Right.ItRightEntities.Any(y => y.Active.HasValue && y.Active.Value && y.EntityId == entityId))
                .Select(x => new Right
                {
                    Glyphicons = x.Right.Glyphicons,
                    Id = x.RightId,
                    IsHeading = x.Right.IsHeading,
                    MenuName = x.Right.MenuName,
                    ParentId = x.Right.ParentId,
                    Path = x.Right.Path,
                    Ranking = x.Right.Ranking,
                    RightType = x.RightTypeId,
                    RightTypeService = x.RightType.Service,
                    TitleName = x.Right.TitleName,
                    Active = x.Right.Active,
                    ShowMenu = x.Right.ShowMenu.Value
                }).IgnoreQueryFilters().AsNoTracking().Distinct().OrderBy(x => x.Ranking).ToListAsync();
        }

        public async Task<User> GetUserInfo(int userId)
        {
            return await _context.ItUserMasters.AsSingleQuery().Where(x => x.Id == userId && x.Active)
                .Select(x => new User
                {
                    ChangePassword = x.ChangePassword == null ? false : x.ChangePassword,
                    CountryId = x.Staff.NationalityCountryId,
                    CustomerId = x.CustomerId == null ? 0 : x.CustomerId.Value,
                    EmailAddress = x.Staff.CompanyEmail,
                    FactoryId = x.FactoryId == null ? 0 : x.FactoryId.Value,
                    FullName = x.FullName,
                    Id = x.Id,
                    LocationId = x.Staff == null || x.Staff.LocationId == null ? 0 : x.Staff.LocationId.Value,
                    LocationList = x.Staff.HrOfficeControls.Select(y => y.LocationId).ToList(),
                    LoginName = x.LoginName,
                    ProfileImageUrl = x.FileUrl,
                    StaffId = x.StaffId == null ? 0 : x.StaffId.Value,
                    UserType = (UserTypeEnum)x.UserTypeId,
                    SupplierId = x.SupplierId == null ? 0 : x.SupplierId.Value,
                    UserProfileList = x.Staff.HrStaffProfiles.Select(y => y.ProfileId).ToList(),
                    StatusId = x.StatusId,
                    SupplierContactId = x.SupplierContactId == null ? 0 : x.SupplierContactId.Value,
                    FactoryContactId = x.FactoryContactId == null ? 0 : x.FactoryContactId.Value,
                    CustomerContactId = x.CustomerContactId == null ? 0 : x.CustomerContactId.Value,
                    FbUserId = x.FbUserId
                }).IgnoreQueryFilters().FirstOrDefaultAsync();
        }
        public IEnumerable<ItRole> GetRoleList()
        {
            return _context.ItRoles.Include(x => x.ItRoleRights).Where(x => x.Active.HasValue && x.Active.Value);
        }

        private static string EncryptPassword(string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(plainTextBytes);
        }

        public async Task<IEnumerable<ApEntity>> GetEntities()
        {
            return await _context.ApEntities.ToArrayAsync();
        }

        public async Task<IEnumerable<MidTask>> GetTasks(int userId, IEnumerable<int> roleList, IEnumerable<int> officeControlList)
        {
            var dataToApprove = await _context.MidTasks
                .Include(x => x.User)
                 .ThenInclude(x => x.Staff)
               .Where(x => !x.IsDone && x.ReportTo == userId && x.CreatedOn != null).OrderByDescending(x => x.CreatedBy).Take(50).AsNoTracking().ToListAsync();

            return dataToApprove;
        }

        public IQueryable<MidTask> GetTaskData()
        {
            return _context.MidTasks;
        }

        public IQueryable<MidNotification> GetNotificationData()
        {
            return _context.MidNotifications;
        }

        public async Task<IEnumerable<MidNotification>> GetNotifications(int userId)
        {
            return await _context.MidNotifications.Where(x => x.UserId == userId && !x.IsRead && x.CreatedOn != null).OrderByDescending(x => x.CreatedOn)
                .Take(50).AsNoTracking().ToListAsync();
        }

        public async Task<MidTask> GetTask(Expression<Func<MidTask, bool>> predicate)
        {
            return await _context.MidTasks
                    .Include(x => x.User)
                    .ThenInclude(x => x.Staff)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<MidNotification> GetNotification(Expression<Func<MidNotification, bool>> predicate)
        {
            return await _context.MidNotifications
                    .Include(x => x.User)
                    .ThenInclude(x => x.Staff)
                .FirstOrDefaultAsync(predicate);
        }
        public async Task<MidNotification> ReadNotification(Guid Id)
        {
            var notification = await _context.MidNotifications.FirstOrDefaultAsync(x => x.Id == Id);

            if (notification == null)
                return null;

            notification.IsRead = true;
            notification.UpdatedOn = DateTime.Now;
            _context.Entry(notification).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return notification;
        }


        public async Task<IEnumerable<ItUserMaster>> GetUserListByRole(int officeId, int roleId)
        {
            return await _context.ItUserMasters
                .Include(x => x.Staff).Where(x => x.ItUserRoles.Any(y => y.RoleId == roleId) && x.Active && x.Staff.Active.Value
                && x.Staff.HrOfficeControls.Any(y => y.LocationId == officeId)).ToListAsync();
        }

        /// <summary>
        /// Get email contact by role and office access
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetUserEmailByRoleAndOffice(int officeId, int roleId)
        {
            return await _context.ItUserMasters
                .Include(x => x.Staff).Where(x => x.ItUserRoles.Any(y => y.RoleId == roleId)
                && x.Staff.HrOfficeControls.Any(y => y.LocationId == officeId) && x.Staff.CompanyEmail != null).
                Select(x => x.Staff.CompanyEmail).ToListAsync();
        }



        public async Task<IEnumerable<ItUserMaster>> GetExternalUserListByRole(int roleId, int userid, UserTypeEnum usertype)
        {
            if (UserTypeEnum.Customer == usertype)
            {
                return await _context.ItUserMasters
                    .Where(x => x.ItUserRoles.Any(y => y.RoleId == roleId) && x.CustomerId != null && x.CustomerId.Value == userid).ToListAsync();
            }
            else if (UserTypeEnum.Supplier == usertype)
            {
                return await _context.ItUserMasters
               .Where(x => x.ItUserRoles.Any(y => y.RoleId == roleId) && x.SupplierId != null && x.SupplierId.Value == userid).ToListAsync();
            }
            else if (UserTypeEnum.Factory == usertype)
            {
                return await _context.ItUserMasters
               .Where(x => x.ItUserRoles.Any(y => y.RoleId == roleId) && x.FactoryId != null && x.SupplierId.Value == userid).ToListAsync();
            }
            else
            {
                return null;
            }
        }


        public bool CheckOldPasswordExists(int userId, string password)
        {
            var encpassword = EncryptPassword(password);
            var user = _context.ItUserMasters
                    .Where(x => x.Id == userId && x.Password == encpassword)
                    .FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            return true;
        }

        public Task<int> UpdatePassword(int userId, string password, string oldpassword)
        {
            var encpassword = EncryptPassword(oldpassword);
            var user = _context.ItUserMasters
                    .Where(x => x.Id == userId && x.Password == encpassword).SingleOrDefault();
            user.Password = EncryptPassword(password);
            user.ChangePassword = true;
            _context.Entry(user).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveRoleDetail(ItRole entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InspTransaction>> GetParentBookingId()
        {
            return await _context.InspTransactions.Where(x => x.SplitPreviousBookingNo != null && x.StatusId == (int)BookingStatus.Received).ToListAsync();

        }
        //get user list based on customerid, service, role, product category, office, email should be active
        public async Task<IEnumerable<User>> GetUserListByRoleOfficeService(UserAccess userAccess)
        {
            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id
                          join darole in _context.DaUserByRoles on dacus.Id equals darole.DauserCustomerId

                          join staff in _context.HrStaffs on itusers.StaffId equals staff.Id into ituserstaff
                          from ituserstaffdata in ituserstaff.DefaultIfEmpty()

                          join daoffice in _context.DaUserRoleNotificationByOffices on dacus.Id equals daoffice.DauserCustomerId into dacusoffice
                          from dacusofficedata in dacusoffice.DefaultIfEmpty()

                          join daproductcategory in _context.DaUserByProductCategories on dacus.Id equals daproductcategory.DauserCustomerId into dacusproductcategory
                          from dacusproductcategorydata in dacusproductcategory.DefaultIfEmpty()

                          join daservice in _context.DaUserByServices on dacus.Id equals daservice.DauserCustomerId into dacusservice
                          from dacusservicedata in dacusservice.DefaultIfEmpty()

                          join dadepart in _context.DaUserByDepartments on dacus.Id equals dadepart.DauserCustomerId into dacusdept
                          from dacusdeptdata in dacusdept.DefaultIfEmpty()

                          join dabrand in _context.DaUserByBrands on dacus.Id equals dabrand.DauserCustomerId into dacusbrand
                          from dacusbranddata in dacusbrand.DefaultIfEmpty()

                          join daFactoryCountry in _context.DaUserByFactoryCountries on dacus.Id equals daFactoryCountry.DaUserCustomerId into daUserFactoryCountry
                          from daUserFactoryCountryData in daUserFactoryCountry.DefaultIfEmpty()

                          join onsiteEmail in _context.CuCsOnsiteEmails on itusers.Id equals onsiteEmail.UserId into onsiteCsEmail
                          from onsiteEmailstaffdata in onsiteCsEmail.DefaultIfEmpty()

                              //join dabuyer in _context.DaUserByBuyers on dacus.Id equals dabuyer.DauserCustomerId into dacusbuyer
                              //from dacusbuyerdata in dacusbuyer.DefaultIfEmpty()

                          where (itusers.Active && dacus.Email && darole.RoleId == userAccess.RoleId &&
                          (dacus.CustomerId == null || dacus.CustomerId == userAccess.CustomerId) &&
                          (dacusofficedata.OfficeId == null || dacusofficedata.OfficeId == userAccess.OfficeId) &&
                          (dacusproductcategorydata.ProductCategoryId == null || userAccess.ProductCategoryIds.Contains(dacusproductcategorydata.ProductCategoryId)) &&
                          (dacusservicedata.ServiceId == null || dacusservicedata.ServiceId == userAccess.ServiceId) &&
                          (dacusdeptdata.DepartmentId == null || userAccess.DepartmentIds.Contains(dacusdeptdata.DepartmentId)) &&
                          (dacusbranddata.BrandId == null || userAccess.BrandIds.Contains(dacusbranddata.BrandId)) &&
                          (daUserFactoryCountryData == null || userAccess.FactoryCountryId == daUserFactoryCountryData.FactoryCountryId) &&
                          (onsiteEmailstaffdata == null || (onsiteEmailstaffdata.Active && onsiteEmailstaffdata.CustomerId == userAccess.CustomerId))) &&
                          (ituserstaffdata == null || ituserstaffdata.Active.Value)
                          //(dacusbuyerdata.BuyerId == null || dacusbuyerdata.BuyerId == userAccess.BuyerId))
                          select new User
                          {
                              FullName = itusers.FullName,
                              Id = itusers.Id,
                              EmailAddress = ituserstaffdata.CompanyEmail,
                              OnsiteEmail = onsiteEmailstaffdata.EmailId
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        //get user list based on role, office, should be active
        public async Task<IEnumerable<User>> GetUserListByRoleOffice(UserAccess userAccess)
        {

            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id
                          join darole in _context.DaUserByRoles on dacus.Id equals darole.DauserCustomerId

                          join staff in _context.HrStaffs on itusers.StaffId equals staff.Id into ituserstaff
                          from ituserstaffdata in ituserstaff.DefaultIfEmpty()

                          join daoffice in _context.DaUserRoleNotificationByOffices on dacus.Id equals daoffice.DauserCustomerId into dacusoffice
                          from dacusofficedata in dacusoffice.DefaultIfEmpty()

                          where (itusers.Active && darole.RoleId == userAccess.RoleId &&
                          (dacusofficedata.OfficeId == null || dacusofficedata.OfficeId == userAccess.OfficeId)) &&
                          (ituserstaffdata == null || ituserstaffdata.Active.Value)
                          select new User
                          {
                              FullName = itusers.FullName,
                              Id = itusers.Id,
                              EmailAddress = ituserstaffdata.CompanyEmail
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the user details mapped to the user config and cs(AE) access 
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        public async Task<List<UserStaffDetails>> GetCsEnabledBookingContacts(UserAccess userAccess)
        {

            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id
                          join darole in _context.DaUserByRoles on dacus.Id equals darole.DauserCustomerId

                          join staff in _context.HrStaffs on itusers.StaffId equals staff.Id into ituserstaff
                          from ituserstaffdata in ituserstaff.DefaultIfEmpty()

                          join daoffice in _context.DaUserRoleNotificationByOffices on dacus.Id equals daoffice.DauserCustomerId into dacusoffice
                          from dacusofficedata in dacusoffice.DefaultIfEmpty()

                          join daproductcategory in _context.DaUserByProductCategories on dacus.Id equals daproductcategory.DauserCustomerId into dacusproductcategory
                          from dacusproductcategorydata in dacusproductcategory.DefaultIfEmpty()

                          join daservice in _context.DaUserByServices on dacus.Id equals daservice.DauserCustomerId into dacusservice
                          from dacusservicedata in dacusservice.DefaultIfEmpty()

                          join dadepart in _context.DaUserByDepartments on dacus.Id equals dadepart.DauserCustomerId into dacusdept
                          from dacusdeptdata in dacusdept.DefaultIfEmpty()

                          join dabrand in _context.DaUserByBrands on dacus.Id equals dabrand.DauserCustomerId into dacusbrand
                          from dacusbranddata in dacusbrand.DefaultIfEmpty()

                          where (itusers.Active && dacus.Email && dacus.UserType == (int)HRProfile.CS && darole.RoleId == userAccess.RoleId &&
                          (dacus.CustomerId == userAccess.CustomerId) &&
                          (dacusofficedata.OfficeId == null || dacusofficedata.OfficeId == userAccess.OfficeId) &&
                          (dacusproductcategorydata.ProductCategoryId == null || userAccess.ProductCategoryIds.Contains(dacusproductcategorydata.ProductCategoryId)) &&
                          (dacusservicedata.ServiceId == null || dacusservicedata.ServiceId == userAccess.ServiceId) &&
                          (dacusdeptdata.DepartmentId == null || userAccess.DepartmentIds.Contains(dacusdeptdata.DepartmentId)) &&
                          (dacusbranddata.BrandId == null || userAccess.BrandIds.Contains(dacusbranddata.BrandId)))
                          select new UserStaffDetails
                          {
                              FullName = ituserstaffdata.PersonName,
                              Id = ituserstaffdata.Id,
                              EmailAddress = ituserstaffdata.CompanyEmail,
                              StaffId = itusers.StaffId.GetValueOrDefault(),
                              MobileNumber = ituserstaffdata.CompanyMobileNo,
                              EmployeeTypeId = ituserstaffdata.EmployeeTypeId,
                              LocationName = ituserstaffdata.Location.LocationName,
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        // Get the user details mapped to the user config and report checker profile
        public async Task<List<UserStaffDetails>> GetUserListByReportCheckerProfile(UserAccess userAccess)
        {

            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id
                          join darole in _context.DaUserByRoles on dacus.Id equals darole.DauserCustomerId

                          join staff in _context.HrStaffs on itusers.StaffId equals staff.Id into ituserstaff
                          from ituserstaffdata in ituserstaff.DefaultIfEmpty()

                          join daoffice in _context.DaUserRoleNotificationByOffices on dacus.Id equals daoffice.DauserCustomerId into dacusoffice
                          from dacusofficedata in dacusoffice.DefaultIfEmpty()

                          join daproductcategory in _context.DaUserByProductCategories on dacus.Id equals daproductcategory.DauserCustomerId into dacusproductcategory
                          from dacusproductcategorydata in dacusproductcategory.DefaultIfEmpty()

                          join daservice in _context.DaUserByServices on dacus.Id equals daservice.DauserCustomerId into dacusservice
                          from dacusservicedata in dacusservice.DefaultIfEmpty()

                          where (itusers.Active && darole.RoleId == userAccess.RoleId &&
                          (dacus.CustomerId == null || dacus.CustomerId == userAccess.CustomerId) &&
                          (dacusofficedata.OfficeId == null || dacusofficedata.OfficeId == userAccess.OfficeId) &&
                          (dacusproductcategorydata.ProductCategoryId == null || userAccess.ProductCategoryIds.Contains(dacusproductcategorydata.ProductCategoryId)) &&
                          (dacusservicedata.ServiceId == null || dacusservicedata.ServiceId == userAccess.ServiceId)) &&
                          (userAccess.StaffOfficeId.HasValue ? ituserstaffdata.LocationId == userAccess.StaffOfficeId : true) &&
                          (ituserstaffdata == null || ituserstaffdata.Active.Value)
                          select new UserStaffDetails
                          {
                              FullName = ituserstaffdata.PersonName,
                              Id = ituserstaffdata.Id,
                              EmailAddress = ituserstaffdata.CompanyEmail,
                              StaffId = itusers.StaffId.GetValueOrDefault(),
                              MobileNumber = ituserstaffdata.CompanyMobileNo,
                              EmployeeTypeId = ituserstaffdata.EmployeeTypeId,
                              LocationName = ituserstaffdata.Location.LocationName,
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the user details mapped to the user config and cs(AE) access 
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        public async Task<List<UserStaffDetails>> GetCSBookingContacts(UserAccess userAccess)
        {

            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id
                          join darole in _context.DaUserByRoles on dacus.Id equals darole.DauserCustomerId

                          join staff in _context.HrStaffs on itusers.StaffId equals staff.Id into ituserstaff
                          from ituserstaffdata in ituserstaff.DefaultIfEmpty()

                          join daoffice in _context.DaUserRoleNotificationByOffices on dacus.Id equals daoffice.DauserCustomerId into dacusoffice
                          from dacusofficedata in dacusoffice.DefaultIfEmpty()

                          join daservice in _context.DaUserByServices on dacus.Id equals daservice.DauserCustomerId into dacusservice
                          from dacusservicedata in dacusservice.DefaultIfEmpty()

                          where (itusers.Active && dacus.Email && dacus.UserType == (int)HRProfile.CS && darole.RoleId == userAccess.RoleId &&
                          (dacus.CustomerId == userAccess.CustomerId) &&
                          (dacusofficedata.OfficeId == null || dacusofficedata.OfficeId == userAccess.OfficeId) &&
                          (dacusservicedata.ServiceId == null || dacusservicedata.ServiceId == userAccess.ServiceId))
                          select new UserStaffDetails
                          {
                              FullName = ituserstaffdata.PersonName,
                              Id = ituserstaffdata.Id,
                              EmailAddress = ituserstaffdata.CompanyEmail,
                              StaffId = itusers.StaffId.GetValueOrDefault(),
                              MobileNumber = ituserstaffdata.CompanyMobileNo,
                          }).Distinct().AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the user details by customer
        /// </summary>
        /// <param name="lstcustomerId"></param>
        /// <returns></returns>
        public async Task<List<AEDetails>> GetAEByCustomer(List<int> lstcustomerId)
        {
            return await (from itusers in _context.ItUserMasters
                          join dacus in _context.DaUserCustomers on itusers.Id equals dacus.UserId into dacusdata
                          from cudata in dacusdata.DefaultIfEmpty()
                          join staff in _context.HrStaffs on cudata.User.StaffId.Value equals staff.Id
                          where (itusers.Active && staff.Active.Value
                          && cudata.UserType == (int)HRProfile.CS
                          && lstcustomerId.Contains(cudata.CustomerId.Value))
                          select new AEDetails
                          {
                              FullName = staff.PersonName,
                              Customerid = cudata.CustomerId.GetValueOrDefault(),
                              EmailAddress = staff.CompanyEmail
                          }).Distinct().AsNoTracking().ToListAsync();

        }
        //get AE list based on customer ids and location ids and da table CS(true) with active fields
        public async Task<IEnumerable<AEDetails>> GetAEByCustomerAndLocation(IEnumerable<int> locationIds, IEnumerable<int> customerIds)
        {
            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id

                          join staff in _context.HrStaffs on itusers.StaffId equals staff.Id into ituserstaff
                          from ituserstaffdata in ituserstaff.DefaultIfEmpty()

                          join daoffice in _context.DaUserRoleNotificationByOffices on dacus.Id equals daoffice.DauserCustomerId into dacusoffice
                          from dacusofficedata in dacusoffice.DefaultIfEmpty()

                          where (itusers.Active && customerIds.Contains(dacus.CustomerId.Value) && dacus.UserType == (int)HRProfile.CS &&
                          ituserstaffdata.Active.Value &&
                          (dacusofficedata.OfficeId == null ||
                          locationIds.Contains((int)dacusofficedata.OfficeId)))
                          select new AEDetails
                          {
                              FullName = itusers.FullName,
                              Id = itusers.Id,
                              EmailAddress = ituserstaffdata.CompanyEmail,
                              MobileNumber = ituserstaffdata.CompanyMobileNo,
                              UserType = dacus.UserType,
                              StaffId = ituserstaffdata.Id
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        //Fetch all the AEs from DA_UserCustomer
        public async Task<List<AEDetails>> GetAEList()
        {
            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id
                          join staff in _context.HrStaffs on dacus.User.StaffId.Value equals staff.Id
                          where (itusers.Active && staff.Active.Value &&
                                dacus.UserType == (int)HRProfile.CS)
                          select new AEDetails
                          {
                              FullName = staff.PersonName,
                              Customerid = dacus.CustomerId.GetValueOrDefault(),
                              Id = itusers.Id
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        //get role list
        public async Task<IEnumerable<ItRole>> GetRolesList(int userId)
        {
            return await _context.ItRoles.Where(x => x.Active.Value && x.SecondaryRole
                                                    && x.ItUserRoles.Any(y => y.UserId == userId)).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUsersIdListByCustomer(int customerid)
        {
            return await _context.ItUserMasters.Where(x => x.CustomerId == customerid).Select(x => x.Id).ToListAsync();
        }

        //get userlist who has role access
        public async Task<IEnumerable<UserStaffDetails>> GetRoleAccessUserList(int roleId)
        {
            return await _context.ItUserRoles.Where(x => x.RoleId == roleId && x.User.Active && x.User.Staff.Active.HasValue && x.User.Staff.Active.Value).
                Select(x => new UserStaffDetails
                {
                    FullName = x.User.Staff.PersonName,
                    StaffId = x.User.Staff.Id,
                    EmailAddress = x.User.Staff.CompanyEmail,
                    MobileNumber = x.User.Staff.CompanyMobileNo,
                    EmployeeTypeId = x.User.Staff.EmployeeTypeId,
                    LocationName = x.User.Staff.Location.LocationName,
                }).AsNoTracking().
                ToListAsync();
        }

        //Get task list based on id, tasktype id, isdone value
        public async Task<IEnumerable<MidTask>> GetTaskList(int linkId, IEnumerable<int> typeIdList, bool isDone)
        {
            return await _context.MidTasks.Where(x => x.LinkId == linkId && typeIdList.Contains(x.TaskTypeId) && x.IsDone == isDone).ToListAsync();
        }

        //get user list based on customerid, dep, brand, office
        public async Task<IEnumerable<CSConfigDetail>> GetUserListByCusDeptBrandData(UserAccess userAccess)
        {
            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id

                          join daoffice in _context.DaUserRoleNotificationByOffices on dacus.Id equals daoffice.DauserCustomerId into dacusoffice
                          from dacusofficedata in dacusoffice.DefaultIfEmpty()

                          join dadepart in _context.DaUserByDepartments on dacus.Id equals dadepart.DauserCustomerId into dacusdept
                          from dacusdeptdata in dacusdept.DefaultIfEmpty()

                          join dabrand in _context.DaUserByBrands on dacus.Id equals dabrand.DauserCustomerId into dacusbrand
                          from dacusbranddata in dacusbrand.DefaultIfEmpty()

                          join dauserbyservice in _context.DaUserByServices on dacus.Id equals dauserbyservice.DauserCustomerId into dacususerbyservice
                          from dacusuerbyservicedata in dacususerbyservice.DefaultIfEmpty()

                          where (itusers.Active && dacus.UserType == (int)HRProfile.CS &&
                          userAccess.CustomerIds.Contains(dacus.CustomerId) &&
                          (dacusofficedata.OfficeId == null || userAccess.OfficeIds.Contains(dacusofficedata.OfficeId)) &&
                          (dacusdeptdata.DepartmentId == null || userAccess.DepartmentIds.Contains(dacusdeptdata.DepartmentId)) &&
                          (dacusbranddata.BrandId == null || userAccess.BrandIds.Contains(dacusbranddata.BrandId)) &&
                          (dacusuerbyservicedata.ServiceId == null || dacusuerbyservicedata.ServiceId == userAccess.ServiceId))
                          select new CSConfigDetail
                          {
                              CsName = itusers.FullName,
                              CsId = itusers.Id,
                              CustomerId = dacus.CustomerId,
                              OfficeId = dacusofficedata.OfficeId,
                              BrandId = dacusbranddata.BrandId,
                              DepartmentId = dacusdeptdata.DepartmentId
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        //get user config details(customer id, brand id, dept id list) by usertype CS , userids
        public async Task<List<CSConfigDetail>> GetAECustomerConfigList(IEnumerable<int> UserIds)
        {
            return await (from dacus in _context.DaUserCustomers
                          join itusers in _context.ItUserMasters on dacus.UserId equals itusers.Id

                          join dadepart in _context.DaUserByDepartments on dacus.Id equals dadepart.DauserCustomerId into dacusdept
                          from dacusdeptdata in dacusdept.DefaultIfEmpty()

                          join dabrand in _context.DaUserByBrands on dacus.Id equals dabrand.DauserCustomerId into dacusbrand
                          from dacusbranddata in dacusbrand.DefaultIfEmpty()

                          where (itusers.Active && dacus.UserType == (int)HRProfile.CS &&
                          UserIds.Contains(dacus.UserId))
                          select new CSConfigDetail
                          {
                              CustomerId = dacus.CustomerId,
                              BrandId = dacusbranddata.BrandId,
                              DepartmentId = dacusdeptdata.DepartmentId
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the AE User List Data
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetAEUserList()
        {

            return await _context.ItUserMasters.Where(x => x.Active && x.DaUserCustomerUsers.Any(y => y.Active.Value && y.UserType == (int)HRProfile.CS) && x.Staff.Active.Value)
                         .Select(x => new CommonDataSource
                         {
                             Name = x.Staff.PersonName,
                             Id = x.Id
                         }).Distinct().OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        public async Task<CommonDataSource> GetInternalUserPrimaryEntity(int? staffId)
        {
            return await _context.HrStaffs.Where(x => x.Active.Value && x.Id == staffId)
                .Select(x => new CommonDataSource() { Id = x.PrimaryEntity.GetValueOrDefault(), Name = x.PrimaryEntityNavigation.Name }).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get internal user service access
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetInternalUserServiceIds(int? staffId, int primaryEntity)
        {
            return await _context.HrStaffEntityServiceMaps.Where(x => x.EntityId == primaryEntity && x.StaffId == staffId)
                .Select(x => x.ServiceId.GetValueOrDefault()).ToListAsync();
        }

        /// <summary>
        /// get internal user entity access by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetInternalUserEntityAccess(int userId)
        {
            return await _context.HrEntityMaps
                         .Where(x => x.Staff.ItUserMasters.Any(y => y.Id == userId && y.Active))
                         .Select(x => new CommonDataSource
                         {
                             Name = x.Entity.Name,
                             Id = x.EntityId
                         }).Distinct().OrderBy(x => x.Name)
                         .IgnoreQueryFilters()
                         .AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get supplier entity access 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetSupplierEntityAccess(int userId)
        {
            return await _context.SuContactEntityMaps
                         .Where(x => x.Contact.ItUserMasterSupplierContacts.Any(y => y.Id == userId && y.Active))
                         .Select(x => new CommonDataSource
                         {
                             Name = x.Entity.Name,
                             Id = x.EntityId
                         }).Distinct().OrderBy(x => x.Name)
                         .IgnoreQueryFilters()
                         .AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get factory entity access 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetFactoryEntityAccess(int userId)
        {
            return await _context.SuContactEntityMaps
                         .Where(x => x.Contact.ItUserMasterFactoryContacts.Any(y => y.Id == userId && y.Active))
                         .Select(x => new CommonDataSource
                         {
                             Name = x.Entity.Name,
                             Id = x.EntityId
                         }).Distinct().OrderBy(x => x.Name)
                         .IgnoreQueryFilters()
                         .AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get customer entity access
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCustomerEntityAccess(int userId)
        {
            return await _context.CuContactEntityMaps
                         .Where(x => x.Contact.ItUserMasters.Any(y => y.Id == userId && y.Active))
                         .Select(x => new CommonDataSource
                         {
                             Name = x.Entity.Name,
                             Id = x.EntityId
                         }).Distinct().OrderBy(x => x.Name)
                         .IgnoreQueryFilters()
                         .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ItUserRole>> GetUserRolesByUserIds(IEnumerable<int> userIds)
        {
            return await _context.ItUserRoles.Where(x => userIds.Contains(x.UserId)).ToListAsync();
        }
        public async Task<IEnumerable<ItUserRole>> GetUserRolesByUserIdsIgnoreQueryFilter(IEnumerable<int> userIds)
        {
            return await _context.ItUserRoles.Where(x => userIds.Contains(x.UserId)).IgnoreQueryFilters().AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<string>> GetUserEmailByRoleAndOfficeList(IEnumerable<int> officeIds, int roleId)
        {
            return await _context.ItUserMasters.Where(x => x.ItUserRoles.Any(y => y.RoleId == roleId)
                && x.Staff.HrOfficeControls.Any(y => officeIds.Contains(y.LocationId)) && x.Staff.CompanyEmail != null).
                Select(x => x.Staff.CompanyEmail).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ItRightEntity>> GetItRightEntitesList()
        {
            return await _context.ItRightEntities.Where(x => x.Active.HasValue && x.Active.Value).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ItUserMaster>> GetUsersByRoleId(int roleId)
        {
            return await _context.ItUserRoles.Where(x => x.RoleId == roleId).Select(y => y.User).AsNoTracking().ToListAsync();
        }

        public Task<int> ResetPassword(int userId, string password)
        {
            var user = _context.ItUserMasters
                    .Where(x => x.Active && x.Id == userId).SingleOrDefault();
            user.Password = EncryptPassword(password);
            user.ChangePassword = true;
            _context.Entry(user).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<int> GetCustomerContactIdByUserId(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Id == userId).AsNoTracking().Select(x => x.CustomerContactId.GetValueOrDefault()).FirstOrDefaultAsync();
        }
        public async Task<List<RightType>> GetRightTypeList()
        {
            return await _context.ItRightTypes.Where(x => x.Active.Value).OrderBy(x => x.Sort)
            .Select(x => new RightType
            {
                Id = x.Id,
                Name = x.Name,
                Service = x.Service
            }).AsNoTracking().ToListAsync();
        }

        public async Task<CuContact> GetCustomerContactByUserId(int userId)
        {
            return await _context.ItUserMasters.Where(x => x.Id == userId).Select(x => x.CustomerContact).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
