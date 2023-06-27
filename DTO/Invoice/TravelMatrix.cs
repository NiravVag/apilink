using System;
using System.Collections.Generic;
using System.Text;
using DTO.CommonClass;

namespace DTO.Invoice
{
    public class TravelMatrix
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int TariffTypeId { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
        public int? InspPortId { get; set; }
        public int? InspPortCityId { get; set; }
        public double? DistanceKM { get; set; }
        public double? TravelTime { get; set; }
        public double? BusCost { get; set; }
        public double? TrainCost { get; set; }
        public double? TaxiCost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double? MarkUpCost { get; set; }
        public double? AirCost { get; set; }
        public double? MarkUpCostAir { get; set; }
        public double FixExchangeRate { get; set; }
        public string Remarks { get; set; }
        public int? SourceCurrencyId { get; set; }
        public int? TravelCurrencyId { get; set; }
    }
    public class TravelMatrixSearch
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? TariffTypeId { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
        public int? CountyCityId { get; set; }
        public int? InspPortId { get; set; }
        public double? DistanceKM { get; set; }
        public double? TravelTime { get; set; }
        public double? BusCost { get; set; }
        public double? TrainCost { get; set; }
        public double? TaxiCost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double? MarkUpCost { get; set; }
        public double? AirCost { get; set; }
        public double? MarkUpCostAir { get; set; }
        public double? FixExchangeRate { get; set; }
        public string Remarks { get; set; }
        public int? SourceCurrencyId { get; set; }
        public int? TravelCurrencyId { get; set; }

        public string CustomerName { get; set; }
        public string TariffTypeName { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string InspPortName { get; set; }
        public string InspPortCityName { get; set; }
        public string SourceCurrencyName { get; set; }
        public string TravelCurrencyName { get; set; }

        public int? InspPortCityId { get; set; }

       

    }

    public enum TravelMatrixConfig
    {
        Province=1,
        City=2,
        County=3
    }

    public enum TravelMatrixResponseResult
    {
        Success = 1,
        Error = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        DefaultData = 5,
        MoreMatrixExists = 6,
        PriceCardNotCorrect = 7
    }

    public class TravelMatixSaveResponse
    {
        public TravelMatrixResponseResult Result { get; set; }
        public IEnumerable<TravelMatrixExists> ExistsData { get; set; }
    }

    public class TravelMatrixSearchResponse
    {
        public IEnumerable<TravelMatrixSearch> GetData { get; set; }
        public TravelMatrixResponseResult Result { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public bool IsDataExist { get; set; }
    }

    public class TravelMatrixSummary
    {
        public int? TariffTypeId { get; set; }
        public int? TravelCurrencyId { get; set; }
        public int? CustomerId { get; set; }
        public int? SourceCurrencyId { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }

        public int? Index { get; set; }

        public int? PageSize { get; set; }

        public bool IsExport { get; set; }

        public int SearchTypeId { get; set; }
    }

    public class AreaDetails
    {
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }

        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }

        public int CountyId { get; set; }

        public string AreaCountryName { get; set; }
        public string AreaProvinceName { get; set; }
        public string AreaCityName { get; set; }
        public string AreaCountyName { get; set; }
    }
    public class AreaData
    {
        public int Id { get; set; }
        public List<CommonDataSource> DataList { get; set; }
    }
    public class AreaDataResponse
    {
        public TravelMatrixResponseResult Result { get; set; }
        public List<AreaData> AreaDataList { get; set; }
    }
    public class TravelMatixDeleteResponse
    {
        public TravelMatrixResponseResult Result { get; set; }
    }

    public class TravelMatrixExists
    {
        public string CustomerName { get; set; }
        public string TariffTypeName { get; set; }
        public string CountryName { get; set; }
        public string CountyName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string SourceCurrencyName { get; set; }
        public string TravelCurrencyName { get; set; }
    }

    public class TravelMatrixData
    {
        public string CustomerName { get; set; }
        public string TariffTypeName { get; set; }
        public string CountryName { get; set; }
        public string CountyName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string SourceCurrencyName { get; set; }
        public string TravelCurrencyName { get; set; }
        public int? CustomerId { get; set; }
        public int TariffTypeId { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        public int? SourceCurrencyId { get; set; }
        public int Id { get; set; }
    }

    public class ExistsTravelMatrix
    {
        public IEnumerable<TravelMatrix> RequestModel { get; set; }
        public IEnumerable<TravelMatrixData> AvailableTravelMatrix { get; set; }
    }

    public class ExistsTravelMatrixRequest
    {
        public IEnumerable<int?> SourceCurrencyList { get; set; }
        public IEnumerable<int> CountryList { get; set; }
        public IEnumerable<int> ProvinceList { get; set; }
        public IEnumerable<int?> CityList { get; set; }
        public IEnumerable<int?> CountyList { get; set; }
        public IEnumerable<int> TariffTypeList { get; set; }
    }

    public class TravelMatrixExportSummary
    {
        public string CustomerName { get; set; }
        public string TariffTypeName { get; set; }
        public string CountryName { get; set; }
        public int? CountryId { get; set; }
        public string ProvinceName { get; set; }
        public int? ProvinceId { get; set; }
        public string CityName { get; set; }
        public int? CityId { get; set; }
        public string CountyName { get; set; }
        public int? CountyId { get; set; }
        public string InspPortNameCounty { get; set; }
        public int? InspPortCountyId { get; set; }
        public string InspPortNameCity { get; set; }
        public int? InspPortCityId { get; set; }
        public string SourceCurrencyName { get; set; }
        public string TravelCurrencyName { get; set; }

        public double? DistanceKM { get; set; }
        public double? TravelTime { get; set; }
        public double? BusCost { get; set; }
        public double? TrainCost { get; set; }
        public double? TaxiCost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double? MarkUpCost { get; set; }
        public double? AirCost { get; set; }
        public double? MarkUpCostAir { get; set; }
        public double? FixExchangeRate { get; set; }
        public string Remarks { get; set; }
    }
    public class TravelMatrixRequest
    {
        public int BookingId { get; set; }
        public int? RuleId { get; set; }
        public int QuotationId { get; set; }
        public int BillMethodId { get; set; }
        public int BillPaidById { get; set; }
        public int? CurrencyId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public int customerId { get; set; }
    }

    public class QuotationTravelMatrixResponse
    {
        public TravelMatrixSearch TravelMatrixDetails { get; set; }
        public TravelMatrixResponseResult Result { get; set; }
    }
    public class QuotationMatrixRequest
    {
        public int? CountyId { get; set; }
        public int CurrencyId { get; set; }
        public int? MatrixTypeId { get; set; }
        public int? customerId { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
    }

    public class TravelMatrixSearchRequest
    {
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
    }

    public enum TravelMatrixSearchEnum
    {
        City = 1,
        County = 2
        
    }



}


