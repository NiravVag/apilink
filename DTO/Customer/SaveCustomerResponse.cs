using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCustomerResponse
    {
        public int Id { get; set; }
        public SaveCustomerResult Result { get; set; }

        public List<ErrorData> ErrorList { get; set; }
    }

    public enum SaveCustomerResult
    {
        Success = 1,
        CustomerIsNotSaved = 2,
        CustomerIsNotFound = 3,
        CustomerExists = 4,
        CustomerAddressNotFound = 5,
        DuplicateCustomerNameFound = 6,
        DuplicateGLCodeFound = 7,
        DuplicateEmailFound = 8,
        CustomErrorFound = 9,
        CustomerOneBrandRequired = 10,
        CustomerEntiyNotRemoved = 11,
        SameCompanyAsSisterComapny = 12

    }

}

