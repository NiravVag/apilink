using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Model
{
    public class FileUploadResponse
    {
        public List<FileUploadData> FileUploadDataList { get; set; }
    }

    public class FileUploadData
    {
        public string FileName { get; set; }
        public string FileCloudUri { get; set; }
        public string FileUniqueId { get; set; }
        public FileUploadResponseResult Result { get; set; }
    }

    public enum FileUploadResponseResult
    {
        Sucess = 1,
        Failure = 2,
        FileSizeExceed = 3
    }


    public class ZipFileUploadResponse
    {
        public FileUploadData FileUploadData { get; set; }
    }

}
