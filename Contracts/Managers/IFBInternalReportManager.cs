using DTO.FBInternalReport;
using DTO.Inspection;
using DTO.MobileApp;
using DTO.Report;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IFBInternalReportManager
    {
        Task<IEnumerable<InternalFBReportItem>> ExportInternalFBReports(InspectionSummarySearchRequest request);
        Task<InternalFBReportSummaryResponse> GetAllInspectionReportProducts(InspectionSummarySearchRequest request);
        Task<IQueryable<InternalReportProductItem>> GetProductsByBooking(int bookingId);
        /// <summary>
        /// GetQCInspectionDetailsPDF 
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        Task<QCInspectionDetailsPDF> GetQCInspectionDetails(int inspectionID);


        /// <summary>
        /// Get Lab Contacts by labId and Customer Id
        /// </summary>
        /// <param name="labId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<IEnumerable<QcPickingData>> GetQcPickingDetails(int bookingId);

        /// <summary>
        /// Get report summary for mobile app
        /// </summary>
        /// <param name="labId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<List<MobileInspectionReportData>> GetMobileReportSummary(InspectionSummarySearchRequest request);

        /// <summary>
        /// Get report details for each supplier - mobile app
        /// </summary>
        /// <returns></returns>
        Task<List<MobileInspectionReportProducData>> GetMobileReportSummaryDetails(InspectionSummarySearchRequest request);
        
        /// <summary>
        /// Get the inspection report details for mobile app
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<InspReportMobileResponse> GetMobileInspectionReportSummary(InspSummaryMobileRequest request);

        /// <summary>
        /// Get the product data and status for Mobile app
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<InspReportProductsMobileResponse> GetMobileBookingProductsAndStatus(InspSummaryMobileRequest request);

        Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration();
    }
}
