using DTO.Invoice;
using System.Collections.Generic;

namespace DTO.CommonClass
{
    public class CommonDTO
    {
    }

    public class CommonDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CommonBankDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public List<BankTaxData> TaxList { get; set; }

    }

    public class CommonPriceCardDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceCardId { get; set; }
    }

    public class CommonId
    {
        public int Id { get; set; }
    }

    public enum DataSourceResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3,
        RequestNotCorrectFormat = 4,
        ServiceIdRequired = 5

    }
    public class DataSourceResponse
    {
        public IEnumerable<CommonDataSource> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }

    public class CurrencyDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CurrencyDataSourceResponse
    {
        public IEnumerable<CurrencyDataSource> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }



    public class PaymentTypeResponse
    {
        public IEnumerable<PaymentTypeSource> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }

    public class PaymentTypeSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Duration { get; set; }
    }

    public class ParentDataSourceResponse
    {
        public IEnumerable<ParentDataSource> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }
    public class ParentDataSource
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
    }
    public class CommonDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int? CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int SupplierType { get; set; }
        public int? Id { get; set; }
        public int? SupplierId { get; set; }
        public int? LocationId { get; set; }
        public List<int> IdList { get; set; }
        public int SupSearchTypeId { get; set; }
        public bool ShowLocation { get; set; } = true;
        public bool ShowAllData { get; set; } = false;
        public IEnumerable<string> CustomerGLCodes { get; set; }
        public int? ServiceId { get; set; }
        public bool IsStatisticsVisible { get; set; } = false;
        public bool IsVirtualScroll { get; set; } = true;
    }

    public class StaffDataSourceRequest : CommonDataSourceRequest
    {
        public int? EmployeeType { get; set; }
        public int? OutSourceCompanyId { get; set; }
        public List<int> LocationIdList { get; set; }
    }


    public class CustomerSubCategory
    {
        public int CustomerId { get; set; }
        public List<int?> ProductCategory { get; set; }
    }

    public class CustomerSubCategory2Request
    {
        public List<int?> ProductCategory { get; set; }
        public List<int?> ProductSubCategory { get; set; }
    }

    public class UserDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int? CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int SupplierType { get; set; }
        public int? Id { get; set; }
        public int? SupplierId { get; set; }
        public int? LocationId { get; set; }
        public List<int> IdList { get; set; }
    }

    public class CommonCountrySourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int?> CountryIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CommonSupplierSourceRequest
    {
        public int? Id { get; set; }
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<int?> CountryIds { get; set; }
        public IEnumerable<int?> SupplierTypes { get; set; }
        public int? CustomerId { get; set; }
        public int? ProvinceId { get; set; }
        public IEnumerable<int?> CityIds { get; set; }

        public bool IsRegionalNameChecked { get; set; }
    }

    public class CommonCustomerSourceRequest
    {
        public string SearchText { get; set; }
        public int CustomerId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public List<int> IdList { get; set; }
    }
    public class CommonZoneSourceRequest
    {
        public string SearchText { get; set; }
        public int? LocationId { get; set; }
        public int ZoneId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class CommonOfficeZoneSourceRequest
    {
        public string SearchText { get; set; }
        public List<int?> Officeids { get; set; }
        public int ZoneId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class CommonCountyByCitySourceRequest
    {
        public string SearchText { get; set; }
        public int CityId { get; set; }
        public int CountyId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CommonQcSourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int?> Qcids { get; set; }
        public List<int?> OfficeCountryIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CommonDataListResponse
    {
        public List<int> IdList { get; set; }
        public DataSourceResult Result { get; set; }
    }

    public class CommonProvinceDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
    }

    public class CommonProvinceSourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int?> CountryIds { get; set; }
        public int? ProvinceId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CommonCitySourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int?> CountryIds { get; set; }
        public int? ProvinceId { get; set; }
        public List<int?> CityIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CommonTownSourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int?> CountryIds { get; set; }
        public int? CountyId { get; set; }
        public List<int?> TownIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class StartPortSourceRequest
    {
        public string SearchText { get; set; }
        public List<int?> StartPortIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CommonCityDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public int CountryId { get; set; }
        public int Priority { get; set; }
    }

    public class CommonTownDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountyId { get; set; }
        public int CountryId { get; set; }
    }
    public class CommonCustomerProductDataSource
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
    }
    public class CustomerProductDataSourceResponse
    {
        public IEnumerable<CommonCustomerProductDataSource> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }



    public class CommonAddressDataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressType { get; set; }
        public int? CustomerId { get; set; }
    }

    public class AddressDataSourceResponse
    {
        public IEnumerable<CommonAddressDataSource> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }

    public class CommonBookingIdSourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int?> Ids { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class ProvinceListSearchRequest
    {
        public List<int> CountryIds { get; set; }
    }

    public class CityListSearchRequest
    {
        public List<int> ProvinceIds { get; set; }
    }

    public class ProvinceListResponse
    {
        public List<CommonDataSource> DataSourceList { get; set; }

        public ProvinceResult Result { get; set; }
    }

    public enum ProvinceResult
    {
        Success = 1,
        RequestShouldNotBeEmpty = 2,
        CountrySearchFilterEmpty = 3,
        DataNotFound = 4,
        Failed = 5
    }

    public class CityListResponse
    {
        public List<CommonDataSource> DataSourceList { get; set; }

        public CityResult Result { get; set; }
    }

    public enum CityResult
    {
        Success = 1,
        RequestShouldNotBeEmpty = 2,
        ProvinceSearchFilterEmpty = 3,
        DataNotFound = 4,
        Failed = 5
    }


    public class InspectionBookingTypeResponse
    {
        public List<CommonDataSource> InspectionBookingTypeList { get; set; }

        public InspectionBookingTypeResult Result { get; set; }
    }

    public enum InspectionBookingTypeResult
    {
        Success = 1,
        NotFound = 2
    }

    public class InspectionPaymentOptionsResponse
    {
        public List<CommonDataSource> InspectionPaymentOptions { get; set; }

        public InspectionPaymentOptionsResult Result { get; set; }
    }

    public enum InspectionPaymentOptionsResult
    {
        Success = 1,
        NotFound = 2
    }

    public enum InspectionBookingTypeEnum
    {
        Announced = 1,
        UnAnnounced = 2
    }

    public enum AuditTypeEnum
    {
        Announced = 1,
        SemiAnnounced = 2,
        UnAnnounced = 3
    }

}
