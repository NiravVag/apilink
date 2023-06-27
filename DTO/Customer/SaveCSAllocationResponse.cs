using DTO.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CSAllocationResponse
    {
        public IEnumerable<CSAllocation> CSList { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public CSAllocationResult Result { get; set; }
    }
    public enum CSAllocationResult
    {
        Success = 1,
        NotFound = 2,
        StaffAlreadyConfigured=3
    }
}