using System.Collections.Generic;

namespace DTO.Customer
{
    public class CheckPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CheckPointResponse
    {
        public IEnumerable<CheckPoint> CheckPointList { get; set; }
        public CheckPointResult Result { get; set; }
    }
}
