using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DataManagement
{
    public class ModuleListResponse
    {
        public IEnumerable<DmModule> List { get; set;  }

        public ModuleListResult Result { get; set;  }
    }

    public class DmModule
    {
       public int Id { get; set;  }
       
       public int?  ParentId { get; set;  }

       public string ModuleName { get; set;  }

       public int Ranking { get; set;  }

       public bool NeedCustomer { get; set;  }

        public IEnumerable<DmModule>  Children { get; set;  }

    }

    public enum ModuleListResult
    {
        Success = 1,
        NotFound = 3, 
        NotAUthroized  = 4
    }
}
