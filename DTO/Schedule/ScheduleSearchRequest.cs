using DTO.Common;
using System.Collections.Generic;

namespace DTO.Schedule
{
    public class ScheduleSearchRequest
    {
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public IEnumerable<int> FactoryIdlst { get; set; }
        public IEnumerable<int> StatusIdlst { get; set; }
        public int DateTypeid { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public IEnumerable<int?> Officeidlst { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public IEnumerable<int> QuotationsStatusIdlst { get; set; }
        public IEnumerable<int> QcIdlst { get; set; }
        public IEnumerable<int> ZoneIdlst { get; set; }
        public int ExportType { get; set; }
        public IEnumerable<int> ServiceTypelist { get; set; }
        public bool? IsEAQF { get; set; }
    }

    public enum ExportScheduleType
    {
        QCLevel = 1,
        ProductLevel = 2
    }
    public class MandayForecastRequest
    {

        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public IEnumerable<int> Officeidlst { get; set; }
        public List<int> ZoneIdlst { get; set; }
    }
    public class QcVisibilityBookingRequest
    {
        public List<int> BookingIdlst { get; set; }
    }

}
