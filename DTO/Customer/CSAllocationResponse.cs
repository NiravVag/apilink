using DTO.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class SaveCSAllocationResponse
    {        
        public CSAllocationResult Result { get; set; }
    }

    public class DeleteCSAllocationResponse
    {
        public CSAllocationResult Result { get; set; }
    }

    public class GetCSAllocationResponse
    {
        public CSAllocationResult Result { get; set; }
        public SaveCSAllocation CSAllocation { get; set; }
    }
}