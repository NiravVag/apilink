using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class ControlTypeResponse
    {
        public IEnumerable<ControlType> ControlTypeList { get; set; }

        public ControlTypeResult Result { get; set; }
    }

    public class ControlType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }

    public enum ControlTypeResult
    {
        Success = 1,
        CannotGetControlTypes = 2
    }
}
