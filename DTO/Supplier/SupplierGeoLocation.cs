using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierGeoLocation
    {
        public int FactoryId { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Town { get; set; }
        public int CountryId { get; set; }
    }
}
