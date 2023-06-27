using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
   public class SeasonYearResponse
    {
       public IEnumerable<SeasonYear> SeasonYearList { get; set;}
        public SeasonYearResponseResult Result { get; set; }
    }
    public enum SeasonYearResponseResult
    {
        success=1,
        error =2
    }
}
