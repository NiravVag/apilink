using DTO.InspectionCertificate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInspectionCertificateManager
    {
        /// <summary>
        /// Save IC details and IC product details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>returns save status</returns>
        Task<InspectionCertificateResponse> SaveIC(InspectionCertificateRequest request);
        /// <summary>
        /// get booking details with product details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>booking details with respective product details</returns>
        Task<ICBookingResponse> GetInspectionICData(ICBookingSearchRequest request);
        /// <summary>
        /// fetch the IC and IC product details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>data of IC and IC product</returns>
        Task<EditInspectionCertificateResponse> EditICDetails(int id);
        /// <summary>
        /// IC table status changes and product table inactive the records
        /// </summary>
        /// <param name="id"></param>
        /// <returns>status of cancel query execution</returns>
        Task<InspectionCertificateResponse> CancelICDetails(int id);
        /// <summary>
        /// get preview pdf IC and booking details 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InspectionCertificatePDF> GetICPreviewDetails(int id, bool IsDraft);

        Task<ICSummarySearchResponse> GetICSummaryDetails(ICSummarySearchRequest request);

        List<ICSummaryProducts> GetICSummaryProducts(int icID);

        ICStatusResponse GetICStatus();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ICTitleResponse> GetICTitleList();
        /// <summary>
        /// get booking ic product list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ICBookingProductResponse> BookingICProduct(ICBookingProductRequest request);
    }
}
