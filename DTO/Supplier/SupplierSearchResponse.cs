using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierSearchResponse
    {
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<SupplierItem> Data { get; set; }

        public SupplierSearchResult Result { get; set; }
    }

    public class SupplierItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }

        public string RegionalLanguageName { get; set; }

        public string RegionName { get; set; }

        public string CityName { get; set; }

        public string CountyName { get; set; }

        public string TownName { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Way { get; set; }

        public bool CanBeDeleted { get; set; }

        public IEnumerable<SupplierItem> List { get; set; }
    }

    public class SupplierSearchItemResponse
    {
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<SupplierSearchItem> Data { get; set; }

        public SupplierSearchResult Result { get; set; }
    }


    public class SupplierSearchItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public string RegionName { get; set; }
        public string RegionalLanguageName { get; set; }

        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string TownName { get; set; }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public bool CanBeDeleted { get; set; }
    }

    public class SupplierSearchItemRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string LocalName { get; set; }

        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public int CustomerId { get; set; }

        public string Address { get; set; }
        public string RegionalAddress { get; set; }

        public int SupplierId { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string RegionalLanguageName { get; set; }
        public string TownName { get; set; }
        public string CountyName { get; set; }
    }

    public enum SupplierSearchResult
    {
        Success = 1,
        NotFound = 2

    }

    public class SupplierExportDataResponse
    {
        public List<SupplierExportItem> SupplierExportList { get; set; }
        public SupplierSearchResult Result { get; set; }
    }

    public class SupplierExportItem
    {

        public string Name { get; set; }

        [Description("Regional Name")]
        public string RegionalName { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        //multiple
        public string Service { get; set; }

        [Description("Contact Person")]
        public string ContactPerson { get; set; }
        [Description("GL Code")]
        public string GLCode { get; set; }
        public string Status { get; set; }

        //address
        public string Address { get; set; }
        [Description("Regional Language Address")]
        public string RegionalLanguageAddress { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Town { get; set; }
        public string OfficeType { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        //multiple
        [Description("Customer")]
        public string CustomerWithCode { get; set; }

        //( name + email + phno) keep email & phno in bracket
        public string Contact { get; set; }

        //if type factory
        public string Supplier { get; set; }
    }

    public class SupplierCustomerRepo
    {
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
    }

    public class SupplierContactRepo
    {
        public int SupplierId { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class SupplierExportRepo
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
    }

    public class SupplierServiceExportRepo
    {
        public int? SupplierId { get; set; }
        public string ServiceName { get; set; }
    }

    public class SupplierItemData
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string RegionalName { get; set; }
        public string Type { get; set; }
        public List<string> Service { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string ContactPerson { get; set; }
        public string GLCode { get; set; }
        public int? TypeId { get; set; }
    }

    public class SupplierCustomerData
    {
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
