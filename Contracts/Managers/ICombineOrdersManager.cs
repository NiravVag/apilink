using DTO.CombineOrders;
using DTO.SamplingQuantity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICombineOrdersManager
    {
        /// <summary>
        /// Get Combine Orders By Booking Id
        /// </summary>
        /// <param name=""></param>
        /// <returns>List Of Combine Orders</returns>
        Task<CombineOrderSummaryResponse> GetCombineOrderDetails(int inspectionId);

        /// <summary>
        /// get po details by inspection and product ref
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        Task<PoDetailsResponse> GetPoDetails(int inspectionId, int productRefId);

        /// <summary>
        /// Get Sampling Quantity by booking Id and Product Id
        /// </summary>
        /// <param name=""></param>
        /// <returns>Sampling Quantity</returns>
        Task<int> GetSamplingQuantityByBookingProduct(int bookingId, int productId);

        /// <summary>
        /// Save Combine Orders
        /// </summary>
        /// <param name=""></param>
        /// <returns>Sampling Quantity</returns>
        Task<SaveCombineOrdersResponse> SaveCombineOrders(List<SaveCombineOrdersRequest> combineOrders, int inspectionId);

        Task<int?> GetCombineAqlQuantityByBookingProduct(int bookingId, int productId);

        Task<CombineSamplingProductListResponse> GetCombinedAQLQty(int bookingId, int? aqlId, List<CombineOrderSamplingData> combineOrders);

        Task<int> GetSamplingQuantity(SamplingQuantityRequest request);

        Task<int> GetSamplingQuantityByBookingAndAql(SamplingQuantityRequest request);

    }
}
