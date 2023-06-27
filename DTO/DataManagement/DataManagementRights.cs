using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DataManagement
{
    public class DataManagementRight
    {
        public int IdModule { get; set; }

        public string ModuleName { get; set; }

        public bool HasRight { get; set; }

        public IEnumerable<DataManagementRight> Children { get; set; }
    }


    public class DataManagementRightResponse
    {
        public int? IdStaff { get; set; }

        public int? IdRole { get; set; }
        public bool UploadRight { get; set; }
        public bool EditRight { get; set; }
        public bool DownloadRight { get; set; }
        public bool DeleteRight { get; set; }
        public IEnumerable<DataManagementRight> Modules { get; set; }
        public string AlreadyExistModules { get; set; }
        public DataManagementRightResult Result { get; set; }
    }


    public enum DataManagementRightResult
    {
        Success = 1,
        NotFound = 2,
        Error = 3,
        RequestRequired = 4,
        IdStaffOrIdRoleRequired = 5,
        RightsRequired = 6,
        RightsAlreadyConfigured = 7
    }

    public class DataManagementRightRequest
    {
        public int? IdStaff { get; set; }

        public int? IdRole { get; set; }

        public int? IdOffice { get; set; }

        public bool UploadRight { get; set; }
        public bool EditRight { get; set; }

        public bool DownloadRight { get; set; }

        public bool DeleteRight { get; set; }

    }

    public class SaveDataManagementRightRequest
    {
        public DataManagementRightRequest RightRequest { get; set; }

        public IEnumerable<int> Modules { get; set; }

    }

    public class DMUserRightResponse
    {
        public List<DMUserRight> Data { get; set; }
        public DataManagementRightResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public class DMRoleRepoItem
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string Role { get; set; }
        public int? StaffId { get; set; }
        public string Staff { get; set; }
        public bool EditRight { get; set; }
        public bool UploadRight { get; set; }
        public bool DownloadRight { get; set; }
        public bool DeleteRight { get; set; }
    }

    public class DMUserRight
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Staff { get; set; }
        public string Rights { get; set; }
        public string Access { get; set; }
    }

    public class DMUserManagementSummaryRequest
    {
        public int? RoleId { get; set; }
        public int? StaffId { get; set; }
        public int? ModuleId { get; set; }
        public List<int> RightIds { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }

    public enum RightEnum
    {
        Edit = 1,
        Delete = 2,
        Upload = 3,
        Download = 4
    }
    public class DMUserManagementDataEditResponse
    {
        public SaveDataManagementRightRequest DMRole { get; set; }
        public DataManagementRightResult Result { get; set; }
    }

    public class DeleteDMUserManagementResponse
    {
        public DataManagementDeleteResult Result { get; set; }
    }
}
