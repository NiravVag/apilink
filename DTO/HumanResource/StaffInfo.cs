using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class StaffInfo
    {
        public int Id { get; set; }

        public string StaffName { get; set; }

        public int? LocationId { get; set; }

        public string LocationName { get; set; }

        public int? CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public int CountryId { get; set; }

        public string Email { get; set; }

        public int UserTypeId { get; set; }
        public int EmployeeTypeId { get; set; }
    }

    public class BookingStaffInfo
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public int EmployeeTypeId { get; set; }
    }

    public class StaffBaseData
    {
        public int Id { get; set; }

        public string StaffName { get; set; }

        public string Email { get; set; }

        public IEnumerable<HrStaffProfile> HrStaffProfiles { get; set; }
    }
}
