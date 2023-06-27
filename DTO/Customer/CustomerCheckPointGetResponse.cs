using System.Collections.Generic;

namespace DTO.Customer
{
    public class CustomerCheckPointGetResponse
    {
        public IEnumerable<CustomerCheckPoint> CustomerCheckPointList { get; set; }
        public CheckPointGetResult Result { get; set; }
    }
}
