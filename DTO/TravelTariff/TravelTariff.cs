using DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.TravelTariff
{
    public class TravelTariffSaveRequest
    {
        public int Id { get; set; }
        public int StartPort { get; set; }
        public int TownId { get; set; }
        public DateObject StartDate { get; set; }
        public DateObject EndDate { get; set; }
        public double? TravelTariff { get; set; }
        public int? TravelCurrency { get; set; }
        public bool? Status { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public int CountyId { get; set; }
    }



    public class TravelTariffSummaryData
    {
        public int Id { get; set; }
        public string StartPortName { get; set; }
        public string TownName { get; set; }
        public double? TravelTariff { get; set; }
        public string TravelCurrency { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime? StartDate_Date { get; set; }
        public DateTime? EndDate_Date { get; set; }
        public bool? Status { get; set; }

    }

    public class TravelTariffGetAllResponse
    {
        public IEnumerable<TravelTariffSummaryData> TravelTariffDetails { get; set; }
        public TravelTariffGetAllResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public enum TravelTariffGetAllResult
    {
        Success = 1,
        Failure = 2,
        NotFound = 3
    }


    public class TravelTariffSearchRequest
    {
        public int? StartPort { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        public int? TownId { get; set; }
        public bool? Status { get; set; }
        public DateObject StartDate { get; set; }
        public DateObject EndDate { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }

    public class TravelTariffData
    {
        public int Id { get; set; }
        public int StartPortId { get; set; }
        public int TownId { get; set; }
        public DateObject StartDate { get; set; }
        public DateObject EndDate { get; set; }
        public double? TravelTariff { get; set; }
        public int? TravelCurrency { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public int CountyId { get; set; }
    }

    public class TravelTariffGetResponse
    {
        public TravelTariffData TravelTariff { get; set; }
        public TravelTariffGetResult Result { get; set; }
    }

    public enum TravelTariffGetResult
    {
        Success = 1,
        Failure = 2,
        NotFound = 3
    }

    public enum TravelTariffResponseResult
    {
        Success = 1,
        Error = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        AllreadyExist = 5
    }

    public class TravelTariffSaveResponse
    {
        public int Id { get; set; }
        public TravelTariffResponseResult Result { get; set; }
    }

    public class TravelTariffDeleteResponse
    {
        public int Id { get; set; }

        public TravelTariffDeleteResult Result { get; set; }
    }
    public enum TravelTariffDeleteResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3,
        RequestNotCorrect = 4
    }
}
