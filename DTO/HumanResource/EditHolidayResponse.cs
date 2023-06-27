using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class EditHolidayResponse
    {
        public IEnumerable<int> Ids { get; set;  }

        public EditHolidayResult Result { get; set;  }
    }

    public enum EditHolidayResult
    {
        Success = 1,
        HolidayIsNotFound = 2,
        HolidayIsNotSaved =3
    }
}
