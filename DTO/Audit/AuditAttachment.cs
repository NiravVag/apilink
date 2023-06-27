using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditAttachment
    {
        public int Id { get; set; }

        public string uniqueld { get; set; }

        public string FileName { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }

        public string FileUrl { get; set; }
    }
}
