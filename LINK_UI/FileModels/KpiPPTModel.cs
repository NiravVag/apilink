using DTO.Common;
using FileGenerationComponent;
using System;
using System.Collections.Generic;

namespace LINK_UI.FileModels
{
    public class KpiPPTModel
    {
        public DateTime StartDate { get; set;  }

        public DateTime EndDate { get; set; }

        public int  CustomerId { get; set;  }

        public bool ByDepartment { get; set;  }
        
        public bool ByFactoryLevel { get; set;  }
        
        [MapData(Type = "Udt_Int")]
        public IEnumerable<int> ListBrand { get; set; }

        [MapData(Type = "Udt_Int")]
        public IEnumerable<int> ListDepartment { get; set; }

        public int? EntityId { get; set; }

        public int SearchDateTypeId { get; set; }
    }

    public class KpiPPTRequest
    {
        public int IdCustomer { get; set;  }

        public DateObject  BeginDate { get; set;  }

        public DateObject EndDate { get; set; }

        public bool LoadDepartment { get; set;  }

        public bool LoadFactory { get; set;  }

        public IEnumerable<int> BrandList { get; set; }

        public IEnumerable<int> DeptList { get; set; }

        public int SearchDateTypeId { get; set; }
    }
}
