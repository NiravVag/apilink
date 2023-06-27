using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
   public class SaveCountryResponse
    {
        public int CountryId{ get; set; }

        public SaveCountryResult Result { get; set; }
    }
    public enum SaveCountryResult
    {
        Success = 1,
        CannotSaveCountry = 2,
        CurrentCountryNotFound = 3,
        CannotMapRequestToEntites = 4
    }
}
