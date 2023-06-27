using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
   
    public class CustomerBrandDetails
    {
        public int Id { get; set; }     
        public List<CustomerBrand> CustomerBrands { get; set; }

    }
}
