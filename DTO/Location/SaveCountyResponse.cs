using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class SaveCountyResponse
    {
        public int CountyId { get; set; }

        public SaveCountyResult Result { get; set; }
    }
    public enum SaveCountyResult
    {
        Success = 1,
        CannotSaveCounty = 2,
        CurrentCountyNotFound = 3,
        CannotMapRequestToEntites = 4,
        CountyExists = 5

    }
}
