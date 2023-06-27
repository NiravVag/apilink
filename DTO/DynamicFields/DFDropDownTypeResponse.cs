using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DFParentDropDownResponse
    {
        public List<DfParentDDLSource> dfParentDDLList { get; set; }
        public DFParentDropDownResult Result { get; set; }
    }

    public enum DFParentDropDownResult
    {
        Success=1,
        NotFound=2
    }
}
