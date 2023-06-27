using System.Collections.Generic;
using System.Linq;

namespace DTO.DataAccess
{
    public class UserAccess
    {
        public int RoleId { get; set; }
        public int OfficeId { get; set; }
        public int? ServiceId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<int?> ProductCategoryIds { get; set; }
        public IEnumerable<int?> DepartmentIds { get; set; }
        public IEnumerable<int?> BrandIds { get; set; }
        public IEnumerable<int?> CustomerIds { get; set; }
        public IEnumerable<int?> OfficeIds { get; set; }
        public int? FactoryCountryId { get; set; }
        public int? StaffOfficeId { get; set; }
        public UserAccess()
        {
            ProductCategoryIds = Enumerable.Empty<int?>();
            DepartmentIds = Enumerable.Empty<int?>();
            BrandIds = Enumerable.Empty<int?>();
            OfficeIds = Enumerable.Empty<int?>();
            CustomerIds = Enumerable.Empty<int?>();
        }
    }
}
