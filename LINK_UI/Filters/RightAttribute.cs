using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Filters
{
    public class RightAttribute : Attribute
    {
        public RightAttribute(params string[] paths)
        {
            this.Paths = paths;
        }

        public string[] Paths { get; set;  }
    }
}
