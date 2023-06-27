using Contracts.Repositories;
using DTO.CommonClass;
using DTO.FullBridge;
using DTO.Kpi;
using DTO.Report;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FullBridgeRepository : Repository, IFullBridgeRepository
    {

        public FullBridgeRepository(API_DBContext context) : base(context)
        {
        }
        /// <summary>
        /// Get inspection reports by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<InspProductTransaction> GetInspectionReports(int reportId)
        {
            return await _context.InspProductTransactions.
                              Include(x => x.Inspection).
                              ThenInclude(x => x.SchScheduleCS).
                              ThenInclude(x => x.Cs).
                               ThenInclude(x => x.ItUserMasters).
                               IgnoreQueryFilters().
                              FirstOrDefaultAsync(x => x.FbReportId == reportId && x.Active.HasValue && x.Active.Value);
        }


        /// <summary>
        /// Get Factory and Address by Report Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<InspProductTransaction> GetInspectionReportsFactoryAddress(int reportId)
        {
            return await _context.InspProductTransactions.
                              Include(x => x.Inspection).
                              ThenInclude(x => x.Factory).
                                ThenInclude(x => x.SuAddresses).
                                FirstOrDefaultAsync(x => x.FbReportId == reportId && x.Active.HasValue && x.Active.Value);
        }

        /// <summary>
        /// Get Container Reports Factory Address by Report Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<InspContainerTransaction> GetContainerReportsFactoryAddress(int reportId)
        {
            return await _context.InspContainerTransactions.
                              Include(x => x.Inspection).
                              ThenInclude(x => x.Factory).
                                ThenInclude(x => x.SuAddresses).
                              FirstOrDefaultAsync(x => x.FbReportId == reportId && x.Active.HasValue && x.Active.Value);
        }


        /// <summary>
        /// get inspection reports from container transaction
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<InspContainerTransaction> GetInspectionReportsFromContainer(int reportId)
        {
            return await _context.InspContainerTransactions.
                              Include(x => x.Inspection).
                              ThenInclude(x => x.SchScheduleCS).
                              ThenInclude(x => x.Cs).
                               ThenInclude(x => x.ItUserMasters).
                               IgnoreQueryFilters().
                              FirstOrDefaultAsync(x => x.FbReportId == reportId && x.Active.HasValue && x.Active.Value);
        }


        /// <summary>
        /// Get inspection fb reports by fb report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<FbReportDetail> GetInspectionFbReports(int reportId)
        {
            return await _context.FbReportDetails.
                            Include(x => x.FbReportInspSummaries).
                               ThenInclude(x => x.FbReportInspSummaryPhotos).
                            Include(x => x.FbReportInspSummaries).
                               ThenInclude(x => x.FbReportProblematicRemarks).
                            Include(x => x.FbReportInspSummaries).
                               ThenInclude(x => x.FbReportInspSubSummaries).
                               Include(x => x.FbReportQuantityDetails).
                               Include(x => x.FbReportInspDefects).
                                 ThenInclude(x => x.FbReportDefectPhotos).
                               Include(x => x.FbReportQcdetails).
                               Include(x => x.FbReportAdditionalPhotos).
                               Include(x => x.InspContainerTransactions).
                               Include(x => x.InspRepCusDecisions).
                               Include(x => x.FbReportComments).
                               Include(x => x.FbReportOtherInformations).
                               Include(x => x.FbReportPackingBatteryInfos).
                               Include(x => x.FbReportPackingDimentions).
                               Include(x => x.FbReportPackingInfos).
                               Include(x => x.FbReportPackingWeights).
                               Include(x => x.FbReportReviewers).
                               Include(x => x.FbReportSamplePickings).
                               Include(x => x.FbReportSampleTypes).
                               Include(x => x.FbReportProductWeights).
                               Include(x => x.FbReportProductDimentions).
                               Include(x => x.FbReportProductBarcodesInfos).
                               Include(x => x.FbReportFabricControlmadeWiths).
                               Include(x => x.FbReportFabricDefects).
                               Include(x => x.Inspection).OrderBy(x => x.Id).
                                                              Include(x => x.FbReportRdnumbers).
                               Include(x => x.FbReportPackingPackagingLabellingProducts).
                                    ThenInclude(x => x.FbReportPackingPackagingLabellingProductDefects).
                               Include(x => x.FbReportQualityPlans).ThenInclude(x => x.FbReportQualityPlanMeasurementDefectsPoms).
                               Include(x => x.FbReportQualityPlans).ThenInclude(x => x.FbReportQualityPlanMeasurementDefectsSizes).
                               IgnoreQueryFilters().
                               FirstOrDefaultAsync(x => x.Id == reportId && x.Active.HasValue && x.Active.Value);
        }
        /// <summary>
        /// get only fb report details 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<FbReportDetail> GetInspectionFbReportsWithStatus(int reportId)
        {
            return await _context.FbReportDetails.
                               FirstOrDefaultAsync(x => x.Id == reportId && x.Active.HasValue && x.Active.Value);
        }


        /// <summary>
        /// Get Purchase order Details based on the po number and booking products.
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="strPoNumber"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<InspPurchaseOrderTransaction> GetInspectionTransaction(int reportId, string strPoNumber, int productId)
        {

            return await _context.InspPurchaseOrderTransactions

                 .Where(y => y.Active.HasValue && y.Active.Value &&

                  y.Po.Pono == strPoNumber &&

                  y.ProductRef.FbReportId == reportId

                  && y.ProductRef.Product.FbCusProdId == productId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get container size id 
        /// </summary>
        /// <param name="containerSize"></param>
        /// <returns></returns>
        public async Task<int> GetContainersizeId(string containerSize)
        {

            return await _context.RefContainerSizes

                  .Where(x => x.Name == containerSize && x.Active).Select(x => x.Id).

                  FirstOrDefaultAsync();

        }


        /// <summary>
        /// get inspection transaction by container
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="strPoNumber"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<InspPurchaseOrderTransaction> GetInspectionTransaction(int reportId, string strPoNumber, int productId, int containerId)
        {

            return await _context.InspPurchaseOrderTransactions

             .Where(y => y.Active.HasValue && y.Active.Value &&

              y.Po.Pono == strPoNumber &&

              y.ContainerRef.FbReportId == reportId

              && y.ContainerRefId == containerId

              && y.ProductRef.Product.FbCusProdId == productId).FirstOrDefaultAsync();

        }

        /// <summary>
        /// Get inspection Product and po information by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<ReportProductsAndPo>> GetInspectionProductContainerReportData(int reportId)
        {
            return await _context.InspPurchaseOrderTransactions

             .Where(y => y.Active.HasValue && y.Active.Value && (y.ContainerRef.FbReportId == reportId || y.ProductRef.FbReportId == reportId)).

              Select(x => new ReportProductsAndPo
              {
                  productId = x.ProductRef.Product.Id,
                  fbProductId = x.ProductRef.Product.FbCusProdId,
                  containerRefId = x.ContainerRefId,
                  poNumber = x.Po.Pono,
                  poTrnsactionId = x.Id,
                  reportId = reportId,
                  productTransactionId = x.ProductRefId
              })
              .IgnoreQueryFilters()
              .ToListAsync();
        }

        /// <summary>
        /// Get Inspection Po and Color Data
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<ReportProductsAndPo>> GetInspectionProductColorReportData(int reportId)
        {
            return await _context.InspPurchaseOrderColorTransactions

             .Where(y => y.Active.HasValue && y.Active.Value && y.PoTrans.ProductRef.FbReportId == reportId).

              Select(x => new ReportProductsAndPo
              {
                  productId = x.ProductRef.Product.Id,
                  fbProductId = x.ProductRef.Product.FbCusProdId,
                  poNumber = x.PoTrans.Po.Pono,
                  colorName = x.ColorName,
                  poTrnsactionId = x.PoTransId.GetValueOrDefault(),
                  colorTransactionId = x.Id,
                  reportId = reportId,
                  productTransactionId = x.ProductRefId.GetValueOrDefault()
              })
              .IgnoreQueryFilters()
              .ToListAsync();
        }

        /// <summary>
        /// Get inspection fb reports by fb report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FbReportDetail>> GetFbReportDetails()
        {
            return await _context.FbReportDetails.
                  Include(x => x.FbReviewStatusNavigation).
                  Include(x => x.FbFillingStatusNavigation).
                  Include(x => x.FbReportStatusNavigation).

                            Where(x => x.Active.HasValue && x.Active.Value).ToListAsync();
        }

        /// <summary>
        /// get customer Product id by fb customer product id
        /// </summary>
        /// <param name="fbProductId"></param>
        /// <returns></returns>
        public async Task<int?> GetProductIdByFbProductId(int? fbProductId)
        {
            return await _context.CuProducts.Where(x => x.FbCusProdId == fbProductId)
                   .Select(x => x.Id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// get Aql level id from fb aql value.
        /// </summary>
        /// <param name="fbAqlLevel"></param>
        /// <returns></returns>
        public async Task<int?> GetAqlLevelbyFbAql(string fbAqlLevel)
        {
            return await _context.RefLevelPick1S.Where(x => x.Fbvalue == fbAqlLevel)
                   .Select(x => x.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Add inspection Report from full bridge
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddInspectionFbReport(FbReportDetail entity)
        {
            _context.FbReportDetails.Add(entity);
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove inspection fb report details
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> RemoveInspectionFbReport(FbReportDetail entity)
        {

            if (entity.FbReportInspSummaries != null && entity.FbReportInspSummaries.Any())
            {
                foreach (var item in entity.FbReportInspSummaries)
                {
                    if (item.FbReportInspSummaryPhotos.Count > 0)
                        RemoveEntities(item.FbReportInspSummaryPhotos);
                }

                RemoveEntities(entity.FbReportInspSummaries);
            }

            if (entity.FbReportQuantityDetails != null && entity.FbReportQuantityDetails.Any())
            {
                RemoveEntities(entity.FbReportQuantityDetails);
            }
            if (entity.FbReportInspDefects != null && entity.FbReportInspDefects.Any())
            {
                RemoveEntities(entity.FbReportInspDefects);
            }
            if (entity.FbReportQcdetails != null && entity.FbReportQcdetails.Any())
            {
                RemoveEntities(entity.FbReportQcdetails);
            }

            if (entity.FbReportAdditionalPhotos != null && entity.FbReportAdditionalPhotos.Any())
            {
                RemoveEntities(entity.FbReportAdditionalPhotos);
            }

            _context.Entry(entity).State = EntityState.Deleted;
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Inspection Reports
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateInspectionFbReport(FbReportDetail entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        /// <summary>
        ///  update inspection po status 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateInspectionStatus(InspTransaction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<int> UpdateInspectionFactoryAddress(SuAddress entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get inspection fb reports by fb report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<FbReportDetail>> GetFbReportStatusCustomerReport(IQueryable<int> bookingId)
        public async Task<IEnumerable<FBReportDetails>> GetFbReportStatusCustomerReport()
        {
            return await _context.FbReportDetails.Select(x => new FBReportDetails
            {
                ReportId = x.Id,
                ReportTitle = x.ReportTitle,

            }).ToListAsync();
        }

        /// <summary>
        /// Check all the reports not validated or invalidated.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<bool> checkAllTheReportsNotValidatedOrInvalidated(int bookingId)
        {
            List<int?> reportStatusList = new List<int?>() { (int)FBStatus.ReportDraft, (int)FBStatus.ReportArchive, (int)FBStatus.ReportInValidated };

            return await _context.FbReportDetails.

                     Where(x => (x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId)

                     || x.InspContainerTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId))

                     && (reportStatusList.Contains(x.FbReportStatus) || x.FbReportStatus == null)).IgnoreQueryFilters().AnyAsync();
        }

        /// <summary>
        /// any report is available or not based on booking id and report status ids
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<bool> IsAnyReportsAvailableByReportStatuses(int bookingId, IEnumerable<int?> reportStatus)
        {
            return await _context.FbReportDetails.
                     Where(x => (x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId)
                     || x.InspContainerTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId))
                     && reportStatus.Contains(x.FbReportStatus) && x.Active.Value).IgnoreQueryFilters().AnyAsync();
        }

        /// <summary>
        /// Get report ids by booking id.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<ReportIdData>> getReportIdsbyBooking(int bookingId)
        {

            return await _context.FbReportDetails.

                     Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId)

                     || x.InspContainerTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId)

                     ).Select(x => new ReportIdData
                     {
                         ApiReportId = x.Id,
                         FbReportId = x.FbReportMapId.GetValueOrDefault()
                     }).ToListAsync();
        }

        /// <summary>
        /// get report ids by booking service date
        /// </summary>
        /// <param name="bookingStartDate"></param>
        /// <param name="bookingEndDate"></param>
        /// <returns></returns>
        public async Task<List<ReportIdData>> getReportIdsbyBookingServiceDates(DateTime bookingStartDate, DateTime bookingEndDate)
        {
            return await _context.FbReportDetails.

                     Where(x => x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value

                     && (y.Inspection.ServiceDateTo <= bookingStartDate

                     && y.Inspection.ServiceDateTo >= bookingEndDate) && x.FbReportStatus != (int)FBStatus.ReportValidated)

                     || x.InspContainerTransactions.Any(y => y.Active.HasValue && y.Active.Value

                     && (y.Inspection.ServiceDateTo <= bookingStartDate && y.Inspection.ServiceDateTo >= bookingEndDate) && x.FbReportStatus != (int)FBStatus.ReportValidated)

                     ).Select(x => new ReportIdData
                     {
                         ApiReportId = x.Id,
                         FbReportId = x.FbReportMapId.GetValueOrDefault()
                     }).ToListAsync();
        }

        public async Task<List<ReportIdData>> getNonValidatedReportIdsbyBooking(int bookingId)
        {

            return await _context.FbReportDetails.

                     Where(x => (x.InspProductTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId)

                     || x.InspContainerTransactions.Any(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId))

                       && x.FbReportStatus != (int)FBStatus.ReportValidated

                     ).Select(x => new ReportIdData
                     {
                         ApiReportId = x.Id,
                         FbReportId = x.FbReportMapId.GetValueOrDefault()
                     }).ToListAsync();
        }

        public async Task<FBReportDetails> GetFbReportStatusCustomerReportbyBooking(int reportId)
        {
            return await _context.FbReportDetails.Where(x => x.Id == reportId).Select(x => new FBReportDetails
            {
                ReportId = x.Id,
                StatusName = x.FbReportStatusNavigation.StatusName,
                InspectedQuantity = x.FbReportQuantityDetails.FirstOrDefault().InspectedQuantity,
                FinalReportPath = x.FinalReportPath,
                OverAllResult = x.OverAllResult,
                ReportTitle = x.ReportTitle
            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FBReportDetails>> GetFbReportStatusListCustomerReportbyBooking(List<int> reportIds)
        {
            return await _context.FbReportDetails.Where(x => reportIds.ToList().Contains(x.Id)).Select(x => new FBReportDetails
            {
                ReportId = x.Id,
                FbReportId = x.FbReportMapId,
                StatusName = x.FbReportStatusNavigation.StatusName,
                FillingStatus = x.FbFillingStatusNavigation.StatusName,
                ReviewStatus = x.FbReviewStatusNavigation.StatusName,
                FillingStatusId = x.FbFillingStatus,
                ReviewStatusId = x.FbReviewStatus,
                ReportStatusId = x.FbReportStatus,
                InspectedQuantity = x.FbReportQuantityDetails.FirstOrDefault().InspectedQuantity,
                FinalReportPath = x.FinalReportPath,
                OverAllResult = x.OverAllResult,
                ReportTitle = x.ReportTitle,
                ReportResult = x.Result.ResultName,
                ReportStatus = x.FbReportStatusNavigation.FbstatusName,
                ReportPath = x.FinalReportPath,
                FinalManualReportPath = x.FinalManualReportPath

            }).ToListAsync();
        }
        /// <summary>
        /// Get inspection id by fb report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<int> GetInspectionIdByReportId(int reportId)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && x.FbReportId == reportId).Select(x => x.InspectionId).FirstOrDefaultAsync();
        }


        public async Task<int> GetApiReportIdbyFbReport(int fbReportId)
        {
            return await _context.FbReportDetails.Where(x => x.Active.Value && x.FbReportMapId == fbReportId).Select(x => x.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get fb report title list by report id list
        /// </summary>
        /// <param name="fbReportMapIdList"></param>
        /// <returns></returns>
        public async Task<List<FBReportDetails>> GetFbReportTitleListByReportIds(IEnumerable<int?> fbReportMapIdList)
        {
            return await _context.FbReportDetails.Where(x => x.Active.Value && fbReportMapIdList.ToList().Contains(x.FbReportMapId)).Select(x => new FBReportDetails
            {
                FbReportId = x.FbReportMapId,
                ReportTitle = x.ReportTitle
            }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get schedule cs table
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="staffIdList"></param>
        /// <returns></returns>
        public async Task<List<SchScheduleC>> GetCSDetailList(int bookingId, List<int?> staffIdList)
        {
            return await _context.SchScheduleCs
                            .Where(x => x.Active && x.BookingId == bookingId && staffIdList.Contains(x.Csid)).ToListAsync();
        }
        /// <summary>
        /// get schedule qc table
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="staffIdList"></param>
        /// <returns></returns>
        public async Task<List<SchScheduleQc>> GetQCDetailList(int bookingId, List<int?> staffIdList)
        {
            return await _context.SchScheduleQcs
                        .Where(x => x.Active && x.BookingId == bookingId && staffIdList.Contains(x.Qcid)).ToListAsync();
        }

        /// <summary>
        /// get audit auditor list
        /// </summary>
        /// <param name="auditId"></param>
        /// <param name="staffIdList"></param>
        /// <returns></returns>
        public async Task<List<AudTranAuditor>> GetAuditAuditorList(int auditId, List<int?> staffIdList)
        {
            return await _context.AudTranAuditors
                        .Where(x => x.Active && x.AuditId== auditId&& staffIdList.Contains(x.StaffId)).ToListAsync();
        }

        /// <summary>
        /// get audit cs list
        /// </summary>
        /// <param name="auditId"></param>
        /// <param name="staffIdList"></param>
        /// <returns></returns>
        public async Task<List<AudTranC>> GetAuditCSList(int auditId, List<int?> staffIdList)
        {
            return await _context.AudTranCs
                        .Where(x => x.Active && x.AuditId == auditId && staffIdList.Contains(x.StaffId)).ToListAsync();
        }

        /// <summary>
        /// get staff id list by user ids
        /// </summary>
        /// <param name="fbUserIds"></param>
        /// <returns></returns>
        public async Task<List<int?>> GetFBReportStaffList(List<int> fbUserIds)
        {
            return await _context.ItUserMasters
                            .Where(x => x.FbUserId.HasValue && fbUserIds.Contains(x.FbUserId.Value)).AsNoTracking()
                            .Select(x => x.StaffId).ToListAsync();
        }

        public async Task<AudTransaction> GetAuditFbReports(int reportId)
        {
            return await _context.AudTransactions
                .Include(x => x.AudFbReportCheckpoints).IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.FbreportId == reportId);
        }
        public async Task<int> GetFbServiceTypeId(int serviceId)
        {
            return await _context.RefServiceTypes.Where(x => x.FbServiceTypeId == serviceId).IgnoreQueryFilters().Select(x => x.ServiceId.GetValueOrDefault()).FirstOrDefaultAsync();
        }

        public async Task<List<AudTranCSDetail>> AudTranCSForAudit(int auditId)
        {
            return await _context.AudTranCs.Where(x => x.Active && x.AuditId == auditId).Select(x => new AudTranCSDetail()
            {
                StaffId = x.Staff.Id,
                PersonName = x.Staff.PersonName,
                CompanyEmail = x.Staff.CompanyEmail,
                ItUserMasters = x.Staff.ItUserMasters
            }).ToListAsync();
        }
    }
}
