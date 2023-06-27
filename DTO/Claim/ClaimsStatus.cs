using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public class ClaimsStatus
    {
        public int Id { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public string StatusColor { get; set; }

        public int TotalCount { get; set; }
    }
}
