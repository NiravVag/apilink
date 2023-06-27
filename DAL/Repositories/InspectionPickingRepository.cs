using Contracts.Repositories;
using DTO.Common;
using DTO.Inspection;
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
    /// This Repository calss will handle Inpection Picking Process for booking
    /// </summary>
    public class InspectionPickingRepository : Repository, IInspectionPickingRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public InspectionPickingRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        /// <summary>
        /// Get the Picking Details by insepction id
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        public async Task<InspTransaction> GetInspectionPickingByBookingId(int inspectionID)
        {
            return await _context.InspTransactions.Where(x => x.Id == inspectionID)
                        .Include(x => x.Customer)
                        .Include(x => x.InspPurchaseOrderTransactions)
                             .ThenInclude(x => x.InspTranPickings)
                               .ThenInclude(x => x.InspTranPickingContacts)
                         .Include(x => x.InspProductTransactions)
                            .ThenInclude(x => x.Product)
                        .Include(x => x.InspPurchaseOrderTransactions)
                            .ThenInclude(x => x.Po)

                   .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the inspection picking data exists
        /// </summary>
        /// <param name="poTransIds"></param>
        /// <returns></returns>
        public async Task<bool> GetInspectionPickingExists(List<int> poTransIds)
        {
            return await _context.InspTranPickings.AnyAsync(x => x.Active && poTransIds.Contains(x.PoTranId) && (x.LabAddressId.HasValue || x.CusAddressId.HasValue));
                          
        }

    }
}
