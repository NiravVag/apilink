using DTO.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Expense
{
    public class ClaimCitiesResponse
    {
        public ClaimCitiesResponse()
        {
            Items = new List<City>();
        }

        public IEnumerable<City>  Items { get; set;  }
    }
}
