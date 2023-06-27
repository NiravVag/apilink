using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
   public class CustomerAccountingAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string EnglishAddress { get; set; }
        public int AddressType { get; set; }
    }
}
