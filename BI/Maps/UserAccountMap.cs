using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using DTO.UserAccount;
using DTO.HumanResource;
using System.Linq;
using DTO.User;
using Entities.Enums;
using DTO.Common;
using DTO.CommonClass;

namespace BI.Maps
{
    public class UserAccountMap : ApiCommonData
    {
        public UserAccount GetInternalUser(HrStaff entity, int userTypeId)
        {
            if (entity == null)
                return null;
            return new UserAccount
            {
                Id = entity.Id,
                Name = entity.PersonName,
                Gender = entity.Gender,
                DepartmentName = entity.Department?.DepartmentName,
                Position = entity.Position?.PositionName,
                Office = entity.Location?.LocationName,
                HasAccount = entity.ItUserMasters.Any(),
                UserTypeId = userTypeId
            };
        }
        
        /// <summary>
        /// map the user account list
        /// </summary>
        /// <param name="staffList"></param>
        /// <param name="userTypeId"></param>
        /// <returns></returns>
        public UserAccount MapUserAccountList(UserAccountSearchData staff, int userTypeId)
        {
            var userAccount = new UserAccount()
            {
                Id = staff.Id,
                Name = staff.PersonName,
                Gender = staff.Gender,
                DepartmentName = staff.DepartmentName,
                Position = staff.PositionName,
                Office = staff.LocationName,
                HasAccount = staff.HasAccount,
                UserTypeId = userTypeId,
                Country = staff.CountryName
            };

            return userAccount;
        }

        public UserAccount GetCustomer(CuCustomer entity)
        {
            if (entity == null)
                return null;
            string countriesName = "";
            int i = 1;
            foreach (var element in entity.CuCustomerBusinessCountries)
            {
                if (element.CustomerId == entity.Id)
                {
                    if (i == entity.CuCustomerBusinessCountries.Count())
                        countriesName += element.BusinessCountry?.CountryName;
                    else
                        countriesName += element.BusinessCountry?.CountryName + ", ";
                }
                i++;
            }
            return new UserAccount
            {
                Id = entity.Id,
                Name = entity.CustomerName,
                Country = countriesName,
                HasAccount = entity.ItUserMasters.Any(),
                UserTypeId = 2
            };
        }

        public Role GetRole(ItRole entity)
        {
            if (entity == null)
                return null;

            return new Role
            {
                Id = entity.Id,
                RoleName = entity.RoleName,
                Active = entity.Active
            };
        }

        public string DecryptPassword(string password)
        {
            var bytes = Convert.FromBase64String(password);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public UserAccountItem GetUserAccountItem(ItUserMaster entity, List<Contact> contactList)
        {
            if (entity == null)
                return null;
            var roleList = new List<int> { };
            var roleEntityList = new List<UserRoleEntity>();
            string roleName = "";

            int contactId = 0;
            int i = 1;
            foreach (var element in entity.ItUserRoles)
            {
                if (element.UserId == entity.Id)
                {

                    if (!roleList.Any(x => x == element.RoleId))
                    {
                        roleList.Add(element.RoleId);
                        if (i == entity.ItUserRoles.Count())
                            roleName += element.Role.RoleName;
                        else
                            roleName += element.Role.RoleName + ", ";
                    }

                }
                i++;
            }

            var groupedUserRoles = entity.ItUserRoles.GroupBy(x => new { x.UserId, x.RoleId }).ToList();

            foreach (var userRole in groupedUserRoles)
            {
                var entityList = new List<int> { };

                foreach (var item in userRole)
                {
                    entityList.Add(item.EntityId);
                }

                roleEntityList.Add(new UserRoleEntity()
                {
                    RoleId = userRole.FirstOrDefault().RoleId,
                    RoleName = userRole.FirstOrDefault().Role.RoleName,
                    RoleEntity = entityList
                });

            }

            if (entity.UserTypeId == (int)UserTypeEnum.Customer)
            {
                contactId = entity.CustomerContactId.GetValueOrDefault();
            }
            else if (entity.UserTypeId == (int)UserTypeEnum.Supplier)
            {
                contactId = entity.SupplierContactId.GetValueOrDefault();
            }
            else if (entity.UserTypeId == (int)UserTypeEnum.Factory)
            {
                contactId = entity.FactoryContactId.GetValueOrDefault();
            }

            var userAccountItem = new UserAccountItem
            {
                Id = entity.Id,
                Password = DecryptPassword(entity.Password),
                Status = entity.Active,
                UserName = entity.LoginName,
                Roles = roleList,
                RoleName = roleName,
                Fullname = entity.FullName,
                ContactList = contactList,
                Contact = contactId,
                // PrimaryEntity = entity.PrimaryEntity,
                UserRoleEntityList = roleEntityList
            };

            return userAccountItem;

        }

        private string EncryptPassword(string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(plainTextBytes);
        }

        public ItUserMaster MapUserAccountEntity(UserAccountItem request, int createdby)
        {

            if (request == null)
                return null;
            ICollection<ItUserRole> roles = new List<ItUserRole>();
            if (request.UserRoleEntityList != null && request.UserRoleEntityList.Any())
            {
                foreach (var userRole in request.UserRoleEntityList)
                {
                    // each role will be assigned to diffrent entity
                    foreach (var entity in userRole.RoleEntity)
                    {
                        ItUserRole role = new ItUserRole();
                        role.UserId = request.Id;
                        role.RoleId = userRole.RoleId;
                        role.EntityId = entity;
                        roles.Add(role);
                    }
                }
            }
            ItUserMaster user = new ItUserMaster
            {
                Active = true,
                LoginName = request.UserName.Trim(),
                Password = EncryptPassword(request.Password.Trim()),
                FullName = request.Fullname.Trim(),
                UserTypeId = (int)request.UserTypeId,
                ItUserRoles = roles,
                CreatedOn = DateTime.Now,
                CreatedBy = createdby
                //  PrimaryEntity = request.PrimaryEntity


            };

            switch (request.UserTypeId)
            {
                case (int)UserTypeEnum.InternalUser:
                    user.StaffId = request.UserId;
                    break;
                case (int)UserTypeEnum.Customer:
                    user.CustomerId = request.UserId;
                    user.CustomerContactId = request.Contact;
                    break;
                case (int)UserTypeEnum.Supplier:
                    user.SupplierId = request.UserId;
                    user.SupplierContactId = request.Contact;
                    break;
                case (int)UserTypeEnum.Factory:
                    user.FactoryId = request.UserId;
                    user.FactoryContactId = request.Contact;
                    break;
                case (int)UserTypeEnum.OutSource:
                    user.StaffId = request.UserId;
                    break;
                default:
                    break;
            }

            return user;
        }

        public ItUserMaster MapEditUserAccountEntity(ItUserMaster entity, UserAccountItem request, int updatedby)
        {

            if (request == null || entity == null)
                return null;

            entity.LoginName = request.UserName.Trim();
            entity.Password = EncryptPassword(request.Password.Trim());
            entity.FullName = request.Fullname.Trim();
            entity.Active = request.Status;
            entity.UpdatedBy = updatedby;
            entity.UpdatedOn = DateTime.Now;

            if (entity.UserTypeId == (int)UserTypeEnum.Customer)
            {
                entity.CustomerContactId = request.Contact;
            }
            else if (entity.UserTypeId == (int)UserTypeEnum.Supplier)
            {
                entity.SupplierContactId = request.Contact;
            }
            else if (entity.UserTypeId == (int)UserTypeEnum.Factory)
            {
                entity.FactoryContactId = request.Contact;
            }

            return entity;
        }
        public UserAccount MapAccountCountry(UserAccount user, List<CommonDataSource> suppliercountry)
        {
            var addresscountry = suppliercountry.Where(x => x.Id == user.Id).Select(x => x.Name).ToList();
            user.Country = string.Join(",", addresscountry);
            return user;
        }
    }
}
