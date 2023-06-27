using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.FileModels
{
    public class LeaveSummaryModel
    {

        public IEnumerable<LeaveItem> Data { get; set; }

        public LeaveSummaryRequest Request { get; set;  }

    }
}
