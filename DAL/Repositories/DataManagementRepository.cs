using Contracts.Repositories;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DTO.DataManagement;
using DTO.Common;
using DTO.CommonClass;

namespace DAL.Repositories
{
    public class DataManagementRepository : Repository, IDataManagementRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public DataManagementRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        /// <summary>
        /// Get the dm modules
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DmRefModule>> GetModules()
        {

            var data = await _context
                .DmRefModules
                //.Include(x => x.DmRights)
                .Where(x => x.Active.HasValue && x.Active.Value).ToListAsync();
            return data;

        }

        /// <summary>
        /// Get the dm file by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataManagementItem> GetItem(int id)
        {
            return await _context.DmDetails.Where(x => x.Id == id).Select(x => new DataManagementItem
            {
                Id = x.Id,
                CustomerName = x.Customer.CustomerName,
                //CreatedTime = x.CreatedOn,
                Description = x.Description,

                IdCustomer = x.CustomerId,
                ModuleId = x.ModuleId,

                ModuleName = x.Module.ModuleName,

            }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the modules list
        /// </summary>
        /// <returns></returns>
        public async Task<List<DMModule>> GetModuleList()
        {
            return await _context.DmRefModules.Select(x => new DMModule()
            { Id = x.Id, Name = x.ModuleName, NeedCustomer = x.NeedCustomer, ParentId = x.ParentId }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the dm detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DmDetail> GetDMDetailById(int id)
        {
            return await _context.DmDetails.
                Include(x => x.DmFiles).
                Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the dm file by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<FileData>> GetDMFileById(int id)
        {
            return await _context.DmFiles.
                Where(x => x.Id == id && x.Active.HasValue && x.Active.Value).Select(x => new FileData()
                {
                    Id = x.Id,
                    DmDetailId = x.DmdetailsId,
                    FileId = x.FileId,
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    FileType = x.FileType,
                    FileSize = x.FileSize
                }).ToListAsync();
        }

        /// <summary>
        /// Add the data managment 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddDataManagement(DmDetail entity)
        {
            _context.DmDetails.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        /// <summary>
        /// Get the DM Rights data
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<List<DMRightData>> GetDMRights(int moduleId)
        {
            return await _context.DmRights.Where(x => x.IdModule == moduleId && x.DmRole.Active.Value).
                        Select(x => new DMRightData()
                        {
                            Id = x.Id,
                            ModuleId = x.IdModule,
                            RoleId = x.DmRole.RoleId,
                            StaffId = x.DmRole.StaffId,
                            EditRight = x.DmRole.EditRight,
                            DeleteRight = x.DmRole.DeleteRight,
                            DownloadRight = x.DmRole.DownloadRight,
                            UploadRight = x.DmRole.UploadRight
                        }).ToListAsync();
        }

        /// <summary>
        /// Get the Dm Detail Query
        /// </summary>
        /// <returns></returns>
        public IQueryable<DmDetail> GetDMDetailQuery()
        {
            return _context.DmDetails;
        }

        /// <summary>
        /// Get the Dm file by dm detail ids
        /// </summary>
        /// <param name="dmDetailIds"></param>
        /// <returns></returns>
        public async Task<List<DMDetailSummaryFileData>> GetDMFileByDetailIds(List<int> dmDetailIds)
        {
            return await _context.DmFiles.
                Where(x => dmDetailIds.Contains(x.DmdetailsId) && x.Active.HasValue && x.Active.Value).Select(x => new DMDetailSummaryFileData()
                {
                    FileId = x.FileId,
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    FileType = x.FileType,
                    FileSize = x.FileSize,
                    DmDetailId = x.DmdetailsId,
                    CreatedOn = x.CreatedOn
                }).ToListAsync();
        }

        /// <summary>
        /// Get the DM File Query
        /// </summary>
        /// <returns></returns>
        public IQueryable<DmFile> GetDMFileQuery()
        {
            return _context.DmFiles.Where(x => x.Active.Value);
        }

        /// <summary>
        /// Get the dm rights
        /// </summary>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        public async Task<List<DMRightData>> GetDMRights(List<int?> moduleIds)
        {
            return await _context.DmRights.Where(x => moduleIds.Contains(x.IdModule) && x.DmRole.Active.Value).
                        Select(x => new DMRightData()
                        {
                            Id = x.Id,
                            ModuleId = x.IdModule,
                            RoleId = x.DmRole.RoleId,
                            StaffId = x.DmRole.StaffId,
                            EditRight = x.DmRole.EditRight,
                            DeleteRight = x.DmRole.DeleteRight,
                            DownloadRight = x.DmRole.DownloadRight
                        }).ToListAsync();
        }

        /// <summary>
        /// Get the dm detail by file id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DmDetail> GetDMDetailByFileId(int id)
        {
            return await _context.DmDetails.
                Include(x => x.DmFiles).
                Where(x => x.Active.HasValue && x.Active.Value && x.DmFiles.Any(y => y.Active.HasValue
                     && y.Active.Value && y.Id == id)).FirstOrDefaultAsync();
        }

        public IQueryable<DmRight> GetDMRightQuery()
        {
            return _context.DmRights;
        }

        public IQueryable<DmRole> GetDmRoles()
        {
            return _context.DmRoles.Where(x => x.Active.Value);
        }

        public async Task<DmRole> GetDmRoleById(int id)
        {
            return await _context.DmRoles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ParentDataSource>> GetRightModulesByDmRoleIds(IEnumerable<int> dmRoleIds)
        {
            return await _context.DmRights.Where(x => dmRoleIds.Contains(x.DmRoleId.GetValueOrDefault()))
                .Select(y => new ParentDataSource()
                {
                    Id = y.Id,
                    Name = y.IdModuleNavigation.ModuleName,
                    ParentId = y.DmRoleId.GetValueOrDefault()
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<DmRight>> GetDmRightsByDmRoleIds(IEnumerable<int> dmRoleIds)
        {
            return await _context.DmRights.Where(x => dmRoleIds.Contains(x.DmRoleId.GetValueOrDefault())).ToListAsync();
        }

        public async Task<DmRole> GetDmRoleData(int id)
        {
            return await _context.DmRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.Active.Value);

        }
        public async Task<List<DmRight>> GetModuleName(int dmRoleId)
        {
            return await _context.DmRights.Where(x => x.DmRoleId == dmRoleId).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Get the dm rights
        /// </summary>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        public async Task<List<DMRightData>> GetDMRightsByStaffId(int staffId)
        {
            return await _context.DmRights.Where(x => x.DmRole.StaffId == staffId && x.DmRole.Active.Value).
                        Select(x => new DMRightData()
                        {
                            Id = x.Id,
                            ModuleId = x.IdModule,
                            ModuleRanking = x.IdModuleNavigation.Ranking.GetValueOrDefault(),
                            RoleId = x.DmRole.RoleId,
                            StaffId = x.DmRole.StaffId,
                            EditRight = x.DmRole.EditRight,
                            DeleteRight = x.DmRole.DeleteRight,
                            DownloadRight = x.DmRole.DownloadRight,
                            UploadRight = x.DmRole.UploadRight
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the dm rights
        /// </summary>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        public async Task<List<DMRightData>> GetDMRightsByRoleIds(IEnumerable<int> roleIds)
        {
            return await _context.DmRights.Where(x => roleIds.Contains(x.DmRole.RoleId.GetValueOrDefault()) && x.DmRole.Active.Value).
                        Select(x => new DMRightData()
                        {
                            Id = x.Id,
                            ModuleId = x.IdModule,
                            ModuleRanking = x.IdModuleNavigation.Ranking.GetValueOrDefault(),
                            EditRight = x.DmRole.EditRight,
                            DeleteRight = x.DmRole.DeleteRight,
                            DownloadRight = x.DmRole.DownloadRight,
                            UploadRight = x.DmRole.UploadRight
                        }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ParentDataSource>> GetDmBrandsByDmFileIds(List<int> dmFileIds)
        {
            return await _context.DmBrands.Where(x => dmFileIds.Contains(x.DmfileId.GetValueOrDefault())).Select(x => new ParentDataSource()
            {
                Id = x.BrandId,
                Name = x.Brand.Name,
                ParentId = x.DmfileId.GetValueOrDefault()
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ParentDataSource>> GetDmDepartmentsByDmFileIds(List<int> dmFileIds)
        {
            return await _context.DmDepartments.Where(x => dmFileIds.Contains((int)x.DmfileId)).Select(x => new ParentDataSource()
            {
                Id = x.DepartmentId,
                Name = x.Department.Name,
                ParentId = x.DmfileId.GetValueOrDefault()
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<DmBrand>> GetDmBrandByDmFileIds(List<int> dmFileIds)
        {
            return await _context.DmBrands.Where(x => dmFileIds.Contains(x.DmfileId.GetValueOrDefault())).ToListAsync();
        }

        public async Task<List<DmDepartment>> GetDmDepartmentByDmFileIds(List<int> dmFileIds)
        {
            return await _context.DmDepartments.Where(x => dmFileIds.Contains(x.DmfileId.GetValueOrDefault())).ToListAsync();
        }
    }
}
