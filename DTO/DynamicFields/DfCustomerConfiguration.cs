using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DfCustomerConfiguration
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ModuleId { get; set; }
        public int ControlTypeId { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public int? DataSourceType { get; set; }
        public int DisplayOrder { get; set; }
        public string CssClass { get; set; }
        public string FbReference { get; set; }
        public bool Active { get; set; }
        public IEnumerable<DfControlAttributes> ControlAttributeList { get; set; }
        public IEnumerable<DfDDLSource> DDLSourceList { get; set; }
    }

    public partial class DfControlAttributes
    {
        public int Id { get; set; }
        public int ControlAttributeId { get; set; }
        public int? AttributeId { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
    }

    public partial class DfDDLSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool Active { get; set; }
    }

    public class DfParentDDLSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
