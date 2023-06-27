using DTO.FullBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.EmailLog
{
    public class BookingFbLogData
    {
        public int BookingId { get; set; }
        public FbBookingSyncType FbBookingSyncType { get; set; }
        public int TryCount { get; set; }
        public bool? IsMissionUpdated { get; set; }        
        public bool? IsQcSyncFb { get; set; }

    }
}