using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CSResponse
    {
        public IEnumerable<CustomerCS> CSList { get; set; }
        public CSResult Result { get; set; }
    }
    public enum CSResult
    {
        Success = 1,
        CSIsNotFound = 2
    }
}
