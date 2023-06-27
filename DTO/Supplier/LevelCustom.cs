using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Supplier
{
    public class LevelCustom
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public int? CustomerId { get; set; }
        public bool IsDefault { get; set; }
    }
}
