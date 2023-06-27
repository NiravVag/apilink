using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class BookingEaqfResponse
    {
        public int MissionId { get; set; }
        public double Price { get; set; }
        public int Mandays { get; set; }
        public int VendorId { get; set; }
        public int FactoryId { get; set; }
        public bool IsTechincalDocumentsAddedOrRemoved { get; set; }
        public int StatusId { get; set; }
        public SaveInsepectionRequest SaveInsepectionRequest { get; set; }
    }
}
