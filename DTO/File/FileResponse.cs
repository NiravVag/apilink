using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.File
{
    public class FileResponse
    {
        public byte[] Content { get; set; }

        public string MimeType { get; set; }

        public string Name { get; set; }

        public string FileLink { get; set; }

        public string FileUniqueId { get; set; }

        public int FileStorageType { get; set; }

        public FileResult Result { get; set; }
    }

    public enum FileStorageType
    {
        Link = 1,
        FileStream = 2
    }

    public enum FileResult
    {
        Success = 1,
        NotFound = 2
    }


    public class FileUploadData
    {
        public string FileName { get; set; }
        public string FileCloudUri { get; set; }
        public FileUploadResponseResult Result { get; set; }
    }

    public enum FileUploadResponseResult
    {
        Sucess = 1,
        Failure = 2,
        FileSizeExceed = 3
    }

    public class FileUploadResponse
    {
        public List<FileUploadData> FileUploadDataList { get; set; }
    }
}
