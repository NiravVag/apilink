using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
    public class ServiceType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public  string LevelName  { get; set;  }

        public double? MinorTolerancePick { get; set;  }

        public double? MajorTolerancePick { get; set;  }

        public double? CriticalPick { get; set;  }

    }

    public class ServiceTypeRequest
    {
        public int CustomerId { get; set; }

        public int ServiceId { get; set; }

        public int? BusinessLineId { get; set; }

        public int? BookingId { get; set; }

        public bool? IsReInspectedService { get; set; }
    }

    public class ServiceTypeData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool? ShowServiceDateTo { get; set; }
        public int? Sort { get; set; }
        public bool Is100Inspection { get; set; }
    }

    public class ServiceTypeResponse
    {
        public List<ServiceTypeData> ServiceTypeList { get; set; }

        public ServiceTypeResult Result { get; set; }
    }

    public enum ServiceTypeResult
    {
        Success=1,
        NotFound=2
    }

 }
