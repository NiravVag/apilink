using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class EditCSConfigResponse
    {
        public SaveOneCSConfig CSConfigDetails { get; set; }
        public EditCSConfigResult Result { get; set; }
    }
    public enum EditCSConfigResult
    {
        Success = 1,
        CannotGetCustomer = 2,
    }
}
