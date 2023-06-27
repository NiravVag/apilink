using DTO.CommonClass;
using DTO.ExtraFees;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IExtraFeesRepository : IRepository
    {
        IQueryable<BookingRepo> GetBookingNoList();

        Task<EditExtraFeesRepo> GetExtraFees(int id);

        Task<bool> ExistsExtraFee(int billedTo, int bookingNo, int extraFeeId, int serviceId);

        Task<IEnumerable<CommonDataSource>> GetInvoiceNoList(int inspectionId, int? billedToId, int serviceId);

        Task<InvExfTransaction> GetExtraFeeData(int id);
        IQueryable<ExtraFeeSummaryItem> GetExFeeData();
        Task<List<ExtraFeeSummaryItem>> GetExFeeTranDetails(List<int> exfTranIdList);
        IQueryable<ExtraFeeSummaryItem> GetExFeeDetailsData();
        Task<List<CommonDataSource>> GetExtraFeeStatus();

        Task<bool> IsExistsInvoiceNumber(string InvoiceNumber);

        IQueryable<BookingRepo> GetAuditBookingList();

        Task<IEnumerable<CommonDataSource>> GetInvoiceNoListByAudit(int auditId, int? billedToId, int serviceId);

        IQueryable<ExtraFeeSummaryItem> GetAuditExFeeData();

        IQueryable<ExtraFeeSummaryItem> GetExtraFeeDetailsAuditData();

        Task<List<ExtraFeeContactData>> GetExtraContactsById(int id);

        Task<List<EditExtraFeeType>> GetExtraFeesTypeById(int id);
    }
}
