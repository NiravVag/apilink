using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.References
{
    public class BankData
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string Remarks { get; set; }
    }
}
