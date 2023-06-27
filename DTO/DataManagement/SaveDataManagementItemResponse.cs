using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DataManagement
{
    public class SaveDataManagementItemResponse
    {
        public DataManagementItem Item { get; set;  }


        public SaveDataManagementItemResult Result { get; set;  }
    }

    public enum SaveDataManagementItemResult
    {
        Success  = 1, 
        NotFound = 2,
        Error = 3
    }

    public class DataManagmentEmailResponse
    {
        public DataManagmentEmailItem DMEmailData { get; set; }
        public DataManagmentEmailResult Result { get; set; }
    }

    public class DataManagmentEmailItem
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string FileHierarchyName { get; set; }
        public string BaseUrl { get; set; }
        public List<DMDetailEmailFileData> FileAttachments { get; set; }
    }

    public enum DataManagmentEmailResult
    {
        Success = 1,
        NotFound = 2,
        Error = 3
    }

    public class DMDetailEmailBaseData
    {
        public int Id { get; set; }
        public int? ModuleId { get; set; }
        public string Customer { get; set; }
    }

    public class DMDetailEmailFileData
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public double? FileSize { get; set; }
    }



}
