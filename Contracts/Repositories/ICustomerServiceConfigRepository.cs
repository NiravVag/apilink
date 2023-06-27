using DTO.CommonClass;
using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Contracts.Repositories
{
    public interface ICustomerServiceConfigRepository : IRepository
    {
        Task<List<CuServiceType>> GetServiceTypeByCustomerID(int? CustomerId, int? ServiceId);

        CuServiceType GetServiceTypeByServiceID(int? ServiceId);

        Task<int> AddCustomerServiceConfig(CuServiceType entity);

        Task<int> EditCustomerServiceConfig(CuServiceType entity);

        Task<bool> RemoveCustomerServiceConfig(int id);

        CuServiceType GetServiceTypeBySPST(int? ServiceID, int? ServiceTypeID,int CustomerID);

        CuServiceType ServiceByCustomerAndServiceTypeID(int customerId,int serviceTypeId);

        Task<List<CommonDataSource>> GetCustomerServiceType(int customerId, int serviceId);

        Task<bool> CheckServiceTypeMappedWithCustomer(int customerId, int serviceTypeId);

    }
}
