using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DataManagement
{
    public class DataManagementListResponse
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }

        public int PageCount { get; set; }


        public List<DMDetailSummaryData> data { get; set; }

        public DataManagementListResult Result { get; set; }

    }

    public enum DataManagementListResult
    {
        Success = 1,
        NotFound = 2
    }

    public class DataManagementItem
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        //public string FileId { get; set;  }

        public int? ModuleId { get; set; }

        public int? ServiceId { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? FileTypeId { get; set; }

        //public string FileType { get; set; }

        public int FileSize { get; set; }

        public string Description { get; set; }

        public int? IdCustomer { get; set; }

        public string CustomerName { get; set; }

        public string ModuleName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedTime { get; set; }

        public List<int> Offices { get; set; }

        public List<int> Positions { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDownload { get; set; }

        public bool CanUpload { get; set; }

        public bool CanDelete { get; set; }

        public List<FileData> FileAttachments { get; set; }

        public List<int> BrandIds { get; set; }
        public List<int> DepartmentIds { get; set; }

    }


    public class SaveDataManagementRequest
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int? IdCustomer { get; set; }

        public string Description { get; set; }

        public List<int> Offices { get; set; }

        public List<int> Positions { get; set; }
        public List<int> CountryIds { get; set; }

        public int EditFileId { get; set; }

        public List<FileData> FileAttachments { get; set; }

        public List<int> BrandIds { get; set; }
        public List<int> DepartmentIds { get; set; }

    }

    public class FileData
    {
        public int Id { get; set; }
        public int? DmDetailId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileId { get; set; }
        public string FileType { get; set; }
        public double? FileSize { get; set; }
    }

    public class DataManagementListRequest
    {
        public int IdModule { get; set; }

        public int? IdCustomer { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public int? Index { get; set; }

        public int? PageSize { get; set; }

        public List<int> BrandIds { get; set; }

        public List<int> DepartmentIds { get; set; }

    }

    public class DMModuleResponse
    {
        public List<DMModule> ModuleList { get; set; }
        public DMModuleResult Result { get; set; }
    }

    public class DMModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool NeedCustomer { get; set; }
        public int? ParentId { get; set; }
    }

    public enum DMModuleResult
    {
        Success = 1,
        NotFound = 2
    }

    public class DMServiceResponse
    {
        public List<DMService> ServiceList { get; set; }
        public DMServiceResult Result { get; set; }
    }

    public class DMService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool NeedCustomer { get; set; }
    }

    public enum DMServiceResult
    {
        Success = 1,
        NotFound = 2
    }

    public class DMProductCategoryResponse
    {
        public List<DMProductCategory> ProductCategoryList { get; set; }
        public DMProductCategoryResult Result { get; set; }
    }

    public class DMProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool NeedCustomer { get; set; }
    }

    public enum DMProductCategoryResult
    {
        Success = 1,
        NotFound = 2
    }

    public class DMFileTypeResponse
    {
        public List<DMFileType> FileTypeList { get; set; }
        public DMFileTypeResult Result { get; set; }
    }

    public class DMFileType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool NeedCustomer { get; set; }
    }

    public enum DMFileTypeResult
    {
        Success = 1,
        NotFound = 2
    }

    public class DMRightData
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int? RoleId { get; set; }
        public int? StaffId { get; set; }
        public bool EditRight { get; set; }
        public bool DeleteRight { get; set; }
        public bool DownloadRight { get; set; }
        public bool UploadRight { get; set; }
        public int? DmRoleId { get; set; }
        public int ModuleRanking { get; set; }
    }

    public class DMDetailSummary
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Module { get; set; }
        public int? ModuleId { get; set; }
        public int? ParentId { get; set; }
    }

    public class DMDetailData
    {
        public int DmDetailId { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public int? ModuleId { get; set; }
        public string Module { get; set; }

        public int? ParentId { get; set; }
    }

    public class DMDetailFileData
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public double? FileSize { get; set; }
        public string FileId { get; set; }
        public int DmDetailId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class DMDetailSummaryFileData
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public double? FileSize { get; set; }
        public string FileId { get; set; }
        public int? DmDetailId { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public int? ModuleId { get; set; }
        public string Module { get; set; }
        public int? ParentId { get; set; }

        public DateTime? CreatedOn { get; set; }
    }

    public class DMDetailSummaryData
    {
        public int Id { get; set; }
        public int? DmDetailId { get; set; }
        public string Customer { get; set; }
        public string Module { get; set; }
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string Description { get; set; }
        public string DocumentSize { get; set; }

        public string DocumentUrl { get; set; }
        public string CreatedOn { get; set; }
        public bool EditRight { get; set; }
        public bool DeleteRight { get; set; }
        public bool DownloadRight { get; set; }
        public string Brands { get; set; }
        public string Departments { get; set; }
        public DMDetailSummaryData()
        {
            EditRight = false;
            DeleteRight = false;
            DownloadRight = false;
        }

    }

    public class ModuleParentChildData
    {
        public string ModuleName { get; set; }
        public int ChildId { get; set; }
        public int? ParentId { get; set; }

    }

    public class ModuleHierarchyData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ModuleHierarchy HierarchyData { get; set; }

    }

    public class ModuleNameHierarchyData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int OrderId { get; set; }

    }

    public class ModuleHierarchy
    {
        public int? ModuleId { get; set; }
        public List<ModuleNameHierarchyData> ModuleHierarchyList { get; set; }
    }





}

