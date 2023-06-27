using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.EntPages
{
    public enum RightEnum
    {
        Booking = 1
    }

    public class EntPageRequest
    {
        public int RightId { get; set; }
        public int ServiceId { get; set; }
        public int UserTypeId { get; set; }
        public int? EntityId { get; set; }
    }

    public class EntPageFieldAccess
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }
        public string Name { get; set; }
    }

    public class EntPageFieldAccessResponse
    {
        public List<EntPageFieldAccess> EntPageFieldAccess { get; set; }
        public EntPageFieldAccessResult Result { get; set; }
    }

    public enum EntPageFieldAccessResult
    {
        Success = 1,
        NotFound = 2
    }
}
