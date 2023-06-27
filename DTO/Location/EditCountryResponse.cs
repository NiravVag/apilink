using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class EditCountryResponse
    {
      
        public IEnumerable<Area> AreaList { get; set; }
        public Country CountryDetails { get; set; }
        public EditCountryResult Result { get; set; }
    }
    public enum EditCountryResult
    {
        Success=1,
        CanNotGetAreaList=2,
        CanNotGetCountry=3
    }
}
