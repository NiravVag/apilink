using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DFDDLSourceTypeResponse
    {
        public IEnumerable<DDLSourceType> DDLSourceTypeList { get; set; }

        public DDLSourceTypeResult Result { get; set; }
    }

    public class DDLSourceType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }

    public enum DDLSourceTypeResult
    {
        Success = 1,
        CannotGetDDLSourceTypes = 2
    }
}
