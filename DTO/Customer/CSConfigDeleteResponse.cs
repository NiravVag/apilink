using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CSConfigDeleteResponse
    {
        public CSConfigDeleteResult Result { get; set; }
    }
    public enum CSConfigDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
