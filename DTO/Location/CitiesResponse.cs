using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CitiesResponse
    {
        public IEnumerable<City> Data { get; set;  }

        public CitiesResult Result { get; set;  }

    }

    public class City
    {
        public int Id { get; set;  }

        public  string Name { get; set;  }

        public string ProvinceName { get; set; }

        public int? ProvinceId { get; set; }

        public string CountryName { get; set; }

        public int? CountryId { get; set; }

        public string OfficeName { get; set; }

        public int? OfficeId { get; set; }

        public string ZoneName { get; set; }

        public int? ZoneId { get; set; }

        public double TravelTimeHH { get; set; }

        public string PhoneCode { get; set; }
    }

    public enum CitiesResult
    {
        Success = 1,
        CannotGetCities = 2
    }

}
