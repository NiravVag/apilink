using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
   public class StaffGenderResponse
    {
        public string Gender { get; set; }
        public bool IsPhotovailable { get; set; }
        public StaffGenderResponseResult Result { get; set; }
    }
    public enum StaffGenderResponseResult
    {
        success=1,
        error=2
    }
}
