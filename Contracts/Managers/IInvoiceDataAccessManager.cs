using DTO.InvoiceDataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInvoiceDataAccessManager
    {
        Task<SaveInvoiceDataAccessResponse> Save(SaveInvoiceDataAccessRequest request);
        Task<InvoiceDataAccessSummaryResponse> GetSummaryData(InvoiceDataAccessSummaryRequest request);
        Task<EditInvoiceDataAccessResponse> Edit(int id);
        Task<DeleteInvoiceDataAccessResponseResult> Delete(int id);
        Task<InvoiceDataAccess> GetStaffInvoiceDataAcesss(int staffId);
    }
}
