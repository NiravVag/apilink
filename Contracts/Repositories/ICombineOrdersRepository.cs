using DTO.CombineOrders;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICombineOrdersRepository : IRepository
    {
        /// <summary>
        /// Get Combine Orders By Booking Id
        /// </summary>
        /// <param name=""></param>
        /// <returns>List Of Combine Orders</returns>
        Task<InspTransaction> GetCombineOrderDetails(int inspectionId);

        /// <summary>
        /// get po details by product ref and inspection
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        Task <IEnumerable<InspPurchaseOrderTransaction>> GetPODetailsbyProductRefId(int inspectionID, int productRefId);

        /// <summary>
        /// Get Sample Size code by order quantity
        /// </summary>
        /// <param name=""></param>
        /// <returns>List Of Sample Size</returns>
        Task<IEnumerable<RefAqlSampleCode>> GetSampleSizeCode(int orderQuantity);

        /// <summary>
        /// Get Sample Size code by order quantity
        /// </summary>
        /// <param name=""></param>
        /// <returns>List Of Sample Size</returns>
        Task<IEnumerable<RefAqlPickSampleSizeCodeValue>> GetSamplingQuantityBySamplingSizeByCode(string strSampleSizeCode);

    }
}
