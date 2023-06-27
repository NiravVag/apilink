using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CustomerSalesCountries")]
    public partial class CuCustomerSalesCountry
    {
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("sales_country_id")]
        public int SalesCountryId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuCustomerSalesCountries")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("SalesCountryId")]
        [InverseProperty("CuCustomerSalesCountries")]
        public virtual RefCountry SalesCountry { get; set; }
    }
}