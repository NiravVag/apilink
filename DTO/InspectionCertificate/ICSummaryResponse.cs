using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{

    public class ICSummarySearchResponse
    {
        public IEnumerable<ICItem> Data { get; set; }

        public ICSummarySearchResponseResult Result { get; set; }

        public IEnumerable<IcStatus> IcStatusList { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

    }
    public class ICItem
    {
        public string BookingNo { get; set; }
        public string ICNo { get; set; }
        public int ICId { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string ServiceDate { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string BuyerName { get; set; }
    }
    public class ICItemRepo
    {
        public int BookingNo { get; set; }
        public string ICNo { get; set; }
        public int ICId { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int customerId { get;set; }
        public int SupplierId { get; set; }
        public DateTime ICCreatedDate { get; set; }
        public string BuyerName { get; set; }
    }
    public enum ICSummarySearchResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }

    public enum ICSummarySearchType
    {
        BookingNo = 1,
        PoNo = 2,
        IcNo = 3
    }

    public enum ICDataSearchType
    {
        ServiceDate = 4,
        ApplyDate = 5
    }


    public class IcStatus
    {
        public int Id { get; set; }

        public string StatusName { get; set; }

        public string StatusColor { get; set; }

        public int TotalCount { get; set; }
    }

}
