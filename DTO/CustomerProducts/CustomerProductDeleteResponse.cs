using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerProducts
{
    public class CustomerProductDeleteResponse
    {
        public int Id { get; set; }

        public CustomerProductDeleteResult Result { get; set; }
    }
    public enum CustomerProductDeleteResult
    {
        Success = 1,
        NotFound = 2,
        MappedtoInsp=3
    }
}
