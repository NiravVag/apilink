using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.CustomReport;


namespace DAL.Repositories
{
    public class InspectionCustomReportRepository:Repository, IInspectionCustomReportRepository
    {
        public InspectionCustomReportRepository(API_DBContext context) : base(context)
        {
        }


        public async Task<List<int>> GetFBReportCSIds(int fbReportId)
        {
            return await _context.FbReportReviewers
                .Where(x => x.Active.Value && x.FbReportId == fbReportId)
                .Select(x => x.ReviewerId.Value).ToListAsync();
        }
        public async Task<List<int>> GetFbReportQcIDs(int fbReportId)
        {
            return await _context.FbReportQcdetails
                            .Where(x => x.Active.Value && x.FbReportDetailId == fbReportId)
                            .Select(x => x.QcId.Value).ToListAsync();
        }

        public async Task<List<InspectionCustomReportStaff>> GetFBReportStaffList(List<int> fbFbUserIds)
        {
            return await _context.ItUserMasters
                            .Where(x => x.FbUserId.HasValue && fbFbUserIds.Contains(x.FbUserId.Value))
                            .Select(x => new InspectionCustomReportStaff()
                            { 
                                StaffId= x.StaffId,
                                PersonName=x.Staff.PersonName
                            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<int>> GetInspProductIds(int fbReportId)
        {
            return await _context.InspProductTransactions
                            .Where(x => x.Active.Value && x.FbReportId == fbReportId)
                            .Select(x => x.Id).ToListAsync();
        }

        public async Task<List<InspPurchaseOrderColorTrans>> GetInspPurchaseOrderColorTransactionList(int? fbreportid ,List<int> ProductIds, List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderColorTransactions.Where(y => y.Active.Value && ProductIds.Contains(y.ProductRefId.Value) && ProductIds.Contains(y.PoTrans.ProductRefId) && y.ProductRef.FbReportId== fbreportid && bookingIds.Contains(y.ProductRef.InspectionId))
                             .Select(x => new InspPurchaseOrderColorTrans()
                             {
                                 ColorTransactionId = x.Id,
                                 PoTransactionId = x.PoTransId,
                                 ProductRefId = x.ProductRefId,
                                 ColorCode = x.ColorCode,
                                 ColorName = x.ColorName,
                                 BookingQuantity = x.BookingQuantity,
                                 PickingQuantity = x.PickingQuantity
                             }).AsNoTracking().ToListAsync();
        }

        public async Task<List<RepFastTemplateConfig>> GetStandardTemplateConfigList(int EntityId, DateTime Inspection_From_Date)
        {
            return await _context.RepFastTemplateConfigs
                            .Where(x => x.Active.Value && x.Entityid == EntityId && x.IsStandardTemplate.Value && x.ScheduleFromDate<= Inspection_From_Date && x.ScheduleToDate>= Inspection_From_Date ).OrderByDescending(x => x.Sort)
                            .Select(x => new RepFastTemplateConfig()
                            {
                                Id = x.Id,
                                CustomerId = x.CustomerId,
                                TemplateId = x.TemplateId,
                                Template = x.Template,
                                ServiceTypeId = x.ServiceTypeId,
                                ProductCategoryId = x.ProductCategoryId,
                                IsStandardTemplate = x.IsStandardTemplate,
                                ScheduleFromDate = x.ScheduleFromDate,
                                ScheduleToDate = x.ScheduleToDate,
                                Sort = x.Sort,
                                BrandId = x.BrandId,
                                DepartId = x.DepartId,
                                FileExtensionId = x.FileExtensionId,
                                ReportToolTypeId = x.ReportToolTypeId
                            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<RepFastTemplateConfig>> GetTemplateConfigList(int EntityId, DateTime Inspection_From_Date, int? CustomerId)
        {
            return await _context.RepFastTemplateConfigs
                            .Where(x => x.Active.Value && x.Entityid== EntityId && !x.IsStandardTemplate.Value && x.CustomerId== CustomerId && x.ScheduleFromDate <= Inspection_From_Date && x.ScheduleToDate >= Inspection_From_Date).OrderByDescending(x => x.Sort)
                            .Select(x => new RepFastTemplateConfig()
                            {
                                Id = x.Id,
                                CustomerId=x.CustomerId,
                                TemplateId=x.TemplateId,
                                Template = x.Template,
                                ServiceTypeId=x.ServiceTypeId,
                                ProductCategoryId=x.ProductCategoryId,
                                IsStandardTemplate=x.IsStandardTemplate,
                                ScheduleFromDate=x.ScheduleFromDate,
                                ScheduleToDate=x.ScheduleToDate,
                                Sort=x.Sort,
                                BrandId=x.BrandId,
                                DepartId=x.DepartId,
                                FileExtensionId = x.FileExtensionId,
                                ReportToolTypeId = x.ReportToolTypeId
                            }).AsNoTracking().ToListAsync();
        }

    }
}
