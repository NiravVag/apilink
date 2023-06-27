using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class ICStatusResponse
    {
        public List<ICStatus> ICStatusList { get; set; }
        public ICStatusResponseResult Result { get; set; }
    }

    public enum ICStatusResponseResult
    {
        Success=1,
        NotFound=2
    }

    public class ICStatus
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
