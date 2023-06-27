using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.RepoRequest.Audit
{
   public class AuditSummaryRepoRequest
    {
        public IEnumerable<int> Customerlst { get; set; }

        public int SupplierId { get; set; }

        public IEnumerable<int> FactoryIdlst { get; set; }

        public IEnumerable<int> Statuslst { get; set; }

        public int SearchTypeId { get; set; }

        public string SearchText { get; set; }

        public int DatetypeId { get; set; }

        public DateTime? Fromdate { get; set; }

        public DateTime? Todate { get; set; }

        public IEnumerable<int?> OfficeIdlst { get; set; }

        public int take { get; set; }

        public int skip { get; set; }

        public IEnumerable<int> FactoryCountryIdList { get; set; }

        public IEnumerable<int> AuditorIdList { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
    }
}
