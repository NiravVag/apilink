using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.UserGuide
{
    public class UserGuideDetailRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string VideoUrl { get; set; }
        public int? TotalPage { get; set; }
        public bool? IsCustomer { get; set; }
        public bool? IsSupplier { get; set; }
        public bool? IsFactory { get; set; }
        public bool? IsInternal { get; set; }
        public int? EntityId { get; set; }
        public IEnumerable<int?> UGRoleIds { get; set; }
    }

    public class UserGuideDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string VideoUrl { get; set; }
        public int? TotalPage { get; set; }
        public string ImageIcon { get; set; }
        public int UGRoleCount { get; set; }
    }

    public class UserGuideDetailResponse
    {
        public List<UserGuideDetail> UserGuideDetails { get; set; }
        public UserGuideDetailResult Result { get; set; }
    }

    public enum UserGuideDetailResult
    {
        Success=1,
        NotFound=2
    }
}
