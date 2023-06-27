using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
   public class EditProvinceResponse
    {
        public IEnumerable<Country>Countryvalues { get; set; }
        public State ProvinceDetails { get; set; }
        public EditProvinceResult Result { get; set; }
    }
    public enum EditProvinceResult
    {
        success=1,
        CanNotGetCountryList=2,
        CanNotGetProvinceDetails=3
    }
}
