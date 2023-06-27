using BI.Maps;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.DataManagement;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class DataManagementManager : IDataManagementManager
    {

        private readonly IDataManagementRepository _repo = null;

        private readonly IAPIUserContext _ApplicationContext = null;

        private readonly IFileManager _fileManager = null;

        private readonly DataManagementMap _dataManagementMap = null;
        private readonly ITenantProvider _filterService = null;

        public DataManagementManager(IDataManagementRepository repo, IAPIUserContext applicationContext, IFileManager fileManager, ITenantProvider filterService)
        {
            _repo = repo;
            _ApplicationContext = applicationContext;
            _fileManager = fileManager;
            _dataManagementMap = new DataManagementMap();
            _filterService = filterService;
        }

        /// <summary>
        /// Get the module lsit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ModuleListResponse> GetModules(int? id = null)
        {
            var data = await _repo.GetModules();

            if (data == null || !data.Any())
                return new ModuleListResponse
                {
                    Result = ModuleListResult.NotFound
                };

            var modules = GetModulesRecusively(data, id);

            if (modules == null || !modules.Any())
                return new ModuleListResponse
                {
                    Result = ModuleListResult.NotFound
                };

            return new ModuleListResponse
            {
                List = modules,
                Result = ModuleListResult.Success
            };

        }

        /// <summary>
        /// Search the Data management details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataManagementListResponse> SearchDMDetail(DataManagementListRequest request)
        {
            if (request == null)
                return new DataManagementListResponse() { Result = DataManagementListResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var response = new DataManagementListResponse { Index = request.Index.Value, PageSize = request.PageSize.Value };

            var dmFileQuery = GetDMFileQuery(request);

            var filteredDmRights = await GetFilteredDataManagementRights();

            if (!filteredDmRights.Any())
            {
                return new DataManagementListResponse() { Result = DataManagementListResult.NotFound };
            }

            var filteredRightsModules = filteredDmRights.Select(x => x.ModuleId).ToList();
            var dataManagementQuery = dmFileQuery.Where(x => x.Active.HasValue && x.Active.Value && filteredRightsModules.Contains(x.Dmdetails.ModuleId.Value)).Select(x => new DMDetailSummaryFileData()
            {
                Id = x.Id,
                FileId = x.FileId,
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                FileType = x.FileType,
                FileSize = x.FileSize,
                DmDetailId = x.DmdetailsId,
                Customer = x.Dmdetails.Customer.CustomerName,
                ModuleId = x.Dmdetails.ModuleId,
                Module = x.Dmdetails.Module.ModuleName,
                ParentId = x.Dmdetails.Module.ParentId,
                Description = x.Dmdetails.Description,
                CreatedOn = x.CreatedOn

            });

            response.TotalCount = await dataManagementQuery.Select(x => x.Id).CountAsync();

            var dataManagementList = await dataManagementQuery.Skip(skip).Take(take).AsNoTracking().ToListAsync();

            //get the module list
            var moduleList = await _repo.GetModules();
            //take the module id list
            var moduleIdList = dataManagementList.Select(x => x.ModuleId).Distinct().ToList();

            //prepare the hierarchy list
            var hierachyDataList = moduleList
                            .Where(x => moduleIdList.Contains(x.Id))
                            .Select(x => new ModuleHierarchy()
                            {
                                ModuleId = x.Id,

                                ModuleHierarchyList = GetModuleHierarchy(moduleList.ToList(), x.Id)
                            })
                            .ToList();



            var dmFileIds = dataManagementList.Select(x => x.Id).Distinct().ToList();
            var dmBrands = await _repo.GetDmBrandsByDmFileIds(dmFileIds);
            var dmDepartments = await _repo.GetDmDepartmentsByDmFileIds(dmFileIds);

            //map the data management 
            var data = _dataManagementMap.GetDataManagementResultData(dataManagementList, hierachyDataList, filteredDmRights, dmBrands, dmDepartments);

            if (dataManagementList == null || !dataManagementList.Any())
                return new DataManagementListResponse() { Result = DataManagementListResult.NotFound };

            return new DataManagementListResponse()
            {
                Result = DataManagementListResult.Success,
                TotalCount = response.TotalCount,
                Index = request.Index.Value,
                PageSize = request.PageSize.Value,
                data = data,
                PageCount = (response.TotalCount / request.PageSize.Value) + (response.TotalCount % request.PageSize.Value > 0 ? 1 : 0),

            };
        }

        private IQueryable<DmFile> GetDMFileQuery(DataManagementListRequest request)
        {
            //get the data file query
            var dmFileQuery = _repo.GetDMFileQuery();

            //apply the module id
            if (request.IdModule > 0)
                dmFileQuery = dmFileQuery.Where(x => x.Dmdetails.ModuleId == request.IdModule);

            //apply the customer id
            if (request.IdCustomer > 0)
                dmFileQuery = dmFileQuery.Where(x => x.Dmdetails.CustomerId == request.IdCustomer);

            //apply the description
            if (!string.IsNullOrEmpty(request.Description))
                dmFileQuery = dmFileQuery.Where(x => x.Dmdetails.Description == request.Description);

            if (!string.IsNullOrEmpty(request.FileName))
                dmFileQuery = dmFileQuery.Where(x => EF.Functions.Like(x.FileName, $"%{request.FileName.Trim()}%"));

            if (request.BrandIds != null && request.BrandIds.Any())
            {
                dmFileQuery = dmFileQuery.Where(x => x.DmBrands.Any(y => request.BrandIds.Contains(y.BrandId)));
            }

            if (request.DepartmentIds != null && request.DepartmentIds.Any())
            {
                dmFileQuery = dmFileQuery.Where(x => x.DmDepartments.Any(y => request.DepartmentIds.Contains(y.DepartmentId)));
            }

            dmFileQuery = dmFileQuery.Where(x => x.Dmdetails.Active.Value);

            //sort the data by createddate descending
            dmFileQuery = dmFileQuery.OrderByDescending(x => x.CreatedOn);

            //dmFileQuery = dmFileQuery.Where(x => x.Dmdetails.EntityId == _filterService.GetCompanyId());

            return dmFileQuery;
        }

        /// <summary>
        /// Get the module hierarchy data for parent level and recursively call the child level
        /// </summary>
        /// <param name="moduleList"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        private List<ModuleNameHierarchyData> GetModuleHierarchy(List<DmRefModule> moduleList, int? moduleId)
        {
            if (moduleId != null)
            {
                var module = moduleList.FirstOrDefault(x => x.Id == moduleId);

                int orderId = 1;
                List<ModuleNameHierarchyData> moduleHierarchyDataList = new List<ModuleNameHierarchyData>();

                var data = new ModuleNameHierarchyData();

                data.Id = module.Id;
                data.Name = module.ModuleName;
                data.OrderId = orderId;
                moduleHierarchyDataList.Add(data);

                orderId = orderId + 1;
                GetModuleHierachyData(module.ParentId, orderId, moduleHierarchyDataList, moduleList);

                return moduleHierarchyDataList;

            }

            return null;

        }

        /// <summary>
        /// Get the module hierarchy data for child level and recursively call to make the list
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="orderId"></param>
        /// <param name="moduleHierarchyDataList"></param>
        /// <param name="moduleList"></param>
        private void GetModuleHierachyData(int? parentId, int orderId, List<ModuleNameHierarchyData> moduleHierarchyDataList, List<DmRefModule> moduleList)
        {
            if (parentId != null)
            {
                var moduleHierarchyData = moduleList.FirstOrDefault(x => x.Id == parentId);

                ModuleNameHierarchyData data = new ModuleNameHierarchyData();
                data.Id = moduleHierarchyData.Id;
                data.Name = moduleHierarchyData.ModuleName;
                data.ParentId = moduleHierarchyData.ParentId;
                data.OrderId = orderId;

                moduleHierarchyDataList.Add(data);

                orderId = orderId + 1;
                GetModuleHierachyData(moduleHierarchyData.ParentId, orderId, moduleHierarchyDataList, moduleList);
            }



        }

        /// <summary>
        /// Get the data management data (file level) by dm id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataManagementItemResponse> GetItem(int id)
        {
            //get the dm file data
            var fileDetails = await _repo.GetDMFileById(id);

            if (fileDetails != null && fileDetails.Any())
            {
                //get the dm detail id
                var dmDetailId = fileDetails.FirstOrDefault()?.DmDetailId.GetValueOrDefault(0);
                //get the dm detail data
                var item = await _repo.GetItem(dmDetailId.GetValueOrDefault());

                if (item != null)
                {

                    var dmFileIds = new List<int>() { id };

                    var brands = await _repo.GetDmBrandsByDmFileIds(dmFileIds);

                    var departments = await _repo.GetDmDepartmentsByDmFileIds(dmFileIds);

                    //get the dm rights data
                    var dmRights = await _repo.GetDMRights(item.ModuleId.GetValueOrDefault());

                    //get the edit,delete and download rights
                    item.CanEdit = dmRights.Any(y => y.EditRight
                                                     && ((y.RoleId != null && _ApplicationContext.RoleList.Contains(y.RoleId.Value)) || y.StaffId == _ApplicationContext.StaffId));
                    item.CanDelete = dmRights.Any(y => y.DeleteRight
                                                && ((y.RoleId != null && _ApplicationContext.RoleList.Contains(y.RoleId.Value)) || y.StaffId == _ApplicationContext.StaffId));
                    item.CanDownload = dmRights.Any(y => y.DeleteRight
                                    && ((y.RoleId != null && _ApplicationContext.RoleList.Contains(y.RoleId.Value)) || y.StaffId == _ApplicationContext.StaffId));

                    item.CanUpload = dmRights.Any(y => y.UploadRight && ((y.RoleId != null && _ApplicationContext.RoleList.Contains(y.RoleId.Value)) || y.StaffId == _ApplicationContext.StaffId));

                    item.FileAttachments = fileDetails;

                    item.BrandIds = brands.Select(x => x.Id).ToList();

                    item.DepartmentIds = departments.Select(x => x.Id).ToList();

                    return new DataManagementItemResponse
                    {
                        Result = DataManagementItemResult.Success,
                        Item = item
                    };
                }

            }

            return new DataManagementItemResponse
            {
                Result = DataManagementItemResult.NotFound
            };


        }

        /// <summary>
        /// save / update the data management 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataManagmentEmailResponse> Save(SaveDataManagementRequest request)
        {
            if (request.Id <= 0)
                return await AddDataManagement(request);

            return await UpdateDataManagement(request);
        }

        /// <summary>
        /// Add the data managment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<DataManagmentEmailResponse> AddDataManagement(SaveDataManagementRequest request)
        {
            //add the dm detail data
            var entity = AddDMDetail(request);

            //add the dm files
            AddDMFiles(request, entity);

            request.Id = await _repo.AddDataManagement(entity);
            //get the email response data
            return await GetEmailResponseData(request.Id);

        }

        /// <summary>
        /// add the dm detail data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private DmDetail AddDMDetail(SaveDataManagementRequest request)
        {
            DmDetail entity = new DmDetail
            {
                Active = true,
                CustomerId = request.IdCustomer,
                Description = request.Description,
                ModuleId = request.ModuleId,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now,
                EntityId = _filterService.GetCompanyId()
            };

            return entity;
        }

        /// <summary>
        /// add the dm file data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        private void AddDMFiles(SaveDataManagementRequest request, DmDetail entity)
        {
            var fileAttachments = request.FileAttachments.Where(x => x.Id == 0);
            //add the dm file attachments
            if (fileAttachments != null && fileAttachments.Any())
            {
                foreach (var item in fileAttachments)
                {
                    var fileAttachment = new DmFile
                    {
                        FileId = item.FileId,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        FileUrl = item.FileUrl,
                        FileType = _fileManager.GetMimeType(Path.GetExtension(item.FileName)),
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        EntityId = _filterService.GetCompanyId()
                    };
                    AddOrDeleteBrandsAndDepartments(fileAttachment, request.BrandIds, request.DepartmentIds, null, null);
                    entity.DmFiles.Add(fileAttachment);
                    _repo.AddEntity(fileAttachment);
                }
            }
        }

        private void AddOrDeleteBrandsAndDepartments(DmFile entity, IEnumerable<int> brandIds, IEnumerable<int> departmentIds, IEnumerable<DmBrand> dmBrands, IEnumerable<DmDepartment> dmDepartments)
        {
            if (brandIds != null)
            {
                AddOrDeleteBrands(entity, dmBrands, brandIds);
            }

            if (departmentIds != null)
            {
                AddOrDeleteDepartments(entity, dmDepartments, departmentIds);
            }
        }

        private void AddOrDeleteBrands(DmFile entity, IEnumerable<DmBrand> dmBrands, IEnumerable<int> brandIds)
        {
            IEnumerable<int> newBrandIds = null;
            if (dmBrands != null && dmBrands.Any())
            {
                var deleteDmBrands = dmBrands.Where(x => !brandIds.Contains(x.BrandId));
                if (deleteDmBrands.Any())
                {
                    _repo.RemoveEntities(deleteDmBrands);
                }
                var dbBrandIds = dmBrands.Select(x => x.BrandId);
                newBrandIds = brandIds.Where(x => !dbBrandIds.Contains(x)).ToList();
            }
            else
            {
                newBrandIds = brandIds;
            }

            foreach (var brandId in newBrandIds)
            {
                var dmBrand = new DmBrand()
                {
                    BrandId = brandId
                };
                entity.DmBrands.Add(dmBrand);
                _repo.AddEntity(dmBrand);
            }
        }

        private void AddOrDeleteDepartments(DmFile entity, IEnumerable<DmDepartment> dmDepartments, IEnumerable<int> departmentIds)
        {
            IEnumerable<int> newDepartmentIds = null;
            if (dmDepartments != null && dmDepartments.Any())
            {
                var deleteDmDepartments = dmDepartments.Where(x => !departmentIds.Contains(x.DepartmentId));
                if (deleteDmDepartments.Any())
                {
                    _repo.RemoveEntities(deleteDmDepartments);
                }

                var dbDepartmentIds = dmDepartments.Select(x => x.DepartmentId);
                newDepartmentIds = departmentIds.Where(x => !dbDepartmentIds.Contains(x)).ToList();
            }
            else
            {
                newDepartmentIds = departmentIds;
            }

            if (newDepartmentIds != null && newDepartmentIds.Any())
            {
                foreach (var departmentId in newDepartmentIds)
                {
                    var dmDepartment = new DmDepartment()
                    {
                        DepartmentId = departmentId
                    };
                    entity.DmDepartments.Add(dmDepartment);
                    _repo.AddEntity(dmDepartment);
                }
            }

        }

        /// <summary>
        /// Update the data management data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<DataManagmentEmailResponse> UpdateDataManagement(SaveDataManagementRequest request)
        {
            var entity = await _repo.GetDMDetailById(request.Id);

            if (entity == null)
                return new DataManagmentEmailResponse
                {
                    Result = DataManagmentEmailResult.NotFound
                };

            //map the updated dm detail data
            UpdateDMDetail(entity, request);

            var dmBrands = await _repo.GetDmBrandByDmFileIds(entity.DmFiles.Select(x => x.Id).ToList());

            var dmDepartments = await _repo.GetDmDepartmentByDmFileIds(entity.DmFiles.Select(x => x.Id).ToList());


            //remove the file if not available in the request
            RemoveDMFile(entity, request, dmBrands, dmDepartments);

            //add the dm files
            AddDMFiles(request, entity);

            var savedFileAttachmentIds = request.FileAttachments.Where(x => x.Id > 0).Select(x => x.Id).ToList();
            var dbDmFiles = entity.DmFiles.Where(x => savedFileAttachmentIds.Contains(x.Id)).ToList();
            foreach (var dmFile in dbDmFiles)
            {
                AddOrDeleteBrandsAndDepartments(dmFile, request.BrandIds, request.DepartmentIds,
                    dmBrands.Where(x => x.DmfileId == dmFile.Id), dmDepartments.Where(x => x.DmfileId == dmFile.Id));
            }


            _repo.EditEntity(entity);
            await _repo.Save();
            int id = entity.Id;

            //get the email response data
            return await GetEmailResponseData(id);

        }

        /// <summary>
        /// Map the updated dm detail data
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void UpdateDMDetail(DmDetail entity, SaveDataManagementRequest request)
        {
            entity.CustomerId = request.IdCustomer;
            entity.ModuleId = request.ModuleId;
            entity.Description = request.Description;
            entity.UpdatedBy = _ApplicationContext.UserId;
            entity.UpdatedOn = DateTime.Now;
        }

        /// <summary>
        /// remove the dm file
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void RemoveDMFile(DmDetail entity, SaveDataManagementRequest request, List<DmBrand> dmBrands, List<DmDepartment> dmDepartments)
        {
            //remove the file from entity if it is not available in the request
            if (!request.FileAttachments.Where(x => x.Id > 0 && x.Id == request.EditFileId).Any())
            {
                var lstAttachmentToRemove = new List<DmFile>();

                var fileAttachmentsToRemove = entity.DmFiles.Where(x => x.Id == request.EditFileId);
                foreach (var item in fileAttachmentsToRemove)
                {
                    item.Active = false;
                    lstAttachmentToRemove.Add(item);

                    AddOrDeleteBrandsAndDepartments(item, brandIds: new List<int>(), departmentIds: new List<int>(),
                        dmBrands: dmBrands.Where(x => x.DmfileId == item.Id), dmDepartments: dmDepartments.Where(x => x.DmfileId == item.Id));
                }
                _repo.EditEntities(lstAttachmentToRemove);
            }
        }

        /// <summary>
        /// Get the email response data
        /// </summary>
        /// <param name="dmDetailId"></param>
        /// <returns></returns>
        private async Task<DataManagmentEmailResponse> GetEmailResponseData(int dmDetailId)
        {
            DataManagmentEmailItem emailData = new DataManagmentEmailItem();

            //get the dm detail query
            var dmDetailQuery = _repo.GetDMDetailQuery();

            //get the email base data
            var dmEmailBaseData = await dmDetailQuery.Where(x => x.Id == dmDetailId).
                                            Select(x => new DMDetailEmailBaseData()
                                            {
                                                Id = x.Id,
                                                ModuleId = x.ModuleId,
                                                Customer = x.Customer.CustomerName
                                            }).FirstOrDefaultAsync();

            //get the module hierarchy
            if (dmEmailBaseData != null)
            {
                emailData.Customer = dmEmailBaseData.Customer;

                var moduleList = await _repo.GetModules();
                if (moduleList != null && moduleList.Any())
                {
                    var moduleHierarchyList = GetModuleHierarchy(moduleList.ToList(), dmEmailBaseData.ModuleId);

                    if (moduleHierarchyList != null && moduleHierarchyList.Any())
                    {
                        moduleHierarchyList = moduleHierarchyList.OrderByDescending(x => x.OrderId).ToList();
                        emailData.FileHierarchyName = string.Join("/", moduleHierarchyList.Select(x => x.Name));
                    }
                }
                //get the dm files
                var dmFilesQuery = _repo.GetDMFileQuery();

                var dmFiles = await dmFilesQuery.Where(x => x.DmdetailsId == dmEmailBaseData.Id).
                                Select(x => new DMDetailEmailFileData()
                                {
                                    FileName = x.FileName,
                                    FileUrl = x.FileUrl,
                                    FileSize = x.FileSize
                                }).ToListAsync();
                //map the file attachments
                if (dmFiles != null && dmFiles.Any())
                {
                    emailData.FileAttachments = dmFiles;
                }

                return new DataManagmentEmailResponse() { DMEmailData = emailData, Result = DataManagmentEmailResult.Success };

            }
            else
            {
                return new DataManagmentEmailResponse() { Result = DataManagmentEmailResult.NotFound };
            }

        }

        /// <summary>
        /// Get the rights data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataManagementRightResponse> GetRights(DataManagementRightRequest request)
        {
            var response = new DataManagementRightResponse();

            var dmRefModules = await _repo.GetModules();

            if (request.UploadRight)
            {
                var filteredDmRights = await GetFilteredDataManagementRights();
                if (!filteredDmRights.Any(x => x.UploadRight))
                {
                    response.Result = DataManagementRightResult.NotFound;
                    return response;
                }

                List<DmRefModule> modules = new List<DmRefModule>();
                var uploadRightModules = filteredDmRights.Where(x => x.UploadRight).OrderBy(y => y.ModuleRanking).ToList();
                foreach (var item in uploadRightModules)
                {
                    CreateRecusivelyModuleList(item.ModuleId, dmRefModules, modules);
                }
                dmRefModules = modules;
            }

            response.Modules = GetModulesRightsRecusively(dmRefModules, null);
            response.Result = DataManagementRightResult.Success;


            return response;
        }

        /// <summary>
        /// save the dm rights data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataManagementRightResponse> SaveRights(SaveDataManagementRightRequest request)
        {
            if (request.RightRequest.EditRight == false && request.RightRequest.DeleteRight == false &&
                request.RightRequest.DownloadRight == false && request.RightRequest.UploadRight == false)
            {
                return new DataManagementRightResponse
                {
                    Result = DataManagementRightResult.RightsRequired
                };
            }

            var dmUserRightResponse = await ValidateDmUserRights(request, null);
            if (dmUserRightResponse != null)
                return dmUserRightResponse;

            // save the selected role/staff and rights            
            var dmRole = _dataManagementMap.MapDmRoleEntity(request, _filterService.GetCompanyId(), _ApplicationContext.UserId);

            foreach (var idModule in request.Modules)
            {
                var item = new DmRight
                {
                    IdModule = idModule,
                    EntityId = _filterService.GetCompanyId()
                };
                dmRole.DmRights.Add(item);
                _repo.AddEntity(item);
            }

            _repo.AddEntity(dmRole);
            await _repo.Save();

            return await GetRights(request.RightRequest);

        }

        /// <summary>
        /// Get the modules recursively
        /// </summary>
        /// <param name="dmRefModules"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<DmModule> GetModulesRecusively(IEnumerable<DmRefModule> dmRefModules, int? id)
        {

            if (dmRefModules == null)
                return new HashSet<DmModule>();

            var modules = dmRefModules.Where(x => x.ParentId == id);

            if (modules == null || !modules.Any())
                return new HashSet<DmModule>();

            return modules.Select(x => new DmModule
            {
                Id = x.Id,
                ParentId = id,
                ModuleName = x.ModuleName,
                NeedCustomer = x.NeedCustomer,
                Children = GetModulesRecusively(dmRefModules, x.Id),
                Ranking = x.Ranking ?? 0
            });

        }

        /// <summary>
        /// based on modules create parent and child list
        /// </summary>
        /// <param name="dmRefModules"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<DataManagementRight> GetModulesRightsRecusively(IEnumerable<DmRefModule> dmRefModules, int? id)
        {

            if (dmRefModules == null)
                return new HashSet<DataManagementRight>();

            var modules = dmRefModules.Where(x => x.ParentId == id);

            if (modules == null || !modules.Any())
                return new HashSet<DataManagementRight>();


            return modules.Select(x => new DataManagementRight
            {
                IdModule = x.Id,
                ModuleName = x.ModuleName,
                //HasRight = x.DmRights.Any(y => GetRight(request, y)),
                Children = GetModulesRightsRecusively(dmRefModules, x.Id)
            });

        }

        private bool GetRight(DataManagementRightRequest request, DmRight right)
        {
            if (request.IdStaff == null && (request.IdRole == null || request.IdRole == 0))
                return false;

            if (request.IdStaff != null && right.IdStaff != request.IdStaff.Value)
                return false;

            if (request.IdRole != null && right.IdRole != request.IdRole.Value)
                return false;

            //if (request.EditRight && !right.EditRight)
            //    return false;

            //if (request.DownloadRight && !right.DownloadRight)
            //    return false;

            //if (request.DeleteRight && !right.DeleteRight)
            //    return false;

            return true;
        }

        /// <summary>
        /// Get the module list
        /// </summary>
        /// <returns></returns>
        public async Task<DMModuleResponse> GetModuleList()
        {
            var modules = await _repo.GetModuleList();

            if (modules != null && modules.Any())
                return new DMModuleResponse() { Result = DMModuleResult.Success, ModuleList = modules };

            return new DMModuleResponse() { Result = DMModuleResult.NotFound };
        }

        /// <summary>
        /// Delete the data managment data
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<DataManagementDeleteResponse> DeleteDataManagement(int fileId)
        {
            //get the dm detail data by file id
            var dmDetail = await _repo.GetDMDetailByFileId(fileId);

            //deactivate the dm file id
            if (dmDetail != null && dmDetail.DmFiles != null && dmDetail.DmFiles.Any())
            {
                var dmFile = dmDetail.DmFiles.FirstOrDefault(x => x.Id == fileId);
                dmFile.Active = false;
                dmFile.DeletedBy = _ApplicationContext.UserId;
                dmFile.DeletedOn = DateTime.Now;

                _repo.EditEntity(dmFile);

                //if no files mapped to dm detail data then deactivate the dm detail data
                if (!dmDetail.DmFiles.Any(x => x.Active.HasValue && x.Active.Value))
                {
                    dmDetail.Active = false;
                    //dmDetail. = false;
                    dmDetail.DeletedBy = _ApplicationContext.UserId;
                    dmDetail.DeletedOn = DateTime.Now;
                    _repo.EditEntity(dmDetail);
                }

                await _repo.Save();

                return new DataManagementDeleteResponse() { Id = fileId, Result = DataManagementDeleteResult.Success };
            }
            return new DataManagementDeleteResponse() { Result = DataManagementDeleteResult.NotFound };
        }

        public async Task<DMUserRightResponse> GetDMUserManagementSummary(DMUserManagementSummaryRequest request)
        {
            var response = new DMUserRightResponse();

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value == 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            if (request.RoleId == null && request.StaffId == null && request.ModuleId == null)
            {
                response.Result = DataManagementRightResult.IdStaffOrIdRoleRequired;
                return response;
            }
            //get the edit,download,upload and delete right data
            var dmRolesQuery = _repo.GetDmRoles();

            if (request.RoleId != null)
                dmRolesQuery = dmRolesQuery.Where(x => x.RoleId == request.RoleId);
            else if (request.StaffId != null)
                dmRolesQuery = dmRolesQuery.Where(x => x.StaffId == request.StaffId);
            else if (request.ModuleId != null)
                dmRolesQuery = dmRolesQuery.Where(x => x.DmRights.Any(x => x.IdModule == request.ModuleId));

            if (request.RightIds != null && request.RightIds.Any())
            {
                dmRolesQuery = dmRolesQuery.Where(x => (request.RightIds.Contains((int)RightEnum.Edit) ? x.EditRight == true : false)
                || (request.RightIds.Contains((int)RightEnum.Delete) ? x.DeleteRight : false)
                || (request.RightIds.Contains((int)RightEnum.Download) ? x.DownloadRight : false)
                || (request.RightIds.Contains((int)RightEnum.Upload) ? x.UploadRight : false));
            }

            var totalCount = await dmRolesQuery.AsNoTracking().CountAsync();
            if (totalCount == 0)
            {
                return new DMUserRightResponse()
                {
                    Result = DataManagementRightResult.NotFound
                };
            }

            var data = dmRolesQuery.Select(x => new DMRoleRepoItem()
            {
                Id = x.Id,
                RoleId = x.RoleId,
                Role = x.Role.RoleName,
                StaffId = x.StaffId,
                Staff = x.Staff.PersonName,
                EditRight = x.EditRight,
                DeleteRight = x.DeleteRight,
                UploadRight = x.UploadRight,
                DownloadRight = x.DownloadRight
            });
            var dmRights = await data.Skip(skip).Take(take).AsNoTracking().ToListAsync();

            var dmRightAccess = await _repo.GetRightModulesByDmRoleIds(dmRights.Select(x => x.Id));

            response.Data = dmRights.Select(x => _dataManagementMap.MapDmUserRightsSummary(x, dmRightAccess)).ToList();
            response.Result = DataManagementRightResult.Success;
            response.PageCount = (totalCount / request.PageSize.Value) + (totalCount % request.PageSize.Value > 0 ? 1 : 0);
            response.PageSize = request.PageSize.GetValueOrDefault();
            response.TotalCount = totalCount;
            response.Index = request.Index.GetValueOrDefault();
            return response;
        }

        /// <summary>
        /// upddate the dm rights data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataManagementRightResponse> UpdateRights(int id, SaveDataManagementRightRequest request)
        {
            if (request.RightRequest.EditRight == false && request.RightRequest.DeleteRight == false && request.RightRequest.DownloadRight == false && request.RightRequest.UploadRight == false)
            {
                return new DataManagementRightResponse
                {
                    Result = DataManagementRightResult.RightsRequired
                };
            }

            var dmRole = await _repo.GetDmRoleById(id);
            if (dmRole == null)
                return new DataManagementRightResponse()
                {
                    Result = DataManagementRightResult.NotFound
                };

            var dmUserRightResponse = await ValidateDmUserRights(request, id);
            if (dmUserRightResponse != null)
                return dmUserRightResponse;

            _dataManagementMap.MapDmRoleEntity(dmRole, request, _ApplicationContext.UserId);

            var dmRights = await _repo.GetDmRightsByDmRoleIds(new List<int>() { id });

            var deleteDmRights = dmRights.Where(x => !request.Modules.Contains(x.IdModule)).ToList();
            if (deleteDmRights != null && deleteDmRights.Any())
            {
                _repo.RemoveEntities(deleteDmRights);
            }

            var dbModuleIds = dmRights.Select(x => x.IdModule).ToList();

            var newModuleIds = request.Modules.Where(x => !dbModuleIds.Contains(x)).ToList();
            if (newModuleIds != null && newModuleIds.Any())
            {
                foreach (var idModule in newModuleIds)
                {
                    var item = new DmRight
                    {
                        IdModule = idModule,
                        EntityId = _filterService.GetCompanyId()
                    };
                    dmRole.DmRights.Add(item);
                    _repo.AddEntity(item);
                }
            }

            _repo.EditEntity(dmRole);
            await _repo.Save();

            return await GetRights(request.RightRequest);

        }

        public async Task<DMUserManagementDataEditResponse> EditDMUserManagement(int id)
        {
            var response = new DMUserManagementDataEditResponse();

            var dmRole = await _repo.GetDmRoleData(id);
            if (dmRole == null)
            {
                return new DMUserManagementDataEditResponse
                {
                    Result = DataManagementRightResult.NotFound
                };
            }

            var modules = await _repo.GetModuleName(id);

            response.DMRole = new SaveDataManagementRightRequest()
            {
                RightRequest = new DataManagementRightRequest()
                {
                    DeleteRight = dmRole.DeleteRight,
                    DownloadRight = dmRole.DownloadRight,
                    EditRight = dmRole.EditRight,
                    UploadRight = dmRole.UploadRight,
                    IdStaff = dmRole.StaffId,
                    IdRole = dmRole.RoleId
                },
                Modules = modules.Select(x => x.IdModule).ToList()
            };

            response.Result = DataManagementRightResult.Success;
            return response;
        }

        public async Task<DeleteDMUserManagementResponse> DeleteDMUserManagement(int id)
        {
            var data = await _repo.GetDmRoleById(id);
            if (data == null)
                return new DeleteDMUserManagementResponse() { Result = DataManagementDeleteResult.NotFound };

            data.DeletedBy = _ApplicationContext.UserId;
            data.DeletedOn = DateTime.Now;
            data.Active = false;

            _repo.EditEntity(data);
            await _repo.Save();
            return new DeleteDMUserManagementResponse() { Result = DataManagementDeleteResult.Success };
        }

        private async Task<DataManagementRightResponse> ValidateDmUserRights(SaveDataManagementRightRequest request, int? id)
        {
            var existingDmRights = _repo.GetDMRightQuery().Where(x => x.DmRoleId.HasValue && x.DmRole.Active.Value);
            if (id.HasValue && id > 0)
                existingDmRights = existingDmRights.Where(x => x.DmRoleId != id);

            if (request.RightRequest.IdRole != null)
                existingDmRights = existingDmRights.Where(x => x.DmRole.RoleId == request.RightRequest.IdRole);

            if (request.RightRequest.IdStaff != null)
                existingDmRights = existingDmRights.Where(x => x.DmRole.StaffId == request.RightRequest.IdStaff);

            if (request.RightRequest.EditRight)
            {
                var result = await existingDmRights.Where(x => x.DmRole.EditRight == true && request.Modules.Contains(x.IdModule))
                    .Select(y => y.IdModuleNavigation.ModuleName)
                    .AsNoTracking().ToListAsync();
                if (result.Any())
                {
                    return new DataManagementRightResponse()
                    {
                        AlreadyExistModules = string.Join(",", result),
                        Result = DataManagementRightResult.RightsAlreadyConfigured
                    };
                }
            }

            if (request.RightRequest.DeleteRight)
            {
                var result = await existingDmRights.Where(x => x.DmRole.DeleteRight == true && request.Modules.Contains(x.IdModule))
                    .Select(y => y.IdModuleNavigation.ModuleName)
                    .AsNoTracking().ToListAsync();
                if (result.Any())
                {
                    return new DataManagementRightResponse()
                    {
                        AlreadyExistModules = string.Join(",", result),
                        Result = DataManagementRightResult.RightsAlreadyConfigured
                    };
                }
            }

            if (request.RightRequest.DownloadRight)
            {
                var result = await existingDmRights.Where(x => x.DmRole.DownloadRight == true && request.Modules.Contains(x.IdModule))
                     .Select(y => y.IdModuleNavigation.ModuleName)
                    .AsNoTracking().ToListAsync();
                if (result.Any())
                {
                    return new DataManagementRightResponse()
                    {
                        AlreadyExistModules = string.Join(",", result),
                        Result = DataManagementRightResult.RightsAlreadyConfigured
                    };
                }
            }

            if (request.RightRequest.UploadRight)
            {
                var result = await existingDmRights.Where(x => x.DmRole.UploadRight == true && request.Modules.Contains(x.IdModule))
                     .Select(y => y.IdModuleNavigation.ModuleName)
                    .AsNoTracking().ToListAsync();
                if (result.Any())
                {
                    return new DataManagementRightResponse()
                    {
                        AlreadyExistModules = string.Join(",", result),
                        Result = DataManagementRightResult.RightsAlreadyConfigured
                    };
                }
            }

            return null;
        }

        public async Task<bool> IsDmUploadRights(int moduleId)
        {
            var filteredDmRights = await _repo.GetDMRightsByStaffId(_ApplicationContext.StaffId);
            if (!filteredDmRights.Any())
                filteredDmRights = await _repo.GetDMRightsByRoleIds(_ApplicationContext.RoleList);

            return filteredDmRights.Any(x => x.ModuleId == moduleId && x.UploadRight == true);
        }

        public async Task<ModuleListResponse> GetModulesByDmRoleId(int id)
        {
            var data = await _repo.GetModules();
            var dmRights = await _repo.GetDmRightsByDmRoleIds(new List<int>() { id });

            List<DmRefModule> modules = new List<DmRefModule>();
            foreach (var item in dmRights)
            {
                CreateRecusivelyModuleList(item.IdModule, data, modules);
            }
            return new ModuleListResponse()
            {
                List = GetModulesRecusively(modules, null),
                Result = ModuleListResult.Success
            };
        }


        /// <summary>
        /// based on child data get the parent data and map to into one method
        /// </summary>
        /// <param name="moduleId">based on this module picking parent data and add the filteredModules List</param>
        /// <param name="masterRefModules">all modules </param>
        /// <param name="finalModuleList">new module list based on child available modules to create</param>
        private void CreateRecusivelyModuleList(int moduleId, IEnumerable<DmRefModule> masterRefModules, List<DmRefModule> finalModuleList)
        {
            var module = masterRefModules.FirstOrDefault(x => x.Id == moduleId);
            if (module != null)
            {
                if (!finalModuleList.Any(x => x.Id == module.Id))
                    finalModuleList.Add(module);
                if (module.ParentId.HasValue)
                {
                    CreateRecusivelyModuleList(module.ParentId.Value, masterRefModules, finalModuleList);
                }
            }
        }

        /// <summary>
        /// based on staff or role id get the rights
        /// </summary>
        /// <returns></returns>
        private async Task<List<DMRightData>> GetFilteredDataManagementRights()
        {
            var filteredDmRights = await _repo.GetDMRightsByStaffId(_ApplicationContext.StaffId);
            if (!filteredDmRights.Any())
                filteredDmRights = await _repo.GetDMRightsByRoleIds(_ApplicationContext.RoleList);

            return filteredDmRights;
        }


    }
}
