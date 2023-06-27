using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class ICSummarySearchRequest
    {
        public int SearchTypeId { get; set; }

        public string SearchTypeText { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public IEnumerable<int> StatusIdlst { get; set; }

        public DateObject FromDate { get; set; }

        public DateObject ToDate { get; set; }


        public int DateTypeid { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }

    }
}
