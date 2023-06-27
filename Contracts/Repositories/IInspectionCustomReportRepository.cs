using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.EntPages;
using DTO.Inspection;
using DTO.Kpi;
using DTO.Quotation;
using DTO.Report;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.CustomReport;
using System;
namespace Contracts.Repositories
{
    public interface IInspectionCustomReportRepository : IRepository
    {
        /// <summary>
        /// Get Fb Reports Reviewer by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<List<int>> GetFBReportCSIds(int fbReportId);

        /// <summary>
        /// Get Fb Reports QC by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<List<int>> GetFbReportQcIDs(int fbReportId);

        Task<List<InspectionCustomReportStaff>> GetFBReportStaffList(List<int> fbFbUserIds);


        Task<List<RepFastTemplateConfig>> GetStandardTemplateConfigList(int EntityId, DateTime Inspection_From_Date);

        Task<List<RepFastTemplateConfig>> GetTemplateConfigList(int EntityId,  DateTime Inspection_From_Date, int? CustomerId);


        Task<List<int>> GetInspProductIds(int fbReportId);

        Task<List<InspPurchaseOrderColorTrans>> GetInspPurchaseOrderColorTransactionList(int? fbreportid, List<int> ProductIds, List<int> bookingIds);

    }
}
