
using DTO.CommonClass;
using System.Collections.Generic;

namespace DTO.Customer
{
    public class CustomerCheckPoint
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CheckPointName { get; set; }
        public string Remarks { get; set; }
        public string ServiceName { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int CheckPointId { get; set; }
        public List<int> BrandList { get; set; }
        public List<int> DeptList { get; set; }
        public List<int> ServiceTypeList { get; set; }
        public string BrandNames { get; set; }
        public string DeptNames { get; set; }
        public string ServiceTypeNames { get; set; }
        public List<int> CountryIdList { get; set; }
        public string CountryNames { get; set; }
    }

    public class CommonCheckPointDataSourceResponse
    {
       public List<int> CheckPointList { get; set;    }

        public CustomerCheckPointResult Result { get; set; }
    }

    public enum CustomerCheckPointResult
    {
        Success=1,
        NotFound=2
    }

    public class CommonCheckPointDataSource
    {
        public int Id { get; set; }
        public int CheckPointId { get; set; }
        public string Name { get; set; }
    }

    public class CommonCheckPointServiceTypeDataSource
    {
        public int Id { get; set; }
        public int CheckPointId { get; set; }
        public string Name { get; set; }
        public int ServiceTypeId { get; set; }
    }
}
