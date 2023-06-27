using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class EditDfCustomerConfiguration
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ModuleId { get; set; }
        public int ControlTypeId { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public string FbReference { get; set; }
        public int? DataSourceType { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsBooking { get; set; }
        public List<EditDfControlAttributes> ControlAttributeList { get; set; }
    }

    public class EditDfControlAttributes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public int AttributeId { get; set; }
        public int ControlAttributeId { get; set; }
        public int ControlTypeId { get; set; }
    }

    public class DfCustomerConfigBaseData
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ModuleId { get; set; }
        public int ControlTypeId { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string FbReference { get; set; }
        public string DataType { get; set; }
        public int? DataSourceType { get; set; }
        public int DisplayOrder { get; set; }

    }

    //public class EditDfControlAttributesRepo
    //{
    //    public int Id { get; set; }
    //    public IEnumerable<DfControlAttribute> DfControlAttributes { get; set; }
    //}

    
}
