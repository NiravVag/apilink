using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO.UserConfig
{
    public class UserConfigSaveRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public bool EmailAccess { get; set; }
        public List<UserAccessMasterData> userAccessList { get; set; }
    }

    public class UserAccessMasterData
    {
        public int? CustomerId { get; set; }
        public IEnumerable<int> RoleIdAccessList { get; set; }
        public IEnumerable<int?> ProductCategoryIdAccessList { get; set; }
        public IEnumerable<int?> OfficeIdAccessList { get; set; }
        public IEnumerable<int?> ServiceIdAccessList { get; set; }
        public IEnumerable<int?> CusDepartmentIdAccessList { get; set; }
        public IEnumerable<int?> CusBrandIdAccessList { get; set; }
        public IEnumerable<CommonDataSource> CustomerDepartmentList { get; set; }
        public IEnumerable<CommonDataSource> CustomerBrandList { get; set; }
        public IEnumerable<int?> CusBuyerIdAccessList { get; set; }
    }

    public class UserConfigSaveResponse
    {
        public int Id { get; set; }
        public ResponseResult Result { get; set; }
    }

    public enum ResponseResult
    {
        Success = 1,
        Faliure = 2,
        RequestNotCorrectFormat = 3,
        Error = 4,
        NotFound = 5
    }
    public class UserConfigEditResponse
    {
        public UserConfigSaveRequest data { get; set; }
        public ResponseResult Result { get; set; }

        public UserConfigEditResponse()
        {
            data = new UserConfigSaveRequest
            {
                userAccessList = new List<UserAccessMasterData>()
            };
        }
    }
}
