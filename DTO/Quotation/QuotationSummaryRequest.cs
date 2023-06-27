using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{

    public abstract class QuotationSummaryRequest
    {
        public int? Customerid { get; set; }

        public int? Supplierid { get; set; }

        public IEnumerable<int> Factoryidlst { get; set; }

        public IEnumerable<int> Statusidlst { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public Entities.Enums.Service ServiceId { get; set; }

        public IEnumerable<int> Officeidlst { get; set; }

        public IEnumerable<int> ServiceTypelst { get; set; }
    }

    public class QuotationSummaryGenRequest : QuotationSummaryRequest
    {
        public int Searchtypeid { get; set;  }

        public string  Searchtypetext { get; set;  }

        public string  Datetypeid { get; set;  }

        public DateObject Fromdate { get; set;  }
    
        public DateObject Todate { get; set; }
        
        public string AdvancedSearchtypeid { get; set; }

        public string AdvancedSearchtypetext { get; set; }

        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> DeptIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        public bool? IsEAQF { get; set; }

    }

    public class QuotationSummaryRepoRequest : QuotationSummaryRequest
    {
        public string No { get; set;  }

        public string Text { get; set; }

        public DateTime? Fromdate { get; set; }

        public DateTime? Todate { get; set; }

        public int Skip { get; set;  }

        public int SearchTypeId { get; set; }

        public int BillPaidBy { get; set; }

        public string AdvancedSearchtypeid { get; set; }

        public string AdvancedSearchtypetext { get; set; }

        public string DateTypeId { get; set; }

        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> DeptIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        public bool? IsEAQF { get; set; }
    }

    public class InvoiceRequest
    {
        public int QuotationId { get; set; }

        public string InvoiceNo { get; set; }

        public DateObject InvoiceDate { get; set; }

        public string InvoiceRemarks { get; set; }

        public int Service { get; set; }
    }

    public class QuotCheckpointRequest
    {
        public List<int> BookingIdList { get; set; }

        public int CustomerId { get; set; }
    }
    public enum QuotationSearchNumber
    {
        bookinkno = 1,
        quotationno = 2,
        cusbookingno = 3
    }
}
