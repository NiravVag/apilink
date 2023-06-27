using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class ModuleResponse
    {
        public IEnumerable<APIModule> ModuleList { get; set; }

        public ModuleResult Result { get; set; }
    }

    public class APIModule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }

    public enum ModuleResult
    {
        Success = 1,
        CannotGetModules = 2
    }
}
