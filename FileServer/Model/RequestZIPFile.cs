using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Model
{
    public class RequestZIPFile
    {
        public int Container { get; set; }
        public int EntityId { get; set; }
        public List<ZIPFileData> ZIPFileDataList { get; set; }
    }
    public class ZIPFileData
    {
        public string UniqueId { get; set; }
        public string FileName { get; set; }
        //public string FileUrl { get; set; }
    }
}
