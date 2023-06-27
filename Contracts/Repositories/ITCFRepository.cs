using DTO.TCF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ITCFRepository:IRepository
    {
        Task<TCFCustomerContact> GetTCFCustomerContact(int contactId);
        Task<TCFSupplier> GetTCFSupplier(int supplierId);

        Task<List<TCFSupplierCustomer>> GetCustomerGLCodesBySupplier(int supplierId);
        /// <summary>
        /// Get the user info to push to tcf
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TCFUserRepo> GetTCFUser(int userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<List<Suppliercontact>> GetTCFSupplierContacts(int supplierId);
       
        Task<TCFCustomer> GetTCFCustomer(int customerId);
        Task<List<TCFBuyer>> GetTCFBuyerList(int customerId, int? entityId);
        Task<TCFProduct> GetTCFProduct(int productId, int? entityId);
    }
}
