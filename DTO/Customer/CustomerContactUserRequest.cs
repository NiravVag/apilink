using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CustomerContactUserRequest
    {
        public int ContactId { get; set; }
        public string Fullname { get; set; }
        public int? UserTypeId { get; set; }
        public int? CustomerId { get; set; }
        public string UserName { get; set; }

        public int? CreatedBy { get; set; }
    }
}
