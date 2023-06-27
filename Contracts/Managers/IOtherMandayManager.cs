using DTO.CommonClass;
using DTO.OtherManday;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IOtherMandayManager
    {
        Task<SaveOtherMandayResponse> SaveOtherManday(SaveOtherMandayRequest request);
        Task<SaveOtherMandayResponse> UpdateOtherManday(SaveOtherMandayRequest request);
        Task<DeleteOtherMandayResponse> DeleteOtherManday(int id);
        Task<EditOtherMandayResponse> GetEditOtherManday(int id);
        Task<OtherMandaySummaryResponse> GetOtherMandaySummary(OtherMandaySummaryRequest request);
        Task<DataSourceResponse> GetPurposeList();
        Task<List<ExportOtherMandayData>> ExportOtherMandaySummary(OtherMandaySummaryRequest request);
    }
}
