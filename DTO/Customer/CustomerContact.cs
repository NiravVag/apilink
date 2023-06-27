using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerContact
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public int Id { get; set; }
        public string ContactName { get; set; }
        public string LastName { get; set; }

        public string ContactEmail { get; set; }

        public string JobTitle { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Mobile { get; set; }

        public string Comment { get; set; }

        public string Email { get; set; }

        public string Others { get; set; }

        public int? Office { get; set; }
        public string Comments { get; set; }
        public int ContactType { get; set; }
        public string ContactTypeName { get; set; }
        public bool? PromotionalEmail { get; set; }
        public int? CustomerId { get; set; }

		public string Brand { get; set; }

		public string Department { get; set; }

		public string Service { get; set; }

        public bool Active { get; set; }

        public int? ReportTo { get; set; }
        public string ReportToName { get; set; }

    }

    public class CustomerContactBaseDetails
    {
        public int Id { get; set; }
        public string ContactName { get; set; }
        public string LastName { get; set; }

        public string ContactEmail { get; set; }

        public string JobTitle { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Mobile { get; set; }

        public string Comment { get; set; }

        public string Email { get; set; }

    }

}
