using System;
using System.Collections.Generic;
using System.Text;

namespace FileGenerationComponent
{
    public class MapDataAttribute : Attribute
    {

        public string Type { get; set;  }
        
        public string  Parameter { get; set;  }

    }
}
