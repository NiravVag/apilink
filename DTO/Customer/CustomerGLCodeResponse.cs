using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{

    public class CustomerGLCodeSource
    {
        public int Id { get; set; }
        public string GLCode { get; set; }
        public string Name { get; set; }
    }
    public enum CustomerGLCodeSourceResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3
    }
    public class CustomerGLCodeResponse
    {
        public IEnumerable<CustomerGLCodeSource> DataSourceList { get; set; }
        public CustomerGLCodeSourceResult Result { get; set; }
    }
}
