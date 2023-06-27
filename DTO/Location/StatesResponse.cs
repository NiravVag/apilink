using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class StatesResponse
    {
        public IEnumerable<State>  Data { get; set;  }

        public StatesResult Result { get; set;  }
    }

    public class State
    {
        public int Id { get; set;  }

        public string Name { get; set;  }

        public string Code { get; set; }

        public string CountryName { get; set; }

        public int? CountryId { get; set; }
    }

    public enum StatesResult
    {
        Success = 1, 
        CannotGetStates = 2
    }



}
