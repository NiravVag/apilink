using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierListResponse
    {
        public IEnumerable<SupplierItem> Data { get; set; }

        public SupplierListResult Result { get; set; }
    }
    public class SupplierAddress
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string RegionalAddress { get; set; }
        public int SupplierAddresstype { get; set; }
        public int SupplierId { get; set; }
        public int countryId { get; set; }
        public string Country { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public string SupplierName { get; set; }
        public string RegionalSupplierName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public SupplierListResult Result { get; set; }
    }

    public class SupplierAddressData
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string RegionalAddress { get; set; }

        public int SupplierId { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string RegionName { get; set; }
        public string RegionalLanguageName { get; set; }
        public string TownName { get; set; }
        public string CountyName { get; set; }

        public string ZipCode { get; set; }
        public string OfficeType { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }

        public int? CountyId { get; set; }
        public int? TownId { get; set; }
    }

    public class SupplierInvolvedData
    {
        public int SupplierId { get; set; }
        public bool InspectionSupplierAvailable { get; set; }
        public bool AuditSupplierAvailable { get; set; }
        public bool AuditFactoryAvailable { get; set; }
        public bool PurchaseOrderSupplierAvailable { get; set; }
        public bool PurchaseOrderFactoryAvailable { get; set; }
        public bool SupplierFactoryMapAvailable { get; set; }
        public bool InspectionFactoryAvailable { get; set; }
        public bool SupplierParentAvailable { get; set; }
    }

    public enum SupplierListResult
    {
        Success = 1,
        NodataFound = 2
    }

    public class SupplierContactDataResponse
    {
        public List<suppliercontact> ContactList { get; set; }
        public SupplierContactDataResult Result { get; set; }
    }

    public enum SupplierContactDataResult
    {
        Success = 1,
        NotFound = 2
    }
}
