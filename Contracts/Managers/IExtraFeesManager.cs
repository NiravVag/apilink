using DTO.CommonClass;
using DTO.ExtraFees;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IExtraFeesManager
    {
        Task<BookingDataResponse> GetBookingNoList(BookingDataSourceRequest request);

        Task<SaveResponse> Save(ExtraFees request);

        Task<TaxResponse> GetTaxValue(int bankId, int bookingId);

        Task<EditResponse> Edit(int extraFeeId);

        Task<DataSourceResponse> GetInvoiceNoList(int bookingId, int billedToId, int serviceId);

        Task<CancelResponse> Cancel(int extraFeeId);

        Task<ExtraFeeResponse> GetExFeeSummary(ExtraFeeRequest request);

        Task<List<ExtraFeeSummaryExportItem>> ExportExtrafeeSearchSummary(ExtraFeeRequest requestDto);

        Task<DataSourceResponse> GetExtraFeeStatusList();

        Task<ManualInvoiceResponse> GenerateManualInvoice(int extraFeeId);
        Task<SaveResponse> CancelExtraFeeInvoice(int extraFeeId);
    }
}
