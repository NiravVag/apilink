using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DFDDLSourceResponse
    {
        public IEnumerable<DDLSource> DDLSourceList { get; set; }

        public DDLSourceResult Result { get; set; }
    }

    public class DDLSource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }

    public enum DDLSourceResult
    {
        Success = 1,
        CannotGetDDLSource = 2
    }
}
