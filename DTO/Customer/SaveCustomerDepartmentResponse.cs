using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCustomerDepartmentResponse
    {
        public int Id { get; set; }
        public SaveCustomerDepartmentResult Result { get; set; }
        public ErrorData ErrorData { get; set; }
    }

    public enum SaveCustomerDepartmentResult
    {
        Success = 1,
        CustomerDepartmentIsNotSaved = 2,
        CustomerDepartmentIsNotFound = 3,
        CustomerDepartmentExists = 4
    }
}
