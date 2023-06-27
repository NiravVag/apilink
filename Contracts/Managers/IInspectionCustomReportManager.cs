using DTO.CustomReport;
using DTO.InspectionCustomReport;
using DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Entities.Enums;

namespace Contracts.Managers
{
    public interface IInspectionCustomReportManager
    {
       
        Task<FBReportInfoResponse> FetchFBReportInfo(int fbReportMapId, string fbToken);
        string GetReportTemplate(int fbReportMapId, InspectionCustomReportItem OrderMainDetail, string ReportTemplateName);
        (string filePath, string uniqueId) FetchCloudReportUrl(MemoryStream reportmemory, string fileName, string fileExtension, FileContainerList container);
        Task<TemplateConfigResponse> GetTemplateConfigInfo(int fbReportMapId);
        Task<List<InspectionCustomReportStaff>> GetFBReportStaffList(List<int> fbFbUserIds);

    }
}
