using DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public class ClaimSearchRequest
    {
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> CustomerList { get; set; }
        public IEnumerable<int> StatusIdlst { get; set; }
        public int DateTypeid { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public IEnumerable<int?> Officeidlst { get; set; }
        public int? CountryId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
    }
}
