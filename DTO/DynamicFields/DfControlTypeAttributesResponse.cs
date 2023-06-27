using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DfControlTypeAttributesResponse
    {
        public IEnumerable<DfControlTypeAttributes> DfControlTypeAttributes { get; set; }

        public DFControlTypeAttributeResult Result { get; set; }
    }

    public class DfControlTypeAttributes
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DataType { get; set; }

        public string DefaultValue { get; set; }

        public int ControlTypeId { get; set; }

        public int AttributeId { get; set; }

        public bool Active { get; set; }
    }

    public enum DFControlTypeAttributeResult
    {
        Success = 1,
        CannotGetDFControlTypeAttribute = 2
    }
}
