using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class SaveStaffResponse
    {
        public int IdStaff { get; set;  }

        public IEnumerable<AttachedFile> AttachedFileList { get; set;  }

        public SaveStaffResult Result { get; set;  }
    }

    public enum SaveStaffResult
    {
        Success = 1 ,
        CannotAddStaff = 2,
        CurrentSaffNotFound = 3,
        CannotMapRequestToEntites = 4,
        StaffAlreadyExistsWithSameEmail=5
    }
}
