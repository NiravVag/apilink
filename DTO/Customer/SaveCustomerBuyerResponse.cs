using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCustomerBuyerResponse
    {
        public int Id { get; set; }
        public SaveCustomerBuyerResult Result { get; set; }
        public ErrorData ErrorData { get; set; }
    }

    public enum SaveCustomerBuyerResult
    {
        Success = 1,
        CustomerBuyerIsNotSaved = 2,
        CustomerBuyerIsNotFound = 3,
        CustomerBuyerExists = 4
    }
}
