using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Table("DM_details")]
    public class DMDetails
    {
        public int Id { get; set;  }

        public int UserId { get; set; }
                
        public int TreeId { get; set;  }

        public string FileId { get; set;  }

        public string FileName { get; set;  }

        public string FileType { get; set;  }

        public string Description { get; set;  }

        public int? FileSize { get; set; }

        public DateTime CreateDate { get; set;  }

        public int? CustomerId { get; set;  }

        public int ModuleId { get; set;  }

        public bool Active { get; set;  }

        public DmRefModule DmModule { get; set;  }

        public CuCustomer Customer { get; set; }

        public ItUserMaster User { get; set; }

    }
}
