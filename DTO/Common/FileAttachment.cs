using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Common
{
    public class FileAttachment
    {
        public int Id { get; set; }

        public Guid uniqueld { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }

        public bool Active { get; set; }
    }

    public enum ZipStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Error = 4
    }

    public enum FileAttachmentCategory
    {
        General = 1,
        GAP=2
    }
}
