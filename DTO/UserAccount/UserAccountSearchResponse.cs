using System;
using System.Collections.Generic;
using System.Text;


namespace DTO.UserAccount
{
    public class UserAccountSearchResponse
    {
        public IEnumerable<UserAccount> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public UserAccountSearchResult Result { get; set; }
    }

    public enum UserAccountSearchResult
    {
        Success = 1,
        NotFound = 2,
        CannotGetUserAccount = 3
    }

    public class UserAccountSearchData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string LocationName { get; set; }
        public string CountryName { get; set; }
        public bool HasAccount { get; set; }
        public int UserTypeId { get; set; }
        public int NationalityCountryId { get; set; }
        public string PersonName { get; set; }
        public int EmployeeTypeId { get; set; }
    }
}
