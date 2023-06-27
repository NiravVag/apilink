using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{
    public class NotDoneTaskResponse
    {
        public int TodayCount { get; set; }
        public int YesterdayCount { get; set; }
        public int OlderCount { get; set; }
        public int TotalCount
        {
            get
            {
                return TodayCount + YesterdayCount + OlderCount;
            }
        }
    }
}
