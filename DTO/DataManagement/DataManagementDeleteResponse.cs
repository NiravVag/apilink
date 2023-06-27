using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DataManagement
{
    public class DataManagementDeleteResponse
    {
        public int Id { get; set; }

        public DataManagementDeleteResult Result { get; set; }
    }

   
    public enum DataManagementDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
