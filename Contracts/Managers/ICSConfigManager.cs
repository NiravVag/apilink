using DTO.Customer;
using System.Threading.Tasks;
using System.Collections.Generic;
using DTO.References;

namespace Contracts.Managers
{
    public interface ICSConfigManager
    {

        Task<CSResponse> GetCustomerService();

        Task<CSConfigDeleteResponse> DeleteCSConfig(CSConfigDelete id);

        Task<EditCSConfigResponse> GetEditCSConfig(int? id);

        Task<SaveCSConfResponse> CSConfigSave(SaveCSConfig request);

        CSConfigSearchResponse GetAllCSConfig(CSConfigSearchRequest request);

        Task<ServiceResponse> GetService();

        Task<CSConfigDeleteResponse> DeleteCSConfigSummaryByLocation(int staffid, List<int> lstloc);

        Task<CSAllocationResponse> GetCSAllocations(CSAllocationSearchRequest input);
        
        Task<SaveCSAllocationResponse> SaveCSAllocationAsync(SaveCSAllocation request);

        Task<DeleteCSAllocationResponse> DeleteAllocationsAsync(CSAllocationDeleteItem request);
        Task<GetCSAllocationResponse> GetCSAllocation(int id);
        Task<List<ExportCSAllocationData>> ExportCSAllocationSummary(CSAllocationSearchRequest request);
    }
}
