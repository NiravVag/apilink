using DTO.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierSearchRequest
    {

        public int? Index { get; set; }

        public int? pageSize { get; set; }

        public IEnumerable<SupplierType> TypeValues { get; set; }

        public IEnumerable<Country> CountryValues { get; set; }

        public SupplierItem SuppValues { get; set; }
    }

    public class SupplierSearchRequestNew
    {
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public IEnumerable<int> TypeValues { get; set; }
        public IEnumerable<int> CountryValues { get; set; }
        public int? SuppValues { get; set; }
        public int? CustomerId { get; set; }
        public int? provinceId { get; set; }
        public IEnumerable<int> CityValues { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsEAQF { get; set; }

    }
}
