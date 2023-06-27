using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{

    public class ModuleListResponse
    {
        public IEnumerable<ModuleItem> Data { get; set; }

        public ModuleListResult Result {get ;set; }
    }

   public class ModuleItem
   {
        public int Id { get; set;  }

        public string Name { get; set;  }
    }

    public enum ModuleListResult
    {
        Success = 1, 
        NotFound = 2
    }
}
