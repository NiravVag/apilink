using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Core.entities
{
    public class FileProperties
    {
        public string Name { get; set;  }

        public string FileModelName { get; set;  }

        public byte[] Content { get; set;  }

        public string MimeType { get; set;  }

        public FileType FileType { get; set;  }
    }
}
