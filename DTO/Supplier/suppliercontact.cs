using DTO.Customer;
using DTO.HumanResource;
using DTO.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class suppliercontact
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public int ContactId { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string JobTitle { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Mobile { get; set; }

        public string Comment { get; set; }

        public List<int> ContactAPIServiceIds { get; set; }
        public List<int> ApiEntityIds { get; set; }
        public IEnumerable<EntityService> EntityServiceIds { get; set; }
        public int? PrimaryEntity { get; set; }
    }
}
