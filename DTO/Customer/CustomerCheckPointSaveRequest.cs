using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Customer
{
    public class CustomerCheckPointSaveRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int CheckPointId { get; set; }
        [StringLength(1000)]
        public string Remarks { get; set; }
        public List<int> BrandId { get; set; }
        public List<int> DeptId { get; set; }
        public List<int> ServiceTypeId { get; set; }
        public List<int> CountryIdList { get; set; }
        public int EntityId { get; set; }
    }
}
