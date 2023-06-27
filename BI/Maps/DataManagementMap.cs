using DTO.Common;
using DTO.CommonClass;
using DTO.DataManagement;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class DataManagementMap : ApiCommonData
    {
        //private void GetModuleHierachyData(ModuleHierarchyData moduleHierarchyData, int orderId, List<ModuleNameHierarchyData> moduleHierarchyDataList)
        //{
        //    ModuleNameHierarchyData data = new ModuleNameHierarchyData();
        //    data.Id = moduleHierarchyData.Id;
        //    data.Name = moduleHierarchyData.Name;
        //    data.OrderId = orderId;
        //    moduleHierarchyDataList.Add(data);
        //    if (moduleHierarchyData.HierarchyData != null)
        //    {
        //        orderId = orderId + 1;
        //        GetModuleHierachyData(moduleHierarchyData.HierarchyData, orderId, moduleHierarchyDataList);
        //    }

        //}


        public List<DMDetailSummaryData> GetDataManagementResultData(List<DMDetailSummaryFileData> dataManagementList,
                                                                                        List<ModuleHierarchy> hierachyDataList, List<DMRightData> dmRights,
            List<ParentDataSource> dmBrands, List<ParentDataSource> dmDepartments)

        {
            List<DMDetailSummaryData> dmDetailSummaryDataList = new List<DMDetailSummaryData>();

            foreach (var item in dataManagementList)
            {
                string moduleName = string.Empty;
                var dmDetailSummaryData = new DMDetailSummaryData();

                List<ModuleNameHierarchyData> moduleNameHierarchyData = new List<ModuleNameHierarchyData>();

                var moduleHierachyData = hierachyDataList.FirstOrDefault(x => x.ModuleId == item.ModuleId);

                if (moduleHierachyData != null)
                {
                    moduleHierachyData.ModuleHierarchyList = moduleHierachyData.ModuleHierarchyList.OrderByDescending(x => x.OrderId).ToList();
                    dmDetailSummaryData.Module = string.Join("/", moduleHierachyData.ModuleHierarchyList.Select(x => x.Name));
                }

                var dmRight = dmRights.FirstOrDefault(x => x.ModuleId == item.ModuleId);

                if (dmRight != null)
                {
                    dmDetailSummaryData.EditRight = dmRight.EditRight;
                    dmDetailSummaryData.DeleteRight = dmRight.DeleteRight;
                    dmDetailSummaryData.DownloadRight = dmRight.DownloadRight;
                }

                dmDetailSummaryData.Customer = item.Customer;
                dmDetailSummaryData.Description = item.Description;
                dmDetailSummaryData.DocumentName = item.FileName;
                dmDetailSummaryData.DocumentType = item.Module;
                dmDetailSummaryData.DocumentSize = String.Format("{0:0.00}", item.FileSize) + " MB";
                dmDetailSummaryData.DocumentUrl = item.FileUrl;
                dmDetailSummaryData.DocumentId = item.FileId;
                dmDetailSummaryData.Id = item.Id;
                dmDetailSummaryData.DmDetailId = item.DmDetailId;
                dmDetailSummaryData.CreatedOn = item.CreatedOn.GetValueOrDefault().ToString(StandardDateFormat);
                dmDetailSummaryData.Brands = dmBrands != null ? string.Join(", ", dmBrands.Where(x => x.ParentId == item.Id).Select(y => y.Name).ToList()) : "";
                dmDetailSummaryData.Departments = dmDepartments != null ? string.Join(", ", dmDepartments.Where(x => x.ParentId == item.Id).Select(y => y.Name).ToList()) : "";
                dmDetailSummaryDataList.Add(dmDetailSummaryData);



            }

            return dmDetailSummaryDataList;

        }


        public DMUserRight MapDmUserRightsSummary(DMRoleRepoItem dMRole, List<ParentDataSource> modules)
        {
            List<string> rights = new List<string>();
            if (dMRole.DownloadRight)
                rights.Add("Download");
            if (dMRole.UploadRight)
                rights.Add("Upload");
            if (dMRole.EditRight)
                rights.Add("Edit");
            if (dMRole.DeleteRight)
                rights.Add("Delete");

            return new DMUserRight()
            {
                Role = dMRole.Role,
                Id = dMRole.Id,
                Staff = dMRole.Staff,
                Rights = string.Join(", ", rights),
                Access = string.Join(", ", modules.Where(x => x.ParentId == dMRole.Id).Select(y => y.Name).ToList())
            };
        }

        public DmRole MapDmRoleEntity(SaveDataManagementRightRequest request, int entityId, int userId)
        {
            return new DmRole()
            {
                RoleId = request.RightRequest.IdRole,
                StaffId = request.RightRequest.IdStaff,
                EditRight = request.RightRequest.EditRight,
                DeleteRight = request.RightRequest.DeleteRight,
                DownloadRight = request.RightRequest.DownloadRight,
                UploadRight = request.RightRequest.UploadRight,
                EntityId = entityId,
                CreatedOn = DateTime.Now,
                Active = true,
                CreatedBy = userId
            };
        }

        public void MapDmRoleEntity(DmRole entity, SaveDataManagementRightRequest request, int userId)
        {
            entity.RoleId = request.RightRequest.IdRole;
            entity.StaffId = request.RightRequest.IdStaff;
            entity.EditRight = request.RightRequest.EditRight;
            entity.DeleteRight = request.RightRequest.DeleteRight;
            entity.DownloadRight = request.RightRequest.DownloadRight;
            entity.UploadRight = request.RightRequest.UploadRight;
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
        }
    }
}
