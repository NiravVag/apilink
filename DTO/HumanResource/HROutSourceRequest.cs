using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.HumanResource
{
    public class SaveHROutSourceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SaveHROutSourceResponse
    {
        public SaveHROutSourceResult Result { get; set; }
    }

    public enum SaveHROutSourceResult
    {
        Success = 1,
        NameNotAvailable = 2,
        NameAlreadyExists = 3,
        Failure = 4
    }

    public class HROutSourceCompanyListResponse
    {
        public List<CommonDataSource> hrOutSourceCompanyList { get; set; }
        public HROutSourceCompanyResult Result { get; set; }
    }

    public enum HROutSourceCompanyResult
    {
        Success = 1,
        DataNotAvailable = 2,
        Failure = 3
    }

    public class HROutSourceCompanyRequest
    {
        public int? Id { get; set; }
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class HRStaffDetail
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
    }

    public class HRStaffResponse
    {
        public List<HRStaffDetail> StaffList { get; set; }
        public HRStaffDetailResult Result { get; set; }
    }

    public enum HRStaffDetailResult
    {
        Success=1,
        NotFound=2
    }
    
    public class HRPayrollCompanyListResponse
    {
        public List<CommonDataSource> HRPayrollCompanyList { get; set; }
        public HRPayrollCompanyListResult Result { get; set; }
    }

    public enum HRPayrollCompanyListResult
    {
        Success = 1,
        DataNotAvailable = 2,
        Failure = 3
    }
}
