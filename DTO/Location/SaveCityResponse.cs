using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class SaveCityResponse
    {
        public int CityId { get; set; }

        public SaveCityResult Result { get; set; }
    }
    public enum SaveCityResult
    {
        Success = 1,
        CannotSaveCity = 2,
        CurrentCityNotFound = 3,
        CannotMapRequestToEntites = 4,
        CityExists=5

    }
}
