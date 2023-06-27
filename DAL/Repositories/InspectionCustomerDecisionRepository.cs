using Contracts.Repositories;
using DTO.EmailSendingDetails;
using DTO.Inspection;
using DTO.InspectionCertificate;
using DTO.InspectionCustomerDecision;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// To handle customer decision repository.
    /// </summary>
    public class InspectionCustomerDecisionRepository : Repository, IInspectionCustomerDecisionRepository
    {
        /// <summary>
        /// Constructor part
        /// </summary>
        /// <param name="context"></param>
        public InspectionCustomerDecisionRepository(API_DBContext context) : base(context)
        {

        }

        /// <summary>
        /// Get customer default decision list 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerDecisionRepo>> GetCustomerDefaultDecisionList()
        {
            return await _context.RefInspCusDecisionConfigs.Where(x => x.Active.HasValue && x.Active.Value
                         && x.Default.HasValue && x.Default.Value)
                        .Select(x => new CustomerDecisionRepo()
                        {
                            Id = x.Id,
                            Name = x.CusDec.Name,
                            CustomName = x.CustomDecisionName
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get customer decision list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<CustomerDecisionRepo>> GetCustomerDecisionListByCustomer(int customerId)
        {
            return await _context.RefInspCusDecisionConfigs.Where(x => x.Active.HasValue && x.Active.Value
                         && x.CustomerId == customerId)
                        .Select(x => new CustomerDecisionRepo()
                        {
                            Id = x.Id,
                            Name = x.CusDec.Name,
                            CustomName = x.CustomDecisionName
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Adding Inspection customer decision data 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddCustomerDecision(InspRepCusDecision entity)
        {
            _context.InspRepCusDecisions.Add(entity);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// updating Inspection customer decision data 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateCustomerDecision(InspRepCusDecision entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Customer decision status
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<ReportCustomerDecision> GetReportCustomerDecision(int reportId)
        {
            return await _context.InspRepCusDecisions.Where(x => x.ReportId == reportId &&
            x.Active.HasValue && x.Active.Value).Select(x => new ReportCustomerDecision
            {
                CustomerDecisionCustomStatus = x.CustomerResult.CustomDecisionName,
                CustomerDecisionStatus = x.CustomerResult.CusDec.Name,
                Comments = x.Comments,
                CustomerResultId = x.CustomerResult.CusDecId
            }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Customer decisions data 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<InspRepCusDecision> GetCustomerDecisionData(int reportId)
        {
            return await _context.InspRepCusDecisions.
                Where(x => x.ReportId == reportId && x.Active.HasValue && x.Active.Value).
                FirstOrDefaultAsync();
        }
        ///<summary>
        /// get customer decision list based on list of fbreport id
        ///</summary>
        /// <param name="fbReportIds"></param>
        /// <returns>customer de</returns>
        public async Task<List<FBReportCustomerDecision>> GetCustomerDescistionWithReportId(List<int> fbReportIds)
        {
            return await _context.InspRepCusDecisions.Where(x => x.Active.HasValue && x.Active.Value && fbReportIds.Contains(x.ReportId))
                .Select(x => new FBReportCustomerDecision
                {
                    ReportId = x.ReportId,
                    CustomerDecisionId = x.CustomerResult.CusDecId
                }).ToListAsync();
        }
        ///<summary>
        /// get customer decision list based on list of booking id
        ///</summary>
        /// <param name="fbReportIds"></param>
        /// <returns>customer decision</returns>
        public async Task<List<CustomerDecisionCount>> GetCustomerDescistionWithBookingId(List<int> bookingIds)
        {
            return await _context.FbReportDetails.Where(x => x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
                .Select(x => new CustomerDecisionCount
                {
                    BookingId = x.InspectionId.GetValueOrDefault(),
                    Count = x.InspRepCusDecisions.Count(y => y.Active == true)
                }).ToListAsync();
        }

        /// <summary>
        /// get product data for the bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerDecisionProductList>> GetCustomerDecisionProductList(List<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active == true
            && x.Inspection.InspTranServiceTypes.Any(y => y.ServiceTypeId != (int)InspectionServiceTypeEnum.Container))
                .Select(x => new CustomerDecisionProductList
                {
                    BookingId = x.InspectionId,
                    ReportId = x.FbReportId,
                    ReportTitle = x.FbReport.ReportTitle,
                    ProductId = x.Product.ProductId,
                    ResportResultId = x.FbReport.ResultId,
                    ReportResultName = x.FbReport.Result.ResultName,
                    CustomerDecisionResultId = x.FbReport.InspRepCusDecisions.Where(y => y.Active == true).Select(y => y.CustomerResultId).FirstOrDefault(),
                    CustomerDecisionComment = x.FbReport.InspRepCusDecisions.Where(y => y.Active == true).Select(y => y.Comments).FirstOrDefault(),
                    CustomerDecisionResultCusDecId = x.FbReport.InspRepCusDecisions.Where(y => y.Active == true).Select(y => y.CustomerResult.CusDecId).FirstOrDefault(),
                    ProductPhoto = x.FbReport.MainProductPhoto
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the container data for the bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerDecisionProductList>> GetCustomerDecisionContainerList(List<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
              && bookingIds.Contains(y.InspectionId) && y.ContainerRefId > 0).Select(z => new CustomerDecisionProductList()
              {
                  BookingId = z.InspectionId,
                  ProductId = z.ContainerRef.ContainerId.ToString(),
                  ReportId = z.ContainerRef.FbReportId,
                  ContainerId = z.ContainerRef.ContainerId,
                  ReportTitle = z.ContainerRef.FbReport.ReportTitle,
                  ResportResultId = z.ContainerRef.FbReport.ResultId,
                  ReportResultName = z.ContainerRef.FbReport.Result.ResultName,
                  CustomerDecisionResultId = z.ContainerRef.FbReport.InspRepCusDecisions.Where(y => y.Active == true).Select(y => y.CustomerResultId).FirstOrDefault(),
                  CustomerDecisionComment = z.ContainerRef.FbReport.InspRepCusDecisions.Where(y => y.Active == true).Select(y => y.Comments).FirstOrDefault(),
                  CustomerDecisionResultCusDecId = z.ContainerRef.FbReport.InspRepCusDecisions.Where(y => y.Active == true).Select(y => y.CustomerResult.CusDecId).FirstOrDefault(),
                  ProductPhoto = z.ContainerRef.FbReport.MainProductPhoto
              }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Customer decisions data 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<InspRepCusDecision>> GetCustomerDecisionListData(List<int> reportIdList)
        {
            return await _context.InspRepCusDecisions.
                Where(x => reportIdList.Contains(x.ReportId) && x.Active.HasValue && x.Active.Value).
                ToListAsync();
        }

        /// <summary>
        /// get probematic remarks
        /// </summary>
        /// <param name="summaryName"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<CusDecisionProblematicRemarks>> GetProblematicRemarksCd(int id, int reportId)
        {
            return await _context.FbReportProblematicRemarks.Where(x => x.FbReportSummary.FbReportDetailId == reportId && x.FbReportSummaryId == id
            && x.FbReportSummary.Active == true && x.Active == true)
                .Select(x => new CusDecisionProblematicRemarks
                {
                    ReportId = reportId,
                    ProductId = x.Product.ProductId,
                    SubCat = x.SubCategory,
                    SubCat2 = x.SubCategory2,
                    Remarks = x.Remarks,
                    Result = x.Result
                }).ToListAsync();

        }

        public async Task<int> GetBookingIdByReportId(int reportId)
        {
            return await _context.FbReportDetails.Where(x => x.Id == reportId).Select(x => x.InspectionId.GetValueOrDefault()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get customer decision list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IQueryable<CustomerDecisionRepo> GetCustomerDecisionListByEfCore()
        {
            return _context.RefInspCusDecisionConfigs.Where(x => x.Active.HasValue && x.Active.Value)
                        .Select(x => new CustomerDecisionRepo()
                        {
                            Id = x.Id,
                            Name = x.CusDec.Name,
                            CustomName = x.CustomDecisionName,
                            CustomerId = x.CustomerId,
                            IsDefault = x.Default,
                            CusDecId = x.CusDecId
                        }).AsNoTracking();
        }


        /// <summary>
        /// get product data for the bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerDecisionProductList>> GetCustomerDecisionProductListByEfCore(IQueryable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.Active == true
            && x.Inspection.InspTranServiceTypes.Any(y => y.ServiceTypeId != (int)InspectionServiceTypeEnum.Container))
                .Select(x => new CustomerDecisionProductList
                {
                    ProductRefId = x.Id,
                    BookingId = x.InspectionId,
                    ReportId = x.FbReportId,
                    ProductId = x.Product.ProductId,
                    ProdDesc = x.Product.ProductDescription,
                    ResportResultId = x.FbReport.ResultId,
                    ReportResultName = x.FbReport.Result.ResultName
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get the container data for the bookings
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerDecisionProductList>> GetCustomerDecisionContainerListByEfCore(IQueryable<int> bookingIds)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
              && bookingIds.Contains(y.InspectionId) && y.ContainerRefId > 0).Select(z => new CustomerDecisionProductList()
              {
                  ProductRefId = z.ProductRefId,
                  ContainerId = z.ContainerRef.ContainerId,
                  BookingId = z.InspectionId,
                  ProductId = z.ProductRef.Product.ProductId,
                  ProdDesc = z.ProductRef.Product.ProductDescription,
                  ReportId = z.ContainerRef.FbReportId,
                  ResportResultId = z.ContainerRef.FbReport.ResultId,
                  ReportResultName = z.ContainerRef.FbReport.Result.ResultName
              }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get Inspection summary by report Id and its type
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InspectionReportSummary>> GetCustomerDecisionInspectionSummary(IQueryable<int> bookingIdlist)
        {
            return await _context.FbReportInspSummaries.Where(y => y.Active.HasValue && y.Active.Value
            && bookingIdlist.Contains(y.FbReportDetail.InspectionId.Value) && y.FbReportInspsumTypeId == (int)InspSummaryType.Main).OrderBy(c => c.Sort).Select(z => new InspectionReportSummary()
            {
                Id = z.Id,
                Name = z.Name,
                Photos = z.FbReportInspSummaryPhotos.Select(x => x.Photo).ToList(),
                Remarks = z.Remarks,
                FbReportDetailId = z.FbReportDetailId,
                Result = z.ResultNavigation.ResultName,
                ResultId = z.ResultId
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CustomerDecisionProductList>> GetCustomerDecisionByEfCore(IQueryable<int> bookingIds)
        {
            return await _context.InspRepCusDecisions.Where(x => x.Active.Value && bookingIds.Contains(x.Report.InspectionId.Value))
                .Select(x => new CustomerDecisionProductList
                {
                    ReportId = x.ReportId,
                    CreatedOn = x.CreatedOn,
                    CustomerDecisionResultId = x.CustomerResultId,
                    CustomerDecisionName = x.CustomerResult.CustomDecisionName,
                    CustomerDecisionComment = x.Comments,
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<CustomerDecisionProductList>> GetCustomerDecisionByEfCoreReportIds(IQueryable<int> reportIds)
        {
            return await _context.InspRepCusDecisions.Where(x => x.Active.Value && reportIds.Contains(x.ReportId))
                .Select(x => new CustomerDecisionProductList
                {
                    ReportId = x.ReportId,
                    CreatedOn = x.CreatedOn,
                    CustomerDecisionResultId = x.CustomerResultId,
                    CustomerDecisionName = x.CustomerResult.CustomDecisionName,
                    CustomerDecisionComment = x.Comments,
                }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<InspRepCusDecisionTemplate>> GetCusDecisionTemplate()
        {
            return await _context.InspRepCusDecisionTemplates.AsNoTracking().ToListAsync();
        }
    }
}
