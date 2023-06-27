using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class Country
    {
        public int Id { get; set;  }

        public int? countrycode { get; set; }

        public string CountryName { get; set;  }

        public string Area { get; set; }

        public int? AreaId { get; set; }

        public string alphacode { get; set; }

        public int Priority { get; set; }
    }
}
