using DTO.Master;
using DTO.TCF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ITCFManager
    {
        /// <summary>
        /// Save the customer contact to TCF
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<TCFResponseMessage> SaveCustomerContactToTCF(int contactId);

        /// <summary>
        /// Save the supplier data to TCF
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<TCFResponseMessage> SaveSupplierToTCF(int supplierId);

        /// <summary>
        /// Save/Update the user info to TCF
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TCFResponseMessage> SaveUserToTCF(int userId);

        Task<TCFResponseMessage> SaveBuyerListToTCF(int customerId, int? entityId);

        /// <summary>
        /// Save/Update the product information to TCF
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<TCFResponseMessage> SaveProductToTCF(int productId, int? entityId);

        Task<TCFResponseMessage> SaveCustomerToTCF(int customerId);

    }
}
