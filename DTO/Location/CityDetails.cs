using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Location
{
    public class CityDetails
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }       
        public string CityName { get; set; }
        public int CountryId { get; set; }
    }
}
