using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class SaveCSAllocation
    {
        public SaveCSAllocation()
        {
            ProductCategoryIds = new List<int>();
            ProductCategoryIds = new List<int>();
            BrandIds = new List<int>();
        }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public IEnumerable<int> ServiceIds { get; set; }
        public IEnumerable<int> BrandIds { get; set; }
        [Required]
        public IEnumerable<int> OfficeIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        [Required]
        public List<SaveSelectdStaff> Staffs { get; set; }
        public bool IsEdit { get; set; }

        public IEnumerable<int> FactoryCountryIds { get; set; }

    }

    public class SaveSelectdStaff
    {
        public SaveSelectdStaff()
        {
            Notification = new List<int>();
        }
        /// <summary>
        /// comes user id 
        /// </summary>
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public int Profile { get; set; }
        public IEnumerable<int> Notification { get; set; }
        public bool? PrimaryCS { get; set; }
        public bool? PrimaryReportChecker { get; set; }
    }
    public class SaveDaUserCustomerItem
    {
        public IEnumerable<int> ServiceIds { get; set; }
        public IEnumerable<int> BrandIds { get; set; }
        public IEnumerable<int> OfficeIds { get; set; }
        public IEnumerable<int> FactoryCountryIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        public IEnumerable<int> NotificationIds { get; set; }
        public bool? PrimaryCS { get; set; }
        public bool? PrimaryReportChecker { get; set; }
        public int EntityId { get; set; }
        public int Profile { get; set; }
        public int CreatedBy { get; set; }
        public int CustomerId { get; set; }
    }

    public class CSAllocationDeleteItem
    {
        public IEnumerable<int> DaUserCustomerIds { get; set; }
    }
}
