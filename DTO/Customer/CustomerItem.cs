using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DTO.Common;
using DTO.CommonClass;

namespace DTO.Customer
{
    public class CustomerItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public int? ComplexityLevel { get; set; }
        public DateObject StartDate { get; set; }
        public string Website { get; set; }
        public string Others { get; set; }
        public int? ProspectStatus { get; set; }
        public int? SkillsRequired { get; set; }
        public int? Kam { get; set; }
        public string Phone { get; set; }
        public int? Category { get; set; }
        public int? MargetSegment { get; set; }
        public IEnumerable<int> BusinessCountry { get; set; }
        public string OtherPhone { get; set; }
        public int? Language { get; set; }
        public int? BusinessType { get; set; }
        public string QuatationName { get; set; }
        public bool? IcRequired { get; set; }
        public string GlCode { get; set; }
        public bool? GlRequired { get; set; }
        public int? Group { get; set; }
        public string GroupName { get; set; }
        public string Comments { get; set; }
        public string BookingDefaultComments { get; set; }
        public int? InvoiceType { get; set; }
        public int? FbCusId { get; set; }
        public int? CreditTerm { get; set; }
        public bool IsStatisticsVisibility { get; set; }
    }

    public class CustomerAddress
    {
        public int Id { get; set; }
        public int AddressType { get; set; }
        [Required]
        [StringLength(1000)]
        public string Address { get; set; }
        [StringLength(100)]
        public string BoxPost { get; set; }
        [StringLength(20)]
        public string ZipCode { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string ZohoCountry { get; set; }
        public string ZohoCity { get; set; }

    }

    public class CrmCustomerAddress
    {
        [Required]
        [StringLength(1000)]
        public string Address { get; set; }
        [StringLength(100)]
        public string BoxPost { get; set; }
        [StringLength(20)]
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

    public class CustomerGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }


    }

    public class CustomerSource
    {
        public int Id { get; set; }

        public string Name { get; set; }


    }

    public class CustomerCustomStatus
    {
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public string CustomStatusName { get; set; }
    }
    public class CustomerDataSourceResponse
    {
        public IEnumerable<CommonDataSource> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }
    public class CustomerDataSource
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
    }
    public class LocationDto
    {
        public int CustomerId { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; }
        public int ProvinceId { get; set; }
        public string Province { get; set; }
    }
}
