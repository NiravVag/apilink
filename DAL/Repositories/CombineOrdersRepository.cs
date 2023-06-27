using Contracts.Repositories;
using DTO.Common;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// This Repository calss will handle Combine orders for booking
    /// </summary>
    public class CombineOrdersRepository : Repository, ICombineOrdersRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public CombineOrdersRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        /// <summary>
        /// Get Combine Order details based on booking Id
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        public async Task<InspTransaction> GetCombineOrderDetails(int inspectionID)
        {
            return await _context.InspTransactions.Where(x => x.Id == inspectionID)
                    .Include(x => x.InspProductTransactions)
                             .ThenInclude(x => x.AqlNavigation)
                       .Include(x => x.InspProductTransactions)
                                 .ThenInclude(x => x.Product)
                              .Include(x => x.InspProductTransactions)
                                 .ThenInclude(x => x.ParentProduct)
                       .Include(x => x.InspPurchaseOrderTransactions)
                       .ThenInclude(x => x.Po)
                   .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Po details by product ref id and inspection id
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InspPurchaseOrderTransaction>> GetPODetailsbyProductRefId(int inspectionID, int productRefId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(y => y.Active.HasValue && y.Active.Value
                     && y.InspectionId == inspectionID && y.ProductRefId == productRefId)
                   .Include(x => x.Po)
                   .ToListAsync();
        }

        /// <summary>
        /// Get the Sample size Code based on the Order Quantity
        /// </summary>
        /// <param name="orderQuantity"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RefAqlSampleCode>> GetSampleSizeCode(int orderQuantity)
        {
            return await _context.RefAqlSampleCodes.Where(x => orderQuantity >= x.MinSize && orderQuantity <= x.MaxSize).ToListAsync();
        }

        /// <summary>
        /// Get the SamplingQuantity By Sampling Size Code
        /// </summary>
        /// <param name="strSampleSizeCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RefAqlPickSampleSizeCodeValue>> GetSamplingQuantityBySamplingSizeByCode(string strSampleSizeCode)
        {
            return await _context.RefAqlPickSampleSizeCodeValues.Where(x => x.SampleSizeCode == strSampleSizeCode).ToListAsync();
        }

    }
}
