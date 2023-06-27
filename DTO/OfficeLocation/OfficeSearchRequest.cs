using DTO.OfficeLocation;
using System.Collections.Generic;

namespace DTO.OfficeLocation
{
    public class OfficeSearchRequest
    {
        public IEnumerable<Office> OfficeValues { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
}
