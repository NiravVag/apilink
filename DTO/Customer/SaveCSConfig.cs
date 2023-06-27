using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCSConfig
    {
        public int Id { get; set; }
        public IEnumerable<int?> CustomerId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<int> OfficeLocationId { get; set; }
        public IEnumerable<int?> ServiceId { get; set; }
        public IEnumerable<int?> ProductCategoryId { get; set; }
    }
    public class SaveOneCSConfig
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int UserId { get; set; }
        public int OfficeLocationId { get; set; }
        public int? ServiceId { get; set; }
        public int? ProductCategoryId { get; set; }
    }
    public class SaveCSConfResponse
    {
        public int CSConfigId { get; set; }
        public CSResult Result { get; set; }
    }
    public class CSConfResponse
    {
        public IEnumerable<SaveOneCSConfig> CustomerServiceConfigList { get; set; }
        public CSResult Result { get; set; }
    }
}
