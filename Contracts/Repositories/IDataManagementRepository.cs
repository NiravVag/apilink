using DTO.CommonClass;
using DTO.DataManagement;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IDataManagementRepository : IRepository
    {
        /// <summary>
        /// get all modules 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DmRefModule>> GetModules();


        /// <summary>
        /// Get item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataManagementItem> GetItem(int id);

        Task<List<DMModule>> GetModuleList();

        Task<DmDetail> GetDMDetailById(int id);

        Task<int> AddDataManagement(DmDetail entity);

        Task<List<FileData>> GetDMFileById(int id);

        Task<List<DMRightData>> GetDMRights(int moduleId);

        IQueryable<DmDetail> GetDMDetailQuery();

        Task<List<DMDetailSummaryFileData>> GetDMFileByDetailIds(List<int> dmDetailIds);

        Task<List<DMRightData>> GetDMRights(List<int?> moduleIds);

        IQueryable<DmFile> GetDMFileQuery();

        Task<DmDetail> GetDMDetailByFileId(int id);

        IQueryable<DmRight> GetDMRightQuery();

        IQueryable<DmRole> GetDmRoles();

        Task<List<ParentDataSource>> GetRightModulesByDmRoleIds(IEnumerable<int> roleIds);

        Task<DmRole> GetDmRoleById(int id);

        Task<List<DmRight>> GetDmRightsByDmRoleIds(IEnumerable<int> dmRoleIds);        
        Task<DmRole> GetDmRoleData(int id);
        Task<List<DmRight>> GetModuleName(int id);

        Task<List<DMRightData>> GetDMRightsByRoleIds(IEnumerable<int> roleIds);

        Task<List<DMRightData>> GetDMRightsByStaffId(int staffId);

        Task<List<ParentDataSource>> GetDmBrandsByDmFileIds(List<int> dmDetailIds);
        Task<List<ParentDataSource>> GetDmDepartmentsByDmFileIds(List<int> dmDetailIds);
        Task<List<DmBrand>> GetDmBrandByDmFileIds(List<int> dmDetailId);
        Task<List<DmDepartment>> GetDmDepartmentByDmFileIds(List<int> dmDetailId);
    }
}
