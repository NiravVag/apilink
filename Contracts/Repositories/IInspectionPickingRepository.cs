using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IInspectionPickingRepository : IRepository
    {
        /// <summary>
        /// Get inspection picking details by booking id
        /// </summary>
        /// <param name=""></param>
        /// <returns>Picking list by inspection</returns>
        Task<InspTransaction> GetInspectionPickingByBookingId(int inspectionID);

        Task<bool> GetInspectionPickingExists(List<int> poTransIds);

    }
}
