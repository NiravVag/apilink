using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CountiesResponse
    {
        public County Data { get; set; }

        public IEnumerable<County> DataList { get; set; }

        public CountiesResult Result { get; set; }
    }

    public enum CountiesResult
    {
        Success = 1,
        CannotGetStates = 2
    }
}
