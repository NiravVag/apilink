using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DataManagement
{
    public class DataManagementItemResponse
    {
        public DataManagementItem Item { get; set;  }

        public DataManagementItemResult Result { get; set;  }
    }

    public enum DataManagementItemResult
    {
        Success  = 1,
        NotFound  =  2,
        NotAuthorized = 3
    }
}
