using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomerProducts
{
    public class UploadCustomerProductResponse
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public List<string> errors { get; set; }

        public FileDetails files { get; set; }
        public UploadCustomerProductResponse()
        {
            this.errors = new List<string>();
        }

    }

    public class FileDetails
    {

        public string UniqueId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int? UserId { get; set; }
        public bool? SendToClient { get; set; }
        public int EntityId { get; set; }
        public int FileTypeId { get; set; }
    }
}
