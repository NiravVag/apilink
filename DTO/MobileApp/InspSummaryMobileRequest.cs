using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspSummaryMobileRequest
    {
        public int? searchTypeId { get; set; }

        public string searchTypeText { get; set; }

        public int? customerId { get; set; }

        public int? supplierId { get; set; }

        public IEnumerable<int> factoryIdlst { get; set; }

        public IEnumerable<int> statusIdlst { get; set; }

        public int? dateTypeid { get; set; }

        public DateObject fromDate { get; set; }

        public DateObject toDate { get; set; }

        public IEnumerable<int> serviceTypelst { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public IEnumerable<int> quotationStatusIdlst { get; set; }

        public int? quotationId { get; set; }
        public int? BookingType { get; set; }
    }
}

