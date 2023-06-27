using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class UserStaffDetails
    {
        public int Id { get; set; }

        public int StaffId { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }

        public string LocationName { get; set; }

        public int EmployeeTypeId { get; set; }

        //public int LocationId { get; set; }

    }
    public class AEDetails
    {
        public int Id { get; set; }

        public int StaffId { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }

        public int Customerid { get; set; }

        public bool CustomerAE { get; set; }

        public int UserType { get; set; }

    }

    public class AECustomerList
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public List<int> Customerid { get; set; }

    }
}
