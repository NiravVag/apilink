using DTO.Inspection;
using DTO.InspectionCertificate;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IInspectionCertificateRepository : IRepository
    {
        ///<summary>
        /// get inspection data based on customer id, supplire id, service date range, booking number
        ///</summary>
        /// <returns>inspection details and product details</returns>
        IQueryable<InspTransaction> GetInspectionICData(ICBookingSearchRequest bookingRequest);
        ///<summary>
        ///get product details by list of booking id
        ///</summary>
        /// <param name="bookingIds"></param>
        /// <returns>product details</returns>
        Task<List<ICBookingSearchProductResponse>> GetBookingProductList(List<int> bookingIds);
        ///<summary>
        ///get full bridge details by list of full bridge report id
        ///</summary>
        /// <param name="fbReportIds"></param>
        /// <returns>full bridge detatils for product</returns>
        Task<List<ICBookingProductFB>> GetProductFBList(List<int> fbReportIds);
        ///<summary>
        ///get ic product details by insppotransaction id
        ///</summary>
        /// <param name="inspPOTransactionId"></param>
        /// <returns>ic product details</returns>
        Task<List<InspIcTranProduct>> GetICProducts(List<int> inspPOTransactionId);
        ///<summary>
        ///save the ic and ic product data
        ///</summary>
        /// <param name="entity"></param>
        /// <returns>saved status</returns>
        Task<int> SaveIC(InspIcTransaction entity);
        ///<summary>
        /// get IC main table details by using id
        ///</summary>
        /// <param name="id"></param>
        /// <returns>IC details</returns>
        Task<InspectionCertificateRequest> GetICDetails(int id);
        ///<summary>
        /// fetch IC product details by icid
        ///</summary>
        /// <param name="id"></param>
        /// <returns>product details</returns>
        Task<List<InspectionCertificateBookingRequest>> GetICProductDetails(int id);
        ///<summary>
        ///fetch IC product details by icid
        ///</summary>
        /// <param name="id"></param>
        /// <returns>product details</returns>
        Task<List<InspIcTranProduct>> GetICProductsByICID(int id);
        ///<summary>
        ///
        ///</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InspIcTransaction> GetICDetailsProductDetails(int id);
        ///<summary>
        ///
        ///</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InspectionCertificatePDF> GetICPDFDetails(int id);
        ///<summary>
        ///
        ///</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<InspectionCertificateProductPDF>> GetICPDFProductDetails(int id);
        /// <summary>
        /// GetInspectionCertificateDetails
        /// </summary>
        /// <returns></returns>
        IQueryable<InspectionCertificateData> GetInspectionCertificateDetails();

        IQueryable<InspectionTransactionData> GetInspectionTransactions(IQueryable<int?> poTransactionIds);

        IQueryable<InspectionCertificateData> GetInspectionCertificateDetails(int icId);

        IEnumerable<ICSummaryProducts> GetICSummaryProducts(IEnumerable<int> poTransactionIds, int icID);

        Task<List<InspIcStatus>> GetICStatus();

        ///<summary>
        ///
        ///</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QuantityPoId>> GetICProductQty(List<int> inspPoTransactionId);
        ///<summary>
        ///
        ///</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QuantityPoId>> GetFBPresentedQty(List<int> inspPoTransactionId);
        ///<summary>
        ///
        ///</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<DropDown>> GetICTitleList();

        IQueryable<ICItemRepo> GetICInspectionTransactions();

        Task<List<CustomerDecisionBookId>> GetInspCusDecision(ICBookingSearchRequest bookingRequest);

        Task<List<BookingDeptAccess>> GetDeptBookingIdsByBookingIds(IEnumerable<int> bookingIds);
        Task<List<BookingBrandAccess>> GetBrandBookingIdsByBookingIds(IEnumerable<int> bookingIds);
        Task<List<BookingServiceType>> GetBookingServiceTypes(IEnumerable<int> bookingIds);

        Task<List<ICBookingModel>> GetPartialIssueICBookingIdList(IQueryable<int> bookingIds);

        Task<List<ICBookingModel>> GetIssuedICBookingIdList(IQueryable<int> bookingIds);

        Task<List<ICBookingSearchProductResponse>> GetBookingProductListSoftline(List<int> bookingIds);
    }
}
