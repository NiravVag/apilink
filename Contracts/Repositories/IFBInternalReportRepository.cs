using DTO.FBInternalReport;
using DTO.Inspection;
using DTO.Report;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IFBInternalReportRepository
    {
        /// <summary>
        /// Get all the inspection details
        /// </summary>
        /// <returns></returns>
        IQueryable<InternalReportBookingValues> GetAllInspectionsReports();


        /// <summary>
        /// Get all the service Type details
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingIds);
        /// <summary>
        /// Get all the service Type list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ServiceTypeList>> GetServiceTypeList(IQueryable<int> bookingIds);


        /// <summary>
        /// Get all the service Type details
        /// </summary>
        /// <returns></returns>
        IQueryable<InternalReportProducts> GetProductsByBooking(int bookingId);

        /// <summary>
        /// Get all the service Type details
        /// </summary>
        /// <returns></returns>
       // Task<IEnumerable<InternalReportProducts>> GetProductListByBooking(List<int> bookingId);
        /// <summary>
        /// Get the base inspection base details
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        QCInspectionDetailsRepo GetQCInspectionDetails(int inspectionID);
        /// <summary>
        /// Get the inspection product details
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        IEnumerable<QCInspectionProductDetails> GetQCInspectionProductDetails(int inspectionID);

        /// <summary>
        /// Get inspection product details by POTrans id for Qc Picking PDF
        /// </summary>
        /// <param name="PoTransIds"></param>
        /// <returns>Picking list by inspection</returns>
        IQueryable<PickingProductData> GetPickingProducts(IEnumerable<int> poTransIds);

        /// <summary>
        /// Get inspection picking details by booking id for Qc Picking PDF
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Picking list by inspection</returns>
        IQueryable<QcPickingItem> GetQcPickingDetails(int bookingId);

        /// <summary>
        /// Get inspection lab details by POTrans id for Qc Picking PDF
        /// </summary>
        /// <param name="PoTransIds"></param>
        /// <returns>Picking list by inspection</returns>
        IQueryable<QcPickingData> GetLabAddress(IEnumerable<int> poTransIds);

        IQueryable<QcPickingData> GetCusAddress(IEnumerable<int> poTransIds);

        Task<List<QCInspectionProductDetails>> GetQCInspectionProductSoftlineDetails(int inspectionID);
    }
}
