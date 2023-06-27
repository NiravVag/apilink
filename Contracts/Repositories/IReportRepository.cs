using DTO.Kpi;
using DTO.Report;
using Entities;
using Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IReportRepository : IRepository
    {
        /// <summary>
        /// Get all the inspection details
        /// </summary>
        /// <returns></returns>
        IQueryable<CustomerReportBookingValues> GetAllInspectionsReports();


        /// <summary>
        /// Get all the service Type details
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportProducts>> GetProductsByBooking(int bookingId);

        /// <summary>
        /// Get all the service Type details
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportProducts>> GetProductListByBooking(IEnumerable<int> bookingId);

        /// <summary>
        /// get po list with products by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<IEnumerable<ReportProducts>> GetProductPoListByBooking(List<int> bookingId);

        /// <summary>
        /// Get the report ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<int>> GetReportIdsByBookingIds(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get Booking Report summary link
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<BookingReportSummaryLinkRepo>> GetBookingReportSummaryLink(List<int> bookingIds, IEnumerable<int> inspectedStatusIds);

        /// <summary>
        /// get QC details by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<SchScheduleQc>> GetQCDetails(int bookingId);

        Task<IEnumerable<ReportProducts>> GetContainersByBooking(int bookingId);

        /// <summary>
        /// Get ReportSummary Link for containerservice type
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        Task<List<BookingReportSummaryLinkRepo>> GetBookingContainerReportSummaryLink(List<int> bookingIds, IEnumerable<int> inspectedStatusIds);

        /// <summary>
        /// get only po number by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingPONumbers>> GetPONumbersbyBooking(int bookingId);

        Task<List<BookingReportSummaryLinkRepo>> GetBookingReportSummaryLinkData(List<int> bookingIds, IEnumerable<int> inspectedStatusIds);

        Task<List<ExportFBReportRepo>> GetExportFBReportData(IQueryable<int> bookingIds);

        Task<List<ExportInspectionReportData>> GetBookingProductList(IQueryable<int> bookingIds);

        Task<List<ExportInspectionReportData>> GetBookingContainerList(IQueryable<int> bookingIds);

        Task<List<ProductPOList>> GetPoListByBookingIds(IQueryable<int> bookingIds);

        Task<FbReportDetail> GetFBReportDetail(int apiReportId);
        Task<List<ProductPOList>> GetPoListByReportIds(IQueryable<int> reportIds);
        Task<IEnumerable<ReportProducts>> GetProductListByReportIds(IEnumerable<int> reportIds, string productRef, string po);

        Task<List<FbReportQualityPlan>> GetFbReportQualityPlansByReportIds(IEnumerable<int> reportIds);

        Task<List<GapKpiFbReportPackingPackagingLabellingProduct>> GetFbReportPackingPackagingLabellingProducts(IEnumerable<int> fbReportDetailIds, List<int> packingTypes);
    }
}
