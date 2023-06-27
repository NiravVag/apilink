using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class SaveOfficeControlResponse
    {
        public int StaffId { get; set;  }

        public SaveOfficeControlResult Result { get; set;  }
    }

    public enum SaveOfficeControlResult
    {
        Success = 1, 
        CannotSave =2
    }
}
