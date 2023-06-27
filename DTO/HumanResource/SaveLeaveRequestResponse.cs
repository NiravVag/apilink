using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class SaveLeaveRequestResponse
    {
        public int Id { get; set;  }

         public Guid TaskId { get; set;  }

        public bool SendNotification { get ; set;  }

        public LeaveEmailModel EmailModel { get; set;  }


        public SaveLeaveRequestResult Result { get; set;  }

    }

    public enum SaveLeaveRequestResult
    {
        Success = 1, 
        StartDateIsRequired = 2, 
        EndDateIsRequired = 3, 
        LeaveTypeIsRequired = 4,
        NotFound = 5,
        UnAuthorized = 6,
        CannotSave = 7,
        StaffEntityNotMatched=8
    }
}
