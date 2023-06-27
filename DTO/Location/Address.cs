using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class Address
    {
        public int Id { get; set;  }

        public  int CountryId { get; set;  }

        public int RegionId { get; set;  }

        public int CityId { get; set;  }

        public int? CountyId { get; set; }

        public int? TownId { get; set; }

        public string ZipCode { get; set; }

        public string Way { get; set; }

        public int AddressTypeId { get; set;  }

        public string LocalLanguage { get; set;  }

        public decimal? Longitude { get; set;  }

        public decimal? Latitude { get; set;  }

    }
}
