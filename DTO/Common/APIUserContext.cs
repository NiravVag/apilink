using Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DTO.Common
{

    public class APIUserContext : IAPIUserContext
    {
        IHttpContextAccessor httpContextAccessor;

        public APIUserContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int CustomerId
        {
            get
            {
                return GetClaim<int>(httpContextAccessor, "CustomerId");
            }
        }

        public int SupplierId
        {
            get
            {
                return GetClaim<int>(httpContextAccessor, "SupplierId");
            }
        }
        public int LocationId
        {
            get
            {
                return GetClaim<int>(httpContextAccessor, "LocationId");
            }
        }
        public int FactoryId
        {
            get
            {
                return GetClaim<int>(httpContextAccessor, "FactoryId");
            }
        }
        public UserTypeEnum UserType
        {
            get
            {
                return GetClaim<UserTypeEnum>(httpContextAccessor, "UserTypeId");
            }
        }
        public int UserId
        {
            get
            {
                return GetClaim<int>(httpContextAccessor, ClaimTypes.NameIdentifier);
            }
        }
        public int StaffId
        {
            get
            {
                return GetClaim<int>(httpContextAccessor, "StaffId");
            }
        }

        public string EmailId
        {
            get
            {
                return GetClaim<string>(httpContextAccessor, "EmailId");
            }
        }

        public string UserName
        {
            get
            {
                return GetClaim<string>(httpContextAccessor, "PersonName");
            }
        }

        //public int EntityId
        //{
        //    get
        //    {
        //        return GetClaim<int>(httpContextAccessor, "EntityId");
        //    }
        //}

        public IEnumerable<int> RoleList
        {
            get
            {
                IEnumerable<int> arrRoleList = null;
                var strRoles = GetClaim<string>(httpContextAccessor, "RoleList");
                if (!string.IsNullOrEmpty(strRoles))
                    arrRoleList = strRoles.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => Convert.ToInt32(x));

                return arrRoleList;


            }
        }

        public IEnumerable<int> LocationList
        {
            get
            {
                IEnumerable<int> arrLocationList = null;
                var strLocations = GetClaim<string>(httpContextAccessor, "LocationList");
                if (!string.IsNullOrEmpty(strLocations))
                    arrLocationList = strLocations.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => Convert.ToInt32(x));
                return arrLocationList;
            }
        }

        public IEnumerable<int> UserProfileList
        {
            get
            {
                IEnumerable<int> arrProfileList = null;
                var strUserProfiles = GetClaim<string>(httpContextAccessor, "ProfileList");
                if (!string.IsNullOrEmpty(strUserProfiles))
                    arrProfileList = strUserProfiles.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => Convert.ToInt32(x));
                return arrProfileList;
            }
        }

        private T GetClaim<T>(IHttpContextAccessor context, string key)
        {
            if (context != null && context.HttpContext != null)
            {
                var elements = context.HttpContext.User.Claims.Where(x => x.Type == key)
               .Select(x => x.Value);

                if (elements == null || !elements.Any())
                    return default(T);

                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(T), elements.First(), true);

                //if (typeof(T).GetTypeInfo().IsEnum)
                //    return (T)Enum.Parse(typeof(T), elements.First());
                return (T)Convert.ChangeType(elements.First(), typeof(T));
            }
            else
            {
                return default(T);
            }


        }

        private IEnumerable<T> GetClaimList<T>(IHttpContextAccessor context, string key)
        {

            if (context != null && context.HttpContext != null)
            {
                var elements = context.HttpContext.User.Claims.Where(x => x.Type == key)
               .Select(x => x.Value);

                if (elements == null || !elements.Any())
                    return null;

                return elements.Select(x => (T)Convert.ChangeType(x, typeof(T)));
            }
            else
            {
                return null;
            }

        }
        public string AppBaseUrl
        {
            get
            {
                return $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}";
            }
        }
    }
}
