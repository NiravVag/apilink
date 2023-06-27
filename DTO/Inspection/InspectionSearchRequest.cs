using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionSummarySearchRequest
    {
        public int SearchTypeId { get; set; }

        public string SearchTypeText { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public List<int> CustomerList { get; set; }

        public IEnumerable<int> FactoryIdlst { get; set; }

        public IEnumerable<int> StatusIdlst { get; set; }

        public int DateTypeid { get; set; }

        public DateObject FromDate { get; set; }

        public DateObject ToDate { get; set; }

        public IEnumerable<int?> Officeidlst { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }

        public bool IsQuotationSearch { get; set; }

        public IEnumerable<int> ServiceTypelst { get; set; }

        public string AdvancedSearchtypeid { get; set; }

        public string AdvancedSearchtypetext { get; set; }

        public IEnumerable<int> QuotationsStatusIdlst { get; set; }

        public IEnumerable<int> UserIdList { get; set; }

        public int CallingFrom { get; set; }

        public IEnumerable<int> SelectedCountryIdList { get; set; }
        public IEnumerable<int> SelectedProvinceIdList { get; set; }
        public IEnumerable<int> SelectedCityIdList { get; set; }

        public IEnumerable<int> SelectedBrandIdList { get; set; }

        public IEnumerable<int> SelectedDeptIdList { get; set; }

        public IEnumerable<int?> SelectedCollectionIdList { get; set; }

        public IEnumerable<int> SelectedBuyerIdList { get; set; }

        public IEnumerable<int?> SelectedPriceCategoryIdList { get; set; }

        public string Barcode { get; set; }

        public bool IsEcoPack { get; set; }

        public bool IsPicking { get; set; }

        public bool IsEAQF { get; set; }

        public int? quotationId { get; set; }
        public int? BookingType { get; set; }
        public IEnumerable<InspectionDFTransactions> InspectionDfTransactions { get; set; }
    }

    public class BookingStatusRequest
    {
        public int BookingId { get; set; }
        public DateObject ReportDate { get; set; }
    } 
}
