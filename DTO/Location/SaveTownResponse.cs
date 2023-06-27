using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class SaveTownResponse
    {
        public int TownId { get; set; }

        public SaveTownResult Result { get; set; }
    }
    public enum SaveTownResult
    {
        Success = 1,
        CannotSaveTown = 2,
        CurrentTownNotFound = 3,
        CannotMapRequestToEntites = 4,
        TownExists = 5

    }
}
