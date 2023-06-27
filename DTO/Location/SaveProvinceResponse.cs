using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
   public class SaveProvinceResponse
    {
        public int ProvinceId { get; set; }

        public SaveProvinceResult Result { get; set; }
    }
    public enum SaveProvinceResult
    {
        Success = 1,
        CannotSaveProvince = 2,
        CurrentProvinceNotFound = 3,
        CannotMapRequestToEntites = 4,
        ProvinceExists=5
    }
}
