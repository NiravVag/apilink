using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCustomerContactResponse
    {
        public int Id { get; set; }
        public SaveCustomerContactResult Result { get; set; }
        public List<ErrorData> ErrorList { get; set; }
    }

    public enum SaveCustomerContactResult
    {
        Success = 1,
        CustomerContactIsNotSaved = 2,
        CustomerContactIsNotFound = 3,
        CustomerContactExists = 4,
        CustomErrors=5,
        DuplicateEmailIDExists=6
    }
}
