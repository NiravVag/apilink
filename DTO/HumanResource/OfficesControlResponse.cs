using DTO.Location;
using DTO.OfficeLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class OfficesControlResponse
    {  
        public OfficesControlResult Result { get; set;  }

        public IEnumerable<Office> LocationList { get; set; }

        public IEnumerable<StaffItem> StaffList { get; set; }
    }



    public enum OfficesControlResult
    {
        Success = 1, 
        CannotFindLocations =2, 
        CannotFindStaffList = 3
    }

}
