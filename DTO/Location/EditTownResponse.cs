using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class EditTownResponse
    {
        public IEnumerable<Country> countryValues { get; set; }

        public IEnumerable<State> provinceValues { get; set; }

        public IEnumerable<City> cityValues { get; set; }

        public IEnumerable<County> countyValues { get; set; }

        public Town townDetails { get; set; }

        public TownResult Result { get; set; }
    }

    public enum TownResult
    {
        Success = 1,
        CannotGetTown = 2
    }
}
