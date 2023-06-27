using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class UnitDetailsResponse
    {
        public IEnumerable<Unit> UnitList { get; set; }
        public UnitResult Result { get; set; }
    }
    public enum UnitResult
    {
        Success = 1,
        CannotGetUnitList = 2
    }
}
