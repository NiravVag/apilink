using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.UserAccount
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DepartmentName { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public string Country { get; set; }
        public bool HasAccount { get; set; }
        public int UserTypeId { get; set; }
    }
}
