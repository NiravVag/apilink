using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CustomerBusinessCountry")]
    public partial class CuCustomerBusinessCountry
    {
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("business_country_id")]
        public int BusinessCountryId { get; set; }

        [ForeignKey("BusinessCountryId")]
        [InverseProperty("CuCustomerBusinessCountries")]
        public virtual RefCountry BusinessCountry { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuCustomerBusinessCountries")]
        public virtual CuCustomer Customer { get; set; }
    }
}