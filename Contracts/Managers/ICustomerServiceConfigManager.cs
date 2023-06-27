using DTO.CommonClass;
using DTO.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerServiceConfigManager
    {
        Task<CustomerServiceConfigSearchResponse> GetCustomerServiceConfigData(CustomerServiceConfigSearchRequest request);

        Task<CustomerServConfigSummaryResponse> GetCustomerServiceConfigSummary();

        Task<CustomerServiceConfigResponse> GetEditCustomerServiceConfigSummary(CustomerServiceConfigRequest request);

        EditCustomerServiceConfigResponse GetEditCustomerServiceConfig(int? id);

        Task<CustomerServiceConfigMasterResponse> GetCustomerServiceConfigMaster();

        Task<SaveCustomerServiceConfigResponse> Save(EditCustomerServiceConfigData request);

        Task<CustomerServiceConfigDeleteResponse> DeleteCustomerService(int id);

        Task<CustomerServicePickResponse> GetLevelPickFirst();

        EditCustomerServiceConfigResponse ServiceByCustomerAndServiceTypeID(int customerId,int serviceTypeId);

        Task<DataSourceResponse> GetServiceconfig(int customerId, int serviceId);

        Task<bool> CheckServiceTypeMappedWithCustomer(int customerId, int serviceTypeId);
    }
}
