using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
   public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ServiceResponse
    {
        public IEnumerable<Service> ServiceList { get; set; }
        public ServiceResult Result { get; set; }
    }
    public enum ServiceResult
    {
        Success = 1,
        NotFound = 5
    }
}
