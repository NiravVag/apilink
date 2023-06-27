using Components.Core.entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FileGenerationComponent.PPT
{
    public class PPTObject : FileObject
    {
        public object ModelRequest { get; set;  }

        public Func<int, string, string> GetFilePath { get; set;  }
        
        public IEnumerable<Image> ImageList { get; set;  }

        public DataSet DataSource { get; set;  }

        public IEnumerable<Variable> VariableList { get; set;  }

        public string FileName { get; set;  }

        public string ModelPath { get; set;  }

    }

}
