using DTO.CombineOrders;
using DTO.Inspection;
using DTO.InspectionPicking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInspectionPickingManager
    {
        /// <summary>
        /// Save Picking Details
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<SaveInspectionPickingResponse> SavePickingDetails(List<InspectionPickingData> inspectionPickings, int inspectionId);
        /// <summary>
        /// Get Picking details by inspection id
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        Task<InspectionPickingSummaryResponse> GetPickingDetails(int inspectionId);

        /// <summary>
        /// Get Customer Address and Customer Contact by Customer Id
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        Task<CustomerContactsResponse> GetCustomerContacts(int customerID);
    }
}
