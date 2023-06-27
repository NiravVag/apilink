using DTO.Location;
using DTO.OfficeLocation;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class EditStaffResponse
    {
        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<Position> PositionList { get; set; }

        public IEnumerable<Qualification> QualificationList { get; set;  }

        public IEnumerable<StaffItem> ReportHeadList { get; set; }

        public IEnumerable<EmployeeType> EmployeeTypeList { get; set; }

        public IEnumerable<Office> OfficeList { get; set; }

        public IEnumerable<Department> DepartmentList { get; set; }

        public IEnumerable<Department> SubDepartmentList { get; set;  }

        public IEnumerable<Profile> ProfileList { get; set; }

        public IEnumerable<Currency> CurrencyList { get; set; }

        public IEnumerable<MarketSegment> MarketSegmentList { get ;set;}

        public IEnumerable<ProductCategory> ProductCategoryList { get; set;  } 

        public IEnumerable<Expertise> ExpertiseList { get; set;  }

        public IEnumerable<FileType> FileTypeList { get; set;  }

        public IEnumerable<City> HomeCityList { get; set;  }

        public IEnumerable<State> HomeStateList { get; set; }

        public IEnumerable<City> CurrentCityList { get; set; }

        public IEnumerable<State> CurrentStateList { get; set; }

        public StaffDetails Staff { get; set;  }

        public EditStaffResult Result { get; set;  }

    }

    public enum EditStaffResult
    {
        Success = 1,
        CannotGetCountryList = 2,
        CannotGetPositionList = 3,
        CannotGetQualificationList = 4,
        CannotGetReportHeadList = 5,
        CannotGetEmployeeTypes = 6,
        CannotGetOfficeList = 7,
        CannotGetDepartmentList = 8,
        CannotGetProfileList = 9,
        CannotGetCurrencyList = 10,
        CannotGetMarketSegmentList = 11,
        CannotGetProductCategoryList = 12,
        CannotGetExpertiseList = 13,
        CannotGetFileTypeList = 14,
        CannotGetStaff = 15,
        CannotGetStateList = 16,
        CannotGeCityList = 17,
        CannotGetEntities = 18
    }
}
