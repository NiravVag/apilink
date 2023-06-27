using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Model
{
    public class FileDownloadResponse
    {
        public Stream BlobStream { get; set; }

        public string BlobContentType { get; set; }

        public string BlobName { get; set; }
    }
}
