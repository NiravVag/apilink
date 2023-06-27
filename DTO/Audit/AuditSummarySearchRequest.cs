using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.Audit
{
    public class AuditSummarySearchRequest
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

        public int? Index { get; set; }

        public int? pageSize { get; set; }

        public IEnumerable<int> FactoryCountryIdList { get; set; }

        public IEnumerable<int> AuditorIdList { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
    }
}
