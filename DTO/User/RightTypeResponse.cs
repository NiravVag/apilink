using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{
    public class RightTypeResponse
    {
        public List<RightType> RightTypeList { get; set; }
        public RightTypeResult Result { get; set; }
    }
    public class RightType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Service { get; set; }
    }
    public enum RightTypeResult
    {
        Success = 1,
        NoDataFound = 2
    }
}
