using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class County
    {
        public int Id { get; set; }

        public string CountyName { get; set; }

        public string CountyCode { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public int? CountryId { get; set; }

        public int? ProvinceId { get; set; }

        public string ProvinceName { get; set; }

        public int? ZoneId { get; set; }
    }
}
