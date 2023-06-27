using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ScheduleJob
{
    public class JobConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public DateTime? StartDate { get; set; }
        public int? ScheduleInterval { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public string CustomerIds { get; set; }
    }
}
