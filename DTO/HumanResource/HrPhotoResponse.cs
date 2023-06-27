using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.HumanResource
{
    public class HrPhotoResponse
    {
        public Guid GuidId { get; set; }
        public string UniqueId { get; set; }
        public int StaffId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int UserId { get; set; }
        public DateTime UploadDate { get; set; }
        public bool Active { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public HrPhotoResult Result { get; set; }
    }
    public enum HrPhotoResult
    {
        Success = 1,
        NotFound = 2
    }
}
